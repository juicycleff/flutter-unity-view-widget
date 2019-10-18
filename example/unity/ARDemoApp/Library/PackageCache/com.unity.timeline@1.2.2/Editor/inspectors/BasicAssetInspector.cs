using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    // Simple inspector used by built in assets
    //  that only need to hide the script field
    class BasicAssetInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();
            bool expanded = true;
            while (property.NextVisible(expanded))
            {
                expanded = false;
                if (SkipField(property.propertyPath))
                    continue;
                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        public virtual void ApplyChanges()
        {
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        static bool SkipField(string fieldName)
        {
            return fieldName == "m_Script";
        }
    }

    [CustomEditor(typeof(ActivationPlayableAsset))]
    class ActivationPlayableAssetInspector : BasicAssetInspector {}
}
