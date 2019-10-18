using System;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// The arguments for the <see cref="ARPointCloud.updated"/>
    /// event. This is currently empty, but may change in the future without the need to change the
    /// subscribers' method signatures.
    /// </summary>
    public struct ARPointCloudUpdatedEventArgs : IEquatable<ARPointCloudUpdatedEventArgs>
    {
        /// <summary>
        /// Generates a hash code suitable for use in a <c>Dictionary</c> or <c>HashSet</c>.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARPointCloudUpdatedEventArgs))
                return false;

            return Equals((ARPointCloudUpdatedEventArgs)obj);
        }

        /// <summary>
        /// Interface for <c>IEquatable</c>
        /// </summary>
        public bool Equals(ARPointCloudUpdatedEventArgs other)
        {
            return true;
        }

        public static bool operator ==(ARPointCloudUpdatedEventArgs lhs, ARPointCloudUpdatedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARPointCloudUpdatedEventArgs lhs, ARPointCloudUpdatedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}