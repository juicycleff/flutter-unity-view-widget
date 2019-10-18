namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Used to refer to a face "region" in ARCore. A face region is a position
    /// and rotation on a face that can be queried.
    /// </summary>
    public enum ARCoreFaceRegion
    {
        /// <summary>
        /// The tip of the nose.
        /// </summary>
        NoseTip = 0,

        /// <summary>
        /// The left side of the forehead.
        /// </summary>
        ForeheadLeft = 1,

        /// <summary>
        /// The right side of the forehead.
        /// </summary>
        ForeheadRight = 2,
    }
}
