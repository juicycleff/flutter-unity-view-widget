using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEngine.XR.ARFoundation;

namespace UnityEditor.XR.ARFoundation
{
    internal class ARSceneValidator
    {
        [PostProcessBuild]
        static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (s_ScenesWithARTypes.Count > 0 && s_SessionCount == 0)
            {
                var scenes = "";
                foreach(var sceneName in s_ScenesWithARTypes)
                {
                    scenes += string.Format("\n\t{0}", sceneName);
                }

                Debug.LogWarningFormat(
                    "The following scenes contain AR components but no ARSession. The ARSession component controls the AR lifecycle, so these components will not do anything at runtime. Was this intended?{0}",
                    scenes);
            }

            s_ScenesWithARTypes.Clear();
            s_SessionCount = 0;
        }

        [PostProcessScene]
        static void OnPostProcessScene()
        {
            if (sceneContainsARTypes)
                s_ScenesWithARTypes.Add(SceneManager.GetActiveScene().name);

            s_SessionCount += UnityEngine.Object.FindObjectsOfType<ARSession>().Length;
        }

        static bool sceneContainsARTypes
        {
            get
            {
                foreach (var type in k_ARTypes)
                {
                    foreach (var component in UnityEngine.Object.FindObjectsOfType(type))
                    {
                        var monobehaviour = component as MonoBehaviour;
                        if (monobehaviour != null && monobehaviour.enabled)
                            return true;
                    }
                }

                return false;
            }
        }

        static List<string> s_ScenesWithARTypes = new List<string>();

        static int s_SessionCount;

        static readonly Type[] k_ARTypes = new Type[]
        {
            typeof(ARCameraBackground),
            typeof(ARPlaneManager),
            typeof(ARPointCloudManager),
            typeof(ARReferencePointManager)
        };
    }
}
