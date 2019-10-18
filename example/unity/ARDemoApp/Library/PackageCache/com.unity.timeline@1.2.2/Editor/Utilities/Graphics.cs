using UnityEngine;

namespace UnityEditor.Timeline
{
    static class Graphics
    {
        public static bool ShadowedButton(Rect rect, string text, GUIStyle style, Color shadowColor)
        {
            var shadowRect = rect;
            shadowRect.xMin += 2.0f;
            shadowRect.yMin += 2.0f;
            style.normal.textColor = shadowColor;
            GUI.Label(shadowRect, text, style);

            return GUI.Button(rect, text, style);
        }

        public static void ShadowLabel(Rect rect, string text, GUIStyle style, Color textColor, Color shadowColor)
        {
            ShadowLabel(rect, GUIContent.Temp(text), style, textColor, shadowColor);
        }

        public static void ShadowLabel(Rect rect, GUIContent content, GUIStyle style, Color textColor, Color shadowColor)
        {
            var shadowRect = rect;
            shadowRect.xMin += 2.0f;
            shadowRect.yMin += 2.0f;
            style.normal.textColor = Color.black;
            GUI.Label(shadowRect, content, style);

            style.normal.textColor = textColor;
            GUI.Label(rect, content, style);
        }

        public static void DrawOutlineRect(Rect rect, Color color)
        {
            DrawLine(new Vector3(rect.x, rect.y, 0.0f), new Vector3(rect.x + rect.width, rect.y, 0.0f), color);
            DrawLine(new Vector3(rect.x + rect.width, rect.y, 0.0f), new Vector3(rect.x + rect.width, rect.y + rect.height, 0.0f), color);
            DrawLine(new Vector3(rect.x, rect.y, 0.0f), new Vector3(rect.x, rect.y + rect.height, 0.0f), color);
            DrawLine(new Vector3(rect.x, rect.y + rect.height, 0.0f), new Vector3(rect.x + rect.width, rect.y + rect.height, 0.0f), color);
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            var c = Handles.color;
            Handles.color = color;
            Handles.DrawLine(p1, p2);
            Handles.color = c;
        }

        public static void DrawLineAA(Vector3 p1, Vector3 p2, Color col)
        {
            var handleColor = Handles.color;
            Handles.color = col;
            Handles.DrawAAPolyLine(1.0f, p1, p2);
            Handles.color = handleColor;
        }

        public static void DrawLineAA(float width, Vector3 p1, Vector3 p2, Color color)
        {
            DrawAAPolyLine(width, p1, p2, color);
        }

        private static readonly Vector3[] s_AAPoints = new Vector3[2];
        public static void DrawAAPolyLine(float width, Vector3 p0, Vector3 p1, Color color)
        {
            s_AAPoints[0] = p0;
            s_AAPoints[1] = p1;

            var prevColor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(width, s_AAPoints);
            Handles.color = prevColor;
        }

        public static void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength, Color col)
        {
            HandleUtility.ApplyWireMaterial();

            GL.Begin(GL.LINES);
            GL.Color(col);

            var length = Vector3.Distance(p1, p2); // ignore z component
            var count = Mathf.CeilToInt(length / segmentsLength);
            for (var i = 0; i < count; i += 2)
            {
                GL.Vertex((Vector3.Lerp(p1, p2, i * segmentsLength / length)));
                GL.Vertex((Vector3.Lerp(p1, p2, (i + 1) * segmentsLength / length)));
            }

            GL.End();
        }

        public static void DrawLineAtTime(WindowState state, double time, Color color, bool dotted = false)
        {
            var t = state.TimeToPixel(time);

            var p0 = new Vector3(t, state.timeAreaRect.yMax);
            var p1 = new Vector3(t, state.timeAreaRect.yMax + state.windowHeight - WindowConstants.sliderWidth);

            if (dotted)
                DrawDottedLine(p0, p1, 4.0f, color);
            else
                DrawLine(p0, p1, color);
        }

        public static void DrawTextureRepeated(Rect area, Texture texture)
        {
            if (texture == null || Event.current.type != EventType.Repaint)
                return;

            GUI.BeginClip(area);
            int w = Mathf.CeilToInt(area.width / texture.width);
            int h = Mathf.CeilToInt(area.height / texture.height);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    GUI.DrawTexture(new Rect(x * texture.width, y * texture.height, texture.width, texture.height), texture);
                }
            }
            GUI.EndClip();
        }

        public static void DrawShadow(Rect clientRect)
        {
            var rect = clientRect;
            rect.height = WindowConstants.shadowUnderTimelineHeight;
            GUI.Box(rect, GUIContent.none, DirectorStyles.Instance.bottomShadow);
        }

        public static void DrawBackgroundRect(WindowState state, Rect rect, bool subSequenceMode = false)
        {
            Color c = subSequenceMode ? DirectorStyles.Instance.customSkin.colorSubSequenceBackground : DirectorStyles.Instance.customSkin.colorSequenceBackground;
            EditorGUI.DrawRect(rect, c);
            if (state.IsEditingAPrefabAsset())
            {
                c = SceneView.kSceneViewPrefabBackground.Color;
                c.a = 0.5f;
                EditorGUI.DrawRect(rect, c);
            }
        }
    }
}
