using System;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Data associated with an <see cref="ARPlane.boundaryChanged" /> event.
    /// </summary>
    public struct ARPlaneBoundaryChangedEventArgs : IEquatable<ARPlaneBoundaryChangedEventArgs>
    {
        /// <summary>
        /// The <see cref="ARPlane" /> which triggered the event.
        /// </summary>
        public ARPlane plane { get; private set; }

        /// <summary>
        /// Constructor for plane changed events.
        /// This is normally only used by the <see cref="ARPlane"/> component for <see cref="ARPlane.boundaryChanged"/> events.
        /// </summary>
        /// <param name="plane">The <see cref="ARPlane"/> that triggered the event.</param>
        public ARPlaneBoundaryChangedEventArgs(ARPlane plane)
        {
            if (plane == null)
                throw new ArgumentNullException("plane");

            this.plane = plane;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = plane == null ? 0 : plane.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARPlaneBoundaryChangedEventArgs))
                return false;

            return Equals((ARPlaneBoundaryChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return plane.trackableId.ToString();
        }

        public bool Equals(ARPlaneBoundaryChangedEventArgs other)
        {
            return (plane == other.plane);
        }

        public static bool operator ==(ARPlaneBoundaryChangedEventArgs lhs, ARPlaneBoundaryChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARPlaneBoundaryChangedEventArgs lhs, ARPlaneBoundaryChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
