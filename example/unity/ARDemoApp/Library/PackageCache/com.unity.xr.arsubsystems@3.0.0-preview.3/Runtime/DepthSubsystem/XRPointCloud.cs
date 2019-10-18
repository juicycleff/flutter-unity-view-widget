using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the session relative data for the <see cref="XRDepthSubsystem"/>.
    /// <see cref="XRPointCloud"/>s are usually created by <see cref="XRDepthSubsystem.GetChanges(Unity.Collections.Allocator)"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct XRPointCloud : ITrackable, IEquatable<XRPointCloud>
    {
        /// <summary>
        /// Gets a default-initialized <see cref="XRPointCloud"/>. This may be
        /// different from the zero-initialized version, e.g., the <see cref="pose"/>
        /// is <c>Pose.identity</c> instead of zero-initialized.
        /// </summary>
        public static XRPointCloud defaultValue => s_Default;

        static readonly XRPointCloud s_Default = new XRPointCloud
        {
            m_TrackableId = TrackableId.invalidId,
            m_Pose = Pose.identity,
        };

        /// <summary>
        /// Constructs a new <see cref="XRPointCloud"/>. This is a container
        /// for the session-relative data. These are typically created by
        /// <see cref="XRDepthSubsystem.GetChanges(Unity.Collections.Allocator)"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> associated with the point cloud.</param>
        /// <param name="pose">The <c>Pose</c> associated with the point cloud.</param>
        /// <param name="trackingState">The <see cref="TrackingState"/> associated with the point cloud.</param>
        /// <param name="nativePtr">The native pointer associated with the point cloud.</param>
        public XRPointCloud(
            TrackableId trackableId,
            Pose pose,
            TrackingState trackingState,
            IntPtr nativePtr)
        {
            m_TrackableId = trackableId;
            m_Pose = pose;
            m_TrackingState = trackingState;
            m_NativePtr = nativePtr;
        }

        /// <summary>
        /// Get the <see cref="TrackableId"/> associated with this point cloud.
        /// </summary>
        public TrackableId trackableId => m_TrackableId;

        /// <summary>
        /// Get the <c>Pose</c> associated with this point cloud.
        /// </summary>
        /// <remarks>
        /// Point cloud points are relative to this pose.
        /// </remarks>
        public Pose pose => m_Pose;

        /// <summary>
        /// Get the <see cref="TrackingState"/> associated with this point cloud.
        /// </summary>
        public TrackingState trackingState => m_TrackingState;

        /// <summary>
        /// Get the native pointer associated with this point cloud.
        /// </summary>
        /// <remarks>
        /// The data this pointer points to is implementation defined.
        /// </remarks>
        public IntPtr nativePtr => m_NativePtr;

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_TrackableId.GetHashCode();
                hash = hash * 486187739 + m_Pose.GetHashCode();
                hash = hash * 486187739 + ((int)m_TrackingState).GetHashCode();
                hash = hash * 486187739 + m_NativePtr.GetHashCode();
                return hash;
            }
        }

        public bool Equals(XRPointCloud other)
        {
            return
                (m_TrackableId == other.m_TrackableId) &&
                (m_Pose.Equals(other.pose)) &&
                (m_TrackingState == other.m_TrackingState) &&
                (m_NativePtr == other.m_NativePtr);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return Equals((XRPointCloud)obj);
        }

        public static bool operator==(XRPointCloud lhs, XRPointCloud rhs) => lhs.Equals(rhs);

        public static bool operator!=(XRPointCloud lhs, XRPointCloud rhs) => !lhs.Equals(rhs);

        TrackableId m_TrackableId;

        Pose m_Pose;

        TrackingState m_TrackingState;

        IntPtr m_NativePtr;
    }
}
