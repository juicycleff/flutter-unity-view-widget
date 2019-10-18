namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents the current state of the AR system.
    /// </summary>
    public enum ARSessionState
    {
        /// <summary>
        /// The AR system has not been initialized. Availability is unknown.
        /// <see cref="ARSubsystemManager.CheckAvailability"/>.
        /// </summary>
        None,

        /// <summary>
        /// AR is not supported on the current device.
        /// </summary>
        Unsupported,

        /// <summary>
        /// The system is checking for the availability of AR.
        /// <see cref="ARSubsystemManager.CheckAvailability"/>.
        /// </summary>
        CheckingAvailability,

        /// <summary>
        /// AR is supported, but requires an additional install.
        /// <see cref="ARSubsystemManager.Install"/>.
        /// </summary>
        NeedsInstall,

        /// <summary>
        /// AR software is being installed. <see cref="ARSubsystemManager.Install"/>.
        /// </summary>
        Installing,

        /// <summary>
        /// AR is supported and ready.
        /// </summary>
        Ready,

        /// <summary>
        /// An AR session is initializing (i.e., starting up). This usually means AR is working
        /// but has not yet gathered enough information about the environment.
        /// </summary>
        SessionInitializing,

        /// <summary>
        /// An AR session is running and is tracking (i.e., the device is able to determine its
        /// position and orientation in the world).
        /// </summary>
        SessionTracking
    }
}
