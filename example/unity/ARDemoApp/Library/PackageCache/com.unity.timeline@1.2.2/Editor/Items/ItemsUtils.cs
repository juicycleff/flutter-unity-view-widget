using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class ItemsUtils
    {
        static readonly Dictionary<TimelineClip, ClipItem> s_ClipCache = new Dictionary<TimelineClip, ClipItem>();
        static readonly Dictionary<IMarker, MarkerItem> s_MarkerCache = new Dictionary<IMarker, MarkerItem>();

        public static IEnumerable<ItemsPerTrack> ToItemsPerTrack(this IEnumerable<ITimelineItem> items)
        {
            var groupedItems = items.GroupBy(c => c.parentTrack);
            foreach (var group in groupedItems)
            {
                yield return new ItemsPerTrack(group.Key, group.ToArray());
            }
        }

        public static ITimelineItem ToItem(this TimelineClip clip)
        {
            if (s_ClipCache.ContainsKey(clip))
                return s_ClipCache[clip];

            var ret = new ClipItem(clip);
            s_ClipCache.Add(clip, ret);
            return ret;
        }

        public static ITimelineItem ToItem(this IMarker marker)
        {
            if (s_MarkerCache.ContainsKey(marker))
                return s_MarkerCache[marker];

            var ret = new MarkerItem(marker);
            s_MarkerCache.Add(marker, ret);
            return ret;
        }

        public static IEnumerable<ITimelineItem> ToItems(this IEnumerable<TimelineClip> clips)
        {
            return clips.Select(ToItem);
        }

        public static IEnumerable<ITimelineItem> ToItems(this IEnumerable<IMarker> markers)
        {
            return markers.Select(ToItem);
        }

        public static IEnumerable<ITimelineItem> GetItems(this TrackAsset track)
        {
            var list = track.clips.Select(clip => (ITimelineItem) new ClipItem(clip)).ToList();
            list.AddRange(track.GetMarkers().Select(marker => (ITimelineItem) new MarkerItem(marker)));

            list = list.OrderBy(x => x.start).ThenBy(x => x.end).ToList();
            return list;
        }

        public static void GetItemRange(this TrackAsset track, out double start, out double end)
        {
            start = 0;
            end = 0;
            var items = track.GetItems().ToList();
            if (items.Any())
            {
                start = items.Min(p => p.start);
                end = items.Max(p => p.end);
            }
        }

        public static IEnumerable<ITimelineItem> GetItemsExcept(this TrackAsset track, IEnumerable<ITimelineItem> items)
        {
            return GetItems(track).Except(items);
        }

        public static IEnumerable<Type> GetItemTypes(IEnumerable<ITimelineItem> items)
        {
            var types = new List<Type>();
            if (items.OfType<ClipItem>().Any())
                types.Add(typeof(ClipItem));
            if (items.OfType<MarkerItem>().Any())
                types.Add(typeof(MarkerItem));

            return types;
        }

        public static IEnumerable<Type> GetItemTypes(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            return GetItemTypes(itemsGroups.SelectMany(i => i.items)).Distinct();
        }

        public static void SetItemsStartTime(IEnumerable<ItemsPerTrack> newItems, double time)
        {
            var startTimes = newItems.Select(d => d.items.Min(x => x.start)).ToList();
            var min = startTimes.Min();
            startTimes = startTimes.Select(x => x - min + time).ToList();

            for (int i = 0; i < newItems.Count(); ++i)
                EditModeUtils.SetStart(newItems.ElementAt(i).items, startTimes[i]);
        }

        public static double TimeGapBetweenItems(ITimelineItem leftItem, ITimelineItem rightItem, WindowState state)
        {
            if (leftItem is MarkerItem && rightItem is MarkerItem)
            {
                var markerType = ((MarkerItem)leftItem).marker.GetType();
                var gap = state.PixelDeltaToDeltaTime(StyleManager.UssStyleForType(markerType).fixedWidth);
                return gap;
            }

            return 0.0;
        }
    }
}
