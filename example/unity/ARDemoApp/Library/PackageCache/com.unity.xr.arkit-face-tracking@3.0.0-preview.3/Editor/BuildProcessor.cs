#if UNITY_IOS
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.XR.ARKit;

namespace UnityEditor.XR.ARKit
{
    class BuildProcessor : IPreprocessBuildWithReport
    {
        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                const string pluginPath = "Packages/com.unity.xr.arkit-face-tracking/Runtime/iOS";
                LibUtil.SelectPlugin(
                    PluginImporter.GetAtPath($"{pluginPath}/Xcode1000/libUnityARKitFaceTracking.a") as PluginImporter,
                    PluginImporter.GetAtPath($"{pluginPath}/Xcode1100/libUnityARKitFaceTracking.a") as PluginImporter);
            }
        }

        public int callbackOrder { get { return 0; } }
    }
}
#endif
