using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A <c>Guid</c> that can be serialized by Unity. The 128-bit <c>Guid</c>
    /// is stored as two 64-bit <c>ulong</c>s. See also the creation utility at
    /// <c>UnityEditor.XR.ARSubsystems.SerializableGuidUtil</c>.
    /// </summary>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>
    {
        /// <summary>
        /// Constructs a <see cref="SerializableGuid"/> from two 64-bit <c>ulong</c>s.
        /// </summary>
        /// <param name="guidLow">The low 8 bytes of the <c>Guid</c>.</param>
        /// <param name="guidHigh">The high 8 bytes of the <c>Guid</c>.</param>
        public SerializableGuid(ulong guidLow, ulong guidHigh)
        {
            m_GuidLow = guidLow;
            m_GuidHigh = guidHigh;
        }

        static readonly SerializableGuid k_Empty = new SerializableGuid(0, 0);

        /// <summary>
        /// Used to represent <c>System.Guid.Empty</c>, e.g., a GUID whose value is all zeros.
        /// </summary>
        public static SerializableGuid empty => k_Empty;

        /// <summary>
        /// Reconstructs the <c>Guid</c> from the serialized data.
        /// </summary>
        public Guid guid => GuidUtil.Compose(m_GuidLow, m_GuidHigh);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_GuidLow.GetHashCode();
                return hash * 486187739 + m_GuidHigh.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SerializableGuid))
                return false;

            return Equals((SerializableGuid)obj);
        }

        /// <summary>
        /// Generates a string representation of the <c>Guid</c>. Same as <see cref="guid"/><c>.ToString()</c>.
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.7.2#System_Guid_ToString">Microsoft's documentation</a>
        /// for more details.
        /// </summary>
        /// <returns>A string representation of the <c>Guid</c>.</returns>
        public override string ToString() => guid.ToString();

        /// <summary>
        /// Generates a string representation of the <c>Guid</c>. Same as <see cref="guid"/><c>.ToString(format)</c>.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of the <c>Guid</c>.
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.7.2#System_Guid_ToString_System_String_">Microsoft's documentation</a>
        /// for more details.</param>
        /// <returns>A string representation of the <c>Guid</c>.</returns>
        public string ToString(string format) => guid.ToString(format);

        /// <summary>
        /// Generates a string representation of the <c>Guid</c>. Same as <see cref="guid"/><c>.ToString(format, provider)</c>.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of the <c>Guid</c>.
        /// See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netframework-4.7.2#System_Guid_ToString_System_String_System_IFormatProvider_">Microsoft's documentation</a>
        /// for more details.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of the <c>Guid</c>.</returns>
        public string ToString(string format, IFormatProvider provider) => guid.ToString(format, provider);

        public bool Equals(SerializableGuid other)
        {
            return
                (m_GuidLow == other.m_GuidLow) &&
                (m_GuidHigh == other.m_GuidHigh);
        }

        public static bool operator ==(SerializableGuid lhs, SerializableGuid rhs) => lhs.Equals(rhs);

        public static bool operator !=(SerializableGuid lhs, SerializableGuid rhs) => !lhs.Equals(rhs);

        [SerializeField]
        ulong m_GuidLow;

        [SerializeField]
        ulong m_GuidHigh;
    }
}
