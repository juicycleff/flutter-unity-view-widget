using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Signals
{
    static class SignalListFactory
    {
        public static SignalReceiverTreeView CreateSignalInspectorList(TreeViewState state, MultiColumnHeaderState columnState, SignalReceiver target, int columnHeight, bool readonlySignal)
        {
            var header = new SignalReceiverHeader(columnState) { height = columnHeight };
            header.ResizeToFit();

            return new SignalReceiverTreeView(state, header, target, readonlySignal);
        }

        public static MultiColumnHeaderState CreateHeaderState()
        {
            return new MultiColumnHeaderState(SignalReceiverTreeView.GetColumns());
        }

        public static TreeViewState CreateViewState()
        {
            return new TreeViewState();
        }
    }
}
