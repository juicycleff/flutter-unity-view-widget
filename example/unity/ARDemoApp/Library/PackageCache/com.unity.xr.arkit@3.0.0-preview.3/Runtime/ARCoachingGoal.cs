namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Defines the goal for the [ARCoachingOverlayView](https://developer.apple.com/documentation/arkit/arcoachingoverlayview)
    /// See [ARCoachingGoal](https://developer.apple.com/documentation/arkit/arcoachinggoal) for details.
    /// </summary>
    public enum ARCoachingGoal
    {
        /// <summary>
        /// The app requires basic world tracking.
        /// </summary>
        Tracking,

        /// <summary>
        /// The app requires a horizontal plane.
        /// </summary>
        HorizontalPlane,

        /// <summary>
        /// The app requires a vertical plane.
        /// </summary>
        VerticalPlane,

        /// <summary>
        /// The app requires a plane of any type.
        /// </summary>
        AnyPlane
    }
}
