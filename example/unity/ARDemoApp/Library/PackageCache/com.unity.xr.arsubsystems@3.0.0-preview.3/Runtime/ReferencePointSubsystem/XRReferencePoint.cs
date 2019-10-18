using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes session relative data for a reference point.
    /// </summary>
    /// <seealso cref="XRReferencePointSubsystem"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRReferencePoint : ITrackable, IEquatable<XRReferencePoint>
    {
        /// <summary>
        /// Gets a default-initialized <see cref="XRReferencePoint"/>. This may be
        /// different from the zero-initialized version, e.g., the <see cref="pose"/>
        /// is <c>Pose.identity</c> instead of zero-initialized.
        /// </summary>
        public static XRReferencePoint defaultValue => s_Default;

        static readonly XRReferencePoint s_Default = new XRReferencePoint
        {
            m_Id = TrackableId.invalidId,
            m_Pose = Pose.identity,
            m_SessionId = Guid.Empty
        };

        /// <summary>
        /// Constructs the session relative data for reference point.
        /// This is typically provided by an implementation of the <see cref="XRReferencePointSubsystem"/>
        /// and not invoked directly.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> associated with this reference point.</param>
        /// <param name="pose">The <c>Pose</c>, in session space, of the reference point.</param>
        /// <param name="trackingState">The <see cref="TrackingState"/> of the reference point.</param>
        /// <param name="nativePtr">A native pointer associated with the reference point. The data pointed to by
        /// this pointer is implementation-specific.</param>
        public XRReferencePoint(
            TrackableId trackableId,
            Pose pose,
            TrackingState trackingState,
            IntPtr nativePtr)
        {
            m_Id = trackableId;
            m_Pose = pose;
            m_TrackingState = trackingState;
            m_NativePtr = nativePtr;
            m_SessionId = Guid.Empty;
        }

        /// <summary>
        /// Constructs the session relative data for reference point.
        /// This is typically provided by an implementation of the <see cref="XRReferencePointSubsystem"/>
        /// and not invoked directly.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> associated with this reference point.</param>
        /// <param name="pose">The <c>Pose</c>, in session space, of the reference point.</param>
        /// <param name="trackingState">The <see cref="TrackingState"/> of the reference point.</param>
        /// <param name="nativePtr">A native pointer associated with the reference point. The data pointed to by
        /// this pointer is implementation-specific.</param>
        /// <param name="sessionId">The session from which this reference point originated.</param>
        public XRReferencePoint(
            TrackableId trackableId,
            Pose pose,
            TrackingState trackingState,
            IntPtr nativePtr,
            Guid sessionId)
        : this(trackableId, pose, trackingState, nativePtr)
        {
            m_SessionId = sessionId;
        }

        /// <summary>
        /// Get the <see cref="TrackableId"/> associated with this reference point.
        /// </summary>
        public TrackableId trackableId => m_Id;

        /// <summary>
        /// Get the <c>Pose</c>, in session space, for this reference point.
        /// </summary>
        public Pose pose => m_Pose;

        /// <summary>
        /// Get the <see cref="TrackingState"/> of this reference point.
        /// </summary>
        public TrackingState trackingState => m_TrackingState;

        /// <summary>
        /// A native pointer associated with the reference point.
        /// The data pointed to by this pointer is implementation-specific.
        /// </summary>
        public IntPtr nativePtr => m_NativePtr;

        /// <summary>
        /// The id of the session from which this reference point originated.
        /// </summary>
        public Guid sessionId => m_SessionId;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m_Id.GetHashCode();
                hashCode = hashCode * 486187739 + m_Pose.GetHashCode();
                hashCode = hashCode * 486187739 + m_TrackingState.GetHashCode();
                hashCode = hashCode * 486187739 + m_NativePtr.GetHashCode();
                hashCode = hashCode * 486187739 + m_SessionId.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(XRReferencePoint other)
        {
            return
                m_Id.Equals(other.m_Id) &&
                m_Pose.Equals(other.m_Pose) &&
                m_TrackingState == other.m_TrackingState &&
                m_NativePtr == other.m_NativePtr &&
                m_SessionId.Equals(other.m_SessionId);

        }

        public override bool Equals(object obj) => obj is XRReferencePoint && Equals((XRReferencePoint)obj);

        public static bool operator==(XRReferencePoint lhs, XRReferencePoint rhs) => lhs.Equals(rhs);

        public static bool operator!=(XRReferencePoint lhs, XRReferencePoint rhs) => !lhs.Equals(rhs);

        TrackableId m_Id;

        Pose m_Pose;

        TrackingState m_TrackingState;

        IntPtr m_NativePtr;

        Guid m_SessionId;
    }
}
