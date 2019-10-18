using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TrimItemModeRipple : ITrimItemMode, ITrimItemDrawer
    {
        double m_OriginalClipStart;
        double m_OriginalClipEnd;

        ITrimmable[] m_NextItems;

        double m_BlendDuration;

        double m_TrimStartShift;

        public void OnBeforeTrim(ITrimmable item, TrimEdge trimDirection)
        {
            m_OriginalClipStart = item.start;
            m_OriginalClipEnd = item.end;
            m_TrimStartShift = 0.0;

            var sortedClips = ItemsUtils.GetItemsExcept(item.parentTrack, new[] { item })
                .OfType<ITrimmable>()
                .OrderBy(c => c.start);

            var clipStart = (DiscreteTime)item.start;
            var clipEnd = (DiscreteTime)item.end;

            m_NextItems = sortedClips.Where(c => (DiscreteTime)c.start >= clipStart && (DiscreteTime)c.end >= clipEnd).ToArray();

            var overlapped = sortedClips.LastOrDefault(c => (DiscreteTime)c.start == clipStart && (DiscreteTime)c.end == clipEnd);

            if (overlapped != null)
            {
                m_BlendDuration = overlapped.end - overlapped.start;
            }
            else
            {
                m_BlendDuration = 0.0;

                var prevClip = sortedClips.LastOrDefault(c => (DiscreteTime)c.start <= clipStart && (DiscreteTime)c.end <= clipEnd);
                if (prevClip != null)
                    m_BlendDuration += Math.Max(prevClip.end - item.start, 0.0);

                var nextClip = sortedClips.FirstOrDefault(c => (DiscreteTime)c.start >= clipStart && (DiscreteTime)c.end >= clipEnd);
                if (nextClip != null)
                    m_BlendDuration += Math.Max(item.end - nextClip.start, 0.0);
            }
        }

        public void TrimStart(ITrimmable item, double time)
        {
            var prevEnd = item.end;

            // HACK If time is negative, make sure we shift the SetStart operation to a positive space.
            if (time < 0.0)
                m_TrimStartShift = Math.Max(-time, m_TrimStartShift);

            item.start = m_OriginalClipEnd - item.duration + m_TrimStartShift;
            time += m_TrimStartShift;

            if (m_BlendDuration > 0.0)
                time = Math.Min(time, item.end - m_BlendDuration);

            item.SetStart(time);

            item.start = m_OriginalClipStart;

            var offset = item.end - prevEnd;
            foreach (var timelineClip in m_NextItems)
                timelineClip.start += offset;
        }

        public void TrimEnd(ITrimmable item, double time, bool affectTimeScale)
        {
            var prevEnd = item.end;

            if (m_BlendDuration > 0.0)
                time = Math.Max(time, item.start + m_BlendDuration);

            item.SetEnd(time, affectTimeScale);

            var offset = item.end - prevEnd;
            foreach (var timelineClip in m_NextItems)
                timelineClip.start += offset;
        }

        public void DrawGUI(WindowState state, Rect bounds, Color color, TrimEdge edge)
        {
            EditModeGUIUtils.DrawBoundsEdge(bounds, color, edge);
            TimelineCursors.SetCursor(TimelineCursors.CursorType.Ripple);
        }
    }
}
