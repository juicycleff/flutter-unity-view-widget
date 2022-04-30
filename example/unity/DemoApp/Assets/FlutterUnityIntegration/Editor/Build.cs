using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

// uncomment for addressables
//using UnityEditor.AddressableAssets;
//using UnityEditor.AddressableAssets.Settings;

public class Build : EditorWindow
{
    static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

    static readonly string apkPath = Path.Combine(ProjectPath, "Builds/" + Application.productName + ".apk");

    static readonly string androidExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/unityLibrary"));
    static readonly string iosExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/UnityLibrary"));
    static readonly string iosExportPluginPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios_xcode/UnityLibrary"));

    bool pluginMode = false;
    static string persistentKey = "flutter-unity-widget-pluginMode";

    //#region GUI Member Methods
    [MenuItem("Flutter/Export Android %&n", false, 1)]
    public static void DoBuildAndroidLibrary()
    {
        DoBuildAndroid(Path.Combine(apkPath, "unityLibrary"), false);

        // Copy over resources from the launcher module that are used by the library
        Copy(Path.Combine(apkPath + "/launcher/src/main/res"), Path.Combine(androidExportPath, "src/main/res"));
    }

    [MenuItem("Flutter/Export Android Plugin %&p", false, 2)]
    public static void DoBuildAndroidPlugin()
    {
        DoBuildAndroid(Path.Combine(apkPath, "unityLibrary"), true);

        // Copy over resources from the launcher module that are used by the library
        Copy(Path.Combine(apkPath + "/launcher/src/main/res"), Path.Combine(androidExportPath, "src/main/res"));
    }

    [MenuItem("Flutter/Export IOS %&i", false, 3)]
    public static void DoBuildIOS()
    {
        BuildIOS(iosExportPath);
    }

    [MenuItem("Flutter/Export IOS Plugin %&o", false, 4)]
    public static void DoBuildIOSPlugin()
    {
        BuildIOS(iosExportPluginPath);

        // Automate so manual steps
        SetupIOSProjectForPlugin();

        // Build Archive
        // BuildUnityFrameworkArchive();

    }

    [MenuItem("Flutter/Settings %&S", false, 5)]
    public static void PluginSettings()
    {
        EditorWindow.GetWindow(typeof(Build));
    }

    void OnGUI()
    {
        GUILayout.Label("Flutter Unity Widget Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        pluginMode = EditorGUILayout.Toggle("Plugin Mode", pluginMode);

        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetBool(persistentKey, pluginMode);
        }
    }

    private void OnEnable()
    {
      pluginMode = EditorPrefs.GetBool(persistentKey, false);
    }
    //#endregion


    //#region Build Member Methods

    public static void DoBuildAndroid(String buildPath, bool isPlugin)
    {
        // Switch to Android standalone build.
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        if (Directory.Exists(apkPath))
            Directory.Delete(apkPath, true);

        if (Directory.Exists(androidExportPath))
            Directory.Delete(androidExportPath, true);

        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

        var playerOptions = new BuildPlayerOptions();
        playerOptions.scenes = GetEnabledScenes();
        playerOptions.target = BuildTarget.Android;
        playerOptions.locationPathName = apkPath;
        playerOptions.options = BuildOptions.AllowDebugging;

        // Switch to Android standalone build.
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        // build addressable
        ExportAddressables();
        var report = BuildPipeline.BuildPlayer(playerOptions);

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");

        Copy(buildPath, androidExportPath);

        // Modify build.gradle
        ModifyAndroidGradle(isPlugin);

        if(isPlugin)
        {
            SetupAndroidProjectForPlugin();
        } else
        {
            SetupAndroidProject();
        }
    }

    private static void ModifyAndroidGradle(bool isPlugin)
    {
        // Modify build.gradle
        var build_file = Path.Combine(androidExportPath, "build.gradle");
        var build_text = File.ReadAllText(build_file);
        build_text = build_text.Replace("com.android.application", "com.android.library");
        build_text = build_text.Replace("bundle {", "splits {");
        build_text = build_text.Replace("enableSplit = false", "enable false");
        build_text = build_text.Replace("enableSplit = true", "enable true");
        build_text = build_text.Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])", "implementation(name: 'unity-classes', ext:'jar')");
        build_text = build_text.Replace(" + unityStreamingAssets.tokenize(', ')", "");

        if(isPlugin)
        {
            build_text = Regex.Replace(build_text, @"implementation\(name: 'androidx.* ext:'aar'\)", "\n");
        }
//        build_text = Regex.Replace(build_text, @"commandLineArgs.add\(\"--enable-debugger\"\)", "\n");
//        build_text = Regex.Replace(build_text, @"commandLineArgs.add\(\"--profiler-report\"\)", "\n");
//        build_text = Regex.Replace(build_text, @"commandLineArgs.add\(\"--profiler-output-file=\" + workingDir + \"/build/il2cpp_\"+ abi + \"_\" + configuration + \"/il2cpp_conv.traceevents\"\)", "\n");

        build_text = Regex.Replace(build_text, @"\n.*applicationId '.+'.*\n", "\n");
        File.WriteAllText(build_file, build_text);

        // Modify AndroidManifest.xml
        var manifest_file = Path.Combine(androidExportPath, "src/main/AndroidManifest.xml");
        var manifest_text = File.ReadAllText(manifest_file);
        manifest_text = Regex.Replace(manifest_text, @"<application .*>", "<application>");
        Regex regex = new Regex(@"<activity.*>(\s|\S)+?</activity>", RegexOptions.Multiline);
        manifest_text = regex.Replace(manifest_text, "");
        File.WriteAllText(manifest_file, manifest_text);

        // Modify proguard-unity.txt
        var proguard_file = Path.Combine(androidExportPath, "proguard-unity.txt");
        var proguard_text = File.ReadAllText(proguard_file);
        proguard_text = proguard_text.Replace("-ignorewarnings", "-keep class com.xraph.plugin.** { *; }\n-ignorewarnings");
        File.WriteAllText(proguard_file, proguard_text);

    }

    private static void BuildIOS(String path)
    {
        // Switch to ios standalone build.
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

        if (Directory.Exists(path))
            Directory.Delete(path, true);

        EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;

        var playerOptions = new BuildPlayerOptions();
        playerOptions.scenes = GetEnabledScenes();
        playerOptions.target = BuildTarget.iOS;
        playerOptions.locationPathName = path;
        playerOptions.options = BuildOptions.AllowDebugging;

        // build addressable
        ExportAddressables();

        var report = BuildPipeline.BuildPlayer(playerOptions);

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");
    }

    //#endregion


    //#region Other Member Methods
    static void Copy(string source, string destinationPath)
    {
        if (Directory.Exists(destinationPath))
            Directory.Delete(destinationPath, true);

        Directory.CreateDirectory(destinationPath);

        foreach (string dirPath in Directory.GetDirectories(source, "*",
            SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(source, destinationPath));

        foreach (string newPath in Directory.GetFiles(source, "*.*",
            SearchOption.AllDirectories))
            File.Copy(newPath, newPath.Replace(source, destinationPath), true);
    }

    static string[] GetEnabledScenes()
    {
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        return scenes;
    }

    // uncomment for addressables
    private static void ExportAddressables() {
        /*
        Debug.Log("Start building player content (Addressables)");
        Debug.Log("BuildAddressablesProcessor.PreExport start");

        AddressableAssetSettings.CleanPlayerContent(
            AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);

        AddressableAssetProfileSettings profileSettings = AddressableAssetSettingsDefaultObject.Settings.profileSettings;
        string profileId = profileSettings.GetProfileId("Default");
        AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profileId;

        AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("BuildAddressablesProcessor.PreExport done");
        */
    }


    /// <summary>
    /// This method tries to autome the build setup required for Android
    /// </summary>
    static void SetupAndroidProject()
    {
        string androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
        string androidAppPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/app"));
        var proj_build_path = Path.Combine(androidPath, "build.gradle");
        var app_build_path = Path.Combine(androidAppPath, "build.gradle");
        var settings_path = Path.Combine(androidPath, "settings.gradle");

        var proj_build_script = File.ReadAllText(proj_build_path);
        var settings_script = File.ReadAllText(settings_path);
        var app_build_script = File.ReadAllText(app_build_path);

        // Sets up the project build.gradle files correctly
        if (!Regex.IsMatch(proj_build_script, @"flatDir[^/]*[^}]*}"))
        {
            Regex regex = new Regex(@"allprojects \{[^\{]*\{", RegexOptions.Multiline);
            proj_build_script = regex.Replace(proj_build_script, @"
allprojects {
    repositories {
        flatDir {
            dirs ""${project(':unityLibrary').projectDir}/libs""
        }
");
            File.WriteAllText(proj_build_path, proj_build_script);
        }

        // Sets up the project settings.gradle files correctly
        if (!Regex.IsMatch(settings_script, @"include "":unityLibrary"""))
        {
            settings_script += @"

include "":unityLibrary""
project("":unityLibrary"").projectDir = file(""./unityLibrary"")
";
            File.WriteAllText(settings_path, settings_script);
        }


        // Sets up the project app build.gradle files correctly
        if (!Regex.IsMatch(app_build_script, @"dependencies \{"))
        {
            app_build_script += @"
dependencies {
    implementation project(':unityLibrary')
}
";
            File.WriteAllText(app_build_path, app_build_script);
        } else
        {
            if (!app_build_script.Contains(@"implementation project(':unityLibrary')"))
            {
                Regex regex = new Regex(@"dependencies \{", RegexOptions.Multiline);
                app_build_script = regex.Replace(app_build_script, @"
dependencies {
    implementation project(':unityLibrary')
");
                File.WriteAllText(app_build_path, app_build_script);
            }
        }
    }

    /// <summary>
    /// This method tries to autome the build setup required for Android
    /// </summary>
    static void SetupAndroidProjectForPlugin()
    {
        string androidPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android"));
        var proj_build_path = Path.Combine(androidPath, "build.gradle");
        var settings_path = Path.Combine(androidPath, "settings.gradle");

        var proj_build_script = File.ReadAllText(proj_build_path);
        var settings_script = File.ReadAllText(settings_path);

        // Sets up the project build.gradle files correctly
        if (Regex.IsMatch(proj_build_script, @"// BUILD_ADD_UNITY_LIBS"))
        {
            Regex regex = new Regex(@"// BUILD_ADD_UNITY_LIBS", RegexOptions.Multiline);
            proj_build_script = regex.Replace(proj_build_script, @"
        flatDir {
            dirs ""${project(':unityLibrary').projectDir}/libs""
        }
");
            File.WriteAllText(proj_build_path, proj_build_script);
        }

        // Sets up the project settings.gradle files correctly
        if (!Regex.IsMatch(settings_script, @"include "":unityLibrary"""))
        {
            settings_script += @"

include "":unityLibrary""
project("":unityLibrary"").projectDir = file(""./unityLibrary"")
";
            File.WriteAllText(settings_path, settings_script);
        }
    }

    static void SetupIOSProjectForPlugin()
    {
        string iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios"));
        var pubsec_file = Path.Combine(iosRunnerPath, "flutter_unity_widget.podspec");
        var pubsec_text = File.ReadAllText(pubsec_file);

        if (!Regex.IsMatch(pubsec_text, @"\w\.xcconfig(?:[^}]*})+") && !Regex.IsMatch(pubsec_text, @"tar -xvjf UnityFramework.tar.bz2"))
        {
            Regex regex = new Regex(@"\w\.xcconfig(?:[^}]*})+", RegexOptions.Multiline);
            pubsec_text = regex.Replace(pubsec_text, @"
	spec.xcconfig = {
        'FRAMEWORK_SEARCH_PATHS' => '""${PODS_ROOT}/../.symlinks/plugins/flutter_unity_widget/ios"" ""${PODS_ROOT}/../.symlinks/flutter/ios-release"" ""${PODS_CONFIGURATION_BUILD_DIR}""',
        'OTHER_LDFLAGS' => '$(inherited) -framework UnityFramework \${PODS_LIBRARIES}'
    }

    spec.vendored_frameworks = ""UnityFramework.framework""
			");
            File.WriteAllText(pubsec_file, pubsec_text);
        }
    }

    // DO NOT USE (Contact before trying)
    static async void BuildUnityFrameworkArchive()
    {
        string XCPROJECT_EXT = "/Unity-iPhone.xcodeproj";

        // check if we have a workspace or not
        if (Directory.Exists(iosExportPluginPath + "/Unity-iPhone.xcworkspace")) {
            XCPROJECT_EXT = "/Unity-iPhone.xcworkspace";
        }

        string FRAMEWORK = "UnityFramework";
        string XCPROJECT_NAME = $"{iosExportPluginPath}{XCPROJECT_EXT}";
        string SCHEME_NAME = $"{FRAMEWORK}";
        string BUILD_PATH = iosExportPluginPath + "/build";
        string FRAMEWORK_NAME_WITH_EXT = $"{FRAMEWORK}.framework";

        string IOS_RUNNER_PATH = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/"));
        string IOS_ARCHIVE_DIR = "Release-iphoneos-archive";
        string IOS_ARCHIVE_FRAMEWORK_PATH = $"{BUILD_PATH}/{IOS_ARCHIVE_DIR}/Products/Library/Frameworks/{FRAMEWORK_NAME_WITH_EXT}";
        string DYSM_NAME_WITH_EXT = $"{FRAMEWORK_NAME_WITH_EXT}.dSYM";

        try
        {
            Debug.Log("### Cleaning up after old builds");
            await $" - rf {IOS_RUNNER_PATH}{FRAMEWORK_NAME_WITH_EXT}".Bash("rm");
            await $" - rf {BUILD_PATH}".Bash("rm");

            Debug.Log("### BUILDING FOR iOS");
            Debug.Log("### Building for device (Archive)");

            await $"archive -workspace {XCPROJECT_NAME} -scheme {SCHEME_NAME} -sdk iphoneos -archivePath {BUILD_PATH}/Release-iphoneos.xcarchive ENABLE_BITCODE=NO |xcpretty".Bash("xcodebuild");

            Debug.Log("### Copying framework files");
            await $" -RL {IOS_ARCHIVE_FRAMEWORK_PATH} {IOS_RUNNER_PATH}/{FRAMEWORK_NAME_WITH_EXT}".Bash("cp");
            await $" -RL {IOS_ARCHIVE_FRAMEWORK_PATH}/{DYSM_NAME_WITH_EXT} {IOS_RUNNER_PATH}/{DYSM_NAME_WITH_EXT}".Bash("cp");
            Debug.Log("### DONE ARCHIVING");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


    }

    //#endregion
}
