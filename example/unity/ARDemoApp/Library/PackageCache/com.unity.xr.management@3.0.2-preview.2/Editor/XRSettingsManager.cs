using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Management;

namespace UnityEditor.XR.Management
{
    class XRSettingsManager : SettingsProvider
    {
        static string s_SettingsRootTitle = "Project/XR Plugin Management";
        static XRSettingsManager s_SettingsManager = null;

        static GUIContent s_LoaderInitOnStartLabel = new GUIContent("Initialize on Startup");
        static GUIContent s_SettingsDetailsFoldout = new GUIContent("Details");

        SerializedObject m_SettingsWrapper;

        private Dictionary<BuildTargetGroup, Editor> CachedSettingsEditor = new Dictionary<BuildTargetGroup, Editor>();

        static XRGeneralSettingsPerBuildTarget currentSettings
        {
            get
            {
                XRGeneralSettingsPerBuildTarget generalSettings = null;
                EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out generalSettings);
                if (generalSettings == null)
                {
                    lock(s_SettingsManager)
                    {
                        EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out generalSettings);
                        if (generalSettings == null)
                        {
                            string searchText = "t:XRGeneralSettings";
                            string[] assets = AssetDatabase.FindAssets(searchText);
                            if (assets.Length > 0)
                            {
                                string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                                generalSettings = AssetDatabase.LoadAssetAtPath(path, typeof(XRGeneralSettingsPerBuildTarget)) as XRGeneralSettingsPerBuildTarget;
                            }
                        }

                        if (generalSettings == null)
                        {
                            generalSettings = ScriptableObject.CreateInstance(typeof(XRGeneralSettingsPerBuildTarget)) as XRGeneralSettingsPerBuildTarget;
                            string assetPath = EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultGeneralSettingsPath);
                            if (!string.IsNullOrEmpty(assetPath))
                            {
                                assetPath = Path.Combine(assetPath, "XRGeneralSettings.asset");
                                AssetDatabase.CreateAsset(generalSettings, assetPath);
                            }
                        }

                        EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, generalSettings, true);

                    }
                }
                return generalSettings;
            }
        }

        [UnityEngine.Internal.ExcludeFromDocs]
        XRSettingsManager(string path, SettingsScope scopes = SettingsScope.Project) : base(path, scopes)
        {
        }

        [SettingsProvider]
        [UnityEngine.Internal.ExcludeFromDocs]
        static SettingsProvider Create()
        {
            if (s_SettingsManager == null)
            {
                s_SettingsManager = new XRSettingsManager(s_SettingsRootTitle);
            }

            return s_SettingsManager;
        }

        [SettingsProviderGroup]
        [UnityEngine.Internal.ExcludeFromDocs]
        static SettingsProvider[] CreateAllChildSettingsProviders()
        {
            List<SettingsProvider> ret = new List<SettingsProvider>();
            if (s_SettingsManager != null)
            {
                var ats = TypeLoaderExtensions.GetAllTypesWithAttribute<XRConfigurationDataAttribute>();
                foreach (var at in ats)
                {
                    XRConfigurationDataAttribute xrbda = at.GetCustomAttributes(typeof(XRConfigurationDataAttribute), true)[0] as XRConfigurationDataAttribute;
                    string settingsPath = String.Format("{1}/{0}", xrbda.displayName, s_SettingsRootTitle);
                    var resProv = new XRConfigurationProvider(settingsPath, xrbda.buildSettingsKey, at);
                    ret.Add(resProv);
                }
            }

            // LIH Package Provider
            string settingsPathLIH = String.Format("{1}/{0}", "Input Helpers", s_SettingsRootTitle);
            var lihProv = new InputHelpersConfigurationProvider(settingsPathLIH);
            ret.Add(lihProv);
            return ret.ToArray();
        }

        void InitEditorData(ScriptableObject settings)
        {
            if (settings != null)
            {
                m_SettingsWrapper = new SerializedObject(settings);
            }
        }


        /// <summary>See <see href="https://docs.unity3d.com/ScriptReference/SettingsProvider.html">SettingsProvider documentation</see>.</summary>
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            InitEditorData(currentSettings);
        }

        /// <summary>See <see href="https://docs.unity3d.com/ScriptReference/SettingsProvider.html">SettingsProvider documentation</see>.</summary>
        public override void OnDeactivate()
        {
            m_SettingsWrapper = null;

            CachedSettingsEditor.Clear();
        }



        /// <summary>See <see href="https://docs.unity3d.com/ScriptReference/SettingsProvider.html">SettingsProvider documentation</see>.</summary>
        public override void OnGUI(string searchContext)
        {
            if (m_SettingsWrapper != null  && m_SettingsWrapper.targetObject != null)
            {
                m_SettingsWrapper.Update();

                BuildTargetGroup buildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();

                XRGeneralSettings settings = currentSettings.SettingsForBuildTarget(buildTargetGroup);
                if (settings == null)
                {
                    settings = ScriptableObject.CreateInstance<XRGeneralSettings>() as XRGeneralSettings;
                    currentSettings.SetSettingsForBuildTarget(buildTargetGroup, settings);
                    settings.name = $"{buildTargetGroup.ToString()} Settings";
                    AssetDatabase.AddObjectToAsset(settings, AssetDatabase.GetAssetOrScenePath(currentSettings));
                }

                var serializedSettingsObject = new SerializedObject(settings);
                serializedSettingsObject.Update();

                SerializedProperty initOnStart = serializedSettingsObject.FindProperty("m_InitManagerOnStart");
                EditorGUILayout.PropertyField(initOnStart, s_LoaderInitOnStartLabel);

                SerializedProperty loaderProp = serializedSettingsObject.FindProperty("m_LoaderManagerInstance");

                if (!CachedSettingsEditor.ContainsKey(buildTargetGroup))
                {
                    CachedSettingsEditor.Add(buildTargetGroup, null);
                }

                if (loaderProp.objectReferenceValue == null)
                {
                    var xrManagerSettings = ScriptableObject.CreateInstance<XRManagerSettings>() as XRManagerSettings;
                    xrManagerSettings.name = $"{buildTargetGroup.ToString()} Loaders";
                    AssetDatabase.AddObjectToAsset(xrManagerSettings, AssetDatabase.GetAssetOrScenePath(currentSettings));
                    loaderProp.objectReferenceValue = xrManagerSettings;
                    serializedSettingsObject.ApplyModifiedProperties();
                }

                var obj = loaderProp.objectReferenceValue;

                if (obj != null)
                {
                    loaderProp.objectReferenceValue = obj;

                    if(CachedSettingsEditor[buildTargetGroup] == null)
                    {
                        CachedSettingsEditor[buildTargetGroup] = Editor.CreateEditor(obj);

                        if (CachedSettingsEditor[buildTargetGroup] == null)
                        {
                            Debug.LogError("Failed to create a view for XR Manager Settings Instance");
                        }
                    }

                    if (CachedSettingsEditor[buildTargetGroup] != null)
                    {
                        CachedSettingsEditor[buildTargetGroup].OnInspectorGUI();
                    }
                }
                else if (obj == null)
                {
                    settings.AssignedSettings = null;
                    loaderProp.objectReferenceValue = null;
                }

                serializedSettingsObject.ApplyModifiedProperties();

                EditorGUILayout.EndBuildTargetSelectionGrouping();

                m_SettingsWrapper.ApplyModifiedProperties();
            }

            base.OnGUI(searchContext);
        }
    }
}

