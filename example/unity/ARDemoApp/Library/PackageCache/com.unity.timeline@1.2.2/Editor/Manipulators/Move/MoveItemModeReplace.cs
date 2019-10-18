using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class MoveItemModeReplace : IMoveItemMode, IMoveItemDrawer
    {
        public void OnTrackDetach(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void HandleTrackSwitch(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
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
            // TODO
        }

        public void OnModeClutchExit(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // TODO
        }

        public void BeginMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            foreach (var itemsGroup in itemsGroups)
            {
                EditModeUtils.SetParentTrack(itemsGroup.items, null);
            }
        }

        public void UpdateMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }

        public void FinishMove(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            EditModeReplaceUtils.Insert(itemsGroups);
        }

        public bool ValidateMove(ItemsPerTrack itemsGroup)
        {
            return true;
        }

        public void DrawGUI(WindowState state, IEnumerable<MovingItems> movingItems, Color color)
        {
            var operationWillReplace = false;

            foreach (var itemsPerTrack in movingItems)
            {
                var bounds = itemsPerTrack.onTrackItemsBounds;

                var counter = 0;
                foreach (var item in itemsPerTrack.items)
                {
                    if (EditModeUtils.GetFirstIntersectedItem(itemsPerTrack.items, item.start) != null)
                    {
                        EditModeGUIUtils.DrawBoundsEdge(bounds[counter], color, TrimEdge.Start);
                        operationWillReplace = true;
                    }

                    if (EditModeUtils.GetFirstIntersectedItem(itemsPerTrack.items, item.end) != null)
                    {
                        EditModeGUIUtils.DrawBoundsEdge(bounds[counter], color, TrimEdge.End);
                        operationWillReplace = true;
                    }

                    counter++;
                    // TODO Display swallowed clips?
                }
            }

            if (operationWillReplace)
            {
                TimelineCursors.SetCursor(TimelineCursors.CursorType.Replace);
            }
            else
            {
                TimelineCursors.ClearCursor();
            }
        }
    }
}
