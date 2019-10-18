using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the intersection of a raycast with a trackable.
    /// </summary>
    /// <seealso cref="XRRaycastSubsystem.Raycast(Ray, TrackableType, Unity.Collections.Allocator)"/>
    /// <seealso cref="XRRaycastSubsystem.Raycast(Vector2, TrackableType, Unity.Collections.Allocator)"/>
    /// <seealso cref="XRRaycastSubsystem.RaycastAsync(Ray, TrackableType)"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRRaycastHit : IEquatable<XRRaycastHit>
    {
        static readonly XRRaycastHit s_Default = new XRRaycastHit(
            TrackableId.invalidId, Pose.identity, 0, TrackableType.None);

        /// <summary>
        /// A default-initialized raycast hit.
        /// This may be different from a zero-initialized raycast hit.
        /// </summary>
        public static XRRaycastHit defaultValue => s_Default;

        /// <summary>
        /// The <see cref="TrackableId"/> of the trackable which was hit. This may be <see cref="TrackableId.invalidId"/>
        /// as not all trackables have ids, e.g., feature points.
        /// </summary>
        public TrackableId trackableId
        {
            get { return m_TrackableId; }
            set { m_TrackableId = value; }
        }

        /// <summary>
        /// The session-space <c>Pose</c> of the intersection.
        /// </summary>
        public Pose pose
        {
            get { return m_Pose; }
            set { m_Pose = value; }
        }

        /// <summary>
        /// The session-space distance from the raycast origin to the intersection point.
        /// </summary>
        public float distance
        {
            get { return m_Distance; }
            set { m_Distance = value; }
        }

        /// <summary>
        /// The type(s) of trackables which were hit by the ray.
        /// </summary>
        public TrackableType hitType
        {
            get { return m_HitType; }
            set { m_HitType = value; }
        }

        /// <summary>
        /// Constructs an <see cref="XRRaycastHit"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> of the trackable which was hit.</param>
        /// <param name="pose">The session-space <c>Pose</c> of the intersection.</param>
        /// <param name="distance">The session-space distance from the raycast origin to the intersection point.</param>
        /// <param name="hitType">The type(s) of trackables which were hit by the ray.</param>
        public XRRaycastHit(
            TrackableId trackableId,
            Pose pose,
            float distance,
            TrackableType hitType)
        {
            m_TrackableId = trackableId;
            m_Pose = pose;
            m_Distance = distance;
            m_HitType = hitType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_TrackableId.GetHashCode();
                hash = hash * 486187739 + m_Pose.GetHashCode();
                hash = hash * 486187739 + m_Distance.GetHashCode();
                hash = hash * 486187739 + ((int)m_HitType).GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is XRRaycastHit))
                return false;

            return Equals((XRRaycastHit)obj);
        }

        public bool Equals(XRRaycastHit other)
        {
            return
                (m_TrackableId.Equals(other.m_TrackableId)) &&
                (m_Pose.Equals(other.m_Pose)) &&
                (m_Distance.Equals(other.m_Distance)) &&
                (m_HitType == other.m_HitType);
        }

        public static bool operator ==(XRRaycastHit lhs, XRRaycastHit rhs) { return lhs.Equals(rhs); }

        public static bool operator !=(XRRaycastHit lhs, XRRaycastHit rhs) { return !lhs.Equals(rhs); }

        TrackableId m_TrackableId;

        Pose m_Pose;

        float m_Distance;

        TrackableType m_HitType;
    }
}
