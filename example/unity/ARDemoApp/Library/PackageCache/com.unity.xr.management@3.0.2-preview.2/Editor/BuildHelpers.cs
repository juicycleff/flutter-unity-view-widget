using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;

namespace UnityEditor.XR.Management
{
    internal static class BuildHelpers
    {
        internal static void CleanOldSettings<T>()
        {
            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            if (preloadedAssets == null)
                return;

            var oldSettings = from s in preloadedAssets
                where s != null && s.GetType() == typeof(T)
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
    }
}
