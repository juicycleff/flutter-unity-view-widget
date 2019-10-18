using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class MoveItemModeRipple : IMoveItemMode, IMoveItemDrawer
    {
        const float k_SnapToEdgeDistance = 30.0f;

        class PrevItemInfo
        {
            public ITimelineItem item;
            public ITimelineItem firstSelectedItem;
            public bool blending;

            public PrevItemInfo(ITimelineItem item, ITimelineItem firstSelectedItem)
            {
                this.item = item;
                this.firstSelectedItem = firstSelectedItem;
                blending = item != null && item.end > firstSelectedItem.start;
            }
        }

        readonly Dictionary<Object, List<ITimelineItem>> m_NextItems = new Dictionary<Object, List<ITimelineItem>>();
        readonly Dictionary<Object, PrevItemInfo> m_PreviousItem = new Dictionary<Object, PrevItemInfo>();
        double m_PreviousEnd;

        bool m_TrackLocked;
        bool m_Detached;

        public void OnTrackDetach(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            if (m_TrackLocked)
                return;

            if (m_Detached)
                return;

            if (itemsGroups.Any(x => x.markers.Any()))
                return;

            // Ripple can either remove or not clips when detaching them from their track.
            // Keep it off for now. TODO: add clutch key to toggle this feature?
            //EditModeRippleUtils.Remove(manipulatedClipsList);

            StartDetachMode(itemsGroups);
        }

        public void HandleTrackSwitch(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public bool AllowTrackSwitch()
        {
            return !m_TrackLocked;
        }

        public double AdjustStartTime(WindowState state, ItemsPerTrack itemsGroup, double time)
        {
            var track = itemsGroup.targetTrack;
            if (track == null)
                return time;

            double start;
            double end;

            if (EditModeUtils.IsInfiniteTrack(track))
            {
                EditModeUtils.GetInfiniteClipBoundaries(track, out start, out end);
            }
            else
            {
                var siblings = ItemsUtils.GetItemsExcept(track, itemsGroup.items);
                var firstIntersectedItem = EditModeUtils.GetFirstIntersectedItem(siblings, time);

                if (firstIntersectedItem == null)
                    return time;

                start = firstIntersectedItem.start;
                end = firstIntersectedItem.end;
            }

            var closestTime = Math.Abs(time - start) < Math.Abs(time - end) ? start : end;

            var pixelTime = state.TimeToPixel(time);
            var pixelClosestTime = state.TimeToPixel(closestTime);

            if (Math.Abs(pixelTime - pixelClosestTime) < k_SnapToEdgeDistance)
                return closestTime;

            return time;
        }

        void StartDetachMode(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            m_Detached = true;

            foreach (var itemsGroup in itemsGroups)
                EditModeUtils.SetParentTrack(itemsGroup.items, null);
        }

        public void OnModeClutchEnter(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            StartDetachMode(itemsGroups);
            m_TrackLocked = false;
        }

        public void OnModeClutchExit(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            m_Detached = false;
            m_TrackLocked = false;
        }

        public void BeginMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            m_NextItems.Clear();
            m_PreviousItem.Clear();
            var itemTypes = ItemsUtils.GetItemTypes(itemsGroups).ToList();

            foreach (var itemsGroup in itemsGroups)
            {
                //can only ripple items of the same type as those selected
                var sortedSelectedItems = itemsGroup.items.OrderBy(i => i.start).ToList();
                var siblings = itemsGroup.targetTrack.GetItemsExcept(itemsGroup.items);
                var sortedSiblingsToRipple = siblings.Where(i => itemTypes.Contains(i.GetType())).OrderBy(i => i.start).ToList();
                var start = sortedSelectedItems.First().start;

                m_NextItems.Add(itemsGroup.targetTrack, sortedSiblingsToRipple.Where(i => i.start > start).ToList());
                m_PreviousItem.Add(itemsGroup.targetTrack, CalculatePrevItemInfo(sortedSelectedItems, sortedSiblingsToRipple, itemTypes));
            }

            m_PreviousEnd = itemsGroups.Max(m => m.items.Max(c => c.end));
        }

        public void UpdateMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            if (m_Detached)
                return;

            m_TrackLocked = true;

            var overlap = 0.0;
            foreach (var itemsGroup in itemsGroups)
            {
                var track = itemsGroup.targetTrack;
                if (track == null) continue;

                var prevItemInfo = m_PreviousItem[track];
                if (prevItemInfo.item != null)
                {
                    var prevItem = prevItemInfo.item;
                    var firstItem = prevItemInfo.firstSelectedItem;

                    if (prevItemInfo.blending)
                        prevItemInfo.blending = prevItem.end > firstItem.start;

                    if (prevItemInfo.blending)
                    {
                        var b = EditModeUtils.BlendDuration(firstItem, TrimEdge.End);
                        overlap = Math.Max(overlap, Math.Max(prevItem.start, prevItem.end - firstItem.end + firstItem.start + b) - firstItem.start);
                    }
                    else
                    {
                        overlap = Math.Max(overlap, prevItem.end - firstItem.start);
                    }
                }
            }

            if (overlap > 0)
            {
                foreach (var itemsGroup in itemsGroups)
                {
                    foreach (var item in itemsGroup.items)
                        item.start += overlap;
                }
            }

            var newEnd = itemsGroups.Max(m => m.items.Max(c => c.end));

            var offset = newEnd - m_PreviousEnd;
            m_PreviousEnd = newEnd;

            foreach (var itemsGroup in itemsGroups)
            {
                foreach (var item in m_NextItems[itemsGroup.targetTrack])
                    item.start += offset;
            }
        }

        static PrevItemInfo CalculatePrevItemInfo(List<ITimelineItem> orderedSelection, List<ITimelineItem> orderedSiblings, IEnumerable<Type> itemTypes)
        {
            ITimelineItem previousItem = null;
            ITimelineItem firstSelectedItem = null;
            var gap = double.PositiveInfinity;

            foreach (var type in itemTypes)
            {
                var firstSelectedItemOfType = orderedSelection.FirstOrDefault(i => i.GetType() == type);
                if (firstSelectedItemOfType == null) continue;

                var previousItemOfType = orderedSiblings.LastOrDefault(i => i.GetType() == type && i.start < firstSelectedItemOfType.start);
                if (previousItemOfType == null) continue;

                var currentGap = firstSelectedItemOfType.start - previousItemOfType.end;
                if (currentGap < gap)
                {
                    gap = currentGap;
                    firstSelectedItem = firstSelectedItemOfType;
                    previousItem = previousItemOfType;
                }
            }

            return new PrevItemInfo(previousItem, firstSelectedItem);
        }

        public bool ValidateMove(ItemsPerTrack itemsGroup)
        {
            return true;
        }

        public void FinishMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            if (m_Detached)
                EditModeRippleUtils.Insert(itemsGroups);

            m_Detached = false;
            m_TrackLocked = false;
        }

        public void DrawGUI(WindowState state, IEnumerable<MovingItems> movingItems, Color color)
        {
            if (m_Detached)
            {
                var xMin = float.MaxValue;
                var xMax = float.MinValue;

                foreach (var grabbedItems in movingItems)
                {
                    xMin = Math.Min(xMin, grabbedItems.onTrackItemsBounds.Min(b => b.xMin)); // TODO Cache this?
                    xMax = Math.Max(xMax, grabbedItems.onTrackItemsBounds.Max(b => b.xMax));
                }

                foreach (var grabbedItems in movingItems)
                {
                    var bounds = Rect.MinMaxRect(xMin, grabbedItems.onTrackItemsBounds[0].yMin,
                        xMax, grabbedItems.onTrackItemsBounds[0].yMax);

                    EditModeGUIUtils.DrawOverlayRect(bounds, new Color(1.0f, 1.0f, 1.0f, 0.5f));

                    EditModeGUIUtils.DrawBoundsEdge(bounds, color, TrimEdge.Start);
                }
            }
            else
            {
                foreach (var grabbedItems in movingItems)
                {
                    var bounds = Rect.MinMaxRect(grabbedItems.onTrackItemsBounds.Min(b => b.xMin), grabbedItems.onTrackItemsBounds[0].yMin,
                        grabbedItems.onTrackItemsBounds.Max(b => b.xMax), grabbedItems.onTrackItemsBounds[0].yMax);

                    EditModeGUIUtils.DrawBoundsEdge(bounds, color, TrimEdge.Start);
                }
            }

            TimelineCursors.SetCursor(TimelineCursors.CursorType.Ripple);
        }
    }
}
