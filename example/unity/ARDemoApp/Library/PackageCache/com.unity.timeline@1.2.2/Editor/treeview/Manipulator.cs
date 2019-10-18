using UnityEngine;

namespace UnityEditor.Timeline
{
    abstract class Manipulator
    {
        int m_Id;

        protected virtual bool MouseDown(Event evt, WindowState state) { return false; }
        protected virtual bool MouseDrag(Event evt, WindowState state) { return false; }
        protected virtual bool MouseWheel(Event evt, WindowState state) { return false; }
        protected virtual bool MouseUp(Event evt, WindowState state) { return false; }
        protected virtual bool DoubleClick(Event evt, WindowState state) { return false; }
        protected virtual bool KeyDown(Event evt, WindowState state) { return false; }
        protected virtual bool KeyUp(Event evt, WindowState state) { return false; }
        protected virtual bool ContextClick(Event evt, WindowState state) { return false; }
        protected virtual bool ValidateCommand(Event evt, WindowState state) { return false; }
        protected virtual bool ExecuteCommand(Event evt, WindowState state) { return false; }

        public virtual void Overlay(Event evt, WindowState state) {}

        public bool HandleEvent(WindowState state)
        {
            if (m_Id == 0)
                m_Id =  GUIUtility.GetPermanentControlID();

            bool isHandled = false;
            var evt = Event.current;

            switch (evt.GetTypeForControl(m_Id))
            {
                case EventType.ScrollWheel:
                    isHandled = MouseWheel(evt, state);
                    break;

                case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == m_Id)
                    {
                        isHandled = MouseUp(evt, state);

                        GUIUtility.hotControl = 0;
                        evt.Use();
                    }
                }
                break;

                case EventType.MouseDown:
                {
                    isHandled = evt.clickCount < 2 ? MouseDown(evt, state) : DoubleClick(evt, state);

                    if (isHandled)
                        GUIUtility.hotControl = m_Id;
                }
                break;

                case EventType.MouseDrag:
                {
                    if (GUIUtility.hotControl == m_Id)
                        isHandled = MouseDrag(evt, state);
                }
                break;

                case EventType.KeyDown:
                    isHandled = KeyDown(evt, state);
                    break;

                case EventType.KeyUp:
                    isHandled = KeyUp(evt, state);
                    break;

                case EventType.ContextClick:
                    isHandled = ContextClick(evt, state);
                    break;

                case EventType.ValidateCommand:
                    isHandled = ValidateCommand(evt, state);
                    break;

                case EventType.ExecuteCommand:
                    isHandled = ExecuteCommand(evt, state);
                    break;
            }

            if (isHandled)
                evt.Use();

            return isHandled;
        }
    }
}
