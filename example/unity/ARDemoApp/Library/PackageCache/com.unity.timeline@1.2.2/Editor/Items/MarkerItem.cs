using System;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class MarkerItem : ITimelineItem
    {
        readonly IMarker m_Marker;

        public IMarker marker
        {
            get { return m_Marker; }
        }

        public MarkerItem(IMarker marker)
        {
            m_Marker = marker;
        }

        public TrackAsset parentTrack
        {
            get { return m_Marker.parent; }
            set {}
        }

        public double start
        {
            get { return m_Marker.time; }
            set { m_Marker.time = value; }
        }

        public double end
        {
            get { return m_Marker.time; }
        }

        public double duration
        {
            get { return 0.0; }
        }

        public bool IsCompatibleWithTrack(TrackAsset track)
        {
            return true;
        }

        public void PushUndo(string operation)
        {
            var m = m_Marker as Object;
            if (m != null)
            {
                TimelineUndo.PushUndo(m, operation);
            }
            else
            {
                TimelineUndo.PushUndo(m_Marker.parent, operation);
            }
        }

        public TimelineItemGUI gui
        {
            get { return ItemToItemGui.GetGuiForMarker(m_Marker); }
        }

        public void Delete()
        {
            MarkerModifier.DeleteMarker(m_Marker);
        }

        public ITimelineItem CloneTo(TrackAsset parent, double time)
        {
            var item = new MarkerItem(MarkerModifier.CloneMarkerToParent(m_Marker, parent));
            item.start = time;
            return item;
        }

        protected bool Equals(MarkerItem otherMarker)
        {
            return Equals(m_Marker, otherMarker.m_Marker);
        }

        public override int GetHashCode()
        {
            return (m_Marker != null ? m_Marker.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return m_Marker.ToString();
        }

        public bool Equals(ITimelineItem other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as MarkerItem;
            return other != null && Equals(other);
        }
    }
}
