using NUnit.Framework;

using System;
using System.Collections;
using System.IO;

using UnityEditor;
using UnityEditor.XR.Management;

using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.XR.Management;


namespace UnityEditor.XR.Management.Tests
{

    class XRGeneralSettingsTests
    {
        internal static readonly string[] s_TempSettingsPath = {"Temp", "Test", "Settings"};

        string testPathToSettings;

        UnityEngine.Object currentSettings = null;

        XRManagerSettings testManager = null;
        XRGeneralSettings testSettings = null;

        [SetUp]
        public void SetupTest()
        {
            testManager = ScriptableObject.CreateInstance<XRManagerSettings>();

            testSettings = ScriptableObject.CreateInstance<XRGeneralSettings>() as XRGeneralSettings;
            testSettings.Manager = testManager;

            testPathToSettings = XRGeneralSettingsTests.GetAssetPathForComponents(XRGeneralSettingsTests.s_TempSettingsPath);
            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                testPathToSettings = Path.Combine(testPathToSettings, "Test_XRGeneralSettings.asset");
                AssetDatabase.CreateAsset(testSettings, testPathToSettings);
                AssetDatabase.SaveAssets();
            }

            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out currentSettings);
            EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, testSettings, true);
        }

        [TearDown]
        public void TearDownTest()
        {
            EditorBuildSettings.RemoveConfigObject(XRGeneralSettings.k_SettingsKey);

            if (!string.IsNullOrEmpty(testPathToSettings))
            {
                AssetDatabase.DeleteAsset(testPathToSettings);
            }

            testSettings.Manager = null;
            UnityEngine.Object.DestroyImmediate(testSettings);
            testSettings = null;

            UnityEngine.Object.DestroyImmediate(testManager);
            testManager = null;

            if (currentSettings != null)
                EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, currentSettings, true);

            AssetDatabase.DeleteAsset(Path.Combine("Assets","Temp"));
        }


        [Test]
        public void UpdateGeneralSettings_ToPerBuildTargetSettings()
        {
            bool success = XRGeneralSettingsUpgrade.UpgradeSettingsToPerBuildTarget(testPathToSettings);
            Assert.IsTrue(success);

            XRGeneralSettingsPerBuildTarget pbtgs = null;

            pbtgs = AssetDatabase.LoadAssetAtPath(testPathToSettings, typeof(XRGeneralSettingsPerBuildTarget)) as XRGeneralSettingsPerBuildTarget;
            Assert.IsNotNull(pbtgs);

            var settings = pbtgs.SettingsForBuildTarget(EditorUserBuildSettings.selectedBuildTargetGroup);
            Assert.IsNotNull(settings);
            Assert.IsNotNull(settings.Manager);
            Assert.AreEqual(testManager, settings.Manager);
        }

        internal static string GetAssetPathForComponents(string[] pathComponents, string root = "Assets")
        {
            if (pathComponents.Length <= 0)
                return null;

            string path = root;
            foreach( var pc in pathComponents)
            {
                string subFolder = Path.Combine(path, pc);
                bool shouldCreate = true;
                foreach (var f in AssetDatabase.GetSubFolders(path))
                {
                    if (String.Compare(Path.GetFullPath(f), Path.GetFullPath(subFolder), true) == 0)
                    {
                        shouldCreate = false;
                        break;
                    }
                }

                if (shouldCreate)
                    AssetDatabase.CreateFolder(path, pc);
                path = subFolder;
            }

            return path;
        }

    }
}
