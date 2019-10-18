using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class RectangleSelect : RectangleTool
    {
        protected override bool enableAutoPan { get { return false; } }

        protected override bool CanStartRectangle(Event evt, Vector2 mousePosition, WindowState state)
        {
            if (evt.button != 0 || evt.alt)
                return false;

            return PickerUtils.pickedElements.All(e => e is IRowGUI);
        }

        protected override bool OnFinish(Event evt, WindowState state, Rect rect)
        {
            var selectables = state.spacePartitioner.GetItemsInArea<ISelectable>(rect).ToList();

            if (!selectables.Any())
                return false;

            if (ItemSelection.CanClearSelection(evt))
                SelectionManager.Clear();

            foreach (var selectable in selectables)
            {
                ItemSelection.HandleItemSelection(evt, selectable);
            }

            return true;
        }
    }
}
