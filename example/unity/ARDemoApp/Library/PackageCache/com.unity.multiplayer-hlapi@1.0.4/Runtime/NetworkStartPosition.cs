using System;
using UnityEngine;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This component is used to make a gameObject a starting position for spawning player objects in multiplayer games.
    /// <para>This object's transform will be automatically registered and unregistered with the NetworkManager as a starting position.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkStartPosition")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkStartPosition : MonoBehaviour
    {
        public void Awake()
        {
            NetworkManager.RegisterStartPosition(transform);
        }

        public void OnDestroy()
        {
            NetworkManager.UnRegisterStartPosition(transform);
        }
    }
}
