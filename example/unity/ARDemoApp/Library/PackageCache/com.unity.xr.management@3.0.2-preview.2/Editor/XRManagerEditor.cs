using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR.Management;

namespace UnityEditor.XR.Management
{
    internal class LoaderInfo : IEquatable<LoaderInfo>
    {
        public Type loaderType;
        public string assetName;
        public XRLoader instance;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LoaderInfo && Equals((LoaderInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (loaderType != null ? loaderType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (instance != null ? instance.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(LoaderInfo other)
        {
            return other != null && Equals(loaderType, other.loaderType) && Equals(instance, other.instance);
        }
    }

    class LoaderOrderUI
    {
        const string k_AtLeastOneLoaderInstance = "Must add at least one XRLoader instance.";

        const string k_AvailableMenuSeparator = "Available/";
        const string k_SuggestedMenuSeparator = "Download/";
        public static string s_DownloadingPackageKey = "defferedLoadPackage";

        ReorderableList m_OrderedList = null;
        List<LoaderInfo> m_LoadersInUse = new List<LoaderInfo>();
        List<LoaderInfo> m_LoadersNotInUse = new List<LoaderInfo>();
        XRCuratedPackages m_CuratedLoaders;

        SerializedProperty m_LoaderProperty;
        bool m_ShouldReload = false;
        Action m_onUpdate;

        internal LoaderOrderUI(Action onUpdate)
        {
            m_onUpdate = onUpdate;
        }

        internal void Reset(List<LoaderInfo> loaderInfos, XRCuratedPackages curatedInfo, List<LoaderInfo> loadersInUse, SerializedProperty loaderProperty)
        {
            m_LoaderProperty = loaderProperty;
            m_CuratedLoaders = curatedInfo;

            m_LoadersInUse = loadersInUse;
            m_LoadersNotInUse.Clear();

            foreach (var info in loaderInfos)
            {
                if (!m_LoadersInUse.Contains(info))
                {
                    m_LoadersNotInUse.Add(info);
                }
            }

            DownloadDefferedLoad();

            m_ShouldReload = true;
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            LoaderInfo info = index < m_LoadersInUse.Count ? m_LoadersInUse[index] : null;
            var label = (info == null || info.instance == null) ? EditorGUIUtility.TrTextContent("Missing (XRLoader)") : EditorGUIUtility.TrTextContent(info.assetName);
            EditorGUI.LabelField(rect, label);
        }

        float GetElementHeight(int index)
        {
            return m_OrderedList.elementHeight;
        }

        internal bool CheckIfChanged(List<LoaderInfo> listToCompare)
        {
            if (m_LoaderProperty != null && m_LoaderProperty.isArray)
            {
                int index = 0;
                foreach (LoaderInfo info in listToCompare)
                {
                    var target = (XRLoader)m_LoaderProperty.GetArrayElementAtIndex(index).objectReferenceValue;

                    if(info.instance != target)
                    {
                        return true;
                    }
                    index++;
                }
            }

            return false;
        }

        void UpdateSerializedProperty()
        {
            if (m_LoaderProperty != null && m_LoaderProperty.isArray)
            {
                m_LoaderProperty.ClearArray();

                int index = 0;
                foreach (LoaderInfo info in m_LoadersInUse)
                {
                    m_LoaderProperty.InsertArrayElementAtIndex(index);
                    var prop = m_LoaderProperty.GetArrayElementAtIndex(index);
                    prop.objectReferenceValue = info.instance;
                    index++;
                }

                m_LoaderProperty.serializedObject.ApplyModifiedProperties();
            }

            if(m_onUpdate != null)
                m_onUpdate();
        }

        void ReorderLoaderList(ReorderableList list)
        {
            UpdateSerializedProperty();
        }

        void DrawAddDropdown(Rect rect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();

            int index = 0;
            if(m_LoadersNotInUse.Count > 0)
            {
                foreach (var info in m_LoadersNotInUse)
                {
                    string name = info.assetName;
                    if (String.IsNullOrEmpty(name) && info.loaderType != null)
                    {
                        name = EditorUtilities.TypeNameToString(info.loaderType);
                    }

                    if (info.instance == null)
                        name = name + " (create new)";

                    menu.AddItem(new GUIContent(string.Format("{1}{0}. {2}", index + 1, k_AvailableMenuSeparator, name)), false, AddLoaderMenuSelected, index);
                    index++;
                }
            }

            string downloadingPackage = "";

            if (EditorPrefs.HasKey(s_DownloadingPackageKey))
            {
                downloadingPackage = EditorPrefs.GetString(s_DownloadingPackageKey);
            }

            index = 0;
            if (m_CuratedLoaders != null)
            {
                foreach (var info in m_CuratedLoaders.CuratedPackages)
                {
                    if (CheckIfPackageAvailable(m_LoadersInUse, info) || CheckIfPackageAvailable(m_LoadersNotInUse, info))
                    {
                        continue;
                    }

                    if (CheckIfCuratedLoaderExists(m_LoadersInUse, info) || CheckIfCuratedLoaderExists(m_LoadersNotInUse, info))
                    {
                        continue;
                    }

                    string name = info.MenuTitle;

                    if (m_CurrentDownloadRequest != null)
                    {
                        menu.AddDisabledItem(new GUIContent(string.Format("{1}{0}. {2}", index + 1, k_SuggestedMenuSeparator, name)));
                    }
                    else
                    {
                        menu.AddItem(new GUIContent(string.Format("{1}{0}. {2}", index + 1, k_SuggestedMenuSeparator, name)), false, DownloadLoaderMenuSelected, info);
                    }
                    index++;
                }
            }

            menu.ShowAsContext();
        }

        private bool CheckIfPackageAvailable(List<LoaderInfo> loaders, CuratedInfo info)
        {
            var packages = from loader in loaders
                           where String.Compare(EditorUtilities.TypeNameToString(loader.loaderType), info.LoaderTypeInfo) == 0
                           select loader;

            return packages.Any();
        }

        private bool CheckIfCuratedLoaderExists(List<LoaderInfo> loaders, CuratedInfo info)
        {
            var skip = false;
            foreach (var loadedInfo in loaders)
            {
                var assets = AssetDatabase.FindAssets(String.Format("t:{0}", loadedInfo.loaderType));

                if (assets.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assets[0]);

                    var typeName = EditorUtilities.TypeNameToString(loadedInfo.loaderType);

                    if (path.Contains(info.PackageName) || string.Equals(typeName, info.LoaderTypeInfo))
                    {
                        skip = true;
                        break;
                    }
                }
            }

            return skip;
        }

        PackageManager.Requests.ListRequest m_listRequest = null;
        PackageManager.Requests.AddRequest m_CurrentDownloadRequest = null;

        void DownloadLoaderMenuSelected(object data)
        {
            var info = (CuratedInfo)data;
            m_CurrentDownloadRequest = PackageManager.Client.Add(info.PackageName);
            EditorPrefs.SetString(s_DownloadingPackageKey, info.PackageName);
            AssetDatabase.Refresh();
        }

        void DownloadDefferedLoad()
        {
            if(m_listRequest == null)
            {
                m_listRequest = PackageManager.Client.List();
            }

            if (m_listRequest.IsCompleted)
            {
                var listResult = m_listRequest.Result;

                if (EditorPrefs.HasKey(s_DownloadingPackageKey))
                {
                    var loadPackage = EditorPrefs.GetString(s_DownloadingPackageKey);

                    PackageManager.PackageInfo info = listResult.FirstOrDefault(p => p.name == loadPackage);
                    if (info != null)
                    {
                        if (InsertPackageXRLoaderToList(info))
                        {
                            EditorPrefs.DeleteKey(s_DownloadingPackageKey);
                        }
                    }
                    else
                    {
                        m_listRequest = PackageManager.Client.List();
                    }
                }
            }
        }

        bool InsertPackageXRLoaderToList(PackageManager.PackageInfo packageInfo)
        {
            List<LoaderInfo> newInfos = new List<LoaderInfo>();

            XRManagerSettingsEditor.GetAllKnownLoaderInfos(newInfos);

            foreach(var info in newInfos)
            {
                var assets = AssetDatabase.FindAssets(String.Format("t:{0}", info.loaderType));

                if (assets.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                    if(path.Contains(packageInfo.name))
                    {
                        AddLoaderMenu(info);
                        return true;
                    }
                }
            }

            return false;
        }

        void AddLoaderMenuSelected(object data)
        {
            int selected = (int)data;
            LoaderInfo info = m_LoadersNotInUse[selected];

            AddLoaderMenu(info);
        }

        void AddLoaderMenu(LoaderInfo info)
        {
            if (info.instance == null)
            {
                string newAssetName = String.Format("{0}.asset", EditorUtilities.TypeNameToString(info.loaderType));
                XRLoader loader = ScriptableObject.CreateInstance(info.loaderType) as XRLoader;
                string assetPath = EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultLoaderPath);
                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                assetPath = Path.Combine(assetPath, newAssetName);
                info.instance = loader;
                info.assetName = Path.GetFileNameWithoutExtension(assetPath);
                AssetDatabase.CreateAsset(loader, assetPath);
                m_ShouldReload = true;
            }

            m_LoadersNotInUse.Remove(info);
            m_LoadersInUse.Add(info);
            UpdateSerializedProperty();
        }

        void RemoveInfo(LoaderInfo info)
        {
            m_LoadersInUse.Remove(info);
            m_ShouldReload = true;
        }

        void RemoveInstanceFromList(ReorderableList list)
        {
            LoaderInfo info = m_LoadersInUse[list.index];
            RemoveInfo(info);
            UpdateSerializedProperty();
        }

        void QueryCleanupOfLostLoaders()
        {
            var missingLoaders = from info in m_LoadersInUse
                                where info.instance == null
                                select info;

            if (missingLoaders.Any())
            {
                m_LoadersInUse = m_LoadersInUse.Except(missingLoaders).ToList();
                m_ShouldReload = true;
                UpdateSerializedProperty();
            }
        }

        internal bool OnGUI()
        {
            if (m_LoaderProperty == null)
                return false;

            m_ShouldReload = false;

            if (EditorPrefs.HasKey(s_DownloadingPackageKey))
            {
                if (m_CurrentDownloadRequest == null || m_CurrentDownloadRequest.IsCompleted)
                {
                    EditorPrefs.DeleteKey(s_DownloadingPackageKey);
                    m_ShouldReload = true;
                }
            }

            QueryCleanupOfLostLoaders();

            if (!m_LoadersInUse.Any())
            {
                EditorGUILayout.HelpBox(k_AtLeastOneLoaderInstance, MessageType.Warning);
            }

            if (m_OrderedList == null)
            {
                m_OrderedList = new ReorderableList(m_LoadersInUse, typeof(XRLoader), true, true, true, true);
                m_OrderedList.drawHeaderCallback = (rect) => GUI.Label(rect, EditorGUIUtility.TrTextContent("Loaders"), EditorStyles.label);
                m_OrderedList.drawElementCallback = (rect, index, isActive, isFocused) => DrawElementCallback(rect, index, isActive, isFocused);
                m_OrderedList.elementHeightCallback = (index) => GetElementHeight(index);
                m_OrderedList.onReorderCallback = (list) => ReorderLoaderList(list);
                m_OrderedList.onAddDropdownCallback = (rect, list) => DrawAddDropdown(rect, list);
                m_OrderedList.onRemoveCallback = (list) => RemoveInstanceFromList(list);
            }

            m_OrderedList.DoLayoutList();

            return m_ShouldReload;
        }
    }


    [CustomEditor(typeof(XRManagerSettings))]
    internal class XRManagerSettingsEditor : Editor
    {
        // Simple class to give us updates when the asset database changes.
        internal class AssetCallbacks : AssetPostprocessor
        {
            static bool s_EditorUpdatable = false;
            internal static System.Action Callback { get; set; }

            static AssetCallbacks()
            {
                if (!s_EditorUpdatable)
                {
                    EditorApplication.update += EditorUpdatable;
                }
                EditorApplication.projectChanged += EditorApplicationOnProjectChanged;
            }

            static void EditorApplicationOnProjectChanged()
            {
                if (Callback != null)
                    Callback.Invoke();
            }

            static void EditorUpdatable()
            {
                s_EditorUpdatable = true;
                EditorApplication.update -= EditorUpdatable;
                if (Callback != null)
                    Callback.Invoke();
            }
        }

        SerializedProperty m_RequiresSettingsUpdate = null;
        SerializedProperty m_LoaderList = null;

        List<LoaderInfo> m_AllLoaderInfos = new List<LoaderInfo>();
        List<LoaderInfo> m_AssignedLoaderInfos = new List<LoaderInfo>();
        XRCuratedPackages m_CuratedInfo;

        LoaderOrderUI m_LoadOrderUI = null;

        void AssetProcessorCallback()
        {
            ShouldReload = true;
        }

        void OnEnable()
        {
            if (m_LoadOrderUI == null)
            {
                m_LoadOrderUI = new LoaderOrderUI(() =>
                {
                    ShouldReload = true;
                });
            }

            AssetCallbacks.Callback += AssetProcessorCallback;
            ShouldReload = true;
        }

        bool ShouldReload
        {
            get
            {
                if (m_RequiresSettingsUpdate != null)
                {
                    serializedObject.Update();

                    return m_RequiresSettingsUpdate.boolValue;
                }
                return false;
            }
            set
            {
                if (m_RequiresSettingsUpdate == null)
                    PopulateProperty("m_RequiresSettingsUpdate", ref m_RequiresSettingsUpdate);

                if (m_RequiresSettingsUpdate != null)
                {
                    m_RequiresSettingsUpdate.boolValue = value;

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        void OnDisable()
        {
            AssetCallbacks.Callback -= null;
        }

        void ReloadData()
        {
            if (m_LoaderList == null || m_LoaderList.serializedObject == null)
                return;

            m_AllLoaderInfos.Clear();

            m_AssignedLoaderInfos.Clear();

            PopulateCuratedLoaders();

            PopulateLoaderInfosFromCurrentAssignedLoaders();

            PopulateLoaderInfosFromUnassignedLoaders();
        }

        void PopulateCuratedLoaders()
        {
            var assets = AssetDatabase.FindAssets(String.Format("t:{0}", typeof(XRCuratedPackages)));

            if(assets.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[0]);

                if(m_CuratedInfo == null)
                    m_CuratedInfo = AssetDatabase.LoadAssetAtPath(path, typeof(XRCuratedPackages)) as XRCuratedPackages;
            }
        }

        void PopulateLoaderInfosFromUnassignedLoaders()
        {
            List<LoaderInfo> newInfos = new List<LoaderInfo>();

            GetAllKnownLoaderInfos(newInfos);
            MergeLoaderInfos(newInfos);
        }

        void MergeLoaderInfos(List<LoaderInfo> newInfos)
        {
            foreach (var info in newInfos)
            {
                bool addNew = true;
                if (info.instance != null)
                {
                    foreach (var li in m_AllLoaderInfos)
                    {
                        if (li.instance == info.instance)
                        {
                            if (!String.IsNullOrEmpty(info.assetName))
                                li.assetName = info.assetName;
                            addNew = false;
                            break;
                        }
                    }
                }

                if (addNew)
                {
                    m_AllLoaderInfos.Add(info);
                }
            }
        }

        internal static void GetAllKnownLoaderInfos(List<LoaderInfo> newInfos)
        {
            var loaderTypes = TypeLoaderExtensions.GetAllTypesWithInterface<XRLoader>();
            foreach (Type loaderType in loaderTypes)
            {
                // HACK: No need for people to see these loaders
                if (String.Compare("DummyLoader", loaderType.Name, StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare("SampleLoader", loaderType.Name, StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                var assets = AssetDatabase.FindAssets(String.Format("t:{0}", loaderType));
                if (!assets.Any())
                {
                    LoaderInfo info = new LoaderInfo();
                    info.loaderType = loaderType;
                    newInfos.Add(info);
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(asset);

                        LoaderInfo info = new LoaderInfo();
                        info.loaderType = loaderType;
                        info.instance = AssetDatabase.LoadAssetAtPath(path, loaderType) as XRLoader;
                        info.assetName = Path.GetFileNameWithoutExtension(path);
                        newInfos.Add(info);
                    }
                }
            }
        }

        string AssetNameFromInstance(UnityEngine.Object asset)
        {
            if (asset == null)
                return "";

            string assetPath = AssetDatabase.GetAssetPath(asset);
            return Path.GetFileNameWithoutExtension(assetPath);
        }

        void PopulateLoaderInfosFromCurrentAssignedLoaders()
        {
            for (int i = 0; i < m_LoaderList.arraySize; i++)
            {
                var prop = m_LoaderList.GetArrayElementAtIndex(i);

                LoaderInfo info = new LoaderInfo();
                info.loaderType = (prop.objectReferenceValue == null) ? null : prop.objectReferenceValue.GetType();
                info.assetName = AssetNameFromInstance(prop.objectReferenceValue);
                info.instance = prop.objectReferenceValue as XRLoader;

                m_AssignedLoaderInfos.Add(info);
                m_AllLoaderInfos.Add(info);
            }
        }

        void PopulateProperty(string propertyPath, ref SerializedProperty prop)
        {
            if (prop == null) prop = serializedObject.FindProperty(propertyPath);
        }

        /// <summary><see href="https://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html">Editor Documentation</see></summary>
        public override void OnInspectorGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
                return;

            PopulateProperty("m_RequiresSettingsUpdate", ref m_RequiresSettingsUpdate);
            PopulateProperty("m_Loaders", ref m_LoaderList);

            serializedObject.Update();

            if (ShouldReload || m_LoadOrderUI.CheckIfChanged(m_AssignedLoaderInfos))
            {
                ShouldReload = false;
                LoaderOrderUICallback();
                m_LoadOrderUI.Reset(m_AllLoaderInfos, m_CuratedInfo, m_AssignedLoaderInfos, m_LoaderList);
            }

            ShouldReload = m_LoadOrderUI.OnGUI();

            serializedObject.ApplyModifiedProperties();
        }

        private void LoaderOrderUICallback()
        {
            m_LoaderList.serializedObject.ApplyModifiedProperties();
            ReloadData();
        }
    }
}
