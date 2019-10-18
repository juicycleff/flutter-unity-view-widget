using System;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a tracked object in the physical environment.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_TrackedObject)]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARTrackedObject.html")]
    public class ARTrackedObject : ARTrackable<XRTrackedObject, ARTrackedObject>
    {
        /// <summary>
        /// Get a native pointer associated with this tracked object.
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
        /// The reference object which was used to detect this object in the environment.
        /// </summary>
        public XRReferenceObject referenceObject { get; internal set; }
    }
}
