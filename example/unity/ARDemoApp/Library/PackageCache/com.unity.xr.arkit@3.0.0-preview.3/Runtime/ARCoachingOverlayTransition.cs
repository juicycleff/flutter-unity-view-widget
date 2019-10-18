namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The type of transition used when showing or hiding the [ARCoachingOverlay](https://developer.apple.com/documentation/arkit/arcoachingoverlayview)
    /// </summary>
    /// <seealso cref="ARKitSessionSubsystem.SetCoachingActive(bool, ARCoachingOverlayTransition)"/>
    public enum ARCoachingOverlayTransition
    {
        /// <summary>
        /// The coaching overlay is shown instantly, with no transition.
        /// </summary>
        Instant,

        /// <summary>
        /// The coaching overlay should be animated when being shown or hidden.
        /// </summary>
        Animated
    }
}
