using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TrimItemModeReplace : ITrimItemMode, ITrimItemDrawer
    {
        ITrimmable m_Item;

        ITrimmable m_ItemToBeReplaced;
        double m_ClipOriginalEdgeValue;
        bool m_TrimReplace;

        double m_Min;
        double m_Max;

        public void OnBeforeTrim(ITrimmable item, TrimEdge trimDirection)
        {
            m_Item = item;

            var sortedClips = ItemsUtils.GetItemsExcept(item.parentTrack, new[] { item })
                .OfType<ITrimmable>()
                .OrderBy(c => c.start);

            var clipStart = (DiscreteTime)item.start;
            var clipEnd = (DiscreteTime)item.end;

            var overlapped = sortedClips.LastOrDefault(c => (DiscreteTime)c.start == clipStart && (DiscreteTime)c.end == clipEnd);

            ITrimmable nextItem;
            ITrimmable prevItem;

            m_Min = 0.0;
            m_Max = double.PositiveInfinity;

            if (trimDirection == TrimEdge.Start)
            {
                nextItem = sortedClips.FirstOrDefault(c => (DiscreteTime)c.start >= clipStart && (DiscreteTime)c.end > clipEnd);
                prevItem = sortedClips.LastOrDefault(c => (DiscreteTime)c.start <= clipStart && (DiscreteTime)c.end < clipEnd);

                if (prevItem != null)
                    m_Min = prevItem.start + EditModeUtils.BlendDuration(prevItem, TrimEdge.Start) + TimelineClip.kMinDuration;

                if (nextItem != null)
                    m_Max = nextItem.start;

                m_ItemToBeReplaced = prevItem;

                if (m_ItemToBeReplaced != null)
                    m_ClipOriginalEdgeValue = m_ItemToBeReplaced.end;
            }
            else
            {
                nextItem = sortedClips.FirstOrDefault(c => c != overlapped && (DiscreteTime)c.start >= clipStart && (DiscreteTime)c.end >= clipEnd);
                prevItem = sortedClips.LastOrDefault(c => c != overlapped && (DiscreteTime)c.start <= clipStart && (DiscreteTime)c.end <= clipEnd);

                if (prevItem != null)
                    m_Min = prevItem.end;

                if (nextItem != null)
                    m_Max = nextItem.end - EditModeUtils.BlendDuration(nextItem, TrimEdge.End) - TimelineClip.kMinDuration;

                m_ItemToBeReplaced = nextItem;

                if (m_ItemToBeReplaced != null)
                    m_ClipOriginalEdgeValue = m_ItemToBeReplaced.start;
            }

            m_TrimReplace = false;
        }

        public void TrimStart(ITrimmable item, double time)
        {
            time = Math.Min(Math.Max(time, m_Min), m_Max);

            if (m_ItemToBeReplaced != null)
            {
                if (!m_TrimReplace)
                    m_TrimReplace = item.start >= m_ItemToBeReplaced.end;
            }

            time = Math.Max(time, 0.0);

            item.SetStart(time);

            if (m_ItemToBeReplaced != null && m_TrimReplace)
            {
                var prevEnd = Math.Min(item.start, m_ClipOriginalEdgeValue);
                m_ItemToBeReplaced.SetEnd(prevEnd, false);
            }
        }

        public void TrimEnd(ITrimmable item, double time, bool affectTimeScale)
        {
            time = Math.Min(Math.Max(time, m_Min), m_Max);

            if (m_ItemToBeReplaced != null)
            {
                if (!m_TrimReplace)
                    m_TrimReplace = item.end <= m_ItemToBeReplaced.start;
            }

            item.SetEnd(time, affectTimeScale);

            if (m_ItemToBeReplaced != null && m_TrimReplace)
            {
                var nextStart = Math.Max(item.end, m_ClipOriginalEdgeValue);
                m_ItemToBeReplaced.SetStart(nextStart);
            }
        }

        public void DrawGUI(WindowState state, Rect bounds, Color color, TrimEdge edge)
        {
            bool shouldDraw = m_ItemToBeReplaced != null && (edge == TrimEdge.End && m_Item.end > m_ClipOriginalEdgeValue) ||
                (edge == TrimEdge.Start && m_Item.start < m_ClipOriginalEdgeValue);

            if (shouldDraw)
            {
                var cursorType = TimelineCursors.CursorType.Replace;
                if (EditModeUtils.HasBlends(m_Item, edge))
                {
                    color = DirectorStyles.kMixToolColor;
                    cursorType = (edge == TrimEdge.End)
                        ? TimelineCursors.CursorType.MixRight
                        : TimelineCursors.CursorType.MixLeft;
                }

                EditModeGUIUtils.DrawBoundsEdge(bounds, color, edge);
                TimelineCursors.SetCursor(cursorType);
            }
            else
            {
                TimelineCursors.ClearCursor();
            }
        }
    }
}
