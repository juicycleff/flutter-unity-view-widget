using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARReferencePointManager.referencePointsChanged"/> event.
    /// </summary>
    public struct ARReferencePointsChangedEventArgs : IEquatable<ARReferencePointsChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARReferencePoint"/>s added since the last event.
        /// </summary>
        public List<ARReferencePoint> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARReferencePoint"/>s udpated since the last event.
        /// </summary>
        public List<ARReferencePoint> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARReferencePoint"/>s removed since the last event.
        /// At the time the event is invoked, the <see cref="ARReferencePoint"/>s in
        /// this list still exist. They are destroyed immediately afterward.
        /// </summary>
        public List<ARReferencePoint> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARReferencePointsChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARReferencePoint"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARReferencePoint"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARReferencePoint"/>s removed since the last event.</param>
        public ARReferencePointsChangedEventArgs(
            List<ARReferencePoint> added,
            List<ARReferencePoint> updated,
            List<ARReferencePoint> removed)
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
            if (!(obj is ARReferencePointsChangedEventArgs))
                return false;

            return Equals((ARReferencePointsChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        public bool Equals(ARReferencePointsChangedEventArgs other)
        {
            return
                (added == other.added) &&
                (updated == other.updated) &&
                (removed == other.removed);
        }

        public static bool operator ==(ARReferencePointsChangedEventArgs lhs, ARReferencePointsChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARReferencePointsChangedEventArgs lhs, ARReferencePointsChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
