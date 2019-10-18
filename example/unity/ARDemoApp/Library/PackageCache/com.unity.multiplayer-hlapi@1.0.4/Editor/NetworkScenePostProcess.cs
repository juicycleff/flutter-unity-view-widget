#if ENABLE_UNET
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkScenePostProcess : MonoBehaviour
    {
        [PostProcessScene]
        public static void OnPostProcessScene()
        {
            var prefabWarnings = new HashSet<string>();

            int nextSceneId = 1;
            foreach (NetworkIdentity uv in FindObjectsOfType<NetworkIdentity>().OrderBy(identity => identity.name))
            {
                // if we had a [ConflictComponent] attribute that would be better than this check.
                // also there is no context about which scene this is in.
                if (uv.GetComponent<NetworkManager>() != null)
                {
                    Debug.LogError("NetworkManager has a NetworkIdentity component. This will cause the NetworkManager object to be disabled, so it is not recommended.");
                }

                if (uv.isClient || uv.isServer)
                    continue;

                uv.gameObject.SetActive(false);
                uv.ForceSceneId(nextSceneId++);

                var prefabGO = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(uv.gameObject) as GameObject;
                if (prefabGO)
                {
                    var prefabRootGO = prefabGO.transform.root.gameObject;
                    if (prefabRootGO)
                    {
                        var identities = prefabRootGO.GetComponentsInChildren<NetworkIdentity>();
                        if (identities.Length > 1 && !prefabWarnings.Contains(prefabRootGO.name))
                        {
                            // make sure we only print one error per prefab
                            prefabWarnings.Add(prefabRootGO.name);

                            Debug.LogWarningFormat("Prefab '{0}' has several NetworkIdentity components attached to itself or its children, this is not supported.", prefabRootGO.name);
                        }
                    }
                }
            }
        }
    }
}
#endif //ENABLE_UNET
