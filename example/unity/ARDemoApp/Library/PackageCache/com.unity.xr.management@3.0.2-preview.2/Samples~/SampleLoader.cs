using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

#if !UNITY_2019_3_OR_NEWER
using UnityEngine.Experimental;
using UnityEngine.Experimental.XR;
#endif

namespace Samples
{
    /// <summary>
    /// Sample loader implentation showing how to create simple loader.
    /// NOTE: You have to rename this class to make it appear in the loader list for
    /// XRManager.
    /// </summary>
    public class SampleLoader : XRLoaderHelper
    {
        static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors =
            new List<XRInputSubsystemDescriptor>();

        /// <summary>Return the currently active Input Subsystem intance, if any.</summary>
        public XRInputSubsystem inputSubsystem
        {
            get { return GetLoadedSubsystem<XRInputSubsystem>(); }
        }

        SampleSettings GetSettings()
        {
            SampleSettings settings = null;
            // When running in the Unity Editor, we have to load user's customization of configuration data directly from
            // EditorBuildSettings. At runtime, we need to grab it from the static instance field instead.
            #if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject(SampleConstants.k_SettingsKey, out settings);
            #else
            settings = SampleSettings.s_RuntimeInstance;
            #endif
            return settings;
        }

        #region XRLoader API Implementation

        /// <summary>Implementaion of <see cref="XRLoader.Initialize"></summary>
        /// <returns>True if successful, false otherwise</returns>
        public override bool Initialize()
        {
            SampleSettings settings = GetSettings();
            if (settings != null)
            {
                // TODO: Pass settings off to plugin prior to subsystem init.
            }

            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "InputSubsystemDescriptor");

            return false;
        }

        /// <summary>Implementaion of <see cref="XRLoader.Start"></summary>
        /// <returns>True if successful, false otherwise</returns>
        public override bool Start()
        {
            StartSubsystem<XRInputSubsystem>();
            return true;
        }

        /// <summary>Implementaion of <see cref="XRLoader.Stop"></summary>
        /// <returns>True if successful, false otherwise</returns>
        public override bool Stop()
        {
            StopSubsystem<XRInputSubsystem>();
            return true;
        }

        /// <summary>Implementaion of <see cref="XRLoader.Deinitialize"></summary>
        /// <returns>True if successful, false otherwise</returns>
        public override bool Deinitialize()
        {
            DestroySubsystem<XRInputSubsystem>();
            return true;
        }

        #endregion
    }
}
