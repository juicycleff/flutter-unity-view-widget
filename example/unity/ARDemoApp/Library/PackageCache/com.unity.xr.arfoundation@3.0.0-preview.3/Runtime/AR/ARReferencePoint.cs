using System;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a Reference Point tracked by an XR device.
    /// </summary>
    /// <remarks>
    /// A reference point is a pose in the physical environment that is tracked by an XR device.
    /// As the device refines its understanding of the environment, reference points will be
    /// updated, helping you to keep virtual content connected to a real-world position and orientation.
    /// </remarks>
    [DefaultExecutionOrder(ARUpdateOrder.k_ReferencePoint)]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARReferencePoint.html")]
    public sealed class ARReferencePoint : ARTrackable<XRReferencePoint, ARReferencePoint>
    {
        /// <summary>
        /// Get the native pointer associated with this <see cref="ARReferencePoint"/>.
        /// </summary>
        /// <remarks>
        /// The data pointed to by this pointer is implementation defined. While its
        /// lifetime is also implementation defined, it should be valid until at least
        /// the next <see cref="ARSession"/> update.
        /// </remarks>
        public IntPtr nativePtr { get { return sessionRelativeData.nativePtr; } }

        /// <summary>
        /// Get the session identifier from which this reference point originated.
        /// </summary>
        public Guid sessionId { get { return sessionRelativeData.sessionId; } }
    }
}
