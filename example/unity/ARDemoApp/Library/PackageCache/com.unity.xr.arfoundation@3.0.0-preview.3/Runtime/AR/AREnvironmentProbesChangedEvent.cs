using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="AREnvironmentProbeManager.environmentProbesChanged"/> event.
    /// </summary>
    public struct AREnvironmentProbesChangedEvent : IEquatable<AREnvironmentProbesChangedEvent>
    {
        /// <summary>
        /// The list of <see cref="AREnvironmentProbe"/>s added since the last event.
        /// </summary>
        public List<AREnvironmentProbe> added { get; private set; }

        /// <summary>
        /// The list of <see cref="AREnvironmentProbe"/>s udpated since the last event.
        /// </summary>
        public List<AREnvironmentProbe> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="AREnvironmentProbe"/>s removed since the last event.
        /// </summary>
        public List<AREnvironmentProbe> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARPlaneChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="AREnvironmentProbe"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="AREnvironmentProbe"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="AREnvironmentProbe"/>s removed since the last event.</param>
        public AREnvironmentProbesChangedEvent(
            List<AREnvironmentProbe> added,
            List<AREnvironmentProbe> updated,
            List<AREnvironmentProbe> removed)
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
            if (!(obj is AREnvironmentProbesChangedEvent))
                return false;

            return Equals((AREnvironmentProbesChangedEvent)obj);
        }

        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        public bool Equals(AREnvironmentProbesChangedEvent other)
        {
            return
                ReferenceEquals(added, other.added) &&
                ReferenceEquals(updated, other.updated) &&
                ReferenceEquals(removed, other.removed);
        }

        public static bool operator ==(AREnvironmentProbesChangedEvent lhs, AREnvironmentProbesChangedEvent rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(AREnvironmentProbesChangedEvent lhs, AREnvironmentProbesChangedEvent rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
