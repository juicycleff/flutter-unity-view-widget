using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Events;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Signals
{
    [CustomEditor(typeof(SignalReceiver))]
    class SignalReceiverInspector : Editor
    {
        SignalReceiver m_Target;
        SerializedProperty m_EventsProperty;
        SerializedProperty m_SignalNameProperty;

        [SerializeField] TreeViewState m_TreeState;
        [SerializeField] MultiColumnHeaderState m_MultiColumnHeaderState;
        internal SignalReceiverTreeView m_TreeView;

        SignalEmitter signalEmitterContext
        {
            get { return m_Context as SignalEmitter;}
        }

        void OnEnable()
        {
            m_Target = target as SignalReceiver;
            m_SignalNameProperty = SignalReceiverUtility.FindSignalsProperty(serializedObject);
            m_EventsProperty = SignalReceiverUtility.FindEventsProperty(serializedObject);
            InitTreeView(m_SignalNameProperty, m_EventsProperty);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
        }

        void OnUndoRedo()
        {
            m_TreeView.dirty = true;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            m_TreeView.RefreshIfDirty();
            DrawEmitterControls(); // Draws buttons coming from the Context (SignalEmitter)

            EditorGUILayout.Space();
            m_TreeView.Draw();

            if (signalEmitterContext == null)
                DrawAddRemoveButtons();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                m_TreeView.dirty = true;
            }
        }

        void DrawEmitterControls()
        {
            var context = signalEmitterContext;
            if (context != null)
            {
                var currentSignal = context.asset;
                if (currentSignal != null && !m_Target.IsSignalAssetHandled(currentSignal))
                {
                    EditorGUILayout.Separator();
                    var message = string.Format(Styles.NoReaction, currentSignal.name);
                    SignalUtility.DrawCenteredMessage(message);
                    if (SignalUtility.DrawCenteredButton(Styles.AddReactionButton))
                        m_Target.AddNewReaction(currentSignal); // Add reaction on the first
                    EditorGUILayout.Separator();
                }
            }
        }

        internal void SetAssetContext(SignalAsset asset)
        {
            m_TreeView.SetSignalContext(asset);
        }

        void DrawAddRemoveButtons()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Styles.AddReactionButton))
                {
                    Undo.RegisterCompleteObjectUndo(m_Target, Styles.UndoAddReaction);
                    m_Target.AddEmptyReaction(new UnityEvent());
                }
                GUILayout.Space(18.0f);
            }
        }

        void InitTreeView(SerializedProperty signals, SerializedProperty events)
        {
            m_TreeState = SignalListFactory.CreateViewState();
            m_MultiColumnHeaderState = SignalListFactory.CreateHeaderState();

            var context = signalEmitterContext;
            m_TreeView = SignalListFactory.CreateSignalInspectorList(m_TreeState, m_MultiColumnHeaderState, target as SignalReceiver, SignalReceiverUtility.headerHeight, context != null);
            m_TreeView.SetSerializedProperties(signals, events);

            if (context != null)
                m_TreeView.SetSignalContext(context.asset);
        }
    }
}
