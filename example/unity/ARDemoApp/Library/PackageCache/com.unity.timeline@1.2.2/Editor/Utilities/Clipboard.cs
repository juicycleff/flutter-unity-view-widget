using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class Clipboard
    {
        class ExposedReferenceTable : IExposedPropertyTable
        {
            Dictionary<PropertyName, Object> m_ReferenceTable = new Dictionary<PropertyName, Object>();
            public void SetReferenceValue(PropertyName id, Object value)
            {
                m_ReferenceTable[id] = value;
            }

            public Object GetReferenceValue(PropertyName id, out bool idValid)
            {
                Object reference;
                idValid = m_ReferenceTable.TryGetValue(id, out reference);
                return reference;
            }

            public void ClearReferenceValue(PropertyName id)
            {
                m_ReferenceTable.Remove(id);
            }

            public void Clear()
            {
                m_ReferenceTable.Clear();
            }
        }
        
        public struct ClipboardTrackEntry
        {
            public TrackAsset item;
            public TrackAsset parent;
        }

        static readonly int kListInitialSize = 10;

        readonly List<ItemsPerTrack> m_ItemsData = new List<ItemsPerTrack>(kListInitialSize);
        readonly List<ClipboardTrackEntry> m_trackData = new List<ClipboardTrackEntry>(kListInitialSize);
        TimelineAsset rootTimeline;
        
        public readonly IExposedPropertyTable exposedPropertyTable = new ExposedReferenceTable();

        public Clipboard()
        {
            rootTimeline = CreateTimeline();

            EditorApplication.playModeStateChanged  += OnPlayModeChanged;
        }

        public void CopyItems(IEnumerable<ITimelineItem> items)
        {
            using (new TimelineUndo.DisableUndoGuard(true))
            {
                var itemsByParent = items.ToLookup(i => i.parentTrack);
                foreach (var itemsGroup in itemsByParent)
                {
                    var parent = itemsGroup.Key;
                    var itemsList = new List<ITimelineItem>();
                    foreach (var item in itemsGroup)
                    {
                        if (item is ClipItem)
                            itemsList.Add(CopyItem((ClipItem)item));
                        else if (item is MarkerItem)
                            itemsList.Add(CopyItem((MarkerItem)item));
                    }
                    m_ItemsData.Add(new ItemsPerTrack(parent, itemsList));
                }
            }
        }

        ClipItem CopyItem(ClipItem clipItem)
        {
            var newClip = TimelineHelpers.Clone(clipItem.clip, TimelineWindow.instance.state.editSequence.director, exposedPropertyTable,rootTimeline);
            return new ClipItem(newClip);
        }

        static MarkerItem CopyItem(MarkerItem markerItem)
        {
            var markerObject = markerItem.marker as Object;
            if (markerObject != null)
            {
                var newMarker = Object.Instantiate(markerObject);
                newMarker.name = markerObject.name;
                return new MarkerItem((IMarker)newMarker);
            }

            return null;
        }

        public void CopyTracks(IEnumerable<TrackAsset> tracks)
        {
            using (new TimelineUndo.DisableUndoGuard(true))
            {
                foreach (var track in TrackExtensions.FilterTracks(tracks))
                {
                    var newTrack = track.Duplicate(TimelineEditor.inspectedDirector, TimelineEditor.clipboard.exposedPropertyTable, rootTimeline);
                    m_trackData.Add(new ClipboardTrackEntry {item = newTrack, parent = track.parent as TrackAsset});
                }
            }
        }

        public IEnumerable<ClipboardTrackEntry> GetTracks()
        {
            return m_trackData;
        }

        public IEnumerable<ItemsPerTrack> GetCopiedItems()
        {
            return m_ItemsData;
        }

        public void Clear()
        {
            m_ItemsData.Clear();
            m_trackData.Clear();
            rootTimeline = CreateTimeline();
            ((ExposedReferenceTable) exposedPropertyTable).Clear();
        }

        private void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode)
                Clear();
        }

        static TimelineAsset CreateTimeline()
        {
            var timeline = ScriptableObject.CreateInstance<TimelineAsset>();
            timeline.hideFlags |= HideFlags.DontSave;
            timeline.name = "Clipboard";

            return timeline;
        }
    }
}
