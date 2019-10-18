using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// This subsystem controls the lifecycle of an XR session. Some platforms,
    /// particularly those that have non-XR modes, need to be able to turn the
    /// session on and off to enter and exit XR mode(s) of operation.
    /// </summary>
    public abstract class XRSessionSubsystem : XRSubsystem<XRSessionSubsystemDescriptor>
    {
        /// <summary>
        /// Returns an implementation-defined pointer associated with the session.
        /// </summary>
        public IntPtr nativePtr => m_Provider.nativePtr;

        /// <summary>
        /// Returns a unique session identifier for this session.
        /// </summary>
        public Guid sessionId => m_Provider.sessionId;

        /// <summary>
        /// Asynchronously retrieves the <see cref="SessionAvailability"/>. Used to determine whether
        /// the current device supports XR and if the necessary software is installed.
        /// </summary>
        /// <remarks>
        /// This platform-agnostic method is typically implemented by a platform-specific package.
        /// </remarks>
        /// <returns>A <see cref="Promise{SessionAvailability}"/> which can be used to determine when the
        /// availability has been determined and retrieve the result.</returns>
        public Promise<SessionAvailability> GetAvailabilityAsync() => m_Provider.GetAvailabilityAsync();

        /// <summary>
        /// Asynchronously attempts to install XR software on the current device.
        /// Throws if <see cref="XRSessionSubsystemDescriptor.supportsInstall"/> is <c>false</c>.
        /// </summary>
        /// <remarks>
        /// This platform-agnostic method is typically implemented by a platform-specific package.
        /// </remarks>
        /// <returns>A <see cref="Promise{SessionInstallationStatus}"/> which can be used to determine when the
        /// installation completes and retrieve the result.</returns>
        public Promise<SessionInstallationStatus> InstallAsync()
        {
            if (!SubsystemDescriptor.supportsInstall)
                throw new NotSupportedException("InstallAsync is not supported on this platform.");

            return m_Provider.InstallAsync();
        }

        /// <summary>
        /// Do not call this directly. Call create on a valid <see cref="XRSessionSubsystemDescriptor"/> instead.
        /// </summary>
        public XRSessionSubsystem() => m_Provider = CreateProvider();

        /// <summary>
        /// Starts or resumes the session.
        /// </summary>
        protected sealed override void OnStart() => m_Provider.Resume();

        /// <summary>
        /// Restarts a session. <see cref="Stop"/> and <see cref="Start"/> pause and resume
        /// a session, respectively. <c>Restart</c> resets the session state and clears
        /// and any detected trackables.
        /// </summary>
        public void Reset() => m_Provider.Reset();

        /// <summary>
        /// Pauses the session.
        /// </summary>
        protected sealed override void OnStop() => m_Provider.Pause();

        /// <summary>
        /// Destroys the session.
        /// </summary>
        protected sealed override void OnDestroyed() => m_Provider.Destroy();

        /// <summary>
        /// Trigger the session's update loop.
        /// </summary>
        /// <param name="updateParams">Data needed by the session to perform its update.</param>
        public void Update(XRSessionUpdateParams updateParams) => m_Provider.Update(updateParams);

        /// <summary>
        /// Should be invoked when the application is paused.
        /// </summary>
        public void OnApplicationPause() =>  m_Provider.OnApplicationPause();

        /// <summary>
        /// Should be invoked when the application is resumed.
        /// </summary>
        public void OnApplicationResume() => m_Provider.OnApplicationResume();

        /// <summary>
        /// Gets the <see cref="TrackingState"/> for the session.
        /// </summary>
        public TrackingState trackingState => m_Provider.trackingState;

        /// <summary>
        /// Gets the <see cref="NotTrackingReason"/> for the session.
        /// </summary>
        public NotTrackingReason notTrackingReason => m_Provider.notTrackingReason;

        /// <summary>
        /// Whether the AR session update is synchronized with the Unity frame rate.
        /// If <c>true</c>, <see cref="Update"/> should block until the next AR frame is available.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Thrown if <see cref="XRSessionSubsystemDescriptor.supportsMatchFrameRate"/> is <c>False</c>.</exception>
        public bool matchFrameRate
        {
            get => m_Provider.matchFrameRate;
            set => m_Provider.matchFrameRate = value;
        }

        /// <summary>
        /// The native update rate of the AR Session.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Thrown if <see cref="XRSessionSubsystemDescriptor.supportsMatchFrameRate"/> is <c>False</c>.</exception>
        public int frameRate => m_Provider.frameRate;

        /// <summary>
        /// Implement this to provide this class with an interface to
        /// platform specific implementations.
        /// </summary>
        /// <returns>An implementation specific provider.</returns>
        protected abstract Provider CreateProvider();

        /// <summary>
        /// The API this subsystem uses to interop with
        /// different provider implementations.
        /// </summary>
        protected class Provider
        {
            /// <summary>
            /// Invoked to start or resume a session. This is different from <see cref="OnApplicationResume"/>.
            /// </summary>
            public virtual void Resume() { }

            /// <summary>
            /// Invoked to pause a running session. This is different from <see cref="OnApplicationPause"/>.
            /// </summary>
            public virtual void Pause() { }

            /// <summary>
            /// Perform any per-frame update logic here.
            /// </summary>
            /// <param name="updateParams">Paramters about the current state that may be needed to inform the session.</param>
            public virtual void Update(XRSessionUpdateParams updateParams) { }

            /// <summary>
            /// Stop the session and destroy all associated resources.
            /// </summary>
            public virtual void Destroy() { }

            /// <summary>
            /// Reset the session. The behavior should be equivalent to destroying and recreating the session.
            /// </summary>
            public virtual void Reset() { }

            /// <summary>
            /// Invoked when the application is paused.
            /// </summary>
            public virtual void OnApplicationPause() { }

            /// <summary>
            /// Invoked when the application is resumed.
            /// </summary>
            public virtual void OnApplicationResume() { }

            /// <summary>
            /// Get a pointer to an object associated with the session.
            /// Callers should be able to manipulate the session in their own code using this.
            /// </summary>
            public virtual IntPtr nativePtr => IntPtr.Zero;

            /// <summary>
            /// Get the session's availability, such as whether the platform supports XR.
            /// </summary>
            /// <returns>A <see cref="Promise{T}"/> that the caller can yield on until availability is determined.</returns>
            public virtual Promise<SessionAvailability> GetAvailabilityAsync()
            {
                return Promise<SessionAvailability>.CreateResolvedPromise(SessionAvailability.None);
            }

            /// <summary>
            /// Attempt to update or install necessary XR software. Will only be called if
            /// <see cref="XRSessionSubsystemDescriptor.supportsInstall"/> is true.
            /// </summary>
            /// <returns></returns>
            public virtual Promise<SessionInstallationStatus> InstallAsync()
            {
                return Promise<SessionInstallationStatus>.CreateResolvedPromise(SessionInstallationStatus.ErrorInstallNotSupported);
            }

            /// <summary>
            /// Get the <see cref="TrackingState"/> for the session.
            /// </summary>
            public virtual TrackingState trackingState => TrackingState.None;

            /// <summary>
            /// Get the <see cref="NotTrackingReason"/> for the session.
            /// </summary>
            public virtual NotTrackingReason notTrackingReason => NotTrackingReason.Unsupported;

            /// <summary>
            /// Get a unique identifier for this session
            /// </summary>
            public virtual Guid sessionId => Guid.Empty;

            /// <summary>
            /// Whether the AR session update is synchronized with the Unity frame rate.
            /// If <c>true</c>, <see cref="Update"/> should block until the next AR frame is available.
            /// Must be implemented if
            /// <see cref="XRSessionSubsystemDescriptor.supportsMatchFrameRate"/>
            /// is <c>True</c>.
            /// </summary>
            public virtual bool matchFrameRate
            {
                get => false;
                set
                {
                    if (value)
                    {
                        throw new NotSupportedException("Matching frame rate is not supported.");
                    }
                }
            }

            /// <summary>
            /// The native update rate of the AR Session. Must be implemented if
            /// <see cref="XRSessionSubsystemDescriptor.supportsMatchFrameRate"/>
            /// is <c>True</c>.
            /// </summary>
            public virtual int frameRate =>
                throw new NotSupportedException("Querying the frame rate is not supported by this session subsystem.");
        }

        Provider m_Provider;
    }
}
