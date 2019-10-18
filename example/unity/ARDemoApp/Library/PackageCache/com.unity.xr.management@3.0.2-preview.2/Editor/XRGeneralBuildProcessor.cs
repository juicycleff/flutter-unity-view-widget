using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;
using UnityEngine.XR.Management;

namespace UnityEditor.XR.Management
{
    class XRGeneralBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport, IPostGenerateGradleAndroidProject
    {
        class PreInitInfo
        {
            public PreInitInfo(IXRLoaderPreInit loader, BuildTarget buildTarget, BuildTargetGroup buildTargetGroup)
            {
                this.loader = loader;
                this.buildTarget = buildTarget;
                this.buildTargetGroup = buildTargetGroup;
            }

            public IXRLoaderPreInit loader;
            public BuildTarget buildTarget;
            public BuildTargetGroup buildTargetGroup;
        }

        static private PreInitInfo preInitInfo = null;

        public int callbackOrder
        {
            get { return 0;  }
        }

        void CleanOldSettings()
        {
            BuildHelpers.CleanOldSettings<XRGeneralSettings>();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Always remember to cleanup preloaded assets after build to make sure we don't
            // dirty later builds with assets that may not be needed or are out of date.
            CleanOldSettings();

            XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return;

            XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(report.summary.platformGroup);
            if (settings == null)
                return;

            // store off some info about the first loader in the list for PreInit boot.config purposes
            preInitInfo = null;
            XRManagerSettings loaderManager = settings.AssignedSettings;
            if (loaderManager != null)
            {
                List<XRLoader> loaders = loaderManager.loaders;
                if (loaders.Count >= 1)
                {
                    preInitInfo = new PreInitInfo(loaders[0] as IXRLoaderPreInit, report.summary.platform, report.summary.platformGroup);
                }
            }

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

            if (preInitInfo == null)
                return;

            // Android build post-processing is handled in OnPostGenerateGradleAndroidProject
            if (report.summary.platform != BuildTarget.Android)
            {
                foreach (BuildFile file in report.files)
                {
                    if (file.role == CommonRoles.bootConfig)
                    {
                        try
                        {
                            var loader = preInitInfo.loader;
                            if (loader != null)
                            {
                                string preInitLibraryName = loader.GetPreInitLibraryName(preInitInfo.buildTarget,
                                    preInitInfo.buildTargetGroup);
                                preInitInfo = null;
 #if UNITY_2019_3_OR_NEWER
                                UnityEditor.XR.BootOptions.SetXRSDKPreInitLibrary(file.path,
                                    preInitLibraryName);
 #else
                                UnityEditor.Experimental.XR.BootOptions.SetXRSDKPreInitLibrary(file.path,
                                    preInitLibraryName);
 #endif
                            }
                        }
                        catch (Exception e)
                        {
                            throw new UnityEditor.Build.BuildFailedException(e);
                        }
                        break;
                    }
                }
            }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            if (preInitInfo == null)
                return;

            // android builds move the files to a different location than is in the BuildReport, so we have to manually find the boot.config
            string[] paths = { "src", "main", "assets", "bin", "Data", "boot.config" };
            string fullPath = System.IO.Path.Combine(path, String.Join(Path.DirectorySeparatorChar.ToString(), paths));

            try
            {
                var loader = preInitInfo.loader;
                if (loader != null)
                {
                    string preInitLibraryName = loader.GetPreInitLibraryName(preInitInfo.buildTarget,
                        preInitInfo.buildTargetGroup);
                    preInitInfo = null;
 #if UNITY_2019_3_OR_NEWER
                    UnityEditor.XR.BootOptions.SetXRSDKPreInitLibrary(fullPath, preInitLibraryName);
 #else
                    UnityEditor.Experimental.XR.BootOptions.SetXRSDKPreInitLibrary(fullPath, preInitLibraryName);
 #endif
                }
            }
            catch (Exception e)
            {
                throw new UnityEditor.Build.BuildFailedException(e);
            }
        }
    }
}

