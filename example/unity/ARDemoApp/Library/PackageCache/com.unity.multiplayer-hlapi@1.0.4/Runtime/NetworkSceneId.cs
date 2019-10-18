using System;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This is used to identify networked objects in a scene. These values are allocated in the editor and are persistent for the lifetime of the object in the scene.
    /// </summary>
    [Serializable]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public struct NetworkSceneId : IEquatable<NetworkSceneId>
    {
        public NetworkSceneId(uint value)
        {
            m_Value = value;
        }

        [SerializeField]
        uint m_Value;

        /// <summary>
        /// Returns true if the value is zero. Non-scene objects - ones which are spawned at runtime will have a sceneId of zero.
        /// </summary>
        /// <returns>True if zero.</returns>
        public bool IsEmpty()
        {
            return m_Value == 0;
        }

        public override int GetHashCode()
        {
            return (int)m_Value;
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkSceneId && Equals((NetworkSceneId)obj);
        }

        public bool Equals(NetworkSceneId other)
        {
            return this == other;
        }

        public static bool operator==(NetworkSceneId c1, NetworkSceneId c2)
        {
            return c1.m_Value == c2.m_Value;
        }

        public static bool operator!=(NetworkSceneId c1, NetworkSceneId c2)
        {
            return c1.m_Value != c2.m_Value;
        }

        /// <summary>
        /// Returns a string like SceneId:value.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return m_Value.ToString();
        }

        /// <summary>
        /// The internal value for this object.
        /// </summary>
        public uint Value { get { return m_Value; } }
    }
}
