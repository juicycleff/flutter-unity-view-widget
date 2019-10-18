using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline.Signals
{
    [CustomEditor(typeof(SignalEmitter), true)]
    [CanEditMultipleObjects]
    class SignalEmitterInspector : MarkerInspector, ISignalAssetProvider
    {
        SerializedProperty m_RetroactiveProperty;
        SerializedProperty m_EmitOnceProperty;

        SignalEmitter m_Signal;
        GameObject m_BoundGameObject;
        PlayableDirector m_AssociatedDirector;
        bool m_TargetsHaveTheSameBinding;

        readonly Dictionary<Component, Editor> m_Editors = new Dictionary<Component, Editor>();
        readonly Dictionary<Component, bool> m_Foldouts = new Dictionary<Component, bool>();
        List<Component> m_Receivers = new List<Component>();

        static GUIStyle s_FoldoutStyle;
        internal static GUIStyle foldoutStyle
        {
            get
            {
                if (s_FoldoutStyle == null)
                {
                    s_FoldoutStyle = new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold};
                }

                return s_FoldoutStyle;
            }
        }

        public SignalAsset signalAsset
        {
            get
            {
                var emitter = target as SignalEmitter;
                return signalAssetSameValue ? emitter.asset : null;
            }
            set
            {
                AssignSignalAsset(value);
            }
        }

        bool signalAssetSameValue
        {
            get
            {
                var emitters = targets.Cast<SignalEmitter>().ToList();
                return emitters.Select(x => x.asset).Distinct().Count() == 1;
            }
        }

        void OnEnable()
        {
            Undo.undoRedoPerformed += OnUndoRedo; // subscribe to the event
            m_Signal = target as SignalEmitter;
            m_RetroactiveProperty = serializedObject.FindProperty("m_Retroactive");
            m_EmitOnceProperty = serializedObject.FindProperty("m_EmitOnce");
            // In a vast majority of the cases, when this becomes enabled,
            // the timeline window will be focused on the correct timeline
            // in which case TimelineEditor.inspectedDirector is safe to use
            m_AssociatedDirector = TimelineEditor.inspectedDirector;
            UpdateState();
        }

        internal override bool IsEnabled()
        {
            return TimelineUtility.IsCurrentSequenceValid() && !IsCurrentSequenceReadOnly() && base.IsEnabled();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                var property = serializedObject.GetIterator();
                var expanded = true;
                while (property.NextVisible(expanded))
                {
                    expanded = false;
                    if (SkipField(property.propertyPath))
                        continue;
                    EditorGUILayout.PropertyField(property, true);
                }

                DrawSignalFlags();
                UpdateState();
                DrawNameSelectorAndSignalList();

                if (changeScope.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    TimelineEditor.Refresh(RefreshReason.ContentsModified | RefreshReason.WindowNeedsRedraw);
                }
            }
        }

        internal override void OnHeaderIconGUI(Rect iconRect)
        {
            using (new EditorGUI.DisabledScope(!TimelineUtility.IsCurrentSequenceValid() || IsCurrentSequenceReadOnly()))
            {
                GUI.Label(iconRect, Styles.SignalEmitterIcon);
            }
        }

        internal override Rect DrawHeaderHelpAndSettingsGUI(Rect r)
        {
            using (new EditorGUI.DisabledScope(!TimelineUtility.IsCurrentSequenceValid() || IsCurrentSequenceReadOnly()))
            {
                var helpSize = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
                const int kTopMargin = 5;
                return EditorGUIUtility.DrawEditorHeaderItems(new Rect(r.xMax - helpSize.x, r.y + kTopMargin, helpSize.x, helpSize.y), targets);
            }
        }

        IEnumerable<SignalAsset> ISignalAssetProvider.AvailableSignalAssets()
        {
            return SignalManager.assets;
        }

        void ISignalAssetProvider.CreateNewSignalAsset(string path)
        {
            var newSignalAsset = SignalManager.CreateSignalAssetInstance(path);
            AssignSignalAsset(newSignalAsset);
            var receivers = m_Receivers.OfType<SignalReceiver>().ToList();
            if (signalAsset != null && receivers.Count == 1 && !receivers.Any(r => r.IsSignalAssetHandled(newSignalAsset))) // Only when one receiver is present
            {
                receivers[0].AddNewReaction(newSignalAsset); // Add reaction on the first receiver from the list
                ApplyChangesAndRefreshReceivers();
            }
        }

        void UpdateState()
        {
            m_BoundGameObject = GetBoundGameObject(m_Signal.parent, m_AssociatedDirector);
            m_Receivers = m_BoundGameObject == null || m_BoundGameObject.Equals(null)
                ? new List<Component>()
                : m_BoundGameObject.GetComponents<Component>().Where(t => t is INotificationReceiver).ToList();

            m_TargetsHaveTheSameBinding = targets.Cast<SignalEmitter>()
                .Select(x => GetBoundGameObject(x.parent, m_AssociatedDirector))
                .Distinct().Count() == 1;
        }

        Editor GetOrCreateReceiverEditor(Component c)
        {
            Editor ret;
            if (m_Editors.TryGetValue(c, out ret))
            {
                return ret;
            }

            ret = CreateEditorWithContext(new Object[] {c}, target);
            m_Editors[c] = ret;
            if (!m_Foldouts.ContainsKey(c))
            {
                m_Foldouts[c] = true;
            }

            return ret;
        }

        void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
        }

        void OnDestroy()
        {
            foreach (var editor in m_Editors)
            {
                DestroyImmediate(editor.Value);
            }
            m_Editors.Clear();
        }

        void OnUndoRedo()
        {
            ApplyChangesAndRefreshReceivers();
        }

        void ApplyChangesAndRefreshReceivers()
        {
            foreach (var receiverInspector in m_Editors.Values.OfType<SignalReceiverInspector>())
            {
                receiverInspector.SetAssetContext(signalAsset);
            }
        }

        void DrawNameSelectorAndSignalList()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                DrawSignal();
                DrawReceivers();

                if (change.changed)
                {
                    ApplyChangesAndRefreshReceivers();
                }
            }
        }

        void DrawReceivers()
        {
            if (!m_TargetsHaveTheSameBinding)
            {
                EditorGUILayout.HelpBox(Styles.MultiEditNotSupportedOnDifferentBindings, MessageType.None);
                return;
            }

            if (targets.OfType<SignalEmitter>().Select(x => x.asset).Distinct().Count() > 1)
            {
                EditorGUILayout.HelpBox(Styles.MultiEditNotSupportedOnDifferentSignals, MessageType.None);
                return;
            }

            //do not display the receiver if the current timeline is not the same as the emitter's timeline
            //can happen if the inspector is locked
            if (m_Signal.parent != null && m_Signal.parent.timelineAsset != TimelineEditor.inspectedAsset)
                return;

            if (m_BoundGameObject != null)
            {
                if (!m_Receivers.Any(x => x is SignalReceiver))
                {
                    EditorGUILayout.Separator();
                    var message = string.Format(Styles.NoSignalReceiverComponent, m_BoundGameObject.name);
                    SignalUtility.DrawCenteredMessage(message);
                    if (SignalUtility.DrawCenteredButton(Styles.AddSignalReceiverComponent))
                        AddReceiverComponent();
                }

                foreach (var receiver in m_Receivers)
                {
                    var editor = GetOrCreateReceiverEditor(receiver);
                    if (DrawReceiverHeader(receiver))
                    {
                        editor.OnInspectorGUI();
                    }
                }
            }
            else if (m_AssociatedDirector != null) //not in asset mode
            {
                EditorGUILayout.HelpBox(Styles.NoBoundGO, MessageType.None);
            }
        }

        void DrawSignalFlags()
        {
            EditorGUILayout.PropertyField(m_RetroactiveProperty, Styles.RetroactiveLabel);
            EditorGUILayout.PropertyField(m_EmitOnceProperty, Styles.EmitOnceLabel);
        }

        void DrawSignal()
        {
            //should show button to create new signal if there are no signals asset in the project
            if (!SignalManager.assets.Any())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    DrawNameSelector();
                }

                EditorGUILayout.Separator();
                SignalUtility.DrawCenteredMessage(Styles.ProjectHasNoSignalAsset);
                if (SignalUtility.DrawCenteredButton(Styles.CreateNewSignal))
                    CreateNewSignalAsset(SignalUtility.GetNewSignalPath());
                EditorGUILayout.Separator();
            }
            else
            {
                DrawNameSelector();
            }
        }

        internal void CreateNewSignalAsset(string path)
        {
            if (!string.IsNullOrEmpty(path))
                ((ISignalAssetProvider)this).CreateNewSignalAsset(path);
            GUIUtility.ExitGUI();
        }

        void AssignSignalAsset(SignalAsset newAsset)
        {
            foreach (var o in targets)
            {
                var signalEmitter = (SignalEmitter)o;
                TimelineUndo.PushUndo(signalEmitter, Styles.UndoCreateSignalAsset);
                signalEmitter.asset = newAsset;
            }
        }

        void DrawNameSelector()
        {
            SignalUtility.DrawSignalNames(this, EditorGUILayout.GetControlRect(), Styles.EmitSignalLabel, !signalAssetSameValue);
        }

        bool DrawReceiverHeader(Component receiver)
        {
            EditorGUILayout.Space();
            var lineRect = GUILayoutUtility.GetRect(10, 4, EditorStyles.inspectorTitlebar);
            DrawSplitLine(lineRect.y);

            var style = EditorGUIUtility.TrTextContentWithIcon(
                ObjectNames.NicifyVariableName(receiver.GetType().Name),
                AssetPreview.GetMiniThumbnail(receiver));

            m_Foldouts[receiver] =
                EditorGUILayout.Foldout(m_Foldouts[receiver], style, true, foldoutStyle);
            if (m_Foldouts[receiver])
            {
                DrawReceiverObjectField();
            }

            return m_Foldouts[receiver];
        }

        void DrawReceiverObjectField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(Styles.ObjectLabel, m_BoundGameObject, typeof(GameObject), false);
            EditorGUI.EndDisabledGroup();
        }

        void AddReceiverComponent()
        {
            var receiver = Undo.AddComponent<SignalReceiver>(m_BoundGameObject);
            receiver.AddNewReaction(signalAsset);
        }

        static bool SkipField(string fieldName)
        {
            return fieldName == "m_Script" || fieldName == "m_Asset" || fieldName == "m_Retroactive" || fieldName == "m_EmitOnce";
        }

        static void DrawSplitLine(float y)
        {
            if (Event.current.type != EventType.Repaint) return;

            var width = EditorGUIUtility.currentViewWidth;
            var position = new Rect(0, y, width + 1, 1);
            
            if (EditorStyles.inspectorTitlebar != null)
                EditorStyles.inspectorTitlebar.Draw(position, false, false, false, false);
        }

        static GameObject GetBoundGameObject(TrackAsset track, PlayableDirector associatedDirector)
        {
            if (associatedDirector == null || track == null) //if in asset mode, no bound object for you
                return null;

            var boundObj = TimelineUtility.GetSceneGameObject(associatedDirector, track);

            //if the signal is on the timeline marker track and user did not set a binding, assume it's bound to PlayableDirector
            if (boundObj == null && track.timelineAsset.markerTrack == track)
                boundObj = associatedDirector.gameObject;

            return boundObj;
        }

        static bool IsCurrentSequenceReadOnly()
        {
            return TimelineWindow.instance.state.editSequence.isReadOnly;
        }
    }
}
