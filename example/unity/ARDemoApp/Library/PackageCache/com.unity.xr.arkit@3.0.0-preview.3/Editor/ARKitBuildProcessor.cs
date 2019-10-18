#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARKit;

using OSVersion = UnityEngine.XR.ARKit.OSVersion;

namespace UnityEditor.XR.ARKit
{
    internal class ARKitBuildProcessor
    {
        public static IEnumerable<T> AssetsOfType<T>() where T : UnityEngine.Object
        {
            foreach(var guid in AssetDatabase.FindAssets("t:" + typeof(T).Name))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                yield return AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }

        class PostProcessor : IPostprocessBuildWithReport
        {
            public int callbackOrder { get { return 0; } }

            public void OnPostprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                {
                    return;
                }

                BuildHelper.RemoveShaderFromProject(ARKitCameraSubsystem.backgroundShaderName);
                HandleARKitRequiredFlag(report.summary.outputPath);
            }

            static void HandleARKitRequiredFlag(string pathToBuiltProject)
            {
                var arkitSettings = ARKitSettings.GetOrCreateSettings();
                string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
                PlistElementDict rootDict = plist.root;

                // Get or create array to manage device capabilities
                const string capsKey = "UIRequiredDeviceCapabilities";
                PlistElementArray capsArray;
                PlistElement pel;
                if (rootDict.values.TryGetValue(capsKey, out pel))
                {
                    capsArray = pel.AsArray();
                }
                else
                {
                    capsArray = rootDict.CreateArray(capsKey);
                }
                // Remove any existing "arkit" plist entries
                const string arkitStr = "arkit";
                capsArray.values.RemoveAll(x => arkitStr.Equals(x.AsString()));
                if (arkitSettings.requirement == ARKitSettings.Requirement.Required)
                {
                    // Add "arkit" plist entry
                    capsArray.AddString(arkitStr);
                }

                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }

        class Preprocessor : IPreprocessBuildWithReport
        {
            // Magic value according to
            // https://docs.unity3d.com/ScriptReference/PlayerSettings.GetArchitecture.html
            // "0 - None, 1 - ARM64, 2 - Universal."
            const int k_TargetArchitectureArm64 = 1;

            void SelectStaticLib()
            {
                const string pluginPath = "Packages/com.unity.xr.arkit/Runtime/iOS";
                LibUtil.SelectPlugin(
                    PluginImporter.GetAtPath($"{pluginPath}/Xcode1000/UnityARKit.a") as PluginImporter,
                    PluginImporter.GetAtPath($"{pluginPath}/Xcode1100/UnityARKit.a") as PluginImporter);
            }

            public void OnPreprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                if (string.IsNullOrEmpty(PlayerSettings.iOS.cameraUsageDescription))
                    throw new BuildFailedException("ARKit requires a Camera Usage Description (Player Settings > iOS > Other Settings > Camera Usage Description)");

                SelectStaticLib();
                EnsureMinimumBuildTarget();
                EnsureOnlyMetalIsUsed();
                EnsureTargetArchitecturesAreSupported(report.summary.platformGroup);

                BuildHelper.AddBackgroundShaderToProject(ARKitCameraSubsystem.backgroundShaderName);
            }

            void EnsureMinimumBuildTarget()
            {
                var userSetTargetVersion = OSVersion.Parse(PlayerSettings.iOS.targetOSVersionString);
                if (userSetTargetVersion < new OSVersion(11))
                {
                    throw new BuildFailedException("You have selected a minimum target iOS version of " + userSetTargetVersion + " and have the ARKit package installed.  "
                        + "ARKit requires at least iOS version 11.0 (See Player Settings > Other Settings > Target minimum iOS Version).");
                }
            }

            void EnsureTargetArchitecturesAreSupported(BuildTargetGroup buildTargetGroup)
            {
                if (PlayerSettings.GetArchitecture(buildTargetGroup) != k_TargetArchitectureArm64)
                    throw new BuildFailedException("ARKit XR Plugin only supports the ARM64 architecture. See Player Settings > Other Settings > Architecture.");
            }

            void EnsureOnlyMetalIsUsed()
            {
                var graphicsApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
                if (graphicsApis.Length > 0)
                {
                    var graphicsApi = graphicsApis[0];
                    if (graphicsApi != GraphicsDeviceType.Metal)
                        throw new BuildFailedException("You have selected the graphics API " + graphicsApi + ". Only the Metal graphics API is supported by the ARKit XR Plugin. (See Player Settings > Other Settings > Graphics APIs)");
                }
            }

            public int callbackOrder { get { return 0; } }
        }
    }
}
#endif
