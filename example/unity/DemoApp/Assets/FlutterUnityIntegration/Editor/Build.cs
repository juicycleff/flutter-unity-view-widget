using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

public class Build
{
    static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

    static readonly string apkPath = Path.Combine(ProjectPath, "Builds/" + Application.productName + ".apk");

    static readonly string androidExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/unityLibrary"));
    static readonly string iosExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/UnityLibrary"));
    static readonly string iosExportPluginPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios_xcode/UnityLibrary"));

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

    [MenuItem("Flutter/Legacy/Export Android", false, 5)]
    public static void DoBuildAndroidLegacy()
    {
        DoBuildAndroid(Path.Combine(apkPath, Application.productName), true);
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
            AndroidGetSomeRestWillYaPlugin();
        } else
        {
            AndroidGetSomeRestWillYa();
        }
    }

    [MenuItem("Flutter/Export IOS %&i", false, 3)]
    public static void DoBuildIOS()
    {
        if (Directory.Exists(iosExportPath))
            Directory.Delete(iosExportPath, true);

        EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

        var options = BuildOptions.AcceptExternalModificationsToPlayer;
        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            iosExportPath,
            BuildTarget.iOS,
            options
        );

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");

        // Automate so manual steps
        GetSomeRestWillYa();
    }


    [MenuItem("Flutter/Export IOS Plugin %&o", false, 4)]
    public static void DoBuildIOSPlugin()
    {
        if (Directory.Exists(iosExportPluginPath))
            Directory.Delete(iosExportPluginPath, true);

        EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

        var options = BuildOptions.AcceptExternalModificationsToPlayer;

        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            iosExportPluginPath,
            BuildTarget.iOS,
            options
        );


        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");

        // Automate so manual steps
        GetSomeRestWillYaPlugin();

        // Build Archive
        buildArchive();

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
    static void AndroidGetSomeRestWillYa()
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
            if (!Regex.IsMatch(app_build_script, @"implementation project(':unityLibrary')"))
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
    static void AndroidGetSomeRestWillYaPlugin()
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

    static void GetSomeRestWillYa()
    {
        string iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/Runner"));
        var info_file = Path.Combine(iosRunnerPath, "info.plist");
        var info_text = File.ReadAllText(info_file);

        if (!Regex.IsMatch(info_text, @"<key>io.flutter.embedded_views_preview</key>"))
        {
            Regex regex = new Regex(@"</dict>", RegexOptions.Multiline);
            info_text = regex.Replace(info_text, @"
				<key>io.flutter.embedded_views_preview</key>
				<true/>
			</dict>
			");
            File.WriteAllText(info_file, info_text);
        }
    }

    static void GetSomeRestWillYaPlugin()
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

    static async void buildArchive() {
        string framework = "UnityFramework";
        string xcproj_name = iosExportPluginPath+"/Unity-iPhone.xcworkspace";
        string scheme_name = $"{framework}";
        string project_dir = iosExportPluginPath;
        string buildPath = iosExportPluginPath + "/build";
        string framework_name_with_ext = $"{framework}.framework";
        string iosRunnerPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/"));
        string framePath = Path.GetFullPath(Path.Combine(iosRunnerPath, $"{framework}"));
        string ios_archive_dir = "Release-iphoneos-archive";
        string ios_universal_dir = "Release-universal-iOS";
        string ios_archive_framework_path = $"{buildPath}/{ios_archive_dir}/Products/Library/Frameworks/{framework_name_with_ext}";
        string dsym_name_with_ext = $"{framework_name_with_ext}.dSYM";

        try
        {
            Debug.Log("### Cleaning up after old builds");
            await $" - rf {framePath}.tar.bz2".Bash("rm");
            await $" - rf {buildPath}".Bash("rm");
        } catch (Exception)
        {
            // Debug.Log(e);
        }


        try
        {
            Debug.Log("### BUILDING FOR iOS");
            Debug.Log("### Building for device (Archive)");
            await $"archive -workspace {xcproj_name} -scheme {scheme_name} - sdk iphoneos -archivePath {buildPath}/Release-iphoneos.xcarchive ENABLE_BITCODE=NO |xcpretty".Bash("xcodebuild");

            Debug.Log("### Copying framework files");
            await $" {buildPath}/Release-iphoneos.xcarchive {buildPath}/{ios_archive_dir}".Bash("mv");
            await $" -p {buildPath}/{ios_universal_dir}".Bash("mkdir");

            Debug.Log("### Copying framework files");
            await $" -RL {ios_archive_framework_path} {buildPath}/{ios_universal_dir}/{framework_name_with_ext}".Bash("cp");
            await $" -RL {ios_archive_framework_path}/{dsym_name_with_ext} {buildPath}/{ios_universal_dir}/{dsym_name_with_ext}".Bash("cp");

            Debug.Log("### Copying iOS files into zip directory");
            string target_dir = iosRunnerPath;
            await $" -cjvf {target_dir}/{framework}.tar.bz2 {buildPath}/{ios_universal_dir}/{framework_name_with_ext} {buildPath}/{ios_universal_dir}/{dsym_name_with_ext}".Bash("tar");

            Debug.Log("### Zipped resulting framework and dSYM to $TAR_DIR/$FRAMEWORK.tar.bz2");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


    }
}
