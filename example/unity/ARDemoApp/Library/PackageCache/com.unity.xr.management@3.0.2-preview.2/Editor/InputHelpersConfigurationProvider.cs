using System;
using System.IO;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.XR.Management;

namespace UnityEditor.XR.Management
{
    internal class InputHelpersConfigurationProvider : SettingsProvider
    {
        const string s_LIHReasonText = "It is required that the Tracked Pose Driver be used to enable a game camera to follow an XR device. The Tracked Pose Driver component is part of the com.unity.xr.legacyinputhelpers package. This package is not currently installed. Please press the button below to install the package.";

        static GUIContent s_InstallLIHLabel = new GUIContent("Install Legacy Input Helpers Package");

        string m_LegacyInputHelpersInstalledVersion;

        private readonly int m_CheckCount = 1000;
        private bool m_HasLIHPackage = false;
        private int m_Count = 1000;
        PackageManager.Requests.ListRequest m_LIHSearchRequest = null;

        private readonly string s_LegacyInputHelpersPackage = "com.unity.xr.legacyinputhelpers@1.*";
        private readonly string s_LegacyInputHelpersPackageName = "com.unity.xr.legacyinputhelpers";

        public InputHelpersConfigurationProvider(string path, SettingsScope scopes = SettingsScope.Project) : base(path, scopes)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
           HasLegacyInputHelpersInstalled(true);
        }

        public override void OnDeactivate()
        {
        }

        public override void OnGUI(string searchContext)
        {
            
            // show information relating to the legacy input helpers
            if (!HasLegacyInputHelpersInstalled())
            {
                EditorGUILayout.Separator();

                EditorGUILayout.HelpBox(s_LIHReasonText, MessageType.Error);

                if (GUILayout.Button(s_InstallLIHLabel))
                {
                    var ret = PackageManager.Client.Add(s_LegacyInputHelpersPackage);
                }
            }
            else
            {
                EditorGUILayout.Separator();

                EditorGUILayout.HelpBox(m_LegacyInputHelpersInstalledVersion, MessageType.Info);
            }
        }

        private bool HasLegacyInputHelpersInstalled(bool forceLookup = false)
        {
            if (m_LIHSearchRequest == null || forceLookup || (m_Count >= m_CheckCount && !m_HasLIHPackage))
            {
                m_LIHSearchRequest = PackageManager.Client.List();
                m_Count = 0;
            }
            else
            {
                m_Count++;
            }
            if (m_LIHSearchRequest != null && m_LIHSearchRequest.IsCompleted)
            {
                m_HasLIHPackage = false;
                PackageManager.PackageCollection packageList = m_LIHSearchRequest.Result;
                foreach (var info in packageList)
                {
                    if (info.name == s_LegacyInputHelpersPackageName)
                    {
                        if (info.status == PackageManager.PackageStatus.Available)
                        {
                            m_HasLIHPackage = true;
                            m_LegacyInputHelpersInstalledVersion = string.Format("{0} version {1} is installed", info.name, info.version);
                        }
                        break;
                    }
                }
                m_LIHSearchRequest = null;
            }
            return m_HasLIHPackage;
        }

    }
}
