using Unity.Collections;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// An interface for performing raycasts against trackables. Generally implemented by 
    /// derived classes of <see cref="ARTrackableManager{TSubsystem, TSubsystemDescriptor, TSessionRelativeData, TTrackable}"/>.
    /// </summary>
    internal interface IRaycaster
    {
        /// <summary>
        /// Performs a raycast.
        /// </summary>
        /// <param name="sessionSpaceRay">A ray, in session space.</param>
        /// <param name="trackableTypeMask">The types of raycast to perform.</param>
        /// <returns>An array of raycast results.</returns>
        NativeArray<XRRaycastHit> Raycast(
            Ray sessionSpaceRay,
            TrackableType trackableTypeMask,
            Allocator allocator);
    }
}
