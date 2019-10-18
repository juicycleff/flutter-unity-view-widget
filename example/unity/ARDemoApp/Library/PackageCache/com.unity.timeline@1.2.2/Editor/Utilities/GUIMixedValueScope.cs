using System;
using UnityEngine;

namespace UnityEditor
{
    struct GUIMixedValueScope : IDisposable
    {
        readonly bool m_PrevValue;
        public GUIMixedValueScope(bool newValue)
        {
            m_PrevValue = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = newValue;
        }

        public void Dispose()
        {
            EditorGUI.showMixedValue = m_PrevValue;
        }
    }
}
