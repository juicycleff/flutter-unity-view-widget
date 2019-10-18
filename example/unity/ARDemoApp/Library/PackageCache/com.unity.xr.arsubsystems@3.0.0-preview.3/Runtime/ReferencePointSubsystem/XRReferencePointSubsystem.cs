using System;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Base class for a reference point subsystem.
    /// </summary>
    /// <remarks>
    /// <para>A reference point is a pose in the physical environment that is tracked by an XR device.
    /// As the device refines its understanding of the environment, reference points will be
    /// updated, allowing developers to keep virtual content connected to a real-world position and orientation.</para>
    /// <para>This abstract class should be implemented by an XR provider and instantiated using the <c>SubsystemManager</c>
    /// to enumerate the available <see cref="XRReferencePointSubsystemDescriptor"/>s.</para>
    /// </remarks>
    public abstract class XRReferencePointSubsystem
        : TrackingSubsystem<XRReferencePoint, XRReferencePointSubsystemDescriptor>
    {
        /// <summary>
        /// Constructor. Do not invoke directly; use the <c>SubsystemManager</c>
        /// to enumerate the available <see cref="XRReferencePointSubsystemDescriptor"/>s
        /// and call <c>Create</c> on the desired descriptor.
        /// </summary>
        public XRReferencePointSubsystem() => m_Provider = CreateProvider();

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
        /// Get the changes (added, updated, & removed) reference points since the last call
        /// to <see cref="GetChanges(Allocator)"/>.
        /// </summary>
        /// <param name="allocator">An allocator to use for the <c>NativeArray</c>s in <see cref="TrackableChanges{T}"/>.</param>
        /// <returns>Changes since the last call to <see cref="GetChanges"/>.</returns>
        public override TrackableChanges<XRReferencePoint> GetChanges(Allocator allocator)
        {
            if (!running)
                throw new InvalidOperationException("Can't call \"GetChanges\" without \"Start\"ing the reference-point subsystem!");

            var changes = m_Provider.GetChanges(XRReferencePoint.defaultValue, allocator);
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            m_ValidationUtility.ValidateAndDisposeIfThrown(changes);
#endif
            return changes;
        }

        /// <summary>
        /// Attempts to create a new reference point with the provide <paramref name="pose"/>.
        /// </summary>
        /// <param name="pose">The pose, in session space, of the new reference point.</param>
        /// <param name="referencePoint">The new reference point. Only valid if this method returns <c>true</c>.</param>
        /// <returns><c>true</c> if the new reference point was added, otherwise <c>false</c>.</returns>
        public bool TryAddReferencePoint(Pose pose, out XRReferencePoint referencePoint)
        {
            return m_Provider.TryAddReferencePoint(pose, out referencePoint);
        }

        /// <summary>
        /// Attempts to create a new reference "attached" to the trackable with id <paramref name="trackableToAffix"/>.
        /// The behavior of the reference point depends on the type of trackable to which this reference point is attached.
        /// </summary>
        /// <param name="trackableToAffix">The id of the trackable to which to attach.</param>
        /// <param name="pose">The pose, in session space, of the reference point to create.</param>
        /// <param name="referencePoint">The new reference point. Only valid if this method returns <c>true</c>.</param>
        /// <returns><c>true</c> if the new reference point was added, otherwise <c>false</c>.</returns>
        public bool TryAttachReferencePoint(TrackableId trackableToAffix, Pose pose, out XRReferencePoint referencePoint)
        {
            return m_Provider.TryAttachReferencePoint(trackableToAffix, pose, out referencePoint);
        }

        /// <summary>
        /// Attempts to remove an existing reference point with <see cref="TrackableId"/> <paramref name="referencePointId"/>.
        /// </summary>
        /// <param name="referencePointId">The id of an existing reference point to remove.</param>
        /// <returns><c>true</c> if the reference point was removed, otherwise <c>false</c>.</returns>
        public bool TryRemoveReferencePoint(TrackableId referencePointId)
        {
            return m_Provider.TryRemoveReferencePoint(referencePointId);
        }

        /// <summary>
        /// An interface to be implemented by providers of this subsystem.
        /// </summary>
        protected abstract class Provider
        {
            /// <summary>
            /// Invoked when <c>Start</c> is called on the subsystem. This method is only called if the subsystem was not previously running.
            /// </summary>
            public virtual void Start() { }

            /// <summary>
            /// Invoked when <c>Stop</c> is called on the subsystem. This method is only called if the subsystem was previously running.
            /// </summary>
            public virtual void Stop() { }

            /// <summary>
            /// Called when <c>Destroy</c> is called on the subsystem.
            /// </summary>
            public virtual void Destroy() { }

            /// <summary>
            /// Invoked to get the changes to reference points (added, updated, & removed) since the last call to <see cref="GetChanges(Allocator)"/>.
            /// </summary>
            /// <param name="defaultReferencePoint">The default reference point. This should be used to initialize the returned
            /// <c>NativeArray</c>s for backwards compatibility.
            /// See <see cref="TrackableChanges{T}.TrackableChanges(void*, int, void*, int, void*, int, T, int, Allocator)"/>.
            /// </param>
            /// <param name="allocator">An allocator to use for the <c>NativeArray</c>s in <see cref="TrackableChanges{T}"/>.</param>
            /// <returns>Changes since the last call to <see cref="GetChanges"/>.</returns>
            public abstract TrackableChanges<XRReferencePoint> GetChanges(XRReferencePoint defaultReferencePoint, Allocator allocator);

            /// <summary>
            /// Should create a new reference point with the provide <paramref name="pose"/>.
            /// </summary>
            /// <param name="pose">The pose, in session space, of the new reference point.</param>
            /// <param name="referencePoint">The new reference point. Must be valid only if this method returns <c>true</c>.</param>
            /// <returns>Should return <c>true</c> if the new reference point was added, otherwise <c>false</c>.</returns>
            public virtual bool TryAddReferencePoint(Pose pose, out XRReferencePoint referencePoint)
            {
                referencePoint = default(XRReferencePoint);
                return false;
            }

            /// <summary>
            /// Should create a new reference "attached" to the trackable with id <paramref name="trackableToAffix"/>.
            /// The behavior of the reference point depends on the type of trackable to which this reference point is attached and
            /// may be implemenation-defined.
            /// </summary>
            /// <param name="trackableToAffix">The id of the trackable to which to attach.</param>
            /// <param name="pose">The pose, in session space, of the reference point to create.</param>
            /// <param name="referencePoint">The new reference point. Must be valid only if this method returns <c>true</c>.</param>
            /// <returns><c>true</c> if the new reference point was added, otherwise <c>false</c>.</returns>
            public virtual bool TryAttachReferencePoint(
                TrackableId trackableToAffix,
                Pose pose,
                out XRReferencePoint referencePoint)
            {
                referencePoint = default(XRReferencePoint);
                return false;
            }

            /// <summary>
            /// Should remove an existing reference point with <see cref="TrackableId"/> <paramref name="referencePointId"/>.
            /// </summary>
            /// <param name="referencePointId">The id of an existing reference point to remove.</param>
            /// <returns>Should return <c>true</c> if the reference point was removed, otherwise <c>false</c>. If the reference
            /// point does not exist, return <c>false</c>.</returns>
            public virtual bool TryRemoveReferencePoint(TrackableId referencePointId) => false;
        }

        /// <summary>
        /// Should return an instance of <see cref="IProvider"/>.
        /// </summary>
        /// <returns>The interface to the implementation-specific provider.</returns>
        protected abstract Provider CreateProvider();

        Provider m_Provider;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        ValidationUtility<XRReferencePoint> m_ValidationUtility =
            new ValidationUtility<XRReferencePoint>();
#endif
    }
}
