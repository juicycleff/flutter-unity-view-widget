using System.Collections.Generic;

namespace UnityEditor.Timeline
{
    class Control
    {
        readonly List<Manipulator> m_Manipulators = new List<Manipulator>();

        public bool HandleManipulatorsEvents(WindowState state)
        {
            var isHandled = false;

            foreach (var manipulator in m_Manipulators)
            {
                isHandled = manipulator.HandleEvent(state);
                if (isHandled)
                    break;
            }

            return isHandled;
        }

        public void AddManipulator(Manipulator m)
        {
            m_Manipulators.Add(m);
        }
    }
}
