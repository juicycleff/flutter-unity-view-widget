namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Used to determine the suitability of the current session for
    /// creating an <see cref="ARWorldMap"/>. See
    /// <a href="https://developer.apple.com/documentation/arkit/arworldmappingstatus">Apple's documentation for ARWorldMappingStatus</a>
    /// for more information.
    /// </summary>
    public enum ARWorldMappingStatus
    {
        /// <summary>
        /// Mapping is not available
        /// </summary>
        NotAvailable = 0,

        /// <summary>
        /// Mapping is available but has limited features.
        /// For the device's current position, it is not recommended to serialize the current session.
        /// </summary>
        Limited,

        /// <summary>
        /// Mapping is actively extending the map with the user's motion.
        /// The session will be relocalizable for previously visited areas but is still being updated for the current space.
        /// </summary>
        Extending,

        /// <summary>
        /// The session has adequately mapped the visible area.
        /// The map can be used to relocalize for the device's current position.
        /// </summary>
        Mapped
    }
}
