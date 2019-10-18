using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor.XR.ARCore
{
    public static class ARCoreSettingsProvider
    {
        [SettingsProvider]
        static SettingsProvider CreateSettingsProvider()
        {
            GUIContent s_WarningToCreateSettings = EditorGUIUtility.TrTextContent(
                "This controls the Build Settings for ARCore.\n\nYou must create a serialized instance of the settings data in order to modify the settings in this UI. Until then only default settings set by the provider will be available.");

            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/XR/ARCore", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "ARCore Build Settings",

                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    if (ARCoreSettings.currentSettings == null)
                    {
                        EditorGUILayout.HelpBox(s_WarningToCreateSettings);
                        if (GUILayout.Button(EditorGUIUtility.TrTextContent("Create")))
                        {
                            Create();
                        }
                        else
                        {
                            return;
                        }
                    }

                    var serializedSettings = ARCoreSettings.GetSerializedSettings();

                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("m_Requirement"), new GUIContent(
                        "Requirement",
                        "Toggles whether ARCore is required for this app. This will make the app only downloadable by devices with ARCore support if set to 'Required'."));

                    serializedSettings.ApplyModifiedProperties();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "ARCore", "optional", "required" })
            };

            return provider;
        }

        static void Create()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save ARCore Settings", "ARCoreSettings", "asset", "Please enter a filename to save the ARCore settings.");
            if (string.IsNullOrEmpty(path))
                return;

            var settings = ScriptableObject.CreateInstance<ARCoreSettings>();
            AssetDatabase.CreateAsset(settings, path);
            ARCoreSettings.currentSettings = settings;
        }
    }
}
