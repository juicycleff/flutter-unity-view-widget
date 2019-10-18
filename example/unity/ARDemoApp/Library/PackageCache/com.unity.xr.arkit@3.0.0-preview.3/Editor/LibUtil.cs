#if UNITY_IOS
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS;
using UnityEngine.XR.ARKit;

namespace UnityEditor.XR.ARKit
{
    internal static class LibUtil
    {
        public static void SelectPlugin(
            PluginImporter libXcode10,
            PluginImporter libXcode11)
        {
            const BuildTarget platform = BuildTarget.iOS;
            var version = GetXcodeVersion();
            if (version == new OSVersion(0))
                throw new BuildFailedException($"Could not determine which version of Xcode was selected in the Build Settings. Xcode app was computed as {GetXcodeApplicationName()}.");

            if (version >= new OSVersion(11))
            {
                libXcode10?.SetCompatibleWithPlatform(platform, false);
                libXcode11?.SetCompatibleWithPlatform(platform, true);
            }
            else
            {
                libXcode10?.SetCompatibleWithPlatform(platform, true);
                libXcode11?.SetCompatibleWithPlatform(platform, false);
            }
        }

        static OSVersion GetXcodeVersion()
        {
            return OSVersion.Parse(GetXcodeApplicationName());
        }

        static string GetXcodeApplicationName()
        {
            var index = Math.Max(0, XcodeApplications.GetPreferedXcodeIndex());
            return XcodeApplications.GetXcodeApplicationPublicName(index);
        }
    }
}
#endif
