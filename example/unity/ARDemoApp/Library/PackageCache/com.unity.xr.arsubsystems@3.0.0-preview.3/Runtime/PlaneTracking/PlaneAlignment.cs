namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the alignment of a plane, e.g., whether it is horizontal or vertical.
    /// </summary>
    /// <seealso cref="BoundedPlane.alignment"/>
    public enum PlaneAlignment
    {
        /// <summary>
        /// No alignment
        /// </summary>
        None = 0,

        /// <summary>
        /// The plane is horizontal with an upward facing normal, e.g., a floor.
        /// </summary>
        HorizontalUp = 100,

        /// <summary>
        /// The plane is horizontal with a downward facing normal, e.g., a ceiling.
        /// </summary>
        HorizontalDown = 101,

        /// <summary>
        /// The plane is vertical, e.g., a wall.
        /// </summary>
        Vertical = 200,

        /// <summary>
        /// The plane is not axis aligned.
        /// </summary>
        NotAxisAligned = 300
    }

    /// <summary>
    /// Extension methods for the <see cref="PlaneAlignment"/> enum.
    /// </summary>
    public static class PlaneAlignmentExtensions
    {
        /// <summary>
        /// Determines whether the plane is horizontal (whether facing up or down).
        /// </summary>
        /// <param name="alignment">The <see cref="PlaneAlignment"/> being extended.</param>
        /// <returns><c>true</c> if the plane is horizontal.</returns>
        public static bool IsHorizontal(this PlaneAlignment alignment)
        {
            return
                (alignment == PlaneAlignment.HorizontalUp) ||
                (alignment == PlaneAlignment.HorizontalDown);
        }

        /// <summary>
        /// Determines whether the plane is vertical.
        /// </summary>
        /// <param name="alignment">The <see cref="PlaneAlignment"/> being extended.</param>
        /// <returns><c>true</c> if the plane is vertical.</returns>
        public static bool IsVertical(this PlaneAlignment alignment)
        {
            return (alignment == PlaneAlignment.Vertical);
        }
    }
}
