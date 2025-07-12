using System.IO;
using UnityEngine;

namespace FlutterUnityIntegration.Editor
{
    public static class BuildPathsHelper 
    {
        private static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        public static string Overriden_AndroidExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/unityLibrary"));
        public static string Overriden_WindowsExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../windows/unityLibrary/data"));
        public static string Overriden_IOSExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/UnityLibrary"));
        public static string Overriden_WebExportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../web/UnityLibrary"));
        public static string Overriden_IOSExportPluginPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../ios_xcode/UnityLibrary"));

        public static string _persistentAndroidExportPath = "flutter-unity-widget-andriodExportPath";
        public static string _persistentWindowsExportPath = "flutter-unity-widget-WindowsExportPath";
        public static string _persistentIOSExportPath = "flutter-unity-widget-IOSExportPath";
        public static string _persistentWebExportPath = "flutter-unity-widget-WebExportPath";
        public static string _persistentIOSExportPluginPath = "flutter-unity-widget-IOSExportPluginPath";
       

    }
}
