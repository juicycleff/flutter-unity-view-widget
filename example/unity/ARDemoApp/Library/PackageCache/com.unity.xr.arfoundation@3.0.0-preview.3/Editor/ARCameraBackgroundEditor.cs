using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UnityEditor.XR.ARFoundation
{
    [CustomEditor(typeof(ARCameraBackground))]
    internal class ARCameraBackgroundEditor : Editor
    {
        SerializedProperty m_UseCustomMaterial;
        SerializedProperty m_CustomMaterial;

        static class Tooltips
        {
            public static readonly GUIContent useCustomMaterial = new GUIContent(
                "Use Custom Material",
                "When false, a material is generated automatically from the shader included in the platform-specific package. When true, the Custom Material is used instead, overriding the automatically generated one. This is not necessary for most AR experiences.");

            public static readonly GUIContent customMaterial = new GUIContent(
                "Custom Material",
                "The material to use for background rendering.");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_UseCustomMaterial, Tooltips.useCustomMaterial);

            if (m_UseCustomMaterial.boolValue)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_CustomMaterial, Tooltips.customMaterial);
                --EditorGUI.indentLevel;
            }

            serializedObject.ApplyModifiedProperties();
        }

        void OnEnable()
        {
            m_UseCustomMaterial = serializedObject.FindProperty("m_UseCustomMaterial");
            m_CustomMaterial = serializedObject.FindProperty("m_CustomMaterial");
        }
    }
}
