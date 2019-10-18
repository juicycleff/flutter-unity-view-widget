using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomPropertyDrawer(typeof(TimeFieldAttribute), true)]
    class TimeFieldDrawer : PropertyDrawer
    {
        static WindowState state
        {
            get { return TimelineWindow.instance != null ? TimelineWindow.instance.state : null; }
        }

        static float currentFrameRate
        {
            get { return state != null ? TimelineWindow.instance.state.referenceSequence.frameRate : 0.0f; }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Float)
            {
                GUILayout.Label("TimeField only works on floating point types");
                return;
            }

            var timeFieldAttribute = attribute as TimeFieldAttribute;
            if (timeFieldAttribute == null)
                return;

            var rect = EditorGUILayout.s_LastRect;
            EditorGUI.BeginChangeCheck();

            if (timeFieldAttribute.useEditMode == TimeFieldAttribute.UseEditMode.ApplyEditMode)
                TimeFieldWithEditMode(rect, property, label);
            else
                TimeField(rect, property, label);

            if (EditorGUI.EndChangeCheck())
            {
                if (state != null)
                    state.Refresh();
            }
        }

        static void TimeField(Rect rect, SerializedProperty property, GUIContent label)
        {
            var evt1 = InputEvent.None;
            TimelineInspectorUtility.TimeField(rect, property, label, false, currentFrameRate, 0, float.MaxValue, ref evt1);
        }

        static void TimeFieldWithEditMode(Rect rect, SerializedProperty property, GUIContent label)
        {
            double minStartTime;
            if (property.hasMultipleDifferentValues)
                minStartTime = SelectionManager.SelectedItems().Min(i => i.start);
            else
                minStartTime = property.doubleValue;

            var evt = InputEvent.None;
            var newValue = TimelineInspectorUtility.TimeField(
                rect, label, minStartTime, false, property.hasMultipleDifferentValues, currentFrameRate, 0.0, float.MaxValue, ref evt);

            EditMode.inputHandler.ProcessMove(evt, newValue);
        }
    }
}
