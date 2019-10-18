using UnityEngine;

namespace UnityEditor.Timeline
{
    class RectangleZoom : RectangleTool
    {
        protected override bool enableAutoPan { get { return true; } }

        protected override bool CanStartRectangle(Event evt, Vector2 mousePosition, WindowState state)
        {
            return evt.button == 1 && evt.modifiers == (EventModifiers.Alt | EventModifiers.Shift);
        }

        protected override bool OnFinish(Event evt, WindowState state, Rect rect)
        {
            var x = state.PixelToTime(rect.xMin);
            var y = state.PixelToTime(rect.xMax);
            state.SetTimeAreaShownRange(x, y);

            return true;
        }
    }
}
