using UnityEngine;
using UnityEditor;

namespace UnityEditor.Timeline
{
    static class ObjectExtension
    {
        public static bool IsSceneObject(this Object obj)
        {
            if (obj == null)
                return false;

            bool isSceneType = obj is GameObject || obj is Component;
            if (!isSceneType)
                return false;

            return !PrefabUtility.IsPartOfPrefabAsset(obj);
        }

        public static bool IsPrefab(this Object obj)
        {
            if (obj == null)
                return false;

            return PrefabUtility.IsPartOfPrefabAsset(obj);
        }
    }
}
