using System;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Base class for a raycast subsystem.
    /// </summary>
    /// <remarks>
    /// This abstract class should be implemented by an XR provider and instantiated using the <c>SubsystemManager</c>
    /// to enumerate the available <see cref="XRRaycastSubsystemDescriptor"/>s.
    /// </remarks>
    public abstract class XRRaycastSubsystem : XRSubsystem<XRRaycastSubsystemDescriptor>
    {
        /// <summary>
        /// Constructor. Do not invoke directly; use the <c>SubsystemManager</c>
        /// to enumerate the available <see cref="XRRaycastSubsystemDescriptor"/>s
        /// and call <c>Create</c> on the desired descriptor.
        /// </summary>
        public XRRaycastSubsystem() => m_Provider = CreateProvider();

        /// <summary>
        /// Starts the subsystem.
        /// </summary>
        protected sealed override void OnStart() => m_Provider.Start();

        /// <summary>
        /// Stops the subsystem.
        /// </summary>
        protected sealed override void OnStop() => m_Provider.Stop();

        /// <summary>
        /// Destroys the subsystem.
        /// </summary>
        protected sealed override void OnDestroyed() => m_Provider.Destroy();

        /// <summary>
        /// Casts <paramref name="ray"/> against trackables specified with <paramref name="trackableTypeMask"/>.
        /// </summary>
        /// <param name="ray">A ray in session space.</param>
        /// <param name="trackableTypeMask">The types of trackables to test for ray intersections.</param>
        /// <param name="allocator">The <c>Allocator</c> used to allocate the returned <c>NativeArray</c>.</param>
        /// <returns>A <c>NativeArray</c> of all the resulting ray intersections.</returns>
        public NativeArray<XRRaycastHit> Raycast(
            Ray ray,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            return m_Provider.Raycast(XRRaycastHit.defaultValue, ray, trackableTypeMask, allocator);
        }

        /// <summary>
        /// Casts a ray originating from <paramref name="screenPoint"/> against trackables specified with <paramref name="trackableTypeMask"/>.
        /// </summary>
        /// <param name="screenPoint">A point on the screen in normalized screen coordinates (0, 0) - (1, 1)</param>
        /// <param name="trackableTypeMask">The types of trackables to test for ray intersections.</param>
        /// <param name="allocator">The <c>Allocator</c> used to allocate the returned <c>NativeArray</c>.</param>
        /// <returns>A <c>NativeArray</c> of all the resulting ray intersections.</returns>
        public NativeArray<XRRaycastHit> Raycast(
            Vector2 screenPoint,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            return m_Provider.Raycast(XRRaycastHit.defaultValue, screenPoint, trackableTypeMask, allocator);
        }

        /// <summary>
        /// Should return an instance of <see cref="Provider"/>.
        /// </summary>
        /// <returns>The interface to the implementation-specific provider.</returns>
        protected abstract Provider CreateProvider();

        /// <summary>
        /// An interface to be implemented by providers of this subsystem.
        /// </summary>
        protected class Provider
        {
            /// <summary>
            /// Called when the subsystem is started. Will not be called again until <see cref="Stop"/>.
            /// </summary>
            public virtual void Start() { }

            /// <summary>
            /// Called when the subsystem is stopped. Will not be called before <see cref="Start"/>.
            /// </summary>
            public virtual void Stop() { }

            /// <summary>
            /// Called when the subsystem is destroyed. <see cref="Stop"/> will be called first if the subsystem is running.
            /// </summary>
            public virtual void Destroy() { }

            /// <summary>
            /// Performs a raycast from an arbitrary ray against the types
            /// specified by <paramref name="trackableTypeMask"/>. Results
            /// should be sorted by distance from the ray origin.
            /// </summary>
            /// <param name="defaultRaycastHit">The default raycast hit that should be used as a template when populating the returned <c>NativeArray</c>.</param>
            /// <param name="ray">A ray in session space from which to raycast.</param>
            /// <param name="trackableTypeMask">The types to raycast against.</param>
            /// <param name="allocator">The allocator with which to allocate the returned <c>NativeArray</c>.</param>
            public virtual NativeArray<XRRaycastHit> Raycast(
                XRRaycastHit defaultRaycastHit,
                Ray ray,
                TrackableType trackableTypeMask,
                Allocator allocator)
            {
                throw new NotSupportedException("Raycasting using a Ray is not supported.");
            }

            /// <summary>
            /// Performs a raycast from the camera against the types
            /// specified by <paramref name="trackableTypeMask"/>. Results
            /// should be sorted by distance from the ray origin.
            /// </summary>
            /// <param name="defaultRaycastHit">The default raycast hit that should be used as a template when populating the returned <c>NativeArray</c>.</param>
            /// <param name="screenPoint">A point on the screen in normalized (0...1) coordinates</param>
            /// <param name="trackableTypeMask">The types to raycast against.</param>
            /// <param name="allocator">The allocator with which to allocate the returned <c>NativeArray</c>.</param>
            public virtual NativeArray<XRRaycastHit> Raycast(
                XRRaycastHit defaultRaycastHit,
                Vector2 screenPoint,
                TrackableType trackableTypeMask,
                Allocator allocator)
            {
                throw new NotSupportedException("Raycasting using a screen point is not supported.");
            }
        }

        Provider m_Provider;
    }
}
