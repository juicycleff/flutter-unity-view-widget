using UnityEngine.XR.Management;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Settings to control the ARCoreLoader behavior.
    /// </summary>
    [XRConfigurationData("ARCore", ARCoreLoaderConstants.k_SettingsKey)]
    [System.Serializable]
    public class ARCoreLoaderSettings : ScriptableObject
    {
        /// <summary>
        /// Static instance that will hold the runtime asset instance we created in our build process.
        /// </summary>
        #if !UNITY_EDITOR
        internal static ARCoreLoaderSettings s_RuntimeInstance = null;
        #endif

        [SerializeField, Tooltip("Allow the ARCore Loader to start and stop subsystems.")]
        bool m_StartAndStopSubsystems = false;

        public bool startAndStopSubsystems
        {
            get { return m_StartAndStopSubsystems; }
            set { m_StartAndStopSubsystems = value; }
        }

        public void Awake()
        {
            #if !UNITY_EDITOR
            s_RuntimeInstance = this;
            #endif
        }
    }
}
