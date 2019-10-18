using System.Collections.Generic;

namespace UnityEditor.Timeline
{
    class AddDeleteItemModeRipple : IAddDeleteItemMode
    {
        public void InsertItemsAtTime(IEnumerable<ItemsPerTrack> itemsGroups, double requestedTime)
        {
            ItemsUtils.SetItemsStartTime(itemsGroups, requestedTime);
            EditModeRippleUtils.Insert(itemsGroups);
        }

        public void RemoveItems(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            EditModeRippleUtils.Remove(itemsGroups);
        }
    }
}
