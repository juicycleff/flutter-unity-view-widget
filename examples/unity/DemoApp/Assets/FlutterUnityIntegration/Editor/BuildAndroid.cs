using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FlutterUnityIntegration.Editor
{
    /// <summary>
    /// Represents a class for building Android applications.
    /// </summary>
    public class BuildAndroid : BaseBuild, IFuwBuilder
    {
        /// <summary>
        /// Gets or sets the build directory where the project is built.
        /// </summary>
        /// <value>
        /// The build directory path.
        /// </value>
        public new string BuildDir { get; set; }

        /// <summary>
        /// Gets or sets the build options for the Fuw object.
        /// </summary>
        /// <value>
        /// The build options for the Fuw object.
        /// </value>
        public FuwBuildOptions Options { get; set; }

        /// <summary>
        /// Indicates whether the software is running in package mode.
        /// </summary>
        private bool _packageMode;

        /// Initializes the FuwBuilder by calling the Bootstrap method with the
        /// specified options. It then sets the BuildDir property by combining
        /// the APKPath and BuildPath from the options. Finally, it returns an
        /// instance of IFuwBuilder.
        /// @returns An instance of IFuwBuilder.
        /// /
        public IFuwBuilder Init()
        {
            Bootstrap(Options);
            BuildDir = Path.Combine(APKPath, Options.BuildPath);
            return this;
        }

        /// <summary>
        /// Builds the Android standalone application.
        /// </summary>
        public void Build()
        {
            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            if (Directory.Exists(APKPath))
                Directory.Delete(APKPath, true);

            if (Directory.Exists(Options.OutputDir))
                Directory.Delete(Options.OutputDir, true);

            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

            var playerOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.Android,
                locationPathName = APKPath
            };

            if (!Options.Release)
            {
                // remove this line if you don't use a debugger and you want to speed up the flutter build
                playerOptions.options = BuildOptions.AllowDebugging | BuildOptions.Development;
            }
#if UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android,
                Options.Release ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
            PlayerSettings.SetIl2CppCodeGeneration(UnityEditor.Build.NamedBuildTarget.Android,
                UnityEditor.Build.Il2CppCodeGeneration.OptimizeSize);
#elif UNITY_2021_2_OR_NEWER
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, isReleaseBuild ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
                EditorUserBuildSettings.il2CppCodeGeneration = UnityEditor.Build.Il2CppCodeGeneration.OptimizeSize;
#endif

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            // build addressable
            ExportAddressables();
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            Copy(BuildDir, Options.OutputDir);

            // Modify build.gradle
            ModifyAndroidGradle(Options.PackageMode);

            if (Options.PackageMode)
            {
                SetupAndroidProjectForPlugin();
            }
            else
            {
                SetupAndroidProject();
            }

            if (Options.Release)
            {
                Debug.Log($"-- Android Release Build: SUCCESSFUL --");
            }
            else
            {
                Debug.Log($"-- Android Debug Build: SUCCESSFUL --");
            }
        }

        /// <summary>
        /// Copies the resources from the launcher module that are used by the library to the specified output directory.
        /// </summary>
        public void Export()
        {
            // Copy over resources from the launcher module that are used by the library
            Copy(Path.Combine(APKPath + "/launcher/src/main/res"), Path.Combine(Options.OutputDir, "src/main/res"));
        }

        /// Modifies the Android Gradle build files and configuration based on the specified parameters.
        /// @param isPlugin A boolean value indicating if the project is a plugin.
        /// /
        private void ModifyAndroidGradle(bool isPlugin)
        {
            // Modify build.gradle
            var buildFile = Path.Combine(Options.OutputDir, "build.gradle");
            var buildText = File.ReadAllText(buildFile);
            buildText = buildText.Replace("com.android.application", "com.android.library");
            buildText = buildText.Replace("bundle {", "splits {");
            buildText = buildText.Replace("enableSplit = false", "enable false");
            buildText = buildText.Replace("enableSplit = true", "enable true");
            buildText = buildText.Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])",
                "implementation(name: 'unity-classes', ext:'jar')");
            buildText = buildText.Replace(" + unityStreamingAssets.tokenize(', ')", "");

            if (isPlugin)
            {
                buildText = Regex.Replace(buildText, @"implementation\(name: 'androidx.* ext:'aar'\)", "\n");
            }

            buildText = Regex.Replace(buildText, @"\n.*applicationId '.+'.*\n", "\n");
            File.WriteAllText(buildFile, buildText);

            // Modify AndroidManifest.xml
            var manifestFile = Path.Combine(Options.OutputDir, "src/main/AndroidManifest.xml");
            var manifestText = File.ReadAllText(manifestFile);
            manifestText = Regex.Replace(manifestText, @"<application .*>", "<application>");
            var regex = new Regex(@"<activity.*>(\s|\S)+?</activity>", RegexOptions.Multiline);
            manifestText = regex.Replace(manifestText, "");
            File.WriteAllText(manifestFile, manifestText);

            // Modify proguard-unity.txt
            var proguardFile = Path.Combine(Options.OutputDir, "proguard-unity.txt");
            var proguardText = File.ReadAllText(proguardFile);
            proguardText = proguardText.Replace("-ignorewarnings",
                "-keep class com.xraph.plugin.** { *; }\n-keep class com.unity3d.plugin.* { *; }\n-ignorewarnings");
            File.WriteAllText(proguardFile, proguardText);
        }


        /// <summary>
        /// Sets up the build configuration required for Android.
        /// </summary>
        private void SetupAndroidProject()
        {
            var androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
            var androidAppPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/app"));
            var projBuildPath = Path.Combine(androidPath, "build.gradle");
            var appBuildPath = Path.Combine(androidAppPath, "build.gradle");
            var settingsPath = Path.Combine(androidPath, "settings.gradle");

            var projBuildScript = File.ReadAllText(projBuildPath);
            var settingsScript = File.ReadAllText(settingsPath);
            var appBuildScript = File.ReadAllText(appBuildPath);

            // Sets up the project build.gradle files correctly
            if (!Regex.IsMatch(projBuildScript, @"flatDir[^/]*[^}]*}"))
            {
                var regex = new Regex(@"allprojects \{[^\{]*\{", RegexOptions.Multiline);
                projBuildScript = regex.Replace(projBuildScript, @"
allprojects {
    repositories {
        flatDir {
            dirs ""${project(':unityLibrary').projectDir}/libs""
        }
");
                File.WriteAllText(projBuildPath, projBuildScript);
            }

            // Sets up the project settings.gradle files correctly
            if (!Regex.IsMatch(settingsScript, @"include "":unityLibrary"""))
            {
                settingsScript += @"

include "":unityLibrary""
project("":unityLibrary"").projectDir = file(""./unityLibrary"")
";
                File.WriteAllText(settingsPath, settingsScript);
            }


            // Sets up the project app build.gradle files correctly
            if (!Regex.IsMatch(appBuildScript, @"dependencies \{"))
            {
                appBuildScript += @"
dependencies {
    implementation project(':unityLibrary')
}
";
                File.WriteAllText(appBuildPath, appBuildScript);
            }
            else
            {
                if (!appBuildScript.Contains(@"implementation project(':unityLibrary')"))
                {
                    var regex = new Regex(@"dependencies \{", RegexOptions.Multiline);
                    appBuildScript = regex.Replace(appBuildScript, @"
dependencies {
    implementation project(':unityLibrary')
");
                    File.WriteAllText(appBuildPath, appBuildScript);
                }
            }
        }

        /// <summary>
        /// This method tries to automate the build setup required for Android.
        /// </summary>
        private void SetupAndroidProjectForPlugin()
        {
            var androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
            var projBuildPath = Path.Combine(androidPath, "build.gradle");
            var settingsPath = Path.Combine(androidPath, "settings.gradle");

            var projBuildScript = File.ReadAllText(projBuildPath);
            var settingsScript = File.ReadAllText(settingsPath);

            // Sets up the project build.gradle files correctly
            if (Regex.IsMatch(projBuildScript, @"// BUILD_ADD_UNITY_LIBS"))
            {
                var regex = new Regex(@"// BUILD_ADD_UNITY_LIBS", RegexOptions.Multiline);
                projBuildScript = regex.Replace(projBuildScript, @"
        flatDir {
            dirs ""${project(':unityLibrary').projectDir}/libs""
        }
");
                File.WriteAllText(projBuildPath, projBuildScript);
            }

            // Sets up the project settings.gradle files correctly
            if (!Regex.IsMatch(settingsScript, @"include "":unityLibrary"""))
            {
                settingsScript += @"

include "":unityLibrary""
project("":unityLibrary"").projectDir = file(""./unityLibrary"")
";
                File.WriteAllText(settingsPath, settingsScript);
            }
        }
    }
}