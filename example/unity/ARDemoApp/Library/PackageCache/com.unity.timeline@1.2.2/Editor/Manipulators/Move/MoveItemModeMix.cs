using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class MoveItemModeMix : IMoveItemMode, IMoveItemDrawer
    {
        public void OnTrackDetach(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void HandleTrackSwitch(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            foreach (var itemsGroup in itemsGroups)
            {
                var targetTrack = itemsGroup.targetTrack;
                if (targetTrack != null && itemsGroup.items.Any())
                {
                    var compatible = itemsGroup.items.First().IsCompatibleWithTrack(targetTrack) &&
                        !EditModeUtils.IsInfiniteTrack(targetTrack);
                    var track = compatible ? targetTrack : null;

                    if (track != null)
                        TimelineUndo.PushUndo(track, "Move Items");

                    EditModeUtils.SetParentTrack(itemsGroup.items, track);
                }
                else
                {
                    EditModeUtils.SetParentTrack(itemsGroup.items, null);
                }
            }
        }

        public bool AllowTrackSwitch()
        {
            return true;
        }

        public double AdjustStartTime(WindowState state, ItemsPerTrack itemsGroup, double time)
        {
            return time;
        }

        public void OnModeClutchEnter(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void OnModeClutchExit(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void BeginMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void UpdateMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void FinishMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public bool ValidateMove(ItemsPerTrack itemsGroup)
        {
            var track = itemsGroup.targetTrack;
            var items = itemsGroup.items;

            if (EditModeUtils.IsInfiniteTrack(track))
            {
                double startTime;
                double stopTime;
                EditModeUtils.GetInfiniteClipBoundaries(track, out startTime, out stopTime);

                return items.All(item =>
                    !EditModeUtils.IsItemWithinRange(item, startTime, stopTime) &&
                    !EditModeUtils.IsRangeWithinItem(startTime, stopTime, item));
            }

            var siblings = ItemsUtils.GetItemsExcept(itemsGroup.targetTrack, items);
            return items.All(item => EditModeMixUtils.GetPlacementValidity(item, siblings) == PlacementValidity.Valid);
        }

        public void DrawGUI(WindowState state, IEnumerable<MovingItems> movingItems, Color color)
        {
            var selectionHasAnyBlendIn = false;
            var selectionHasAnyBlendOut = false;

            foreach (var grabbedItems in movingItems)
            {
                var bounds = grabbedItems.onTrackItemsBounds;

                var counter = 0;
                foreach (var item in grabbedItems.items.OfType<IBlendable>())
                {
                    if (item.hasLeftBlend)
                    {
                        EditModeGUIUtils.DrawBoundsEdge(bounds[counter], color, TrimEdge.Start);
                        selectionHasAnyBlendIn = true;
                    }

                    if (item.hasRightBlend)
                    {
                        EditModeGUIUtils.DrawBoundsEdge(bounds[counter], color, TrimEdge.End);
                        selectionHasAnyBlendOut = true;
                    }
                    counter++;
                }
            }

            if (selectionHasAnyBlendIn && selectionHasAnyBlendOut)
            {
                TimelineCursors.SetCursor(TimelineCursors.CursorType.MixBoth);
            }
            else if (selectionHasAnyBlendIn)
            {
                TimelineCursors.SetCursor(TimelineCursors.CursorType.MixLeft);
            }
            else if (selectionHasAnyBlendOut)
            {
                TimelineCursors.SetCursor(TimelineCursors.CursorType.MixRight);
            }
            else
            {
                TimelineCursors.ClearCursor();
            }
        }
    }
}
