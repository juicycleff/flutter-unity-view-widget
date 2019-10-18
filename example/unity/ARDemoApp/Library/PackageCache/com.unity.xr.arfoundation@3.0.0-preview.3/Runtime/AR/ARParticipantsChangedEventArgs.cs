using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARParticipantManager.participantsChanged"/> event.
    /// </summary>
    public struct ARParticipantsChangedEventArgs : IEquatable<ARParticipantsChangedEventArgs>
    {
        /// <summary>
        /// The list of <see cref="ARParticipant"/>s added since the last event.
        /// </summary>
        public List<ARParticipant> added { get; private set; }

        /// <summary>
        /// The list of <see cref="ARParticipant"/>s udpated since the last event.
        /// </summary>
        public List<ARParticipant> updated { get; private set; }

        /// <summary>
        /// The list of <see cref="ARParticipant"/>s removed since the last event.
        /// </summary>
        public List<ARParticipant> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARParticipantsChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <see cref="ARParticipant"/>s added since the last event.</param>
        /// <param name="updated">The list of <see cref="ARParticipant"/>s updated since the last event.</param>
        /// <param name="removed">The list of <see cref="ARParticipant"/>s removed since the last event.</param>
        public ARParticipantsChangedEventArgs(
            List<ARParticipant> added,
            List<ARParticipant> updated,
            List<ARParticipant> removed)
        {
            this.added = added;
            this.updated = updated;
            this.removed = removed;
        }

        /// <summary>
        /// Generates a hash suitable for use with containers like <c>HashSet</c> and <c>Dictionary</c>.
        /// </summary>
        /// <returns>A hash suitable for use with containers like <c>HashSet</c> and <c>Dictionary</c>.</returns>
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

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="other">The other <see cref="ARParticipantsChangedEventArgs"/> to compare against.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this <see cref="ARParticipantsChangedEventArgs"/>.</returns>
        public override bool Equals(object obj) => (obj is ARParticipantsChangedEventArgs) && Equals((ARParticipantsChangedEventArgs)obj);

        int GetCount(List<ARParticipant> list) => list != null ? list.Count : 0;

        /// <summary>
        /// Generates a string suitable for debugging. The string contains the number of added, updated, and removed participants.
        /// </summary>
        /// <returns>A string suitable for debug logging.</returns>
        public override string ToString() => $"Added: {GetCount(added)}, Updated: {GetCount(updated)}, Removed: {GetCount(removed)}";

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="other">The other <see cref="ARParticipantsChangedEventArgs"/> to compare against.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this <see cref="ARParticipantsChangedEventArgs"/>.</returns>
        public bool Equals(ARParticipantsChangedEventArgs other)
        {
            return
                ReferenceEquals(added, other.added) &&
                ReferenceEquals(updated, other.updated) &&
                ReferenceEquals(removed, other.removed);
        }
        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(ARParticipantsChangedEventArgs)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is equal to <paramref name="rhs"/>.</returns>
        public static bool operator ==(ARParticipantsChangedEventArgs lhs, ARParticipantsChangedEventArgs rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as <c>!</c><see cref="Equals(ARParticipantsChangedEventArgs)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><c>true</c> if <paramref name="lhs"/> is not equal to <paramref name="rhs"/>.</returns>
        public static bool operator !=(ARParticipantsChangedEventArgs lhs, ARParticipantsChangedEventArgs rhs) => !lhs.Equals(rhs);
    }
}
