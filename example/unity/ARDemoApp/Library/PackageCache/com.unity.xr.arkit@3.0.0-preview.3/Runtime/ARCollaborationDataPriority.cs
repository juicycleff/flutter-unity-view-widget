namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Defines the priority of <see cref="ARCollaborationData"/>.
    /// </summary>
    public enum ARCollaborationDataPriority
    {
        /// <summary>
        /// No priority is set.
        /// </summary>
        None,

        /// <summary>
        /// The data is important to the collaborative session and should be sent reliably, e.g., using TCP.
        /// </summary>
        Critical,

        /// <summary>
        /// The data is not important to collaborative session quality and may be sent unreliably, e.g., using UDP.
        /// </summary>
        Optional
    }
}
