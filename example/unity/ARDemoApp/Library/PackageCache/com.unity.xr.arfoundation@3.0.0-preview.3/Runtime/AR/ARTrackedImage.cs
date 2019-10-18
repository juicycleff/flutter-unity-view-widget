using System;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a tracked image in the physical environment.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_TrackedImage)]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARTrackedImage.html")]
    public class ARTrackedImage : ARTrackable<XRTrackedImage, ARTrackedImage>
    {
        /// <summary>
        /// The 2D extents of the image. This is half the <see cref="size"/>.
        /// </summary>
        public Vector2 extents
        {
            get { return sessionRelativeData.size * 0.5f; }
        }

        /// <summary>
        /// The 2D size of the image. This is the dimensions of the image.
        /// </summary>
        /// <value></value>
        public Vector2 size
        {
            get { return sessionRelativeData.size; }
        }

        /// <summary>
        /// Get a native pointer associated with this tracked image.
        /// </summary>
        /// <remarks>
        /// The data pointed to by this member is implementation defined.
        /// The lifetime of the pointed to object is also
        /// implementation defined, but should be valid at least until the next
        /// <see cref="ARSession"/> update.
        /// </remarks>
        public IntPtr nativePtr
        {
            get { return sessionRelativeData.nativePtr; }
        }

        /// <summary>
        /// The reference image which was used to detect this image in the environment.
        /// </summary>
        public XRReferenceImage referenceImage { get; internal set; }
    }
}
