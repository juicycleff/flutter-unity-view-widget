using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents the Objective-C type ARCollaborationData.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This struct mirrors the Objective-C type ARCollaborationData. Because it
    /// represents a native resource, it must be explicitly disposed when no
    /// longer needed.
    /// </para><para>
    /// ARCollaborationData can be constructed from a byte array, or from
    /// <see cref="ARKitSessionSubsystem.DequeueCollaborationData"/>.
    /// </para><para>
    /// This struct is not thread-safe, but it may be constructed and disposed on any thread.
    /// </para>
    /// </remarks>
    public struct ARCollaborationData : IDisposable, IEquatable<ARCollaborationData>
    {
        /// <summary>
        /// Constructs an ARCollaborationData from a byte array.
        /// Check <see cref="valid"/> after construction to ensure <paramref name="bytes"/> was successfully deserialized.
        /// </summary>
        /// <param name="bytes">An array of <c>byte</c>s to convert to <see cref="ARCollaborationData"/>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="bytes"/> is null.</exception>
        /// <seealso cref="ToSerialized"/>
        public unsafe ARCollaborationData(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            m_NativePtr = ConstructUnchecked(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Constructs an ARCollaborationData from a byte array.
        /// Check <see cref="valid"/> after construction to ensure <paramref name="bytes"/> was successfully deserialized.
        /// </summary>
        /// <param name="bytes">An array of <c>byte</c>s to convert to <see cref="ARCollaborationData"/>.</param>
        /// <param name="offset">The offset into the <paramref name="bytes"/> array from which to start constructing <see cref="ARCollaborationData"/>.</param>
        /// <param name="length">The number of bytes in <paramref name="bytes"/> to convert to <see cref="ARCollaborationData"/>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="bytes"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is outside the range [0..bytes.Length).</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="length"/> is outside the range [0..(bytes.Length - offset)].</exception>
        /// <seealso cref="ToSerialized"/>
        public unsafe ARCollaborationData(byte[] bytes, int offset, int length)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"'{nameof(offset)}' must be greater than or equal to zero.");

            if (offset >= bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"'{nameof(offset)}' must be less than the length of the byte array ({bytes.Length}).");

            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, $"'{nameof(length)}' must be greater than zero.");

            if (length > (bytes.Length - offset))
                throw new ArgumentOutOfRangeException(nameof(length), length, $"'{nameof(length)}' is greater than the number of available bytes in the buffer ({bytes.Length - offset})");

            m_NativePtr = ConstructUnchecked(bytes, offset, length);
        }

        /// <summary>
        /// Constructs an ARCollaborationData from a <c>NativeSlice</c> of <c>byte</c>s.
        /// Check <see cref="valid"/> after construction to ensure <paramref name="bytes"/> was successfully deserialized.
        /// </summary>
        /// <param name="bytes">An array of <c>byte</c>s to convert to <see cref="ARCollaborationData"/>.</param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="bytes"/> does not refer to valid data.</exception>
        /// <seealso cref="ToSerialized"/>
        public unsafe ARCollaborationData(NativeSlice<byte> bytes)
        {
            void* ptr = bytes.GetUnsafePtr();
            if ((ptr == null) || (bytes.Length == 0))
                throw new ArgumentException("Invalid NativeSlice", nameof(bytes));

            m_NativePtr = ConstructUnchecked(ptr, bytes.Length);
        }

        /// <summary>
        /// True if the data is valid. The data may be invalid if this object was constructed
        /// with an invalid byte array, or if it has been disposed.
        /// </summary>
        public bool valid => m_NativePtr != IntPtr.Zero;

        /// <summary>
        /// Gets the priority of the collaboration data. Use this to determine how
        /// you should send the information to peers in a collaborative session,
        /// e.g., reliably vs unreliably.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="valid"/> is false.</exception>
        public ARCollaborationDataPriority priority
        {
            get
            {
                ValidateAndThrow();
                return UnityARKit_session_getCollaborationDataPriority(m_NativePtr);
            }
        }

        /// <summary>
        /// Dispose the native ARCollaborationData. <see cref="valid"/> will be false after disposal.
        /// It is safe to dispose an invalid or already disposed ARCollaborationData.
        /// </summary>
        public void Dispose()
        {
            UnityARKit_CFRelease(m_NativePtr);
            m_NativePtr = IntPtr.Zero;
        }

        /// <summary>
        /// Copies the bytes representing the serialized <see cref="ARCollaborationData"/> to a
        /// <see cref="SerializedARCollaborationData"/>.
        /// A common use case would be to send these bytes to another device over a network.
        /// </summary>
        /// <returns>A container representing the serialized bytes of this <see cref="ARCollaborationData"/>.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="valid"/> is false.</exception>
        public unsafe SerializedARCollaborationData ToSerialized()
        {
            ValidateAndThrow();

            var nsData = new NSData(UnityARKit_session_serializeCollaborationDataToNSData(m_NativePtr));
            return new SerializedARCollaborationData(nsData);
        }

        /// <summary>
        /// Generates a hash code suitable for use in <c>HashSet</c> and <c>Dictionary</c>.
        /// </summary>
        /// <returns>A hash of the <see cref="ARCollaborationData"/>.</returns>
        public override int GetHashCode() => m_NativePtr.GetHashCode();

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="obj">An <c>object</c> to compare against.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is an <see cref="ARCollaborationData"/> and
        /// <see cref="Equals(ARCollaborationData)"/> is also <c>true</c>. Otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => (obj is ARCollaborationData) && Equals((ARCollaborationData)obj);

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="other">The other <see cref="ARCollaborationData"/> to compare against.</param>
        /// <returns><c>true</c> if the <see cref="ARCollaborationData"/> represents the same object.</returns>
        public bool Equals(ARCollaborationData other) => m_NativePtr == other.m_NativePtr;

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/> for equality using <see cref="Equals(ARCollaborationData)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand-side <see cref="ARCollaborationData"/> of the comparison.</param>
        /// <param name="rhs">The right-hand-side <see cref="ARCollaborationData"/> of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> compares equal to <paramref name="rhs"/>, <c>false</c> otherwise.</returns>
        public static bool operator ==(ARCollaborationData lhs, ARCollaborationData rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Compares <paramref name="lhs"/> and <paramref name="rhs"/> for inequality using <see cref="Equals(ARCollaborationData)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand-side <see cref="ARCollaborationData"/> of the comparison.</param>
        /// <param name="rhs">The right-hand-side <see cref="ARCollaborationData"/> of the comparison.</param>
        /// <returns><c>false</c> if <paramref name="lhs"/> compares equal to <paramref name="rhs"/>, <c>true</c> otherwise.</returns>
        public static bool operator !=(ARCollaborationData lhs, ARCollaborationData rhs) => !lhs.Equals(rhs);

        internal ARCollaborationData(IntPtr data) => m_NativePtr = data;

        internal ARCollaborationData(NSData data) => m_NativePtr = UnityARKit_session_deserializeCollaborationDataFromNSData(data);

        void ValidateAndThrow()
        {
            if (!valid)
                throw new InvalidOperationException("ARCollaborationData has already been disposed.");
        }

        unsafe static IntPtr ConstructUnchecked(void* bytes, int length)
        {
            using (var nsData = NSData.CreateWithBytesNoCopy(bytes, length))
            {
                return UnityARKit_session_deserializeCollaborationDataFromNSData(nsData);
            }
        }

        unsafe static IntPtr ConstructUnchecked(byte[] bytes, int offset, int length)
        {
            fixed(void* ptr = &bytes[offset])
            {
                return ConstructUnchecked(ptr, length);
            }
        }

        [DllImport("__Internal")]
        static extern void UnityARKit_CFRelease(IntPtr ptr);

        [DllImport("__Internal")]
        static extern IntPtr UnityARKit_session_deserializeCollaborationDataFromNSData(IntPtr nsData);

        [DllImport("__Internal")]
        static extern IntPtr UnityARKit_session_serializeCollaborationDataToNSData(IntPtr collaborationData);

        [DllImport("__Internal")]
        static extern ARCollaborationDataPriority UnityARKit_session_getCollaborationDataPriority(IntPtr collaborationData);

        internal IntPtr m_NativePtr;
    }
}
