using System;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This is used to identify networked objects across all participants of a network. It is assigned at runtime by the server when an object is spawned.
    /// </summary>
    [Serializable]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public struct NetworkInstanceId : IEquatable<NetworkInstanceId>
    {
        public NetworkInstanceId(uint value)
        {
            m_Value = value;
        }

        [SerializeField]
        readonly uint m_Value;

        /// <summary>
        /// Returns true if the value of the NetworkInstanceId is zero.
        /// <para>Object that have not been spawned will have a value of zero.</para>
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
            return obj is NetworkInstanceId && Equals((NetworkInstanceId)obj);
        }

        public bool Equals(NetworkInstanceId other)
        {
            return this == other;
        }

        public static bool operator==(NetworkInstanceId c1, NetworkInstanceId c2)
        {
            return c1.m_Value == c2.m_Value;
        }

        public static bool operator!=(NetworkInstanceId c1, NetworkInstanceId c2)
        {
            return c1.m_Value != c2.m_Value;
        }

        /// <summary>
        /// Returns a string of "NetID:value".
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return m_Value.ToString();
        }

        /// <summary>
        /// The internal value of this identifier.
        /// </summary>
        public uint Value { get { return m_Value; } }

        /// <summary>
        /// A static invalid NetworkInstanceId that can be used for comparisons.
        /// <para>The default value of NetworkInstanceId.Value is zero, and IsEmpty() can be used to check this. But NetworkInstanceId.Invalid is available for specifically setting and checking for invalid IDs.</para>
        /// </summary>
        public static NetworkInstanceId Invalid = new NetworkInstanceId(uint.MaxValue);
        internal static NetworkInstanceId Zero = new NetworkInstanceId(0);
    }
}
