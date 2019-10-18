using System;
using System.IO;
using UnityEditorInternal;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class ScriptableObjectViewPrefs<TViewModel> : IDisposable where TViewModel : ScriptableObject
    {
        const string k_DefaultFilePath = "Library/";
        const string k_Extension = ".pref";

        readonly string m_RelativePath;
        readonly string m_AbsolutePath;
        readonly string m_FileName;
        ScriptableObject m_Asset;
        TViewModel m_ViewModel;

        bool isSavable
        {
            get
            {
                return m_Asset != null &&
                    m_ViewModel != null &&
                    !string.IsNullOrEmpty(m_FileName);
            }
        }

        public ScriptableObjectViewPrefs(ScriptableObject asset, string relativeSavePath)
        {
            m_Asset = asset;
            m_RelativePath = string.IsNullOrEmpty(relativeSavePath) ? k_DefaultFilePath : relativeSavePath;
            if (!m_RelativePath.EndsWith("/", StringComparison.Ordinal))
                m_RelativePath += "/";

            m_AbsolutePath = Application.dataPath + "/../" + m_RelativePath;

            var assetKey = GetAssetKey(asset);
            m_FileName = string.IsNullOrEmpty(assetKey) ? string.Empty : assetKey + k_Extension;
        }

        public TViewModel viewModel
        {
            get
            {
                if (m_ViewModel == null)
                {
                    if (m_Asset == null)
                        m_ViewModel = CreateViewModel();
                    else
                        m_ViewModel = LoadViewModel() ?? CreateViewModel();
                }
                return m_ViewModel;
            }
        }

        public void Save()
        {
            if (!isSavable)
                return;

            // make sure the path exists or file write will fail
            if (!Directory.Exists(m_AbsolutePath))
                Directory.CreateDirectory(m_AbsolutePath);

            const bool saveAsText = true;
            InternalEditorUtility.SaveToSerializedFileAndForget(new UnityObject[] { m_ViewModel }, m_RelativePath + m_FileName, saveAsText);
        }

        public void DeleteFile()
        {
            if (!isSavable)
                return;

            var path = m_AbsolutePath + m_FileName;

            if (!File.Exists(path))
                return;

            File.Delete(path);
        }

        public void Dispose()
        {
            if (m_ViewModel != null)
                UnityObject.DestroyImmediate(m_ViewModel);

            m_Asset = null;
        }

        public static TViewModel CreateViewModel()
        {
            var model = ScriptableObject.CreateInstance<TViewModel>();
            model.hideFlags |= HideFlags.HideAndDontSave;
            return model;
        }

        TViewModel LoadViewModel()
        {
            if (string.IsNullOrEmpty(m_FileName))
                return null;

            var objects = InternalEditorUtility.LoadSerializedFileAndForget(m_RelativePath + m_FileName);
            if (objects.Length <= 0 || objects[0] == null)
                return null;

            var model = (TViewModel)objects[0];
            model.hideFlags |= HideFlags.HideAndDontSave;

            return model;
        }

        static string GetAssetKey(UnityObject asset)
        {
            return asset == null ? string.Empty : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
        }
    }
}
