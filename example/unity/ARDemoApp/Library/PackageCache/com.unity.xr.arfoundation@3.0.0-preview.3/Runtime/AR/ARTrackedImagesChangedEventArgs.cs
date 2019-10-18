using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARTrackedImageManager.trackedImagesChanged"/> event.
    /// </summary>
    public struct ARTrackedImagesChangedEventArgs : IEquatable<ARTrackedImagesChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARTrackedImage"/>s added since the last event.
        /// </summary>
        public List<ARTrackedImage> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARTrackedImage"/>s udpated since the last event.
        /// </summary>
        public List<ARTrackedImage> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARTrackedImage"/>s removed since the last event.
        /// </summary>
        public List<ARTrackedImage> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARTrackedImagesChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARTrackedImage"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARTrackedImage"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARTrackedImage"/>s removed since the last event.</param>
        public ARTrackedImagesChangedEventArgs(
            List<ARTrackedImage> added,
            List<ARTrackedImage> updated,
            List<ARTrackedImage> removed)
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
            if (!(obj is ARTrackedImagesChangedEventArgs))
                return false;

            return Equals((ARTrackedImagesChangedEventArgs)obj);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        public bool Equals(ARTrackedImagesChangedEventArgs other)
        {
            return
                (added == other.added) &&
                (updated == other.updated) &&
                (removed == other.removed);
        }

        public static bool operator ==(ARTrackedImagesChangedEventArgs lhs, ARTrackedImagesChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARTrackedImagesChangedEventArgs lhs, ARTrackedImagesChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
