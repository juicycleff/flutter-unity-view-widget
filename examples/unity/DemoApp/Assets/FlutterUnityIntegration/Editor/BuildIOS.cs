using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FlutterUnityIntegration.Editor
{
    /// <summary>
    /// A class for building iOS Unity projects.
    /// </summary>
    public class BuildIOS : BaseBuild, IFuwBuilder
    {
        /// <summary>
        /// Gets or sets the build directory path.
        /// </summary>
        /// <value>
        /// The build directory path.
        /// </value>
        public new string BuildDir { get; set; }

        /// <summary>
        /// Gets or sets the options for the Fuw build.
        /// </summary>
        /// <value>
        /// The options for the Fuw build.
        /// </value>
        public FuwBuildOptions Options { get; set; }

        /// <summary>
        /// Indicates whether the current mode is package mode.
        /// </summary>
        private bool _packageMode;

        /// <summary>
        /// Initializes the FuwBuilder.
        /// </summary>
        /// <returns>
        /// An instance of the IFuwBuilder interface.
        /// </returns>
        public IFuwBuilder Init()
        {
            Bootstrap(Options);
            return this;
        }


        /// <summary>
        /// Builds the iOS standalone application.
        /// </summary>
        public void Build()
        {
            // Switch to ios standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

            if (Directory.Exists(OutputDir))
                Directory.Delete(OutputDir, true);

#if (UNITY_2021_1_OR_NEWER)
            EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;
#else
                EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
#endif

#if UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.iOS,
                Options.Release ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
            PlayerSettings.SetIl2CppCodeGeneration(UnityEditor.Build.NamedBuildTarget.iOS,
                UnityEditor.Build.Il2CppCodeGeneration.OptimizeSize);
#elif UNITY_2021_2_OR_NEWER
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.iOS, isReleaseBuild ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
                EditorUserBuildSettings.il2CppCodeGeneration = UnityEditor.Build.Il2CppCodeGeneration.OptimizeSize;
#endif

            var playerOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.iOS,
                locationPathName = OutputDir
            };

            if (!Options.Release)
            {
                playerOptions.options = BuildOptions.AllowDebugging | BuildOptions.Development;
            }

            // build addressable
            ExportAddressables();

            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            //trigger postbuild script manually
#if UNITY_IOS
            XcodePostBuild.PostBuild(BuildTarget.iOS, report.summary.outputPath);
#endif

            if (Options.Release)
            {
                Debug.Log("-- iOS Release Build: SUCCESSFUL --");
            }
            else
            {
                Debug.Log("-- iOS Debug Build: SUCCESSFUL --");
            }

            if (Options.PackageMode)
            {
                // Automate some manual steps
                SetupIOSProjectForPlugin();

                // Build Archive
                // BuildUnityFrameworkArchive();
            }
        }

        /// <summary>
        /// Exports the data to an external file or system.
        /// </summary>
        public void Export()
        {
        }

        /// ` and `vendored_frameworks` properties.
        private void SetupIOSProjectForPlugin()
        {
            var iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios"));
            var pubsecFile = Path.Combine(iosRunnerPath, "flutter_unity_widget.podspec");
            var pubsecText = File.ReadAllText(pubsecFile);

            if (!Regex.IsMatch(pubsecText, @"\w\.xcconfig(?:[^}]*})+") &&
                !Regex.IsMatch(pubsecText, @"tar -xvjf UnityFramework.tar.bz2"))
            {
                var regex = new Regex(@"\w\.xcconfig(?:[^}]*})+", RegexOptions.Multiline);
                pubsecText = regex.Replace(pubsecText, @"
	spec.xcconfig = {
        'FRAMEWORK_SEARCH_PATHS' => '""${PODS_ROOT}/../.symlinks/plugins/flutter_unity_widget/ios"" ""${PODS_ROOT}/../.symlinks/flutter/ios-release"" ""${PODS_CONFIGURATION_BUILD_DIR}""',
        'OTHER_LDFLAGS' => '$(inherited) -framework UnityFramework \${PODS_LIBRARIES}'
    }

    spec.vendored_frameworks = ""UnityFramework.framework""
			");
                File.WriteAllText(pubsecFile, pubsecText);
            }
        }

        // DO NOT USE (Contact before trying)
        /// <summary>
        /// Builds the Unity framework archive for iOS.
        /// </summary>
        private async void BuildUnityFrameworkArchive()
        {
            var xcprojectExt = "/Unity-iPhone.xcodeproj";

            // check if we have a workspace or not
            if (Directory.Exists(OutputDir + "/Unity-iPhone.xcworkspace"))
            {
                xcprojectExt = "/Unity-iPhone.xcworkspace";
            }

            const string framework = "UnityFramework";
            var xcprojectName = $"{OutputDir}{xcprojectExt}";
            var schemeName = $"{framework}";
            var buildPath = OutputDir + "/build";
            var frameworkNameWithExt = $"{framework}.framework";

            var iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/"));
            const string iosArchiveDir = "Release-iphoneos-archive";
            var iosArchiveFrameworkPath =
                $"{buildPath}/{iosArchiveDir}/Products/Library/Frameworks/{frameworkNameWithExt}";
            var dysmNameWithExt = $"{frameworkNameWithExt}.dSYM";

            try
            {
                Debug.Log("### Cleaning up after old builds");
                await $" - rf {iosRunnerPath}{frameworkNameWithExt}".Bash("rm");
                await $" - rf {buildPath}".Bash("rm");

                Debug.Log("### BUILDING FOR iOS");
                Debug.Log("### Building for device (Archive)");

                await
                    $"archive -workspace {xcprojectName} -scheme {schemeName} -sdk iphoneos -archivePath {buildPath}/Release-iphoneos.xcarchive ENABLE_BITCODE=NO |xcpretty"
                        .Bash("xcodebuild");

                Debug.Log("### Copying framework files");
                await $" -RL {iosArchiveFrameworkPath} {iosRunnerPath}/{frameworkNameWithExt}".Bash("cp");
                await $" -RL {iosArchiveFrameworkPath}/{dysmNameWithExt} {iosRunnerPath}/{dysmNameWithExt}".Bash("cp");
                Debug.Log("### DONE ARCHIVING");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}