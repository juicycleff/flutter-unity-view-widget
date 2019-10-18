using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARTrackedObjectManager.trackedObjectsChanged"/> event.
    /// </summary>
    public struct ARTrackedObjectsChangedEventArgs : IEquatable<ARTrackedObjectsChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARTrackedObject"/>s added since the last event.
        /// </summary>
        public List<ARTrackedObject> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARTrackedObject"/>s udpated since the last event.
        /// </summary>
        public List<ARTrackedObject> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARTrackedObject"/>s removed since the last event.
        /// </summary>
        public List<ARTrackedObject> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARTrackedObjectsChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARTrackedObject"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARTrackedObject"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARTrackedObject"/>s removed since the last event.</param>
        public ARTrackedObjectsChangedEventArgs(
            List<ARTrackedObject> added,
            List<ARTrackedObject> updated,
            List<ARTrackedObject> removed)
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
            if (!(obj is ARTrackedObjectsChangedEventArgs))
                return false;

            return Equals((ARTrackedObjectsChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        public bool Equals(ARTrackedObjectsChangedEventArgs other)
        {
            return
                (added == other.added) &&
                (updated == other.updated) &&
                (removed == other.removed);
        }

        public static bool operator ==(ARTrackedObjectsChangedEventArgs lhs, ARTrackedObjectsChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARTrackedObjectsChangedEventArgs lhs, ARTrackedObjectsChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
