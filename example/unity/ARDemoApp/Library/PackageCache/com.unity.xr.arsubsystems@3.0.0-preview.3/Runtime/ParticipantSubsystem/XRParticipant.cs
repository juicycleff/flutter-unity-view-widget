using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// The session-relative data associated with a participant.
    /// </summary>
    /// <remarks>
    /// A "participant" is another device in a multi-user collaborative session.
    /// </remarks>
    /// <seealso cref="XRParticipantSubsystem"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRParticipant : ITrackable, IEquatable<XRParticipant>
    {
        TrackableId m_TrackableId;
        Pose m_Pose;
        TrackingState m_TrackingState;
        IntPtr m_NativePtr;
        Guid m_SessionId;

        /// <summary>
        /// Constructs an <see cref="XRParticipant"/>. <see cref="XRParticipant"/>s are generated
        /// by <see cref="XRParticipantSubsystem.GetChanges(Unity.Collections.Allocator)"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> associated with this participant.</param>
        /// <param name="pose">The <c>Pose</c> associated with this participant.</param>
        /// <param name="trackingState">The <see cref="TrackingState"/> associated with this participant.</param>
        /// <param name="nativePtr">A native pointer associated with this participant.</param>
        /// <param name="sessionId">The session from which this participant originated.</param>
        public XRParticipant(
            TrackableId trackableId,
            Pose pose,
            TrackingState trackingState,
            IntPtr nativePtr,
            Guid sessionId)
        {
            m_TrackableId = trackableId;
            m_Pose = pose;
            m_TrackingState = trackingState;
            m_NativePtr = nativePtr;
            m_SessionId = sessionId;
        }

        /// <summary>
        /// An <see cref="XRParticipant"/> with default values. This is mostly zero-initialized,
        /// except for objects like <c>Pose</c>s, which are initialized to <c>Pose.identity</c>.
        /// </summary>
        public static XRParticipant defaultParticipant => k_Default;

        /// <summary>
        /// The <see cref="TrackableId"/> associated with this participant.
        /// </summary>
        public TrackableId trackableId => m_TrackableId;

        /// <summary>
        /// The <c>Pose</c>, in session-space, associated with this participant.
        /// </summary>
        public Pose pose => m_Pose;

        /// <summary>
        /// The <see cref="TrackingState"/> associated with this participant.
        /// </summary>
        public TrackingState trackingState => m_TrackingState;

        /// <summary>
        /// A native pointer associated with this participant.
        /// The data pointer to by this pointer is implementation defined.
        /// </summary>
        public IntPtr nativePtr => m_NativePtr;

        /// <summary>
        /// This participant's session identifier.
        /// </summary>
        public Guid sessionId => m_SessionId;

        static readonly XRParticipant k_Default = new XRParticipant
        {
            m_TrackableId = TrackableId.invalidId,
            m_Pose = Pose.identity,
            m_NativePtr = IntPtr.Zero
        };

        /// <summary>
        /// Generates a hash suitable for use with containers like <c>HashSet</c> and <c>Dictionary</c>.
        /// </summary>
        /// <returns>A hash suitable for use with containers like <c>HashSet</c> and <c>Dictionary</c>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = m_TrackableId.GetHashCode();
                hash = hash * 486187739 + m_Pose.GetHashCode();
                hash = hash * 486187739 + ((int)m_TrackingState).GetHashCode();
                hash = hash * 486187739 + m_NativePtr.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="other">The other <see cref="XRParticipant"/> to compare against.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this <see cref="XRParticipant"/>.</returns>
        public bool Equals(XRParticipant other)
        {
            return
                m_TrackableId.Equals(other.m_TrackableId) &&
                m_Pose.Equals(other.m_Pose) &&
                (m_TrackingState == other.m_TrackingState) &&
                (m_NativePtr == other.m_NativePtr);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">The <c>object</c> to compare against.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is of type <see cref="XRParticipant"/> and <see cref="Equals(XRParticipant)"/>
        /// also returns <c>true</c>.</returns>
        public override bool Equals(object obj) => (obj is XRParticipant) && Equals((XRParticipant)obj);

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(XRParticipant)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is equal to <paramref name="rhs"/>.</returns>
        public static bool operator ==(XRParticipant lhs, XRParticipant rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as <c>!</c><see cref="Equals(XRParticipant)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is not equal to <paramref name="rhs"/>.</returns>
        public static bool operator !=(XRParticipant lhs, XRParticipant rhs) => !lhs.Equals(rhs);
    }
}
