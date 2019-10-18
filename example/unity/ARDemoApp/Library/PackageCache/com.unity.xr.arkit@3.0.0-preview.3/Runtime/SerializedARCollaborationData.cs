using System;
using Unity.Collections;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents the serialized form of an <see cref="ARCollaborationData"/>. Obtain with <see cref="ARCollaborationData.ToSerialized"/>.
    /// </summary>
    public struct SerializedARCollaborationData : IDisposable, IEquatable<SerializedARCollaborationData>
    {
        /// <summary>
        /// Whether the <see cref="SerializedARCollaborationData"/> has been created or not.
        /// </summary>
        public bool created => m_NSData.created;

        /// <summary>
        /// Get the raw bytes of the serialized <see cref="ARCollaborationData"/> as a <c>NativeSlice</c>.
        /// No copies are made; the <c>NativeSlice</c> is a "view" into the raw data.
        /// The <c>NativeSlice</c> is valid until this object is <see cref="Dispose"/>d.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="created"/> is <c>false</c>.</exception>
        public unsafe NativeSlice<byte> bytes
        {
            get
            {
                if (!created)
                    throw new InvalidOperationException("The SerializedARCollaborationData has not been created.");

                return m_NSData.ToNativeSlice();
            }
        }

        /// <summary>
        /// Disposes the native resource associated with this <see cref="SerializedARCollaborationData"/>.
        /// </summary>
        public void Dispose() => m_NSData.Dispose();

        /// <summary>
        /// Generates a hash code suitable for use in <c>HashSet</c> and <c>Dictionary</c>.
        /// </summary>
        /// <returns>A hash of the <see cref="SerializedARCollaborationData"/>.</returns>
        public override int GetHashCode() => m_NSData.GetHashCode();

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="obj">An <c>object</c> to compare against.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is an <see cref="SerializedARCollaborationData"/> and
        /// <see cref="Equals(SerializedARCollaborationData)"/> is also <c>true</c>. Otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => (obj is SerializedARCollaborationData) && Equals((SerializedARCollaborationData)obj);

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="other">The other <see cref="SerializedARCollaborationData"/> to compare against.</param>
        /// <returns><c>true</c> if the <see cref="SerializedARCollaborationData"/> represents the same object.</returns>
        public bool Equals(SerializedARCollaborationData other) => m_NSData.Equals(other.m_NSData);

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/> for equality using <see cref="Equals(SerializedARCollaborationData)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand-side <see cref="SerializedARCollaborationData"/> of the comparison.</param>
        /// <param name="rhs">The right-hand-side <see cref="SerializedARCollaborationData"/> of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> compares equal to <paramref name="rhs"/>, <c>false</c> otherwise.</returns>
        public static bool operator ==(SerializedARCollaborationData lhs, SerializedARCollaborationData rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/> for inequality using <see cref="Equals(SerializedARCollaborationData)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand-side <see cref="SerializedARCollaborationData"/> of the comparison.</param>
        /// <param name="rhs">The right-hand-side <see cref="SerializedARCollaborationData"/> of the comparison.</param>
        /// <returns><c>false</c> if <paramref name="lhs"/> compares equal to <paramref name="rhs"/>, <c>true</c> otherwise.</returns>
        public static bool operator !=(SerializedARCollaborationData lhs, SerializedARCollaborationData rhs) => !lhs.Equals(rhs);

        internal SerializedARCollaborationData(NSData nsData)
        {
            m_NSData = nsData;
        }

        NSData m_NSData;
    }
}
