using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor.Timeline.Signals
{
    class SignalReceiverHeader : MultiColumnHeader
    {
        public SignalReceiverHeader(MultiColumnHeaderState state) : base(state)
        {
        }

        protected override void AddColumnHeaderContextMenuItems(GenericMenu menu)
        {
            menu.AddItem(EditorGUIUtility.TrTextContent("Resize to Fit"), false, ResizeToFit);
        }
    }
}
