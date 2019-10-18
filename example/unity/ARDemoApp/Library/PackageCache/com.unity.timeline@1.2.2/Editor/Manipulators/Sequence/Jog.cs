using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class Jog : Manipulator
    {
        Vector2 m_MouseDownOrigin = Vector2.zero;

        [ClutchShortcut("Timeline/Jog", typeof(TimelineWindow), KeyCode.J)]
        static void JogShortcut(ShortcutArguments args)
        {
            if (args.stage == ShortcutStage.Begin)
            {
                (args.context as TimelineWindow).state.isJogging = true;
            }
            else if (args.stage == ShortcutStage.End)
            {
                (args.context as TimelineWindow).state.isJogging = false;
            }
        }

        protected override bool MouseDown(Event evt, WindowState state)
        {
            if (!state.isJogging)
                return false;

            m_MouseDownOrigin = evt.mousePosition;
            state.playbackSpeed = 0.0f;
            state.Play();

            return true;
        }

        protected override bool MouseUp(Event evt, WindowState state)
        {
            if (!state.isJogging)
            {
                return false;
            }

            m_MouseDownOrigin = evt.mousePosition;
            state.playbackSpeed = 0.0f;
            state.Play();
            return false;
        }

        protected override bool MouseDrag(Event evt, WindowState state)
        {
            if (!state.isJogging)
                return false;

            var distance = evt.mousePosition - m_MouseDownOrigin;

            state.playbackSpeed = distance.x * 0.002f;
            state.Play();
            return true;
        }
    }
}
