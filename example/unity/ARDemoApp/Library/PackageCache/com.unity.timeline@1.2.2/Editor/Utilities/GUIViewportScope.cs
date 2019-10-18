using System;
using UnityEngine;

namespace UnityEditor
{
    // Special Clip Scope that only effects painting, and keeps the coordinate system identical
    struct GUIViewportScope : IDisposable
    {
        bool m_open;
        public GUIViewportScope(Rect position)
        {
            m_open = false;
            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                GUI.BeginClip(position, -position.min, Vector2.zero, false);
                m_open = true;
            }
        }

        public void Dispose()
        {
            CloseScope();
        }

        void CloseScope()
        {
            if (m_open)
            {
                GUI.EndClip();
                m_open = false;
            }
        }
    }
}
