using System;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Information about the camera image planes. An image "plane" refers to an image channel used in video encoding.
    /// </summary>
    public struct XRCameraImagePlane : IEquatable<XRCameraImagePlane>
    {
        /// <summary>
        /// The number of bytes per row for this plane.
        /// </summary>
        /// <value>
        /// The number of bytes per row for this plane.
        /// </value>
        public int rowStride { get; internal set; }

        /// <summary>
        /// The number of bytes per pixel for this plane.
        /// </summary>
        /// <value>
        /// The number of bytes per pixel for this plane.
        /// </value>
        public int pixelStride { get; internal set; }

        /// <summary>
        /// A "view" into the platform-specific plane data. It is an error to access <c>data</c> after the owning
        /// <see cref="XRCameraImage"/> has been disposed.
        /// </summary>
        /// <value>
        /// The platform-specific plane data.
        /// </value>
        public NativeArray<byte> data { get; internal set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = data.GetHashCode();
                hash = hash * 486187739 + rowStride.GetHashCode();
                hash = hash * 486187739 + pixelStride.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return ((obj is XRCameraImagePlane) && Equals((XRCameraImagePlane)obj));
        }

        public bool Equals(XRCameraImagePlane other)
        {
            return
                (data.Equals(other.data)) &&
                (rowStride == other.rowStride) &&
                (pixelStride == other.pixelStride);
        }

        public static bool operator ==(XRCameraImagePlane lhs, XRCameraImagePlane rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRCameraImagePlane lhs, XRCameraImagePlane rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return string.Format("(Data: {0}, Row Stride: {1}, Pixel Stride: {2})",
                data.ToString(), rowStride, pixelStride);
        }
    }
}
