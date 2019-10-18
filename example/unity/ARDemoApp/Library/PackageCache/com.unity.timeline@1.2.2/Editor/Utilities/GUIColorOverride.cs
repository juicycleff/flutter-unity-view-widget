using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    struct GUIColorOverride : IDisposable
    {
        readonly Color m_OldColor;

        public GUIColorOverride(Color newColor)
        {
            m_OldColor = GUI.color;
            GUI.color = newColor;
        }

        public void Dispose()
        {
            GUI.color = m_OldColor;
        }
    }
}
