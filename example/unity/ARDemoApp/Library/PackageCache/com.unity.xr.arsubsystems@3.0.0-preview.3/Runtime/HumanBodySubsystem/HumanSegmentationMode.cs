using System.ComponentModel;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the human segmentation mode.
    /// </summary>
    public enum HumanSegmentationMode
    {
        /// <summary>
        /// The human segmentation is disabled and will not be generated.
        /// </summary>
        [Description("Disabled")]
        Disabled = 0,

        /// <summary>
        /// The human segmentation is enabled and will be generated at standard resolution.
        /// </summary>
        [Description("StandardResolution")]
        StandardResolution = 1,

        /// <summary>
        /// The human segmentation is enabled and will be generated at the half screen resolution.
        /// </summary>
        [Description("HalfScreenResolution")]
        HalfScreenResolution = 2,

        /// <summary>
        /// The human segmentation is enabled and will be generated at the full screen resolution.
        /// </summary>
        [Description("FullScreenResolution")]
        FullScreenResolution = 3,
    }

    /// <summary>
    /// Extension for the <see cref="HumanSegmentationMode"/>.
    /// </summary>
    public static class HumanSegmentationModeExtension
    {
        /// <summary>
        /// Determine whether the human segmentation mode is enabled.
        /// </summary>
        /// <param name="humanSegmentationMode">The human segmentation mode to check.</param>
        /// <returns>
        /// <c>true</c> if the human segmentation mode is enabled. Otherwise, <c>false</c>.
        /// </returns>
        public static bool Enabled(this HumanSegmentationMode humanSegmentationMode)
        {
            return humanSegmentationMode != HumanSegmentationMode.Disabled;
        }
    }
}
