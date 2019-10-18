using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;
#if MODULE_URP_ENABLED
using UnityEngine.Rendering.Universal;
#elif MODULE_LWRP_ENABLED
using UnityEngine.Rendering.LWRP;
#endif
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The camera subsystem implementation for ARKit.
    /// </summary>
    [Preserve]
    public sealed class ARKitCameraSubsystem : XRCameraSubsystem
    {
        /// <summary>
        /// The identifying name for the camera-providing implementation.
        /// </summary>
        /// <value>
        /// The identifying name for the camera-providing implementation.
        /// </value>
        const string k_SubsystemId = "ARKit-Camera";

        /// <summary>
        /// The name for the shader for rendering the camera texture in the legacy render pipeline.
        /// </summary>
        /// <value>
        /// The name for the shader for rendering the camera texture in the legacy render pipeline.
        /// </value>
        const string k_BackgroundLegacyRPShaderName = "Unlit/ARKitBackground";

#if MODULE_URP_ENABLED
        /// <summary>
        /// The name for the shader for rendering the camera texture in the universal render pipeline.
        /// </summary>
        /// <value>
        /// The name for the shader for rendering the camera texture in the universal render pipeline.
        /// </value>
        const string k_BackgroundUniversalRPShaderName = "Unlit/ARKitURPBackground";

#elif MODULE_LWRP_ENABLED
        /// <summary>
        /// The name for the shader for rendering the camera texture in the lightweight render pipeline.
        /// </summary>
        /// <value>
        /// The name for the shader for rendering the camera texture in the lightweight render pipeline.
        /// </value>
        const string k_BackgroundLightweightRPShaderName = "Unlit/ARKitLWRPBackground";
#endif

        /// <summary>
        /// Resulting values from setting the camera configuration.
        /// </summary>
        enum CameraConfigurationResult
        {
            /// <summary>
            /// Setting the camera configuration was successful.
            /// </summary>
            Success = 0,

            /// <summary>
            /// Setting camera configuration was not supported by the provider.
            /// </summary>
            Unsupported = 1,

            /// <summary>
            /// The given camera configuration was not valid to be set by the provider.
            /// </summary>
            InvalidCameraConfiguration = 2,

            /// <summary>
            /// The provider session was invalid.
            /// </summary>
            InvalidSession = 3,
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
        public static string backgroundShaderName
        {
            get
            {
                if (GraphicsSettings.renderPipelineAsset == null)
                {
                    return k_BackgroundLegacyRPShaderName;
                }
#if MODULE_URP_ENABLED
                else if (GraphicsSettings.renderPipelineAsset is UniversalRenderPipelineAsset)
                {
                    return k_BackgroundUniversalRPShaderName;
                }
#elif MODULE_LWRP_ENABLED
                else if (GraphicsSettings.renderPipelineAsset is LightweightRenderPipelineAsset)
                {
                    return k_BackgroundLightweightRPShaderName;
                }
#endif
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Create and register the camera subsystem descriptor to advertise a providing implementation for camera
        /// functionality.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_IOS && !UNITY_EDITOR
            XRCameraSubsystemCinfo cameraSubsystemCinfo = new XRCameraSubsystemCinfo
            {
                id = k_SubsystemId,
                implementationType = typeof(ARKitCameraSubsystem),
                supportsAverageBrightness = false,
                supportsAverageColorTemperature = true,
                supportsColorCorrection = false,
                supportsDisplayMatrix = true,
                supportsProjectionMatrix = true,
                supportsTimestamp = true,
                supportsCameraConfigurations = true,
                supportsCameraImage = true,
                supportsAverageIntensityInLumens = true
            };

            if (!XRCameraSubsystem.Register(cameraSubsystemCinfo))
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", k_SubsystemId);
            }
#endif // UNITY_IOS && !UNITY_EDITOR
        }

        /// <summary>
        /// Create the ARKit camera functionality provider for the camera subsystem.
        /// </summary>
        /// <returns>
        /// The ARKit camera functionality provider for the camera subsystem.
        /// </returns>
        protected override Provider CreateProvider() => new ARKitProvider();

        /// <summary>
        /// Provides the camera functionality for the ARKit implementation.
        /// </summary>
        class ARKitProvider : Provider
        {
            /// <summary>
            /// The shader property name for the luminance component of the camera video frame.
            /// </summary>
            /// <value>
            /// The shader property name for the luminance component of the camera video frame.
            /// </value>
            const string k_TextureYPropertyName = "_textureY";

            /// <summary>
            /// The shader property name for the chrominance components of the camera video frame.
            /// </summary>
            /// <value>
            /// The shader property name for the chrominance components of the camera video frame.
            /// </value>
            const string k_TextureCbCrPropertyName = "_textureCbCr";

            /// <summary>
            /// The shader property name identifier for the luminance component of the camera video frame.
            /// </summary>
            /// <value>
            /// The shader property name identifier for the luminance component of the camera video frame.
            /// </value>
            static readonly int k_TextureYPropertyNameId = Shader.PropertyToID(k_TextureYPropertyName);

            /// <summary>
            /// The shader property name identifier for the chrominance components of the camera video frame.
            /// </summary>
            /// <value>
            /// The shader property name identifier for the chrominance components of the camera video frame.
            /// </value>
            static readonly int k_TextureCbCrPropertyNameId = Shader.PropertyToID(k_TextureCbCrPropertyName);

            /// <summary>
            /// Get the material used by <c>XRCameraSubsystem</c> to render the camera texture.
            /// </summary>
            /// <returns>
            /// The material to render the camera texture.
            /// </returns>
            public override Material cameraMaterial => m_CameraMaterial;
            Material m_CameraMaterial;

            /// <summary>
            /// Whether camera permission has been granted.
            /// </summary>
            /// <value>
            /// <c>true</c> if camera permission has been granted for this app. Otherwise, <c>false</c>.
            /// </value>
            public override bool permissionGranted => NativeApi.UnityARKit_Camera_IsCameraPermissionGranted();

            /// <summary>
            /// Constructs the ARKit camera functionality provider.
            /// </summary>
            public ARKitProvider()
            {
                NativeApi.UnityARKit_Camera_Construct(k_TextureYPropertyNameId,
                                                      k_TextureCbCrPropertyNameId);

                string shaderName = ARKitCameraSubsystem.backgroundShaderName;
                if (shaderName == null)
                {
                    Debug.LogError("Cannot create camera background material compatible with the render pipeline");
                }
                else
                {
                    m_CameraMaterial = CreateCameraMaterial(shaderName);
                }
            }

            /// <summary>
            /// Start the camera functionality.
            /// </summary>
            public override void Start() => NativeApi.UnityARKit_Camera_Start();

            /// <summary>
            /// Stop the camera functionality.
            /// </summary>
            public override void Stop() => NativeApi.UnityARKit_Camera_Stop();

            /// <summary>
            /// Destroy any resources required for the camera functionality.
            /// </summary>
            public override void Destroy() => NativeApi.UnityARKit_Camera_Destruct();

            /// <summary>
            /// Get the current camera frame for the subsystem.
            /// </summary>
            /// <param name="cameraParams">The current Unity <c>Camera</c> parameters.</param>
            /// <param name="cameraFrame">The current camera frame returned by the method.</param>
            /// <returns>
            /// <c>true</c> if the method successfully got a frame. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
            {
                return NativeApi.UnityARKit_Camera_TryGetFrame(cameraParams, out cameraFrame);
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
                return NativeApi.UnityARKit_Camera_TrySetFocusMode(cameraFocusMode);
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
                return NativeApi.UnityARKit_Camera_TrySetLightEstimationMode(lightEstimationMode);
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
                return NativeApi.UnityARKit_Camera_TryGetIntrinsics(out cameraIntrinsics);
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
                IntPtr configurations = NativeApi.UnityARKit_Camera_AcquireConfigurations(out configurationsCount,
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
                    NativeApi.UnityARKit_Camera_ReleaseConfigurations(configurations);
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
            /// <item><description>Version of iOS does not support camera configurations</description></item>
            /// <item><description>ARKit session is invalid</description></item>
            /// </list>
            /// </exception>
            public override XRCameraConfiguration? currentConfiguration
            {
                get
                {
                    XRCameraConfiguration cameraConfiguration;
                    if (NativeApi.UnityARKit_Camera_TryGetCurrentConfiguration(out cameraConfiguration))
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

                    switch (NativeApi.UnityARKit_Camera_TrySetCurrentConfiguration((XRCameraConfiguration)value))
                    {
                        case CameraConfigurationResult.Success:
                            break;
                        case CameraConfigurationResult.Unsupported:
                            throw new InvalidOperationException("cannot set camera configuration because ARKit version "
                                                                + "does not support camera configurations");
                        case CameraConfigurationResult.InvalidCameraConfiguration:
                            throw new ArgumentException("camera configuration does not exist in the available "
                                                        + "configurations", "value");
                        case CameraConfigurationResult.InvalidSession:
                            throw new InvalidOperationException("cannot set camera configuration because the ARKit "
                                                                + "session is not valid");
                        default:
                            throw new InvalidOperationException("cannot set camera configuration for ARKit");
                    }
                }
            }

            /// <summary>
            /// Gets the texture descriptors associated with th current camera
            /// frame.
            /// </summary>
            /// <param name="defaultDescriptor">Default descriptor.</param>
            /// <param name="allocator">Allocator.</param>
            /// <returns>The texture descriptors.</returns>
            public unsafe override NativeArray<XRTextureDescriptor> GetTextureDescriptors(
                XRTextureDescriptor defaultDescriptor,
                Allocator allocator)
            {
                int length, elementSize;
                var textureDescriptors = NativeApi.UnityARKit_Camera_AcquireTextureDescriptors(
                    out length, out elementSize);

                try
                {
                    return NativeCopyUtility.PtrToNativeArrayWithDefault(
                        defaultDescriptor,
                        textureDescriptors, elementSize, length, allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_Camera_ReleaseTextureDescriptors(textureDescriptors);
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
                return NativeApi.UnityARKit_Camera_TryAcquireLatestImage(out cameraImageCinfo);
            }

            /// <summary>
            /// Get the status of an existing asynchronous conversion request.
            /// </summary>
            /// <param name="requestId">The unique identifier associated with a request.</param>
            /// <returns>The state of the request.</returns>
            /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
            public override AsyncCameraImageConversionStatus GetAsyncRequestStatus(int requestId)
            {
                return NativeApi.UnityARKit_Camera_GetAsyncRequestStatus(requestId);
            }

            /// <summary>
            /// Dispose an existing native image identified by <paramref name="nativeHandle"/>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for this camera image.</param>
            /// <seealso cref="TryAcquireLatestImage"/>
            public override void DisposeImage(int nativeHandle) => NativeApi.UnityARKit_Camera_DisposeImage(nativeHandle);

            /// <summary>
            /// Dispose an existing async conversion request.
            /// </summary>
            /// <param name="requestId">A unique identifier for the request.</param>
            /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
            public override void DisposeAsyncRequest(int requestId) => NativeApi.UnityARKit_Camera_DisposeAsyncRequest(requestId);

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
               return NativeApi.UnityARKit_Camera_TryGetPlane(nativeHandle, planeIndex, out planeCinfo);
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
            public override bool NativeHandleValid(int nativeHandle) => NativeApi.UnityARKit_Camera_HandleValid(nativeHandle);

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
                return NativeApi.UnityARKit_Camera_TryGetConvertedDataSize(nativeHandle, dimensions, format, out size);
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
                return NativeApi.UnityARKit_Camera_TryConvert(
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
                return NativeApi.UnityARKit_Camera_CreateAsyncConversionRequest(nativeHandle, conversionParams);
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
                return NativeApi.UnityARKit_Camera_TryGetAsyncRequestData(requestId, out dataPtr, out dataLength);
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
                NativeApi.UnityARKit_Camera_CreateAsyncConversionRequestWithCallback(
                    nativeHandle, conversionParams, callback, context);
            }

        }

        /// <summary>
        /// Container to wrap the native ARKit camera APIs.
        /// </summary>
        static class NativeApi
        {
            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Construct(int textureYPropertyNameId,
                                                                  int textureCbCrPropertyNameId);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Destruct();

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Start();

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Stop();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetFrame(XRCameraParams cameraParams,
                                                                    out XRCameraFrame cameraFrame);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TrySetFocusMode(CameraFocusMode cameraFocusMode);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TrySetLightEstimationMode(LightEstimationMode lightEstimationMode);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_IsCameraPermissionGranted();

            [DllImport("__Internal")]
            public static extern IntPtr UnityARKit_Camera_AcquireConfigurations(out int configurationsCount,
                                                                                out int configurationSize);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_ReleaseConfigurations(IntPtr configurations);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetCurrentConfiguration(out XRCameraConfiguration cameraConfiguration);

            [DllImport("__Internal")]
            public static extern CameraConfigurationResult UnityARKit_Camera_TrySetCurrentConfiguration(XRCameraConfiguration cameraConfiguration);

            [DllImport("__Internal")]
            public static unsafe extern void* UnityARKit_Camera_AcquireTextureDescriptors(
                out int length, out int elementSize);

            [DllImport("__Internal")]
            public static unsafe extern void UnityARKit_Camera_ReleaseTextureDescriptors(
                void* descriptors);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryAcquireLatestImage(out CameraImageCinfo cameraImageCinfo);

            [DllImport("__Internal")]
            public static extern AsyncCameraImageConversionStatus
                UnityARKit_Camera_GetAsyncRequestStatus(int requestId);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_DisposeImage(
                int nativeHandle);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_DisposeAsyncRequest(
                int requestHandle);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetPlane(int nativeHandle, int planeIndex,
                                                                    out CameraImagePlaneCinfo planeCinfo);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_HandleValid(
                int nativeHandle);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetConvertedDataSize(
                int nativeHandle, Vector2Int dimensions, TextureFormat format, out int size);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryConvert(
                int nativeHandle, XRCameraImageConversionParams conversionParams,
                IntPtr buffer, int bufferLength);

            [DllImport("__Internal")]
            public static extern int UnityARKit_Camera_CreateAsyncConversionRequest(
                int nativeHandle, XRCameraImageConversionParams conversionParams);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetAsyncRequestData(
                int requestHandle, out IntPtr dataPtr, out int dataLength);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_CreateAsyncConversionRequestWithCallback(
                int nativeHandle, XRCameraImageConversionParams conversionParams,
                XRCameraSubsystem.OnImageRequestCompleteDelegate callback, IntPtr context);
        }
    }
}
