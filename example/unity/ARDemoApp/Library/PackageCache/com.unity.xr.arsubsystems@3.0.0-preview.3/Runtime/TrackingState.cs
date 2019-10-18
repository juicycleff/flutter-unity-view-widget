namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents pose tracking quality.
    /// May apply to a device or trackables it is tracking in the environment.
    /// </summary>
    public enum TrackingState
    {
        /// <summary>
        /// Not tracking.
        /// </summary>
        None,

        /// <summary>
        /// Some tracking information is available, but it is limited or of poor quality.
        /// </summary>
        Limited,

        /// <summary>
        /// Tracking is working normally.
        /// </summary>
        Tracking,
    }
}
