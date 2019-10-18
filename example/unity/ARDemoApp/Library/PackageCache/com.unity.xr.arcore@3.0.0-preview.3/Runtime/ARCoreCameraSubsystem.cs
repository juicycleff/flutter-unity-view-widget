using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// The camera subsystem implementation for ARCore.
    /// </summary>
    [Preserve]
    public sealed class ARCoreCameraSubsystem : XRCameraSubsystem
    {
        /// <summary>
        /// The identifying name for the camera-providing implementation.
        /// </summary>
        /// <value>
        /// The identifying name for the camera-providing implementation.
        /// </value>
        const string k_SubsystemId = "ARCore-Camera";

        /// <summary>
        /// The name for the shader for rendering the camera texture.
        /// </summary>
        /// <value>
        /// The name for the shader for rendering the camera texture.
        /// </value>
        const string k_DefaultBackgroundShaderName = "Unlit/ARCoreBackground";

        enum CameraConfigurationResult
        {
            /// <summary>
            /// Setting the camera configuration was successful.
            /// </summary>
            Success = 0,

            /// <summary>
            /// The given camera configuration was not valid to be set by the provider.
            /// </summary>
            InvalidCameraConfiguration = 1,

            /// <summary>
            /// The provider session was invalid.
            /// </summary>
            InvalidSession = 2,

            /// <summary>
            /// An error occurred because the user did not dispose of all <c>XRCameraImages</c> and did not allow all
            /// asynchronous conversion jobs complete before changing the camera configuration.
            /// </summary>
            ErrorImagesNotDisposed = 3,
        }

        /// <summary>
        /// The name for the background shader based on the current render pipeline.
        /// </summary>
        /// <value>
        /// The name for the background shader based on the current render pipeline. Or, <c>null</c> if the current
        /// render pipeline is incompatible with the set of shaders.
        /// </value>
        /// <remarks>
        /// The value for the <c>GraphicsSettings.renderPipelineAsset</c> is not expected to change within the lifetime
        /// of the application.
        /// </remarks>
        public static string backgroundShaderName => k_DefaultBackgroundShaderName;

        /// <summary>
        /// Create and register the camera subsystem descriptor to advertise a providing implementation for camera
        /// functionality.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            XRCameraSubsystemCinfo cameraSubsystemCinfo = new XRCameraSubsystemCinfo
            {
                id = k_SubsystemId,
                implementationType = typeof(ARCoreCameraSubsystem),
                supportsAverageBrightness = true,
                supportsAverageColorTemperature = false,
                supportsColorCorrection = true,
                supportsDisplayMatrix = true,
                supportsProjectionMatrix = true,
                supportsTimestamp = true,
                supportsCameraConfigurations = true,
                supportsCameraImage = true,
                supportsAverageIntensityInLumens = false
            };

            if (!XRCameraSubsystem.Register(cameraSubsystemCinfo))
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", k_SubsystemId);
            }
#endif
        }

        /// <summary>
        /// Create the ARCore camera functionality provider for the camera subsystem.
        /// </summary>
        /// <returns>
        /// The ARCore camera functionality provider for the camera subsystem.
        /// </returns>
        protected override Provider CreateProvider() => new ARCoreProvider();

        /// <summary>
        /// Provides the camera functionality for the ARCore implementation.
        /// </summary>
        class ARCoreProvider : Provider
        {
            /// <summary>
            /// The shader property name for the main texture of the camera video frame.
            /// </summary>
            /// <value>
            /// The shader property name for the main texture of the camera video frame.
            /// </value>
            const string k_MainTexPropertyName = "_MainTex";

            /// <summary>
            /// The name of the camera permission for Android.
            /// </summary>
            /// <value>
            /// The name of the camera permission for Android.
            /// </value>
            const string k_CameraPermissionName = "android.permission.CAMERA";

            /// <summary>
            /// The shader property name identifier for the main texture of the camera video frame.
            /// </summary>
            /// <value>
            /// The shader property name identifier for the main texture of the camera video frame.
            /// </value>
            static readonly int k_MainTexPropertyNameId = Shader.PropertyToID(k_MainTexPropertyName);

            /// <summary>
            /// Get the material used by <c>XRCameraSubsystem</c> to render the camera texture.
            /// </summary>
            /// <returns>
            /// The material to render the camera texture.
            /// </returns>
            public override Material cameraMaterial => m_CameraMaterial;
            Material m_CameraMaterial;

            /// <summary>
            /// Determine whether camera permission has been granted.
            /// </summary>
            /// <returns>
            /// <c>true</c> if camera permission has been granted for this app. Otherwise, <c>false</c>.
            /// </returns>
            public override bool permissionGranted => ARCorePermissionManager.IsPermissionGranted(k_CameraPermissionName);

            public override bool invertCulling => NativeApi.UnityARCore_Camera_ShouldInvertCulling();

            /// <summary>
            /// Construct the camera functionality provider for ARCore.
            /// </summary>
            public ARCoreProvider()
            {
                NativeApi.UnityARCore_Camera_Construct(k_MainTexPropertyNameId);
                m_CameraMaterial = CreateCameraMaterial(ARCoreCameraSubsystem.backgroundShaderName);
            }

            /// <summary>
            /// Start the camera functionality.
            /// </summary>
            public override void Start() => NativeApi.UnityARCore_Camera_Start();

            /// <summary>
            /// Stop the camera functionality.
            /// </summary>
            public override void Stop() => NativeApi.UnityARCore_Camera_Stop();

            /// <summary>
            /// Destroy any resources required for the camera functionality.
            /// </summary>
            public override void Destroy() => NativeApi.UnityARCore_Camera_Destruct();

            /// <summary>
            /// Get the camera frame for the subsystem.
            /// </summary>
            /// <param name="cameraParams">The current Unity <c>Camera</c> parameters.</param>
            /// <param name="cameraFrame">The current camera frame returned by the method.</param>
            /// <returns>
            /// <c>true</c> if the method successfully got a frame. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
            {
                return NativeApi.UnityARCore_Camera_TryGetFrame(cameraParams, out cameraFrame);
            }

            /// <summary>
            /// Set the focus mode for the camera.
            /// </summary>
            /// <param name="cameraFocusMode">The focus mode to set for the camera.</param>
            /// <returns>
            /// <c>true</c> if the method successfully set the focus mode for the camera. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TrySetFocusMode(CameraFocusMode cameraFocusMode)
            {
                return NativeApi.UnityARCore_Camera_TrySetFocusMode(cameraFocusMode);
            }

            /// <summary>
            /// Set the light estimation mode.
            /// </summary>
            /// <param name="lightEstimationMode">The light estimation mode to set.</param>
            /// <returns>
            /// <c>true</c> if the method successfully set the light estimation mode. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TrySetLightEstimationMode(LightEstimationMode lightEstimationMode)
            {
                return NativeApi.UnityARCore_Camera_TrySetLightEstimationMode(lightEstimationMode);
            }

            /// <summary>
            /// Get the camera intrinisics information.
            /// </summary>
            /// <param name="cameraIntrinsics">The camera intrinsics information returned from the method.</param>
            /// <returns>
            /// <c>true</c> if the method successfully gets the camera intrinsics information. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics)
            {
                return NativeApi.UnityARCore_Camera_TryGetIntrinsics(out cameraIntrinsics);
            }

            /// <summary>
            /// Queries the supported camera configurations.
            /// </summary>
            /// <param name="defaultCameraConfiguration">A default value used to fill the returned array before copying
            /// in real values. This ensures future additions to this struct are backwards compatible.</param>
            /// <param name="allocator">The allocation strategy to use for the returned data.</param>
            /// <returns>
            /// The supported camera configurations.
            /// </returns>
            public override NativeArray<XRCameraConfiguration> GetConfigurations(XRCameraConfiguration defaultCameraConfiguration,
                                                                                 Allocator allocator)
            {
                int configurationsCount;
                int configurationSize;
                IntPtr configurations = NativeApi.UnityARCore_Camera_AcquireConfigurations(out configurationsCount,
                                                                                           out configurationSize);
                try
                {
                    unsafe
                    {
                        return NativeCopyUtility.PtrToNativeArrayWithDefault(defaultCameraConfiguration,
                                                                             (void*)configurations,
                                                                             configurationSize, configurationsCount,
                                                                             allocator);
                    }
                }
                finally
                {
                    NativeApi.UnityARCore_Camera_ReleaseConfigurations(configurations);
                }
            }

            /// <summary>
            /// The current camera configuration.
            /// </summary>
            /// <value>
            /// The current camera configuration if it exists. Otherise, <c>null</c>.
            /// </value>
            /// <exception cref="System.ArgumentException">Thrown when setting the current configuration if the given
            /// configuration is not a valid, supported camera configuration.</exception>
            /// <exception cref="System.InvalidOperationException">Thrown when setting the current configuration if the
            /// implementation is unable to set the current camera configuration for various reasons such as:
            /// <list type="bullet">
            /// <item><description>ARCore session is invalid</description></item>
            /// <item><description>Captured <c>XRCameraImages</c> have not been disposed</description></item>
            /// </list>
            /// </exception>
            /// <seealso cref="TryAcquireLatestImage"/>
            public override XRCameraConfiguration? currentConfiguration
            {
                get
                {
                    XRCameraConfiguration cameraConfiguration;
                    if (NativeApi.UnityARCore_Camera_TryGetCurrentConfiguration(out cameraConfiguration))
                    {
                        return cameraConfiguration;
                    }

                    return null;
                }
                set
                {
                    // Assert that the camera configuration is not null.
                    // The XRCameraSubsystem should have already checked this.
                    Debug.Assert(value != null, "Cannot set the current camera configuration to null");

                    switch (NativeApi.UnityARCore_Camera_TrySetCurrentConfiguration((XRCameraConfiguration)value))
                    {
                        case CameraConfigurationResult.Success:
                            break;
                        case CameraConfigurationResult.InvalidCameraConfiguration:
                            throw new ArgumentException("Camera configuration does not exist in the available "
                                                        + "configurations", "value");
                        case CameraConfigurationResult.InvalidSession:
                            throw new InvalidOperationException("Cannot set camera configuration because the ARCore "
                                                                + "session is not valid");
                        case CameraConfigurationResult.ErrorImagesNotDisposed:
                            throw new InvalidOperationException("Cannot set camera configuration because you have not "
                                                                + "disposed of all XRCameraImages and allowed all "
                                                                + "asynchronous conversion jobs to complete");
                        default:
                            throw new InvalidOperationException("cannot set camera configuration for ARCore");
                    }
                }
            }

            /// <summary>
            /// Gets the texture descriptors associated with the camera image.
            /// </summary>
            /// <returns>The texture descriptors.</returns>
            /// <param name="defaultDescriptor">Default descriptor.</param>
            /// <param name="allocator">Allocator.</param>
            public unsafe override NativeArray<XRTextureDescriptor> GetTextureDescriptors(
                XRTextureDescriptor defaultDescriptor,
                Allocator allocator)
            {
                int length, elementSize;
                var textureDescriptors = NativeApi.UnityARCore_Camera_AcquireTextureDescriptors(
                    out length, out elementSize);

                try
                {
                    return NativeCopyUtility.PtrToNativeArrayWithDefault(
                        defaultDescriptor,
                        textureDescriptors, elementSize, length, allocator);
                }
                finally
                {
                    NativeApi.UnityARCore_Camera_ReleaseTextureDescriptors(textureDescriptors);
                }
            }

            /// <summary>
            /// Query for the latest native camera image.
            /// </summary>
            /// <param name="cameraImageCinfo">The metadata required to construct a <see cref="XRCameraImage"/></param>
            /// <returns>
            /// <c>true</c> if the camera image is acquired. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TryAcquireLatestImage(out CameraImageCinfo cameraImageCinfo)
            {
                return NativeApi.UnityARCore_Camera_TryAcquireLatestImage(out cameraImageCinfo);
            }

            /// <summary>
            /// Get the status of an existing asynchronous conversion request.
            /// </summary>
            /// <param name="requestId">The unique identifier associated with a request.</param>
            /// <returns>The state of the request.</returns>
            /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
            public override AsyncCameraImageConversionStatus GetAsyncRequestStatus(int requestId)
            {
                return NativeApi.UnityARCore_Camera_GetAsyncRequestStatus(requestId);
            }

            /// <summary>
            /// Dispose an existing native image identified by <paramref name="nativeHandle"/>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for this camera image.</param>
            /// <seealso cref="TryAcquireLatestImage"/>
            public override void DisposeImage(int nativeHandle)
            {
                NativeApi.UnityARCore_Camera_DisposeImage(nativeHandle);
            }

            /// <summary>
            /// Dispose an existing async conversion request.
            /// </summary>
            /// <param name="requestId">A unique identifier for the request.</param>
            /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
            public override void DisposeAsyncRequest(int requestId)
            {
                NativeApi.UnityARCore_Camera_DisposeAsyncRequest(requestId);
            }

            /// <summary>
            /// Get information about an image plane from a native image handle by index.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for this camera image.</param>
            /// <param name="planeIndex">The index of the plane to get.</param>
            /// <param name="planeCinfo">The returned camera plane information if successful.</param>
            /// <returns>
            /// <c>true</c> if the image plane was acquired. Otherwise, <c>false</c>.
            /// </returns>
            /// <seealso cref="TryAcquireLatestImage"/>
            public override bool TryGetPlane(
                int nativeHandle,
                int planeIndex,
                out CameraImagePlaneCinfo planeCinfo)
            {
                return NativeApi.UnityARCore_Camera_TryGetPlane(nativeHandle, planeIndex, out planeCinfo);
            }

            /// <summary>
            /// Determine whether a native image handle returned by <see cref="TryAcquireLatestImage"/> is currently
            /// valid. An image may become invalid if it has been disposed.
            /// </summary>
            /// <remarks>
            /// If a handle is valid, <see cref="TryConvert"/> and <see cref="TryGetConvertedDataSize"/> should not fail.
            /// </remarks>
            /// <param name="nativeHandle">A unique identifier for the camera image in question.</param>
            /// <returns><c>true</c>, if it is a valid handle. Otherwise, <c>false</c>.</returns>
            /// <seealso cref="DisposeImage"/>
            public override bool NativeHandleValid(
                int nativeHandle)
            {

                return NativeApi.UnityARCore_Camera_HandleValid(nativeHandle);
            }

            /// <summary>
            /// Get the number of bytes required to store an image with the iven dimensions and <c>TextureFormat</c>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="dimensions">The dimensions of the output image.</param>
            /// <param name="format">The <c>TextureFormat</c> for the image.</param>
            /// <param name="size">The number of bytes required to store the converted image.</param>
            /// <returns><c>true</c> if the output <paramref name="size"/> was set.</returns>
            public override bool TryGetConvertedDataSize(
                int nativeHandle,
                Vector2Int dimensions,
                TextureFormat format,
                out int size)
            {
                return NativeApi.UnityARCore_Camera_TryGetConvertedDataSize(nativeHandle, dimensions, format, out size);
            }

            /// <summary>
            /// Convert the image with handle <paramref name="nativeHandle"/> using the provided
            /// <paramref cref="conversionParams"/>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="conversionParams">The parameters to use during the conversion.</param>
            /// <param name="destinationBuffer">A buffer to write the converted image to.</param>
            /// <param name="bufferLength">The number of bytes available in the buffer.</param>
            /// <returns>
            /// <c>true</c> if the image was converted and stored in <paramref name="destinationBuffer"/>.
            /// </returns>
            public override bool TryConvert(
                int nativeHandle,
                XRCameraImageConversionParams conversionParams,
                IntPtr destinationBuffer,
                int bufferLength)
            {
                return NativeApi.UnityARCore_Camera_TryConvert(
                    nativeHandle, conversionParams, destinationBuffer, bufferLength);
            }

            /// <summary>
            /// Create an asynchronous request to convert a camera image, similar to <see cref="TryConvert"/> except
            /// the conversion should happen on a thread other than the calling (main) thread.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="conversionParams">The parameters to use during the conversion.</param>
            /// <returns>A unique identifier for this request.</returns>
            public override int ConvertAsync(
                int nativeHandle,
                XRCameraImageConversionParams conversionParams)
            {
                return NativeApi.UnityARCore_Camera_CreateAsyncConversionRequest(nativeHandle, conversionParams);
            }

            /// <summary>
            /// Get a pointer to the image data from a completed asynchronous request. This method should only succeed
            /// if <see cref="GetAsyncRequestStatus"/> returns <see cref="AsyncCameraImageConversionStatus.Ready"/>.
            /// </summary>
            /// <param name="requestId">The unique identifier associated with a request.</param>
            /// <param name="dataPtr">A pointer to the native buffer containing the data.</param>
            /// <param name="dataLength">The number of bytes in <paramref name="dataPtr"/>.</param>
            /// <returns><c>true</c> if <paramref name="dataPtr"/> and <paramref name="dataLength"/> were set and point
            ///  to the image data.</returns>
            public override bool TryGetAsyncRequestData(int requestId, out IntPtr dataPtr, out int dataLength)
            {
                return NativeApi.UnityARCore_Camera_TryGetAsyncRequestData(requestId, out dataPtr, out dataLength);
            }

            /// <summary>
            /// Similar to <see cref="ConvertAsync(int, XRCameraImageConversionParams)"/> but takes a delegate to
            /// invoke when the request is complete, rather than returning a request id.
            /// </summary>
            /// <remarks>
            /// If the first parameter to <paramref name="callback"/> is
            /// <see cref="AsyncCameraImageConversionStatus.Ready"/> then the <c>dataPtr</c> parameter must be valid
            /// for the duration of the invocation. The data may be destroyed immediately upon return. The
            /// <paramref name="context"/> parameter must be passed back to the <paramref name="callback"/>.
            /// </remarks>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="conversionParams">The parameters to use during the conversion.</param>
            /// <param name="callback">A delegate which must be invoked when the request is complete, whether the
            /// conversion was successfully or not.</param>
            /// <param name="context">A native pointer which must be passed back unaltered to
            /// <paramref name="callback"/>.</param>
            public override void ConvertAsync(
                int nativeHandle,
                XRCameraImageConversionParams conversionParams,
                OnImageRequestCompleteDelegate callback,
                IntPtr context)
            {
                NativeApi.UnityARCore_Camera_CreateAsyncConversionRequestWithCallback(
                    nativeHandle, conversionParams, callback, context);
            }
        }

        /// <summary>
        /// Container to wrap the native ARCore camera APIs.
        /// </summary>
        static class NativeApi
        {
            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_Construct(int mainTexPropertyNameId);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_Destruct();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_Start();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_Stop();

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryGetFrame(XRCameraParams cameraParams,
                                                                    out XRCameraFrame cameraFrame);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TrySetFocusMode(CameraFocusMode cameraFocusMode);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TrySetLightEstimationMode(LightEstimationMode lightEstimationMode);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics);

            [DllImport("UnityARCore")]
            public static extern IntPtr UnityARCore_Camera_AcquireConfigurations(out int configurationsCount,
                                                                                 out int configurationSize);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_ReleaseConfigurations(IntPtr configurations);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryGetCurrentConfiguration(out XRCameraConfiguration cameraConfiguration);

            [DllImport("UnityARCore")]
            public static extern CameraConfigurationResult UnityARCore_Camera_TrySetCurrentConfiguration(XRCameraConfiguration cameraConfiguration);

            [DllImport("UnityARCore")]
            public static unsafe extern void* UnityARCore_Camera_AcquireTextureDescriptors(
                out int length, out int elementSize);

            [DllImport("UnityARCore")]
            public static unsafe extern void UnityARCore_Camera_ReleaseTextureDescriptors(
                void* descriptors);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_ShouldInvertCulling();

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryAcquireLatestImage(out CameraImageCinfo cameraImageCinfo);

            [DllImport("UnityARCore")]
            public static extern AsyncCameraImageConversionStatus
                UnityARCore_Camera_GetAsyncRequestStatus(int requestId);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_DisposeImage(
                int nativeHandle);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_DisposeAsyncRequest(
                int requestHandle);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryGetPlane(int nativeHandle, int planeIndex,
                                                                     out CameraImagePlaneCinfo planeCinfo);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_HandleValid(
                int nativeHandle);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryGetConvertedDataSize(
                int nativeHandle, Vector2Int dimensions, TextureFormat format, out int size);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryConvert(
                int nativeHandle, XRCameraImageConversionParams conversionParams,
                IntPtr buffer, int bufferLength);

            [DllImport("UnityARCore")]
            public static extern int UnityARCore_Camera_CreateAsyncConversionRequest(
                int nativeHandle, XRCameraImageConversionParams conversionParams);

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_Camera_TryGetAsyncRequestData(
                int requestHandle, out IntPtr dataPtr, out int dataLength);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_Camera_CreateAsyncConversionRequestWithCallback(
                int nativeHandle, XRCameraImageConversionParams conversionParams,
                XRCameraSubsystem.OnImageRequestCompleteDelegate callback, IntPtr context);
        }
    }
}
