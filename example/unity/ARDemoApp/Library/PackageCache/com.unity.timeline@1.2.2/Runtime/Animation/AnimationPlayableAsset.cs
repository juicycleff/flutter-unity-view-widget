using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A Playable Asset that represents a single AnimationClip clip.
    /// </summary>
    [System.Serializable, NotKeyable]
    public partial class AnimationPlayableAsset : PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        /// <summary>
        /// Whether the source AnimationClip loops during playback.
        /// </summary>
        public enum LoopMode
        {
            /// <summary>
            /// Use the loop time setting from the source AnimationClip.
            /// </summary>
            [Tooltip("Use the loop time setting from the source AnimationClip.")]
            UseSourceAsset = 0,

            /// <summary>
            /// The source AnimationClip loops during playback.
            /// </summary>
            [Tooltip("The source AnimationClip loops during playback.")]
            On = 1,

            /// <summary>
            /// The source AnimationClip does not loop during playback.
            /// </summary>
            [Tooltip("The source AnimationClip does not loop during playback.")]
            Off = 2
        }


        [SerializeField] private AnimationClip m_Clip;
        [SerializeField] private Vector3 m_Position =  Vector3.zero;
        [SerializeField] private Vector3 m_EulerAngles = Vector3.zero;
        [SerializeField] private bool m_UseTrackMatchFields = true;
        [SerializeField] private MatchTargetFields m_MatchTargetFields = MatchTargetFieldConstants.All;
        [SerializeField] private bool m_RemoveStartOffset = true; // set by animation track prior to compilation
        [SerializeField] private bool m_ApplyFootIK = true;
        [SerializeField] private LoopMode m_Loop = LoopMode.UseSourceAsset;


#if UNITY_EDITOR
        private AnimationOffsetPlayable m_AnimationOffsetPlayable;
#endif

        /// <summary>
        /// The translational offset of the clip
        /// </summary>
        public Vector3 position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
#if UNITY_EDITOR
                if (m_AnimationOffsetPlayable.IsValid())
                    m_AnimationOffsetPlayable.SetPosition(position);
#endif
            }
        }

        /// <summary>
        /// The rotational offset of the clip, expressed as a Quaternion
        /// </summary>
        public Quaternion rotation
        {
            get
            {
                return Quaternion.Euler(m_EulerAngles);
            }

            set
            {
                m_EulerAngles = value.eulerAngles;
#if UNITY_EDITOR
                if (m_AnimationOffsetPlayable.IsValid())
                    m_AnimationOffsetPlayable.SetRotation(value);
#endif
            }
        }

        /// <summary>
        /// The rotational offset of the clip, expressed in Euler angles
        /// </summary>
        public Vector3 eulerAngles
        {
            get { return m_EulerAngles; }
            set
            {
                m_EulerAngles = value;
#if UNITY_EDITOR
                if (m_AnimationOffsetPlayable.IsValid())
                    m_AnimationOffsetPlayable.SetRotation(rotation);
#endif
            }
        }

        /// <summary>
        /// Specifies whether to use offset matching options as defined by the track.
        /// </summary>
        public bool useTrackMatchFields
        {
            get { return m_UseTrackMatchFields; }
            set { m_UseTrackMatchFields = value; }
        }

        /// <summary>
        /// Specifies which fields should be matched when aligning offsets.
        /// </summary>
        public MatchTargetFields matchTargetFields
        {
            get { return m_MatchTargetFields; }
            set { m_MatchTargetFields = value; }
        }

        /// <summary>
        /// Whether to make the animation clip play relative to its first keyframe.
        /// </summary>
        /// <remarks>
        /// This option only applies to animation clips that animate Transform components.
        /// </remarks>
        public bool removeStartOffset
        {
            get { return m_RemoveStartOffset; }
            set { m_RemoveStartOffset = value; }
        }


        /// <summary>
        /// Enable to apply foot IK to the AnimationClip when the target is humanoid.
        /// </summary>
        public bool applyFootIK
        {
            get { return m_ApplyFootIK; }
            set { m_ApplyFootIK = value; }
        }

        /// <summary>
        /// Whether the source AnimationClip loops during playback
        /// </summary>
        public LoopMode loop
        {
            get { return m_Loop; }
            set { m_Loop = value; }
        }


        internal bool hasRootTransforms
        {
            get { return m_Clip != null && HasRootTransforms(m_Clip); }
        }

        // used for legacy 'scene' mode.
        internal AppliedOffsetMode appliedOffsetMode { get; set; }


        /// <summary>
        /// The source animation clip
        /// </summary>
        public AnimationClip clip
        {
            get { return m_Clip; }
            set
            {
                if (value != null)
                    name = "AnimationPlayableAsset of " + value.name;
                m_Clip = value;
            }
        }

        /// <summary>
        /// Returns the duration required to play the animation clip exactly once
        /// </summary>
        public override double duration
        {
            get
            {
                double length = TimeUtility.GetAnimationClipLength(clip);
                if (length < float.Epsilon)
                    return base.duration;
                return length;
            }
        }

        /// <summary>
        /// Returns a description of the PlayableOutputs that may be created for this asset.
        /// </summary>
        public override IEnumerable<PlayableBinding> outputs
        {
            get { yield return AnimationPlayableBinding.Create(name, this); }
        }

        /// <summary>
        /// Creates the root of a Playable subgraph to play the animation clip.
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable</param>
        /// <param name="go">The gameobject that triggered the graph build</param>
        /// <returns>The root playable of the subgraph</returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            Playable root = CreatePlayable(graph, m_Clip, position, eulerAngles, removeStartOffset, appliedOffsetMode, applyFootIK, m_Loop);

#if UNITY_EDITOR
            m_AnimationOffsetPlayable = AnimationOffsetPlayable.Null;
            if (root.IsValid() && root.IsPlayableOfType<AnimationOffsetPlayable>())
            {
                m_AnimationOffsetPlayable = (AnimationOffsetPlayable)root;
            }

            LiveLink();
#endif

            return root;
        }

        internal static Playable CreatePlayable(PlayableGraph graph, AnimationClip clip, Vector3 positionOffset, Vector3 eulerOffset, bool removeStartOffset, AppliedOffsetMode mode, bool applyFootIK, LoopMode loop)
        {
            if (clip == null || clip.legacy)
                return Playable.Null;


            var clipPlayable = AnimationClipPlayable.Create(graph, clip);
            clipPlayable.SetRemoveStartOffset(removeStartOffset);
            clipPlayable.SetApplyFootIK(applyFootIK);
            clipPlayable.SetOverrideLoopTime(loop != LoopMode.UseSourceAsset);
            clipPlayable.SetLoopTime(loop == LoopMode.On);

            Playable root = clipPlayable;

            if (ShouldApplyScaleRemove(mode))
            {
                var removeScale = AnimationRemoveScalePlayable.Create(graph, 1);
                graph.Connect(root, 0, removeScale, 0);
                removeScale.SetInputWeight(0, 1.0f);
                root = removeScale;
            }

            if (ShouldApplyOffset(mode, clip))
            {
                var offsetPlayable = AnimationOffsetPlayable.Create(graph, positionOffset, Quaternion.Euler(eulerOffset), 1);
                graph.Connect(root, 0, offsetPlayable, 0);
                offsetPlayable.SetInputWeight(0, 1.0F);
                root = offsetPlayable;
            }

            return root;
        }

        private static bool ShouldApplyOffset(AppliedOffsetMode mode, AnimationClip clip)
        {
            if (mode == AppliedOffsetMode.NoRootTransform || mode == AppliedOffsetMode.SceneOffsetLegacy)
                return false;

            return HasRootTransforms(clip);
        }

        private static bool ShouldApplyScaleRemove(AppliedOffsetMode mode)
        {
            return mode == AppliedOffsetMode.SceneOffsetLegacyEditor || mode == AppliedOffsetMode.SceneOffsetLegacy || mode == AppliedOffsetMode.TransformOffsetLegacy;
        }

#if UNITY_EDITOR
        public void LiveLink()
        {
            if (m_AnimationOffsetPlayable.IsValid())
            {
                m_AnimationOffsetPlayable.SetPosition(position);
                m_AnimationOffsetPlayable.SetRotation(rotation);
            }
        }

#endif

        /// <summary>
        /// Returns the capabilities of TimelineClips that contain a AnimationPlayableAsset
        /// </summary>
        public ClipCaps clipCaps
        {
            get
            {
                var caps = ClipCaps.All;
                if (m_Clip == null || (m_Loop == LoopMode.Off) || (m_Loop == LoopMode.UseSourceAsset && !m_Clip.isLooping))
                    caps &= ~ClipCaps.Looping;

                // empty clips don't support clip in. This allows trim operations to simply become
                //  move operations
                if (m_Clip == null || m_Clip.empty)
                    caps &= ~ClipCaps.ClipIn;

                return caps;
            }
        }

        /// <summary>
        /// Resets the offsets to default values
        /// </summary>
        public void ResetOffsets()
        {
            position = Vector3.zero;
            eulerAngles = Vector3.zero;
        }

        /// <inheritdoc/>
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            driver.AddFromClip(m_Clip);
        }

        internal static bool HasRootTransforms(AnimationClip clip)
        {
            if (clip == null || clip.empty)
                return false;

            return clip.hasRootMotion || clip.hasGenericRootTransform || clip.hasMotionCurves || clip.hasRootCurves;
        }
    }
}
