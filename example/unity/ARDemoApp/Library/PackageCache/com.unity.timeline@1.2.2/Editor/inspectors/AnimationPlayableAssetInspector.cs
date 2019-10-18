using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(AnimationPlayableAsset)), CanEditMultipleObjects]
    class AnimationPlayableAssetInspector : Editor
    {
        static class Styles
        {
            public static readonly GUIContent RotationText = EditorGUIUtility.TrTextContent("Rotation");
            public static readonly GUIContent AnimClipText = EditorGUIUtility.TrTextContent("Animation Clip");
            public static readonly GUIContent TransformOffsetTitle = EditorGUIUtility.TrTextContent("Clip Transform Offsets", "Use this to offset the root transform position and rotation relative to the track when playing this clip");
            public static readonly GUIContent AnimationClipName = EditorGUIUtility.TrTextContent("Animation Clip Name");
            public static readonly GUIContent MatchTargetFieldsTitle = EditorGUIUtility.TrTextContent("Offsets Match Fields", "Fields to apply when matching offsets on clips. The defaults can be set on the track.");
            public static readonly GUIContent UseDefaults = EditorGUIUtility.TrTextContent("Use defaults");
            public static readonly GUIContent RemoveStartOffset = EditorGUIUtility.TrTextContent("Remove Start Offset", "Makes playback of the clip play relative to first key of the root transform");
            public static readonly GUIContent ApplyFootIK = EditorGUIUtility.TrTextContent("Foot IK", "Enable to apply foot IK to the AnimationClip when the target is humanoid.");
            public static readonly GUIContent Loop = EditorGUIUtility.TrTextContent("Loop", "Whether the source Animation Clip loops during playback.");
        }

        TimelineWindow m_TimelineWindow;
        GameObject m_Binding;

        TimelineAnimationUtilities.OffsetEditMode m_OffsetEditMode = TimelineAnimationUtilities.OffsetEditMode.None;
        EditorClip m_EditorClip;
        EditorClip[] m_EditorClips;

        SerializedProperty m_PositionProperty;
        SerializedProperty m_RotationProperty;
        SerializedProperty m_AnimClipProperty;
        SerializedProperty m_UseTrackMatchFieldsProperty;
        SerializedProperty m_MatchTargetFieldsProperty;
        SerializedObject m_SerializedAnimClip;
        SerializedProperty m_SerializedAnimClipName;
        SerializedProperty m_RemoveStartOffsetProperty;
        SerializedProperty m_ApplyFootIK;
        SerializedProperty m_Loop;

        Vector3 m_LastPosition;
        Vector3 m_LastRotation;

        public override void OnInspectorGUI()
        {
            if (target == null)
                return;

            serializedObject.Update();

            if (!m_TimelineWindow) m_TimelineWindow = TimelineWindow.instance;

            ShowAnimationClipField();
            ShowRecordableClipRename();
            ShowAnimationClipWarnings();

            EditorGUI.BeginChangeCheck();

            TransformOffsetsGUI();

            // extra checks are because the context menu may need to cause a re-evaluate
            bool changed = EditorGUI.EndChangeCheck() ||
                m_LastPosition != m_PositionProperty.vector3Value ||
                m_LastRotation != m_RotationProperty.vector3Value;
            m_LastPosition = m_PositionProperty.vector3Value;
            m_LastRotation = m_RotationProperty.vector3Value;

            if (changed)
            {
                // updates the changed properties and pushes them to the active playable
                serializedObject.ApplyModifiedProperties();
                ((AnimationPlayableAsset)target).LiveLink();

                // force an evaluate to happen next frame
                if (TimelineWindow.instance != null && TimelineWindow.instance.state != null)
                {
                    TimelineWindow.instance.state.Evaluate();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ApplyFootIK, Styles.ApplyFootIK);
            EditorGUILayout.PropertyField(m_Loop, Styles.Loop);
            if (EditorGUI.EndChangeCheck())
                TimelineEditor.Refresh(RefreshReason.ContentsModified);

            serializedObject.ApplyModifiedProperties();
        }

        void ShowAnimationClipField()
        {
            bool disabled = m_EditorClips == null || m_EditorClips.Any(c => c.clip == null || c.clip.recordable);
            using (new EditorGUI.DisabledScope(disabled))
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_AnimClipProperty, Styles.AnimClipText);
                if (EditorGUI.EndChangeCheck())
                {
                    // rename the timeline clips to match the animation name if it did previously
                    if (m_AnimClipProperty.objectReferenceValue != null && m_EditorClips != null)
                    {
                        var newName = m_AnimClipProperty.objectReferenceValue.name;
                        foreach (var c in m_EditorClips)
                        {
                            if (c == null || c.clip == null || c.clip.asset == null)
                                continue;

                            var apa = c.clip.asset as AnimationPlayableAsset;
                            if (apa != null && apa.clip != null && c.clip.displayName == apa.clip.name)
                            {
                                if (c.clip.parentTrack != null)
                                    Undo.RegisterCompleteObjectUndo(c.clip.parentTrack, "Inspector");
                                c.clip.displayName = newName;
                            }
                        }
                    }

                    TimelineEditor.Refresh(RefreshReason.ContentsModified);
                }
            }
        }

        void TransformOffsetsMatchFieldsGUI()
        {
            var rect = EditorGUILayout.GetControlRect(true);
            EditorGUI.BeginProperty(rect, Styles.MatchTargetFieldsTitle, m_UseTrackMatchFieldsProperty);

            rect = EditorGUI.PrefixLabel(rect, Styles.MatchTargetFieldsTitle);
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.BeginChangeCheck();
            bool val = m_UseTrackMatchFieldsProperty.boolValue;
            val = EditorGUI.ToggleLeft(rect, Styles.UseDefaults, val);
            if (EditorGUI.EndChangeCheck())
                m_UseTrackMatchFieldsProperty.boolValue = val;

            EditorGUI.indentLevel = oldIndent;
            EditorGUI.EndProperty();


            if (!val || m_UseTrackMatchFieldsProperty.hasMultipleDifferentValues)
            {
                EditorGUI.indentLevel++;
                AnimationTrackInspector.MatchTargetsFieldGUI(m_MatchTargetFieldsProperty);
                EditorGUI.indentLevel--;
            }
        }

        void TransformOffsetsGUI()
        {
            if (ShouldShowOffsets())
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(Styles.TransformOffsetTitle);
                EditorGUI.indentLevel++;

                using (new EditorGUI.DisabledScope(targets.Length > 1))
                {
                    var previousOffsetMode = m_OffsetEditMode;
                    AnimationTrackInspector.ShowMotionOffsetEditModeToolbar(ref m_OffsetEditMode);
                    if (previousOffsetMode != m_OffsetEditMode)
                    {
                        SetTimeToClip();
                        SceneView.RepaintAll();
                    }
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PositionProperty);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_RotationProperty, Styles.RotationText);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUI.indentLevel--;

                TransformOffsetsMatchFieldsGUI();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_RemoveStartOffsetProperty, Styles.RemoveStartOffset);
                if (EditorGUI.EndChangeCheck())
                {
                    TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
                    Repaint();
                }
            }
        }

        void Reevaluate()
        {
            if (m_TimelineWindow != null && m_TimelineWindow.state != null)
            {
                m_TimelineWindow.state.Refresh();
                m_TimelineWindow.state.EvaluateImmediate();
            }
        }

        // Make sure the director time is within the bounds of the clip
        void SetTimeToClip()
        {
            if (m_TimelineWindow != null && m_TimelineWindow.state != null)
            {
                m_TimelineWindow.state.editSequence.time = Math.Min(m_EditorClip.clip.end, Math.Max(m_EditorClip.clip.start, m_TimelineWindow.state.editSequence.time));
            }
        }

        public void OnEnable()
        {
            if (target == null) // case 946080
                return;

            m_EditorClip = UnityEditor.Selection.activeObject as EditorClip;
            m_EditorClips = UnityEditor.Selection.objects.OfType<EditorClip>().ToArray();
            SceneView.duringSceneGui += OnSceneGUI;

            m_PositionProperty = serializedObject.FindProperty("m_Position");
            m_PositionProperty.isExpanded = true;
            m_RotationProperty = serializedObject.FindProperty("m_EulerAngles");
            m_AnimClipProperty = serializedObject.FindProperty("m_Clip");
            m_UseTrackMatchFieldsProperty = serializedObject.FindProperty("m_UseTrackMatchFields");
            m_MatchTargetFieldsProperty = serializedObject.FindProperty("m_MatchTargetFields");
            m_RemoveStartOffsetProperty = serializedObject.FindProperty("m_RemoveStartOffset");
            m_ApplyFootIK = serializedObject.FindProperty("m_ApplyFootIK");
            m_Loop = serializedObject.FindProperty("m_Loop");

            m_LastPosition = m_PositionProperty.vector3Value;
            m_LastRotation = m_RotationProperty.vector3Value;
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            DoManipulators();
        }

        Transform GetTransform()
        {
            if (m_Binding != null)
                return m_Binding.transform;

            if (m_TimelineWindow != null &&  m_TimelineWindow.state != null &&
                m_TimelineWindow.state.editSequence.director != null &&
                m_EditorClip != null && m_EditorClip.clip != null)
            {
                var obj = TimelineUtility.GetSceneGameObject(m_TimelineWindow.state.editSequence.director,
                    m_EditorClip.clip.parentTrack);
                m_Binding = obj;
                if (obj != null)
                    return obj.transform;
            }
            return null;
        }

        void DoManipulators()
        {
            if (m_EditorClip == null || m_EditorClip.clip == null)
                return;

            AnimationPlayableAsset animationPlayable = m_EditorClip.clip.asset as AnimationPlayableAsset;
            AnimationTrack track = m_EditorClip.clip.parentTrack as AnimationTrack;
            Transform transform = GetTransform();

            if (transform != null && animationPlayable != null && m_OffsetEditMode != TimelineAnimationUtilities.OffsetEditMode.None && track != null)
            {
                TimelineUndo.PushUndo(animationPlayable, "Inspector");
                Vector3 position = transform.position;
                Quaternion rotation = transform.rotation;

                EditorGUI.BeginChangeCheck();
                if (m_OffsetEditMode == TimelineAnimationUtilities.OffsetEditMode.Translation)
                {
                    position = Handles.PositionHandle(position, Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : rotation);
                }
                else if (m_OffsetEditMode == TimelineAnimationUtilities.OffsetEditMode.Rotation)
                {
                    rotation = Handles.RotationHandle(rotation, position);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    var res = TimelineAnimationUtilities.UpdateClipOffsets(animationPlayable, track, transform, position, rotation);
                    animationPlayable.position = res.position;
                    animationPlayable.eulerAngles = AnimationUtility.GetClosestEuler(res.rotation, animationPlayable.eulerAngles, RotationOrder.OrderZXY);
                    Reevaluate();
                    Repaint();
                }
            }
        }

        void ShowAnimationClipWarnings()
        {
            AnimationClip clip = m_AnimClipProperty.objectReferenceValue as AnimationClip;
            if (clip == null)
            {
                EditorGUILayout.HelpBox(AnimationPlayableAssetEditor.k_NoClipAssignedError, MessageType.Warning);
            }
            else if (clip.legacy)
            {
                EditorGUILayout.HelpBox(AnimationPlayableAssetEditor.k_LegacyClipError, MessageType.Warning);
            }
        }

        bool ShouldShowOffsets()
        {
            return targets.OfType<AnimationPlayableAsset>().All(x => x.hasRootTransforms);
        }

        void ShowRecordableClipRename()
        {
            if (targets.Length > 1 || m_EditorClip == null || m_EditorClip.clip == null || !m_EditorClip.clip.recordable)
                return;

            AnimationClip clip = m_AnimClipProperty.objectReferenceValue as AnimationClip;
            if (clip == null || !AssetDatabase.IsSubAsset(clip))
                return;

            if (m_SerializedAnimClip == null)
            {
                m_SerializedAnimClip = new SerializedObject(clip);
                m_SerializedAnimClipName = m_SerializedAnimClip.FindProperty("m_Name");
            }

            if (m_SerializedAnimClipName != null)
            {
                m_SerializedAnimClip.Update();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.DelayedTextField(m_SerializedAnimClipName, Styles.AnimationClipName);
                if (EditorGUI.EndChangeCheck())
                    m_SerializedAnimClip.ApplyModifiedProperties();
            }
        }
    }
}
