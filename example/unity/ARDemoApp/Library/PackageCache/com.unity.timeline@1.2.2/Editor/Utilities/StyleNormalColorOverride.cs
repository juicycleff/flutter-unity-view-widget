using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class StyleNormalColorOverride : IDisposable
    {
        readonly GUIStyle m_Style;
        readonly Color m_OldColor;

        public StyleNormalColorOverride(GUIStyle style, Color newColor)
        {
            m_Style = style;
            m_OldColor = style.normal.textColor;
            style.normal.textColor = newColor;
        }

        public void Dispose()
        {
            m_Style.normal.textColor = m_OldColor;
        }
    }
}
