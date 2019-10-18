using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class ItemsPerTrack
    {
        public virtual TrackAsset targetTrack { get; }

        public IEnumerable<ITimelineItem> items
        {
            get { return m_ItemsGroup.items; }
        }

        public IEnumerable<TimelineClip> clips
        {
            get { return m_ItemsGroup.items.OfType<ClipItem>().Select(i => i.clip); }
        }

        public IEnumerable<IMarker> markers
        {
            get { return m_ItemsGroup.items.OfType<MarkerItem>().Select(i => i.marker); }
        }

        public ITimelineItem leftMostItem
        {
            get { return m_ItemsGroup.leftMostItem; }
        }

        public ITimelineItem rightMostItem
        {
            get { return m_ItemsGroup.rightMostItem; }
        }

        protected readonly ItemsGroup m_ItemsGroup;

        public ItemsPerTrack(TrackAsset targetTrack, IEnumerable<ITimelineItem> items)
        {
            this.targetTrack = targetTrack;
            m_ItemsGroup = new ItemsGroup(items);
        }
    }
}
