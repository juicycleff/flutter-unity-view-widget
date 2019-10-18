using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Flags used to determine session availability.
    /// </summary>
    [Flags]
    public enum SessionAvailability
    {
        /// <summary>
        /// Default value. The availability is unknown.
        /// </summary>
        None = 0,

        /// <summary>
        /// The current device is AR capable (but may require a software update).
        /// </summary>
        Supported = 1 << 1,

        /// <summary>
        /// The required AR software is installed on the device.
        /// </summary>
        Installed = 1 << 2
    }

    /// <summary>
    /// Extensions to the <see cref="SessionAvailability"/> and <see cref="SessionInstallationStatus"/> enums.
    /// </summary>
    public static class SessionAvailabilityExtensions
    {
        /// <summary>
        /// A helper method for <see cref="SessionAvailability"/> flags.
        /// </summary>
        /// <param name="availability">A <see cref="SessionAvailability"/> enum</param>
        /// <returns>True if the <see cref="SessionAvailability.Supported"/> flag is set.</returns>
        public static bool IsSupported(this SessionAvailability availability)
        {
            return (availability & SessionAvailability.Supported) != SessionAvailability.None;
        }

        /// <summary>
        /// A helper method for <see cref="SessionAvailability"/> flags.
        /// </summary>
        /// <param name="availability">A <see cref="SessionAvailability"/> enum</param>
        /// <returns>True if the <see cref="SessionAvailability.Installed"/> flag is set.</returns>
        public static bool IsInstalled(this SessionAvailability availability)
        {
            return (availability & SessionAvailability.Installed) != SessionAvailability.None;
        }
    }
}
