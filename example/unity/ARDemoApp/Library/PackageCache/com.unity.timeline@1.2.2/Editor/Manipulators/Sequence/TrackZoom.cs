using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TrackZoom : Manipulator
    {
        // only handles 'vertical' zoom. horizontal is handled in timelineGUI
        protected override bool MouseWheel(Event evt, WindowState state)
        {
            if (EditorGUI.actionKey)
            {
                state.trackScale = Mathf.Min(Mathf.Max(state.trackScale + (evt.delta.y * 0.1f), 1.0f), 100.0f);
                return true;
            }

            return false;
        }
    }
}
