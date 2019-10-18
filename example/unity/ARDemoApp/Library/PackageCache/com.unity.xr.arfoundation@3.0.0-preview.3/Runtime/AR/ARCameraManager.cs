using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Manages the lifetime of the <c>XRCameraSubsystem</c>. Add one of these to a <c>Camera</c> in your scene
    /// if you want camera texture and light estimation information to be available.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_CameraManager)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARCameraManager.html")]
    public sealed class ARCameraManager : SubsystemLifecycleManager<XRCameraSubsystem, XRCameraSubsystemDescriptor>
    {
        [SerializeField]
        [Tooltip("The focus mode to request on the (physical) AR camera.")]
        CameraFocusMode m_FocusMode = CameraFocusMode.Auto;

        /// <summary>
        /// The <c>CameraFocusMode</c> for the camera.
        /// </summary>
        /// <value>
        /// The focus mode for the camera.
        /// </value>
        public CameraFocusMode focusMode
        {
            get { return m_FocusMode; }
            set
            {
                m_FocusMode = value;
                if (enabled)
                    subsystem.focusMode = focusMode;
            }
        }

        [SerializeField]
        [Tooltip("The light estimation mode for the AR camera.")]
        LightEstimationMode m_LightEstimationMode = LightEstimationMode.Disabled;

        /// <summary>
        /// The <c>LightEstimationMode</c> for the camera.
        /// </summary>
        /// <value>
        /// The light estimation mode for the camera.
        /// </value>
        public LightEstimationMode lightEstimationMode
        {
            get { return m_LightEstimationMode; }
            set
            {
                m_LightEstimationMode = value;
                if (enabled && subsystem != null)
                    subsystem.lightEstimationMode = value;
            }
        }

        /// <summary>
        /// Determines whether camera permission has been granted.
        /// </summary>
        /// <value>
        /// <c>true</c> if permission has been granted. Otherwise, <c>false</c>.
        /// </value>
        public bool permissionGranted
        {
            get
            {
                if (subsystem != null)
                    return subsystem.permissionGranted;

                return false;
            }
        }

        /// <summary>
        /// An event which fires each time a new camera frame is received.
        /// </summary>
        public event Action<ARCameraFrameEventArgs> frameReceived;

        /// <summary>
        /// The material used in background rendering.
        /// </summary>
        /// <value>
        /// The material used in background rendering.
        /// </value>
        public Material cameraMaterial { get => (subsystem == null) ? null : subsystem.cameraMaterial; }

        /// <summary>
        /// Tries to get camera intrinsics. Camera intrinsics refers to properties
        /// of a physical camera which may be useful when performing additional
        /// computer vision processing on the camera image.
        /// </summary>
        /// <param name="cameraIntrinsics">The camera intrinsics to be populated if the camera supports intrinsics
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="cameraIntrinsics"/> was populated. Otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics)
        {
            if (subsystem == null)
            {
                cameraIntrinsics = default(XRCameraIntrinsics);
                return false;
            }

            return subsystem.TryGetIntrinsics(out cameraIntrinsics);
        }

        /// <summary>
        /// Get the camera configurations currently supported for the implementation.
        /// </summary>
        /// <param name="allocator">The allocation strategy to use for the returned data.</param>
        /// <returns>
        /// The supported camera configurations.
        /// </returns>
        public NativeArray<XRCameraConfiguration> GetConfigurations(Allocator allocator)
        {
            return ((subsystem == null) ? new NativeArray<XRCameraConfiguration>(0, allocator)
                    : subsystem.GetConfigurations(allocator));
        }

        /// <summary>
        /// The current camera configuration.
        /// </summary>
        /// <value>
        /// The current camera configuration, if it exists. Otherise, <c>null</c>.
        /// </value>
        /// <exception cref="System.NotSupportedException">Thrown when setting the current configuration if the
        /// implementation does not support camera configurations.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when setting the current configuration if the given
        /// configuration is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Thrown when setting the current configuration if the given
        /// configuration is not a supported camera configuration.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when setting the current configuration if the
        /// implementation is unable to set the current camera configuration.</exception>
        public XRCameraConfiguration? currentConfiguration
        {
            get { return (subsystem == null) ? null : subsystem.currentConfiguration; }
            set
            {
                if (subsystem != null)
                {
                    subsystem.currentConfiguration = value;
                }
            }
        }

        /// <summary>
        /// Attempt to get the latest camera image. This provides directly access to the raw pixel data, as well as
        /// utilities to convert to RGB and Grayscale formats.
        /// </summary>
        /// <remarks>
        /// The <c>XRCameraImage</c> must be disposed to avoid resource leaks.
        /// </remarks>
        /// <param name="cameraImage">A valid <c>XRCameraImage</c> if this method returns <c>true</c>.</param>
        /// <returns>
        /// <c>true</c> if the image was acquired. Otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetLatestImage(out XRCameraImage cameraImage)
        {
            if (subsystem == null)
            {
                cameraImage = default(XRCameraImage);
                return false;
            }

            return subsystem.TryGetLatestImage(out cameraImage);
        }

        void Awake()
        {
            m_Camera = GetComponent<Camera>();
        }

        /// <summary>
        /// Callback before the subsystem is started (but after it is created).
        /// </summary>
        protected override void OnBeforeStart()
        {
            subsystem.focusMode = m_FocusMode;
            subsystem.lightEstimationMode = m_LightEstimationMode;
        }

        void Update()
        {
            if (subsystem == null)
                return;

            var cameraParams = new XRCameraParams
            {
                zNear = m_Camera.nearClipPlane,
                zFar = m_Camera.farClipPlane,
                screenWidth = Screen.width,
                screenHeight = Screen.height,
                screenOrientation = Screen.orientation
            };

            XRCameraFrame frame;
            if (subsystem.TryGetLatestFrame(cameraParams, out frame))
            {
                UpdateTexturesInfos(frame);

                if (frameReceived != null)
                    InvokeFrameReceivedEvent(frame);
            }
        }

        void OnPreRender()
        {
            m_PreRenderInvertCullingValue = GL.invertCulling;
            if (subsystem != null)
                GL.invertCulling = subsystem.invertCulling;
        }

        void OnPostRender()
        {
            GL.invertCulling = m_PreRenderInvertCullingValue;
        }

        /// <summary>
        /// Pull the texture descriptors from the camera subsystem, and update the texture information maintained by
        /// this component.
        /// </summary>
        /// <param name="frame">The latest updated camera frame.</param>
        void UpdateTexturesInfos(XRCameraFrame frame)
        {
            var textureDescriptors = subsystem.GetTextureDescriptors(Allocator.Temp);
            try
            {
                int numUpdated = Math.Min(m_TextureInfos.Count, textureDescriptors.Length);

                // Update the existing textures that are in common between the two arrays.
                for (int i = 0; i < numUpdated; ++i)
                {
                    m_TextureInfos[i] = ARTextureInfo.GetUpdatedTextureInfo(m_TextureInfos[i], textureDescriptors[i]);
                }

                // If there are fewer textures in the current frame than we had previously, destroy any remaining unneeded
                // textures.
                if (numUpdated < m_TextureInfos.Count)
                {
                    for (int i = numUpdated; i < m_TextureInfos.Count; ++i)
                    {
                        m_TextureInfos[i].Reset();
                    }
                    m_TextureInfos.RemoveRange(numUpdated, (m_TextureInfos.Count - numUpdated));
                }
                // Else, if there are more textures in the current frame than we have previously, add new textures for any
                // additional descriptors.
                else if (textureDescriptors.Length > m_TextureInfos.Count)
                {
                    for (int i = numUpdated; i < textureDescriptors.Length; ++i)
                    {
                        m_TextureInfos.Add(new ARTextureInfo(textureDescriptors[i]));
                    }
                }
            }
            finally
            {
                if (textureDescriptors.IsCreated)
                    textureDescriptors.Dispose();
            }
        }

        /// <summary>
        /// Invoke the camera frame received event packing the frame information into the event argument.
        /// <summary>
        /// <param name="frame">The camera frame raising the event.</param>
        void InvokeFrameReceivedEvent(XRCameraFrame frame)
        {
            var lightEstimation = new ARLightEstimationData();

            if (frame.hasAverageBrightness)
                lightEstimation.averageBrightness = frame.averageBrightness;

            if (frame.hasAverageIntensityInLumens)
                lightEstimation.averageIntensityInLumens = frame.averageIntensityInLumens;

            if (frame.hasAverageColorTemperature)
                lightEstimation.averageColorTemperature = frame.averageColorTemperature;

            if (frame.hasColorCorrection)
                lightEstimation.colorCorrection = frame.colorCorrection;

            var eventArgs = new ARCameraFrameEventArgs();

            eventArgs.lightEstimation = lightEstimation;

            if (frame.hasTimestamp)
                eventArgs.timestampNs = frame.timestampNs;

            if (frame.hasProjectionMatrix)
                eventArgs.projectionMatrix = frame.projectionMatrix;

            if (frame.hasDisplayMatrix)
                eventArgs.displayMatrix = frame.displayMatrix;

            if (frame.hasExposureDuration)
                eventArgs.exposureDuration = frame.exposureDuration;

            if (frame.hasExposureOffset)
                eventArgs.exposureOffset = frame.exposureOffset;

            s_Textures.Clear();
            s_PropertyIds.Clear();
            foreach (var textureInfo in m_TextureInfos)
            {
                s_Textures.Add(textureInfo.texture);
                s_PropertyIds.Add(textureInfo.descriptor.propertyNameId);
            }

            eventArgs.textures = s_Textures;
            eventArgs.propertyNameIds = s_PropertyIds;

            frameReceived(eventArgs);
        }

        static List<Texture2D> s_Textures = new List<Texture2D>();

        static List<int> s_PropertyIds = new List<int>();

        readonly List<ARTextureInfo> m_TextureInfos = new List<ARTextureInfo>();

        Camera m_Camera;

        bool m_PreRenderInvertCullingValue;
    }
}
