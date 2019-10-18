using System;

namespace UnityEditor.Timeline
{
    interface ISelectable : ILayerable
    {
        void Select();
        bool IsSelected();
        void Deselect();
    }
}
