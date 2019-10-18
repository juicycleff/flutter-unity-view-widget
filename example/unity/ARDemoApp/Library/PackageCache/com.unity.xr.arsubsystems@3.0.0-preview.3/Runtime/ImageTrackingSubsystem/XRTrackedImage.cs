using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Contains low-level data for a tracked image in the environment.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRTrackedImage : ITrackable, IEquatable<XRTrackedImage>
    {
        /// <summary>
        /// Constructs an <see cref="XRTrackedImage"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> associated with this tracked image.</param>
        /// <param name="sourceImageId">A <c>GUID</c> associated with the source image.</param>
        /// <param name="pose">The <c>Pose</c> associated with the detected image.</param>
        /// <param name="size">The size (i.e., dimensions) of the detected image.</param>
        /// <param name="trackingState">The <see cref="TrackingState"/> of the detected image.</param>
        /// <param name="nativePtr">A native pointer associated with the detected image.</param>
        public XRTrackedImage(
            TrackableId trackableId,
            Guid sourceImageId,
            Pose pose,
            Vector2 size,
            TrackingState trackingState,
            IntPtr nativePtr)
        {
            m_Id = trackableId;
            m_SourceImageId = sourceImageId;
            m_Pose = pose;
            m_Size = size;
            m_TrackingState = trackingState;
            m_NativePtr = nativePtr;
        }

        /// <summary>
        /// Generates a <see cref="XRTrackedImage"/> populated with default values.
        /// </summary>
        public static XRTrackedImage defaultValue => s_Default;

        static readonly XRTrackedImage s_Default = new XRTrackedImage
        {
            m_Id = TrackableId.invalidId,
            m_SourceImageId = Guid.Empty,
            m_Pose = Pose.identity,
        };

        /// <summary>
        /// The <see cref="TrackableId"/> associated with this tracked image.
        /// </summary>
        public TrackableId trackableId => m_Id;

        /// <summary>
        /// The <c>GUID</c> associated with the source image.
        /// </summary>
        public Guid sourceImageId => m_SourceImageId;

        /// <summary>
        /// The <c>Pose</c> associated with this tracked image.
        /// </summary>
        public Pose pose => m_Pose;

        /// <summary>
        /// The size (i.e., dimensions) of this tracked image.
        /// </summary>
        public Vector2 size => m_Size;

        /// <summary>
        /// The <see cref="TrackingState"/> associated with this tracked image.
        /// </summary>
        public TrackingState trackingState => m_TrackingState;

        /// <summary>
        /// A native pointer associated with this tracked image.
        /// The data pointed to by this pointer is implementation-defined.
        /// While its lifetime is also implementation-defined, it should be
        /// valid at least until the next call to
        /// <see cref="XRImageTrackingSubsystem.GetChanges(Allocator)"/>.
        /// </summary>
        public IntPtr nativePtr => m_NativePtr;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m_Id.GetHashCode();
                hashCode = hashCode * 486187739 + m_SourceImageId.GetHashCode();
                hashCode = hashCode * 486187739 + m_Pose.GetHashCode();
                hashCode = hashCode * 486187739 + m_Size.GetHashCode();
                hashCode = hashCode * 486187739 + m_TrackingState.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(XRTrackedImage other)
        {
            return
                m_Id.Equals(other.m_Id) &&
                m_SourceImageId.Equals(other.m_SourceImageId) &&
                m_Pose.Equals(other.m_Pose) &&
                m_Size.Equals(other.m_Size) &&
                m_TrackingState == other.m_TrackingState;
        }

        public override bool Equals(object obj) => obj is XRTrackedImage && Equals((XRTrackedImage)obj);

        public static bool operator==(XRTrackedImage lhs, XRTrackedImage rhs) => lhs.Equals(rhs);

        public static bool operator!=(XRTrackedImage lhs, XRTrackedImage rhs) => !lhs.Equals(rhs);

        TrackableId m_Id;
        Guid m_SourceImageId;
        Pose m_Pose;
        Vector2 m_Size;
        TrackingState m_TrackingState;
        IntPtr m_NativePtr;
    }
}
