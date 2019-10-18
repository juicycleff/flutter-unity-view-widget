using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Timeline
{
    class AddDeleteItemModeMix : IAddDeleteItemMode
    {
        public void InsertItemsAtTime(IEnumerable<ItemsPerTrack> itemsGroups, double requestedTime)
        {
            ItemsUtils.SetItemsStartTime(itemsGroups, requestedTime);
            EditModeMixUtils.PrepareItemsForInsertion(itemsGroups);

            if (!EditModeMixUtils.CanInsert(itemsGroups))
            {
                var validTime = itemsGroups.Select(c => c.targetTrack).Max(parent => parent.duration);
                ItemsUtils.SetItemsStartTime(itemsGroups, validTime);
            }
        }

        public void RemoveItems(IEnumerable<ItemsPerTrack> itemsGroups)
        {
            // Nothing
        }
    }
}
