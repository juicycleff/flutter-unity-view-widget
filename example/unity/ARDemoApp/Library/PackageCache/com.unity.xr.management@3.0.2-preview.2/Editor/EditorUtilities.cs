using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;

namespace UnityEditor.XR.Management
{
    internal static class EditorUtilities
    {
        internal static readonly string[] s_DefaultGeneralSettingsPath = {"XR"};
        internal static readonly string[] s_DefaultLoaderPath = {"XR","Loaders"};
        internal static readonly string[] s_DefaultSettingsPath = {"XR","Settings"};

        internal static bool AssetDatabaseHasInstanceOfType(string type)
        {
            var assets = AssetDatabase.FindAssets(String.Format("t:{0}", type));
            return assets.Any();
        }


        internal static string GetAssetPathForComponents(string[] pathComponents, string root = "Assets")
        {
            if (pathComponents.Length <= 0)
                return null;

            string path = root;
            foreach( var pc in pathComponents)
            {
                string subFolder = Path.Combine(path, pc);
                bool shouldCreate = true;
                foreach (var f in AssetDatabase.GetSubFolders(path))
                {
                    if (String.Compare(Path.GetFullPath(f), Path.GetFullPath(subFolder), true) == 0)
                    {
                        shouldCreate = false;
                        break;
                    }
                }

                if (shouldCreate)
                    AssetDatabase.CreateFolder(path, pc);
                path = subFolder;
            }

            return path;
        }

        internal static string TypeNameToString(Type type)
        {
            return type == null ? "" : TypeNameToString(type.FullName);
        }

        internal static string TypeNameToString(string type)
        {
            string[] typeParts = type.Split(new char[] { '.' });
            if (!typeParts.Any())
                return String.Empty;

            string[] words = Regex.Matches(typeParts.Last(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
                .OfType<Match>()
                .Select(m => m.Value)
                .ToArray();
            return string.Join(" ", words);
        }
    }
}
