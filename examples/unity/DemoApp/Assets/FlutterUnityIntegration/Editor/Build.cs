using UnityEditor;
using UnityEngine;

// uncomment for addressables
//using UnityEditor.AddressableAssets;
//using UnityEditor.AddressableAssets.Settings;

namespace FlutterUnityIntegration.Editor
{
    /// <summary>
    /// Represents a Unity Editor window for exporting Flutter projects to different platforms.
    /// </summary>
    public class Build : EditorWindow
    {
        /// <summary>
        /// The key used to access the Android project path value in the Editor's preferences.
        /// </summary>
        private string editorAndroidPrefKey = "BuildWindow.androidProjectPath";

        /// <summary>
        /// Represents the key used to store the Android plugin project path in the editor preferences.
        /// </summary>
        private string editorAndroidPluginPrefKey = "BuildWindow.androidPluginProjectPath";

        /// <summary>
        /// The variable <see cref="editorIOSPrefKey"/> represents the key used to store and retrieve the iOS project path in the build window preferences.
        /// </summary>
        private string editorIOSPrefKey = "BuildWindow.iosProjectPath";

        /// <summary>
        /// The key used to store the iOS plugin project path in the editor preferences.
        /// </summary>
        private string editorIOSPluginPrefKey = "BuildWindow.iosPluginProjectPath";

        /// <summary>
        /// Represents the key used in editor preferences to store the web project path for the build window.
        /// </summary>
        private string editorWebPrefKey = "BuildWindow.webProjectPath";

        /// /
        private string editorWindowsPrefKey = "BuildWindow.windowsProjectPath";

        /// <summary>
        /// The path to the Android project.
        /// </summary>
        /// <value>
        /// A string representing the file path to the Android project.
        /// </value>
        private static string androidProjectPath = "";

        /// <summary>
        /// Represents the file path of the Android plugin project.
        /// </summary>
        private static string androidPluginProjectPath = "";

        /// <summary>
        /// Represents the path to the iOS project. </summary>
        /// /
        private static string iosProjectPath = "";

        /// <summary>
        /// The path to the iOS plugin project.
        /// </summary>
        private static string iosPluginProjectPath = "";

        /// <summary>
        /// Represents the file path to the web project.
        /// </summary>
        private static string webProjectPath = "";

        /// <summary>
        /// The path of the Windows project.
        /// </summary>
        private static string windowsProjectPath = "";

        /// <summary>
        /// Default export path for Android project in Unity.
        /// </summary>
        private static readonly string defaultAndroidExportPath = "../../standard/android/unityLibrary";

        /// <summary>
        /// The default export path for Android plugins in Unity.
        /// </summary>
        private static readonly string defaultAndroidPluginExportPath = "../../standard/android/unityLibrary";

        /// <summary>
        /// The default export path for Windows platform in Unity Library.
        /// </summary>
        private static readonly string defaultWindowsExportPath = "../../standard/windows/unityLibrary/data";

        /// <summary>
        /// The default export path for iOS projects in Unity.
        /// </summary>
        private static readonly string defaultIOSExportPath = "../../standard/ios/UnityLibrary";

        /// <summary>
        /// The default path for web export in UnityLibrary.
        /// </summary>
        private static readonly string defaultWebExportPath = "../../standard/web/UnityLibrary";

        /// <summary>
        /// The default path for the iOS export plugin.
        /// </summary>
        private static readonly string defaultIOSExportPluginPath = "../../standard/ios_xcode/UnityLibrary";

        /// <summary>
        /// This method is used to build an Android library in debug mode using the Flutter framework.
        /// </summary>
        /// <remarks>
        /// This method is invoked when the "Export Android (Debug)" menu item is clicked. It generates a build with the specified build options.
        /// </remarks>
        [MenuItem("Flutter/Export Android (Debug) %&n", false, 101)]
        public static void DoBuildAndroidLibraryDebug() => GenerateBuild<BuildAndroid>(new FuwBuildOptions
        {
            OutputDir = androidProjectPath,
            PackageMode = false,
            Release = false
        });

        /// <summary>
        /// Builds the Android library in release mode.
        /// </summary>
        /// <remarks>
        /// This method is called when the "Export Android (Release)" menu item is clicked.
        /// It generates the build using the <see cref="BuildAndroid"/> class and the specified build options.
        /// The build options include the output directory, build path, package mode, and release mode.
        /// </remarks>
        [MenuItem("Flutter/Export Android (Release) %&m", false, 102)]
        public static void DoBuildAndroidLibraryRelease() => GenerateBuild<BuildAndroid>(new FuwBuildOptions
        {
            OutputDir = androidProjectPath,
            BuildPath = "unityLibrary",
            PackageMode = false,
            Release = true
        });

        /// <summary>
        /// Builds an Android plugin for the Flutter project.
        /// </summary>
        /// <remarks>
        /// This method is invoked via the menu item "Flutter/Export Android Plugin" with the shortcut
        /// "%&p". It generates the build using the <see cref="BuildAndroid"/> class.
        /// </remarks>
        [MenuItem("Flutter/Export Android Plugin %&p", false, 103)]
        public static void DoBuildAndroidPlugin() => GenerateBuild<BuildAndroid>(new FuwBuildOptions
        {
            OutputDir = androidPluginProjectPath,
            BuildPath = "unityLibrary",
            PackageMode = true,
            Release = true
        });

        /// <summary>
        /// This method is used to build an iOS debug version of a Flutter project.
        /// It is executed when the menu item "Flutter/Export IOS (Debug)" is clicked.
        /// </summary>
        /// <remarks>
        /// The generated build will have the following options:
        /// - Output directory will be set to the 'iosProjectPath' variable
        /// - Package mode will be disabled
        /// - Release mode will be disabled
        /// </remarks>
        [MenuItem("Flutter/Export IOS (Debug) %&i", false, 201)]
        public static void DoBuildIOSDebug() => GenerateBuild<BuildIOS>(new FuwBuildOptions
        {
            OutputDir = iosProjectPath,
            PackageMode = false,
            Release = false
        });

        /// <summary>
        /// Builds the IOS project for release.
        /// </summary>
        /// <remarks>
        /// This method is triggered when the menu item "Flutter/Export IOS (Release)" is clicked. It executes the GenerateBuild method
        /// by passing in BuildIOS as the generic type and a new instance of FuwBuildOptions with the necessary parameters for building
        /// the IOS project in release mode.
        /// </remarks>
        [MenuItem("Flutter/Export IOS (Release) %&ir", false, 202)]
        public static void DoBuildIOSRelease() => GenerateBuild<BuildIOS>(new FuwBuildOptions
        {
            OutputDir = iosProjectPath,
            PackageMode = false,
            Release = true
        });

        /// <summary>
        /// Builds the iOS plugin project for exporting.
        /// </summary>
        /// <remarks>
        /// This method is used to generate an iOS plugin build using the specified build options.
        /// It is triggered when the "Export IOS Plugin" menu item is clicked.
        /// </remarks>
        [MenuItem("Flutter/Export IOS Plugin %&o", false, 203)]
        public static void DoBuildIOSPlugin() => GenerateBuild<BuildIOS>(new FuwBuildOptions
        {
            OutputDir = iosPluginProjectPath,
            PackageMode = true,
            Release = true
        });

        /// <summary>
        /// Method to build the WebGL project.
        /// </summary>
        [MenuItem("Flutter/Export Web GL %&w", false, 301)]
        public static void DoBuildWebGL() => GenerateBuild<BuildWeb>(new FuwBuildOptions
        {
            OutputDir = webProjectPath,
            PackageMode = true,
            Release = true
        });

        /// <summary>
        /// Builds the Windows OS version of the Flutter application.
        /// </summary>
        /// <remarks>
        /// This method is invoked when the user selects the "Export Windows" option from the Flutter menu.
        /// It generates the build using the <see cref="BuildWindows"/> build configuration and the provided build options.
        /// </remarks>
        [MenuItem("Flutter/Export Windows %&d", false, 401)]
        public static void DoBuildWindowsOS() => GenerateBuild<BuildWindows>(new FuwBuildOptions
        {
            OutputDir = windowsProjectPath,
            Release = true
        });

        /// <summary>
        /// Opens the plugin settings window.
        /// </summary>
        [MenuItem("Flutter/Settings %&S", false, 501)]
        public static void PluginSettings()
        {
            GetWindow(typeof(Build));
        }

        /// Description: This method is called by Unity every frame when rendering or handling GUI events.
        /// It is responsible for displaying the settings GUI for the Flutter Unity Widget.
        /// Remarks:
        /// - The method displays a series of labels and text fields to specify various output directory paths.
        /// - The directory paths must be relative to the Unity project.
        /// - Two modes are available: Library Mode and Package Mode.
        /// - In Library Mode, the method allows specifying output directories for Android, iOS, Web, and Windows.
        /// - In Package Mode, the method allows specifying output directories for Android and iOS plugins.
        /// - The method uses EditorGUILayout.TextField to create text fields for inputting directory paths.
        /// Example Usage:
        /// // This method is automatically called by Unity during rendering or handling GUI events.
        /// private void OnGUI()
        /// {
        /// // Display labels for settings sections.
        /// GUILayout.Label("Flutter Unity Widget Settings", EditorStyles.boldLabel);
        /// GUILayout.Label("The dir path must be relative to the unity project", EditorStyles.miniLabel);
        /// // Display the Library Mode settings.
        /// GUILayout.Label("Library Mode", EditorStyles.miniBoldLabel);
        /// androidProjectPath = EditorGUILayout.TextField("Android Output Dir", androidProjectPath);
        /// iosProjectPath = EditorGUILayout.TextField("iOS Output Dir", iosProjectPath);
        /// webProjectPath = EditorGUILayout.TextField("Web Output Dir", webProjectPath);
        /// windowsProjectPath = EditorGUILayout.TextField("Windows Output Dir", windowsProjectPath);
        /// // Display the Package Mode settings.
        /// GUILayout.Label("Package Mode", EditorStyles.miniBoldLabel);
        /// androidPluginProjectPath = EditorGUILayout.TextField("Android Plugin Output Dir", androidPluginProjectPath);
        /// iosPluginProjectPath = EditorGUILayout.TextField("iOS Plugin Output Dir", iosPluginProjectPath);
        /// // Save the settings if any changes are made to the GUI.
        /// if (!GUI.changed) return;
        /// EditorPrefs.SetString(editorAndroidPrefKey, androidProjectPath);
        /// EditorPrefs.SetString(editorAndroidPluginPrefKey, androidPluginProjectPath);
        /// EditorPrefs.SetString(editorIOSPrefKey, iosProjectPath);
        /// EditorPrefs.SetString(editorIOSPluginPrefKey, iosPluginProjectPath);
        /// EditorPrefs.SetString(editorWebPrefKey, webProjectPath);
        /// EditorPrefs.SetString(editorWindowsPrefKey, windowsProjectPath);
        /// }
        /// /
        private void OnGUI()
        {
            GUILayout.Label("Flutter Unity Widget Settings", EditorStyles.boldLabel);
            GUILayout.Label("The dir path must be relative to the unity project", EditorStyles.miniLabel);

            GUILayout.Label("Library Mode", EditorStyles.miniBoldLabel);
            androidProjectPath = EditorGUILayout.TextField("Android Output Dir", androidProjectPath);
            iosProjectPath = EditorGUILayout.TextField("iOS Output Dir", iosProjectPath);
            webProjectPath = EditorGUILayout.TextField("Web Output Dir", webProjectPath);
            windowsProjectPath = EditorGUILayout.TextField("Windows Output Dir", windowsProjectPath);

            GUILayout.Label("Package Mode", EditorStyles.miniBoldLabel);
            androidPluginProjectPath = EditorGUILayout.TextField("Android Plugin Output Dir", androidPluginProjectPath);
            iosPluginProjectPath = EditorGUILayout.TextField("iOS Plugin Output Dir", iosPluginProjectPath);

            if (!GUI.changed) return;

            EditorPrefs.SetString(editorAndroidPrefKey, androidProjectPath);
            EditorPrefs.SetString(editorAndroidPluginPrefKey, androidPluginProjectPath);
            EditorPrefs.SetString(editorIOSPrefKey, iosProjectPath);
            EditorPrefs.SetString(editorIOSPluginPrefKey, iosPluginProjectPath);
            EditorPrefs.SetString(editorWebPrefKey, webProjectPath);
            EditorPrefs.SetString(editorWindowsPrefKey, windowsProjectPath);
        }

        /// <summary>
        /// This method is called when the script instance is being loaded.
        /// </summary>
        private void OnEnable()
        {
            androidProjectPath = EditorPrefs.GetString(editorAndroidPrefKey, defaultAndroidExportPath);
            androidPluginProjectPath =
                EditorPrefs.GetString(editorAndroidPluginPrefKey, defaultAndroidPluginExportPath);
            iosProjectPath = EditorPrefs.GetString(editorIOSPrefKey, defaultIOSExportPath);
            webProjectPath = EditorPrefs.GetString(editorWebPrefKey, defaultWebExportPath);
            windowsProjectPath = EditorPrefs.GetString(editorWindowsPrefKey, defaultWindowsExportPath);
            iosPluginProjectPath = EditorPrefs.GetString(editorIOSPluginPrefKey, defaultIOSExportPluginPath);
        }

        /// <summary>
        /// Generates a build using the specified builder type and options.
        /// </summary>
        /// <typeparam name="T">The type of builder implementing the IFuwBuilder interface.</typeparam>
        /// <param name="options">The options for the build.</param>
        private static void GenerateBuild<T>(FuwBuildOptions options) where T : IFuwBuilder, new()
        {
            var builder = new T() { Options = options };
            builder.Build();
            builder.Export();
        }

        // skipped for brevity
    }
}