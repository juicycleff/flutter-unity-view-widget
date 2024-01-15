using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FlutterUnityIntegration.Editor
{
    /// <summary>
    /// Represents a class for building a web application.
    /// </summary>
    public class BuildWeb : BaseBuild, IFuwBuilder
    {
        /// <summary>
        /// Gets the directory where the build artifacts are stored.
        /// </summary>
        /// <value>
        /// The directory where the build artifacts are stored.
        /// </value>
        public string BuildDir { get; }

        /// <summary>
        /// Gets the output directory path.
        /// </summary>
        /// <value>
        /// The output directory path.
        /// </value>
        /// <remarks>
        /// This property is read-only.
        /// It stores the file path of the output directory where the generated files will be saved.
        /// </remarks>
        public string OutputDir { get; }

        /// <summary>
        /// Gets or sets the FuwBuildOptions.
        /// </summary>
        public FuwBuildOptions Options { get; set; }

        /// <summary>
        /// Represents the package mode setting.
        /// </summary>
        /// <remarks>
        /// The package mode indicates whether the system is running in package mode or not.
        /// Package mode enables specific features and functionality for the system.
        /// </remarks>
        private bool _packageMode;

        /// <summary>
        /// Initializes the FuwBuilder.
        /// </summary>
        /// <returns>
        /// An instance of IFuwBuilder.
        /// </returns>
        public IFuwBuilder Init()
        {
            Bootstrap(Options);
            return this;
        }

        /// <summary>
        /// Builds the WebGL version of the game.
        /// </summary>
        public void Build()
        {
            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            if (Directory.Exists(OutputDir))
                Directory.Delete(OutputDir, true);

            // EditorUserBuildSettings. = true;

            var playerOptions = new BuildPlayerOptions();
            playerOptions.scenes = GetEnabledScenes();
            playerOptions.target = BuildTarget.WebGL;
            playerOptions.locationPathName = OutputDir;

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            // build addressable
            ExportAddressables();
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            // Copy(path, WebExportPath);
            modifyWebGLExport();

            Debug.Log("-- WebGL Build: SUCCESSFUL --");
        }

        /// <summary>
        /// Method to export data.
        /// </summary>
        public void Export()
        {
        }


        /// Modifies the WebGL export files.
        /// /
        private void modifyWebGLExport()
        {
            // Modify index.html
            var indexFile = Path.Combine(OutputDir, "index.html");
            var indexHtmlText = File.ReadAllText(indexFile);

            indexHtmlText = indexHtmlText.Replace("<script>", @"
    <script>
        var mainUnityInstance;

        window['handleUnityMessage'] = function (params) {
        window.parent.postMessage({
            name: 'onUnityMessage',
            data: params,
            }, '*');
        };

        window['handleUnitySceneLoaded'] = function (name, buildIndex, isLoaded, isValid) {
        window.parent.postMessage({
            name: 'onUnitySceneLoaded',
            data: {
                'name': name,
                'buildIndex': buildIndex,
                'isLoaded': isLoaded == 1,
                'isValid': isValid == 1,
            }
            }, '*');
        };

        window.parent.addEventListener('unityFlutterBiding', function (args) {
            const obj = JSON.parse(args.data);
            mainUnityInstance.SendMessage(obj.gameObject, obj.methodName, obj.message);
        });

        window.parent.addEventListener('unityFlutterBidingFnCal', function (args) {
            mainUnityInstance.SendMessage('GameManager', 'HandleWebFnCall', args);
        });
        ");

            indexHtmlText = indexHtmlText.Replace("canvas.style.width = \"960px\";", "canvas.style.width = \"100%\";");
            indexHtmlText =
                indexHtmlText.Replace("canvas.style.height = \"600px\";", "canvas.style.height = \"100%\";");

            indexHtmlText = indexHtmlText.Replace("}).then((unityInstance) => {", @"
         }).then((unityInstance) => {
           window.parent.postMessage('unityReady', '*');
           mainUnityInstance = unityInstance;
         ");
            File.WriteAllText(indexFile, indexHtmlText);

            // Modidy style.css
            var cssFile = Path.Combine($"{OutputDir}/TemplateData", "style.css");
            var fullScreenCss = File.ReadAllText(cssFile);
            fullScreenCss = @"
body { padding: 0; margin: 0; overflow: hidden; }
#unity-container { position: absolute }
#unity-container.unity-desktop { width: 100%; height: 100% }
#unity-container.unity-mobile { width: 100%; height: 100% }
#unity-canvas { background: #231F20 }
.unity-mobile #unity-canvas { width: 100%; height: 100% }
#unity-loading-bar { position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%); display: none }
#unity-logo { width: 154px; height: 130px; background: url('unity-logo-dark.png') no-repeat center }
#unity-progress-bar-empty { width: 141px; height: 18px; margin-top: 10px; background: url('progress-bar-empty-dark.png') no-repeat center }
#unity-progress-bar-full { width: 0%; height: 18px; margin-top: 10px; background: url('progress-bar-full-dark.png') no-repeat center }
#unity-footer { display: none }
.unity-mobile #unity-footer { display: none }
#unity-webgl-logo { float:left; width: 204px; height: 38px; background: url('webgl-logo.png') no-repeat center }
#unity-build-title { float: right; margin-right: 10px; line-height: 38px; font-family: arial; font-size: 18px }
#unity-fullscreen-button { float: right; width: 38px; height: 38px; background: url('fullscreen-button.png') no-repeat center }
#unity-mobile-warning { position: absolute; left: 50%; top: 5%; transform: translate(-50%); background: white; padding: 10px; display: none }
            ";
            File.WriteAllText(cssFile, fullScreenCss);
        }
    }
}