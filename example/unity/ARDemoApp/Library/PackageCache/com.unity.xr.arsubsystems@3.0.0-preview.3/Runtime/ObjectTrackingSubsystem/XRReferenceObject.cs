using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents a 3D scan of a real object that can be recognized in the environment.
    /// </summary>
    /// <remarks>
    /// Reference objects contain a list of provider-specific "entries".
    /// Each "entry" must have previously been generated in a
    /// format specific to its implementation of the <see cref="XRObjectTrackingSubsystem"/>.
    /// </remarks>
    /// <seealso cref="XRReferenceObjectLibrary"/>
    /// <seealso cref="XRReferenceObjectEntry"/>
    [Serializable]
    public struct XRReferenceObject : IEquatable<XRReferenceObject>
    {
        /// <summary>
        /// A string name for this reference object.
        /// </summary>
        public string name => m_Name;

        /// <summary>
        /// A <c>Guid</c> unique to this reference object.
        /// </summary>
        public Guid guid => GuidUtil.Compose(m_GuidLow, m_GuidHigh);

        /// <summary>
        /// Finds an <see cref="XRReferenceObjectEntry"/> by type.
        /// </summary>
        /// <typeparam name="T">The specific type of <see cref="XRReferenceObjectEntry"/> to find.</typeparam>
        /// <returns>The provider-specific <see cref="XRReferenceObjectEntry"/> if found, otherwise <c>null</c>.</returns>
        /// <seealso cref="FindEntry(Type)"/>
        public T FindEntry<T>() where T : XRReferenceObjectEntry => FindEntry(typeof(T)) as T;

        /// <summary>
        /// Finds an <see cref="XRReferenceObjectEntry"/> by type.
        /// </summary>
        /// <param name="type">The specific type of <see cref="XRReferenceObjectEntry"/> to find.</param>
        /// <returns>The provider-specific <see cref="XRReferenceObjectEntry"/> if found, otherwise <c>null</c>.</returns>
        /// <seealso cref="FindEntry{T}"/>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="type"/> is <c>null</c>.</exception>
        public XRReferenceObjectEntry FindEntry(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            foreach (var entry in m_Entries)
            {
                if ((entry != null) && (entry.GetType() == type))
                    return entry;
            }

            return null;
        }

        public bool Equals(XRReferenceObject other)
        {
            return
                (m_GuidLow == other.m_GuidLow) &&
                (m_GuidHigh == other.m_GuidHigh) &&
                string.Equals(m_Name, other.m_Name) &&
                ReferenceEquals(m_Entries, other.m_Entries);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_GuidLow.GetHashCode();
                hash = hash * 486187739 + m_GuidHigh.GetHashCode();
                hash = hash * 486187739 + (m_Name == null ? 0 : m_Name.GetHashCode());
                hash = hash * 486187739 + (m_Entries == null ? 0 : m_Entries.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object obj) => obj is XRReferenceObject && Equals((XRReferenceObject)obj);

        public static bool operator ==(XRReferenceObject lhs, XRReferenceObject rhs) => lhs.Equals(rhs);

        public static bool operator !=(XRReferenceObject lhs, XRReferenceObject rhs) => !lhs.Equals(rhs);

#pragma warning disable CS0649
        [SerializeField]
        internal ulong m_GuidLow;

        [SerializeField]
        internal ulong m_GuidHigh;

        [SerializeField]
        internal string m_Name;

        [SerializeField]
        internal List<XRReferenceObjectEntry> m_Entries;
#pragma warning restore CS0649
    }
}
