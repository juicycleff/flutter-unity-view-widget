using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using Application = UnityEngine.Application;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

#if USING_ADDRESSABLES
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
namespace FlutterUnityIntegration.Editor
{
    public class Build : EditorWindow
    {
        private static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private static readonly string APKPath = Path.Combine(ProjectPath, "Builds/" + Application.productName + ".apk");

        private static readonly string AndroidExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/unityLibrary"));
        private static readonly string WindowsExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../windows/unityLibrary/data"));
        private static readonly string IOSExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/UnityLibrary"));
        private static readonly string WebExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../web/UnityLibrary"));
        private static readonly string IOSExportPluginPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios_xcode/UnityLibrary"));

        private static bool _pluginMode = false;
        private static string _persistentKey = "flutter-unity-widget-pluginMode";

        private const string USING_ADDRESSABLES = "USING_ADDRESSABLES";

        private static string _persistentKeyHasAddressable = "flutter-unity-widget-hasAddressable";
        private static bool _usingAddressables = false;

        public static string _persistentOverrideStatus = "flutter-unity-widget-OverrideStatus";
        private static bool _overrideBuildPaths = false;


        //#region GUI Member Methods
        [MenuItem("Flutter/Export Android (Debug) %&n", false, 101)]
        public static void DoBuildAndroidLibraryDebug()
        {
            DoBuildAndroid(Path.Combine(APKPath, "unityLibrary"), false, false);
        }

        [MenuItem("Flutter/Export Android (Release) %&m", false, 102)]
        public static void DoBuildAndroidLibraryRelease()
        {
            DoBuildAndroid(Path.Combine(APKPath, "unityLibrary"), false, true);
        }

        [MenuItem("Flutter/Export Android Plugin %&p", false, 103)]
        public static void DoBuildAndroidPlugin()
        {
            DoBuildAndroid(Path.Combine(APKPath, "unityLibrary"), true, true);
        }

        [MenuItem("Flutter/Export IOS (Debug) %&i", false, 201)]
        public static void DoBuildIOSDebug()
        {
            BuildIOS(IOSExportPath, false);
        }

        [MenuItem("Flutter/Export IOS (Release) %&i", false, 202)]
        public static void DoBuildIOSRelease()
        {
            BuildIOS(IOSExportPath, true);
        }

        [MenuItem("Flutter/Export IOS Plugin %&o", false, 203)]
        public static void DoBuildIOSPlugin()
        {
            BuildIOS(IOSExportPluginPath, true);

            // Automate so manual steps
            SetupIOSProjectForPlugin();

            // Build Archive
            // BuildUnityFrameworkArchive();

        }

        [MenuItem("Flutter/Export Web GL %&w", false, 301)]
        public static void DoBuildWebGL()
        {
            BuildWebGL(WebExportPath);
        }

        // Hide this button as windows isn't implemented in the Flutter plugin yet.
        //  [MenuItem("Flutter/Export Windows %&d", false, 401)]
        public static void DoBuildWindowsOS()
        {
            BuildWindowsOS(WindowsExportPath);
        }

        [MenuItem("Flutter/Settings %&S", false, 501)]
        public static void PluginSettings()
        {
            EditorWindow.GetWindow(typeof(Build));
        }

        bool hasChanges = false;
        private void OnGUI()
        {
            GUILayout.Label("Flutter Unity Widget Settings", EditorStyles.boldLabel);


            EditorGUI.BeginChangeCheck();
            _pluginMode = EditorGUILayout.Toggle("Plugin Mode", _pluginMode);

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(_persistentKey, _pluginMode);
            }

            using (new EditorGUILayout.HorizontalScope())
            {

                EditorGUI.BeginChangeCheck();
                _usingAddressables = EditorGUILayout.Toggle("Using Addressables", _usingAddressables);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorPrefs.SetBool(_persistentKeyHasAddressable, _usingAddressables);
                    hasChanges = true;
                    Repaint();
                }

                if (GUILayout.Button("Apply Symbols"))
                {
                    if (hasChanges)
                    {
                        Apply();
                        Repaint();
                    }
                    else
                    {
                        Debug.Log("No Changes required!");
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            _overrideBuildPaths = EditorGUILayout.Toggle("Override Build Paths", _overrideBuildPaths);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(_persistentOverrideStatus, _overrideBuildPaths);
                Repaint();
            }


            // Web Export Path
            using (var horizontalScope = new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("WebGL Export Path", GUILayout.Width(150));
                GUI.enabled = false;
                EditorGUILayout.TextField(BuildPathsHelper.Overriden_WebExportPath);
                GUI.enabled = true;

                if (GUILayout.Button("...", GUILayout.Width(30)))
                {
                    string selectedPath = EditorUtility.OpenFolderPanel("Select WebGL Export Folder", "", "");
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        selectedPath = Path.Combine(selectedPath, "UnityLibrary");
                        BuildPathsHelper.Overriden_WebExportPath = selectedPath;
                    }

                    EditorPrefs.SetString(BuildPathsHelper._persistentWebExportPath, selectedPath);
                }
            }

            GUI.enabled = false;

            if (_overrideBuildPaths)
            {
                // Android Export Path
                using (var horizontalScope = new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Android Export Path", GUILayout.Width(150));

                    GUI.enabled = false;
                    EditorGUILayout.TextField(BuildPathsHelper.Overriden_AndroidExportPath);
                    //GUI.enabled = true;

                    if (GUILayout.Button("...", GUILayout.Width(30)))
                    {
                        string selectedPath = EditorUtility.OpenFolderPanel("Select Android Export Folder", "", "unityLibrary");
                        if (!string.IsNullOrEmpty(selectedPath))
                        {
                            selectedPath = Path.Combine(selectedPath, "UnityLibrary");
                            BuildPathsHelper.Overriden_AndroidExportPath = selectedPath;
                        }
                        EditorPrefs.SetString(BuildPathsHelper.Overriden_AndroidExportPath, selectedPath);
                    }
                }

                // Windows Export Path 

                //using (var horizontalScope = new GUILayout.HorizontalScope())
                //{
                //    EditorGUILayout.LabelField("Windows Export Path", GUILayout.Width(150));

                //    GUI.enabled = false;
                //    EditorGUILayout.TextField(BuildPathsHelper.Overriden_WindowsExportPath);
                //    GUI.enabled = true;

                //    if (GUILayout.Button("...", GUILayout.Width(30)))
                //    {
                //        string selectedPath = EditorUtility.OpenFolderPanel("Select Windows Export Folder", "", "");
                //        if (!string.IsNullOrEmpty(selectedPath))
                //        {
                //            selectedPath = Path.Combine(selectedPath, "UnityLibrary");
                //            BuildPathsHelper.Overriden_WindowsExportPath = selectedPath;
                //        }
                //        EditorPrefs.SetString(BuildPathsHelper._persistentWindowsExportPath, selectedPath);
                //    }
                //}

                // iOS Export Path
                using (var horizontalScope = new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("iOS Export Path", GUILayout.Width(150));

                    GUI.enabled = false;
                    EditorGUILayout.TextField(BuildPathsHelper.Overriden_IOSExportPath);
                    //GUI.enabled = true;

                    if (GUILayout.Button("...", GUILayout.Width(30)))
                    {
                        string selectedPath = EditorUtility.OpenFolderPanel("Select iOS Export Folder", "", "");
                        if (!string.IsNullOrEmpty(selectedPath))
                        {
                            selectedPath = Path.Combine(selectedPath, "UnityLibrary");
                            BuildPathsHelper.Overriden_IOSExportPath = selectedPath;
                        }
                        EditorPrefs.SetString(BuildPathsHelper._persistentIOSExportPath, selectedPath);
                    }
                }


                // iOS Plugin Export Path
                using (var horizontalScope = new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("iOS Plugin Path", GUILayout.Width(150));

                    GUI.enabled = false;
                    EditorGUILayout.TextField(BuildPathsHelper.Overriden_IOSExportPluginPath);
                    //GUI.enabled = true;

                    if (GUILayout.Button("...", GUILayout.Width(30)))
                    {
                        string selectedPath = EditorUtility.OpenFolderPanel("Select iOS Plugin Folder", "", "");
                        if (!string.IsNullOrEmpty(selectedPath))
                        {
                            selectedPath = Path.Combine(selectedPath, "UnityLibrary");
                            BuildPathsHelper.Overriden_IOSExportPluginPath = selectedPath;
                        }
                        EditorPrefs.SetString(BuildPathsHelper._persistentIOSExportPluginPath, selectedPath);
                    }

                }
                GUI.enabled = true;

            }

        }

        private void Apply()
        {
            EditorApplication.delayCall += () => {
                SymbolDefineHelper.SetScriptingDefine(USING_ADDRESSABLES, _usingAddressables);
            };
            Close();
        }



        private void OnEnable()
        {
            InitPrefs();
        }

        private static void InitPrefs()
        {
            _pluginMode = EditorPrefs.GetBool(_persistentKey, false);
            _usingAddressables = EditorPrefs.GetBool(_persistentKeyHasAddressable, false);
            _overrideBuildPaths = EditorPrefs.GetBool(_persistentOverrideStatus, false);
            BuildPathsHelper.Overriden_WebExportPath = EditorPrefs.GetString(BuildPathsHelper._persistentWebExportPath, BuildPathsHelper.Overriden_WebExportPath);
        }

        //#endregion


        //#region Build Member Methods

        private static void BuildWindowsOS(String path)
        {
            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            if (Directory.Exists(WindowsExportPath))
                Directory.Delete(WindowsExportPath, true);

            var playerOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.StandaloneWindows64,
                locationPathName = path,
                options = BuildOptions.AllowDebugging
            };

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

            // build addressable
            ExportAddressables();
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            Debug.Log("-- Windows Build: SUCCESSFUL --");
        }

        private static void BuildWebGL(String path)
        {
            InitPrefs();


            if (_overrideBuildPaths)
            {
                // Check if the Unity project is in the expected location
                if (!IsProjectLocationValid(path, "web"))
                {
                    return;
                }
            }

            path = _overrideBuildPaths ? BuildPathsHelper.Overriden_WebExportPath : path;

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            if (Directory.Exists(WebExportPath))
                Directory.Delete(WebExportPath, true);



            // EditorUserBuildSettings. = true;

            var playerOptions = new BuildPlayerOptions();
            playerOptions.scenes = GetEnabledScenes();
            playerOptions.target = BuildTarget.WebGL;
            playerOptions.locationPathName = path;

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            // build addressable
            ExportAddressables();
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            // Copy(path, WebExportPath);
            ModifyWebGLExport();

            Debug.Log("-- WebGL Build: SUCCESSFUL --");
        }

        private static void DoBuildAndroid(String buildPath, bool isPlugin, bool isReleaseBuild)
        {
            InitPrefs();

            if (_overrideBuildPaths)
            {
                // Check if the Unity project is in the expected location
                if (!IsProjectLocationValid(AndroidExportPath, "android"))
                {
                    return;
                }
            }
            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);



            if (Directory.Exists(APKPath))
                Directory.Delete(APKPath, true);

            if (Directory.Exists(AndroidExportPath))
                Directory.Delete(AndroidExportPath, true);

            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

            var playerOptions = new BuildPlayerOptions();
            playerOptions.scenes = GetEnabledScenes();
            playerOptions.target = BuildTarget.Android;
            playerOptions.locationPathName = APKPath;
            if (!isReleaseBuild)
            {
                // remove this line if you don't use a debugger and you want to speed up the flutter build
                playerOptions.options = BuildOptions.AllowDebugging | BuildOptions.Development;
            }
#if UNITY_6000_0_OR_NEWER
            PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.Android, isReleaseBuild ? Il2CppCodeGeneration.OptimizeSpeed : Il2CppCodeGeneration.OptimizeSize);
#elif UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, isReleaseBuild ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
#elif UNITY_2021_2_OR_NEWER
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, isReleaseBuild ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
                EditorUserBuildSettings.il2CppCodeGeneration = isReleaseBuild ? Il2CppCodeGeneration.OptimizeSpeed : Il2CppCodeGeneration.OptimizeSize;
#endif

            // Switch to Android standalone build.
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            // build addressable
            ExportAddressables();
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            Copy(buildPath, AndroidExportPath);

            // Modify build.gradle
            ModifyAndroidGradle(isPlugin);

            if (isPlugin)
            {
                SetupAndroidProjectForPlugin();
            }
            else
            {
                SetupAndroidProject();
            }

            // Copy over resources from the launcher module that are used by the library, Avoid deleting the existing src/main/res contents.
            Copy(Path.Combine(APKPath + "/launcher/src/main/res"), Path.Combine(AndroidExportPath, "src/main/res"), false);

            if (isReleaseBuild)
            {
                Debug.Log($"-- Android Release Build: SUCCESSFUL --");
            }
            else
            {
                Debug.Log($"-- Android Debug Build: SUCCESSFUL --");
            }
        }

        private static void ModifyWebGLExport()
        {

            var path = _overrideBuildPaths ? BuildPathsHelper.Overriden_WebExportPath : WebExportPath;

            // Modify index.html
            var indexFile = Path.Combine(path, "index.html");
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
            mainUnityInstance.SendMessage('GameManager', 'HandleWebFnCall', args.data);
        });
        ");

            indexHtmlText = indexHtmlText.Replace("canvas.style.width = \"960px\";", "canvas.style.width = \"100%\";");
            indexHtmlText = indexHtmlText.Replace("canvas.style.height = \"600px\";", "canvas.style.height = \"100%\";");

            indexHtmlText = indexHtmlText.Replace("}).then((unityInstance) => {", @"
         }).then((unityInstance) => {
           window.parent.postMessage('unityReady', '*');
           mainUnityInstance = unityInstance;
         ");
            File.WriteAllText(indexFile, indexHtmlText);

            /// Modidy style.css
            var cssFile = Path.Combine($"{(_overrideBuildPaths ? BuildPathsHelper.Overriden_WebExportPath : WebExportPath)}/TemplateData", "style.css");
            var fullScreenCss = File.ReadAllText(cssFile);
            fullScreenCss = @"
body { padding: 0; margin: 0; overflow: hidden; }
# unity-container { position: absolute }
# unity-container.unity-desktop { width: 100%; height: 100% }
# unity-container.unity-mobile { width: 100%; height: 100% }
# unity-canvas { background: #231F20 }
.unity-mobile #unity-canvas { width: 100%; height: 100% }
# unity-loading-bar { position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%); display: none }
# unity-logo { width: 154px; height: 130px; background: url('unity-logo-dark.png') no-repeat center }
# unity-progress-bar-empty { width: 141px; height: 18px; margin-top: 10px; background: url('progress-bar-empty-dark.png') no-repeat center }
# unity-progress-bar-full { width: 0%; height: 18px; margin-top: 10px; background: url('progress-bar-full-dark.png') no-repeat center }
# unity-footer { display: none }
.unity-mobile #unity-footer { display: none }
# unity-webgl-logo { float:left; width: 204px; height: 38px; background: url('webgl-logo.png') no-repeat center }
# unity-build-title { float: right; margin-right: 10px; line-height: 38px; font-family: arial; font-size: 18px }
# unity-fullscreen-button { float: right; width: 38px; height: 38px; background: url('fullscreen-button.png') no-repeat center }
# unity-mobile-warning { position: absolute; left: 50%; top: 5%; transform: translate(-50%); background: white; padding: 10px; display: none }
            ";
            File.WriteAllText(cssFile, fullScreenCss);
        }

        private static void ModifyAndroidGradle(bool isPlugin)
        {
            // Modify build.gradle
            var buildFile = Path.Combine(AndroidExportPath, "build.gradle");
            var buildText = File.ReadAllText(buildFile);
            buildText = buildText.Replace("com.android.application", "com.android.library");
            buildText = buildText.Replace("bundle {", "splits {");
            buildText = buildText.Replace("enableSplit = false", "enable false");
            buildText = buildText.Replace("enableSplit = true", "enable true");
            buildText = buildText.Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])", "implementation(name: 'unity-classes', ext:'jar')");
            buildText = buildText.Replace(" + unityStreamingAssets.tokenize(', ')", "");
            // disable the Unity ndk path as it will conflict with Flutter.
            buildText = buildText.Replace("ndkPath \"", "// ndkPath \"");

            // check for namespace definition (Android gradle plugin 8+), add a backwards compatible version if it is missing.
            if (!buildText.Contains("namespace"))
            {
                buildText = buildText.Replace("compileOptions {",
                    "if (project.android.hasProperty(\"namespace\")) {\n        namespace 'com.unity3d.player'\n    }\n\n    compileOptions {"
                );
            }

            if (isPlugin)
            {
                buildText = Regex.Replace(buildText, @"implementation\(name: 'androidx.* ext:'aar'\)", "\n");
            }

            buildText = Regex.Replace(buildText, @"\n.*applicationId '.+'.*\n", "\n");
            File.WriteAllText(buildFile, buildText);

            // Modify AndroidManifest.xml
            var manifestFile = Path.Combine(AndroidExportPath, "src/main/AndroidManifest.xml");
            var manifestText = File.ReadAllText(manifestFile);
            manifestText = Regex.Replace(manifestText, @"<application .*>", "<application>");
            var regex = new Regex(@"<activity.*>(\s|\S)+?</activity>", RegexOptions.Multiline);
            manifestText = regex.Replace(manifestText, "");
            File.WriteAllText(manifestFile, manifestText);

            // Modify proguard-unity.txt
            var proguardFile = Path.Combine(AndroidExportPath, "proguard-unity.txt");
            var proguardText = File.ReadAllText(proguardFile);
            proguardText = proguardText.Replace("-ignorewarnings", "-keep class com.xraph.plugin.** { *; }\n-keep class com.unity3d.plugin.* { *; }\n-ignorewarnings");
            File.WriteAllText(proguardFile, proguardText);

            // Make sure "game_view_content_description" is in strings.xml
            var stringsFile = Path.Combine(APKPath, "launcher", "src", "main", "res", "values", "strings.xml");
            if (File.Exists(stringsFile))
            {
                var stringsText = File.ReadAllText(stringsFile);
                if (!stringsText.Contains("game_view_content_description"))
                {
                    stringsText = stringsText.Replace("<resources>", "<resources>\n  <string name=\"game_view_content_description\">Game view</string>");
                    File.WriteAllText(stringsFile, stringsText);
                }
            }
            else
            {
                Debug.LogError("Android res/values/strings.xml file not found during export.");
            }
        }

        private static void BuildIOS(String path, bool isReleaseBuild)
        {
            // Check if the Unity project is in the expected location
            if (!IsProjectLocationValid(path, "ios"))
            {
                return;
            }

            bool abortBuild = false;

            // abort iOS export if #UNITY_IOS is false.
            // Even after SwitchActiveBuildTarget() it will still be false as the code isn't recompiled yet.
            // As a workaround, make the user trigger an export again after the switch.

#if !UNITY_IOS
            abortBuild = true;
            if (Application.isBatchMode)
            {
                Debug.LogError("Incorrect iOS buildtarget, use the -buildTarget argument to set iOS");
            }
            else
            {
                bool dialogResult = EditorUtility.DisplayDialog(
                    "Switch build target to iOS?",
                    "Exporting to iOS first requires a build target switch.\nClick 'Export iOS' again after all importing has finished.",
                    "Switch to iOS",
                    "Cancel"
                );
                if (dialogResult)
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                }
            }
#endif
            //don't return within #if !UNITY_IOS as that results in unreachable code warnings.
            if (abortBuild)
                return;

            if (Directory.Exists(path))
                Directory.Delete(path, true);

#if (UNITY_2021_1_OR_NEWER)
            EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;
#else
                EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
#endif

#if UNITY_6000_0_OR_NEWER
            PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.iOS, isReleaseBuild ? Il2CppCodeGeneration.OptimizeSpeed : Il2CppCodeGeneration.OptimizeSize);
#elif UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.iOS, isReleaseBuild ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
#elif UNITY_2021_2_OR_NEWER
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.iOS, isReleaseBuild ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
                EditorUserBuildSettings.il2CppCodeGeneration = isReleaseBuild ? Il2CppCodeGeneration.OptimizeSpeed : Il2CppCodeGeneration.OptimizeSize;
#endif

            var playerOptions = new BuildPlayerOptions
            {
                scenes = GetEnabledScenes(),
                target = BuildTarget.iOS,
                locationPathName = path
            };

            if (!isReleaseBuild)
            {
                playerOptions.options = BuildOptions.AllowDebugging | BuildOptions.Development;
            }

            // build addressable
            ExportAddressables();

            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

            // log an error if this code is skipped. (might happen when buildtarget is switched from code)
            bool postBuildExecuted = false;
#if UNITY_IOS
            XcodePostBuild.PostBuild(BuildTarget.iOS, report.summary.outputPath);
            postBuildExecuted = true;
#endif
            if (postBuildExecuted)
            {
                if (isReleaseBuild)
                {
                    Debug.Log("-- iOS Release Build: SUCCESSFUL --");
                }
                else
                {
                    Debug.Log("-- iOS Debug Build: SUCCESSFUL --");
                }
            }
            else
            {
                Debug.LogError("iOS export failed. Failed to modify Unity's Xcode project.");
            }
        }

        //#endregion


        //#region Other Member Methods
        private static void Copy(string source, string destinationPath, bool clearDestination = true)
        {
            if (clearDestination && Directory.Exists(destinationPath))
                Directory.Delete(destinationPath, true);

            Directory.CreateDirectory(destinationPath);

            foreach (var dirPath in Directory.GetDirectories(source, "*",
                         SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(source, destinationPath));

            foreach (var newPath in Directory.GetFiles(source, "*.*",
                         SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(source, destinationPath), true);
        }

        private static string[] GetEnabledScenes()
        {
            var scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            return scenes;
        }

        private static void ExportAddressables()
        {
#if USING_ADDRESSABLES
            Debug.Log("Start building player content (Addressables)");
            Debug.Log("BuildAddressablesProcessor.PreExport start");

            AddressableAssetSettings.CleanPlayerContent(
                AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);

            AddressableAssetProfileSettings profileSettings = AddressableAssetSettingsDefaultObject.Settings.profileSettings;
            string profileId = profileSettings.GetProfileId("Default");
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profileId;

            AddressableAssetSettings.BuildPlayerContent();
            Debug.Log("BuildAddressablesProcessor.PreExport done");
#endif
        }


        /// <summary>
        /// This method tries to autome the build setup required for Android
        /// </summary>
        private static void SetupAndroidProject()
        {
            var androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
            var androidAppPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/app"));
            var projBuildPath = Path.Combine(androidPath, "build.gradle");
            var appBuildPath = Path.Combine(androidAppPath, "build.gradle");
            var settingsPath = Path.Combine(androidPath, "settings.gradle");

            // switch to Kotlin DSL gradle if .kts file is detected (Fluter 3.29+ by default)
            if (File.Exists(projBuildPath + ".kts"))
            {
                SetupAndroidProjectKotlin();
                return;
            }

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


        // Copy of SetupAndroidProject() adapted to Kotlin DLS .gradle.kts. Generated since Flutter 3.29
        private static void SetupAndroidProjectKotlin()
        {
            var androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
            var androidAppPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/app"));
            var projBuildPath = Path.Combine(androidPath, "build.gradle.kts");
            var appBuildPath = Path.Combine(androidAppPath, "build.gradle.kts");
            var settingsPath = Path.Combine(androidPath, "settings.gradle.kts");


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
            dirs(file(""${project("":unityLibrary"").projectDir}/libs""))
        }
");
                File.WriteAllText(projBuildPath, projBuildScript);
            }

            // Sets up the project settings.gradle files correctly
            if (!Regex.IsMatch(settingsScript, @"include("":unityLibrary"")"))
            {
                settingsScript += @"

include("":unityLibrary"")
project("":unityLibrary"").projectDir = file(""./unityLibrary"")
";
                File.WriteAllText(settingsPath, settingsScript);
            }


            // Sets up the project app build.gradle files correctly
            if (!Regex.IsMatch(appBuildScript, @"dependencies \{"))
            {
                appBuildScript += @"
dependencies {
    implementation(project("":unityLibrary""))
}
";
                File.WriteAllText(appBuildPath, appBuildScript);
            }
            else
            {
                if (!appBuildScript.Contains(@"implementation(project("":unityLibrary"")"))
                {
                    var regex = new Regex(@"dependencies \{", RegexOptions.Multiline);
                    appBuildScript = regex.Replace(appBuildScript, @"
dependencies {
    implementation(project("":unityLibrary""))
");
                    File.WriteAllText(appBuildPath, appBuildScript);
                }
            }
        }

        /// <summary>
        /// This method tries to autome the build setup required for Android
        /// </summary>
        private static void SetupAndroidProjectForPlugin()
        {
            var androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
            var projBuildPath = Path.Combine(androidPath, "build.gradle");
            var settingsPath = Path.Combine(androidPath, "settings.gradle");

            if (File.Exists(projBuildPath + ".kts"))
            {
                SetupAndroidProjectForPluginKotlin();
                return;
            }

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

        // Copy of SetupAndroidProjectForPlugin() adapted to Kotlin DLS .gradle.kts. Generated since Flutter 3.29
        private static void SetupAndroidProjectForPluginKotlin()
        {
            var androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
            var projBuildPath = Path.Combine(androidPath, "build.gradle.kts");
            var settingsPath = Path.Combine(androidPath, "settings.gradle.kts");

            var projBuildScript = File.ReadAllText(projBuildPath);
            var settingsScript = File.ReadAllText(settingsPath);

            // Sets up the project build.gradle files correctly
            if (Regex.IsMatch(projBuildScript, @"// BUILD_ADD_UNITY_LIBS"))
            {
                var regex = new Regex(@"// BUILD_ADD_UNITY_LIBS", RegexOptions.Multiline);
                projBuildScript = regex.Replace(projBuildScript, @"
        flatDir {
            dirs(file(""${project("":unityLibrary"").projectDir}/libs""))
        }
");
                File.WriteAllText(projBuildPath, projBuildScript);
            }

            // Sets up the project settings.gradle files correctly
            if (!Regex.IsMatch(settingsScript, @"include("":unityLibrary"")"))
            {
                settingsScript += @"

include("":unityLibrary"")
project("":unityLibrary"").projectDir = file(""./unityLibrary"")
";
                File.WriteAllText(settingsPath, settingsScript);
            }
        }

        private static void SetupIOSProjectForPlugin()
        {
            var iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios"));
            var pubsecFile = Path.Combine(iosRunnerPath, "flutter_unity_widget.podspec");
            var pubsecText = File.ReadAllText(pubsecFile);

            if (!Regex.IsMatch(pubsecText, @"\w\.xcconfig(?:[^}]*})+") && !Regex.IsMatch(pubsecText, @"tar -xvjf UnityFramework.tar.bz2"))
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
        private static async void BuildUnityFrameworkArchive()
        {
            var xcprojectExt = "/Unity-iPhone.xcodeproj";

            // check if we have a workspace or not
            if (Directory.Exists(IOSExportPluginPath + "/Unity-iPhone.xcworkspace"))
            {
                xcprojectExt = "/Unity-iPhone.xcworkspace";
            }

            const string framework = "UnityFramework";
            var xcprojectName = $"{IOSExportPluginPath}{xcprojectExt}";
            var schemeName = $"{framework}";
            var buildPath = IOSExportPluginPath + "/build";
            var frameworkNameWithExt = $"{framework}.framework";

            var iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/"));
            const string iosArchiveDir = "Release-iphoneos-archive";
            var iosArchiveFrameworkPath = $"{buildPath}/{iosArchiveDir}/Products/Library/Frameworks/{frameworkNameWithExt}";
            var dysmNameWithExt = $"{frameworkNameWithExt}.dSYM";

            try
            {
                Debug.Log("### Cleaning up after old builds");
                await $" - rf {iosRunnerPath}{frameworkNameWithExt}".Bash("rm");
                await $" - rf {buildPath}".Bash("rm");

                Debug.Log("### BUILDING FOR iOS");
                Debug.Log("### Building for device (Archive)");

                await $"archive -workspace {xcprojectName} -scheme {schemeName} -sdk iphoneos -archivePath {buildPath}/Release-iphoneos.xcarchive ENABLE_BITCODE=NO |xcpretty".Bash("xcodebuild");

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


        // check if the Unity project is in the expected location
        private static bool IsProjectLocationValid(string unityLibraryPath, string platform)
        {
            // android, ios and web use platform/unityLibrary, move up one step.
            string platformPath = Path.Combine(unityLibraryPath, "../");
            if (!Directory.Exists(platformPath))
            {
                Debug.LogError($"Could not find the Flutter project {platform} folder. Make sure the Unity project folder is located in '<flutter-project>/unity/<unity-project-folder>' .");
                Debug.Log($"-- Build: Failed --");
                return false;
            }
            return true;
        }

        //#endregion
    }
}
