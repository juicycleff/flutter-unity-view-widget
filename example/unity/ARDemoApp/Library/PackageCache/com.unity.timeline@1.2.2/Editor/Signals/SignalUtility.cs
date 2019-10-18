using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Signals
{
    interface ISignalAssetProvider
    {
        SignalAsset signalAsset { get; set; }
        IEnumerable<SignalAsset> AvailableSignalAssets();
        void CreateNewSignalAsset(string path);
    }

    static class SignalUtility
    {
        const string k_SignalExtension = "signal";

        public static void DrawSignalNames(ISignalAssetProvider assetProvider, Rect position, GUIContent label, bool multipleValues)
        {
            var assets = assetProvider.AvailableSignalAssets().ToList();
            var index = assets.IndexOf(assetProvider.signalAsset);

            var availableNames = new List<string>();
            using (new GUIMixedValueScope(multipleValues))
            {
                availableNames.Add(Styles.EmptySignalList.text);

                availableNames.AddRange(assets.Select(x => x.name));
                availableNames.Add(Styles.CreateNewSignal.text);

                var curValue = index + 1;
                var selected = EditorGUI.Popup(position, label, curValue, availableNames.ToArray());

                if (selected != curValue)
                {
                    var noneEntryIdx = 0;
                    if (selected == noneEntryIdx) // None
                        assetProvider.signalAsset = null;
                    else if (selected == availableNames.Count - 1) // "Create New Asset"
                    {
                        var path = GetNewSignalPath();
                        if (!string.IsNullOrEmpty(path))
                            assetProvider.CreateNewSignalAsset(path);
                        GUIUtility.ExitGUI();
                    }
                    else
                        assetProvider.signalAsset = assets[selected - 1];
                }
            }
        }

        public static string GetNewSignalPath()
        {
            return EditorUtility.SaveFilePanelInProject(
                Styles.NewSignalWindowTitle.text,
                Styles.NewSignalDefaultName.text,
                k_SignalExtension,
                Styles.NewSignalWindowMessage.text);
        }

        public static bool IsSignalAssetHandled(this SignalReceiver receiver, SignalAsset asset)
        {
            return receiver != null && asset != null && receiver.GetRegisteredSignals().Contains(asset);
        }

        public static void AddNewReaction(this SignalReceiver receiver, SignalAsset signalAsset)
        {
            if (signalAsset != null && receiver != null)
            {
                Undo.RegisterCompleteObjectUndo(receiver, Styles.UndoAddReaction);

                var newEvent = new UnityEvent();
                newEvent.AddPersistentListener();
                var evtIndex = newEvent.GetPersistentEventCount() - 1;
                newEvent.RegisterVoidPersistentListenerWithoutValidation(evtIndex, receiver.gameObject, string.Empty);
                receiver.AddReaction(signalAsset, newEvent);
            }
        }

        public static void DrawCenteredMessage(string message)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(message);
                GUILayout.FlexibleSpace();
            }
        }

        public static bool DrawCenteredButton(GUIContent buttonLabel)
        {
            bool buttonClicked;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                buttonClicked = GUILayout.Button(buttonLabel);
                GUILayout.FlexibleSpace();
            }
            return buttonClicked;
        }
    }

    static class SignalReceiverUtility
    {
        const int k_DefaultTreeviewHeaderHeight = 20;

        public static int headerHeight
        {
            get { return k_DefaultTreeviewHeaderHeight; }
        }

        public static SerializedProperty FindSignalsProperty(SerializedObject obj)
        {
            return obj.FindProperty("m_Events.m_Signals");
        }

        public static SerializedProperty FindEventsProperty(SerializedObject obj)
        {
            return obj.FindProperty("m_Events.m_Events");
        }
    }
}
