using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline.Signals
{
    class SignalReceiverItem : TreeViewItem, ISignalAssetProvider
    {
        static readonly SignalEventDrawer k_EvtDrawer = new SignalEventDrawer();

        readonly SerializedProperty m_Asset;
        readonly SerializedProperty m_Evt;
        readonly SignalReceiverTreeView m_TreeView;

        int m_CurrentRowIdx;
        SignalReceiver m_CurrentReceiver;

        internal readonly bool enabled;
        internal readonly bool readonlySignal;

        internal const string SignalName = "SignalName";
        internal const string SignalNameReadOnly = "SignalNameReadOnly";
        internal const string SignalOptions = "SignalOptions";

        public SignalReceiverItem(SerializedProperty signalAsset, SerializedProperty eventListEntry, int id, bool readonlySignal, bool enabled, SignalReceiverTreeView treeView)
            : base(id, 0)
        {
            m_Asset = signalAsset;
            m_Evt = eventListEntry;
            this.enabled = enabled;
            this.readonlySignal = readonlySignal;
            m_TreeView = treeView;
        }

        public SignalAsset signalAsset
        {
            get { return m_CurrentReceiver.GetSignalAssetAtIndex(m_CurrentRowIdx); }
            set
            {
                Undo.RegisterCompleteObjectUndo(m_CurrentReceiver, Styles.UndoCreateSignalAsset);
                m_CurrentReceiver.ChangeSignalAtIndex(m_CurrentRowIdx, value);
            }
        }

        public float GetHeight()
        {
            return k_EvtDrawer.GetPropertyHeight(m_Evt, GUIContent.none);
        }

        public void Draw(Rect rect, int colIdx, int rowIdx, float padding, SignalReceiver target)
        {
            switch (colIdx)
            {
                case 0:
                    DrawSignalNameColumn(rect, padding, target, rowIdx);
                    break;
                case 1:
                    DrawReactionColumn(rect, rowIdx);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void DrawSignalNameColumn(Rect rect, float padding, SignalReceiver target, int rowIdx)
        {
            using (new EditorGUI.DisabledScope(!enabled))
            {
                if (!readonlySignal)
                {
                    m_CurrentRowIdx = rowIdx;
                    m_CurrentReceiver = target;

                    rect.x += padding;
                    rect.width -= padding;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    GUI.SetNextControlName(SignalName);
                    SignalUtility.DrawSignalNames(this, rect, GUIContent.none, false);
                }
                else
                {
                    GUI.SetNextControlName(SignalNameReadOnly);
                    var signalAsset = m_Asset.objectReferenceValue;
                    GUI.Label(rect,
                        signalAsset != null
                        ? EditorGUIUtility.TempContent(signalAsset.name)
                        : Styles.EmptySignalList);
                }
            }
        }

        void DrawReactionColumn(Rect rect, int rowIdx)
        {
            if (!readonlySignal)
            {
                var optionButtonSize = GetOptionButtonSize();
                rect.width -= optionButtonSize.x;

                var optionButtonRect = new Rect
                {
                    x = rect.xMax,
                    y = rect.y,
                    width = optionButtonSize.x,
                    height = optionButtonSize.y
                };
                DrawOptionsButton(optionButtonRect, rowIdx, m_CurrentReceiver);
            }

            using (new EditorGUI.DisabledScope(!enabled))
            {
                var nameAsString = m_Asset.objectReferenceValue == null ? "Null" : m_Asset.objectReferenceValue.name;
                using (var change = new EditorGUI.ChangeCheckScope())
                {
                    EditorGUI.PropertyField(rect, m_Evt, EditorGUIUtility.TempContent(nameAsString));
                    if (change.changed)
                        m_TreeView.dirty = true;
                }
            }
        }

        static Vector2 GetOptionButtonSize()
        {
            EditorGUIUtility.SetIconSize(Vector2.zero);
            return EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.titleSettingsIcon);
        }

        void DrawOptionsButton(Rect rect, int rowIdx, SignalReceiver target)
        {
            GUI.SetNextControlName(SignalOptions);
            if (EditorGUI.DropdownButton(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Passive, EditorStyles.iconButton))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent(Styles.SignalListDuplicateOption), false, () =>
                {
                    Undo.RegisterCompleteObjectUndo(target, "Duplicate Row");
                    var evtCloner = ScriptableObject.CreateInstance<UnityEventCloner>();
                    evtCloner.evt = target.GetReactionAtIndex(rowIdx);
                    var clone = Object.Instantiate(evtCloner);
                    target.AddEmptyReaction(clone.evt);
                    m_TreeView.dirty = true;
                });
                menu.AddItem(new GUIContent(Styles.SignalListDeleteOption), false, () =>
                {
                    Undo.RegisterCompleteObjectUndo(target, "Delete Row");
                    target.RemoveAtIndex(rowIdx);
                    m_TreeView.dirty = true;
                });
                menu.ShowAsContext();
            }
        }

        IEnumerable<SignalAsset> ISignalAssetProvider.AvailableSignalAssets()
        {
            var ret = SignalManager.assets.Except(m_CurrentReceiver.GetRegisteredSignals());
            return signalAsset == null ? ret : ret.Union(new List<SignalAsset> {signalAsset}).ToList();
        }

        void ISignalAssetProvider.CreateNewSignalAsset(string path)
        {
            var newSignalAsset = SignalManager.CreateSignalAssetInstance(path);
            m_CurrentReceiver.ChangeSignalAtIndex(m_CurrentRowIdx, newSignalAsset);
        }

        class UnityEventCloner : ScriptableObject
        {
            public UnityEvent evt;
        }
    }
}
