using UnityEngine;

namespace UnityEditor.Timeline
{
    static class TimeIndicator
    {
        static readonly Tooltip s_Tooltip = new Tooltip(DirectorStyles.Instance.displayBackground, DirectorStyles.Instance.tinyFont);

        public static void Draw(WindowState state, double time)
        {
            var bounds = state.timeAreaRect;
            bounds.xMin = Mathf.Max(bounds.xMin, state.TimeToTimeAreaPixel(time));

            using (new GUIViewportScope(state.timeAreaRect))
            {
                s_Tooltip.text = TimeReferenceUtility.ToTimeString(time);

                var tooltipBounds = s_Tooltip.bounds;
                tooltipBounds.xMin = bounds.xMin - (tooltipBounds.width / 2.0f);
                tooltipBounds.y = bounds.y;
                s_Tooltip.bounds = tooltipBounds;

                if (time >= 0)
                    s_Tooltip.Draw();
            }

            if (time >= 0)
            {
                Graphics.DrawLineAtTime(state, time, Color.black, true);
            }
        }

        public static void Draw(WindowState state, double start, double end)
        {
            var bounds = state.timeAreaRect;
            bounds.xMin = Mathf.Max(bounds.xMin, state.TimeToTimeAreaPixel(start));
            bounds.xMax = Mathf.Min(bounds.xMax, state.TimeToTimeAreaPixel(end));

            var color = DirectorStyles.Instance.selectedStyle.focused.textColor;
            color.a = 0.12f;
            EditorGUI.DrawRect(bounds, color);

            Draw(state, start);
            Draw(state, end);
        }
    }
}
