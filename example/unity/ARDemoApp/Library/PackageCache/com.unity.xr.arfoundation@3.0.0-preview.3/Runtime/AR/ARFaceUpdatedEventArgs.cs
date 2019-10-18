using System;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Holds data relevant to the <see cref="ARFace.updated"/> event.
    /// </summary>
    public struct ARFaceUpdatedEventArgs : IEquatable<ARFaceUpdatedEventArgs>
    {
        /// <summary>
        /// The <see cref="ARFace"/> component that was updated.
        /// </summary>
        public ARFace face { get; private set; }

        /// <summary>
        /// Constructor invoked by the <see cref="ARFaceManager"/> which triggered the event.
        /// </summary>
        /// <param name="face">The <see cref="ARFace"/> component that was updated.</param>
        public ARFaceUpdatedEventArgs(ARFace face)
        {
            if (face == null)
                throw new ArgumentNullException("face");

            this.face = face;
        }

        public override int GetHashCode()
        {
            return (face == null) ? 0 : face.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARFaceUpdatedEventArgs))
                return false;

            return Equals((ARFaceUpdatedEventArgs)obj);
        }

        public bool Equals(ARFaceUpdatedEventArgs other)
        {
            return face == other.face;
        }

        public static bool operator==(ARFaceUpdatedEventArgs lhs, ARFaceUpdatedEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(ARFaceUpdatedEventArgs lhs, ARFaceUpdatedEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
