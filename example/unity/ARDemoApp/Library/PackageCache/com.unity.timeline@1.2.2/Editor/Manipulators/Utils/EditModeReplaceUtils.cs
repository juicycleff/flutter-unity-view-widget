using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class EditModeReplaceUtils
    {
        public static void Insert(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            foreach (var itemsGroup in itemsGroups)
            {
                Insert(itemsGroup.targetTrack, itemsGroup.items);
            }
        }

        static void Insert(TrackAsset track, IEnumerable<ITimelineItem> items)
        {
            if (track == null) return;
            var orderedItems = ItemsUtils.GetItemsExcept(track, items)
                .OfType<ITrimmable>()
                .OrderBy(i => i.start).ToArray();

            foreach (var item in items.OfType<ITrimmable>())
            {
                var from = item.start;
                var to = item.end;

                var overlappedItems = orderedItems.Where(i => EditModeUtils.Overlaps(i, from, to));

                foreach (var overlappedItem in overlappedItems)
                {
                    if (EditModeUtils.IsItemWithinRange(overlappedItem, from, to))
                    {
                        overlappedItem.Delete();
                    }
                    else
                    {
                        if (overlappedItem.start >= from)
                            overlappedItem.TrimStart(to);
                        else
                            overlappedItem.TrimEnd(from);
                    }
                }

                var includingItems = orderedItems.Where(c => c.start<from && c.end> to);
                foreach (var includingItem in includingItems)
                {
                    var newItem = includingItem.CloneTo(track, includingItem.start) as ITrimmable;
                    includingItem.TrimStart(to);
                    if (newItem != null)
                        newItem.SetEnd(from, false);
                }
            }
        }
    }
}
