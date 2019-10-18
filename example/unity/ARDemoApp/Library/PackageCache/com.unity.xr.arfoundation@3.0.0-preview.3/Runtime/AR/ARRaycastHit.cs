using System;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents the result of a raycast intersection with a trackable.
    /// </summary>
    public struct ARRaycastHit : IEquatable<ARRaycastHit>, IComparable<ARRaycastHit>
    {
        /// <summary>
        /// Constructor invoked by the <see cref="ARSessionOrigin.Raycast"/> methods.
        /// </summary>
        /// <param name="hit">Session-relative raycast hit data.</param>
        /// <param name="distance">The distance, in Unity world space, of the hit.</param>
        /// <param name="transform">The <c>Transform</c> that transforms from session space to world space.</param>
        public ARRaycastHit(XRRaycastHit hit, float distance, Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            m_Hit = hit;
            this.distance = distance;
            m_Transform = transform;
        }

        /// <summary>
        /// The distance, in Unity world space, from the ray origin to the intersection point.
        /// </summary>
        public float distance { get; private set; }

        /// <summary>
        /// The type of trackable hit by the raycast.
        /// </summary>
        public TrackableType hitType
        {
            get { return m_Hit.hitType; }
        }

        /// <summary>
        /// The <c>Pose</c>, in Unity world space, of the intersection point.
        /// </summary>
        public Pose pose
        {
            get { return m_Transform.TransformPose(sessionRelativePose); }
        }

        /// <summary>
        /// The session-unique identifier for the trackable that was hit.
        /// </summary>
        public TrackableId trackableId
        {
            get { return m_Hit.trackableId; }
        }

        /// <summary>
        /// The <c>Pose</c>, in local (session) space, of the intersection point.
        /// </summary>
        public Pose sessionRelativePose
        {
            get { return m_Hit.pose; }
        }

        /// <summary>
        /// The distance, in local (session) space, from the ray origin to the intersection point.
        /// </summary>
        public float sessionRelativeDistance
        {
            get { return m_Hit.distance; }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_Hit.GetHashCode();
                hash = hash * 486187739 + distance.GetHashCode();
                hash = hash * 486187739 + (m_Transform == null ? 0 : m_Transform.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARRaycastHit))
                return false;

            return Equals((ARRaycastHit)obj);
        }

        public bool Equals(ARRaycastHit other)
        {
            return
                (m_Hit.Equals(other.m_Hit)) &&
                (distance.Equals(other.distance)) &&
                (m_Transform.Equals(other.m_Transform));
        }

        public static bool operator ==(ARRaycastHit lhs, ARRaycastHit rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARRaycastHit lhs, ARRaycastHit rhs)
        {
            return !lhs.Equals(rhs);
        }

        public int CompareTo(ARRaycastHit other)
        {
            return m_Hit.distance.CompareTo(other.m_Hit.distance);
        }

        XRRaycastHit m_Hit;

        Transform m_Transform;
    }
}
