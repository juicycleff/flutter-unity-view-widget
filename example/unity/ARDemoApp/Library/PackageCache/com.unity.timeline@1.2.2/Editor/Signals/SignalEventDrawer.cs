using System;
using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Signals
{
    [CustomPropertyDrawer(typeof(CustomSignalEventDrawer))]
    [UsedImplicitly]
    class SignalEventDrawer : UnityEventDrawer
    {
        static GameObject FindBoundObject(SerializedProperty property)
        {
            var component = property.serializedObject.targetObject as Component;
            return component != null ? component.gameObject : null;
        }

        protected override void OnAddEvent(ReorderableList list)
        {
            base.OnAddEvent(list);
            var listProperty = list.serializedProperty;
            if (listProperty.arraySize > 0)
            {
                var lastCall = list.serializedProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                var targetProperty = lastCall.FindPropertyRelative(kInstancePath);
                targetProperty.objectReferenceValue = FindBoundObject(listProperty);
            }
        }

        protected override void DrawEventHeader(Rect headerRect) {}

        protected override void SetupReorderableList(ReorderableList list)
        {
            base.SetupReorderableList(list);
            list.headerHeight = 4;
        }
    }
}
