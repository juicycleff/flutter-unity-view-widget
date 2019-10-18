using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

namespace UnityEngine.XR.ARKit
{
    public class ARKitLoader : XRLoaderHelper
    {
        private static List<XRSessionSubsystemDescriptor> s_SessionSubsystemDescriptors = new List<XRSessionSubsystemDescriptor>();
        private static List<XRCameraSubsystemDescriptor> s_CameraSubsystemDescriptors = new List<XRCameraSubsystemDescriptor>();
        private static List<XRDepthSubsystemDescriptor> s_DepthSubsystemDescriptors = new List<XRDepthSubsystemDescriptor>();
        private static List<XRPlaneSubsystemDescriptor> s_PlaneSubsystemDescriptors = new List<XRPlaneSubsystemDescriptor>();
        private static List<XRReferencePointSubsystemDescriptor> s_ReferencePointSubsystemDescriptors = new List<XRReferencePointSubsystemDescriptor>();
        private static List<XRRaycastSubsystemDescriptor> s_RaycastSubsystemDescriptors = new List<XRRaycastSubsystemDescriptor>();
        private static List<XRHumanBodySubsystemDescriptor> s_HumanBodySubsystemDescriptors = new List<XRHumanBodySubsystemDescriptor>();
        private static List<XREnvironmentProbeSubsystemDescriptor> s_EnvironmentProbeSubsystemDescriptors = new List<XREnvironmentProbeSubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();
        private static List<XRImageTrackingSubsystemDescriptor> s_ImageTrackingSubsystemDescriptors = new List<XRImageTrackingSubsystemDescriptor>();
        private static List<XRObjectTrackingSubsystemDescriptor> s_ObjectTrackingSubsystemDescriptors = new List<XRObjectTrackingSubsystemDescriptor>();
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

        public XRHumanBodySubsystem humanBodySubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRHumanBodySubsystem>();
            }
        }

        public XREnvironmentProbeSubsystem environmentProbeSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XREnvironmentProbeSubsystem>();
            }
        }

        public XRInputSubsystem inputSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRInputSubsystem>();
            }
        }

        public XRImageTrackingSubsystem imageTrackingSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRImageTrackingSubsystem>();
            }
        }

        public XRObjectTrackingSubsystem objectTrackingSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRObjectTrackingSubsystem>();
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
#if UNITY_IOS && !UNITY_EDITOR
            CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(s_SessionSubsystemDescriptors, "ARKit-Session");
            CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(s_CameraSubsystemDescriptors, "ARKit-Camera");
            CreateSubsystem<XRDepthSubsystemDescriptor, XRDepthSubsystem>(s_DepthSubsystemDescriptors, "ARKit-Depth");
            CreateSubsystem<XRPlaneSubsystemDescriptor, XRPlaneSubsystem>(s_PlaneSubsystemDescriptors, "ARKit-Plane");
            CreateSubsystem<XRReferencePointSubsystemDescriptor, XRReferencePointSubsystem>(s_ReferencePointSubsystemDescriptors, "ARKit-ReferencePoint");
            CreateSubsystem<XRRaycastSubsystemDescriptor, XRRaycastSubsystem>(s_RaycastSubsystemDescriptors, "ARKit-Raycast");
            CreateSubsystem<XRHumanBodySubsystemDescriptor, XRHumanBodySubsystem>(s_HumanBodySubsystemDescriptors, "ARKit-HumanBody");
            CreateSubsystem<XREnvironmentProbeSubsystemDescriptor, XREnvironmentProbeSubsystem>(s_EnvironmentProbeSubsystemDescriptors, "ARKit-EnvironmentProbe");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "ARKit-Input");

            // Optional subsystems that might not have been registered, based on the iOS version.
            CreateSubsystem<XRImageTrackingSubsystemDescriptor, XRImageTrackingSubsystem>(s_ImageTrackingSubsystemDescriptors, "ARKit-ImageTracking");
            CreateSubsystem<XRObjectTrackingSubsystemDescriptor, XRObjectTrackingSubsystem>(s_ObjectTrackingSubsystemDescriptors, "ARKit-ObjectTracking");
            CreateSubsystem<XRFaceSubsystemDescriptor, XRFaceSubsystem>(s_FaceSubsystemDescriptors, "ARKit-Face");

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
                StartSubsystem<XRHumanBodySubsystem>();
                StartSubsystem<XREnvironmentProbeSubsystem>();
                StartSubsystem<XRInputSubsystem>();
                StartSubsystem<XRImageTrackingSubsystem>();
                StartSubsystem<XRObjectTrackingSubsystem>();
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
                StopSubsystem<XRHumanBodySubsystem>();
                StopSubsystem<XREnvironmentProbeSubsystem>();
                StopSubsystem<XRInputSubsystem>();
                StopSubsystem<XRImageTrackingSubsystem>();
                StopSubsystem<XRObjectTrackingSubsystem>();
                StopSubsystem<XRFaceSubsystem>();
            }
            return true;
        }

        public override bool Deinitialize()
        {
#if UNITY_IOS && !UNITY_EDITOR
            DestroySubsystem<XRSessionSubsystem>();
            DestroySubsystem<XRCameraSubsystem>();
            DestroySubsystem<XRDepthSubsystem>();
            DestroySubsystem<XRPlaneSubsystem>();
            DestroySubsystem<XRReferencePointSubsystem>();
            DestroySubsystem<XRRaycastSubsystem>();
            DestroySubsystem<XRHumanBodySubsystem>();
            DestroySubsystem<XREnvironmentProbeSubsystem>();
            DestroySubsystem<XRInputSubsystem>();
            DestroySubsystem<XRImageTrackingSubsystem>();
            DestroySubsystem<XRObjectTrackingSubsystem>();
            DestroySubsystem<XRFaceSubsystem>();
#endif
            return true;
        }

        ARKitLoaderSettings GetSettings()
        {
            ARKitLoaderSettings settings = null;
            // When running in the Unity Editor, we have to load user's customization of configuration data directly from
            // EditorBuildSettings. At runtime, we need to grab it from the static instance field instead.
            #if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject(ARKitLoaderConstants.k_SettingsKey, out settings);
            #else
            settings = ARKitLoaderSettings.s_RuntimeInstance;
            #endif
            return settings;
        }
    }
}
