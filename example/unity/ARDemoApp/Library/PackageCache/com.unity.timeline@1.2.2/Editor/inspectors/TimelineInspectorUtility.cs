using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [Flags]
    enum InputEvent
    {
        None = 0,
        DragEnter = 1,
        DragExit = 2,
        Drag = 4,
        KeyboardInput = 8
    }

    static class InputEventMethods
    {
        public static bool InputHasBegun(this InputEvent evt)
        {
            return evt == InputEvent.DragEnter || evt == InputEvent.KeyboardInput;
        }
    }

    static class TimelineInspectorUtility
    {
        internal static class Styles
        {
            public static readonly GUIContent SecondsPrefix = EditorGUIUtility.TrTextContent("s", "Seconds");
            public static readonly GUIContent FramesPrefix = EditorGUIUtility.TrTextContent("f", "Frames");
        }

        public static void TimeField(SerializedProperty property, GUIContent label, bool readOnly, double frameRate, double minValue, double maxValue, ref InputEvent inputEvent)
        {
            var rect = EditorGUILayout.GetControlRect();
            TimeField(rect, property, label, readOnly, frameRate, minValue, maxValue, ref inputEvent);
        }

        // Display Time related properties in frames and seconds
        public static void TimeField(Rect rect, SerializedProperty property, GUIContent label, bool readOnly, double frameRate, double minValue, double maxValue, ref InputEvent inputEvent)
        {
            GUIContent title = EditorGUI.BeginProperty(rect, label, property);
            rect = EditorGUI.PrefixLabel(rect, title);

            int indentLevel = EditorGUI.indentLevel;
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = (int)EditorGUI.kMiniLabelW;
            using (new GUIMixedValueScope(property.hasMultipleDifferentValues))
            {
                var secondsRect = new Rect(rect.xMin, rect.yMin, rect.width / 2 - EditorGUI.kSpacingSubLabel, rect.height);
                var framesRect = new Rect(rect.xMin + rect.width / 2, rect.yMin, rect.width / 2, rect.height);

                if (readOnly)
                {
                    EditorGUI.FloatField(secondsRect, Styles.SecondsPrefix, (float)property.doubleValue, EditorStyles.label);
                }
                else
                {
                    EditorGUI.BeginChangeCheck();
                    DelayedAndDraggableDoubleField(secondsRect, Styles.SecondsPrefix, property, ref inputEvent);
                    if (EditorGUI.EndChangeCheck())
                    {
                        property.doubleValue = Clamp(property.doubleValue, minValue, maxValue);
                    }
                }

                if (frameRate > TimeUtility.kTimeEpsilon)
                {
                    EditorGUI.BeginChangeCheck();

                    double time = property.doubleValue;
                    int frames = TimeUtility.ToFrames(time, frameRate);
                    double exactFrames = TimeUtility.ToExactFrames(time, frameRate);
                    bool useIntField = TimeUtility.OnFrameBoundary(time, frameRate);

                    if (readOnly)
                    {
                        if (useIntField)
                            EditorGUI.IntField(framesRect, Styles.FramesPrefix, frames, EditorStyles.label);
                        else
                            EditorGUI.DoubleField(framesRect, Styles.FramesPrefix, exactFrames, EditorStyles.label);
                    }
                    else
                    {
                        if (useIntField)
                        {
                            int newFrames = DelayedAndDraggableIntField(framesRect, Styles.FramesPrefix, frames, ref inputEvent);
                            time = Math.Max(0, TimeUtility.FromFrames(newFrames, frameRate));
                        }
                        else
                        {
                            double newExactFrames = DelayedAndDraggableDoubleField(framesRect, Styles.FramesPrefix, exactFrames, ref inputEvent);
                            time = Math.Max(0, TimeUtility.FromFrames((int)Math.Floor(newExactFrames), frameRate));
                        }
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.doubleValue = Clamp(time, minValue, maxValue);
                    }
                }

                EditorGUI.indentLevel = indentLevel;
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.EndProperty();
            }
        }

        public static double TimeFieldUsingTimeReference(
            GUIContent label, double time, bool readOnly, bool showMixed, double frameRate, double minValue,
            double maxValue, ref InputEvent inputEvent)
        {
            var state = TimelineWindow.instance.state;
            var needsTimeConversion = state != null && state.timeReferenceMode == TimeReferenceMode.Global;

            if (needsTimeConversion)
                time = state.editSequence.ToGlobalTime(time);

            var t = TimeField(label, time, readOnly, showMixed, frameRate, minValue, maxValue, ref inputEvent);

            if (needsTimeConversion)
                t = state.editSequence.ToLocalTime(t);

            return t;
        }

        public static double DurationFieldUsingTimeReference(
            GUIContent label, double start, double end, bool readOnly, bool showMixed, double frameRate,
            double minValue, double maxValue, ref InputEvent inputEvent)
        {
            var state = TimelineWindow.instance.state;
            var needsTimeConversion = state != null && state.timeReferenceMode == TimeReferenceMode.Global;

            if (needsTimeConversion)
            {
                start = state.editSequence.ToGlobalTime(start);
                end = state.editSequence.ToGlobalTime(end);
            }

            var duration = end - start;

            var t = TimeField(label, duration, readOnly, showMixed, frameRate, minValue, maxValue, ref inputEvent);

            end = start + t;

            if (needsTimeConversion)
            {
                start = state.editSequence.ToLocalTime(start);
                end = state.editSequence.ToLocalTime(end);
            }

            return end - start;
        }

        public static double TimeField(Rect rect, GUIContent label, double time, bool readOnly, bool showMixed, double frameRate, double minValue, double maxValue, ref InputEvent inputEvent)
        {
            EditorGUILayout.BeginHorizontal(label, GUIStyle.none);
            rect = EditorGUI.PrefixLabel(rect, label);

            int indentLevel = EditorGUI.indentLevel;
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = (int)EditorGUI.kMiniLabelW;
            using (new GUIMixedValueScope(showMixed))
            {
                var secondsRect = new Rect(rect.xMin, rect.yMin, rect.width / 2 - EditorGUI.kSpacingSubLabel, rect.height);
                var framesRect = new Rect(rect.xMin + rect.width / 2, rect.yMin, rect.width / 2, rect.height);

                if (readOnly)
                {
                    EditorGUI.FloatField(secondsRect, Styles.SecondsPrefix, (float)time, EditorStyles.label);
                }
                else
                {
                    time = DelayedAndDraggableDoubleField(secondsRect, Styles.SecondsPrefix, time, ref inputEvent);
                }

                if (frameRate > TimeUtility.kTimeEpsilon)
                {
                    int frames = TimeUtility.ToFrames(time, frameRate);
                    double exactFrames = TimeUtility.ToExactFrames(time, frameRate);
                    bool useIntField = TimeUtility.OnFrameBoundary(time, frameRate);
                    if (readOnly)
                    {
                        if (useIntField)
                            EditorGUI.IntField(framesRect, Styles.FramesPrefix, frames, EditorStyles.label);
                        else
                            EditorGUI.FloatField(framesRect, Styles.FramesPrefix, (float)exactFrames, EditorStyles.label);
                    }
                    else
                    {
                        double newTime;
                        EditorGUI.BeginChangeCheck();
                        if (useIntField)
                        {
                            int newFrames = DelayedAndDraggableIntField(framesRect, Styles.FramesPrefix, frames, ref inputEvent);
                            newTime = Math.Max(0, TimeUtility.FromFrames(newFrames, frameRate));
                        }
                        else
                        {
                            double newExactFrames = DelayedAndDraggableDoubleField(framesRect, Styles.FramesPrefix, exactFrames, ref inputEvent);
                            newTime = Math.Max(0, TimeUtility.FromFrames((int)Math.Floor(newExactFrames), frameRate));
                        }

                        if (EditorGUI.EndChangeCheck())
                        {
                            time = newTime;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = indentLevel;
                EditorGUIUtility.labelWidth = labelWidth;
            }

            return Clamp(time, minValue, maxValue);
        }

        public static double TimeField(GUIContent label, double time, bool readOnly, bool showMixed, double frameRate, double minValue, double maxValue, ref InputEvent inputEvent)
        {
            var rect = EditorGUILayout.GetControlRect();
            return TimeField(rect, label, time, readOnly, showMixed, frameRate, minValue, maxValue, ref inputEvent);
        }

        static InputEvent InputEventType(Rect rect, int id)
        {
            var evt = Event.current;
            switch (evt.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    if (rect.Contains(evt.mousePosition) && evt.button == 0)
                    {
                        return InputEvent.DragEnter;
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        return InputEvent.DragExit;
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        return InputEvent.Drag;
                    }
                    break;
                case EventType.KeyDown:
                    if (GUIUtility.hotControl == id && evt.keyCode == KeyCode.Escape)
                    {
                        return InputEvent.DragExit;
                    }
                    break;
            }
            return InputEvent.None;
        }

        static double DelayedAndDraggableDoubleField(Rect rect, GUIContent label, double value, ref InputEvent inputEvent, double dragSensitivity)
        {
            var id = GUIUtility.GetControlID(FocusType.Keyboard);
            var fieldRect = EditorGUI.PrefixLabel(rect, id, label);
            rect.xMax = fieldRect.x;

            double refValue = value;
            long dummy = 0;

            inputEvent |= InputEventType(rect, id);

            EditorGUI.DragNumberValue(rect, id, true, ref refValue, ref dummy, dragSensitivity);

            EditorGUI.BeginChangeCheck();
            var result = EditorGUI.DelayedDoubleFieldInternal(fieldRect, GUIContent.none, refValue, EditorStyles.numberField);
            if (EditorGUI.EndChangeCheck())
                inputEvent |= InputEvent.KeyboardInput;

            return result;
        }

        static int DelayedAndDraggableIntField(Rect rect, GUIContent label, int value, ref InputEvent inputEvent, long dragSensitivity)
        {
            var id = GUIUtility.GetControlID(FocusType.Keyboard);
            var fieldRect = EditorGUI.PrefixLabel(rect, id, label);
            rect.xMax = fieldRect.x;

            double dummy = 0.0;
            long refValue = value;

            inputEvent |= InputEventType(rect, id);

            EditorGUI.DragNumberValue(rect, id, false, ref dummy, ref refValue, dragSensitivity);

            EditorGUI.BeginChangeCheck();
            var result = EditorGUI.DelayedIntFieldInternal(fieldRect, GUIContent.none, (int)refValue, EditorStyles.numberField);
            if (EditorGUI.EndChangeCheck())
                inputEvent |= InputEvent.KeyboardInput;

            return result;
        }

        internal static double DelayedAndDraggableDoubleField(GUIContent label, double value, ref InputEvent action, double dragSensitivity)
        {
            var r = EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, EditorGUI.kSingleLineHeight);
            return DelayedAndDraggableDoubleField(r, label, value, ref action, dragSensitivity);
        }

        static void DelayedAndDraggableDoubleField(Rect rect, GUIContent label, SerializedProperty property, ref InputEvent inputEvent)
        {
            EditorGUI.BeginChangeCheck();
            var newValue = DelayedAndDraggableDoubleField(rect, label, property.doubleValue, ref inputEvent);
            if (EditorGUI.EndChangeCheck())
                property.doubleValue = newValue;
        }

        static double DelayedAndDraggableDoubleField(Rect rect, GUIContent label, double value, ref InputEvent inputEvent)
        {
            var dragSensitivity = NumericFieldDraggerUtility.CalculateFloatDragSensitivity(value);
            return DelayedAndDraggableDoubleField(rect, label, value, ref inputEvent, dragSensitivity);
        }

        static int DelayedAndDraggableIntField(Rect rect, GUIContent label, int value, ref InputEvent inputEvent)
        {
            var dragSensitivity = NumericFieldDraggerUtility.CalculateIntDragSensitivity(value);
            return DelayedAndDraggableIntField(rect, label, value, ref inputEvent, dragSensitivity);
        }

        internal static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static Editor GetInspectorForObjects(UnityEngine.Object[] objects)
        {
            // create cached editor throws on assembly reload...
            try
            {
                if (objects.Any(x => x != null))
                {
                    var director = TimelineWindow.instance.state.editSequence.director;
                    return Editor.CreateEditorWithContext(objects, director, null);
                }
            }
            catch (Exception)
            {}

            return null;
        }
    }
}
