using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    // Removing from summary of the configuration class because this has yet to be ported.
    // The camera image configuration affects the resolution of the image
    // returned by <see cref="XRCameraSubsystem.TryGetLatestImage(Experimental.XR.XRCameraSubsystem, out CameraImage)"/>.


    /// <summary>
    /// Contains information regarding the camera configuration. Different
    /// devices support different camera configurations. This includes
    /// the resolution of the image and may include framerate on some platforms.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRCameraConfiguration : IEquatable<XRCameraConfiguration>
    {
        Vector2Int m_Resolution;

        int m_Framerate;

        /// <summary>
        /// The width of the camera resolution
        /// </summary>
        /// <value>
        /// The width, in pixels, of the camera resolution
        /// </value>
        public int width { get { return m_Resolution.x; } }

        /// <summary>
        /// The height of the camera resolution
        /// </summary>
        /// <value>
        /// The height, in pixels, of the camera resolution
        /// </value>
        public int height { get { return m_Resolution.y; } }

        /// <summary>
        /// The camera resolution.
        /// </summary>
        /// <value>
        /// The camera resolution in pixels.
        /// </value>
        public Vector2Int resolution { get { return m_Resolution; } }

        /// <summary>
        /// The framerate, if this camera configuration specifies one.
        /// </summary>
        /// <value>
        /// The framerate, if this camera configuration specifies one. Otherwise, <c>null</c>.
        /// </value>
        /// <remarks>
        /// On some platforms, different resolutions may affect the available framerate.
        /// </remarks>
        public int? framerate
        {
            get
            {
                return (m_Framerate > 0) ? new int?(m_Framerate) : new int?();
            }
        }

        /// <summary>
        /// Constructs a camera configuration with a framerate.
        /// </summary>
        /// <param name="resolution">The resolution of the camera image.</param>
        /// <param name="framerate">The camera framerate. Throws <c>ArgumentOutOfRangeException</c>
        /// if <paramref name="framerate"/> is less than or equal to zero.</param>
        internal XRCameraConfiguration(Vector2Int resolution, int framerate)
        {
            if (framerate <= 0)
                throw new ArgumentOutOfRangeException(
                    string.Format("{0} is not a valid framerate; it must be greater than zero.", framerate));

            m_Resolution = resolution;
            m_Framerate = framerate;
        }

        /// <summary>
        /// Constructs a camera configuration without a framerate.
        /// </summary>
        /// <param name="resolution">The resolution of the camera image.</param>
        internal XRCameraConfiguration(Vector2Int resolution)
        {
            m_Resolution = resolution;
            m_Framerate = 0;
        }

        /// <summary>
        /// Converts the configuration to a string, suitable for debug logging.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (framerate.HasValue)
                return string.Format("{0}x{1} at {2} Hz", width, height, framerate.Value);
            else
                return string.Format("{0}x{1}", width, height);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_Resolution.GetHashCode();
                return hash * 486187739 + framerate.GetHashCode();
            }
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is XRCameraConfiguration) && Equals((XRCameraConfiguration)obj));
        }

        public bool Equals(XRCameraConfiguration other)
        {
            return (m_Resolution == other.m_Resolution) && (framerate == other.framerate);
        }

        public static bool operator ==(XRCameraConfiguration lhs, XRCameraConfiguration rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRCameraConfiguration lhs, XRCameraConfiguration rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
