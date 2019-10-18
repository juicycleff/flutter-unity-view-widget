using System;
using System.IO;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;

namespace UnityEditor.XR.Management
{
    internal class XRConfigurationProvider : SettingsProvider
    {
        static readonly GUIContent s_WarningToCreateSettings = EditorGUIUtility.TrTextContent("You must create a serialized instance of the settings data in order to modify the settings in this UI. Until then only default settings set by the provider will be available.");

        Type m_BuildDataType = null;
        string m_BuildSettingsKey;
        Editor m_CachedEditor;
        SerializedObject m_SettingsWrapper;

        public XRConfigurationProvider(string path, string buildSettingsKey, Type buildDataType, SettingsScope scopes = SettingsScope.Project) : base(path, scopes)
        {
            m_BuildDataType = buildDataType;
            m_BuildSettingsKey = buildSettingsKey;
        }

        ScriptableObject currentSettings
        {
            get
            {
                ScriptableObject settings = null;
                EditorBuildSettings.TryGetConfigObject(m_BuildSettingsKey, out settings);
                if (settings == null)
                {
                    string searchText = String.Format("t:{0}", m_BuildDataType.Name);
                    string[] assets = AssetDatabase.FindAssets(searchText);
                    if (assets.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                        settings = AssetDatabase.LoadAssetAtPath(path, m_BuildDataType) as ScriptableObject;
                        EditorBuildSettings.AddConfigObject(m_BuildSettingsKey, settings, true);
                    }
                }
                return settings;
            }
        }

        void InitEditorData(ScriptableObject settings)
        {
            if (settings != null)
            {
                m_SettingsWrapper = new SerializedObject(settings);
                Editor.CreateCachedEditor(settings, null, ref m_CachedEditor);
            }
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            InitEditorData(currentSettings);
        }

        public override void OnDeactivate()
        {
            m_CachedEditor = null;
            m_SettingsWrapper = null;
        }

        public override void OnGUI(string searchContext)
        {
            if (m_SettingsWrapper == null || m_SettingsWrapper.targetObject == null)
            {
                EditorGUILayout.HelpBox(s_WarningToCreateSettings);
                if (GUILayout.Button(EditorGUIUtility.TrTextContent("Create")))
                {
                    ScriptableObject settings = Create();
                    InitEditorData(settings);
                }
            }

            if (m_SettingsWrapper != null  && m_SettingsWrapper.targetObject != null && m_CachedEditor != null)
            {
                m_SettingsWrapper.Update();
                m_CachedEditor.OnInspectorGUI();
                m_SettingsWrapper.ApplyModifiedProperties();
            }
        }

        ScriptableObject Create()
        {
            ScriptableObject settings = ScriptableObject.CreateInstance(m_BuildDataType) as ScriptableObject;
            if (settings != null)
            {
                string newAssetName = String.Format("{0}.asset", EditorUtilities.TypeNameToString(m_BuildDataType));
                string assetPath = EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultSettingsPath);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    assetPath = Path.Combine(assetPath, newAssetName);
                    AssetDatabase.CreateAsset(settings, assetPath);
                    EditorBuildSettings.AddConfigObject(m_BuildSettingsKey, settings, true);
                    return settings;
                }
            }
            return null;
        }
    }
}
