using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace FlutterUnityIntegration.Editor
{
    public static class SymbolDefineHelper 
    {
        public static void SetScriptingDefine(string symbol, bool enable)
        {
            bool changesMade = false;

            // Iterate over all named build targets available
            foreach (NamedBuildTarget namedTarget in GetAllNamedBuildTargets())
            {
                string defines = PlayerSettings.GetScriptingDefineSymbols(namedTarget);
                var defineList = new List<string>(defines.Split(';'));

                bool contains = defineList.Contains(symbol);

                if (enable && !contains)
                {
                    defineList.Add(symbol);
                    changesMade = true;
                    Debug.Log($"Added '{symbol}' to {namedTarget.TargetName}");
                }
                else if (!enable && contains)
                {
                    defineList.Remove(symbol);
                    changesMade = true;
                    Debug.Log($"Removed '{symbol}' from {namedTarget.TargetName}");
                }

                string updatedDefines = string.Join(";", defineList);
                PlayerSettings.SetScriptingDefineSymbols(namedTarget, updatedDefines);
            }

            if (changesMade)
            {
                EditorUtility.RequestScriptReload();
                Debug.Log("Script reload requested.");
            }
            else
            {
                Debug.Log("No changes made to scripting define symbols.");
            }
        }

        private static IEnumerable<NamedBuildTarget> GetAllNamedBuildTargets()
        {
            yield return NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.Standalone);
            yield return NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.Android);
            yield return NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.iOS);
            yield return NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.WebGL);
        }
    }
}
