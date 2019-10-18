using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes transformations that may be applied to a camera image.
    /// </summary>
    [Flags]
    public enum CameraImageTransformation
    {
        /// <summary>
        /// No transformations will be applied. The image will match the native format.
        /// </summary>
        None = 0,

        /// <summary>
        /// Mirror the image across the X axis. This reverses the order of the horizontal rows, flipping the image
        /// upside down.
        /// </summary>
        MirrorX = 1 << 0,

        /// <summary>
        /// Mirror the image across the Y axis. This reverses the pixels in each horizontal row such that the left part
        /// of the image will appear on the right and vice versa.
        /// </summary>
        MirrorY = 1 << 1
    }
}
