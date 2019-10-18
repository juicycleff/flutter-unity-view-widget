using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{
    interface IMoveItemMode
    {
        void OnTrackDetach(IEnumerable<ItemsPerTrack> itemsGroups);
        void HandleTrackSwitch(IEnumerable<ItemsPerTrack> itemsGroups);
        bool AllowTrackSwitch();

        double AdjustStartTime(WindowState state, ItemsPerTrack itemsGroup, double time);

        void OnModeClutchEnter(IEnumerable<ItemsPerTrack> itemsGroups);
        void OnModeClutchExit(IEnumerable<ItemsPerTrack> itemsGroups);

        void BeginMove(IEnumerable<ItemsPerTrack> itemsGroups);
        void UpdateMove(IEnumerable<ItemsPerTrack> itemsGroups);
        void FinishMove(IEnumerable<ItemsPerTrack> itemsGroups);

        bool ValidateMove(ItemsPerTrack itemsGroup);
    }

    interface IMoveItemDrawer
    {
        void DrawGUI(WindowState state, IEnumerable<MovingItems> movingItems, Color color);
    }
}
