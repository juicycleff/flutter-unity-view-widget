using UnityEngine;

namespace UnityEditor.Timeline
{
    static class EditModeGUIUtils
    {
        public static void DrawBoundsEdge(Rect bounds, Color color, TrimEdge edge, float width = 4.0f)
        {
            var r = bounds;
            r.yMin += 2.0f;
            r.yMax -= 2.0f;
            r.width = width;

            r.x = edge == TrimEdge.End ? bounds.xMax : bounds.xMin - width;

            EditorGUI.DrawRect(r, color);
        }

        public static void DrawOverlayRect(Rect bounds, Color overlayColor)
        {
            var c = overlayColor;
            c.a = 0.2f;
            EditorGUI.DrawRect(bounds, c);
            EditorGUI.DrawOutline(bounds, 1.0f, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }
    }
}
