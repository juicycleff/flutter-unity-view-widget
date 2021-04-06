using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

public class Build : EditorWindow
{
    static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

    static readonly string apkPath = Path.Combine(ProjectPath, "Builds/" + Application.productName + ".apk");

    static readonly string androidExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/unityLibrary"));
    static readonly string iosExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/UnityLibrary"));
    static readonly string iosExportPluginPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios_xcode/UnityLibrary"));

    bool pluginMode = false;
    static string persistentKey = "flutter-unity-widget-pluginMode";

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

    public static void DoBuildAndroid(String buildPath, bool isPlugin)
    {
        if (Directory.Exists(apkPath))
            Directory.Delete(apkPath, true);

        if (Directory.Exists(androidExportPath))
            Directory.Delete(androidExportPath, true);

        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        var options = BuildOptions.AcceptExternalModificationsToPlayer;
        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            apkPath,
            BuildTarget.Android,
            options
        );

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");

        Copy(buildPath, androidExportPath);

        // Modify build.gradle
        var build_file = Path.Combine(androidExportPath, "build.gradle");
        var build_text = File.ReadAllText(build_file);
        build_text = build_text.Replace("com.android.application", "com.android.library");
        build_text = build_text.Replace("bundle {", "splits {");
        build_text = build_text.Replace("enableSplit = false", "enable false");
        build_text = build_text.Replace("enableSplit = true", "enable true");
        build_text = build_text.Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])", "implementation(name: 'unity-classes', ext:'jar')");
        build_text = Regex.Replace(build_text, @"\n.*applicationId '.+'.*\n", "\n");
        File.WriteAllText(build_file, build_text);

        // Modify AndroidManifest.xml
        var manifest_file = Path.Combine(androidExportPath, "src/main/AndroidManifest.xml");
        var manifest_text = File.ReadAllText(manifest_file);
        manifest_text = Regex.Replace(manifest_text, @"<application .*>", "<application>");
        Regex regex = new Regex(@"<activity.*>(\s|\S)+?</activity>", RegexOptions.Multiline);
        manifest_text = regex.Replace(manifest_text, "");
        File.WriteAllText(manifest_file, manifest_text);

        if(isPlugin)
        {
            SetupAndroidProjectForPlugin();
        } else
        {
            SetupAndroidProject();
        }
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

    private static void BuildIOS(String path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, true);

        EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

        var options = BuildOptions.AcceptExternalModificationsToPlayer;
        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            path,
            BuildTarget.iOS,
            options
        );

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");
    }

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
}
