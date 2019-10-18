using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class EditModeRippleUtils
    {
        public static void Insert(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            var start = double.MaxValue;
            var end = double.MinValue;

            foreach (var itemsGroup in itemsGroups)
            {
                start = Math.Min(start, itemsGroup.items.Min(c => c.start));
                end = Math.Max(end, itemsGroup.items.Max(c => c.end));
            }

            var offset = 0.0;
            var discreteStart = (DiscreteTime)start;
            var discreteEnd = (DiscreteTime)end;
            var itemTypes = ItemsUtils.GetItemTypes(itemsGroups);
            var siblingsToRipple = new List<ITimelineItem>();

            foreach (var itemsGroup in itemsGroups)
            {
                //can only ripple items of the same type as those selected
                siblingsToRipple.AddRange(ItemsUtils.GetItemsExcept(itemsGroup.targetTrack, itemsGroup.items).Where(i => itemTypes.Contains(i.GetType())));
                foreach (var item in siblingsToRipple)
                {
                    var discreteItemStart = (DiscreteTime)item.start;
                    var discreteItemEnd = (DiscreteTime)item.end;

                    if ((discreteItemStart < discreteStart && discreteItemEnd > discreteStart) || (discreteItemStart >= discreteStart && discreteItemStart < discreteEnd))
                        offset = Math.Max(offset, end - item.start);
                }
            }

            if (offset > 0.0)
            {
                foreach (var sibling in siblingsToRipple)
                {
                    if ((DiscreteTime)sibling.end > (DiscreteTime)start)
                        sibling.start += offset;
                }
            }
        }

        public static void Remove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            foreach (var itemsGroup in itemsGroups)
                Remove(itemsGroup.targetTrack, itemsGroup.items);
        }

        static void Remove(TrackAsset track, IEnumerable<ITimelineItem> items)
        {
            if (track == null) return;

            //can only ripple items of the same type as those selected
            var itemTypes = ItemsUtils.GetItemTypes(items);
            var siblingsToRipple = ItemsUtils.GetItemsExcept(track, items)
                .Where(i => itemTypes.Contains(i.GetType()))
                .OrderBy(c => c.start)
                .ToArray();

            var orderedItems = items
                .OrderBy(c => c.start)
                .ToArray();

            var cumulativeOffset = 0.0;

            foreach (var item in orderedItems)
            {
                var offset = item.end - item.start;
                var start = item.start - cumulativeOffset;
                var end = item.end - cumulativeOffset;

                var nextItem = siblingsToRipple.FirstOrDefault(c => (DiscreteTime)c.start > (DiscreteTime)start && (DiscreteTime)c.start < (DiscreteTime)end);
                if (nextItem != null)
                {
                    offset -= end - nextItem.start;
                }

                var prevItem = siblingsToRipple.FirstOrDefault(c => (DiscreteTime)c.end > (DiscreteTime)start && (DiscreteTime)c.end < (DiscreteTime)end);
                if (prevItem != null)
                {
                    offset -= prevItem.end - start;
                }

                if (offset <= 0.0)
                    continue;

                cumulativeOffset += offset;

                for (int i = siblingsToRipple.Length - 1; i >= 0; --i)
                {
                    var c = siblingsToRipple[i];
                    if ((DiscreteTime)c.start < (DiscreteTime)start)
                        break;

                    c.start = c.start - offset;
                }
            }
        }
    }
}
