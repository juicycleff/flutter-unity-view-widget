using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A PlayableAsset representing a track inside a timeline.
    /// </summary>
    [Serializable]
    [IgnoreOnPlayableTrack]
    public abstract partial class TrackAsset : PlayableAsset, IPropertyPreview, ICurvesOwner
    {
        // Internal caches used to avoid memory allocation during graph construction
        private struct TransientBuildData
        {
            public List<TrackAsset> trackList;
            public List<TimelineClip> clipList;
            public List<IMarker> markerList;

            public static TransientBuildData Create()
            {
                return new TransientBuildData()
                {
                    trackList = new List<TrackAsset>(20),
                    clipList = new List<TimelineClip>(500),
                    markerList = new List<IMarker>(100),
                };
            }

            public void Clear()
            {
                trackList.Clear();
                clipList.Clear();
                markerList.Clear();
            }
        }

        private static TransientBuildData s_BuildData = TransientBuildData.Create();

        internal const string kDefaultCurvesName = "Track Parameters";

        internal static event Action<TimelineClip, GameObject, Playable> OnClipPlayableCreate;
        internal static event Action<TrackAsset, GameObject, Playable> OnTrackAnimationPlayableCreate;

        [SerializeField, HideInInspector] bool m_Locked;
        [SerializeField, HideInInspector] bool m_Muted;
        [SerializeField, HideInInspector] string m_CustomPlayableFullTypename = string.Empty;
        [SerializeField, HideInInspector] AnimationClip m_Curves;
        [SerializeField, HideInInspector] PlayableAsset m_Parent;
        [SerializeField, HideInInspector] List<ScriptableObject> m_Children;

        [NonSerialized] int m_ItemsHash;
        [NonSerialized] TimelineClip[] m_ClipsCache;

        DiscreteTime m_Start;
        DiscreteTime m_End;
        bool m_CacheSorted;
        bool? m_SupportsNotifications;

        static TrackAsset[] s_EmptyCache = new TrackAsset[0];
        IEnumerable<TrackAsset> m_ChildTrackCache;

        static Dictionary<Type, TrackBindingTypeAttribute> s_TrackBindingTypeAttributeCache = new Dictionary<Type, TrackBindingTypeAttribute>();

        [SerializeField, HideInInspector] protected internal List<TimelineClip> m_Clips = new List<TimelineClip>();

        [SerializeField, HideInInspector] MarkerList m_Markers = new MarkerList(0);

#if UNITY_EDITOR
        internal int DirtyIndex { get; private set; }
        internal void MarkDirty()
        {
            DirtyIndex++;
            foreach (var clip in GetClips())
            {
                if (clip != null)
                    clip.MarkDirty();
            }
        }

#endif

        /// <summary>
        /// The start time, in seconds, of this track
        /// </summary>
        public double start
        {
            get
            {
                UpdateDuration();
                return (double)m_Start;
            }
        }

        /// <summary>
        /// The end time, in seconds, of this track
        /// </summary>
        public double end
        {
            get
            {
                UpdateDuration();
                return (double)m_End;
            }
        }

        /// <summary>
        /// The length, in seconds, of this track
        /// </summary>
        public sealed override double duration
        {
            get
            {
                UpdateDuration();
                return (double)(m_End - m_Start);
            }
        }

        /// <summary>
        /// Whether the track is muted or not.
        /// </summary>
        /// <remarks>
        /// A muted track is excluded from the generated PlayableGraph
        /// </remarks>
        public bool muted
        {
            get { return m_Muted; }
            set { m_Muted = value; }
        }

        /// <summary>
        /// The muted state of a track.
        /// </summary>
        /// <remarks>
        /// A track is also muted when one of its parent tracks are muted.
        /// </remarks>
        public bool mutedInHierarchy
        {
            get
            {
                if (muted)
                    return true;

                TrackAsset p = this;
                while (p.parent as TrackAsset != null)
                {
                    p = (TrackAsset)p.parent;
                    if (p as GroupTrack != null)
                        return p.mutedInHierarchy;
                }

                return false;
            }
        }

        /// <summary>
        /// The TimelineAsset that this track belongs to.
        /// </summary>
        public TimelineAsset timelineAsset
        {
            get
            {
                var node = this;
                while (node != null)
                {
                    if (node.parent == null)
                        return null;

                    var seq = node.parent as TimelineAsset;
                    if (seq != null)
                        return seq;

                    node = node.parent as TrackAsset;
                }
                return null;
            }
        }

        /// <summary>
        /// The owner of this track.
        /// </summary>
        /// <remarks>
        /// If this track is a subtrack, the parent is a TrackAsset. Otherwise the parent is a TimelineAsset.
        /// </remarks>
        public PlayableAsset parent
        {
            get { return m_Parent; }
            internal set { m_Parent = value; }
        }

        /// <summary>
        /// A list of clips owned by this track
        /// </summary>
        /// <returns>Returns an enumerable list of clips owned by the track.</returns>
        public IEnumerable<TimelineClip> GetClips()
        {
            return clips;
        }

        internal TimelineClip[] clips
        {
            get
            {
                if (m_Clips == null)
                    m_Clips = new List<TimelineClip>();

                if (m_ClipsCache == null)
                {
                    m_CacheSorted = false;
                    m_ClipsCache = m_Clips.ToArray();
                }

                return m_ClipsCache;
            }
        }

        /// <summary>
        /// Whether this track is considered empty.
        /// </summary>
        /// <remarks>
        /// A track is considered empty when it does not contain a TimelineClip, Marker, or Curve.
        /// </remarks>
        /// <remarks>
        /// Empty tracks are not included in the playable graph.
        /// </remarks>
        public virtual bool isEmpty
        {
            get { return !hasClips && !hasCurves && GetMarkerCount() == 0; }
        }

        /// <summary>
        /// Whether this track contains any TimelineClip.
        /// </summary>
        public bool hasClips
        {
            get { return m_Clips != null && m_Clips.Count != 0; }
        }

        /// <summary>
        /// Whether this track contains animated properties for the attached PlayableAsset.
        /// </summary>
        /// <remarks>
        /// This property is false if the curves property is null or if it contains no information.
        /// </remarks>
        public bool hasCurves
        {
            get { return m_Curves != null && !m_Curves.empty; }
        }

        /// <summary>
        /// Returns whether this track is a subtrack
        /// </summary>
        public bool isSubTrack
        {
            get
            {
                var owner = parent as TrackAsset;
                return owner != null && owner.GetType() == GetType();
            }
        }


        /// <summary>
        /// Returns a description of the PlayableOutputs that will be created by this track.
        /// </summary>
        public override IEnumerable<PlayableBinding> outputs
        {
            get
            {
                TrackBindingTypeAttribute attribute;
                if (!s_TrackBindingTypeAttributeCache.TryGetValue(GetType(), out attribute))
                {
                    attribute = (TrackBindingTypeAttribute)Attribute.GetCustomAttribute(GetType(), typeof(TrackBindingTypeAttribute));
                    s_TrackBindingTypeAttributeCache.Add(GetType(), attribute);
                }

                var trackBindingType = attribute != null ? attribute.type : null;
                yield return ScriptPlayableBinding.Create(name, this, trackBindingType);
            }
        }

        /// <summary>
        /// The list of subtracks or child tracks attached to this track.
        /// </summary>
        /// <returns>Returns an enumerable list of child tracks owned directly by this track.</returns>
        /// <remarks>
        /// In the case of GroupTracks, this returns all tracks contained in the group. This will return the all subtracks or override tracks, if supported by the track.
        /// </remarks>
        public IEnumerable<TrackAsset> GetChildTracks()
        {
            UpdateChildTrackCache();
            return m_ChildTrackCache;
        }

        internal string customPlayableTypename
        {
            get { return m_CustomPlayableFullTypename; }
            set { m_CustomPlayableFullTypename = value; }
        }

        /// <summary>
        /// An animation clip storing animated properties of the attached PlayableAsset
        /// </summary>
        public AnimationClip curves
        {
            get { return m_Curves; }
            internal set { m_Curves = value; }
        }

        string ICurvesOwner.defaultCurvesName
        {
            get { return kDefaultCurvesName; }
        }

        Object ICurvesOwner.asset
        {
            get { return this; }
        }

        Object ICurvesOwner.assetOwner
        {
            get { return timelineAsset; }
        }

        TrackAsset ICurvesOwner.targetTrack
        {
            get { return this; }
        }

        // for UI where we need to detect 'null' objects
        internal List<ScriptableObject> subTracksObjects
        {
            get { return m_Children; }
        }

        /// <summary>
        /// The local locked state of the track.
        /// </summary>
        /// <remarks>
        /// Note that locking a track only affects operations in the Timeline Editor. It does not prevent other API calls from changing a track or it's clips.
        ///
        /// This returns or sets the local locked state of the track. A track may still be locked for editing because one or more of it's parent tracks in the hierarchy is locked. Use lockedInHierarchy to test if a track is locked because of it's own locked state or because of a parent tracks locked state.
        /// </remarks>
        public bool locked
        {
            get { return m_Locked; }
            set { m_Locked = value; }
        }

        /// <summary>
        /// The locked state of a track. (RO)
        /// </summary>
        /// <remarks>
        /// Note that locking a track only affects operations in the Timeline Editor. It does not prevent other API calls from changing a track or it's clips.
        ///
        /// This indicates whether a track is locked in the Timeline Editor because either it's locked property is enabled or a parent track is locked.
        /// </remarks>
        public bool lockedInHierarchy
        {
            get
            {
                if (locked)
                    return true;

                TrackAsset p = this;
                while (p.parent as TrackAsset != null)
                {
                    p = (TrackAsset)p.parent;
                    if (p as GroupTrack != null)
                        return p.lockedInHierarchy;
                }

                return false;
            }
        }

        /// <summary>
        /// Indicates if a track accepts markers that implement <see cref="UnityEngine.Playables.INotification"/>.
        /// </summary>
        /// <remarks>
        /// Only tracks with a bound object of type <see cref="UnityEngine.GameObject"/> or <see cref="UnityEngine.Component"/> can accept notifications.
        /// </remarks>
        public bool supportsNotifications
        {
            get
            {
                if (!m_SupportsNotifications.HasValue)
                {
                    m_SupportsNotifications = NotificationUtilities.TrackTypeSupportsNotifications(GetType());
                }

                return m_SupportsNotifications.Value;
            }
        }

        void __internalAwake() //do not use OnEnable, since users will want it to initialize their class
        {
            if (m_Clips == null)
                m_Clips = new List<TimelineClip>();

            m_ChildTrackCache = null;
            if (m_Children == null)
                m_Children = new List<ScriptableObject>();
#if UNITY_EDITOR
            // validate the array. DON'T remove Unity null objects, just actual null objects
            for (int i = m_Children.Count - 1; i >= 0; i--)
            {
                object o = m_Children[i];
                if (o == null)
                {
                    Debug.LogWarning("Empty child track found while loading timeline. It will be removed.");
                    m_Children.RemoveAt(i);
                }
            }
#endif
        }

        /// <summary>
        /// Creates an AnimationClip to store animated properties for the attached PlayableAsset.
        /// </summary>
        /// <remarks>
        /// If curves already exists for this track, this method produces no result regardless of
        /// the value specified for curvesClipName.
        /// </remarks>
        /// <remarks>
        /// When used from the editor, this method attempts to save the created curves clip to the TimelineAsset.
        /// The TimelineAsset must already exist in the AssetDatabase to save the curves clip. If the TimelineAsset
        /// does not exist, the curves clip is still created but it is not saved.
        /// </remarks>
        /// <param name="curvesClipName">
        /// The name of the AnimationClip to create.
        /// This method does not ensure unique names. If you want a unique clip name, you must provide one.
        /// See ObjectNames.GetUniqueName for information on a method that creates unique names.
        /// </param>
        public void CreateCurves(string curvesClipName)
        {
            if (m_Curves != null)
                return;

            m_Curves = TimelineCreateUtilities.CreateAnimationClipForTrack(string.IsNullOrEmpty(curvesClipName) ? kDefaultCurvesName : curvesClipName, this, true);
        }

        /// <summary>
        /// Creates a mixer used to blend playables generated by clips on the track.
        /// </summary>
        /// <param name="graph">The graph to inject playables into</param>
        /// <param name="go">The GameObject that requested the graph.</param>
        /// <param name="inputCount">The number of playables from clips that will be inputs to the returned mixer</param>
        /// <returns>A handle to the [[Playable]] representing the mixer.</returns>
        /// <remarks>
        /// Override this method to provide a custom playable for mixing clips on a graph.
        /// </remarks>
        public virtual Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return Playable.Create(graph, inputCount);
        }

        /// <summary>
        /// Overrides PlayableAsset.CreatePlayable(). Not used in Timeline.
        /// </summary>
        public sealed override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return Playable.Null;
        }

        /// <summary>
        /// Creates a TimelineClip on this track.
        /// </summary>
        /// <returns>Returns a new TimelineClip that is attached to the track.</returns>
        /// <remarks>
        /// The type of the playable asset attached to the clip is determined by TrackClip attributes that decorate the TrackAsset derived class
        /// </remarks>
        public TimelineClip CreateDefaultClip()
        {
            var trackClipTypeAttributes = GetType().GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
            Type playableAssetType = null;
            foreach (var trackClipTypeAttribute in trackClipTypeAttributes)
            {
                var attribute = trackClipTypeAttribute as TrackClipTypeAttribute;
                if (attribute != null && typeof(IPlayableAsset).IsAssignableFrom(attribute.inspectedType) && typeof(ScriptableObject).IsAssignableFrom(attribute.inspectedType))
                {
                    playableAssetType = attribute.inspectedType;
                    break;
                }
            }

            if (playableAssetType == null)
            {
                Debug.LogWarning("Cannot create a default clip for type " + GetType());
                return null;
            }
            return CreateAndAddNewClipOfType(playableAssetType);
        }

        /// <summary>
        /// Creates a clip on the track with a playable asset attached, whose derived type is specified by T
        /// </summary>
        /// <typeparam name="T">A PlayableAsset derived type</typeparam>
        /// <returns>Returns a TimelineClip whose asset is of type T</returns>
        /// <remarks>
        /// Throws an InvalidOperationException if the specified type is not supported by the track.
        /// Supported types are determined by TrackClip attributes that decorate the TrackAsset derived class
        /// </remarks>
        public TimelineClip CreateClip<T>() where T : ScriptableObject, IPlayableAsset
        {
            return CreateClip(typeof(T));
        }

        /// <summary>
        /// Creates a marker of the requested type, at a specific time, and adds the marker to the current asset.
        /// </summary>
        /// <param name="type">The type of marker.</param>
        /// <param name="time">The time where the marker is created.</param>
        /// <returns>Returns the instance of the created marker.</returns>
        /// <remarks>
        /// All markers that implement IMarker and inherit from <see cref="UnityEngine.ScriptableObject"/> are supported.
        /// Markers that implement the INotification interface cannot be added to tracks that do not support notifications.
        /// CreateMarker will throw an <code>InvalidOperationException</code> with tracks that do not support notifications if <code>type</code> implements the INotification interface.
        /// </remarks>
        /// <seealso cref="UnityEngine.Timeline.Marker"/>
        /// <seealso cref="UnityEngine.Timeline.TrackAsset.supportsNotifications"/>
        public IMarker CreateMarker(Type type, double time)
        {
            return m_Markers.CreateMarker(type, time, this);
        }

        /// <summary>
        /// Creates a marker of the requested type, at a specific time, and adds the marker to the current asset.
        /// </summary>
        /// <param name="time">The time where the marker is created.</param>
        /// <returns>Returns the instance of the created marker.</returns>
        /// <remarks>
        /// All markers that implement IMarker and inherit from <see cref="UnityEngine.ScriptableObject"/> are supported.
        /// CreateMarker will throw an <code>InvalidOperationException</code> with tracks that do not support notifications if <code>T</code> implements the INotification interface.
        /// </remarks>
        /// <seealso cref="UnityEngine.Timeline.Marker"/>
        /// <seealso cref="UnityEngine.Timeline.TrackAsset.supportsNotifications"/>
        public T CreateMarker<T>(double time) where T : ScriptableObject, IMarker
        {
            return (T)CreateMarker(typeof(T), time);
        }

        /// <summary>
        /// Removes a marker from the current asset.
        /// </summary>
        /// <param name="marker">The marker instance to be removed.</param>
        /// <returns>Returns true if the marker instance was successfully removed. Returns false otherwise.</returns>
        public bool DeleteMarker(IMarker marker)
        {
            return m_Markers.Remove(marker);
        }

        /// <summary>
        /// Returns an enumerable list of markers on the current asset.
        /// </summary>
        /// <returns>The list of markers on the asset.
        /// </returns>
        public IEnumerable<IMarker> GetMarkers()
        {
            return m_Markers.GetMarkers();
        }

        /// <summary>
        /// Returns the number of markers on the current asset.
        /// </summary>
        /// <returns>The number of markers.</returns>
        public int GetMarkerCount()
        {
            return m_Markers.Count;
        }

        /// <summary>
        /// Returns the marker at a given position, on the current asset.
        /// </summary>
        /// <param name="idx">The index of the marker to be returned.</param>
        /// <returns>The marker.</returns>
        /// <remarks>The ordering of the markers is not guaranteed.
        /// </remarks>
        public IMarker GetMarker(int idx)
        {
            return m_Markers[idx];
        }

        internal TimelineClip CreateClip(System.Type requestedType)
        {
            if (ValidateClipType(requestedType))
                return CreateAndAddNewClipOfType(requestedType);

            throw new InvalidOperationException("Clips of type " + requestedType + " are not permitted on tracks of type " + GetType());
        }

        internal TimelineClip CreateAndAddNewClipOfType(Type requestedType)
        {
            var newClip = CreateClipOfType(requestedType);
            AddClip(newClip);
            return newClip;
        }

        internal TimelineClip CreateClipOfType(Type requestedType)
        {
            if (!ValidateClipType(requestedType))
                throw new System.InvalidOperationException("Clips of type " + requestedType + " are not permitted on tracks of type " + GetType());

            var playableAsset = CreateInstance(requestedType);
            if (playableAsset == null)
            {
                throw new System.InvalidOperationException("Could not create an instance of the ScriptableObject type " + requestedType.Name);
            }
            playableAsset.name = requestedType.Name;
            TimelineCreateUtilities.SaveAssetIntoObject(playableAsset, this);
            TimelineUndo.RegisterCreatedObjectUndo(playableAsset, "Create Clip");

            return CreateClipFromAsset(playableAsset);
        }

        /// <summary>
        /// Creates a timeline clip from an existing playable asset.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        internal TimelineClip CreateClipFromPlayableAsset(IPlayableAsset asset)
        {
            if (asset == null)
                throw new ArgumentNullException("asset");

            if ((asset as ScriptableObject) == null)
                throw new System.ArgumentException("CreateClipFromPlayableAsset " + " only supports ScriptableObject-derived Types");

            if (!ValidateClipType(asset.GetType()))
                throw new System.InvalidOperationException("Clips of type " + asset.GetType() + " are not permitted on tracks of type " + GetType());

            return CreateClipFromAsset(asset as ScriptableObject);
        }

        private TimelineClip CreateClipFromAsset(ScriptableObject playableAsset)
        {
            TimelineUndo.PushUndo(this, "Create Clip");

            var newClip = CreateNewClipContainerInternal();
            newClip.displayName = playableAsset.name;
            newClip.asset = playableAsset;

            IPlayableAsset iPlayableAsset = playableAsset as IPlayableAsset;
            if (iPlayableAsset != null)
            {
                var candidateDuration = iPlayableAsset.duration;

                if (!double.IsInfinity(candidateDuration) && candidateDuration > 0)
                    newClip.duration = Math.Min(Math.Max(candidateDuration, TimelineClip.kMinDuration), TimelineClip.kMaxTimeValue);
            }

            try
            {
                OnCreateClip(newClip);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message, playableAsset);
                return null;
            }

            return newClip;
        }

        internal IEnumerable<ScriptableObject> GetMarkersRaw()
        {
            return m_Markers.GetRawMarkerList();
        }

        internal void ClearMarkers()
        {
            m_Markers.Clear();
        }

        internal void AddMarker(ScriptableObject e)
        {
            m_Markers.Add(e);
        }

        internal bool DeleteMarkerRaw(ScriptableObject marker)
        {
            return m_Markers.Remove(marker, timelineAsset, this);
        }

        int GetTimeRangeHash()
        {
            double start = double.MaxValue, end = double.MinValue;
            foreach (var marker in GetMarkers())
            {
                if (!(marker is INotification))
                {
                    continue;
                }

                if (marker.time < start)
                    start = marker.time;
                if (marker.time > end)
                    end = marker.time;
            }

            return start.GetHashCode().CombineHash(end.GetHashCode());
        }

        internal void AddClip(TimelineClip newClip)
        {
            if (!m_Clips.Contains(newClip))
            {
                m_Clips.Add(newClip);
                m_ClipsCache = null;
            }
        }

        Playable CreateNotificationsPlayable(PlayableGraph graph, Playable mixerPlayable, GameObject go, Playable timelinePlayable)
        {
            s_BuildData.markerList.Clear();
            GatherNotificiations(s_BuildData.markerList);
            var notificationPlayable = NotificationUtilities.CreateNotificationsPlayable(graph, s_BuildData.markerList, go);
            if (notificationPlayable.IsValid())
            {
                notificationPlayable.GetBehaviour().timeSource = timelinePlayable;
                if (mixerPlayable.IsValid())
                {
                    notificationPlayable.SetInputCount(1);
                    graph.Connect(mixerPlayable, 0, notificationPlayable, 0);
                    notificationPlayable.SetInputWeight(mixerPlayable, 1);
                }
            }

            return notificationPlayable;
        }

        internal Playable CreatePlayableGraph(PlayableGraph graph, GameObject go, IntervalTree<RuntimeElement> tree, Playable timelinePlayable)
        {
            UpdateDuration();
            var mixerPlayable = Playable.Null;
            if (CanCompileClipsRecursive())
                mixerPlayable = OnCreateClipPlayableGraph(graph, go, tree);

            var notificationsPlayable = CreateNotificationsPlayable(graph, mixerPlayable, go, timelinePlayable);
            if (!notificationsPlayable.IsValid() && !mixerPlayable.IsValid())
            {
                Debug.LogErrorFormat("Track {0} of type {1} has no notifications and returns an invalid mixer Playable", name,
                    GetType().FullName);

                return Playable.Create(graph);
            }

            return notificationsPlayable.IsValid() ? notificationsPlayable : mixerPlayable;
        }

        internal virtual Playable CompileClips(PlayableGraph graph, GameObject go, IList<TimelineClip> timelineClips, IntervalTree<RuntimeElement> tree)
        {
            var blend = CreateTrackMixer(graph, go, timelineClips.Count);
            for (var c = 0; c < timelineClips.Count; c++)
            {
                var source = CreatePlayable(graph, go, timelineClips[c]);
                if (source.IsValid())
                {
                    source.SetDuration(timelineClips[c].duration);
                    var clip = new RuntimeClip(timelineClips[c], source, blend);
                    tree.Add(clip);
                    graph.Connect(source, 0, blend, c);
                    blend.SetInputWeight(c, 0.0f);
                }
            }
            ConfigureTrackAnimation(tree, go, blend);
            return blend;
        }

        void GatherCompilableTracks(IList<TrackAsset> tracks)
        {
            if (!muted && CanCompileClips())
                tracks.Add(this);

            foreach (var c in GetChildTracks())
            {
                if (c != null)
                    c.GatherCompilableTracks(tracks);
            }
        }

        void GatherNotificiations(List<IMarker> markers)
        {
            if (!muted && CanCompileNotifications())
                markers.AddRange(GetMarkers());
            foreach (var c in GetChildTracks())
            {
                if (c != null)
                    c.GatherNotificiations(markers);
            }
        }

        internal virtual Playable OnCreateClipPlayableGraph(PlayableGraph graph, GameObject go, IntervalTree<RuntimeElement> tree)
        {
            if (tree == null)
                throw new ArgumentException("IntervalTree argument cannot be null", "tree");

            if (go == null)
                throw new ArgumentException("GameObject argument cannot be null", "go");

            s_BuildData.Clear();
            GatherCompilableTracks(s_BuildData.trackList);

            // nothing to compile
            if (s_BuildData.trackList.Count == 0)
                return Playable.Null;

            // check if layers are supported
            Playable layerMixer = Playable.Null;
            ILayerable layerable = this as ILayerable;
            if (layerable != null)
                layerMixer = layerable.CreateLayerMixer(graph, go, s_BuildData.trackList.Count);

            if (layerMixer.IsValid())
            {
                for (int i = 0; i < s_BuildData.trackList.Count; i++)
                {
                    var mixer = s_BuildData.trackList[i].CompileClips(graph, go, s_BuildData.trackList[i].clips, tree);
                    if (mixer.IsValid())
                    {
                        graph.Connect(mixer, 0, layerMixer, i);
                        layerMixer.SetInputWeight(i, 1.0f);
                    }
                }
                return layerMixer;
            }

            // one track compiles. Add track mixer and clips
            if (s_BuildData.trackList.Count == 1)
                return s_BuildData.trackList[0].CompileClips(graph, go, s_BuildData.trackList[0].clips, tree);

            // no layer mixer provided. merge down all clips.
            for (int i = 0; i < s_BuildData.trackList.Count; i++)
                s_BuildData.clipList.AddRange(s_BuildData.trackList[i].clips);

#if UNITY_EDITOR
            bool applyWarning = false;
            for (int i = 0; i < s_BuildData.trackList.Count; i++)
                applyWarning |= i > 0 && s_BuildData.trackList[i].hasCurves;

            if (applyWarning)
                Debug.LogWarning("A layered track contains animated fields, but no layer mixer has been provided. Animated fields on layers will be ignored. Override CreateLayerMixer in " + s_BuildData.trackList[0].GetType().Name + " and return a valid playable to support animated fields on layered tracks.");
#endif
            // compile all the clips into a single mixer
            return CompileClips(graph, go, s_BuildData.clipList, tree);
        }

        internal void ConfigureTrackAnimation(IntervalTree<RuntimeElement> tree, GameObject go, Playable blend)
        {
            if (!hasCurves)
                return;

            blend.SetAnimatedProperties(m_Curves);
            tree.Add(new InfiniteRuntimeClip(blend));

            if (OnTrackAnimationPlayableCreate != null)
                OnTrackAnimationPlayableCreate.Invoke(this, go, blend);
        }

        // sorts clips by start time
        internal void SortClips()
        {
            var clipsAsArray = clips; // will alloc
            if (!m_CacheSorted)
            {
                Array.Sort(clips, (clip1, clip2) => clip1.start.CompareTo(clip2.start));
                m_CacheSorted = true;
            }
        }

        // clears the clips after a clone
        internal void ClearClipsInternal()
        {
            m_Clips = new List<TimelineClip>();
            m_ClipsCache = null;
        }

        internal void ClearSubTracksInternal()
        {
            m_Children = new List<ScriptableObject>();
            Invalidate();
        }

        // called by an owned clip when it moves
        internal void OnClipMove()
        {
            m_CacheSorted = false;
        }

        internal TimelineClip CreateNewClipContainerInternal()
        {
            var clipContainer = new TimelineClip(this);
            clipContainer.asset = null;

            // position clip at end of sequence
            var newClipStart = 0.0;
            for (var a = 0; a < m_Clips.Count - 1; a++)
            {
                var clipDuration = m_Clips[a].duration;
                if (double.IsInfinity(clipDuration))
                    clipDuration = TimelineClip.kDefaultClipDurationInSeconds;
                newClipStart = Math.Max(newClipStart, m_Clips[a].start + clipDuration);
            }

            clipContainer.mixInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            clipContainer.mixOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
            clipContainer.start = newClipStart;
            clipContainer.duration = TimelineClip.kDefaultClipDurationInSeconds;
            clipContainer.displayName = "untitled";
            return clipContainer;
        }

        internal void AddChild(TrackAsset child)
        {
            if (child == null)
                return;

            m_Children.Add(child);
            child.parent = this;
            Invalidate();
        }

        internal void MoveLastTrackBefore(TrackAsset asset)
        {
            if (m_Children == null || m_Children.Count < 2 || asset == null)
                return;

            var lastTrack = m_Children[m_Children.Count - 1];
            if (lastTrack == asset)
                return;

            for (int i = 0; i < m_Children.Count - 1; i++)
            {
                if (m_Children[i] == asset)
                {
                    for (int j = m_Children.Count - 1; j > i; j--)
                        m_Children[j] = m_Children[j - 1];
                    m_Children[i] = lastTrack;
                    Invalidate();
                    break;
                }
            }
        }

        internal bool RemoveSubTrack(TrackAsset child)
        {
            if (m_Children.Remove(child))
            {
                Invalidate();
                child.parent = null;
                return true;
            }
            return false;
        }

        internal void RemoveClip(TimelineClip clip)
        {
            m_Clips.Remove(clip);
            m_ClipsCache = null;
        }

        // Is this track compilable for the sequence
        // calculate the time interval that this track will be evaluated in.
        internal virtual void GetEvaluationTime(out double outStart, out double outDuration)
        {
            outStart = double.PositiveInfinity;
            var outEnd = double.NegativeInfinity;

            if (hasCurves)
            {
                outStart = 0.0;
                outEnd = TimeUtility.GetAnimationClipLength(curves);
            }

            foreach (var clip in clips)
            {
                outStart = Math.Min(clip.start, outStart);
                outEnd = Math.Max(clip.end, outEnd);
            }

            if (HasNotifications())
            {
                var notificationDuration = GetNotificationDuration();
                outStart = Math.Min(notificationDuration, outStart);
                outEnd = Math.Max(notificationDuration, outEnd);
            }

            if (double.IsInfinity(outStart) || double.IsInfinity(outEnd))
                outStart = outDuration = 0.0;
            else
                outDuration = outEnd - outStart;
        }

        // calculate the time interval that the sequence will use to determine length.
        // by default this is the same as the evaluation, but subclasses can have different
        // behaviour
        internal virtual void GetSequenceTime(out double outStart, out double outDuration)
        {
            GetEvaluationTime(out outStart, out outDuration);
        }

        /// <summary>
        /// Called by the Timeline Editor to gather properties requiring preview.
        /// </summary>
        /// <param name="director">The PlayableDirector invoking the preview</param>
        /// <param name="driver">PropertyCollector used to gather previewable properties</param>
        public virtual void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            // only push on game objects if there is a binding. Subtracks
            //  will use objects on the stack
            var gameObject = GetGameObjectBinding(director);
            if (gameObject != null)
                driver.PushActiveGameObject(gameObject);

            if (hasCurves)
                driver.AddObjectProperties(this, m_Curves);

            foreach (var clip in clips)
            {
                if (clip.curves != null && clip.asset != null)
                    driver.AddObjectProperties(clip.asset, clip.curves);

                IPropertyPreview modifier = clip.asset as IPropertyPreview;
                if (modifier != null)
                    modifier.GatherProperties(director, driver);
            }

            foreach (var subtrack in GetChildTracks())
            {
                if (subtrack != null)
                    subtrack.GatherProperties(director, driver);
            }

            if (gameObject != null)
                driver.PopActiveGameObject();
        }

        internal GameObject GetGameObjectBinding(PlayableDirector director)
        {
            if (director == null)
                return null;

            var binding = director.GetGenericBinding(this);

            var gameObject = binding as GameObject;
            if (gameObject != null)
                return gameObject;

            var comp = binding as Component;
            if (comp != null)
                return comp.gameObject;

            return null;
        }

        internal bool ValidateClipType(Type clipType)
        {
            var attrs = GetType().GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
            for (var c = 0; c < attrs.Length; ++c)
            {
                var attr = (TrackClipTypeAttribute)attrs[c];
                if (attr.inspectedType.IsAssignableFrom(clipType))
                    return true;
            }

            // special case for playable tracks, they accept all clips (in the runtime)
            return typeof(PlayableTrack).IsAssignableFrom(GetType()) &&
                typeof(IPlayableAsset).IsAssignableFrom(clipType) &&
                typeof(ScriptableObject).IsAssignableFrom(clipType);
        }

        /// <summary>
        /// Called when a clip is created on a track.
        /// </summary>
        /// <param name="clip">The timeline clip added to this track</param>
        /// <remarks>Use this method to set default values on a timeline clip, or it's PlayableAsset.</remarks>
        protected virtual void OnCreateClip(TimelineClip clip) {}

        void UpdateDuration()
        {
            // check if something changed in the clips that require a re-calculation of the evaluation times.
            var itemsHash = CalculateItemsHash();
            if (itemsHash == m_ItemsHash)
                return;
            m_ItemsHash = itemsHash;

            double trackStart, trackDuration;
            GetSequenceTime(out trackStart, out trackDuration);

            m_Start = (DiscreteTime)trackStart;
            m_End = (DiscreteTime)(trackStart + trackDuration);

            // calculate the extrapolations time.
            // TODO Extrapolation time should probably be extracted from the SequenceClip so only a track is aware of it.
            this.CalculateExtrapolationTimes();
        }

        protected internal virtual int CalculateItemsHash()
        {
            return HashUtility.CombineHash(GetClipsHash(), GetAnimationClipHash(m_Curves), GetTimeRangeHash());
        }

        /// <summary>
        /// Constructs a Playable from a TimelineClip.
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable.</param>
        /// <param name="gameObject">The GameObject that builds the PlayableGraph.</param>
        /// <param name="clip">The TimelineClip to construct a playable for.</param>
        /// <returns>A playable that will be set as an input to the Track Mixer playable, or Playable.Null if the clip does not have a valid PlayableAsset</returns>
        /// <exception cref="ArgumentException">Thrown if the specified PlayableGraph is not valid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the specified TimelineClip is not valid.</exception>
        /// <remarks>
        /// By default, this method invokes Playable.CreatePlayable, sets animated properties, and sets the speed of the created playable. Override this method to change this default implementation.
        /// </remarks>
        protected virtual Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            if (!graph.IsValid())
                throw new ArgumentException("graph must be a valid PlayableGraph");
            if (clip == null)
                throw new ArgumentNullException("clip");

            var asset = clip.asset as IPlayableAsset;
            if (asset != null)
            {
                var handle = asset.CreatePlayable(graph, gameObject);
                if (handle.IsValid())
                {
                    handle.SetAnimatedProperties(clip.curves);
                    handle.SetSpeed(clip.timeScale);
                    if (OnClipPlayableCreate != null)
                        OnClipPlayableCreate(clip, gameObject, handle);
                }
                return handle;
            }
            return Playable.Null;
        }

        internal void Invalidate()
        {
            m_ChildTrackCache = null;
            var timeline = timelineAsset;
            if (timeline != null)
            {
                timeline.Invalidate();
            }
        }

        internal double GetNotificationDuration()
        {
            if (!supportsNotifications)
            {
                return 0;
            }

            var maxTime = 0.0;
            foreach (var marker in GetMarkers())
            {
                if (!(marker is INotification))
                {
                    continue;
                }
                maxTime = Math.Max(maxTime, marker.time);
            }

            return maxTime;
        }

        internal virtual bool CanCompileClips()
        {
            return hasClips || hasCurves;
        }

        internal bool IsCompilable()
        {
            var isContainer = typeof(GroupTrack).IsAssignableFrom(GetType());

            if (isContainer)
                return false;

            var ret = !mutedInHierarchy && (CanCompileClips() || CanCompileNotifications());
            if (!ret)
            {
                foreach (var t in GetChildTracks())
                {
                    if (t.IsCompilable())
                        return true;
                }
            }

            return ret;
        }

        private void UpdateChildTrackCache()
        {
            if (m_ChildTrackCache == null)
            {
                if (m_Children == null || m_Children.Count == 0)
                    m_ChildTrackCache = s_EmptyCache;
                else
                {
                    var childTracks = new List<TrackAsset>(m_Children.Count);
                    for (int i = 0; i < m_Children.Count; i++)
                    {
                        var subTrack = m_Children[i] as TrackAsset;
                        if (subTrack != null)
                            childTracks.Add(subTrack);
                    }
                    m_ChildTrackCache = childTracks;
                }
            }
        }

        internal virtual int Hash()
        {
            return clips.Length + (m_Markers.Count << 16);
        }

        int GetClipsHash()
        {
            var hash = 0;
            foreach (var clip in m_Clips)
            {
                hash = hash.CombineHash(clip.Hash());
            }
            return hash;
        }

        protected static int GetAnimationClipHash(AnimationClip clip)
        {
            var hash = 0;
            if (clip != null && !clip.empty)
                hash = hash.CombineHash(clip.frameRate.GetHashCode())
                    .CombineHash(clip.length.GetHashCode());

            return hash;
        }

        bool HasNotifications()
        {
            return m_Markers.HasNotifications();
        }

        bool CanCompileNotifications()
        {
            return supportsNotifications && m_Markers.HasNotifications();
        }

        bool CanCompileClipsRecursive()
        {
            if (CanCompileClips())
                return true;
            foreach (var track in GetChildTracks())
            {
                if (track.CanCompileClipsRecursive())
                    return true;
            }

            return false;
        }
    }
}
