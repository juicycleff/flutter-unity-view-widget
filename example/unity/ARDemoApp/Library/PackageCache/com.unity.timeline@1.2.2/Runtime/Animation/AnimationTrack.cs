using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Flags specifying which offset fields to match
    /// </summary>
    [Flags]
    public enum MatchTargetFields
    {
        /// <summary>
        /// Translation X value
        /// </summary>
        PositionX = 1 << 0,
        /// <summary>
        /// Translation Y value
        /// </summary>
        PositionY = 1 << 1,
        /// <summary>
        /// Translation Z value
        /// </summary>
        PositionZ = 1 << 2,
        /// <summary>
        /// Rotation Euler Angle X value
        /// </summary>
        RotationX = 1 << 3,
        /// <summary>
        /// Rotation Euler Angle Y value
        /// </summary>
        RotationY = 1 << 4,
        /// <summary>
        /// Rotation Euler Angle Z value
        /// </summary>
        RotationZ = 1 << 5
    }

    /// <summary>
    /// Describes what is used to set the starting position and orientation of each Animation Track.
    /// </summary>
    /// <remarks>
    /// By default, each Animation Track uses ApplyTransformOffsets to start from a set position and orientation.
    /// To offset each Animation Track based on the current position and orientation in the scene, use ApplySceneOffsets.
    /// </remarks>
    public enum TrackOffset
    {
        /// <summary>
        /// Use this setting to offset each Animation Track based on a set position and orientation.
        /// </summary>
        ApplyTransformOffsets,
        /// <summary>
        /// Use this setting to offset each Animation Track based on the current position and orientation in the scene.
        /// </summary>
        ApplySceneOffsets,
        /// <summary>
        /// Use this setting to offset root transforms based on the state of the animator.
        /// </summary>
        /// <remarks>
        /// Only use this setting to support legacy Animation Tracks. This mode may be deprecated in a future release.
        ///
        /// In Auto mode, when the animator bound to the animation track contains an AnimatorController, it offsets all animations similar to ApplySceneOffsets.
        /// If no controller is assigned, then all offsets are set to start from a fixed position and orientation, similar to ApplyTransformOffsets.
        /// In Auto mode, in most cases, root transforms are not affected by local scale or Animator.humanScale, unless the animator has an AnimatorController and Animator.applyRootMotion is set to true.
        /// </remarks>
        Auto
    }


    // offset mode
    enum AppliedOffsetMode
    {
        NoRootTransform,
        TransformOffset,
        SceneOffset,
        TransformOffsetLegacy,
        SceneOffsetLegacy,
        SceneOffsetEditor, // scene offset mode in editor
        SceneOffsetLegacyEditor,
    }


    // separate from the enum to hide them from UI elements
    static class MatchTargetFieldConstants
    {
        public static MatchTargetFields All = MatchTargetFields.PositionX | MatchTargetFields.PositionY |
            MatchTargetFields.PositionZ | MatchTargetFields.RotationX |
            MatchTargetFields.RotationY | MatchTargetFields.RotationZ;

        public static MatchTargetFields None = 0;

        public static MatchTargetFields Position = MatchTargetFields.PositionX | MatchTargetFields.PositionY |
            MatchTargetFields.PositionZ;

        public static MatchTargetFields Rotation = MatchTargetFields.RotationX | MatchTargetFields.RotationY |
            MatchTargetFields.RotationZ;

        public static bool HasAny(this MatchTargetFields me, MatchTargetFields fields)
        {
            return (me & fields) != None;
        }

        public static MatchTargetFields Toggle(this MatchTargetFields me, MatchTargetFields flag)
        {
            return me ^ flag;
        }
    }


    /// <summary>
    /// A Timeline track used for playing back animations on an Animator.
    /// </summary>
    [Serializable]
    [TrackClipType(typeof(AnimationPlayableAsset), false)]
    [TrackBindingType(typeof(Animator))]
    public partial class AnimationTrack : TrackAsset, ILayerable
    {
        const string k_DefaultInfiniteClipName = "Recorded";
        const string k_DefaultRecordableClipName = "Recorded";

        [SerializeField, FormerlySerializedAs("m_OpenClipPreExtrapolation")]
        TimelineClip.ClipExtrapolation m_InfiniteClipPreExtrapolation = TimelineClip.ClipExtrapolation.None;

        [SerializeField, FormerlySerializedAs("m_OpenClipPostExtrapolation")]
        TimelineClip.ClipExtrapolation m_InfiniteClipPostExtrapolation = TimelineClip.ClipExtrapolation.None;

        [SerializeField, FormerlySerializedAs("m_OpenClipOffsetPosition")]
        Vector3 m_InfiniteClipOffsetPosition = Vector3.zero;

        [SerializeField, FormerlySerializedAs("m_OpenClipOffsetEulerAngles")]
        Vector3 m_InfiniteClipOffsetEulerAngles = Vector3.zero;

        [SerializeField, FormerlySerializedAs("m_OpenClipTimeOffset")]
        double m_InfiniteClipTimeOffset;

        [SerializeField, FormerlySerializedAs("m_OpenClipRemoveOffset")]
        bool m_InfiniteClipRemoveOffset; // cached value for remove offset

        [SerializeField]
        bool m_InfiniteClipApplyFootIK = true;

        [SerializeField, HideInInspector]
        AnimationPlayableAsset.LoopMode mInfiniteClipLoop = AnimationPlayableAsset.LoopMode.UseSourceAsset;

        [SerializeField]
        MatchTargetFields m_MatchTargetFields = MatchTargetFieldConstants.All;
        [SerializeField]
        Vector3 m_Position = Vector3.zero;
        [SerializeField]
        Vector3 m_EulerAngles = Vector3.zero;


        [SerializeField] AvatarMask m_AvatarMask;
        [SerializeField] bool       m_ApplyAvatarMask  = true;

        [SerializeField] TrackOffset m_TrackOffset = TrackOffset.ApplyTransformOffsets;

        [SerializeField, HideInInspector] AnimationClip m_InfiniteClip;


#if UNITY_EDITOR
        private AnimationClip m_DefaultPoseClip;
        private AnimationClip m_CachedPropertiesClip;

        AnimationOffsetPlayable m_ClipOffset;

        private Vector3 m_SceneOffsetPosition = Vector3.zero;
        private Vector3 m_SceneOffsetRotation = Vector3.zero;

        private bool m_HasPreviewComponents = false;
#endif

        /// <summary>
        /// The translation offset of the entire track.
        /// </summary>
        public Vector3 position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        /// <summary>
        /// The rotation offset of the entire track, expressed as a quaternion.
        /// </summary>
        public Quaternion rotation
        {
            get { return Quaternion.Euler(m_EulerAngles); }
            set { m_EulerAngles = value.eulerAngles; }
        }

        /// <summary>
        /// The euler angle representation of the rotation offset of the entire track.
        /// </summary>
        public Vector3 eulerAngles
        {
            get { return m_EulerAngles; }
            set { m_EulerAngles = value; }
        }

        /// <summary>
        /// Specifies whether to apply track offsets to all clips on the track.
        /// </summary>
        /// <remarks>
        /// This can be used to offset all clips on a track, in addition to the clips individual offsets.
        /// </remarks>
        [Obsolete("applyOffset is deprecated. Use trackOffset instead", true)]
        public bool applyOffsets
        {
            get { return false; }
            set {}
        }

        /// <summary>
        /// Specifies what is used to set the starting position and orientation of an Animation Track.
        /// </summary>
        /// <remarks>
        /// Track Offset is only applied when the Animation Track contains animation that modifies the root Transform.
        /// </remarks>
        public TrackOffset trackOffset
        {
            get { return m_TrackOffset; }
            set { m_TrackOffset = value; }
        }

        /// <summary>
        /// Specifies which fields to match when aligning offsets of clips.
        /// </summary>
        public MatchTargetFields matchTargetFields
        {
            get { return m_MatchTargetFields; }
            set { m_MatchTargetFields = value & MatchTargetFieldConstants.All; }
        }

        /// <summary>
        /// An AnimationClip storing the data for an infinite track.
        /// </summary>
        /// <remarks>
        /// The value of this property is null when the AnimationTrack is in Clip Mode.
        /// </remarks>
        public AnimationClip infiniteClip
        {
            get { return m_InfiniteClip; }
            internal set { m_InfiniteClip = value; }
        }

        // saved value for converting to/from infinite mode
        internal bool infiniteClipRemoveOffset
        {
            get { return m_InfiniteClipRemoveOffset; }
            set { m_InfiniteClipRemoveOffset = value; }
        }

        /// <summary>
        /// Specifies the AvatarMask to be applied to all clips on the track.
        /// </summary>
        /// <remarks>
        /// Applying an AvatarMask to an animation track will allow discarding portions of the animation being applied on the track.
        /// </remarks>
        public AvatarMask avatarMask
        {
            get { return m_AvatarMask; }
            set { m_AvatarMask = value; }
        }

        /// <summary>
        /// Specifies whether to apply the AvatarMask to the track.
        /// </summary>
        public bool applyAvatarMask
        {
            get { return m_ApplyAvatarMask; }
            set { m_ApplyAvatarMask = value; }
        }

        // is this track compilable

        internal override bool CanCompileClips()
        {
            return !muted && (m_Clips.Count > 0 || (m_InfiniteClip != null && !m_InfiniteClip.empty));
        }

        /// <inheritdoc/>
        public override IEnumerable<PlayableBinding> outputs
        {
            get { yield return AnimationPlayableBinding.Create(name, this); }
        }


        /// <summary>
        /// Specifies whether the Animation Track has clips, or is in infinite mode.
        /// </summary>
        public bool inClipMode
        {
            get { return clips != null && clips.Length != 0; }
        }

        /// <summary>
        /// The translation offset of a track in infinite mode.
        /// </summary>
        public Vector3 infiniteClipOffsetPosition
        {
            get { return m_InfiniteClipOffsetPosition; }
            set { m_InfiniteClipOffsetPosition = value; }
        }

        /// <summary>
        /// The rotation offset of a track in infinite mode.
        /// </summary>
        public Quaternion infiniteClipOffsetRotation
        {
            get { return Quaternion.Euler(m_InfiniteClipOffsetEulerAngles); }
            set { m_InfiniteClipOffsetEulerAngles = value.eulerAngles; }
        }

        /// <summary>
        /// The euler angle representation of the rotation offset of the track when in infinite mode.
        /// </summary>
        public Vector3 infiniteClipOffsetEulerAngles
        {
            get { return m_InfiniteClipOffsetEulerAngles; }
            set { m_InfiniteClipOffsetEulerAngles = value; }
        }

        internal bool infiniteClipApplyFootIK
        {
            get { return m_InfiniteClipApplyFootIK;  }
            set { m_InfiniteClipApplyFootIK = value; }
        }

        internal double infiniteClipTimeOffset
        {
            get { return m_InfiniteClipTimeOffset; }
            set { m_InfiniteClipTimeOffset = value; }
        }

        /// <summary>
        /// The saved state of pre-extrapolation for clips converted to infinite mode.
        /// </summary>
        public TimelineClip.ClipExtrapolation infiniteClipPreExtrapolation
        {
            get { return m_InfiniteClipPreExtrapolation; }
            set { m_InfiniteClipPreExtrapolation = value; }
        }

        /// <summary>
        /// The saved state of post-extrapolation for clips when converted to infinite mode.
        /// </summary>
        public TimelineClip.ClipExtrapolation infiniteClipPostExtrapolation
        {
            get { return m_InfiniteClipPostExtrapolation; }
            set { m_InfiniteClipPostExtrapolation = value; }
        }

        /// <summary>
        /// The saved state of animation clip loop state when converted to infinite mode
        /// </summary>
        internal AnimationPlayableAsset.LoopMode infiniteClipLoop
        {
            get { return mInfiniteClipLoop; }
            set { mInfiniteClipLoop = value; }
        }

        [ContextMenu("Reset Offsets")]
        void ResetOffsets()
        {
            m_Position = Vector3.zero;
            m_EulerAngles = Vector3.zero;
            UpdateClipOffsets();
        }

        /// <summary>
        /// Creates a TimelineClip on this track that uses an AnimationClip.
        /// </summary>
        /// <param name="clip">Source animation clip of the resulting TimelineClip.</param>
        /// <returns>A new TimelineClip which has an AnimationPlayableAsset asset attached.</returns>
        public TimelineClip CreateClip(AnimationClip clip)
        {
            if (clip == null)
                return null;

            var newClip = CreateClip<AnimationPlayableAsset>();
            AssignAnimationClip(newClip, clip);
            return newClip;
        }

        /// <summary>
        /// Creates an AnimationClip that stores the data for an infinite track.
        /// </summary>
        /// <remarks>
        /// If an infiniteClip already exists, this method produces no result, even if you provide a different value
        /// for infiniteClipName.
        /// </remarks>
        /// <remarks>
        /// This method can't create an infinite clip for an AnimationTrack that contains one or more Timeline clips.
        /// Use AnimationTrack.inClipMode to determine whether it is possible to create an infinite clip on an AnimationTrack.
        /// </remarks>
        /// <remarks>
        /// When used from the editor, this method attempts to save the created infinite clip to the TimelineAsset.
        /// The TimelineAsset must already exist in the AssetDatabase to save the infinite clip. If the TimelineAsset
        /// does not exist, the infinite clip is still created but it is not saved.
        /// </remarks>
        /// <param name="infiniteClipName">
        /// The name of the AnimationClip to create.
        /// This method does not ensure unique names. If you want a unique clip name, you must provide one.
        /// See ObjectNames.GetUniqueName for information on a method that creates unique names.
        /// </param>
        public void CreateInfiniteClip(string infiniteClipName)
        {
            if (inClipMode)
            {
                Debug.LogWarning("CreateInfiniteClip cannot create an infinite clip for an AnimationTrack that contains one or more Timeline Clips.");
                return;
            }

            if (m_InfiniteClip != null)
                return;

            m_InfiniteClip = TimelineCreateUtilities.CreateAnimationClipForTrack(string.IsNullOrEmpty(infiniteClipName) ? k_DefaultInfiniteClipName : infiniteClipName, this, false);
        }

        /// <summary>
        /// Creates a TimelineClip, AnimationPlayableAsset and an AnimationClip. Use this clip to record in a timeline.
        /// </summary>
        /// <remarks>
        /// When used from the editor, this method attempts to save the created recordable clip to the TimelineAsset.
        /// The TimelineAsset must already exist in the AssetDatabase to save the recordable clip. If the TimelineAsset
        /// does not exist, the recordable clip is still created but it is not saved.
        /// </remarks>
        /// <param name="animClipName">
        /// The name of the AnimationClip to create.
        /// This method does not ensure unique names. If you want a unique clip name, you must provide one.
        /// See ObjectNames.GetUniqueName for information on a method that creates unique names.
        /// </param>
        /// <returns>
        /// Returns a new TimelineClip with an AnimationPlayableAsset asset attached.
        /// </returns>
        public TimelineClip CreateRecordableClip(string animClipName)
        {
            var clip = TimelineCreateUtilities.CreateAnimationClipForTrack(string.IsNullOrEmpty(animClipName) ? k_DefaultRecordableClipName : animClipName, this, false);

            var timelineClip = CreateClip(clip);
            timelineClip.displayName = animClipName;
            timelineClip.recordable = true;
            timelineClip.start = 0;
            timelineClip.duration = 1;

            var apa = timelineClip.asset as AnimationPlayableAsset;
            if (apa != null)
                apa.removeStartOffset = false;

            return timelineClip;
        }

#if UNITY_EDITOR
        internal Vector3 sceneOffsetPosition
        {
            get { return m_SceneOffsetPosition; }
            set { m_SceneOffsetPosition = value; }
        }

        internal Vector3 sceneOffsetRotation
        {
            get { return m_SceneOffsetRotation; }
            set { m_SceneOffsetRotation = value; }
        }

        internal bool hasPreviewComponents
        {
            get
            {
                if (m_HasPreviewComponents)
                    return true;

                var parentTrack = parent as AnimationTrack;
                if (parentTrack != null)
                {
                    return parentTrack.hasPreviewComponents;
                }

                return false;
            }
        }
#endif

        /// <summary>
        /// Used to initialize default values on a newly created clip
        /// </summary>
        /// <param name="clip">The clip added to the track</param>
        protected override void OnCreateClip(TimelineClip clip)
        {
            var extrapolation = TimelineClip.ClipExtrapolation.None;
            if (!isSubTrack)
                extrapolation = TimelineClip.ClipExtrapolation.Hold;
            clip.preExtrapolationMode = extrapolation;
            clip.postExtrapolationMode = extrapolation;
        }

        protected internal override int CalculateItemsHash()
        {
            return GetAnimationClipHash(m_InfiniteClip).CombineHash(base.CalculateItemsHash());
        }

        internal void UpdateClipOffsets()
        {
#if UNITY_EDITOR
            if (m_ClipOffset.IsValid())
            {
                m_ClipOffset.SetPosition(position);
                m_ClipOffset.SetRotation(rotation);
            }
#endif
        }

        Playable CompileTrackPlayable(PlayableGraph graph, TrackAsset track, GameObject go, IntervalTree<RuntimeElement> tree, AppliedOffsetMode mode)
        {
            var mixer = AnimationMixerPlayable.Create(graph, track.clips.Length);
            for (int i = 0; i < track.clips.Length; i++)
            {
                var c = track.clips[i];
                var asset = c.asset as PlayableAsset;
                if (asset == null)
                    continue;

                var animationAsset = asset as AnimationPlayableAsset;
                if (animationAsset != null)
                    animationAsset.appliedOffsetMode = mode;

                var source = asset.CreatePlayable(graph, go);
                if (source.IsValid())
                {
                    var clip = new RuntimeClip(c, source, mixer);
                    tree.Add(clip);
                    graph.Connect(source, 0, mixer, i);
                    mixer.SetInputWeight(i, 0.0f);
                }
            }

            return ApplyTrackOffset(graph, mixer, go, mode);
        }

        Playable ILayerable.CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return Playable.Null;
        }

        internal override Playable OnCreateClipPlayableGraph(PlayableGraph graph, GameObject go, IntervalTree<RuntimeElement> tree)
        {
            if (isSubTrack)
                throw new InvalidOperationException("Nested animation tracks should never be asked to create a graph directly");

            List<AnimationTrack> flattenTracks = new List<AnimationTrack>();
            if (CanCompileClips())
                flattenTracks.Add(this);


            bool animatesRootTransform = AnimatesRootTransform();
            foreach (var subTrack in GetChildTracks())
            {
                var child = subTrack as AnimationTrack;
                if (child != null && child.CanCompileClips())
                {
                    animatesRootTransform |= child.AnimatesRootTransform();
                    flattenTracks.Add(child);
                }
            }

            // figure out which mode to apply
            AppliedOffsetMode mode = GetOffsetMode(go, animatesRootTransform);
            var layerMixer = CreateGroupMixer(graph, go, flattenTracks.Count);
            for (int c = 0; c < flattenTracks.Count; c++)
            {
                var compiledTrackPlayable = flattenTracks[c].inClipMode ?
                    CompileTrackPlayable(graph, flattenTracks[c], go, tree, mode) :
                    flattenTracks[c].CreateInfiniteTrackPlayable(graph, go, tree, mode);
                graph.Connect(compiledTrackPlayable, 0, layerMixer, c);
                layerMixer.SetInputWeight(c, flattenTracks[c].inClipMode ? 0 : 1);
                if (flattenTracks[c].applyAvatarMask && flattenTracks[c].avatarMask != null)
                {
                    layerMixer.SetLayerMaskFromAvatarMask((uint)c, flattenTracks[c].avatarMask);
                }
            }

            bool requiresMotionXPlayable = RequiresMotionXPlayable(mode, go);
            
            Playable mixer = layerMixer;
           mixer = CreateDefaultBlend(graph, go, mixer, requiresMotionXPlayable);
            
            // motionX playable not required in scene offset mode, or root transform mode
            if (requiresMotionXPlayable)
            {
                // If we are animating a root transform, add the motionX to delta playable as the root node
                var motionXToDelta = AnimationMotionXToDeltaPlayable.Create(graph);
                graph.Connect(mixer, 0, motionXToDelta, 0);
                motionXToDelta.SetInputWeight(0, 1.0f);
                motionXToDelta.SetAbsoluteMotion(UsesAbsoluteMotion(mode));
                mixer = (Playable)motionXToDelta;
            }
            
            

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var animator = GetBinding(go != null ? go.GetComponent<PlayableDirector>() : null);
                if (animator != null)
                {
                    GameObject targetGO = animator.gameObject;
                    IAnimationWindowPreview[] previewComponents = targetGO.GetComponents<IAnimationWindowPreview>();

                    m_HasPreviewComponents = previewComponents.Length > 0;
                    if (m_HasPreviewComponents)
                    {
                        foreach (var component in previewComponents)
                        {
                            mixer = component.BuildPreviewGraph(graph, mixer);
                        }
                    }
                }
            }
#endif

            return mixer;
        }
        
        

        // Creates a layer mixer containing default blends
        // the base layer is a default clip of all driven properties
        // the next layer is optionally the desired default pose (in the case of humanoid, the tpose
        private Playable CreateDefaultBlend(PlayableGraph graph, GameObject go, Playable mixer, bool requireOffset)
        {
#if  UNITY_EDITOR
            if (Application.isPlaying)
                return mixer;

            var animator = GetBinding(go != null ? go.GetComponent<PlayableDirector>() : null);

            int inputs = 1 + ((m_CachedPropertiesClip != null) ? 1 : 0) + ((m_DefaultPoseClip != null) ? 1 : 0);
            if (inputs == 1)
                return mixer;
            
            var defaultPoseMixer = AnimationLayerMixerPlayable.Create(graph, inputs);

            int mixerInput = 0;
            if (m_CachedPropertiesClip)
            {
                var defaults = (Playable) AnimationClipPlayable.Create(graph, m_CachedPropertiesClip);
                if (requireOffset)
                    defaults = AttachOffsetPlayable(graph, defaults, m_SceneOffsetPosition, Quaternion.Euler(m_SceneOffsetRotation));
                graph.Connect(defaults, 0, defaultPoseMixer, mixerInput);
                defaultPoseMixer.SetInputWeight(mixerInput, 1.0f);
                mixerInput++;
            }

            if (m_DefaultPoseClip)
            {
                var blendDefault = (Playable) AnimationClipPlayable.Create(graph, m_DefaultPoseClip);
                if (requireOffset)
                    blendDefault = AttachOffsetPlayable(graph, blendDefault, m_SceneOffsetPosition, Quaternion.Euler(m_SceneOffsetRotation));
                
                graph.Connect(blendDefault, 0, defaultPoseMixer, mixerInput);
                defaultPoseMixer.SetInputWeight(mixerInput, 1.0f);
                mixerInput++;
            }
            
            
            graph.Connect(mixer, 0, defaultPoseMixer, mixerInput);
            defaultPoseMixer.SetInputWeight(mixerInput, 1.0f);
            
            return defaultPoseMixer;
#else 
            return mixer;
#endif 

        }


        private Playable AttachOffsetPlayable(PlayableGraph graph, Playable playable, Vector3 pos, Quaternion rot)
        {
            var offsetPlayable = AnimationOffsetPlayable.Create(graph, pos, rot, 1);
            offsetPlayable.SetInputWeight(0, 1.0f);
            graph.Connect(playable, 0, offsetPlayable, 0);
            return offsetPlayable;
        }

#if UNITY_EDITOR 
        private static string k_DefaultHumanoidClipPath = "Editors/TimelineWindow/HumanoidDefault.anim";
        private static AnimationClip s_DefaultHumanoidClip = null;

        AnimationClip GetDefaultHumanoidClip()
        {
            if (s_DefaultHumanoidClip == null)
            {
                s_DefaultHumanoidClip = UnityEditor.EditorGUIUtility.LoadRequired(k_DefaultHumanoidClipPath) as AnimationClip;
                if (s_DefaultHumanoidClip == null)
                    Debug.LogError("Could not load default humanoid animation clip for Timeline");
            }

            return s_DefaultHumanoidClip;
        }

#endif

        bool RequiresMotionXPlayable(AppliedOffsetMode mode, GameObject gameObject)
        {
            if (mode == AppliedOffsetMode.NoRootTransform)
                return false;
            if (mode == AppliedOffsetMode.SceneOffsetLegacy)
            {
                var animator = GetBinding(gameObject != null ? gameObject.GetComponent<PlayableDirector>() : null);
                return animator != null && animator.hasRootMotion;
            }
            return true;
        }

        static bool UsesAbsoluteMotion(AppliedOffsetMode mode)
        {
#if UNITY_EDITOR
            // in editor, previewing is always done in absolute motion
            if (!Application.isPlaying)
                return true;
#endif
            return mode != AppliedOffsetMode.SceneOffset &&
                mode != AppliedOffsetMode.SceneOffsetLegacy;
        }

        bool HasController(GameObject gameObject)
        {
            var animator = GetBinding(gameObject != null ? gameObject.GetComponent<PlayableDirector>() : null);

            return animator != null && animator.runtimeAnimatorController != null;
        }

        internal Animator GetBinding(PlayableDirector director)
        {
            if (director == null)
                return null;

            UnityEngine.Object key = this;
            if (isSubTrack)
                key = parent;

            UnityEngine.Object binding = null;
            if (director != null)
                binding = director.GetGenericBinding(key);

            Animator animator = null;
            if (binding != null) // the binding can be an animator or game object
            {
                animator = binding as Animator;
                var gameObject = binding as GameObject;
                if (animator == null && gameObject != null)
                    animator = gameObject.GetComponent<Animator>();
            }

            return animator;
        }

        static AnimationLayerMixerPlayable CreateGroupMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return AnimationLayerMixerPlayable.Create(graph, inputCount);
        }

        Playable CreateInfiniteTrackPlayable(PlayableGraph graph, GameObject go, IntervalTree<RuntimeElement> tree, AppliedOffsetMode mode)
        {
            if (m_InfiniteClip == null)
                return Playable.Null;

            var mixer = AnimationMixerPlayable.Create(graph, 1);

            // In infinite mode, we always force the loop mode of the clip off because the clip keys are offset in infinite mode
            //  which causes loop to behave different.
            // The inline curve editor never shows loops in infinite mode.
            var playable = AnimationPlayableAsset.CreatePlayable(graph, m_InfiniteClip, m_InfiniteClipOffsetPosition, m_InfiniteClipOffsetEulerAngles, false, mode, infiniteClipApplyFootIK, AnimationPlayableAsset.LoopMode.Off);
            if (playable.IsValid())
            {
                tree.Add(new InfiniteRuntimeClip(playable));
                graph.Connect(playable, 0, mixer, 0);
                mixer.SetInputWeight(0, 1.0f);
            }

            return ApplyTrackOffset(graph, mixer, go, mode);
        }

        Playable ApplyTrackOffset(PlayableGraph graph, Playable root, GameObject go, AppliedOffsetMode mode)
        {
#if UNITY_EDITOR
            m_ClipOffset = AnimationOffsetPlayable.Null;
#endif

            // offsets don't apply in scene offset, or if there is no root transform (globally or on this track)
            if (mode == AppliedOffsetMode.SceneOffsetLegacy ||
                mode == AppliedOffsetMode.SceneOffset     ||
                mode == AppliedOffsetMode.NoRootTransform ||
                !AnimatesRootTransform()
            )
                return root;


            var pos = position;
            var rot = rotation;

#if UNITY_EDITOR
            // in the editor use the preview position to playback from if available
            if (mode == AppliedOffsetMode.SceneOffsetEditor)
            {
                pos = m_SceneOffsetPosition;
                rot = Quaternion.Euler(m_SceneOffsetRotation);
            }
#endif

            var offsetPlayable = AnimationOffsetPlayable.Create(graph, pos, rot, 1);
#if UNITY_EDITOR
            m_ClipOffset = offsetPlayable;
#endif
            graph.Connect(root, 0, offsetPlayable, 0);
            offsetPlayable.SetInputWeight(0, 1);

            return offsetPlayable;
        }

        // the evaluation time is large so that the properties always get evaluated
        internal override void GetEvaluationTime(out double outStart, out double outDuration)
        {
            if (inClipMode)
            {
                base.GetEvaluationTime(out outStart, out outDuration);
            }
            else
            {
                outStart = 0;
                outDuration = TimelineClip.kMaxTimeValue;
            }
        }

        internal override void GetSequenceTime(out double outStart, out double outDuration)
        {
            if (inClipMode)
            {
                base.GetSequenceTime(out outStart, out outDuration);
            }
            else
            {
                outStart = 0;
                outDuration = Math.Max(GetNotificationDuration(), TimeUtility.GetAnimationClipLength(m_InfiniteClip));
            }
        }

        void AssignAnimationClip(TimelineClip clip, AnimationClip animClip)
        {
            if (clip == null || animClip == null)
                return;

            if (animClip.legacy)
                throw new InvalidOperationException("Legacy Animation Clips are not supported");

            AnimationPlayableAsset asset = clip.asset as AnimationPlayableAsset;
            if (asset != null)
            {
                asset.clip = animClip;
                asset.name = animClip.name;
                var duration = asset.duration;
                if (!double.IsInfinity(duration) && duration >= TimelineClip.kMinDuration && duration < TimelineClip.kMaxTimeValue)
                    clip.duration = duration;
            }
            clip.displayName = animClip.name;
        }

        /// <summary>
        /// Called by the Timeline Editor to gather properties requiring preview.
        /// </summary>
        /// <param name="director">The PlayableDirector invoking the preview</param>
        /// <param name="driver">PropertyCollector used to gather previewable properties</param>
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            m_SceneOffsetPosition = Vector3.zero;
            m_SceneOffsetRotation = Vector3.zero;

            var animator = GetBinding(director);
            if (animator == null)
                return;

            var animClips = new List<AnimationClip>(this.clips.Length + 2);
            GetAnimationClips(animClips);

            var hasHumanMotion = animClips.Exists(clip => clip.humanMotion);

            m_SceneOffsetPosition = animator.transform.localPosition;
            m_SceneOffsetRotation = animator.transform.localEulerAngles;
            
            // Create default pose clip from collected properties
            if (hasHumanMotion)
                animClips.Add(GetDefaultHumanoidClip());

            var bindings = AnimationPreviewUtilities.GetBindings(animator.gameObject, animClips);
            
            m_CachedPropertiesClip = AnimationPreviewUtilities.CreateDefaultClip(animator.gameObject, bindings);
            AnimationPreviewUtilities.PreviewFromCurves(animator.gameObject, bindings); // faster to preview from curves then an animation clip
            m_DefaultPoseClip = hasHumanMotion ? GetDefaultHumanoidClip() : null;
#endif
        }

        /// <summary>
        /// Gather all the animation clips for this track
        /// </summary>
        /// <param name="animClips"></param>
        private void GetAnimationClips(List<AnimationClip> animClips)
        {
            foreach (var c in clips)
            {
                var a = c.asset as AnimationPlayableAsset;
                if (a != null && a.clip != null)
                    animClips.Add(a.clip);
            }

            if (m_InfiniteClip != null)
                animClips.Add(m_InfiniteClip);

            foreach (var childTrack in GetChildTracks())
            {
                var animChildTrack = childTrack as AnimationTrack;
                if (animChildTrack != null)
                    animChildTrack.GetAnimationClips(animClips);
            }
        }

        // calculate which offset mode to apply
        AppliedOffsetMode GetOffsetMode(GameObject go, bool animatesRootTransform)
        {
            if (!animatesRootTransform)
                return AppliedOffsetMode.NoRootTransform;

            if (m_TrackOffset == TrackOffset.ApplyTransformOffsets)
                return AppliedOffsetMode.TransformOffset;

            if (m_TrackOffset == TrackOffset.ApplySceneOffsets)
                return (Application.isPlaying) ? AppliedOffsetMode.SceneOffset : AppliedOffsetMode.SceneOffsetEditor;

            if (HasController(go))
            {
                if (!Application.isPlaying)
                    return AppliedOffsetMode.SceneOffsetLegacyEditor;
                return AppliedOffsetMode.SceneOffsetLegacy;
            }

            return AppliedOffsetMode.TransformOffsetLegacy;
        }

        internal bool AnimatesRootTransform()
        {
            // infinite mode
            if (AnimationPlayableAsset.HasRootTransforms(m_InfiniteClip))
                return true;

            // clip mode
            foreach (var c in GetClips())
            {
                var apa = c.asset as AnimationPlayableAsset;
                if (apa != null && apa.hasRootTransforms)
                    return true;
            }

            return false;
        }
    }
}
