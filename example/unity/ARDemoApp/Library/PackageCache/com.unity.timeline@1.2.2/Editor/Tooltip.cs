using UnityEngine;

namespace UnityEditor.Timeline
{
    class Tooltip
    {
        public GUIStyle style { get; set; }

        public string text { get; set; }

        GUIStyle m_Font;

        public GUIStyle font
        {
            get
            {
                if (m_Font != null)
                    return m_Font;

                if (style != null)
                    return style;

                // Default Font.
                m_Font = new GUIStyle();
                m_Font.font = EditorStyles.label.font;

                return m_Font;
            }
            set { m_Font = value; }
        }

        float m_Pad = 4.0f;

        public float pad
        {
            get { return m_Pad; }
            set { m_Pad = value; }
        }

        GUIContent m_TextContent;

        GUIContent textContent
        {
            get
            {
                if (m_TextContent == null)
                    m_TextContent = new GUIContent();

                m_TextContent.text = text;

                return m_TextContent;
            }
        }

        Color m_ForeColor = Color.white;

        public Color foreColor
        {
            get { return m_ForeColor; }
            set { m_ForeColor = value; }
        }

        Rect m_Bounds;

        public Rect bounds
        {
            get
            {
                var size = font.CalcSize(textContent);
                m_Bounds.width = size.x + (2.0f * pad);
                m_Bounds.height = size.y + 2.0f;

                return m_Bounds;
            }

            set { m_Bounds = value; }
        }

        public Tooltip(GUIStyle theStyle, GUIStyle font)
        {
            style = theStyle;
            m_Font = font;
        }

        public Tooltip()
        {
            style = null;
            m_Font = null;
        }

        public void Draw()
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (style != null)
            {
                using (new GUIColorOverride(DirectorStyles.Instance.customSkin.colorTooltipBackground))
                    GUI.Label(bounds, GUIContent.none, style);
            }

            var textBounds = bounds;
            textBounds.x += pad;
            textBounds.width -= pad;

            using (new GUIColorOverride(foreColor))
                GUI.Label(textBounds, textContent, font);
        }
    }
}
