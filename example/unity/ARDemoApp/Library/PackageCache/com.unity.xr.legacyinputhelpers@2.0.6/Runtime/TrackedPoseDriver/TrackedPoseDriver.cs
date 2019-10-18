using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.XR.Interaction;

#if ENABLE_VR
using UnityEngine.XR;
using UnityEngine.XR.Tango;
#endif

[assembly: InternalsVisibleTo("UnityEditor.SpatialTracking")]


namespace UnityEngine.SpatialTracking
{
    internal class TrackedPoseDriverDataDescription
    {        
        internal struct PoseData
        {
            public List<string> PoseNames;
            public List<TrackedPoseDriver.TrackedPose> Poses;
        }
     
        internal static List<PoseData> DeviceData = new List<PoseData>
        {
            // Generic XR Device
            new PoseData
            {
                PoseNames = new List<string>
                {
                    "Left Eye", "Right Eye", "Center Eye - HMD Reference", "Head", "Color Camera"
                },
                Poses = new List<TrackedPoseDriver.TrackedPose>
                {
                    TrackedPoseDriver.TrackedPose.LeftEye,
                    TrackedPoseDriver.TrackedPose.RightEye,
                    TrackedPoseDriver.TrackedPose.Center,
                    TrackedPoseDriver.TrackedPose.Head,
                    TrackedPoseDriver.TrackedPose.ColorCamera
                }
            },
            // generic controller
            new PoseData
            {
                PoseNames = new List<string>
                {
                    "Left Controller", "Right Controller"
                },
                Poses = new List<TrackedPoseDriver.TrackedPose>
                {
                    TrackedPoseDriver.TrackedPose.LeftPose,
                    TrackedPoseDriver.TrackedPose.RightPose
                }
            },
            // generic remote
            new PoseData
            {
                PoseNames = new List<string>
                {
                    "Device Pose"
                },
                Poses = new List<TrackedPoseDriver.TrackedPose>
                {
                    TrackedPoseDriver.TrackedPose.RemotePose,
                }
            },
        };
    }

 	/// <summary>
    /// Bitflag enum which represents what data was set on an associated Pose struct
    /// </summary>

    [Flags]
    public enum PoseDataFlags
    {
        /// <summary>
        /// No data was actually set on the pose
        /// </summary>
        NoData = 0,
        /// <summary>
        /// If this flag is set, position data was updated on the associated pose struct
        /// </summary>
        Position = 1 << 0,
        /// <summary>
        /// If this flag is set, rotation data was updated on the associated pose struct
        /// </summary>
        Rotation = 1 << 1,
    }

    /// <summary>
    /// The PoseDataSource class acts as a container for the GetDatafromSource method call that should be used by PoseProviders wanting to query data for a particular pose.
    /// </summary>
    static public class PoseDataSource
    {
#if ENABLE_VR
        static internal List<XR.XRNodeState> nodeStates = new List<XR.XRNodeState>();        
        static internal PoseDataFlags GetNodePoseData(XR.XRNode node, out Pose resultPose)
        {
            PoseDataFlags retData = PoseDataFlags.NoData;
            XR.InputTracking.GetNodeStates(nodeStates);
            foreach (XR.XRNodeState nodeState in nodeStates)
            {
                if (nodeState.nodeType == node)
                {
                    if(nodeState.TryGetPosition(out resultPose.position))
                    {
                        retData |= PoseDataFlags.Position;
                    }
                    if (nodeState.TryGetRotation(out resultPose.rotation))
                    {
                        retData |= PoseDataFlags.Rotation;
                    }
                    return retData;
                }
            }
            resultPose = Pose.identity;
            return retData;
        }
#endif

        /// <summary>
        /// <signature><![CDATA[GetDataFromSource(TrackedPose,Pose)]]></signature>
        /// <summary>The GetDatafromSource method is used to query data from the XRNode subsystem based on the provided pose source.</summary>
        /// <param name = "poseSource" > The pose source to request data for.</param>
        /// <param name = "resultPose" > The resulting pose data. This function will return the Center Eye pose if the Color Camera pose is not available. </param>
        /// <returns>Retuns a bitflag which represents which data has been retrieved from the provided pose source</returns>                            
        static public PoseDataFlags GetDataFromSource(TrackedPoseDriver.TrackedPose poseSource, out Pose resultPose)
        {
#if ENABLE_VR
            switch (poseSource)
            {
                case TrackedPoseDriver.TrackedPose.RemotePose:
                {
                   PoseDataFlags retFlags = GetNodePoseData(XR.XRNode.RightHand, out resultPose);
                    if (retFlags == PoseDataFlags.NoData)
                        return GetNodePoseData(XR.XRNode.LeftHand, out resultPose);
                    return retFlags;
                }
                case TrackedPoseDriver.TrackedPose.LeftEye:
                {
                    return GetNodePoseData(XR.XRNode.LeftEye, out resultPose);
                }
                case TrackedPoseDriver.TrackedPose.RightEye:
                {
                    return GetNodePoseData(XR.XRNode.RightEye, out resultPose);
                }
                case TrackedPoseDriver.TrackedPose.Head:
                {
                    return GetNodePoseData(XR.XRNode.Head, out resultPose);
                }
                case TrackedPoseDriver.TrackedPose.Center:
                {
                    return GetNodePoseData(XR.XRNode.CenterEye, out resultPose);
                }
                case TrackedPoseDriver.TrackedPose.LeftPose:
                {
                    return GetNodePoseData(XR.XRNode.LeftHand, out resultPose);
                }
                case TrackedPoseDriver.TrackedPose.RightPose:
                {
                    return GetNodePoseData(XR.XRNode.RightHand, out resultPose);
                }
                case TrackedPoseDriver.TrackedPose.ColorCamera:
                {
                    PoseDataFlags retFlags = TryGetTangoPose(out resultPose);
                    if(retFlags == PoseDataFlags.NoData)
                    {
                        // We fall back to CenterEye because we can't currently extend the XRNode structure, nor are we ready to replace it.
                        return GetNodePoseData(XR.XRNode.CenterEye, out resultPose);
                    }
                    return retFlags;
                } 
                default:
                {
                    Debug.LogWarningFormat("Unable to retrieve pose data for poseSource: {0}", poseSource.ToString());
                    break;
                }              
            }
#endif
            resultPose = Pose.identity;
            return PoseDataFlags.NoData;
        }
       
        static PoseDataFlags TryGetTangoPose(out Pose pose)
        {
#if ENABLE_VR
            PoseData poseOut;
            if (TangoInputTracking.TryGetPoseAtTime(out poseOut) && poseOut.statusCode == PoseStatus.Valid)
            {
                pose.position = poseOut.position;
                pose.rotation = poseOut.rotation;
                return PoseDataFlags.Position | PoseDataFlags.Rotation; ;
            }
#endif
            pose = Pose.identity;

            return PoseDataFlags.NoData;
        }
    }

    // The DefaultExecutionOrder is needed because TrackedPoseDriver does some
    // of its work in regular Update and FixedUpdate calls, but this needs to
    // be done before regular user scripts have their own Update and
    // FixedUpdate calls, in order that they correctly get the values for this
    // frame and not the previous.
    // -32000 is the minimal possible execution order value; -30000 makes it
    // is unlikely users chose lower values for their scripts by accident, but
    // still makes it possible.

    /// <summary>
    /// The TrackedPoseDriver component applies the current Pose value of a tracked device to the transform of the GameObject.
    /// TrackedPoseDriver can track multiple types of devices including XR HMDs, controllers, and remotes.
    /// </summary>
    [DefaultExecutionOrder(-30000)]
    [Serializable]
    [AddComponentMenu("XR/Tracked Pose Driver")]
    public class TrackedPoseDriver : MonoBehaviour
    {
        /// <summary>
        /// The device being tracked by the tracked pose driver
        /// </summary>
        public enum DeviceType
        {
            /// <summary>
            /// An XR Controller, use this value for controllers
            /// </summary>
            GenericXRDevice = 0,
            /// <summary>
            /// An Generic XR Devices, use this value for HMD and AR Mobile device tracking
            /// </summary>
            GenericXRController = 1,
            /// <summary>
            /// An XR Remote, use this value for mobile remotes
            /// </summary>
            GenericXRRemote = 2
        }

        public enum TrackedPose
        {
            /// <summary>
            /// The left eye of a HMD style device
            /// </summary>
            LeftEye = 0,
            /// <summary>
            /// The right eye of a HMD style device
            /// </summary>
            RightEye = 1,
            /// <summary>
            /// The center eye of a HMD style device, this is usually the default for most HMDs
            /// </summary>
            Center = 2,
            /// <summary>
            /// The head eye of a HMD style device
            /// </summary>
            Head = 3,
            /// <summary>
            /// The left hand controller pose
            /// </summary>
            LeftPose = 4,
            /// <summary>
            /// The right hand controller pose
            /// </summary>
            RightPose = 5,
            /// <summary>
            /// The color camera of a mobile device
            /// </summary>
            ColorCamera = 6,
            /// <summary>
            /// No Longer Used
            /// </summary>
            DepthCameraDeprecated = 7,
            /// <summary>
            /// No Longer Used
            /// </summary>
            FisheyeCameraDeprected = 8,
            /// <summary>
            /// No Longer Used
            /// </summary>
            DeviceDeprecated = 9,
            /// <summary>
            /// The pose of a mobile remote
            /// </summary>
            RemotePose = 10,
        }       

        [SerializeField]
        DeviceType m_Device;
        /// <summary>
        /// This is used to indicate which pose the TrackedPoseDriver is currently tracking.
        /// </summary>
        public DeviceType deviceType
        {
            get { return m_Device; }
            internal set { m_Device = value; }
        }

        [SerializeField]
        TrackedPose m_PoseSource = TrackedPoseDriver.TrackedPose.Center;
        /// <summary>
        /// The pose being tracked by the tracked pose driver
        /// </summary>
        public TrackedPose poseSource
        {
            get { return m_PoseSource; }
            internal set { m_PoseSource = value; }
        }

        /// <summary>
        /// This method is used to set the device / pose pair for the SpatialTracking.TrackedPoseDriver. setting an invalid combination of these values will return false.
        /// </summary>
        /// <param name="deviceType">The device type that we wish to track </param>
        /// <param name="pose">The pose source that we wish to track</param>
        /// <returns>true if the values provided are sensible, otherwise false</returns>
        public bool SetPoseSource(DeviceType deviceType, TrackedPose pose)
        {
            if ((int)deviceType < TrackedPoseDriverDataDescription.DeviceData.Count)
            {
                TrackedPoseDriverDataDescription.PoseData val = TrackedPoseDriverDataDescription.DeviceData[(int)deviceType];
                for (int i = 0; i < val.Poses.Count; ++i)
                {
                    if (val.Poses[i] == pose)
                    {
                        this.deviceType = deviceType;
                        poseSource = pose;
                        return true;
                    }
                }
            }
            return false;
        }

        [SerializeField]
        BasePoseProvider m_PoseProviderComponent = null;
        /// <summary>
        /// Optional: This field holds the reference to the PoseProvider instance that, if set, will be used to override the behavior of 
        /// the TrackedPoseDriver. When this field is empty, the TrackedPoseDriver will operate as per usual, with pose data being 
        /// retrieved from the device or pose settings of the TrackedPoseDriver. When this field is set, the pose data will be 
        /// provided by the attached PoseProvider. The device or pose fields will be hidden as they are no longer used to 
        /// control the parent GameObject Transform.
        /// </summary>
        public BasePoseProvider poseProviderComponent
        {
            get { return m_PoseProviderComponent; }
            set
            {
                m_PoseProviderComponent = value;
            }
        }

        PoseDataFlags GetPoseData(DeviceType device, TrackedPose poseSource, out Pose resultPose)
        {
            if (m_PoseProviderComponent != null)
            {
                return m_PoseProviderComponent.GetPoseFromProvider(out resultPose);
            }

            return PoseDataSource.GetDataFromSource(poseSource, out resultPose);
        }

        /// <summary>
        /// This enum is used to indicate which parts of the pose will be applied to the parent transform
        /// </summary>
        public enum TrackingType
        {
            /// <summary>
            /// With this setting, both the pose's rotation and position will be applied to the parent transform
            /// </summary>
            RotationAndPosition,
            /// <summary>
            /// With this setting, only the pose's rotation will be applied to the parent transform
            /// </summary>
            RotationOnly,
            /// <summary>
            /// With this setting, only the pose's position will be applied to the parent transform
            /// </summary>
            PositionOnly
        }

        [SerializeField]
        TrackingType m_TrackingType;
        /// <summary>
        /// The tracking type being used by the tracked pose driver
        /// </summary>
        public TrackingType trackingType
        {
            get { return m_TrackingType; }
            set { m_TrackingType = value; }
        }

        /// <summary>
        /// The update type being used by the tracked pose driver
        /// </summary>
        public enum UpdateType
        {
            /// <summary>
            /// Sample input at both update, and directly before rendering. For smooth head pose tracking, 
            /// we recommend using this value as it will provide the lowest input latency for the device. 
            /// This is the default value for the UpdateType option
            /// </summary>
            UpdateAndBeforeRender,
            /// <summary>
            /// Only sample input during the update phase of the frame.
            /// </summary>
            Update,
            /// <summary>
            /// Only sample input directly before rendering
            /// </summary>
            BeforeRender,
        }

        [SerializeField]
        UpdateType m_UpdateType = UpdateType.UpdateAndBeforeRender;
        /// <summary>
        /// The update type being used by the tracked pose driver
        /// </summary>
        public UpdateType updateType
        {
            get { return m_UpdateType; }
            set { m_UpdateType = value; }
        }

        [SerializeField]
        bool m_UseRelativeTransform = false;
        /// <summary>
        ///  This is used to indicate whether the TrackedPoseDriver will use the object's original transform as its basis.
        /// </summary>
        public bool UseRelativeTransform
        {
            get { return m_UseRelativeTransform; }
            set { m_UseRelativeTransform = value; }
        }

        protected Pose m_OriginPose;

        // originPose is an offset applied to any tracking data read from this object.
        // Setting this value should be reserved for dealing with edge-cases, such as
        // achieving parity between room-scale (floor centered) and stationary (head centered)
        // tracking - without having to alter the transform hierarchy.
        // For user locomotion and gameplay purposes you are usually better off just
        // moving the parent transform of this object.
        public Pose originPose
        {
            get { return m_OriginPose; }
            set { m_OriginPose = value; }
        }

        private void CacheLocalPosition()
        {
            m_OriginPose.position = transform.localPosition;
            m_OriginPose.rotation = transform.localRotation;
        }

        private void ResetToCachedLocalPosition()
        {
            SetLocalTransform(m_OriginPose.position, m_OriginPose.rotation, PoseDataFlags.Position | PoseDataFlags.Rotation);
        }

        protected virtual void Awake()
        {
            CacheLocalPosition();

            if (HasStereoCamera())
            {
#if ENABLE_VR
                XRDevice.DisableAutoXRCameraTracking(GetComponent<Camera>(), true);
#endif
            }
        }

        protected virtual void OnDestroy()
        {
            if (HasStereoCamera())
            {
#if ENABLE_VR
                XRDevice.DisableAutoXRCameraTracking(GetComponent<Camera>(), false);
#endif
            }
        }

        protected virtual void OnEnable()
        {
            Application.onBeforeRender += OnBeforeRender;
        }

        protected virtual void OnDisable()
        {
            // remove delegate registration            
            ResetToCachedLocalPosition();
            Application.onBeforeRender -= OnBeforeRender;
        }

        protected virtual void FixedUpdate()
        {
            if (m_UpdateType == UpdateType.Update ||
                m_UpdateType == UpdateType.UpdateAndBeforeRender)
            {
                PerformUpdate();
            }
        }

        protected virtual void Update()
        {
            if (m_UpdateType == UpdateType.Update ||
                m_UpdateType == UpdateType.UpdateAndBeforeRender)
            {
                PerformUpdate();
            }
        }

        protected virtual void OnBeforeRender()
        {
            if (m_UpdateType == UpdateType.BeforeRender ||
                m_UpdateType == UpdateType.UpdateAndBeforeRender)
            {
                PerformUpdate();
            }
        }

        protected virtual void SetLocalTransform(Vector3 newPosition, Quaternion newRotation, PoseDataFlags poseFlags)
        {
            if ((m_TrackingType == TrackingType.RotationAndPosition ||
                m_TrackingType == TrackingType.RotationOnly) && 
                (poseFlags & PoseDataFlags.Rotation) > 0)
            {
                transform.localRotation = newRotation;
            }

            if ((m_TrackingType == TrackingType.RotationAndPosition ||
                m_TrackingType == TrackingType.PositionOnly) &&
                (poseFlags & PoseDataFlags.Position) > 0)
            {
                transform.localPosition = newPosition;
            }
        }

        protected Pose TransformPoseByOriginIfNeeded(Pose pose)
        {
            if (m_UseRelativeTransform)
            {
                return pose.GetTransformedBy(m_OriginPose);
            }
            else
            {
                return pose;
            }
        }

        private bool HasStereoCamera()
        {
            Camera camera = GetComponent<Camera>();
            return camera != null && camera.stereoEnabled;
        }

        protected virtual void PerformUpdate()
        {
            if (!enabled)
                return;
            Pose currentPose = new Pose();
            currentPose = Pose.identity;
            PoseDataFlags poseFlags = GetPoseData(m_Device, m_PoseSource, out currentPose);
            if(poseFlags != PoseDataFlags.NoData)
            {
                Pose localPose = TransformPoseByOriginIfNeeded(currentPose);
                SetLocalTransform(localPose.position, localPose.rotation, poseFlags);
            }
        }
    }
}
