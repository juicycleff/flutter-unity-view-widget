using System.Linq;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The build processor that makes sure that any custom configuration is passed to the
    /// ARKit Loader at runtime.
    ///
    /// Custom configuration instances that are stored in EditorBuildSettings are not copied to the target build
    /// as they are considered unreferenced assets. In order to get them to the runtime side of things, they need
    /// to be serialized to the build app and deserialized at runtime. Previously this would be a manual process
    /// requiring the implementor to manually serialize to some location that can then be read from to deserialize
    /// at runtime. With the new PlayerSettings Preloaded Assets API we can now just add our asset to the preloaded
    /// list and have it be instantiated at app launch.
    ///
    /// Note that the preloaded assets are only notified with Awake, so anything you want or need to do with the
    /// asset after launch needs to be handled there.
    ///
    /// More info on APIs used here:
    /// * <a href="https://docs.unity3d.com/ScriptReference/EditorBuildSettings.html">EditorBuildSettings</a>
    /// * <a href="https://docs.unity3d.com/ScriptReference/PlayerSettings.GetPreloadedAssets.html">PlayerSettings.GetPreloadedAssets</a>
    /// * <a href="https://docs.unity3d.com/ScriptReference/PlayerSettings.SetPreloadedAssets.html">PlayerSettings.SetPreloadedAssets</a>
    /// </summary>
    public class ARKitLoaderBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0;  }
        }

        void CleanOldSettings()
        {
            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            if (preloadedAssets == null)
                return;

            var oldSettings = from s in preloadedAssets
                where s != null && s.GetType() == typeof(ARKitLoaderSettings)
                select s;

            if (oldSettings != null && oldSettings.Any())
            {
                var assets = preloadedAssets.ToList();
                foreach (var s in oldSettings)
                {
                    assets.Remove(s);
                }

                PlayerSettings.SetPreloadedAssets(assets.ToArray());
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Always remember to cleanup preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();

            ARKitLoaderSettings settings = null;
            EditorBuildSettings.TryGetConfigObject(ARKitLoaderConstants.k_SettingsKey, out settings);
            if (settings == null)
                return;

            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            if (!preloadedAssets.Contains(settings))
            {
                var assets = preloadedAssets.ToList();
                assets.Add(settings);
                PlayerSettings.SetPreloadedAssets(assets.ToArray());
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Always remember to cleanup preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();
        }
    }
}
