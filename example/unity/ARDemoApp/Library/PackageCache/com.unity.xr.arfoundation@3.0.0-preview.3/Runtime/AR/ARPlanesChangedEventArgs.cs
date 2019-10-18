using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARPlaneManager.planesChanged"/> event.
    /// </summary>
    public struct ARPlanesChangedEventArgs : IEquatable<ARPlanesChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARPlane"/>s added since the last event.
        /// </summary>
        public List<ARPlane> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARPlane"/>s udpated since the last event.
        /// </summary>
        public List<ARPlane> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARPlane"/>s removed since the last event.
        /// </summary>
        public List<ARPlane> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARPlaneChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARPlane"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARPlane"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARPlane"/>s removed since the last event.</param>
        public ARPlanesChangedEventArgs(
            List<ARPlane> added,
            List<ARPlane> updated,
            List<ARPlane> removed)
        {
            this.added = added;
            this.updated = updated;
            this.removed = removed;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 0;
                hash = hash * 486187739 + (added == null ? 0 : added.GetHashCode());
                hash = hash * 486187739 + (updated == null ? 0 : updated.GetHashCode());
                hash = hash * 486187739 + (removed == null ? 0 : removed.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARPlanesChangedEventArgs))
                return false;

            return Equals((ARPlanesChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        public bool Equals(ARPlanesChangedEventArgs other)
        {
            return
                (added == other.added) &&
                (updated == other.updated) &&
                (removed == other.removed);
        }

        public static bool operator ==(ARPlanesChangedEventArgs lhs, ARPlanesChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARPlanesChangedEventArgs lhs, ARPlanesChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
