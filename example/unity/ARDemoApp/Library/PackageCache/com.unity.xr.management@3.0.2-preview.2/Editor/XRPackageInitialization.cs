using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;

using UnityEngine;
using UnityEditor;

namespace UnityEditor.XR.Management
{
    /// <summary>Interface for specifying package initialization information</summary>
    public interface XRPackageInitializationBase
    {
        /// <summary>Package name property</summary>
        /// <value>The name of the package</value>
        string PackageName { get; }
        /// <summary>The loader full type name for this package</summary>
        /// <value>Loader fulltype name</value>
        string LoaderFullTypeName { get; }
        /// <summary>The loader type name for this package</summary>
        /// <value>Loader type name</value>
        string LoaderTypeName { get; }
        /// <summary>The settings full type name for this package</summary>
        /// <value>Settings full type name</value>
        string SettingsFullTypeName { get; }
        /// <summary>The settings type name for this package</summary>
        /// <value>Settings type name</value>
        string SettingsTypeName { get; }
        /// <summary>Package initialization key</summary>
        /// <value>The init key for the package</value>
        string PackageInitKey { get; }

        /// <summary>Initialize package settings</summary>
        /// <param name="obj">The scriptable object instance to initialize</param>
        /// <returns>True if successful, false if not.</returns>
        bool PopulateSettingsOnInitialization(ScriptableObject obj);
    }

    class PackageInitializationSettings : ScriptableObject
    {
        private static PackageInitializationSettings s_PackageSettings = null;
        private static object s_Lock = new object();

        [SerializeField]
        private List<string> m_Settings = new List<string>();

        private PackageInitializationSettings(){ }

        internal static PackageInitializationSettings Instance
        {
            get
            {
                if (s_PackageSettings == null)
                {
                    lock(s_Lock)
                    {
                        if (s_PackageSettings == null)
                        {
                            s_PackageSettings = ScriptableObject.CreateInstance<PackageInitializationSettings>();
                        }
                    }
                }
                return s_PackageSettings;
            }
        }

        internal void LoadSettings()
        {
            string packageInitPath = Path.Combine("ProjectSettings", "XRPackageSettings.asset");

            if (File.Exists(packageInitPath))
            {
                using (StreamReader sr = new StreamReader(packageInitPath))
                {
                    string settings = sr.ReadToEnd();
                    JsonUtility.FromJsonOverwrite(settings, this);
                }
            }
        }


        internal void SaveSettings()
        {
            string packageInitPath = Path.Combine("ProjectSettings", "XRPackageSettings.asset");
            using (StreamWriter sw = new StreamWriter(packageInitPath))
            {
                string settings = JsonUtility.ToJson(this, true);
                sw.Write(settings);
            }
        }

        internal bool HasSettings(string key)
        {
            return m_Settings.Contains(key);
        }

        internal void AddSettings(string key)
        {
            if (!HasSettings(key))
                m_Settings.Add(key);
        }
    }


    [InitializeOnLoad]
    class PackageInitializationBootstrap
    {
        static PackageInitializationBootstrap()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.update += BeginPackageInitialization;
            }
        }

        static void BeginPackageInitialization()
        {

            foreach (var t in TypeLoaderExtensions.GetAllTypesWithInterface<XRPackageInitializationBase>())
            {
                XRPackageInitializationBase packageInit = Activator.CreateInstance(t) as XRPackageInitializationBase;
                InitPackage(packageInit);
            }
        }

        static void InitPackage(XRPackageInitializationBase packageInit)
        {
            PackageInitializationSettings.Instance.LoadSettings();

            if (PackageInitializationSettings.Instance.HasSettings(packageInit.PackageInitKey))
                return;

            EditorApplication.update -= BeginPackageInitialization;

            if (!InitializeLoaderInstance(packageInit))
            {
                Debug.LogWarning(
                    String.Format("{0} Loader Initialization not completed. You will need to create an instance of the loader using an instance of XRManager before you can use the intended XR Package.", packageInit.PackageName));
            }

            if (!InitializeSettingsInstance(packageInit))
            {
                Debug.LogWarning(
                    String.Format("{0} Settings Initialization not completed. You will need to create an instance of settings to customize options specific to this pacakge.", packageInit.PackageName));
            }

            PackageInitializationSettings.Instance.AddSettings(packageInit.PackageInitKey);
            PackageInitializationSettings.Instance.SaveSettings();
        }

        static ScriptableObject CreateScriptableObjectInstance(string packageName, string typeName, string instanceType, string path)
        {
            ScriptableObject obj = ScriptableObject.CreateInstance(typeName) as ScriptableObject;
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    string fileName = String.Format("{0}.asset", EditorUtilities.TypeNameToString(typeName));
                    string targetPath = Path.Combine(path, fileName);
                    AssetDatabase.CreateAsset(obj, targetPath);
                    Debug.LogFormat("{0} package initialization created default {1} instance at path {2}", packageName, instanceType.ToLower(), path);
                    return obj;
                }
            }
            return null;
        }

        static bool InitializeLoaderInstance(XRPackageInitializationBase packageInit)
        {
            bool ret = EditorUtilities.AssetDatabaseHasInstanceOfType(packageInit.LoaderTypeName);
            if (Application.isBatchMode)
                return true;

            if (!ret)
            {
                ret = EditorUtility.DisplayDialog(
                    String.Format("{0} Package Initialization", packageInit.PackageName),
                    String.Format("Before using the {0} package you need to create an instance of the {0} Loader. Would you like to do that now?", packageInit.PackageName),
                    "Create Loader",
                    "Cancel");
                if (ret)
                {
                    var obj = CreateScriptableObjectInstance(packageInit.PackageName,
                        packageInit.LoaderFullTypeName,
                        "Loader",
                        EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultLoaderPath));
                    ret = (obj != null);
                }
            }

            return ret;
        }

        static bool InitializeSettingsInstance(XRPackageInitializationBase packageInit)
        {
            bool ret = EditorUtilities.AssetDatabaseHasInstanceOfType(packageInit.SettingsTypeName);
            if (Application.isBatchMode)
                return true;

            if (!ret)
            {
                ret = EditorUtility.DisplayDialog(
                    String.Format("{0} Package Initialization", packageInit.PackageName),
                    String.Format("Before using the {0} package you should create an instance of {0} Settings to provide for custom configuration. Would you like to do that now?", packageInit.PackageName),
                    "Create Settings",
                    "Cancel");
                if (ret)
                {
                    var obj = CreateScriptableObjectInstance(packageInit.PackageName,
                        packageInit.SettingsFullTypeName,
                        "Settings",
                        EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultSettingsPath));
                    ret = packageInit.PopulateSettingsOnInitialization(obj);
                }
            }

            return ret;
        }

    }
}
