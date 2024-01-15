using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// uncomment for addressables
//using UnityEditor.AddressableAssets;
//using UnityEditor.AddressableAssets.Settings;

namespace FlutterUnityIntegration.Editor
{
    public record FuwBuildOptions
    {
        public string OutputDir { get; set; }
        public string BuildPath { get; set; }
        public bool PackageMode { get; set; }
        public bool Release { get; set; }
    }

    public class BaseBuild
    {
        public string ProjectPath;
        public string APKPath;
        public string OutputDir { get; set; }
        public string BuildDir { get; set; }

        public void Bootstrap(FuwBuildOptions options)
        {
            ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            APKPath = Path.Combine(ProjectPath, "Builds/" + Application.productName + ".apk");
            OutputDir = Path.GetFullPath(Path.Combine(ProjectPath, options.OutputDir));
        }

        protected void Copy(string source, string destinationPath)
        {
            if (Directory.Exists(destinationPath))
                Directory.Delete(destinationPath, true);

            Directory.CreateDirectory(destinationPath);

            foreach (var dirPath in Directory.GetDirectories(source, "*",
                         SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(source, destinationPath));

            foreach (var newPath in Directory.GetFiles(source, "*.*",
                         SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(source, destinationPath), true);
        }

        protected string[] GetEnabledScenes()
        {
            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            return scenes;
        }

        protected void ExportAddressables()
        {
            // Debug.Log("Start building player content (Addressables)");
            // Debug.Log("BuildAddressablesProcessor.PreExport start");
            //
            // AddressableAssetSettings.CleanPlayerContent(
            //     AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            //
            // AddressableAssetProfileSettings profileSettings = AddressableAssetSettingsDefaultObject.Settings.profileSettings;
            // string profileId = profileSettings.GetProfileId("Default");
            // AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profileId;
            //
            // AddressableAssetSettings.BuildPlayerContent();
            // Debug.Log("BuildAddressablesProcessor.PreExport done");
        }
    }
}