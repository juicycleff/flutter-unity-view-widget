using System.ComponentModel;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the focus mode of the camera.
    /// </summary>
    public enum CameraFocusMode
    {
        /// <summary>
        /// The focus is fixed and does not change.
        /// </summary>
        [Description("Fixed")]
        Fixed = 0,

        /// <summary>
        /// The focus will change automatically.
        /// </summary>
        [Description("Auto")]
        Auto = 1,
    }
}
