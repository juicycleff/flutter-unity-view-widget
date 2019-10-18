using AOT;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents a single, raw image from a device camera. Provides access to the raw image plane data, as well as
    /// conversion methods to convert to color and grayscale formats. See <see cref="Convert"/> and
    /// <see cref="ConvertAsync(XRCameraImageConversionParams)"/>. Use
    /// <see cref="XRCameraSubsystem.TryGetLatestImage"/> to get a <c>XRCameraImage</c>.
    /// </summary>
    /// <remarks>
    /// Each <c>XRCameraImage</c> must be explicitly disposed. Failing to do so will leak resources and could prevent
    /// future camera image access.
    /// </remarks>
    public struct XRCameraImage : IDisposable, IEquatable<XRCameraImage>
    {
        int m_NativeHandle;

        XRCameraSubsystem m_CameraSubsystem;

        static XRCameraSubsystem.OnImageRequestCompleteDelegate s_OnAsyncConversionComplete;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle m_SafetyHandle;
#endif

        /// <summary>
        /// The dimensions (width and height) of the image.
        /// </summary>
        /// <value>
        /// The dimensions (width and height) of the image.
        /// </value>
        public Vector2Int dimensions { get; private set; }

        /// <summary>
        /// The image width.
        /// </summary>
        /// <value>
        /// The image width.
        /// </value>
        public int width { get { return dimensions.x; } }

        /// <summary>
        /// The image height.
        /// </summary>
        /// <value>
        /// The image height.
        /// </value>
        public int height { get { return dimensions.y; } }

        /// <summary>
        /// The number of image planes. A "plane" in this context refers to a channel in the raw video format,
        /// not a physical surface.
        /// </summary>
        /// <value>
        /// The number of image planes.
        /// </value>
        public int planeCount { get; private set; }

        /// <summary>
        /// The format used by the image planes. You will only need this if you plan to interpret the raw plane data.
        /// </summary>
        /// <value>
        /// The format used by the image planes.
        /// </value>
        public CameraImageFormat format { get; private set; }

        /// <summary>
        /// The timestamp, in seconds, associated with this camera image
        /// </summary>
        /// <value>
        /// The timestamp, in seconds, associated with this camera image
        /// </value>
        public double timestamp { get; private set; }

        /// <summary>
        /// Whether this <c>XRCameraImage</c> represents a valid image (i.e., not Disposed).
        /// </summary>
        /// <value>
        /// <c>true</c> if this <c>XRCameraImage</c> represents a valid image. Otherwise, <c>false</c>.
        /// </value>
        public bool valid
        {
            get { return (m_CameraSubsystem != null) && m_CameraSubsystem.NativeHandleValid(m_NativeHandle); }
        }

        /// <summary>
        /// Initialize the static callback for when the image request completes.
        /// </summary>
        static XRCameraImage()
        {
            s_OnAsyncConversionComplete = new XRCameraSubsystem.OnImageRequestCompleteDelegate(OnAsyncConversionComplete);
        }

        /// <summary>
        /// Construct the <c>XRCameraImage</c> with the given native image information.
        /// </summary>
        /// <param name="cameraSubsystem">The camera subsystem to use for interacting with the native image.</param>
        /// <param name="nativeHandle">The native image handle.</param>
        /// <param name="dimensions">The dimensions of the native image.</param>
        /// <param name="planeCount">The number of video planes in the native image.</param>
        /// <param name="timestamp">The timestamp for when the native image was captured.</param>
        /// <param name="format">The camera image format of the native image.</param>
        internal XRCameraImage(
            XRCameraSubsystem cameraSubsystem,
            int nativeHandle,
            Vector2Int dimensions,
            int planeCount,
            double timestamp,
            CameraImageFormat format)
        {
            m_CameraSubsystem = cameraSubsystem;
            m_NativeHandle = nativeHandle;
            this.dimensions = dimensions;
            this.planeCount = planeCount;
            this.timestamp = timestamp;
            this.format = format;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_SafetyHandle = AtomicSafetyHandle.Create();
#endif
        }

        /// <summary>
        /// Determines whether the given <c>TextureFormat</c> is supported for conversion.
        /// </summary>
        /// <remarks>
        /// These texture formats are supported:
        /// <list type="bullet">
        /// <item><description><c>TextureFormat.R8</c></description></item>
        /// <item><description><c>TextureFormat.Alpha8</c></description></item>
        /// <item><description><c>TextureFormat.RGB24</c></description></item>
        /// <item><description><c>TextureFormat.RGBA32</c></description></item>
        /// <item><description><c>TextureFormat.ARGBA32</c></description></item>
        /// <item><description><c>TextureFormat.BGRA32</c></description></item>
        /// </list>
        /// </remarks>
        /// <param name="format">A <c>TextureFormat</c> to test.</param>
        /// <returns><c>true</c> if the format is supported by the various conversion methods.</returns>
        public static bool FormatSupported(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.Alpha8:
                case TextureFormat.R8:
                case TextureFormat.RGB24:
                case TextureFormat.RGBA32:
                case TextureFormat.ARGB32:
                case TextureFormat.BGRA32:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get an image "plane". A "plane" in this context refers to a channel in the raw video format, not a physical
        /// surface.
        /// </summary>
        /// <param name="planeIndex">The index of the plane to get.</param>
        /// <returns>A <see cref="XRCameraImagePlane"/> describing the plane.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="planeIndex"/> is not within
        /// the range [0, <see cref="planeCount"/>).</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the requested plane is not valid.</exception>
        public unsafe XRCameraImagePlane GetPlane(int planeIndex)
        {
            ValidateNativeHandleAndThrow();
            if (planeIndex < 0 || planeIndex >= planeCount)
            {
                throw new ArgumentOutOfRangeException("planeIndex",
                    string.Format("planeIndex must be in the range 0 to {0}", planeCount - 1));
            }

            XRCameraSubsystem.CameraImagePlaneCinfo imagePlaneCinfo;
            if (!m_CameraSubsystem.TryGetPlane(m_NativeHandle, planeIndex, out imagePlaneCinfo))
            {
                throw new InvalidOperationException("The requested plane is not valid for this XRCameraImage.");
            }

            var data = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(
                (void*)imagePlaneCinfo.dataPtr, imagePlaneCinfo.dataLength, Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref data, m_SafetyHandle);
#endif

            return new XRCameraImagePlane
            {
                rowStride = imagePlaneCinfo.rowStride,
                pixelStride = imagePlaneCinfo.pixelStride,
                data = data
            };
        }

        /// <summary>
        /// Get the number of bytes required to store a converted image with the given parameters.
        /// </summary>
        /// <param name="dimensions">The desired dimensions of the converted image.</param>
        /// <param name="format">The desired <c>TextureFormat</c> for the converted image.</param>
        /// <returns>The number of bytes required to store the converted image.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the desired <paramref name="format"/> is not
        /// supported.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the desired <paramref name="dimensions"/>
        /// exceed the native image dimensions.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the image is invalid.</exception>
        /// <seealso cref="FormatSupported"/>
        public int GetConvertedDataSize(Vector2Int dimensions, TextureFormat format)
        {
            ValidateNativeHandleAndThrow();

            if (dimensions.x > this.dimensions.x)
            {
                throw new ArgumentOutOfRangeException("width",
                    string.Format("Converted image width must be less than or equal to native image width. {0} > {1}",
                                  dimensions.x, this.dimensions.x));
            }

            if (dimensions.y > this.dimensions.y)
            {
                throw new ArgumentOutOfRangeException("height",
                    string.Format("Converted image height must be less than or equal to native image height. {0} > {1}",
                                  dimensions.y, this.dimensions.y));
            }

            if (!FormatSupported(format))
            {
                throw new ArgumentException("Invalid texture format.", "format");
            }

            int size;
            if (!m_CameraSubsystem.TryGetConvertedDataSize(m_NativeHandle, dimensions, format, out size))
            {
                throw new InvalidOperationException("XRCameraImage is not valid.");
            }

            return size;
        }

        /// <summary>
        /// Get the number of bytes required to store a converted image with the given parameters.
        /// </summary>
        /// <param name="conversionParams">The desired conversion parameters.</param>
        /// <returns>The number of bytes required to store the converted image.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the desired format is not supported.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the desired dimensions exceed the native
        /// image dimensions.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the image is invalid.</exception>
        /// <seealso cref="FormatSupported"/>
        public int GetConvertedDataSize(XRCameraImageConversionParams conversionParams)
        {
            return GetConvertedDataSize(
                conversionParams.outputDimensions,
                conversionParams.outputFormat);
        }

        /// <summary>
        /// Convert the <c>XRCameraImage</c> to one of the supported formats using the specified
        /// <paramref name="conversionParams"/>.
        /// </summary>
        /// <param name="conversionParams">The parameters for the image conversion.</param>
        /// <param name="destinationBuffer">A pointer to memory to which to write the converted image.</param>
        /// <param name="bufferLength">The number of bytes pointed to by <paramref name="destinationBuffer"/>. Must be
        /// greater than or equal to the value returned by
        /// <see cref="GetConvertedDataSize(XRCameraImageConversionParams)"/>.</param>
        /// <exception cref="System.ArgumentException">Thrown if the <paramref name="bufferLength"/> is smaller than
        /// the data size required by the conversion.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion failed.</exception>
        /// <seealso cref="FormatSupported"/>
        public void Convert(XRCameraImageConversionParams conversionParams, IntPtr destinationBuffer, int bufferLength)
        {
            ValidateNativeHandleAndThrow();
            ValidateConversionParamsAndThrow(conversionParams);
            int requiredDataSize = GetConvertedDataSize(conversionParams);

            if (bufferLength < requiredDataSize)
            {
                throw new ArgumentException(string.Format(
                    "Conversion requires {0} bytes but only provided {1} bytes.", requiredDataSize, bufferLength),
                    "bufferLength");
            }

            if (!m_CameraSubsystem.TryConvert(m_NativeHandle, conversionParams, destinationBuffer, bufferLength))
            {
                throw new InvalidOperationException("Conversion failed.");
            }
        }

        /// <summary>
        /// Convert the <c>XRCameraImage</c> to one of the supported formats using the specified
        /// <paramref name="conversionParams"/>. The conversion is performed asynchronously. Use the returned
        /// <see cref="XRAsyncCameraImageConversion"/> to check for the status of the conversion, and retrieve the data
        /// when complete.
        /// </summary>
        /// <remarks>
        /// It is safe to <c>Dispose</c> the <c>XRCameraImage</c> before the asynchronous operation has completed.
        /// </remarks>
        /// <param name="conversionParams">The parameters to use for the conversion.</param>
        /// <returns>A <see cref="XRAsyncCameraImageConversion"/> which can be used to check the status of the
        /// conversion operation and get the resulting data.</returns>
        /// <seealso cref="FormatSupported"/>
        public XRAsyncCameraImageConversion ConvertAsync(XRCameraImageConversionParams conversionParams)
        {
            ValidateNativeHandleAndThrow();
            ValidateConversionParamsAndThrow(conversionParams);

            return new XRAsyncCameraImageConversion(m_CameraSubsystem, m_NativeHandle, conversionParams);
        }

        /// <summary>
        /// <para>Convert the <c>XRCameraImage</c> to one of the supported formats using the specified
        /// <paramref name="conversionParams"/>. The conversion is performed asynchronously, and
        /// <paramref name="onComplete"/> is invoked when the conversion is complete, whether successful or not.</para>
        /// <para>The <c>NativeArray</c> provided in the <paramref name="onComplete"/> delegate is only valid during
        /// the invocation and is disposed immediately upon return.</para>
        /// </summary>
        /// <param name="conversionParams">The parameters to use for the conversion.</param>
        /// <param name="onComplete">A delegate to invoke when the conversion operation completes. The delegate is
        /// always invoked.</param>
        /// <seealso cref="FormatSupported"/>
        public void ConvertAsync(
            XRCameraImageConversionParams conversionParams,
            Action<AsyncCameraImageConversionStatus, XRCameraImageConversionParams, NativeArray<byte>> onComplete)
        {
            ValidateNativeHandleAndThrow();
            ValidateConversionParamsAndThrow(conversionParams);

            var handle = GCHandle.Alloc(onComplete);
            var context = GCHandle.ToIntPtr(handle);
            m_CameraSubsystem.ConvertAsync(m_NativeHandle, conversionParams, s_OnAsyncConversionComplete, context);
        }

        /// <summary>
        /// Callback from native code for when the asychronous conversion is complete.
        /// </summary>
        /// <param name="status">The status of the conversion operation.</param>
        /// <param name="conversionParams">The parameters for the conversion.</param>
        /// <param name="dataPtr">The native pointer to the converted data.</param>
        /// <param name="dataLength">The memory size of the converted data.</param>
        /// <param name="context">The native context for the conversion operation.</param>
        [MonoPInvokeCallback(typeof(XRCameraSubsystem.OnImageRequestCompleteDelegate))]
        static unsafe void OnAsyncConversionComplete(
            AsyncCameraImageConversionStatus status, XRCameraImageConversionParams conversionParams, IntPtr dataPtr,
            int dataLength, IntPtr context)
        {
            var handle = GCHandle.FromIntPtr(context);
            var onComplete = (Action<AsyncCameraImageConversionStatus, XRCameraImageConversionParams, NativeArray<byte>>)handle.Target;

            if (onComplete != null)
            {
                var data = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(
                    (void*)dataPtr, dataLength, Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                var safetyHandle = AtomicSafetyHandle.Create();
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref data, safetyHandle);
#endif

                onComplete(status, conversionParams, data);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.Release(safetyHandle);
#endif
            }

            handle.Free();
        }

        /// <summary>
        /// Ensure the image is valid.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the image is invalid.</exception>
        void ValidateNativeHandleAndThrow()
        {
            if (!valid)
            {
                throw new InvalidOperationException("XRCameraImage is not valid.");
            }
        }

        /// <summary>
        /// Ensure the conversion parameters are valid.
        /// </summary>
        /// <param name="conversionParams">The conversion parameters to validate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the input image rect exceeds the actual
        /// image dimensions or if the output dimensions exceed the input dimensions.</exception>
        /// <exception cref="System.ArgumentException">Thrown if the texture format is not suppported</exception>
        /// <seealso cref="FormatSupported"/>
        void ValidateConversionParamsAndThrow(XRCameraImageConversionParams conversionParams)
        {
            if ((conversionParams.inputRect.x + conversionParams.inputRect.width > width) ||
                (conversionParams.inputRect.y + conversionParams.inputRect.height > height))
            {
                throw new ArgumentOutOfRangeException(
                    "conversionParams.inputRect",
                    "Input rect must be completely within the original image.");
            }

            if ((conversionParams.outputDimensions.x > conversionParams.inputRect.width) ||
                (conversionParams.outputDimensions.y > conversionParams.inputRect.height))
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Output dimensions must be less than or equal to the inputRect's dimensions: ({0}x{1} > {2}x{3}).",
                    conversionParams.outputDimensions.x, conversionParams.outputDimensions.y,
                    conversionParams.inputRect.width, conversionParams.inputRect.height));
            }

            if (!FormatSupported(conversionParams.outputFormat))
            {
                throw new ArgumentException("TextureFormat not supported.", "conversionParams.format");
            }
        }

        /// <summary>
        /// Dispose native resources associated with this request, including the raw image data. Any
        /// <see cref="XRCameraImagePlane"/>s returned by <see cref="GetPlane"/> are invalidated immediately after
        /// calling <c>Dispose</c>.
        /// </summary>
        public void Dispose()
        {
            if (m_CameraSubsystem == null || m_NativeHandle == 0)
            {
                return;
            }

            m_CameraSubsystem.DisposeImage(m_NativeHandle);
            m_NativeHandle = 0;
            m_CameraSubsystem = null;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.Release(m_SafetyHandle);
#endif
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = width.GetHashCode();
                hash = hash * 486187739 + height.GetHashCode();
                hash = hash * 486187739 + planeCount.GetHashCode();
                hash = hash * 486187739 + m_NativeHandle.GetHashCode();
                hash = hash * 486187739 + ((int)format).GetHashCode();
                if (m_CameraSubsystem != null)
                {
                    hash = hash * 486187739 + m_CameraSubsystem.GetHashCode();
                }
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return ((obj is XRCameraImage) && Equals((XRCameraImage)obj));
        }

        public bool Equals(XRCameraImage other)
        {
            return
                (width == other.width) &&
                (height == other.height) &&
                (planeCount == other.planeCount) &&
                (format == other.format) &&
                (m_NativeHandle == other.m_NativeHandle) &&
                (m_CameraSubsystem == other.m_CameraSubsystem);
        }

        public static bool operator ==(XRCameraImage lhs, XRCameraImage rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRCameraImage lhs, XRCameraImage rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return string.Format(
                "(Width: {0}, Height: {1}, PlaneCount: {2}, Format: {3})",
                width, height, planeCount, format);
        }
    }
}
