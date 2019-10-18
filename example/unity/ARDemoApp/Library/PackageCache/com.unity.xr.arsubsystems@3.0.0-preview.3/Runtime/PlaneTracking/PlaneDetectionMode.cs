using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Used to configure the types of planes to detect.
    /// </summary>
    [Flags]
    public enum PlaneDetectionMode
    {
        /// <summary>
        /// Plane detection is disabled.
        /// </summary>
        None = 0,

        /// <summary>
        /// Plane detection will only detect horizontal planes.
        /// </summary>
        Horizontal = 1 << 0,

        /// <summary>
        /// Plane detection will only detect vertical planes.
        /// </summary>
        Vertical = 1 << 1
    }
}
