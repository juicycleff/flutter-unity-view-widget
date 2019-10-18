using System.Collections.Generic;

#if !UNITY_2019_2_OR_NEWER
using UnityEngine.Experimental;
using UnityEngine.Experimental.XR;
#endif

#if USE_XR_MANAGEMENT
using UnityEngine.XR.Management;
#endif

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Manages the lifetime of the <c>XRInputSubsystem</c>. Add one of these to any <c>GameObject</c> in your scene
    /// if you want device pose information to be available. Read the input by using the <c>TrackedPoseDriver</c>
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_InputManager)]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARInputManager.html")]
    public sealed class ARInputManager : MonoBehaviour
    {
        /// <summary>
        /// Get the <c>XRInputSubsystem</c> whose lifetime this component manages.
        /// </summary>
        public XRInputSubsystem subsystem { get; private set; }

        bool m_CleanupSubsystemOnDestroy = true;

        void OnEnable()
        {
            CreateSubsystemIfNecessary();

            if (subsystem != null)
                subsystem.Start();
        }

        void OnDisable()
        {
            if (subsystem != null)
                subsystem.Stop();
        }

        void OnDestroy()
        {
            if (m_CleanupSubsystemOnDestroy && subsystem != null)
                subsystem.Destroy();

            subsystem = null;
        }

        void CreateSubsystemIfNecessary()
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

        XRInputSubsystem CreateSubsystem()
        {
            SubsystemManager.GetSubsystemDescriptors(s_SubsystemDescriptors);
            if (s_SubsystemDescriptors.Count > 0)
            {
                var descriptor = s_SubsystemDescriptors[0];
                if (s_SubsystemDescriptors.Count > 1)
                {
                    Debug.LogWarningFormat("Multiple {0} found. Using {1}",
                        typeof(XRInputSubsystem).Name,
                        descriptor.id);
                }

                return descriptor.Create();
            }
            else
            {
                return null;
            }
        }

        XRInputSubsystem GetActiveSubsystemInstance()
        {
            XRInputSubsystem activeSubsystem = null;

#if USE_XR_MANAGEMENT
            // If the XR management package has been included, query the currently
            // active loader for the created subsystem, if one exists.
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                XRLoader loader = XRGeneralSettings.Instance.Manager.activeLoader;
                if (loader != null)
                    activeSubsystem = loader.GetLoadedSubsystem<XRInputSubsystem>();
            }
#endif
            // If XR management is not used or no loader has been set, check for
            // any active subsystem instances in the SubsystemManager.
            if (activeSubsystem == null)
            {
                List<XRInputSubsystem> subsystemInstances = new List<XRInputSubsystem>();
                SubsystemManager.GetInstances(subsystemInstances);
                if (subsystemInstances.Count > 0)
                    activeSubsystem = subsystemInstances[0];
            }

            return activeSubsystem;
        }

        static List<XRInputSubsystemDescriptor> s_SubsystemDescriptors =
            new List<XRInputSubsystemDescriptor>();
    }
}
