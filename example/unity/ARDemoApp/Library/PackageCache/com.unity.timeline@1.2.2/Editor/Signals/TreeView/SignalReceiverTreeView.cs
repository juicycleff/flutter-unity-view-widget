using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Signals
{
    class SignalReceiverTreeView : TreeView
    {
        public bool dirty { private get; set; }

        SerializedProperty signals { get; set; }
        SerializedProperty events { get; set; }

        readonly SignalReceiver m_Target;

        const float k_VerticalPadding = 5;
        const float k_HorizontalPadding = 5;

        public SignalReceiverTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, SignalReceiver receiver, bool readonlySignal) : base(state, multiColumnHeader)
        {
            m_Target = receiver;
            var receiverSO = new SerializedObject(receiver);
            signals = SignalReceiverUtility.FindSignalsProperty(receiverSO);
            events = SignalReceiverUtility.FindEventsProperty(receiverSO);
            readonlySignals = readonlySignal;
            useScrollView = true;
            getNewSelectionOverride = (item, selection, shift) => new List<int>(); // Disable Selection
        }

        SignalAsset signalAssetContext { get; set; }
        bool readonlySignals { get; }

        public void SetSignalContext(SignalAsset assetContext = null)
        {
            signalAssetContext = assetContext;
            dirty = true;
        }

        public void SetSerializedProperties(SerializedProperty signalsProperty, SerializedProperty eventsProperty)
        {
            signals = signalsProperty;
            events = eventsProperty;

            Reload();
        }

        public void Draw()
        {
            var rect = EditorGUILayout.GetControlRect(true, GetTotalHeight());
            OnGUI(rect);
        }

        public void RefreshIfDirty()
        {
            if (dirty)
                Reload();
            dirty = false;
        }

        public static MultiColumnHeaderState.Column[] GetColumns()
        {
            return new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = EditorGUIUtility.TrTextContent("Signal"),
                    contextMenuText = "",
                    headerTextAlignment = TextAlignment.Center,
                    width = 50, minWidth = 50,
                    autoResize = true,
                    allowToggleVisibility = false,
                    canSort = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = EditorGUIUtility.TrTextContent("Reaction"),
                    contextMenuText = "",
                    headerTextAlignment = TextAlignment.Center,
                    width = 120, minWidth = 120,
                    autoResize = true,
                    allowToggleVisibility = false,
                    canSort = false
                }
            };
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1) { children = new List<TreeViewItem>() };

            var matchingId = signalAssetContext != null && readonlySignals ? FindIdForSignal(signals, signalAssetContext) : -1;
            if (matchingId >= 0)
                AddItem(root, matchingId);

            for (var i = 0; i < signals.arraySize; ++i)
            {
                if (i == matchingId) continue;
                AddItem(root, i, !readonlySignals);
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (SignalReceiverItem)args.item;
            for (var i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                var rect = args.GetCellRect(i);
                rect.y += k_VerticalPadding;
                item.Draw(rect, args.GetColumn(i), args.row, k_HorizontalPadding, m_Target);
            }
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem treeItem)
        {
            var item = treeItem as SignalReceiverItem;
            return item.GetHeight() + k_VerticalPadding;
        }

        void AddItem(TreeViewItem root, int id, bool enabled = true)
        {
            var signal = signals.GetArrayElementAtIndex(id);
            var evt = events.GetArrayElementAtIndex(id);
            root.children.Add(new SignalReceiverItem(signal, evt, id, readonlySignals, enabled, this));
        }

        float GetTotalHeight()
        {
            var height = 0.0f;
            foreach (var item in GetRows())
            {
                var signalListItem = item as SignalReceiverItem;
                height += signalListItem.GetHeight() + k_VerticalPadding;
            }

            var scrollbarPadding = showingHorizontalScrollBar ? GUI.skin.horizontalScrollbar.fixedHeight : k_VerticalPadding;
            return height + multiColumnHeader.height + scrollbarPadding;
        }

        static int FindIdForSignal(SerializedProperty signals, SignalAsset signalToFind)
        {
            for (var i = 0; i < signals.arraySize; ++i)
            {
                //signal in the receiver that matches the current signal asset will be displayed first
                var serializedProperty = signals.GetArrayElementAtIndex(i);
                var signalReferenceValue = serializedProperty.objectReferenceValue;
                var signalToFindRefValue = signalToFind;
                if (signalReferenceValue != null && signalReferenceValue == signalToFindRefValue)
                    return i;
            }

            return -1;
        }
    }
}
