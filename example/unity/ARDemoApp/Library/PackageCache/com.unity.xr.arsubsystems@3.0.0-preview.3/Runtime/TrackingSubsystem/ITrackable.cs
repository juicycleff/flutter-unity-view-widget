using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Defines an interface for the <c>struct</c>s associated with tracking subsystems.
    /// </summary>
    /// <remarks>
    /// Subsystems that detect and track features in the environment, such as planes or images,
    /// follow a similar pattern and should use <see cref="ITrackable"/> for the structs defining
    /// their session relative data.
    /// </remarks>
    public interface ITrackable
    {
        /// <summary>
        /// The <see cref="TrackableId"/> associated with this trackable.
        /// </summary>
        TrackableId trackableId { get; }

        /// <summary>
        /// The <c>Pose</c> associated with this trackable.
        /// </summary>
        Pose pose { get; }

        /// <summary>
        /// The <see cref="TrackingState"/> associated with this trackable.
        /// </summary>
        TrackingState trackingState { get; }

        /// <summary>
        /// The native pointer associated with this trackable.
        /// </summary>
        IntPtr nativePtr { get; }
    }
}
