using System;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Subsystem for managing the participants in a multi-user collaborative session.
    /// </summary>
    public abstract class XRParticipantSubsystem : TrackingSubsystem<XRParticipant, XRParticipantSubsystemDescriptor>
    {
        /// <summary>
        /// Do not call this directly. Call create on a valid <see cref="XRParticipantSubsystemDescriptor"/> instead.
        /// </summary>
        public XRParticipantSubsystem() => m_Provider = CreateProvider();

        /// <summary>
        /// Starts the participant subsystem.
        /// </summary>
        protected sealed override void OnStart() => m_Provider.Start();

        /// <summary>
        /// Stops the participant subsystem.
        /// </summary>
        protected sealed override void OnStop() => m_Provider.Stop();

        /// <summary>
        /// Destroys the participant subsystem.
        /// </summary>
        protected sealed override void OnDestroyed() => m_Provider.Destroy();

        /// <summary>
        /// Get the changed (added, updated, and removed) participants since the last call to <see cref="GetChanges(Allocator)"/>.
        /// </summary>
        /// <param name="allocator">An <c>Allocator</c> to use when allocating the returned <c>NativeArray</c>s.</param>
        /// <returns>
        /// <see cref="TrackableChanges{T}"/> describing the participants that have been added, updated, and removed
        /// since the last call to <see cref="GetChanges(Allocator)"/>. The caller owns the memory allocated with <c>Allocator</c>.
        /// </returns>
        public override TrackableChanges<XRParticipant> GetChanges(Allocator allocator)
        {
            var changes = m_Provider.GetChanges(XRParticipant.defaultParticipant, allocator);
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            m_ValidationUtility.ValidateAndDisposeIfThrown(changes);
#endif
            return changes;
        }

        /// <summary>
        /// Implement this to provide this class with an interface to platform specific implementations.
        /// </summary>
        /// <returns>An implementation specific provider.</returns>
        protected abstract Provider CreateProvider();

        /// <summary>
        /// The API this subsystem uses to interop with different provider implementations.
        /// </summary>
        protected abstract class Provider
        {
            /// <summary>
            /// Invoked to start the participant subsystem.
            /// </summary>
            public virtual void Start()
            { }

            /// <summary>
            /// Invoked to stop the participant subsystem.
            /// </summary>
            public virtual void Stop()
            { }

            /// <summary>
            /// Invoked to shutdown and destroy the participant subsystem. The implementation should release any
            /// resources required by the subsystem. If the subsystem is running, <see cref="Stop"/> will always
            /// be called before <see cref="Destroy"/>.
            /// </summary>
            public virtual void Destroy()
            { }

            /// <summary>
            /// Get the changed (added, updated, and removed) participants since the last call to <see cref="GetChanges(Allocator)"/>.
            /// </summary>
            /// <param name="defaultParticipant">
            /// The default participant. This should be used to initialize the returned <c>NativeArray</c>s for backwards compatibility.
            /// See <see cref="TrackableChanges{T}.TrackableChanges(void*, int, void*, int, void*, int, T, int, Allocator)"/>.
            /// </param>
            /// <param name="allocator">An <c>Allocator</c> to use when allocating the returned <c>NativeArray</c>s.</param>
            /// <returns>
            /// <see cref="TrackableChanges{T}"/> describing the participants that have been added, updated, and removed
            /// since the last call to <see cref="GetChanges(Allocator)"/>. The changes should be allocated using
            /// <paramref name="allocator"/>.
            /// </returns>
            public abstract TrackableChanges<XRParticipant> GetChanges(
                XRParticipant defaultParticipant,
                Allocator allocator);
        }

        Provider m_Provider;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        ValidationUtility<XRParticipant> m_ValidationUtility = new ValidationUtility<XRParticipant>();
#endif
    }
}
