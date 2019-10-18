using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(TimelineAsset)), CanEditMultipleObjects]
    class TimelineAssetInspector : Editor
    {
        static class Styles
        {
            public static readonly GUIContent FrameRate = EditorGUIUtility.TrTextContent("Frame Rate", "The frame rate at which this sequence updates");
            public static readonly GUIContent DurationMode = EditorGUIUtility.TrTextContent("Duration Mode", "Specified how the duration of the sequence is calculated");
            public static readonly GUIContent Duration = EditorGUIUtility.TrTextContent("Duration", "The length of the sequence");
            public static readonly GUIContent HeaderTitleMultiselection = EditorGUIUtility.TrTextContent("Timeline Assets");
        }

        SerializedProperty m_FrameRateProperty;
        SerializedProperty m_DurationModeProperty;
        SerializedProperty m_FixedDurationProperty;

        void InitializeProperties()
        {
            m_FrameRateProperty = serializedObject.FindProperty("m_EditorSettings").FindPropertyRelative("m_Framerate");
            m_DurationModeProperty = serializedObject.FindProperty("m_DurationMode");
            m_FixedDurationProperty = serializedObject.FindProperty("m_FixedDuration");
        }

        public void OnEnable()
        {
            InitializeProperties();
        }

        internal override bool IsEnabled()
        {
            return !FileUtil.HasReadOnly(targets) && base.IsEnabled();
        }

        protected override void OnHeaderGUI()
        {
            string headerTitle;
            if (targets.Length == 1)
                headerTitle = target.name;
            else
                headerTitle = targets.Length.ToString() + " " + Styles.HeaderTitleMultiselection.text;

            DrawHeaderGUI(this, headerTitle, 0);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_FrameRateProperty, Styles.FrameRate, GUILayout.MinWidth(140));
            var frameRate = m_FrameRateProperty.doubleValue;

            EditorGUILayout.PropertyField(m_DurationModeProperty, Styles.DurationMode, GUILayout.MinWidth(140));

            var durationMode = (TimelineAsset.DurationMode)m_DurationModeProperty.enumValueIndex;
            var inputEvent = InputEvent.None;
            if (durationMode == TimelineAsset.DurationMode.FixedLength)
                TimelineInspectorUtility.TimeField(m_FixedDurationProperty, Styles.Duration, false, frameRate, double.Epsilon, TimelineClip.kMaxTimeValue * 2, ref inputEvent);
            else
            {
                var isMixed = targets.Length > 1;
                TimelineInspectorUtility.TimeField(Styles.Duration, ((TimelineAsset)target).duration, true, isMixed, frameRate, double.MinValue, double.MaxValue, ref inputEvent);
            }

            bool changed = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            if (changed)
                TimelineWindow.RepaintIfEditingTimelineAsset((TimelineAsset)target);
        }
    }
}
