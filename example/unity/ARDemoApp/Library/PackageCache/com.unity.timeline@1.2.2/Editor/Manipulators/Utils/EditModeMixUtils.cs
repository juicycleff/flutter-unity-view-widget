using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class EditModeMixUtils
    {
        static readonly List<PlacementValidity> k_UnrecoverablePlacements = new List<PlacementValidity>
        {
            PlacementValidity.InvalidIsWithin,
            PlacementValidity.InvalidStartsInBlend,
            PlacementValidity.InvalidContainsBlend
        };

        public static bool CanInsert(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            foreach (var itemsGroup in itemsGroups)
            {
                var siblings = ItemsUtils.GetItemsExcept(itemsGroup.targetTrack, itemsGroup.items);
                foreach (var item in itemsGroup.items)
                {
                    var placementValidity = GetPlacementValidity(item, siblings);

                    if (k_UnrecoverablePlacements.Contains(placementValidity))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Corrects clips durations to fit at insertion point, if needed
        public static void PrepareItemsForInsertion(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            foreach (var itemsGroup in itemsGroups)
            {
                var siblings = ItemsUtils.GetItemsExcept(itemsGroup.targetTrack, itemsGroup.items);
                foreach (var item in itemsGroup.items.OfType<ITrimmable>())
                {
                    var eatenItems = siblings.Where(c => EditModeUtils.IsItemWithinRange(c, item.start, item.end)).ToList();

                    var intersectedItem = EditModeUtils.GetFirstIntersectedItem(siblings, item.end);
                    if (intersectedItem != null)
                        eatenItems.Add(intersectedItem);

                    var blendableItems = eatenItems.OfType<IBlendable>();
                    if (blendableItems.Any())
                    {
                        var minTime = blendableItems.Min(c => c.end - c.rightBlendDuration);

                        if (item.end > minTime)
                            item.SetEnd(minTime, false);
                    }
                }
            }
        }

        public static PlacementValidity GetPlacementValidity(ITimelineItem item, IEnumerable<ITimelineItem> otherItems)
        {
            if (item.duration <= 0.0)
                return PlacementValidity.Valid;  //items without any duration can always be placed

            var sortedItems = otherItems.Where(i => i.duration > 0.0).OrderBy(c => c.start);
            var candidates = new List<ITimelineItem>();
            foreach (var sortedItem in sortedItems)
            {
                if ((DiscreteTime)sortedItem.start >= (DiscreteTime)item.end)
                {
                    // No need to process further
                    break;
                }

                if ((DiscreteTime)sortedItem.end <= (DiscreteTime)item.start)
                {
                    // Skip
                    continue;
                }

                candidates.Add(sortedItem);
            }

            var discreteStart = (DiscreteTime)item.start;
            var discreteEnd = (DiscreteTime)item.end;

            // Note: Order of tests matters
            for (int i = 0, n = candidates.Count; i < n; i++)
            {
                var candidate = candidates[i];

                var blendItem = item as IBlendable;
                if (blendItem != null && blendItem.supportsBlending)
                {
                    if (EditModeUtils.Contains(candidate.start, candidate.end, item))
                        return PlacementValidity.InvalidIsWithin;

                    if (i < n - 1)
                    {
                        var nextCandidate = candidates[i + 1];

                        var discreteNextCandidateStart = (DiscreteTime)nextCandidate.start;
                        var discreteCandidateEnd = (DiscreteTime)candidate.end;

                        if (discreteCandidateEnd > discreteNextCandidateStart)
                        {
                            if (discreteStart >= discreteNextCandidateStart)
                            {
                                // Note: In case the placement is fully within a blend,
                                // InvalidStartsInBlend MUST have priority
                                return PlacementValidity.InvalidStartsInBlend;
                            }

                            if (discreteEnd > discreteNextCandidateStart && discreteEnd <= discreteCandidateEnd)
                                return PlacementValidity.InvalidEndsInBlend;

                            if (discreteStart < discreteNextCandidateStart && discreteEnd > discreteCandidateEnd)
                                return PlacementValidity.InvalidContainsBlend;
                        }
                    }

                    if (EditModeUtils.Contains(item.start, item.end, candidate))
                        return PlacementValidity.InvalidContains;
                }
                else
                {
                    if (EditModeUtils.Overlaps(item, candidate.start, candidate.end)
                        || EditModeUtils.Overlaps(candidate, item.start, item.end))
                        return PlacementValidity.InvalidOverlapWithNonBlendableClip;
                }
            }

            return PlacementValidity.Valid;
        }
    }
}
