namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// An interface for a reference image library. This is a set of reference images
    /// to search for in the physical environment.
    /// </summary>
    /// <seealso cref="RuntimeReferenceImageLibrary"/>
    /// <seealso cref="MutableRuntimeReferenceImageLibrary"/>
    /// <seealso cref="XRReferenceImageLibrary"/>
    /// <seealso cref="XRReferenceImage"/>
    public interface IReferenceImageLibrary
    {
        /// <summary>
        /// Get the number of reference images in the library.
        /// </summary>
        int count { get; }

        /// <summary>
        /// Get the <see cref="XRReferenceImage"/> image at <c>index</c>.
        /// </summary>
        XRReferenceImage this[int index] { get; }
    }
}
