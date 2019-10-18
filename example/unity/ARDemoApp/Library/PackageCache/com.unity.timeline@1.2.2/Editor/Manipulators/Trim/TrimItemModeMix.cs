using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TrimItemModeMix : ITrimItemMode, ITrimItemDrawer
    {
        ITrimmable m_Item;

        double m_Min;
        double m_Max;

        public void OnBeforeTrim(ITrimmable item, TrimEdge trimDirection)
        {
            m_Item = item;

            var sortedItems = ItemsUtils.GetItemsExcept(item.parentTrack, new[] {item})
                .OfType<ITrimmable>()
                .OrderBy(c => c.start);

            var itemStart = (DiscreteTime)item.start;
            var itemEnd = (DiscreteTime)item.end;

            var overlapped = sortedItems.LastOrDefault(c => (DiscreteTime)c.start == itemStart && (DiscreteTime)c.end == itemEnd);

            ITrimmable nextItem;
            ITrimmable prevItem;

            m_Min = 0.0;
            m_Max = double.PositiveInfinity;

            var blendableItem = item as IBlendable;
            if (blendableItem != null && blendableItem.supportsBlending)
            {
                if (trimDirection == TrimEdge.Start)
                {
                    nextItem = sortedItems.FirstOrDefault(c => (DiscreteTime)c.start >= itemStart && (DiscreteTime)c.end > itemEnd);
                    prevItem = sortedItems.LastOrDefault(c => (DiscreteTime)c.start <= itemStart && (DiscreteTime)c.end < itemEnd);

                    if (prevItem != null)
                        m_Min = prevItem.start + EditModeUtils.BlendDuration(prevItem, TrimEdge.Start);

                    if (nextItem != null)
                        m_Max = nextItem.start;
                }
                else
                {
                    nextItem = sortedItems.FirstOrDefault(c => c != overlapped && (DiscreteTime)c.start >= itemStart && (DiscreteTime)c.end >= itemEnd);
                    prevItem = sortedItems.LastOrDefault(c => c != overlapped && (DiscreteTime)c.start <= itemStart && (DiscreteTime)c.end <= itemEnd);

                    if (prevItem != null)
                        m_Min = prevItem.end;

                    if (nextItem != null)
                        m_Max = nextItem.end - EditModeUtils.BlendDuration(nextItem, TrimEdge.End);
                }
            }
            else
            {
                nextItem = sortedItems.FirstOrDefault(c => (DiscreteTime)c.start > itemStart);
                prevItem = sortedItems.LastOrDefault(c => (DiscreteTime)c.start < itemStart);

                if (prevItem != null)
                    m_Min = prevItem.end;

                if (nextItem != null)
                    m_Max = nextItem.start;
            }
        }

        public void TrimStart(ITrimmable item, double time)
        {
            time = Math.Min(Math.Max(time, m_Min), m_Max);
            item.SetStart(time);
        }

        public void TrimEnd(ITrimmable item, double time, bool affectTimeScale)
        {
            time = Math.Min(Math.Max(time, m_Min), m_Max);
            item.SetEnd(time, affectTimeScale);
        }

        public void DrawGUI(WindowState state, Rect bounds, Color color, TrimEdge edge)
        {
            if (EditModeUtils.HasBlends(m_Item, edge))
            {
                EditModeGUIUtils.DrawBoundsEdge(bounds, color, edge);
                var cursorType = (edge == TrimEdge.End)
                    ? TimelineCursors.CursorType.MixRight
                    : TimelineCursors.CursorType.MixLeft;

                TimelineCursors.SetCursor(cursorType);
            }
            else
            {
                TimelineCursors.ClearCursor();
            }
        }
    }
}
