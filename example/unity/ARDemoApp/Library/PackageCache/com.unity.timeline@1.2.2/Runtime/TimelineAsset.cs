using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A PlayableAsset that represents a timeline.
    /// </summary>
    [Serializable]
    public partial class TimelineAsset : PlayableAsset, ISerializationCallbackReceiver, ITimelineClipAsset, IPropertyPreview
    {
        /// <summary>
        /// How the duration of the timeline is determined.
        /// </summary>
        public enum DurationMode
        {
            /// <summary>
            /// The duration of the timeline is determined based on the clips present.
            /// </summary>
            BasedOnClips,
            /// <summary>
            /// The duration of the timeline is a fixed length.
            /// </summary>
            FixedLength
        }

        /// <summary>
        /// Properties of the timeline that are used by the editor
        /// </summary>
        [Serializable]
        public class EditorSettings
        {
            internal static readonly float kMinFps = (float)TimeUtility.kFrameRateEpsilon;
            internal static readonly float kMaxFps = 1000.0f;
            internal static readonly float kDefaultFps = 60.0f;
            [HideInInspector, SerializeField] float m_Framerate = kDefaultFps;

            /// <summary>
            /// The frames per second used for snapping and time ruler display
            /// </summary>
            public float fps
            {
                get
                {
                    return m_Framerate;
                }
                set
                {
                    m_Framerate = GetValidFramerate(value);
                }
            }
        }

        [HideInInspector, SerializeField] List<ScriptableObject> m_Tracks;
        [HideInInspector, SerializeField] double m_FixedDuration; // only applied if duration mode is Fixed
        [HideInInspector, NonSerialized] TrackAsset[] m_CacheOutputTracks;
        [HideInInspector, NonSerialized] List<TrackAsset> m_CacheRootTracks;
        [HideInInspector, NonSerialized] List<TrackAsset> m_CacheFlattenedTracks;
        [HideInInspector, SerializeField] EditorSettings m_EditorSettings = new EditorSettings();
        [SerializeField] DurationMode m_DurationMode;

        [HideInInspector, SerializeField] MarkerTrack m_MarkerTrack;

        /// <summary>
        /// Settings used by timeline for editing purposes
        /// </summary>
        public EditorSettings editorSettings
        {
            get { return m_EditorSettings; }
        }

        /// <summary>
        /// The length, in seconds, of the timeline
        /// </summary>
        public override double duration
        {
            get
            {
                // @todo cache this value when rebuilt
                if (m_DurationMode == DurationMode.BasedOnClips)
                    return CalculateDuration();

                return m_FixedDuration;
            }
        }

        /// <summary>
        /// The length of the timeline when durationMode is set to fixed length.
        /// </summary>
        public double fixedDuration
        {
            get
            {
                DiscreteTime discreteDuration = (DiscreteTime)m_FixedDuration;
                if (discreteDuration <= 0)
                    return 0.0;

                //avoid having no clip evaluated at the end by removing a tick from the total duration
                return (double)discreteDuration.OneTickBefore();
            }
            set { m_FixedDuration = Math.Max(0.0, value); }
        }

        /// <summary>
        /// The mode used to determine the duration of the Timeline
        /// </summary>
        public DurationMode durationMode
        {
            get { return m_DurationMode; }
            set { m_DurationMode = value; }
        }

        /// <summary>
        /// A description of the PlayableOutputs that will be created by the timeline when instantiated.
        /// </summary>
        /// <remarks>
        /// Each track will create an PlayableOutput
        /// </remarks>
        public override IEnumerable<PlayableBinding> outputs
        {
            get
            {
                foreach (var outputTracks in GetOutputTracks())
                    foreach (var output in outputTracks.outputs)
                        yield return output;
            }
        }

        public ClipCaps clipCaps
        {
            get
            {
                var caps = ClipCaps.All;
                foreach (var track in GetRootTracks())
                {
                    foreach (var clip in track.clips)
                        caps &= clip.clipCaps;
                }
                return caps;
            }
        }

        /// <summary>
        /// Returns the the number of output tracks in the Timeline.
        /// </summary>
        /// <remarks>
        /// An output track is a track the generates a PlayableOutput. In general, an output track is any track that is not a GroupTrack, a subtrack, or override track.
        /// </remarks>
        public int outputTrackCount
        {
            get
            {
                UpdateOutputTrackCache(); // updates the cache if necessary
                return m_CacheOutputTracks.Length;
            }
        }

        /// <summary>
        /// Returns the number of tracks at the root level of the timeline.
        /// </summary>
        /// <remarks>
        /// A root track refers to all tracks that occur at the root of the timeline. These are the outmost level GroupTracks, and output tracks that do not belong to any group
        /// </remarks>
        public int rootTrackCount
        {
            get
            {
                UpdateRootTrackCache();
                return m_CacheRootTracks.Count;
            }
        }

        void OnValidate()
        {
            editorSettings.fps = GetValidFramerate(editorSettings.fps);
        }

        static float GetValidFramerate(float framerate)
        {
            return Mathf.Clamp(framerate, EditorSettings.kMinFps, EditorSettings.kMaxFps);
        }

        /// <summary>
        /// Retrieves at root track at the specified index.
        /// </summary>
        /// <param name="index">Index of the root track to get. Must be between 0 and rootTrackCount</param>
        /// <remarks>
        /// A root track refers to all tracks that occur at the root of the timeline. These are the outmost level GroupTracks, and output tracks that do not belong to any group.
        /// </remarks>
        public TrackAsset GetRootTrack(int index)
        {
            UpdateRootTrackCache();
            return m_CacheRootTracks[index];
        }

        /// <summary>
        /// Get an enumerable list of all root tracks.
        /// </summary>
        /// <returns>An IEnumerable of all root tracks.</returns>
        /// <remarks>A root track refers to all tracks that occur at the root of the timeline. These are the outmost level GroupTracks, and output tracks that do not belong to any group.</remarks>
        public IEnumerable<TrackAsset> GetRootTracks()
        {
            UpdateRootTrackCache();
            return m_CacheRootTracks;
        }

        /// <summary>
        /// Retrives the output track from the given index.
        /// </summary>
        /// <param name="index">Index of the output track to retrieve. Must be between 0 and outputTrackCount</param>
        /// <returns>The output track from the given index</returns>
        public TrackAsset GetOutputTrack(int index)
        {
            UpdateOutputTrackCache();
            return m_CacheOutputTracks[index];
        }

        /// <summary>
        /// Gets a list of all output tracks in the Timeline.
        /// </summary>
        /// <returns>An IEnumerable of all output tracks</returns>
        /// <remarks>
        /// An output track is a track the generates a PlayableOutput. In general, an output track is any track that is not a GroupTrack or subtrack.
        /// </remarks>
        public IEnumerable<TrackAsset> GetOutputTracks()
        {
            UpdateOutputTrackCache();
            return m_CacheOutputTracks;
        }

        void UpdateRootTrackCache()
        {
            if (m_CacheRootTracks == null)
            {
                if (m_Tracks == null)
                    m_CacheRootTracks = new List<TrackAsset>();
                else
                {
                    m_CacheRootTracks = new List<TrackAsset>(m_Tracks.Count);
                    if (markerTrack != null)
                    {
                        m_CacheRootTracks.Add(markerTrack);
                    }

                    foreach (var t in m_Tracks)
                    {
                        var trackAsset = t as TrackAsset;
                        if (trackAsset != null)
                            m_CacheRootTracks.Add(trackAsset);
                    }
                }
            }
        }

        void UpdateOutputTrackCache()
        {
            if (m_CacheOutputTracks == null)
            {
                var outputTracks = new List<TrackAsset>();
                foreach (var flattenedTrack in flattenedTracks)
                {
                    if (flattenedTrack != null && flattenedTrack.GetType() != typeof(GroupTrack) && !flattenedTrack.isSubTrack)
                        outputTracks.Add(flattenedTrack);
                }
                m_CacheOutputTracks = outputTracks.ToArray();
            }
        }

        internal IEnumerable<TrackAsset> flattenedTracks
        {
            get
            {
                if (m_CacheFlattenedTracks == null)
                {
                    m_CacheFlattenedTracks = new List<TrackAsset>(m_Tracks.Count * 2);
                    UpdateRootTrackCache();

                    m_CacheFlattenedTracks.AddRange(m_CacheRootTracks);
                    for (int i = 0; i < m_CacheRootTracks.Count; i++)
                    {
                        AddSubTracksRecursive(m_CacheRootTracks[i], ref m_CacheFlattenedTracks);
                    }
                }
                return m_CacheFlattenedTracks;
            }
        }

        /// <summary>
        /// Gets the marker track for this TimelineAsset.
        /// </summary>
        /// <returns>Returns the marker track.</returns>
        /// <remarks>
        /// Use <see cref="TrackAsset.GetMarkers"/> to get a list of the markers on the returned track.
        /// </remarks>
        public MarkerTrack markerTrack
        {
            get { return m_MarkerTrack; }
        }

        // access to the track list as scriptable object
        internal List<ScriptableObject> trackObjects
        {
            get { return m_Tracks; }
        }

        internal void AddTrackInternal(TrackAsset track)
        {
            m_Tracks.Add(track);
            track.parent = this;
            Invalidate();
        }

        internal void RemoveTrack(TrackAsset track)
        {
            m_Tracks.Remove(track);
            Invalidate();
            var parentTrack = track.parent as TrackAsset;
            if (parentTrack != null)
            {
                parentTrack.RemoveSubTrack(track);
            }
        }

        /// <summary>
        /// Creates an instance of the timeline
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable</param>
        /// <param name="go">The gameobject that triggered the graph build</param>
        /// <returns>The Root Playable of the Timeline</returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            bool autoRebalanceTree = false;
            #if UNITY_EDITOR
            autoRebalanceTree = true;
            #endif

            // only create outputs if we are not nested
            bool createOutputs = graph.GetPlayableCount() == 0;
            var timeline = TimelinePlayable.Create(graph, GetOutputTracks(), go, autoRebalanceTree, createOutputs);
            timeline.SetPropagateSetTime(true);
            return timeline.IsValid() ? timeline : Playable.Null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            m_Version = k_LatestVersion;
        }

        // resets cache on an Undo
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Invalidate(); // resets cache on an Undo
            if (m_Version < k_LatestVersion)
            {
                UpgradeToLatestVersion();
            }
        }

        void __internalAwake()
        {
            if (m_Tracks == null)
                m_Tracks = new List<ScriptableObject>();

            // validate the array. DON'T remove Unity null objects, just actual null objects
            for (int i = m_Tracks.Count - 1; i >= 0; i--)
            {
                TrackAsset asset = m_Tracks[i] as TrackAsset;
                if (asset != null)
                    asset.parent = this;
#if UNITY_EDITOR
                object o = m_Tracks[i];
                if (o == null)
                {
                    Debug.LogWarning("Empty track found while loading timeline. It will be removed.");
                    m_Tracks.RemoveAt(i);
                }
#endif
            }
        }

        /// <summary>
        /// Called by the Timeline Editor to gather properties requiring preview.
        /// </summary>
        /// <param name="director">The PlayableDirector invoking the preview</param>
        /// <param name="driver">PropertyCollector used to gather previewable properties</param>
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var outputTracks = GetOutputTracks();
            foreach (var track in outputTracks)
            {
                track.GatherProperties(director, driver);
            }
        }

        /// <summary>
        /// Creates a marker track for the TimelineAsset.
        /// </summary>
        /// In the editor, the marker track appears under the Timeline ruler.
        /// <remarks>
        /// This track is always bound to the GameObject that contains the PlayableDirector component for the current timeline.
        /// The marker track is created the first time this method is called. If the marker track is already created, this method does nothing.
        /// </remarks>
        public void CreateMarkerTrack()
        {
            if (m_MarkerTrack == null)
            {
                m_MarkerTrack = CreateInstance<MarkerTrack>();
                TimelineCreateUtilities.SaveAssetIntoObject(m_MarkerTrack, this);
                m_MarkerTrack.parent = this;
                m_MarkerTrack.name = "Markers"; // This name will show up in the bindings list if it contains signals
                Invalidate();
            }
        }

        // Invalidates the asset, call this if changing the asset data
        internal void Invalidate()
        {
            m_CacheRootTracks = null;
            m_CacheOutputTracks = null;
            m_CacheFlattenedTracks = null;
        }

        double CalculateDuration()
        {
            var discreteDuration = new DiscreteTime(0);
            foreach (var track in flattenedTracks)
            {
                if (track.muted)
                    continue;

                discreteDuration = DiscreteTime.Max(discreteDuration, (DiscreteTime)track.end);
            }

            if (discreteDuration <= 0)
                return 0.0;

            //avoid having no clip evaluated at the end by removing a tick from the total duration
            return (double)discreteDuration.OneTickBefore();
        }

        static void AddSubTracksRecursive(TrackAsset track, ref List<TrackAsset> allTracks)
        {
            if (track == null)
                return;

            allTracks.AddRange(track.GetChildTracks());
            foreach (TrackAsset subTrack in track.GetChildTracks())
            {
                AddSubTracksRecursive(subTrack, ref allTracks);
            }
        }
    }
}
