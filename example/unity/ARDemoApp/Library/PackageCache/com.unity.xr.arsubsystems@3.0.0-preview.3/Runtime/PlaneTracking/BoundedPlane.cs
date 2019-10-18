using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// The session relative data associated with a plane.
    /// </summary>
    /// <seealso cref="XRPlaneSubsystem"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundedPlane : ITrackable, IEquatable<BoundedPlane>
    {
        static readonly BoundedPlane s_Default = new BoundedPlane(
                TrackableId.invalidId,
                TrackableId.invalidId,
                Pose.identity,
                Vector2.zero,
                Vector2.zero,
                PlaneAlignment.None,
                TrackingState.None,
                IntPtr.Zero);

        /// <summary>
        /// Gets a default-initialized <see cref="BoundedPlane"/>. This may be
        /// different from the zero-initialized version, e.g., the <see cref="pose"/>
        /// is <c>Pose.identity</c> instead of zero-initialized.
        /// </summary>
        public static BoundedPlane defaultValue => s_Default;

        /// <summary>
        /// Constructs a new <see cref="BoundedPlane"/>. This is just a data container
        /// for a plane's session relative data. These are typically created by
        /// <see cref="XRPlaneSubsystem.GetChanges(Unity.Collections.Allocator)"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> associated with the point cloud.</param>
        /// <param name="subsumedBy">The plane which subsumed this one. Use <see cref="TrackableId.invalidId"/> if it has not been subsumed.</param>
        /// <param name="pose">The <c>Pose</c> associated with the point cloud.</param>
        /// <param name="center">The center, in plane-space (relative to <paramref name="pose"/>) of the plane.</param>
        /// <param name="size">The dimensions associated with the point cloud.</param>
        /// <param name="alignment">The <see cref="PlaneAlignment"/> associated with the point cloud.</param>
        /// <param name="trackingState">The <see cref="TrackingState"/> associated with the point cloud.</param>
        /// <param name="nativePtr">The native pointer associated with the point cloud.</param>
        public BoundedPlane(
            TrackableId trackableId,
            TrackableId subsumedBy,
            Pose pose,
            Vector2 center,
            Vector2 size,
            PlaneAlignment alignment,
            TrackingState trackingState,
            IntPtr nativePtr)
        {
            m_TrackableId = trackableId;
            m_SubsumedById = subsumedBy;
            m_Pose = pose;
            m_Center = center;
            m_Size = size;
            m_Alignment = alignment;
            m_TrackingState = trackingState;
            m_NativePtr = nativePtr;
        }

        /// <summary>
        /// The <see cref="TrackableId"/> associated with this plane.
        /// </summary>
        public TrackableId trackableId { get { return m_TrackableId; } }

        /// <summary>
        /// The <see cref="TrackableId"/> associated with the plane which subsumed this one. Will be <see cref="TrackableId.invalidId"/>
        /// if this plane has not been subsumed.
        /// </summary>
        public TrackableId subsumedById { get { return m_SubsumedById; } }

        /// <summary>
        /// The <c>Pose</c>, in session space, of the plane.
        /// </summary>
        public Pose pose { get { return m_Pose; } }

        /// <summary>
        /// The center of the plane in plane space (relative to its <see cref="pose"/>).
        /// </summary>
        public Vector2 center { get { return m_Center; } }

        /// <summary>
        /// The extents of the plane (half dimensions) in meters.
        /// </summary>
        public Vector2 extents { get { return m_Size * 0.5f; } }

        /// <summary>
        /// The size (dimensions) of the plane in meters.
        /// </summary>
        public Vector2 size { get { return m_Size; } }

        /// <summary>
        /// The <see cref="PlaneAlignment"/> of the plane.
        /// </summary>
        public PlaneAlignment alignment { get { return m_Alignment; } }

        /// <summary>
        /// The <see cref="TrackingState"/> of the plane.
        /// </summary>
        public TrackingState trackingState { get { return m_TrackingState; } }

        /// <summary>
        /// A native pointer associated with this plane.
        /// The data pointer to by this pointer is implementation defined.
        /// </summary>
        public IntPtr nativePtr { get { return m_NativePtr; } }

        /// <summary>
        /// The width of the plane in meters.
        /// </summary>
        public float width { get { return m_Size.x; } }

        /// <summary>
        /// The height (or depth) of the plane in meters.
        /// </summary>
        public float height { get { return m_Size.y; } }

        /// <summary>
        /// The normal of the plane in session space.
        /// </summary>
        public Vector3 normal { get { return m_Pose.up; } }

        /// <summary>
        /// Gets an infinite plane in session space.
        /// </summary>
        public Plane plane { get { return new Plane(normal, center); } }

        /// <summary>
        /// Get the four corners of the plane in session space in clockwise order.
        /// </summary>
        /// <param name="p0">The first vertex.</param>
        /// <param name="p1">The second vertex.</param>
        /// <param name="p2">The third vertex.</param>
        /// <param name="p3">The fourth vertex.</param>
        public void GetCorners(
            out Vector3 p0,
            out Vector3 p1,
            out Vector3 p2,
            out Vector3 p3)
        {
            var sessionCenter = m_Pose.rotation * center + m_Pose.position;
            var sessionHalfX = (m_Pose.right) * (width * .5f);
            var sessionHalfZ = (m_Pose.forward) * (height * .5f);
            p0 = sessionCenter - sessionHalfX - sessionHalfZ;
            p1 = sessionCenter - sessionHalfX + sessionHalfZ;
            p2 = sessionCenter + sessionHalfX + sessionHalfZ;
            p3 = sessionCenter + sessionHalfX - sessionHalfZ;
        }

        /// <summary>
        /// Generates a new string describing the plane's properties suitable for debugging purposes.
        /// </summary>
        /// <returns>A string describing the plane's properties.</returns>
        public override string ToString()
        {
            return string.Format(
                "Plane:\n\ttrackableId: {0}\n\tsubsumedById: {1}\n\tpose: {2}\n\tcenter: {3}\n\tsize: {4}\n\talignment: {5}\n\ttrackingState: {6}\n\tnativePtr: {7:X16}",
                trackableId,
                subsumedById,
                pose,
                center,
                size,
                alignment,
                trackingState,
                nativePtr.ToInt64());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BoundedPlane && Equals((BoundedPlane)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m_TrackableId.GetHashCode();
                hashCode = (hashCode * 486187739) + m_SubsumedById.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Pose.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Center.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Size.GetHashCode();
                hashCode = (hashCode * 486187739) + ((int)m_Alignment).GetHashCode();
                hashCode = (hashCode * 486187739) + ((int)m_TrackingState).GetHashCode();
                hashCode = (hashCode * 486187739) + m_NativePtr.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(BoundedPlane lhs, BoundedPlane rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(BoundedPlane lhs, BoundedPlane rhs)
        {
            return !lhs.Equals(rhs);
        }

        public bool Equals(BoundedPlane other)
        {
            return
                m_TrackableId.Equals(other.m_TrackableId) &&
                m_SubsumedById.Equals(other.m_SubsumedById) &&
                m_Pose.Equals(other.m_Pose) &&
                m_Center.Equals(other.m_Center) &&
                m_Size.Equals(other.m_Size) &&
                (m_Alignment == other.m_Alignment) &&
                (m_TrackingState == other.m_TrackingState) &&
                (m_NativePtr == other.m_NativePtr);
        }

        TrackableId m_TrackableId;

        TrackableId m_SubsumedById;

        Vector2 m_Center;

        Pose m_Pose;

        Vector2 m_Size;

        PlaneAlignment m_Alignment;

        TrackingState m_TrackingState;

        IntPtr m_NativePtr;
    }
}
