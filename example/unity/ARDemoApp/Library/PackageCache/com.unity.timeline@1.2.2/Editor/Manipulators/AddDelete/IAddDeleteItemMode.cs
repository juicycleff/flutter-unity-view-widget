using System.Collections.Generic;

namespace UnityEditor.Timeline
{
    interface IAddDeleteItemMode
    {
        void InsertItemsAtTime(IEnumerable<ItemsPerTrack> itemsGroups, double requestedTime);
        void RemoveItems(IEnumerable<ItemsPerTrack> itemsGroups);
    }
}
