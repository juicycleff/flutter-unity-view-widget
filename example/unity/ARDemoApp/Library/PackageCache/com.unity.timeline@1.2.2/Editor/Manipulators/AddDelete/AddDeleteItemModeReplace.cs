using System.Collections.Generic;

namespace UnityEditor.Timeline
{
    class AddDeleteItemModeReplace : IAddDeleteItemMode
    {
        public void InsertItemsAtTime(IEnumerable<ItemsPerTrack> itemsGroups, double requestedTime)
        {
            ItemsUtils.SetItemsStartTime(itemsGroups, requestedTime);
            EditModeReplaceUtils.Insert(itemsGroups);
        }

        public void RemoveItems(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }
    }
}
