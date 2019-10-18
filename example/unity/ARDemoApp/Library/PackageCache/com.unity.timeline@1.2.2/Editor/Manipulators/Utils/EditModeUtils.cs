using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class EditModeUtils
    {
        public static void Delete(IEnumerable<ITimelineItem> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
                item.Delete();
        }

        public static void SetStart(IEnumerable<ITimelineItem> items, double time)
        {
            var offset = time - items.Min(c => c.start);

            foreach (var item in items)
                item.start += offset;
        }

        public static void SetParentTrack(IEnumerable<ITimelineItem> items, TrackAsset parentTrack)
        {
            foreach (var item in items)
            {
                if (item.parentTrack == parentTrack)
                    continue;

                item.parentTrack = parentTrack;

                var clipGUI = item.gui as TimelineClipGUI;
                if (clipGUI != null)
                {
                    clipGUI.clipCurveEditor = null;
                }
            }
        }

        public static ITimelineItem GetFirstIntersectedItem(IEnumerable<ITimelineItem> items, double time)
        {
            return items.FirstOrDefault(c => Intersects(time, c.start, c.end));
        }

        static bool Intersects(double time, double start, double end)
        {
            var discreteTime = (DiscreteTime)time;
            return discreteTime > (DiscreteTime)start && discreteTime < (DiscreteTime)end;
        }

        public static bool Overlaps(ITimelineItem item, double from, double to)
        {
            var discreteFrom = (DiscreteTime)from;
            var discreteTo = (DiscreteTime)to;
            var discreteStart = (DiscreteTime)item.start;

            if (discreteStart >= discreteFrom && discreteStart < discreteTo)
                return true;

            var discreteEnd = (DiscreteTime)item.end;

            if (discreteEnd > discreteFrom && discreteEnd <= discreteTo)
                return true;

            return false;
        }

        public static bool IsItemWithinRange(ITimelineItem item, double from, double to)
        {
            return (DiscreteTime)item.start >= (DiscreteTime)from && (DiscreteTime)item.end <= (DiscreteTime)to;
        }

        public static bool IsRangeWithinItem(double from, double to, ITimelineItem item)
        {
            return (DiscreteTime)from >= (DiscreteTime)item.start && (DiscreteTime)to <= (DiscreteTime)item.end;
        }

        public static bool Contains(double from, double to, ITimelineItem item)
        {
            return (DiscreteTime)from < (DiscreteTime)item.start && (DiscreteTime)to > (DiscreteTime)item.end;
        }

        public static bool HasBlends(ITimelineItem item, TrimEdge edge)
        {
            var blendable = item as IBlendable;
            if (blendable == null) return false;

            return edge == TrimEdge.Start && blendable.hasLeftBlend || edge == TrimEdge.End && blendable.hasRightBlend;
        }

        public static double BlendDuration(ITimelineItem item, TrimEdge edge)
        {
            var blendable = item as IBlendable;
            if (blendable == null) return 0.0;

            return edge == TrimEdge.Start ? blendable.leftBlendDuration : blendable.rightBlendDuration;
        }

        public static bool IsInfiniteTrack(TrackAsset track)
        {
            var aTrack = track as AnimationTrack;
            return aTrack != null && aTrack.CanConvertToClipMode();
        }

        public static void GetInfiniteClipBoundaries(TrackAsset track, out double start, out double end)
        {
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(((AnimationTrack)track).infiniteClip);
            if (info.keyTimes.Length > 0)
            {
                start = info.keyTimes.Min();
                end = info.keyTimes.Max();
            }
            else
            {
                start = end = 0.0f;
            }
        }
    }
}
