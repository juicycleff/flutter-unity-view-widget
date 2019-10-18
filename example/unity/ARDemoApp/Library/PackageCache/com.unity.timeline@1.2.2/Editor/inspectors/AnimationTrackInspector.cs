//#define PERF_PROFILE

using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(AnimationTrack)), CanEditMultipleObjects]
    class AnimationTrackInspector : TrackAssetInspector
    {
        static class Styles
        {
            public static GUIContent MatchTargetFieldsTitle = EditorGUIUtility.TrTextContent("Default Offset Match Fields", "Fields to apply when matching offsets on clips. These are the defaults, and can be overridden for each clip.");
            public static readonly GUIContent PositionIcon = EditorGUIUtility.IconContent("MoveTool");
            public static readonly GUIContent RotationIcon = EditorGUIUtility.IconContent("RotateTool");

            public static GUIContent XTitle = EditorGUIUtility.TextContent("X");
            public static GUIContent YTitle = EditorGUIUtility.TextContent("Y");
            public static GUIContent ZTitle = EditorGUIUtility.TextContent("Z");
            public static GUIContent PositionTitle = EditorGUIUtility.TrTextContent("Position");
            public static GUIContent RotationTitle = EditorGUIUtility.TrTextContent("Rotation");

            public static readonly GUIContent OffsetModeTitle = EditorGUIUtility.TrTextContent("Track Offsets");
            public static readonly string TransformOffsetInfo = L10n.Tr("Transform offsets are applied to the entire track. Use this mode to play the animation track at a fixed position and rotation.");
            public static readonly string SceneOffsetInfo = L10n.Tr("Scene offsets will use the existing transform as initial offsets. Use this to play the track from the gameObjects current position and rotation.");
            public static readonly string AutoOffsetInfo = L10n.Tr("Auto will apply scene offsets if there is a controller attached to the animator and transform offsets otherwise.");
            public static readonly string AutoOffsetWarning = L10n.Tr("This mode is deprecated may be removed in a future release.");
            public static readonly string InheritedFromParent = L10n.Tr("Inherited");
            public static readonly string InheritedToolTip = L10n.Tr("This value is inherited from it's parent track.");

            public static readonly GUIContent RecordingOffsets = EditorGUIUtility.TrTextContent("Recorded Offsets", "Offsets applied to recorded position and rotation keys");

            public static readonly GUIContent[] OffsetContents;
            public static readonly GUIContent[] OffsetInheritContents;

            static Styles()
            {
                var values = Enum.GetValues(typeof(TrackOffset));
                OffsetContents = new GUIContent[values.Length];
                OffsetInheritContents = new GUIContent[values.Length];
                for (var index = 0; index < values.Length; index++)
                {
                    var offset = (TrackOffset)index;
                    var name = ObjectNames.NicifyVariableName(L10n.Tr(offset.ToString()));
                    var memInfo =  typeof(TrackOffset).GetMember(offset.ToString());
                    var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes.Length > 0)
                    {
                        name = ((DescriptionAttribute)attributes[0]).Description;
                    }

                    OffsetContents[index] = new GUIContent(name);
                    OffsetInheritContents[index] = new GUIContent(string.Format("{0} ({1})", InheritedFromParent, name));
                }
            }
        }

        TimelineAnimationUtilities.OffsetEditMode m_OffsetEditMode = TimelineAnimationUtilities.OffsetEditMode.None;

        SerializedProperty m_MatchFieldsProperty;
        SerializedProperty m_TrackPositionProperty;
        SerializedProperty m_TrackRotationProperty;
        SerializedProperty m_AvatarMaskProperty;
        SerializedProperty m_ApplyAvatarMaskProperty;
        SerializedProperty m_TrackOffsetProperty;

        SerializedProperty m_RecordedOffsetPositionProperty;
        SerializedProperty m_RecordedOffsetEulerProperty;

        Vector3            m_lastPosition;
        Vector3            m_lastRotation;

        GUIContent         m_TempContent = new GUIContent();


        void Evaluate()
        {
            if (timelineWindow.state != null && timelineWindow.state.editSequence.director != null)
            {
                // force the update immediately, the deferred doesn't always work with the inspector
                timelineWindow.state.editSequence.director.Evaluate();
            }
        }

        void RebuildGraph()
        {
            if (timelineWindow.state != null)
            {
                timelineWindow.state.rebuildGraph = true;
                timelineWindow.Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(IsTrackLocked()))
            {
                serializedObject.Update();

                DrawRootTransformOffset();

                EditorGUI.BeginChangeCheck();
                DrawRecordedOffsetProperties();
                if (EditorGUI.EndChangeCheck())
                    RebuildGraph();

                DrawAvatarProperties();
                DrawMatchFieldsGUI();

                serializedObject.ApplyModifiedProperties();
            }
        }

        bool AnimatesRootTransform()
        {
            return targets.OfType<AnimationTrack>().All(t => t.AnimatesRootTransform());
        }

        bool ShouldDrawOffsets()
        {
            bool hasMultiple;
            var offsetMode = GetOffsetMode(out hasMultiple);
            if (hasMultiple)
                return false;

            if (offsetMode == TrackOffset.ApplySceneOffsets)
                return false;

            if (offsetMode == TrackOffset.ApplyTransformOffsets)
                return true;

            // Auto mode.
            PlayableDirector director = this.m_Context as PlayableDirector;
            if (director == null)
                return false;

            // If any bound animators have controllers don't show
            foreach (var track in targets.OfType<AnimationTrack>())
            {
                var animator = track.GetBinding(director);
                if (animator != null && animator.runtimeAnimatorController != null)
                    return false;
            }

            return true;
        }

        void DrawRootTransformOffset()
        {
            if (!AnimatesRootTransform())
                return;

            bool showWarning = SetupOffsetTooltip();
            DrawRootTransformDropDown();

            if (ShouldDrawOffsets())
            {
                EditorGUI.indentLevel++;
                DrawRootMotionToolBar();
                DrawRootMotionOffsetFields();
                EditorGUI.indentLevel--;
            }

            if (showWarning)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox(Styles.AutoOffsetWarning, MessageType.Warning, true);
                EditorGUI.indentLevel--;
            }
        }

        bool SetupOffsetTooltip()
        {
            Styles.OffsetModeTitle.tooltip = string.Empty;
            bool hasMultiple;
            var offsetMode = GetOffsetMode(out hasMultiple);
            bool showWarning = false;
            if (!hasMultiple)
            {
                if (offsetMode == TrackOffset.ApplyTransformOffsets)
                    Styles.OffsetModeTitle.tooltip = Styles.TransformOffsetInfo;
                else if (offsetMode == TrackOffset.ApplySceneOffsets)
                    Styles.OffsetModeTitle.tooltip = Styles.SceneOffsetInfo;
                else if (offsetMode == TrackOffset.Auto)
                {
                    Styles.OffsetModeTitle.tooltip = Styles.AutoOffsetInfo;
                    showWarning = true;
                }
            }

            return showWarning;
        }

        void DrawRootTransformDropDown()
        {
            bool anySubTracks = targets.OfType<AnimationTrack>().Any(t => t.isSubTrack);
            bool allSubTracks = targets.OfType<AnimationTrack>().All(t => t.isSubTrack);

            bool mixed;
            var rootOffsetMode = GetOffsetMode(out mixed);

            // if we are showing subtracks, we need to show the current mode from the parent
            //  BUT keep it disabled
            if (anySubTracks)
            {
                m_TempContent.tooltip = string.Empty;
                if (mixed)
                    m_TempContent.text = EditorGUI.mixedValueContent.text;
                else if (!allSubTracks)
                    m_TempContent.text = Styles.OffsetContents[(int)rootOffsetMode].text;
                else
                {
                    m_TempContent.text = Styles.OffsetInheritContents[(int)rootOffsetMode].text;
                    m_TempContent.tooltip = Styles.InheritedToolTip;
                }

                using (new EditorGUI.DisabledScope(true))
                    EditorGUILayout.LabelField(Styles.OffsetModeTitle, m_TempContent, EditorStyles.popup);
            }
            else
            {
                // We use an enum popup explicitly because it will handle the description attribute on the enum
                using (new GUIMixedValueScope(mixed))
                {
                    var rect = EditorGUILayout.GetControlRect(true, EditorGUI.kSingleLineHeight);
                    EditorGUI.BeginProperty(rect, Styles.OffsetModeTitle, m_TrackOffsetProperty);
                    EditorGUI.BeginChangeCheck();
                    var result = (TrackOffset)EditorGUI.EnumPopup(rect, Styles.OffsetModeTitle, (TrackOffset)m_TrackOffsetProperty.intValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_TrackOffsetProperty.enumValueIndex = (int)result;

                        // this property changes the recordable state of the objects, so auto disable recording
                        if (TimelineWindow.instance != null)
                        {
                            if (TimelineWindow.instance.state != null)
                                TimelineWindow.instance.state.recording = false;
                            RebuildGraph();
                        }
                    }

                    EditorGUI.EndProperty();
                }
            }
        }

        void DrawMatchFieldsGUI()
        {
            if (!AnimatesRootTransform())
                return;

            m_MatchFieldsProperty.isExpanded = EditorGUILayout.Foldout(m_MatchFieldsProperty.isExpanded, Styles.MatchTargetFieldsTitle);
            if (m_MatchFieldsProperty.isExpanded)
            {
                EditorGUI.indentLevel++;
                MatchTargetsFieldGUI(m_MatchFieldsProperty);
                EditorGUI.indentLevel--;
            }
        }

        void DrawRootMotionOffsetFields()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_TrackPositionProperty);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_TrackRotationProperty, Styles.RotationTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (EditorGUI.EndChangeCheck())
            {
                UpdateOffsets();
            }
        }

        void DrawRootMotionToolBar()
        {
            bool disable = targets.Length > 1;
            bool changed = false;

            if (!disable)
            {
                // detects external changes
                changed |= m_lastPosition != m_TrackPositionProperty.vector3Value || m_lastRotation != m_TrackRotationProperty.vector3Value;
                m_lastPosition = m_TrackPositionProperty.vector3Value;
                m_lastRotation = m_TrackRotationProperty.vector3Value;
                SceneView.RepaintAll();
            }

            EditorGUI.BeginChangeCheck();
            using (new EditorGUI.DisabledScope(disable))
                ShowMotionOffsetEditModeToolbar(ref m_OffsetEditMode);
            changed |= EditorGUI.EndChangeCheck();

            if (changed)
            {
                UpdateOffsets();
            }
        }

        void UpdateOffsets()
        {
            foreach (var track in targets.OfType<AnimationTrack>())
                track.UpdateClipOffsets();
            Evaluate();
        }

        void DrawAvatarProperties()
        {
            EditorGUILayout.PropertyField(m_ApplyAvatarMaskProperty);
            if (m_ApplyAvatarMaskProperty.hasMultipleDifferentValues || m_ApplyAvatarMaskProperty.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_AvatarMaskProperty);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
        }

        public static void ShowMotionOffsetEditModeToolbar(ref TimelineAnimationUtilities.OffsetEditMode motionOffset)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();

            int newMotionOffsetMode = GUILayout.Toolbar((int)motionOffset, new[] { Styles.PositionIcon, Styles.RotationIcon });

            if (GUI.changed)
            {
                if ((int)motionOffset == newMotionOffsetMode) //untoggle the button
                    motionOffset = TimelineAnimationUtilities.OffsetEditMode.None;
                else
                    motionOffset = (TimelineAnimationUtilities.OffsetEditMode)newMotionOffsetMode;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            SceneView.duringSceneGui += OnSceneGUI;

            m_MatchFieldsProperty = serializedObject.FindProperty("m_MatchTargetFields");
            m_TrackPositionProperty = serializedObject.FindProperty("m_Position");
            m_TrackRotationProperty = serializedObject.FindProperty("m_EulerAngles");
            m_TrackOffsetProperty = serializedObject.FindProperty("m_TrackOffset");
            m_AvatarMaskProperty = serializedObject.FindProperty("m_AvatarMask");
            m_ApplyAvatarMaskProperty = serializedObject.FindProperty("m_ApplyAvatarMask");
            m_RecordedOffsetPositionProperty = serializedObject.FindProperty("m_InfiniteClipOffsetPosition");
            m_RecordedOffsetEulerProperty = serializedObject.FindProperty("m_InfiniteClipOffsetEulerAngles");

            m_lastPosition = m_TrackPositionProperty.vector3Value;
            m_lastRotation = m_TrackRotationProperty.vector3Value;
        }

        public void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            DoOffsetManipulator();
        }

        void DoOffsetManipulator()
        {
            if (targets.Length > 1) //do not edit the track offset on a multiple selection
                return;

            if (timelineWindow == null || timelineWindow.state == null || timelineWindow.state.editSequence.director == null)
                return;

            AnimationTrack animationTrack = target as AnimationTrack;
            if (animationTrack != null && (animationTrack.trackOffset == TrackOffset.ApplyTransformOffsets) && m_OffsetEditMode != TimelineAnimationUtilities.OffsetEditMode.None)
            {
                var boundObject = TimelineUtility.GetSceneGameObject(timelineWindow.state.editSequence.director, animationTrack);
                var boundObjectTransform = boundObject != null ? boundObject.transform : null;

                var offsets = TimelineAnimationUtilities.GetTrackOffsets(animationTrack, boundObjectTransform);
                EditorGUI.BeginChangeCheck();

                switch (m_OffsetEditMode)
                {
                    case TimelineAnimationUtilities.OffsetEditMode.Translation:
                        offsets.position = Handles.PositionHandle(offsets.position, (Tools.pivotRotation == PivotRotation.Global)
                            ? Quaternion.identity
                            : offsets.rotation);
                        break;
                    case TimelineAnimationUtilities.OffsetEditMode.Rotation:
                        offsets.rotation = Handles.RotationHandle(offsets.rotation, offsets.position);
                        break;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    TimelineUndo.PushUndo(animationTrack, "Inspector");
                    TimelineAnimationUtilities.UpdateTrackOffset(animationTrack, boundObjectTransform, offsets);
                    Evaluate();
                    Repaint();
                }
            }
        }

        public void DrawRecordedOffsetProperties()
        {
            // only show if this applies to all targets
            foreach (var track in targets)
            {
                var animationTrack = track as AnimationTrack;
                if (animationTrack == null || animationTrack.inClipMode || animationTrack.infiniteClip == null || animationTrack.infiniteClip.empty)
                    return;
            }

            GUILayout.Label(Styles.RecordingOffsets);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_RecordedOffsetPositionProperty, Styles.PositionTitle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_RecordedOffsetEulerProperty, Styles.RotationTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        public static void MatchTargetsFieldGUI(SerializedProperty property)
        {
            const float ToggleWidth = 20;
            int value = 0;

            MatchTargetFields enumValue = (MatchTargetFields)property.intValue;

            EditorGUI.BeginChangeCheck();
            Rect rect = EditorGUILayout.GetControlRect(false, kLineHeight * 2);
            Rect itemRect = new Rect(rect.x, rect.y, rect.width, kLineHeight);
            EditorGUI.BeginProperty(rect, Styles.MatchTargetFieldsTitle, property);
            float minWidth = 0, maxWidth = 0;
            EditorStyles.label.CalcMinMaxWidth(Styles.XTitle, out minWidth, out maxWidth);
            float width = minWidth + ToggleWidth;

            GUILayout.BeginHorizontal();
            Rect r = EditorGUI.PrefixLabel(itemRect, Styles.PositionTitle);
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            r.width = width;
            value |= EditorGUI.ToggleLeft(r, Styles.XTitle, enumValue.HasAny(MatchTargetFields.PositionX)) ? (int)MatchTargetFields.PositionX : 0;
            r.x += width;
            value |= EditorGUI.ToggleLeft(r, Styles.YTitle, enumValue.HasAny(MatchTargetFields.PositionY)) ? (int)MatchTargetFields.PositionY : 0;
            r.x += width;
            value |= EditorGUI.ToggleLeft(r, Styles.ZTitle, enumValue.HasAny(MatchTargetFields.PositionZ)) ? (int)MatchTargetFields.PositionZ : 0;
            EditorGUI.indentLevel = oldIndent;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            itemRect.y += kLineHeight;
            r = EditorGUI.PrefixLabel(itemRect, Styles.RotationTitle);
            EditorGUI.indentLevel = 0;
            r.width = width;
            value |= EditorGUI.ToggleLeft(r, Styles.XTitle, enumValue.HasAny(MatchTargetFields.RotationX)) ? (int)MatchTargetFields.RotationX : 0;
            r.x += width;
            value |= EditorGUI.ToggleLeft(r, Styles.YTitle, enumValue.HasAny(MatchTargetFields.RotationY)) ? (int)MatchTargetFields.RotationY : 0;
            r.x += width;
            value |= EditorGUI.ToggleLeft(r, Styles.ZTitle, enumValue.HasAny(MatchTargetFields.RotationZ)) ? (int)MatchTargetFields.RotationZ : 0;
            EditorGUI.indentLevel = oldIndent;
            GUILayout.EndHorizontal();

            EditorGUI.EndProperty();
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = value;
            }
        }

        static TrackOffset GetOffsetMode(AnimationTrack track)
        {
            if (track.isSubTrack)
            {
                var parent = track.parent as AnimationTrack;
                if (parent != null) // fallback to the current track if there is an error
                    track = parent;
            }

            return track.trackOffset;
        }

        // gets the current mode,
        TrackOffset GetOffsetMode(out bool hasMultiple)
        {
            var rootOffsetMode = GetOffsetMode(target as AnimationTrack);
            hasMultiple = targets.OfType<AnimationTrack>().Any(t => GetOffsetMode(t) != rootOffsetMode);
            return rootOffsetMode;
        }
    }
}
