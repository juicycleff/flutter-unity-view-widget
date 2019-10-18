using System.ComponentModel;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents the method used to place an environment probe in the AR session.
    /// </summary>
    public enum AREnvironmentProbePlacementType
    {
        /// <summary>
        /// The method of placement is unknown.
        /// </summary>
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// The environment probe is placed through a manual call into the subsystem.
        /// </summary>
        [Description("Manual")]
        Manual = 1,

        /// <summary>
        /// The environment probe is placed through an internal, automated method implemented by the provider.
        /// </summary>
        [Description("Automatic")]
        Automatic = 2
    }
}
