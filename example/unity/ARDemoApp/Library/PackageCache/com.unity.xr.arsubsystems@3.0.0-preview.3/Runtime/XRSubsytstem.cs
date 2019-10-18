namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Serves as the base class for all the
    /// [subsystems](https://docs.unity3d.com/ScriptReference/Subsystem.html)
    /// in this package.
    /// </summary>
    /// <typeparam name="TSubsystemDescriptor">The
    /// [Subsystem Descriptor](https://docs.unity3d.com/ScriptReference/SubsystemDescriptor.html)
    /// for the
    /// [Subsystem](https://docs.unity3d.com/ScriptReference/Subsystem.html)
    /// </typeparam>
    public abstract class XRSubsystem<TSubsystemDescriptor> : Subsystem<TSubsystemDescriptor>
        where TSubsystemDescriptor : ISubsystemDescriptor
    {
        /// <summary>
        /// Invoked when <see cref="Start"/> is called and <see cref="running"/> is <c>false</c>.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Invoked when <see cref="Stop"/> is called and <see cref="running"/> is <c>true</c>.
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// Invoked when [Destroy](https://docs.unity3d.com/ScriptReference/Subsystem.Destroy.html)
        /// is called. This method will not be invoked more than once, even if <c>Destroy</c> is
        /// called multiple times.
        /// </summary>
        protected abstract void OnDestroyed();

        /// <summary>
        /// <c>true</c> if the Subsystem has been <c>Start</c>ed and is currently running,
        /// otherwise <c>false</c>.
        /// </summary>
        public sealed override bool running => m_Running;

        bool m_Running;

#if !UNITY_2020_1_OR_NEWER
        bool m_Destroyed;
#endif

        /// <summary>
        /// Destroys the [subsystem](https://docs.unity3d.com/ScriptReference/Subsystem.html).
        /// If the subsystem is <see cref="running"/>, <see cref="Stop"/> is also called.
        /// </summary>
#if UNITY_2019_3_OR_NEWER
        protected sealed override void OnDestroy()
#else
        public sealed override void Destroy()
#endif
        {
#if !UNITY_2020_1_OR_NEWER
            // 2020.1 has this logic built into Unity core, but before
            // that we need to track the destroyed state ourselves.
            if (m_Destroyed)
                return;
            m_Destroyed = true;
#endif
            Stop();
            OnDestroyed();
        }

        /// <summary>
        /// Starts the [subsystem](https://docs.unity3d.com/ScriptReference/Subsystem.html).
        /// </summary>
        public sealed override void Start()
        {
            if (!m_Running)
            {
                OnStart();
            }

            m_Running = true;
        }

        /// <summary>
        /// Stops the [subsystem](https://docs.unity3d.com/ScriptReference/Subsystem.html).
        /// </summary>
        public sealed override void Stop()
        {
            if (m_Running)
            {
                OnStop();
            }

            m_Running = false;
        }
    }
}
