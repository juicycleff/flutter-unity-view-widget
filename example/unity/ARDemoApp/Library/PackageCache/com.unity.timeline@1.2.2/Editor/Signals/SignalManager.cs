using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline.Signals
{
    class SignalManager : IDisposable
    {
        static SignalManager m_Instance;
        readonly List<SignalAsset> m_assets = new List<SignalAsset>();

        internal static SignalManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new SignalManager();
                    m_Instance.Refresh();
                }

                return m_Instance;
            }

            set { m_Instance = value; }
        }

        internal SignalManager()
        {
            SignalAsset.OnEnableCallback += Register;
        }

        public static IEnumerable<SignalAsset> assets
        {
            get
            {
                foreach (var asset in instance.m_assets)
                {
                    if (asset != null)
                        yield return asset;
                }
            }
        }

        public static SignalAsset CreateSignalAssetInstance(string path)
        {
            var newSignal = ScriptableObject.CreateInstance<SignalAsset>();
            newSignal.name = Path.GetFileNameWithoutExtension(path);

            var asset = AssetDatabase.LoadMainAssetAtPath(path) as SignalAsset;
            if (asset != null)
            {
                TimelineUndo.PushUndo(asset, Styles.UndoCreateSignalAsset);
                EditorUtility.CopySerialized(newSignal, asset);
                Object.DestroyImmediate(newSignal);
                return asset;
            }

            AssetDatabase.CreateAsset(newSignal, path);
            return newSignal;
        }

        public void Dispose()
        {
            SignalAsset.OnEnableCallback -= Register;
        }

        void Register(SignalAsset a)
        {
            m_assets.Add(a);
        }

        void Refresh()
        {
            var guids = AssetDatabase.FindAssets("t:SignalAsset");
            foreach (var g in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(g);
                var asset = AssetDatabase.LoadAssetAtPath<SignalAsset>(path);
                m_assets.Add(asset);
            }
        }
    }
}
