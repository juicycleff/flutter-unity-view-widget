using System;
using System.Collections.Generic;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Provides access to a device's camera.
    /// </summary>
    /// <remarks>
    /// The <c>XRCameraSubsystem</c> links a Unity <c>Camera</c> to a device camera for video overlay (pass-thru
    /// rendering). It also allows developers to query for environmental light estimation, when available.
    /// </remarks>
    public abstract class XRCameraSubsystem : XRSubsystem<XRCameraSubsystemDescriptor>
    {
        /// <summary>
        /// The provider created by the implementation that contains the required camera functionality.
        /// </summary>
        /// <value>
        /// The provider created by the implementation that contains the required camera functionality.
        /// </value>
        Provider m_Provider;

        /// <summary>
        /// Construct the <c>XRCameraSubsystem</c>.
        /// </summary>
        public XRCameraSubsystem()
        {
            m_Provider = CreateProvider();
            Debug.Assert(m_Provider != null, "camera functionality provider cannot be null");
        }

        /// <summary>
        /// Interface for providing camera functionality for the implementation.
        /// </summary>
        protected class Provider
        {
            /// <summary>
            /// Property to be implemented by the provder to get the material used by <c>XRCameraSubsystem</c> to
            /// render the camera texture.
            /// </summary>
            /// <returns>
            /// The material to render the camera texture.
            /// </returns>
            public virtual Material cameraMaterial => null;

            /// <summary>
            /// Property to be implemented by the provider to determine whether camera permission has been granted.
            /// </summary>
            /// <value>
            /// <c>true</c> if camera permission has been granted. Otherwise, <c>false</c>.
            /// </value>
            public virtual bool permissionGranted => false;

            /// <summary>
            /// Whether or not culling should be inverted during rendering. Some front-facing
            /// camera modes may require this.
            /// </summary>
            public virtual bool invertCulling => false;

            /// <summary>
            /// Method to be implemented by provider to start the camera for the subsystem.
            /// </summary>
            public virtual void Start() { }

            /// <summary>
            /// Method to be implemented by provider to stop the camera for the subsystem.
            /// </summary>
            public virtual void Stop() { }

            /// <summary>
            /// Method to be implemented by provider to destroy the camera for the subsystem.
            /// </summary>
            public virtual void Destroy() { }

            /// <summary>
            /// Method to be implemented by provider to get the camera frame for the subsystem.
            /// </summary>
            /// <param name="cameraParams">The current Unity <c>Camera</c> parameters.</param>
            /// <param name="cameraFrame">The current camera frame returned by the method.</param>
            /// <returns>
            /// <c>true</c> if the method successfully got a frame. Otherwise, <c>false</c>.
            /// </returns>
            public virtual bool TryGetFrame(
                XRCameraParams cameraParams,
                out XRCameraFrame cameraFrame)
            {
                cameraFrame = default(XRCameraFrame);
                return false;
            }

            /// <summary>
            /// Method to be implemented by the provider to set the focus mode for the camera.
            /// </summary>
            /// <param name="cameraFocusMode">The focus mode to set for the camera.</param>
            /// <returns>
            /// <c>true</c> if the method successfully set the focus mode for the camera. Otherwise, <c>false</c>.
            /// </returns>
            public virtual bool TrySetFocusMode(CameraFocusMode cameraFocusMode) => false;

            /// <summary>
            /// Method to be implemented by the provider to set the light estimation mode.
            /// </summary>
            /// <param name="lightEstimationMode">The light estimation mode to set.</param>
            /// <returns>
            /// <c>true</c> if the method successfully set the light estimation mode. Otherwise, <c>false</c>.
            /// </returns>
            public virtual bool TrySetLightEstimationMode(LightEstimationMode lightEstimationMode) => false;

            /// <summary>
            /// Method to be implemented by the provider to get the camera intrinisics information.
            /// </summary>
            /// <param name="cameraIntrinsics">The camera intrinsics information returned from the method.</param>
            /// <returns>
            /// <c>true</c> if the method successfully gets the camera intrinsics information. Otherwise, <c>false</c>.
            /// </returns>
            public virtual bool TryGetIntrinsics(
                out XRCameraIntrinsics cameraIntrinsics)
            {
                cameraIntrinsics = default(XRCameraIntrinsics);
                return false;
            }

            /// <summary>
            /// Method to be implemented by the provider to query the supported camera configurations.
            /// </summary>
            /// <param name="defaultCameraConfiguration">A default value used to fill the returned array before copying
            /// in real values. This ensures future additions to this struct are backwards compatible.</param>
            /// <param name="allocator">The allocation strategy to use for the returned data.</param>
            /// <returns>
            /// The supported camera configurations.
            /// </returns>
            public virtual NativeArray<XRCameraConfiguration> GetConfigurations(XRCameraConfiguration defaultCameraConfiguration,
                                                                                Allocator allocator)
            {
                return new NativeArray<XRCameraConfiguration>(0, allocator);
            }

            /// <summary>
            /// Property to be implemented by the provider to query/set the current camera configuration.
            /// </summary>
            /// <value>
            /// The current camera configuration if it exists. Otherise, <c>null</c>.
            /// </value>
            /// <exception cref="System.NotSupportedException">Thrown when setting the current configuration if the
            /// implementation does not support camera configurations.</exception>
            /// <exception cref="System.ArgumentException">Thrown when setting the current configuration if the given
            /// configuration is not a valid, supported camera configuration.</exception>
            /// <exception cref="System.InvalidOperationException">Thrown when setting the current configuration if the
            /// implementation is unable to set the current camera configuration.</exception>
            public virtual XRCameraConfiguration? currentConfiguration
            {
                get => null;
                set => throw new NotSupportedException("setting current camera configuration is not supported by this implementation");
            }

            /// <summary>
            /// Get the <see cref="XRTextureDescriptor"/>s associated with the current
            /// <see cref="XRCameraFrame"/>.
            /// </summary>
            /// <returns>The current texture descriptors.</returns>
            /// <param name="defaultDescriptor">A default value which should
            /// be used to fill the returned array before copying in the
            /// real values. This ensures future additions to this struct
            /// are backwards compatible.</param>
            /// <param name="allocator">The allocator to use when creating
            /// the returned <c>NativeArray</c>.</param>
            public virtual NativeArray<XRTextureDescriptor> GetTextureDescriptors(
                XRTextureDescriptor defaultDescriptor,
                Allocator allocator)
            {
                return new NativeArray<XRTextureDescriptor>(0, allocator);
            }

            /// <summary>
            /// Method to be implemented by the provider to query for the latest native camera image.
            /// </summary>
            /// <param name="cameraImageCinfo">The metadata required to construct a <see cref="XRCameraImage"/></param>
            /// <returns>
            /// <c>true</c> if the camera image is acquired. Otherwise, <c>false</c>.
            /// </returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            public virtual bool TryAcquireLatestImage(out CameraImageCinfo cameraImageCinfo)
            {
                throw new NotSupportedException("getting camera image is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to get the status of an existing asynchronous conversion
            /// request.
            /// </summary>
            /// <param name="requestId">The unique identifier associated with a request.</param>
            /// <returns>The state of the request.</returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
            public virtual AsyncCameraImageConversionStatus GetAsyncRequestStatus(int requestId)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to dispose an existing native image identified by
            /// <paramref name="nativeHandle"/>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for this camera image.</param>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            /// <seealso cref="TryAcquireLatestImage"/>
            public virtual void DisposeImage(int nativeHandle)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to dispose an existing async conversion request.
            /// </summary>
            /// <param name="requestId">A unique identifier for the request.</param>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
            public virtual void DisposeAsyncRequest(int requestId)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to get information about an image plane from a native image
            /// handle by index.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for this camera image.</param>
            /// <param name="planeIndex">The index of the plane to get.</param>
            /// <param name="planeCinfo">The returned camera plane information if successful.</param>
            /// <returns>
            /// <c>true</c> if the image plane was acquired. Otherwise, <c>false</c>.
            /// </returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            /// <seealso cref="TryAcquireLatestImage"/>
            public virtual bool TryGetPlane(
                int nativeHandle,
                int planeIndex,
                out CameraImagePlaneCinfo planeCinfo)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to determine whether a native image handle returned by
            /// <see cref="TryAcquireLatestImage"/> is currently valid. An image may become invalid if it has been
            /// disposed.
            /// </summary>
            /// <remarks>
            /// If a handle is valid, <see cref="TryConvert"/> and <see cref="TryGetConvertedDataSize"/> should not fail.
            /// </remarks>
            /// <param name="nativeHandle">A unique identifier for the camera image in question.</param>
            /// <returns><c>true</c>, if it is a valid handle. Otherwise, <c>false</c>.</returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            /// <seealso cref="DisposeImage"/>
            public virtual bool NativeHandleValid(
                int nativeHandle)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to get the number of bytes required to store an image with the
            /// given dimensions and <c>TextureFormat</c>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="dimensions">The dimensions of the output image.</param>
            /// <param name="format">The <c>TextureFormat</c> for the image.</param>
            /// <param name="size">The number of bytes required to store the converted image.</param>
            /// <returns><c>true</c> if the output <paramref name="size"/> was set.</returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            public virtual bool TryGetConvertedDataSize(
                int nativeHandle,
                Vector2Int dimensions,
                TextureFormat format,
                out int size)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to convert the image with handle
            /// <paramref name="nativeHandle"/> using the provided <paramref cref="conversionParams"/>.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="conversionParams">The parameters to use during the conversion.</param>
            /// <param name="destinationBuffer">A buffer to write the converted image to.</param>
            /// <param name="bufferLength">The number of bytes available in the buffer.</param>
            /// <returns>
            /// <c>true</c> if the image was converted and stored in <paramref name="destinationBuffer"/>.
            /// </returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            public virtual bool TryConvert(
                int nativeHandle,
                XRCameraImageConversionParams conversionParams,
                IntPtr destinationBuffer,
                int bufferLength)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to create an asynchronous request to convert a camera image,
            /// similar to <see cref="TryConvert"/> except the conversion should happen on a thread other than the
            /// calling (main) thread.
            /// </summary>
            /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
            /// <param name="conversionParams">The parameters to use during the conversion.</param>
            /// <returns>A unique identifier for this request.</returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            public virtual int ConvertAsync(
                int nativeHandle,
                XRCameraImageConversionParams conversionParams)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to get a pointer to the image data from a completed
            /// asynchronous request. This method should only succeed if <see cref="GetAsyncRequestStatus"/> returns
            /// <see cref="AsyncCameraImageConversionStatus.Ready"/>.
            /// </summary>
            /// <param name="requestId">The unique identifier associated with a request.</param>
            /// <param name="dataPtr">A pointer to the native buffer containing the data.</param>
            /// <param name="dataLength">The number of bytes in <paramref name="dataPtr"/>.</param>
            /// <returns><c>true</c> if <paramref name="dataPtr"/> and <paramref name="dataLength"/> were set and point
            ///  to the image data.</returns>
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            public virtual bool TryGetAsyncRequestData(int requestId, out IntPtr dataPtr, out int dataLength)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Method to be implemented by the provider to similar to
            /// <see cref="ConvertAsync(int, XRCameraImageConversionParams)"/> but takes a delegate to invoke when the
            /// request is complete, rather than returning a request id.
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
            /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera
            /// image.</exception>
            public virtual void ConvertAsync(
                int nativeHandle,
                XRCameraImageConversionParams conversionParams,
                OnImageRequestCompleteDelegate callback,
                IntPtr context)
            {
                throw new NotSupportedException("camera image conversion is not supported by this implementation");
            }

            /// <summary>
            /// Create the camera material from the given camera shader name.
            /// </summary>
            /// <param name="cameraShaderName">The name of the camera shader.</param>
            /// <returns>
            /// The created camera material shader.
            /// </returns>
            /// <exception cref="System.InvalidOperationException">Thrown if the shader cannot be found or if a
            /// material cannot be created for the shader.</exception>
            protected Material CreateCameraMaterial(string cameraShaderName)
            {
                var shader = Shader.Find(cameraShaderName);
                if (shader == null)
                {
                    throw new InvalidOperationException($"Could not find shader named '{cameraShaderName}' required "
                                                        + $"for video overlay on camera subsystem.");
                }

                Material material = new Material(shader);
                if (material == null)
                {
                    throw new InvalidOperationException($"Could not create a material for shader named "
                                                        + $"'{cameraShaderName}' required for video overlay on camera "
                                                        + $"subsystem.");
                }

                return material;
            }
        }

        /// <summary>
        /// Specifies the focus mode for the camera.
        /// </summary>
        /// <value>
        /// The focus mode for the camera.
        /// </value>
        public CameraFocusMode focusMode
        {
            get => m_FocusMode;
            set
            {
                if (m_Provider.TrySetFocusMode(value))
                {
                    m_FocusMode = value;
                }
            }
        }
        CameraFocusMode m_FocusMode = CameraFocusMode.Fixed;

        /// <summary>
        /// Specifies the light estimation mode.
        /// </summary>
        /// <value>
        /// The light estimation mode.
        /// </value>
        public LightEstimationMode lightEstimationMode
        {
            get => m_LightEstimationMode;
            set
            {
                if ((m_LightEstimationMode != value) && m_Provider.TrySetLightEstimationMode(value))
                {
                    m_LightEstimationMode = value;
                }
            }
        }
        LightEstimationMode m_LightEstimationMode = LightEstimationMode.Disabled;

        /// <summary>
        /// Start the camera subsystem.
        /// </summary>
        protected sealed override void OnStart() => m_Provider.Start();

        /// <summary>
        /// Stop the camera subsystem.
        /// </summary>
        protected sealed override void OnStop() => m_Provider.Stop();

        /// <summary>
        /// Destroy the camera subsystem.
        /// </summary>
        protected sealed override void OnDestroyed() => m_Provider.Destroy();

        /// <summary>
        /// Gets the <see cref="XRTextureDescriptor"/>s associated with the
        /// current frame. The caller owns the returned <c>NativeArray</c>
        /// and is responsible for calling <c>Dispose</c> on it.
        /// </summary>
        /// <returns>An array of texture descriptors.</returns>
        /// <param name="allocator">The allocator to use when creating
        /// the returned <c>NativeArray</c>.</param>
        public NativeArray<XRTextureDescriptor> GetTextureDescriptors(
            Allocator allocator)
        {
            return m_Provider.GetTextureDescriptors(
                default(XRTextureDescriptor),
                allocator);
        }

        /// <summary>
        /// Get the material used by <c>XRCameraSubsystem</c> to render the camera texture.
        /// </summary>
        /// <value>
        /// The material to render the camera texture.
        /// </value>
        public Material cameraMaterial => m_Provider.cameraMaterial;

        /// <summary>
        /// Returns the camera intrinsics information.
        /// </summary>
        /// <param name="cameraIntrinsics">The camera intrinsics information returned from the method.</param>
        /// <returns>
        /// <c>true</c> if the method successfully gets the camera intrinsics information. Otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics)
        {
            return m_Provider.TryGetIntrinsics(out cameraIntrinsics);
        }

        /// <summary>
        /// Queries for the supported camera configurations.
        /// </summary>
        /// <param name="allocator">The allocation strategy to use for the returned data.</param>
        /// <returns>
        /// The supported camera configurations.
        /// </returns>
        public NativeArray<XRCameraConfiguration> GetConfigurations(Allocator allocator)
        {
            return m_Provider.GetConfigurations(default(XRCameraConfiguration), allocator);
        }

        /// <summary>
        /// The current camera configuration.
        /// </summary>
        /// <value>
        /// The current camera configuration if it exists. Otherise, <c>null</c>.
        /// </value>
        /// <exception cref="System.NotSupportedException">Thrown when setting the current configuration if the
        /// implementation does not support camera configurations.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when setting the current configuration if the given
        /// configuration is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Thrown when setting the current configuration if the given
        /// configuration is not a supported camera configuration.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when setting the current configuration if the
        /// implementation is unable to set the current camera configuration.</exception>
        public virtual XRCameraConfiguration? currentConfiguration
        {
            get => m_Provider.currentConfiguration;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "cannot set the camera configuration to null");
                }

                m_Provider.currentConfiguration = value;
            }
        }

        /// <summary>
        /// Whether to invert the culling mode during rendering. Some front-facing
        /// camera modes may require this.
        /// </summary>
        public bool invertCulling => m_Provider.invertCulling;

        /// <summary>
        /// Method for the implementation to create the camera functionality provider.
        /// </summary>
        /// <returns>
        /// The camera functionality provider.
        /// </returns>
        protected abstract Provider CreateProvider();

        /// <summary>
        /// Get the latest frame from the provider.
        /// </summary>
        /// <param name="cameraParams">The Unity <c>Camera</c> parameters.</param>
        /// <param name="frame">The camera frame to be populated if the subsystem is running and successfully provides
        /// the latest camera frame.</param>
        /// <returns>
        /// <c>true</c> if the camera frame is successfully returned. Otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetLatestFrame(
            XRCameraParams cameraParams,
            out XRCameraFrame frame)
        {
            if (running && m_Provider.TryGetFrame(cameraParams, out frame))
            {
                return true;
            }

            frame = default(XRCameraFrame);
            return false;
        }

        /// <summary>
        /// Determines whether camera permission has been granted.
        /// </summary>
        /// <value>
        /// <c>true</c> if camera permission has been granted. Otherwise, <c>false</c>.
        /// </value>
        public bool permissionGranted => m_Provider.permissionGranted;

        /// <summary>
        /// Attempt to get the latest camera image. This provides directly access to the raw pixel data, as well as
        /// utilities to convert to RGB and Grayscale formats.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="XRCameraImage"/> must be disposed to avoid resource leaks.
        /// </remarks>
        /// <param name="cameraImage">A valid <see cref="XRCameraImage"/> if this method returns <c>true</c>.</param>
        /// <returns>
        /// <c>true</c> if the image was acquired. Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        public bool TryGetLatestImage(out XRCameraImage cameraImage)
        {
            CameraImageCinfo cameraImageCinfo;
            if (m_Provider.TryAcquireLatestImage(out cameraImageCinfo))
            {
                cameraImage = new XRCameraImage(this, cameraImageCinfo.nativeHandle, cameraImageCinfo.dimensions,
                                                cameraImageCinfo.planeCount, cameraImageCinfo.timestamp,
                                                cameraImageCinfo.format);
                return true;
            }
            else
            {
                cameraImage = default(XRCameraImage);
                return false;
            }
        }

        /// <summary>
        /// Registers a camera subsystem implementation based on the given subsystem parameters.
        /// </summary>
        /// <param name="cameraSubsystemParams">The parameters defining the camera subsystem functionality implemented
        /// by the subsystem provider.</param>
        /// <returns>
        /// <c>true</c> if the subsystem implementation is registered. Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown when the values specified in the
        /// <see cref="XRCameraSubsystemCinfo"/> parameter are invalid. Typically, this will occur
        /// <list type="bullet">
        /// <item>
        /// <description>if <see cref="XRCameraSubsystemCinfo.id"/> is <c>null</c> or empty</description>
        /// </item>
        /// <item>
        /// <description>if <see cref="XRCameraSubsystemCinfo.implementationType"/> is <c>null</c></description>
        /// </item>
        /// <item>
        /// <description>if <see cref="XRCameraSubsystemCinfo.implementationType"/> does not derive from the
        /// <see cref="XRCameraSubsystem"/> class
        /// </description>
        /// </item>
        /// </list>
        /// </exception>
        public static bool Register(XRCameraSubsystemCinfo cameraSubsystemParams)
        {
            XRCameraSubsystemDescriptor cameraSubsystemDescriptor = XRCameraSubsystemDescriptor.Create(cameraSubsystemParams);
            return SubsystemRegistration.CreateDescriptor(cameraSubsystemDescriptor);
        }

        /// <summary>
        /// Get the status of an existing asynchronous conversion request.
        /// </summary>
        /// <param name="requestId">The unique identifier associated with a request.</param>
        /// <returns>The state of the request.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        /// <seealso cref="ConvertAsync(int, XRCameraImageConversionParams)"/>
        internal AsyncCameraImageConversionStatus GetAsyncRequestStatus(int requestId)
        {
            return m_Provider.GetAsyncRequestStatus(requestId);
        }

        /// <summary>
        /// Dispose an existing native image identified by <paramref name="nativeHandle"/>.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for this camera image.</param>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        /// <seealso cref="Provider.TryAcquireLatestImage"/>
        internal void DisposeImage(int nativeHandle) => m_Provider.DisposeImage(nativeHandle);

        /// <summary>
        /// Dispose an existing async conversion request.
        /// </summary>
        /// <param name="requestId">A unique identifier for the request.</param>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        /// <seealso cref="Provider.ConvertAsync(int, XRCameraImageConversionParams)"/>
        internal void DisposeAsyncRequest(int requestId) => m_Provider.DisposeAsyncRequest(requestId);

        /// <summary>
        /// Attempt to get information about an image plane from a native image by index.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for this camera image.</param>
        /// <param name="planeIndex">The index of the plane to get.</param>
        /// <param name="planeCinfo">The returned camera plane information if successful.</param>
        /// <returns>
        /// <c>true</c> if the image plane is successfully acquired. Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        /// <seealso cref="Provider.TryAcquireLatestImage"/>
        internal bool TryGetPlane(
            int nativeHandle,
            int planeIndex,
            out CameraImagePlaneCinfo planeCinfo)
        {
            return m_Provider.TryGetPlane(nativeHandle, planeIndex, out planeCinfo);
        }

        /// <summary>
        /// Determine whether a native image handle returned by <see cref="Provider.TryAcquireLatestImage"/> is
        /// currently valid. An image may become invalid if it has been disposed.
        /// </summary>
        /// <remarks>
        /// If a handle is valid, <see cref="TryConvert"/> and <see cref="TryGetConvertedDataSize"/> should not fail.
        /// </remarks>
        /// <param name="nativeHandle">A unique identifier for the camera image in question.</param>
        /// <returns><c>true</c> if it is a valid handle, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        /// <seealso cref="DisposeImage"/>
        internal bool NativeHandleValid(int nativeHandle) => m_Provider.NativeHandleValid(nativeHandle);

        /// <summary>
        /// Get the number of bytes required to store an image with the given dimensions and <c>TextureFormat</c>.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="dimensions">The dimensions of the output image.</param>
        /// <param name="format">The <c>TextureFormat</c> for the image.</param>
        /// <param name="size">The number of bytes required to store the converted image.</param>
        /// <returns><c>true</c> if the output <paramref name="size"/> was set.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        internal bool TryGetConvertedDataSize(
            int nativeHandle,
            Vector2Int dimensions,
            TextureFormat format,
            out int size)
        {
            return m_Provider.TryGetConvertedDataSize(nativeHandle, dimensions, format, out size);
        }

        /// <summary>
        /// Convert the image with handle <paramref name="nativeHandle"/> using the provided
        /// <see cref="XRCameraImageConversionParams"/>.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="conversionParams">The parameters to use during the conversion.</param>
        /// <param name="destinationBuffer">A buffer to write the converted image to.</param>
        /// <param name="bufferLength">The number of bytes available in the buffer.</param>
        /// <returns>
        /// <c>true</c> if the image was converted and stored in <paramref name="destinationBuffer"/>.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        internal bool TryConvert(
            int nativeHandle,
            XRCameraImageConversionParams conversionParams,
            IntPtr destinationBuffer,
            int bufferLength)
        {
            return m_Provider.TryConvert(nativeHandle, conversionParams, destinationBuffer, bufferLength);
        }

        /// <summary>
        /// Create an asynchronous request to convert a camera image, similar to <see cref="TryConvert"/> except the
        /// conversion should happen on a thread other than the calling (main) thread.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="conversionParams">The parameters to use during the conversion.</param>
        /// <returns>A unique identifier for this request.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        internal int ConvertAsync(
            int nativeHandle,
            XRCameraImageConversionParams conversionParams)
        {
            return m_Provider.ConvertAsync(nativeHandle, conversionParams);
        }

        /// <summary>
        /// Get a pointer to the image data from a completed asynchronous request. This method should only succeed if
        /// <see cref="GetAsyncRequestStatus"/> returns <see cref="AsyncCameraImageConversionStatus.Ready"/>.
        /// </summary>
        /// <param name="requestId">The unique identifier associated with a request.</param>
        /// <param name="dataPtr">A pointer to the native buffer containing the data.</param>
        /// <param name="dataLength">The number of bytes in <paramref name="dataPtr"/>.</param>
        /// <returns><c>true</c> if <paramref name="dataPtr"/> and <paramref name="dataLength"/> were set and point to
        /// the image data.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        internal bool TryGetAsyncRequestData(int requestId, out IntPtr dataPtr, out int dataLength)
        {
            return m_Provider.TryGetAsyncRequestData(requestId, out dataPtr, out dataLength);
        }

        /// <summary>
        /// Callback from native code for when the asychronous conversion is complete.
        /// </summary>
        /// <param name="status">The status of the conversion operation.</param>
        /// <param name="conversionParams">The parameters for the conversion.</param>
        /// <param name="dataPtr">The native pointer to the converted data.</param>
        /// <param name="dataLength">The memory size of the converted data.</param>
        /// <param name="context">The native context for the conversion operation.</param>
        protected internal delegate void OnImageRequestCompleteDelegate(
            AsyncCameraImageConversionStatus status,
            XRCameraImageConversionParams conversionParams,
            IntPtr dataPtr,
            int dataLength,
            IntPtr context);

        /// <summary>
        /// Similar to <see cref="ConvertAsync(int, XRCameraImageConversionParams)"/> but takes a delegate to invoke
        /// when the request is complete, rather than returning a request id.
        /// </summary>
        /// <remarks>
        /// If the first parameter to <paramref name="callback"/> is
        /// <see cref="AsyncCameraImageConversionStatus.Ready"/> then the <c>dataPtr</c> parameter must be valid for
        /// the duration of the invocation. The data may be destroyed immediately upon return. The
        /// <paramref name="context"/> parameter must be passed back to the <paramref name="callback"/>.
        /// </remarks>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="conversionParams">The parameters to use during the conversion.</param>
        /// <param name="callback">A delegate which must be invoked when the request is complete, whether the
        /// conversion was successfully or not.</param>
        /// <param name="context">A native pointer which must be passed back unaltered to <paramref name="callback"/>.
        /// </param>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support camera image.
        /// </exception>
        internal void ConvertAsync(
            int nativeHandle,
            XRCameraImageConversionParams conversionParams,
            OnImageRequestCompleteDelegate callback,
            IntPtr context)
        {
            m_Provider.ConvertAsync(nativeHandle, conversionParams, callback, context);
        }

        /// <summary>
        /// Container for native camera image construction metadata.
        /// </summary>
        protected struct CameraImageCinfo : IEquatable<CameraImageCinfo>
        {
            /// <summary>
            /// The handle representing the camera image on the native level.
            /// </summary>
            /// <value>
            /// The handle representing the camera image on the native level.
            /// </value>
            public int nativeHandle => m_NativeHandle;
            int m_NativeHandle;

            /// <summary>
            /// The dimensions of the camera image.
            /// </summary>
            /// <value>
            /// The dimensions of the camera image.
            /// </value>
            public Vector2Int dimensions => m_Dimensions;
            Vector2Int m_Dimensions;

            /// <summary>
            /// The number of video planes in the camera image.
            /// </summary>
            /// <value>
            /// The number of video planes in the camera image.
            /// </value>
            public int planeCount => m_PlaneCount;
            int m_PlaneCount;

            /// <summary>
            /// The timestamp for when the camera image was captured.
            /// </summary>
            /// <value>
            /// The timestamp for when the camera image was captured.
            /// </value>
            public double timestamp => m_Timestamp;
            double m_Timestamp;

            /// <summary>
            /// The format of the camera image.
            /// </summary>
            /// <value>
            /// The format of the camera image.
            /// </value>
            public CameraImageFormat format => m_Format;
            CameraImageFormat m_Format;

            /// <summary>
            /// Constructs the camera image cinfo.
            /// </summary>
            /// <param name="nativeHandle">The handle representing the camera image on the native level.</param>
            /// <param name="dimensions">The dimensions of the camera image.</param>
            /// <param name="planeCount">The number of video planes in the camera image.</param>
            /// <param name="timestamp">The timestamp for when the camera image was captured.</param>
            /// <param name="format">The format of the camera image.</param>
            public CameraImageCinfo(int nativeHandle, Vector2Int dimensions, int planeCount, double timestamp,
                                    CameraImageFormat format)
            {
                this.m_NativeHandle = nativeHandle;
                this.m_Dimensions = dimensions;
                this.m_PlaneCount = planeCount;
                this.m_Timestamp = timestamp;
                this.m_Format = format;
            }

            public bool Equals(CameraImageCinfo other)
            {
                return (nativeHandle.Equals(other.nativeHandle) && dimensions.Equals(other.dimensions)
                        && planeCount.Equals(other.planeCount) && timestamp.Equals(other.timestamp)
                        && format.Equals(other.format));
            }

            public override bool Equals(System.Object obj)
            {
                return ReferenceEquals(this, obj) || ((obj is CameraImageCinfo) && Equals((CameraImageCinfo)obj));
            }

            public static bool operator ==(CameraImageCinfo lhs, CameraImageCinfo rhs) => lhs.Equals(rhs);

            public static bool operator !=(CameraImageCinfo lhs, CameraImageCinfo rhs) => !(lhs == rhs);

            public override int GetHashCode()
            {
                int hashCode = 486187739;
                unchecked
                {
                    hashCode = (hashCode * 486187739) + nativeHandle.GetHashCode();
                    hashCode = (hashCode * 486187739) + dimensions.GetHashCode();
                    hashCode = (hashCode * 486187739) + planeCount.GetHashCode();
                    hashCode = (hashCode * 486187739) + timestamp.GetHashCode();
                    hashCode = (hashCode * 486187739) + ((int)format).GetHashCode();
                }
                return hashCode;
            }

            public override string ToString()
            {
                return string.Format("nativeHandle: {0} dimensions:{1} planes:{2} timestamp:{3} format:{4}",
                                     nativeHandle.ToString(), dimensions.ToString(), planeCount.ToString(),
                                     timestamp.ToString(), format.ToString());
            }
        }

        /// <summary>
        /// Container for the metadata describing access to the raw camera image plane data.
        /// </summary>
        protected internal struct CameraImagePlaneCinfo : IEquatable<CameraImagePlaneCinfo>
        {
            /// <summary>
            /// The pointer to the raw native image data.
            /// </summary>
            /// <value>
            /// The pointer to the raw native image data.
            /// </value>
            public IntPtr dataPtr => m_DataPtr;
            IntPtr m_DataPtr;

            /// <summary>
            /// The length of the native image data.
            /// </summary>
            /// <value>
            /// The length of the native image data.
            /// </value>
            public int dataLength => m_DataLength;
            int m_DataLength;

            /// <summary>
            /// The stride for iterating through the rows of the native image data.
            /// </summary>
            /// <value>
            /// The stride for iterating through the rows of the native image data.
            /// </value>
            public int rowStride => m_RowStride;
            int m_RowStride;

            /// <summary>
            /// The stride for iterating through the pixels of the native image data.
            /// </summary>
            /// <value>
            /// The stride for iterating through the pixels of the native image data.
            /// </value>
            public int pixelStride => m_PixelStride;
            int m_PixelStride;

            /// <summary>
            /// Constructs the camera image plane cinfo.
            /// </summary>
            /// <param name="dataPtr">The pointer to the raw native image data.</param>
            /// <param name="dataLength">The length of the native image data.</param>
            /// <param name="rowStride">The stride for iterating through the rows of the native image data.</param>
            /// <param name="pixelStride">The stride for iterating through the pixels of the native image data.</param>
            public CameraImagePlaneCinfo(IntPtr dataPtr, int dataLength, int rowStride, int pixelStride)
            {
                this.m_DataPtr = dataPtr;
                this.m_DataLength = dataLength;
                this.m_RowStride = rowStride;
                this.m_PixelStride = pixelStride;
            }

            public bool Equals(CameraImagePlaneCinfo other)
            {
                return (dataPtr.Equals(other.dataPtr) && dataLength.Equals(other.dataLength)
                        && rowStride.Equals(other.rowStride) && pixelStride.Equals(other.pixelStride));
            }

            public override bool Equals(System.Object obj)
            {
                return ReferenceEquals(this, obj) || ((obj is CameraImagePlaneCinfo) && Equals((CameraImagePlaneCinfo)obj));
            }

            public static bool operator ==(CameraImagePlaneCinfo lhs, CameraImagePlaneCinfo rhs) => lhs.Equals(rhs);

            public static bool operator !=(CameraImagePlaneCinfo lhs, CameraImagePlaneCinfo rhs) => !(lhs == rhs);

            public override int GetHashCode()
            {
                int hashCode = 486187739;
                unchecked
                {
                    hashCode = (hashCode * 486187739) + dataPtr.GetHashCode();
                    hashCode = (hashCode * 486187739) + dataLength.GetHashCode();
                    hashCode = (hashCode * 486187739) + rowStride.GetHashCode();
                    hashCode = (hashCode * 486187739) + pixelStride.GetHashCode();
                }
                return hashCode;
            }

            public override string ToString()
            {
                return string.Format("dataPtr: {0} length:{1} rowStride:{2} pixelStride:{3}", dataPtr.ToString(),
                                     dataLength.ToString(), rowStride.ToString(), pixelStride.ToString());
            }
        }
    }
}
