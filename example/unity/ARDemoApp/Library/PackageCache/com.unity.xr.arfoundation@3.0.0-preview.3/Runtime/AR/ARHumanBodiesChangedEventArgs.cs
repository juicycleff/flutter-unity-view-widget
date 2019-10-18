using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Container for the changed <c>ARHumanBody</c> of the event.
    /// </summary>
    public struct ARHumanBodiesChangedEventArgs : IEquatable<ARHumanBodiesChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARHumanBody"/>s added since the last event.
        /// </summary>
        public List<ARHumanBody> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARHumanBody"/>s udpated since the last event.
        /// </summary>
        public List<ARHumanBody> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARHumanBody"/>s removed since the last event.
        /// </summary>
        public List<ARHumanBody> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARPlaneChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARHumanBody"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARHumanBody"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARHumanBody"/>s removed since the last event.</param>
        public ARHumanBodiesChangedEventArgs(
            List<ARHumanBody> added,
            List<ARHumanBody> updated,
            List<ARHumanBody> removed)
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

        public bool Equals(ARHumanBodiesChangedEventArgs other)
        {
            return
                ReferenceEquals(added, other.added) &&
                ReferenceEquals(updated, other.updated) &&
                ReferenceEquals(removed, other.removed);
        }

        public override bool Equals(object obj)
        {
            return ((obj is ARHumanBodiesChangedEventArgs) && Equals((ARHumanBodiesChangedEventArgs)obj));
        }

        public static bool operator ==(ARHumanBodiesChangedEventArgs lhs, ARHumanBodiesChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARHumanBodiesChangedEventArgs lhs, ARHumanBodiesChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }
    }
}
