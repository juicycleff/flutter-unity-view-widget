using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class ItemsGroup
    {
        readonly ITimelineItem[] m_Items;
        readonly ITimelineItem m_LeftMostItem;
        readonly ITimelineItem m_RightMostItem;

        public ITimelineItem[] items
        {
            get { return m_Items; }
        }

        public double start
        {
            get { return m_LeftMostItem.start; }
            set
            {
                var offset = value - m_LeftMostItem.start;

                foreach (var clip in m_Items)
                    clip.start += offset;
            }
        }

        public double end
        {
            get { return m_RightMostItem.end; }
        }

        public ITimelineItem leftMostItem
        {
            get { return m_LeftMostItem;  }
        }

        public ITimelineItem rightMostItem
        {
            get { return m_RightMostItem; }
        }

        public ItemsGroup(IEnumerable<ITimelineItem> items)
        {
            Debug.Assert(items != null && items.Any());

            m_Items = items.ToArray();
            m_LeftMostItem = null;
            m_RightMostItem = null;

            foreach (var item in m_Items)
            {
                if (m_LeftMostItem == null || item.start < m_LeftMostItem.start)
                    m_LeftMostItem = item;

                if (m_RightMostItem == null || item.end > m_RightMostItem.end)
                    m_RightMostItem = item;
            }
        }
    }
}
