using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FlutterUnityIntegration.Editor
{
    /// <summary>
    /// A class that builds a Windows application using Unity.
    /// </summary>
    public class BuildWindows : BaseBuild, IFuwBuilder
    {
        /// <summary>
        /// Gets or sets the FuwBuildOptions for the Options property.
        /// </summary>
        /// <value>
        /// The FuwBuildOptions object that contains the build options.
        /// </value>
        /// <remarks>
        /// This property is used to set and retrieve the build options for the FuwBuild class.
        /// The FuwBuildOptions object contains various options that affect the build process,
        /// such as debug mode, optimization level, and target platform.
        /// </remarks>
        public FuwBuildOptions Options { get; set; }

        /// <summary>
        /// Represents the mode in which the package is operating.
        /// </summary>
        private bool _packageMode;

        /// <summary>
        /// Initializes the IFuwBuilder and boots up the application with the given options.
        /// </summary>
        /// <returns>
        /// The initialized IFuwBuilder.
        /// </returns>
        public IFuwBuilder Init()
        {
            Bootstrap(Options);
            return this;
        }

        /// <summary>
        /// Builds the project for Android standalone build and Windows build.
        /// </summary>
        public void Build()
        {
            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            if (Directory.Exists(OutputDir))
                Directory.Delete(OutputDir, true);

            var playerOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.StandaloneWindows64,
                locationPathName = OutputDir,
                options = BuildOptions.AllowDebugging
            };

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone,
                BuildTarget.StandaloneWindows64);

            // build addressable
            ExportAddressables();
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            Debug.Log("-- Windows Build: SUCCESSFUL --");
        }

        /// <summary>
        /// Exports data to a specified destination.
        /// </summary>
        /// <remarks>
        /// This method exports data to a specified destination.
        /// </remarks>
        public void Export()
        {
        }
    }
}