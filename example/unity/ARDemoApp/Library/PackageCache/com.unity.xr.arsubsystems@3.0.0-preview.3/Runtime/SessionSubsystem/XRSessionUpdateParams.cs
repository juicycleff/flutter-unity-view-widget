using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Update parameters for <see cref="XRSessionSubsystem.Update(XRSessionUpdateParams)"/>.
    /// </summary>
    public struct XRSessionUpdateParams : IEquatable<XRSessionUpdateParams>
    {
        /// <summary>
        /// The current screen orientation
        /// </summary>
        public ScreenOrientation screenOrientation { get; set; }

        /// <summary>
        /// The current screen dimensions.
        /// </summary>
        public Vector2Int screenDimensions { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = ((int)screenOrientation).GetHashCode();
                hash = hash * 486187739 + screenDimensions.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is XRSessionUpdateParams))
                return false;

            return Equals((XRSessionUpdateParams)obj);
        }

        public override string ToString()
        {
            return string.Format("Screen Orientation: {0}, Screen Dimensions: {1}",
                screenOrientation, screenDimensions);
        }

        public bool Equals(XRSessionUpdateParams other)
        {
            return
                (screenOrientation == other.screenOrientation) &&
                (screenDimensions.Equals(other.screenDimensions));
        }

        public static bool operator ==(XRSessionUpdateParams lhs, XRSessionUpdateParams rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRSessionUpdateParams lhs, XRSessionUpdateParams rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
