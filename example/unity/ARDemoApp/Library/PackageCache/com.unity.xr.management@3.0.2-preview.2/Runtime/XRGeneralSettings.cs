using System;
using System.Collections;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.XR.Management
{
    /// <summary>General settings container used to house the instance of the active settings as well as the manager
    /// instance used to load the loaders with.
    /// </summary>
    public class XRGeneralSettings : ScriptableObject
    {
        /// <summary>The key used to query to get the current loader settings.</summary>
        public static string k_SettingsKey = "com.unity.xr.management.loader_settings";
        internal static XRGeneralSettings s_RuntimeSettingsInstance = null;

        [SerializeField]
        internal XRManagerSettings m_LoaderManagerInstance = null;

        [SerializeField]
        internal bool m_InitManagerOnStart = true;

        /// <summary>The current active manager used to manage XR lifetime.</summary>
        public XRManagerSettings Manager
        {
            get { return m_LoaderManagerInstance; }
            set { m_LoaderManagerInstance = value; }
        }

        private XRManagerSettings m_XRManager = null;

        /// <summary>The current settings instance.</summary>
        public static XRGeneralSettings Instance
        {
            get
            {
                return s_RuntimeSettingsInstance;
            }
        }

        /// <summary>The current active manager used to manage XR lifetime.</summary>
        public XRManagerSettings AssignedSettings
        {
            get
            {
                return m_LoaderManagerInstance;
            }
#if UNITY_EDITOR
            set
            {
                m_LoaderManagerInstance = value;
            }
#endif
        }

        /// <summary>Used to set if the manager is activated and initialized on startup.</summary>
        public bool InitManagerOnStart
        {
            get
            {
                return m_InitManagerOnStart;
            }
            #if UNITY_EDITOR
            set
            {
                m_InitManagerOnStart = value;
            }
            #endif
        }


#if !UNITY_EDITOR
        void Awake()
        {
            Debug.Log("XRGeneral Settings awakening...");
            s_RuntimeSettingsInstance = this;
            Application.quitting += Quit;
            DontDestroyOnLoad(s_RuntimeSettingsInstance);
        }
#endif

#if UNITY_EDITOR
        bool m_IsPlaying = false;

        void EnterPlayMode()
        {
            if (!m_IsPlaying)
            {
                if (s_RuntimeSettingsInstance == null)
                    s_RuntimeSettingsInstance = this;

                InitXRSDK();
                StartXRSDK();
                m_IsPlaying = true;
            }
        }

        void ExitPlayMode()
        {
            if (m_IsPlaying)
            {
                m_IsPlaying = false;
                StopXRSDK();
                DeInitXRSDK();

                if (s_RuntimeSettingsInstance != null)
                    s_RuntimeSettingsInstance = null;

            }
        }

        /// <summary>For internal use only.</summary>
        public void InternalPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    EnterPlayMode();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    ExitPlayMode();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    break;
            }
        }
#else

        static void Quit()
        {
            XRGeneralSettings instance = XRGeneralSettings.Instance;
            if (instance == null)
                return;

            instance.OnDisable();
            instance.OnDestroy();
        }

        void Start()
        {
            StartXRSDK();
        }

        void OnDisable()
        {
            StopXRSDK();
        }

        void OnDestroy()
        {
            DeInitXRSDK();
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void AttemptInitializeXRSDKOnLoad()
        {
#if !UNITY_EDITOR
            XRGeneralSettings instance = XRGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.InitXRSDK();
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        internal static void AttemptStartXRSDKOnBeforeSplashScreen()
        {
#if !UNITY_EDITOR
            XRGeneralSettings instance = XRGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.StartXRSDK();
#endif
        }

        private void InitXRSDK()
        {
            if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.m_LoaderManagerInstance == null || XRGeneralSettings.Instance.m_InitManagerOnStart == false)
                return;

            m_XRManager = XRGeneralSettings.Instance.m_LoaderManagerInstance;
            if (m_XRManager == null)
            {
                Debug.LogError("Assigned GameObject for XR Management loading is invalid. XR SDK will not be automatically loaded.");
                return;
            }

            m_XRManager.automaticLoading = false;
            m_XRManager.automaticRunning = false;
            m_XRManager.InitializeLoaderSync();
        }

        private void StartXRSDK()
        {
            if (m_XRManager != null && m_XRManager.activeLoader != null)
            {
                m_XRManager.StartSubsystems();
            }
        }

        private void StopXRSDK()
        {
            if (m_XRManager != null && m_XRManager.activeLoader != null)
            {
                m_XRManager.StopSubsystems();
            }
        }

        private void DeInitXRSDK()
        {
            if (m_XRManager != null && m_XRManager.activeLoader != null)
            {
                m_XRManager.DeinitializeLoader();
                m_XRManager = null;
            }
        }

    }
}
