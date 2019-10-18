namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// An enum used to determine that current status of the software install.
    /// </summary>
    /// <remarks>
    /// Some devices may support AR but require a software update. Some platforms allow
    /// prompting a user to install the required software. This enum is used to indicate
    /// the result of an installation request.
    /// </remarks>
    public enum SessionInstallationStatus
    {
        /// <summary>
        /// Default value. The installation status is not known.
        /// </summary>
        None = 0,

        /// <summary>
        /// The installation was successful.
        /// </summary>
        Success,

        /// <summary>
        /// The installation was not successful because the user declined the installation.
        /// </summary>
        ErrorUserDeclined,

        /// <summary>
        /// The installation was not successful because the device is not compatible.
        /// </summary>
        ErrorDeviceNotCompatible,

        /// <summary>
        /// The installation was not successful because installation is not supported.
        /// </summary>
        ErrorInstallNotSupported,

        /// <summary>
        /// An unknown error occurred during installation.
        /// </summary>
        Error
    }
}
