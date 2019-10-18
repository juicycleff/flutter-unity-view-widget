using UnityEditor;
using UnityEditor.XR.Management;

using UnityEngine;

namespace Samples
{
    /// <summary>
    /// Simple custom editor used to show how to enable custom UI for XR Management
    /// configuraton data.
    /// </summary>
    [CustomEditor(typeof(SampleSettings))]
    public class SampleSettingsEditor : Editor
    {
        static string k_RequiresProperty = "m_RequiresItem";
        static string k_RuntimeToggleProperty  = "m_RuntimeToggle";

        static GUIContent k_ShowBuildSettingsLabel = new GUIContent("Build Settings");
        static GUIContent k_RequiresLabel = new GUIContent("Item Requirement");

        static GUIContent k_ShowRuntimeSettingsLabel = new GUIContent("Runtime Settings");
        static GUIContent k_RuntimeToggleLabel = new GUIContent("Should I stay or should I go?");

        bool m_ShowBuildSettings = true;
        bool m_ShowRuntimeSettings = true;

        SerializedProperty m_RequiesItemProperty;
        SerializedProperty m_RuntimeToggleProperty;

        /// <summary>Override of Editor callback.</summary>
        public override void OnInspectorGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
                return;

            if (m_RequiesItemProperty == null) m_RequiesItemProperty = serializedObject.FindProperty(k_RequiresProperty);
            if (m_RuntimeToggleProperty == null) m_RuntimeToggleProperty = serializedObject.FindProperty(k_RuntimeToggleProperty);

            serializedObject.Update();
            m_ShowBuildSettings = EditorGUILayout.Foldout(m_ShowBuildSettings, k_ShowBuildSettingsLabel);
            if (m_ShowBuildSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_RequiesItemProperty, k_RequiresLabel);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            m_ShowRuntimeSettings = EditorGUILayout.Foldout(m_ShowRuntimeSettings, k_ShowRuntimeSettingsLabel);
            if (m_ShowRuntimeSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_RuntimeToggleProperty, k_RuntimeToggleLabel);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
