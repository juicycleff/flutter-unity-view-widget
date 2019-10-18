using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Event arguments for the <see cref="ARMeshManager.meshesChanged"/> event.
    /// </summary>
    public struct ARMeshesChangedEventArgs : IEquatable<ARMeshesChangedEventArgs>
    {
        /// <summary>
        /// The list of <c>MeshFilter</c>s added since the last event.
        /// </summary>
        public List<MeshFilter> added { get; private set; }

        /// <summary>
        /// The list of <c>MeshFilter</c>s udpated since the last event.
        /// </summary>
        public List<MeshFilter> updated { get; private set; }

        /// <summary>
        /// The list of <c>MeshFilter</c>s removed since the last event.
        /// </summary>
        public List<MeshFilter> removed { get; private set; }

        /// <summary>
        /// Constructs an <see cref="ARMeshesChangedEventArgs"/>.
        /// </summary>
        /// <param name="added">The list of <c>MeshFilter</c>s added since the last event.</param>
        /// <param name="updated">The list of <c>MeshFilter</c>s updated since the last event.</param>
        /// <param name="removed">The list of <c>MeshFilter</c>s removed since the last event.</param>
        public ARMeshesChangedEventArgs(
            List<MeshFilter> added,
            List<MeshFilter> updated,
            List<MeshFilter> removed)
        {
            this.added = added;
            this.updated = updated;
            this.removed = removed;
        }

        /// <summary>
        /// Generates a hash code suitable for use in a <c>Dictionary</c> or <c>HashSet</c>.
        /// </summary>
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
        /// <c>IEquatable</c> interface.
        /// </summary>
        /// <param name="obj">The object to compare for equality.</param>
        /// <returns><c>True</c> if <paramref name="obj"/> is of type <see cref="ARMeshesChangedEventArgs"/>
        /// and compares equal using <see cref="Equals(ARMeshesChangedEventArgs)"/>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ARMeshesChangedEventArgs))
                return false;

            return Equals((ARMeshesChangedEventArgs)obj);
        }

        /// <summary>
        /// Generates a string representation of this struct, including the number of
        /// added, updated, and removed meshes.
        /// </summary>
        /// <returns>A string representation of this struct.</returns>
        public override string ToString()
        {
            return string.Format("Added: {0}, Updated: {1}, Removed: {2}",
                added == null ? 0 : added.Count,
                updated == null ? 0 : updated.Count,
                removed == null ? 0 : removed.Count);

        }

        /// <summary>
        /// Compares <paramref name="other"/> for equality.
        /// </summary>
        /// <param name="other">The <see cref="ARMeshesChangedEventArgs"/> to compare for equality.</param>
        /// <returns><c>True</c> if <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/>
        /// have the same <c>List</c> references as the corresponding properties of <paramref name="other"/>.</returns>
        public bool Equals(ARMeshesChangedEventArgs other)
        {
            return
                ReferenceEquals(added, other.added) &&
                ReferenceEquals(updated, other.updated) &&
                ReferenceEquals(removed, other.removed);
        }

        /// <summary>
        /// Compares for equality. Same as <see cref="Equals(ARMeshesChangedEventArgs)"/>.
        /// </summary>
        /// <param name="lhs">The first <see cref="ARMeshesChangedEventArgs"/> to compare.</param>
        /// <param name="rhs">The second <see cref="ARMeshesChangedEventArgs"/> to compare.</param>
        /// <returns>The same value as <see cref="Equals(ARMeshesChangedEventArgs)"/></returns>
        public static bool operator ==(ARMeshesChangedEventArgs lhs, ARMeshesChangedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Compares for inequality. Same as <c>!</c><see cref="Equals(ARMeshesChangedEventArgs)"/>.
        /// </summary>
        /// <param name="lhs">The first <see cref="ARMeshesChangedEventArgs"/> to compare.</param>
        /// <param name="rhs">The second <see cref="ARMeshesChangedEventArgs"/> to compare.</param>
        /// <returns>The same value as <c>!</c><see cref="Equals(ARMeshesChangedEventArgs)"/></returns>
        public static bool operator !=(ARMeshesChangedEventArgs lhs, ARMeshesChangedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
