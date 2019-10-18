using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimelineCursors
    {
        public enum CursorType
        {
            MixBoth,
            MixLeft,
            MixRight,
            Replace,
            Ripple,

            Pan
        }

        class CursorInfo
        {
            public readonly string assetPath;
            public readonly Vector2 hotSpot;
            public readonly MouseCursor mouseCursorType;

            public CursorInfo(string assetPath, Vector2 hotSpot, MouseCursor mouseCursorType)
            {
                this.assetPath = assetPath;
                this.hotSpot = hotSpot;
                this.mouseCursorType = mouseCursorType;
            }
        }

        const string k_CursorAssetRoot         = "Cursors/";
        const string k_CursorAssetsNamespace   = "Timeline.";
        const string k_CursorAssetExtension    = ".png";

        const string k_MixBothCursorAssetName  = k_CursorAssetsNamespace + "MixBoth"  + k_CursorAssetExtension;
        const string k_MixLeftCursorAssetName  = k_CursorAssetsNamespace + "MixLeft"  + k_CursorAssetExtension;
        const string k_MixRightCursorAssetName = k_CursorAssetsNamespace + "MixRight" + k_CursorAssetExtension;
        const string k_ReplaceCursorAssetName  = k_CursorAssetsNamespace + "Replace"  + k_CursorAssetExtension;
        const string k_RippleCursorAssetName   = k_CursorAssetsNamespace + "Ripple"   + k_CursorAssetExtension;

        static readonly string s_PlatformPath = (Application.platform == RuntimePlatform.WindowsEditor) ? "Windows/" : "macOS/";
        static readonly string s_CursorAssetDirectory = k_CursorAssetRoot + s_PlatformPath;

        static readonly Dictionary<CursorType, CursorInfo> s_CursorInfoLookup = new Dictionary<CursorType, CursorInfo>
        {
            {CursorType.MixBoth,  new CursorInfo(s_CursorAssetDirectory + k_MixBothCursorAssetName,  new Vector2(16, 18), MouseCursor.CustomCursor)},
            {CursorType.MixLeft,  new CursorInfo(s_CursorAssetDirectory + k_MixLeftCursorAssetName,  new Vector2(7, 18), MouseCursor.CustomCursor)},
            {CursorType.MixRight, new CursorInfo(s_CursorAssetDirectory + k_MixRightCursorAssetName, new Vector2(25, 18), MouseCursor.CustomCursor)},
            {CursorType.Replace,  new CursorInfo(s_CursorAssetDirectory + k_ReplaceCursorAssetName,  new Vector2(16, 28), MouseCursor.CustomCursor)},
            {CursorType.Ripple,   new CursorInfo(s_CursorAssetDirectory + k_RippleCursorAssetName,   new Vector2(26, 19), MouseCursor.CustomCursor)},
            {CursorType.Pan,      new CursorInfo(null, Vector2.zero, MouseCursor.Pan)}
        };

        static readonly Dictionary<string, Texture2D> s_CursorAssetCache = new Dictionary<string, Texture2D>();

        static CursorType? s_CurrentCursor;

        public static void SetCursor(CursorType cursorType)
        {
            if (s_CurrentCursor.HasValue && s_CurrentCursor.Value == cursorType) return;

            s_CurrentCursor = cursorType;
            var cursorInfo = s_CursorInfoLookup[cursorType];

            Texture2D cursorAsset = null;

            if (cursorInfo.mouseCursorType == MouseCursor.CustomCursor)
            {
                cursorAsset = LoadCursorAsset(cursorInfo.assetPath);
            }

            EditorGUIUtility.SetCurrentViewCursor(cursorAsset, cursorInfo.hotSpot, cursorInfo.mouseCursorType);
        }

        public static void ClearCursor()
        {
            if (!s_CurrentCursor.HasValue) return;

            EditorGUIUtility.ClearCurrentViewCursor();
            s_CurrentCursor = null;
        }

        static Texture2D LoadCursorAsset(string assetPath)
        {
            if (!s_CursorAssetCache.ContainsKey(assetPath))
            {
                s_CursorAssetCache.Add(assetPath, (Texture2D)EditorGUIUtility.Load(assetPath));
            }

            return s_CursorAssetCache[assetPath];
        }
    }
}
