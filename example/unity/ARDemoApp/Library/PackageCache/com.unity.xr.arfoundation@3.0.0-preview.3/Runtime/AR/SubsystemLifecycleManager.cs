using System;
using System.Collections.Generic;

#if !UNITY_2019_2_OR_NEWER
using UnityEngine.Experimental;
#endif

#if USE_XR_MANAGEMENT
using UnityEngine.XR.Management;
#endif

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A base class for subsystems whose lifetime is managed by a <c>MonoBehaviour</c>.
    /// </summary>
    /// <typeparam name="TSubsystem">The <c>Subsystem</c> which provides this manager data.</typeparam>
    /// <typeparam name="TSubsystemDescriptor">The <c>SubsystemDescriptor</c> required to create the Subsystem.</typeparam>
    public class SubsystemLifecycleManager<TSubsystem, TSubsystemDescriptor> : MonoBehaviour
        where TSubsystem : Subsystem<TSubsystemDescriptor>
        where TSubsystemDescriptor : SubsystemDescriptor<TSubsystem>
    {
        /// <summary>
        /// Get the <c>TSubsystem</c> whose lifetime this component manages.
        /// </summary>
        public TSubsystem subsystem { get; private set; }

        /// <summary>
        /// The descriptor for the subsystem.
        /// </summary>
        /// <value>
        /// The descriptor for the subsystem.
        /// </value>
        public TSubsystemDescriptor descriptor
        {
            get { return (subsystem == null) ? null : subsystem.SubsystemDescriptor; }
        }

        bool m_CleanupSubsystemOnDestroy = true;

        /// <summary>
        /// Creates a <c>TSubsystem</c>.
        /// </summary>
        /// <returns>The first Subsystem of matching the <c>TSubsystemDescriptor</c>, or <c>null</c> if there aren't any.</returns>
        protected virtual TSubsystem CreateSubsystem()
        {
            SubsystemManager.GetSubsystemDescriptors(s_SubsystemDescriptors);
            if (s_SubsystemDescriptors.Count > 0)
            {
                var descriptor = s_SubsystemDescriptors[0];
                if (s_SubsystemDescriptors.Count > 1)
                {
                    Debug.LogWarningFormat("Multiple {0} found. Using {1}",
                        typeof(TSubsystem).Name,
                        descriptor.id);
                }

                return descriptor.Create();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a subsystem if subsystem is <c>null</c>.
        /// </summary>
        protected void CreateSubsystemIfNecessary()
        {
            // Use the subsystem that has been instantiated by XR Management
            // if available, otherwise create the subsystem.

            if (subsystem == null)
            {
                subsystem = GetActiveSubsystemInstance();

                // If the subsystem has already been created by XR management, it controls the lifetime
                // of the subsystem.
                if (subsystem != null)
                    m_CleanupSubsystemOnDestroy = false;
            }

            if (subsystem == null)
                subsystem = CreateSubsystem();
        }

        /// <summary>
        /// Returns the active <c>TSubsystem</c> instance if present, otherwise returns null.
        /// </summary>
        protected TSubsystem GetActiveSubsystemInstance()
        {
            TSubsystem activeSubsystem = null;

#if USE_XR_MANAGEMENT
            // If the XR management package has been included, query the currently
            // active loader for the created subsystem, if one exists.
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                XRLoader loader = XRGeneralSettings.Instance.Manager.activeLoader;
                if (loader != null)
                    activeSubsystem = loader.GetLoadedSubsystem<TSubsystem>();
            }
#endif
            // If XR management is not used or no loader has been set, check for
            // any active subsystem instances in the SubsystemManager.
            if (activeSubsystem == null)
            {
                SubsystemManager.GetInstances(s_SubsystemInstances);

#if !UNITY_2019_3_OR_NEWER
                foreach (var instance in s_SubsystemInstances)
                {
                   if (!s_DestroyedSubsystemTypes.Contains(instance))
                   {
                        return instance;
                   }
                }
#else
                if (s_SubsystemInstances.Count > 0)
                {
                    activeSubsystem = s_SubsystemInstances[0];
                }
#endif
            }

            return activeSubsystem;
        }

        /// <summary>
        /// Creates the <c>TSubsystem</c>.
        /// </summary>
        protected virtual void OnEnable()
        {
            CreateSubsystemIfNecessary();

            if (subsystem != null)
            {
                OnBeforeStart();

                // The derived class may disable the
                // component if it has invalid state
                if (enabled)
                {
                    subsystem.Start();
                    OnAfterStart();
                }
            }
        }

        /// <summary>
        /// Stops the <c>TSubsystem</c>.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (subsystem != null)
                subsystem.Stop();
        }

        /// <summary>
        /// Destroys the <c>TSubsystem</c>.
        /// </summary>
        protected virtual void OnDestroy()
        {

            if (m_CleanupSubsystemOnDestroy && subsystem != null)
            {
#if !UNITY_2019_3_OR_NEWER
                s_DestroyedSubsystemTypes.Add(subsystem);
#endif

                subsystem.Destroy();
            }

            subsystem = null;
        }

        /// <summary>
        /// Invoked after creating the subsystem and before calling Start on it.
        /// The <see cref="subsystem"/> is not <c>null</c>.
        /// </summary>
        protected virtual void OnBeforeStart()
        { }

        /// <summary>
        /// Invoked after calling Start on it the Subsystem.
        /// The <see cref="subsystem"/> is not <c>null</c>.
        /// </summary>
        protected virtual void OnAfterStart()
        { }

        static List<TSubsystemDescriptor> s_SubsystemDescriptors =
            new List<TSubsystemDescriptor>();

        static List<TSubsystem> s_SubsystemInstances = 
            new List<TSubsystem>();
            
#if !UNITY_2019_3_OR_NEWER
        static HashSet<TSubsystem> s_DestroyedSubsystemTypes = 
            new HashSet<TSubsystem>();
#endif
    }
}
