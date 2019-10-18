using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    [Serializable]
    class TrackViewModelData : ISerializationCallbackReceiver
    {
        public static readonly float DefaultinlineAnimationCurveHeight = 100.0f;

        public bool collapsed = true;
        public bool showMarkers = true;

        public bool showInlineCurves = false;
        public float inlineAnimationCurveHeight = DefaultinlineAnimationCurveHeight;
        public int lastInlineCurveDataID = -1;
        public TreeViewState inlineCurvesState = null;
        public Rect inlineCurvesShownAreaInsideMargins = new Rect(1, 1, 1, 1);

        public Dictionary<int, long> markerTimeStamps = new Dictionary<int, long>();
        [SerializeField] List<int> m_MarkerTimeStampsKeys;
        [SerializeField] List<long> m_MarkerTimeStampsValues;

        public void OnBeforeSerialize()
        {
            if (markerTimeStamps == null)
                return;

            m_MarkerTimeStampsKeys = new List<int>(markerTimeStamps.Count);
            m_MarkerTimeStampsValues = new List<long>(markerTimeStamps.Count);

            foreach (var kvp in markerTimeStamps)
            {
                m_MarkerTimeStampsKeys.Add(kvp.Key);
                m_MarkerTimeStampsValues.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            markerTimeStamps = new Dictionary<int, long>();

            if (m_MarkerTimeStampsKeys == null || m_MarkerTimeStampsValues == null ||
                m_MarkerTimeStampsKeys.Count != m_MarkerTimeStampsValues.Count)
                return;

            for (int i = 0; i < m_MarkerTimeStampsKeys.Count; ++i)
                markerTimeStamps.Add(m_MarkerTimeStampsKeys[i], m_MarkerTimeStampsValues[i]);
        }
    }

    [Serializable]
    class TimelineAssetViewModel : ScriptableObject, ISerializationCallbackReceiver
    {
        public const float DefaultTrackScale = 1.0f;
        public const float DefaultVerticalScroll = 0;

        public static readonly Vector2 TimeAreaDefaultRange = new Vector2(-WindowConstants.timeAreaShownRangePadding, 5.0f); // in seconds. Hack: using negative value to force the UI to have a left margin at 0.
        public static readonly Vector2 NoPlayRangeSet = new Vector2(float.MaxValue, float.MaxValue);

        public bool timeInFrames = true;
        public Vector2 timeAreaShownRange = TimeAreaDefaultRange;
        public bool showAudioWaveform = true;
        public float trackScale = DefaultTrackScale;
        public bool playRangeEnabled;
        public Vector2 timeAreaPlayRange = NoPlayRangeSet;
        public double windowTime;
        public float verticalScroll = DefaultVerticalScroll;
        public bool showMarkerHeader;

        public Dictionary<TrackAsset, TrackViewModelData> tracksViewModelData = new Dictionary<TrackAsset, TrackViewModelData>();

        // Used only for serialization of the dictionary
        [SerializeField] List<TrackAsset> m_Keys = new List<TrackAsset>();
        [SerializeField] List<TrackViewModelData> m_Vals = new List<TrackViewModelData>();

        public void OnBeforeSerialize()
        {
            m_Keys.Clear();
            m_Vals.Clear();
            foreach (var data in tracksViewModelData)
            {
                // Assets that don't save, will create nulls when deserializeds
                if (data.Key != null && data.Value != null && (data.Key.hideFlags & HideFlags.DontSave) == 0)
                {
                    m_Keys.Add(data.Key);
                    m_Vals.Add(data.Value);
                }
            }
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnEnable()
        {
            if (m_Keys.Count == m_Vals.Count)
            {
                tracksViewModelData.Clear();
                for (int i = 0; i < m_Keys.Count; i++)
                {
                    if (m_Keys[i] != null) // if the asset is overwritten the tracks can be null
                        tracksViewModelData[m_Keys[i]] = m_Vals[i];
                }
            }

            m_Keys.Clear();
            m_Vals.Clear();
        }
    }
}
