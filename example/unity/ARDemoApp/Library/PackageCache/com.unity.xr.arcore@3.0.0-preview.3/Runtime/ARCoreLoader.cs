using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

namespace UnityEngine.XR.ARCore
{
    public class ARCoreLoader : XRLoaderHelper
    {
        private static List<XRSessionSubsystemDescriptor> s_SessionSubsystemDescriptors = new List<XRSessionSubsystemDescriptor>();
        private static List<XRCameraSubsystemDescriptor> s_CameraSubsystemDescriptors = new List<XRCameraSubsystemDescriptor>();
        private static List<XRDepthSubsystemDescriptor> s_DepthSubsystemDescriptors = new List<XRDepthSubsystemDescriptor>();
        private static List<XRPlaneSubsystemDescriptor> s_PlaneSubsystemDescriptors = new List<XRPlaneSubsystemDescriptor>();
        private static List<XRReferencePointSubsystemDescriptor> s_ReferencePointSubsystemDescriptors = new List<XRReferencePointSubsystemDescriptor>();
        private static List<XRRaycastSubsystemDescriptor> s_RaycastSubsystemDescriptors = new List<XRRaycastSubsystemDescriptor>();
        private static List<XRImageTrackingSubsystemDescriptor> s_ImageTrackingSubsystemDescriptors = new List<XRImageTrackingSubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();
        private static List<XRFaceSubsystemDescriptor> s_FaceSubsystemDescriptors = new List<XRFaceSubsystemDescriptor>();

        public XRSessionSubsystem sessionSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRSessionSubsystem>();
            }
        }

        public XRCameraSubsystem cameraSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRCameraSubsystem>();
            }
        }

        public XRDepthSubsystem depthSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRDepthSubsystem>();
            }
        }

        public XRPlaneSubsystem planeSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRPlaneSubsystem>();
            }
        }

        public XRReferencePointSubsystem referencePointSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRReferencePointSubsystem>();
            }
        }

        public XRRaycastSubsystem raycastSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRRaycastSubsystem>();
            }
        }

        public XRImageTrackingSubsystem imageTrackingSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRImageTrackingSubsystem>();
            }
        }

        public XRInputSubsystem inputSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRInputSubsystem>();
            }
        }

        public XRFaceSubsystem faceSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRFaceSubsystem>();
            }
        }

        public override bool Initialize()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(s_SessionSubsystemDescriptors, "ARCore-Session");
            CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(s_CameraSubsystemDescriptors, "ARCore-Camera");
            CreateSubsystem<XRDepthSubsystemDescriptor, XRDepthSubsystem>(s_DepthSubsystemDescriptors, "ARCore-Depth");
            CreateSubsystem<XRPlaneSubsystemDescriptor, XRPlaneSubsystem>(s_PlaneSubsystemDescriptors, "ARCore-Plane");
            CreateSubsystem<XRReferencePointSubsystemDescriptor, XRReferencePointSubsystem>(s_ReferencePointSubsystemDescriptors, "ARCore-ReferencePoint");
            CreateSubsystem<XRRaycastSubsystemDescriptor, XRRaycastSubsystem>(s_RaycastSubsystemDescriptors, "ARCore-Raycast");
            CreateSubsystem<XRImageTrackingSubsystemDescriptor, XRImageTrackingSubsystem>(s_ImageTrackingSubsystemDescriptors, "ARCore-ImageTracking");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "ARCore-Input");
            CreateSubsystem<XRFaceSubsystemDescriptor, XRFaceSubsystem>(s_FaceSubsystemDescriptors, "ARCore-Face");

            if (sessionSubsystem == null)
            {
                Debug.LogError("Failed to load session subsystem.");
            }

            return sessionSubsystem != null;
#else
            return false;
#endif
        }

        public override bool Start()
        {
            var settings = GetSettings();
            if (settings != null && settings.startAndStopSubsystems)
            {
                StartSubsystem<XRSessionSubsystem>();
                StartSubsystem<XRCameraSubsystem>();
                StartSubsystem<XRDepthSubsystem>();
                StartSubsystem<XRPlaneSubsystem>();
                StartSubsystem<XRReferencePointSubsystem>();
                StartSubsystem<XRRaycastSubsystem>();
                StartSubsystem<XRImageTrackingSubsystem>();
                StartSubsystem<XRInputSubsystem>();
                StartSubsystem<XRFaceSubsystem>();
            }
            return true;
        }

        public override bool Stop()
        {
            var settings = GetSettings();
            if (settings != null && settings.startAndStopSubsystems)
            {
                StopSubsystem<XRSessionSubsystem>();
                StopSubsystem<XRCameraSubsystem>();
                StopSubsystem<XRDepthSubsystem>();
                StopSubsystem<XRPlaneSubsystem>();
                StopSubsystem<XRReferencePointSubsystem>();
                StopSubsystem<XRRaycastSubsystem>();
                StopSubsystem<XRImageTrackingSubsystem>();
                StopSubsystem<XRInputSubsystem>();
                StopSubsystem<XRFaceSubsystem>();
            }
            return true;
        }

        public override bool Deinitialize()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            DestroySubsystem<XRSessionSubsystem>();
            DestroySubsystem<XRCameraSubsystem>();
            DestroySubsystem<XRDepthSubsystem>();
            DestroySubsystem<XRPlaneSubsystem>();
            DestroySubsystem<XRReferencePointSubsystem>();
            DestroySubsystem<XRRaycastSubsystem>();
            DestroySubsystem<XRImageTrackingSubsystem>();
            DestroySubsystem<XRInputSubsystem>();
            DestroySubsystem<XRFaceSubsystem>();
#endif
            return true;
        }

        ARCoreLoaderSettings GetSettings()
        {
            ARCoreLoaderSettings settings = null;
            // When running in the Unity Editor, we have to load user's customization of configuration data directly from
            // EditorBuildSettings. At runtime, we need to grab it from the static instance field instead.
            #if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject(ARCoreLoaderConstants.k_SettingsKey, out settings);
            #else
            settings = ARCoreLoaderSettings.s_RuntimeInstance;
            #endif
            return settings;
        }
    }
}
