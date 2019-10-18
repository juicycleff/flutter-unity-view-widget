using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Holds information related to an asynchronous camera image conversion request. Returned by
    /// <see cref="XRCameraImage.ConvertAsync"/>.
    /// </summary>
    public struct XRAsyncCameraImageConversion : IDisposable, IEquatable<XRAsyncCameraImageConversion>
    {
        XRCameraSubsystem m_CameraSubsystem;
        int m_RequestId;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle m_SafetyHandle;
#endif

        /// <summary>
        /// The <see cref="XRCameraImageConversionParams"/> used during the conversion.
        /// </summary>
        /// <value>
        /// The parameters used during the conversion.
        /// </value>
        public XRCameraImageConversionParams conversionParams { get; private set; }

        /// <summary>
        /// The status of the request.
        /// </summary>
        /// <value>
        /// The status of the request.
        /// </value>
        public AsyncCameraImageConversionStatus status
        {
            get
            {
                if (m_CameraSubsystem == null)
                {
                    return AsyncCameraImageConversionStatus.Disposed;
                }

                return m_CameraSubsystem.GetAsyncRequestStatus(m_RequestId);
            }
        }

        /// <summary>
        /// Start the image conversion using this class to interact with the asynchronous conversion and results.
        /// </summary>
        /// <param name="cameraSubsystem">The camera subsystem performing the image conversion.</param>
        /// <param name="nativeHandle">The native handle for the camera image.</param>
        /// <param name="conversionParams">The parameters for image conversion.</param>
        internal XRAsyncCameraImageConversion(XRCameraSubsystem cameraSubsystem, int nativeHandle,
                                              XRCameraImageConversionParams conversionParams)
        {
            m_CameraSubsystem = cameraSubsystem;
            m_RequestId = m_CameraSubsystem.ConvertAsync(nativeHandle, conversionParams);
            this.conversionParams = conversionParams;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_SafetyHandle = AtomicSafetyHandle.Create();
#endif
        }

        /// <summary>
        /// Get the raw image data. The returned <c>NativeArray</c> is a direct "view" into the native memory. The
        /// memory is only valid until this <see cref="XRAsyncCameraImageConversion"/> is disposed.
        /// </summary>
        /// <typeparam name="T">The type of data to return. No conversion is performed based on the type; this is
        /// merely for access convenience.</typeparam>
        /// <returns>
        /// A new <c>NativeArray</c> representing the raw image data. This method may fail; use
        /// <c>NativeArray.IsCreated</c> to determine the validity of the data.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the asynchronous conversion
        /// <see cref="status"/> is not <see cref="AsyncCameraImageConversionStatus.Ready"/> or if the conversion is
        /// invalid.</exception>
        public unsafe NativeArray<T> GetData<T>() where T : struct
        {
            if (status != AsyncCameraImageConversionStatus.Ready)
                throw new InvalidOperationException("Async request is not ready.");

            IntPtr dataPtr;
            int dataLength;
            if (m_CameraSubsystem.TryGetAsyncRequestData(m_RequestId, out dataPtr, out dataLength))
            {
                int stride = UnsafeUtility.SizeOf<T>();
                var array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(
                    (void*)dataPtr, dataLength / stride, Allocator.None);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, m_SafetyHandle);
#endif
                return array;
            }

            throw new InvalidOperationException("The XRAsyncCameraImageConversion is not valid.");
        }

        /// <summary>
        /// Dispose native resources associated with this request, including the raw image data. The <c>NativeArray</c>
        /// returned by <see cref="GetData"/> is invalidated immediately after calling <c>Dispose</c>.
        /// </summary>
        public void Dispose()
        {
            if (m_CameraSubsystem == null || m_RequestId == 0)
                return;

            m_CameraSubsystem.DisposeAsyncRequest(m_RequestId);
            m_CameraSubsystem = null;
            m_RequestId = 0;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.Release(m_SafetyHandle);
#endif
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = conversionParams.GetHashCode();
                hash = hash * 486187739 + m_RequestId.GetHashCode();
                if (m_CameraSubsystem != null)
                    hash = hash * 486187739 + m_CameraSubsystem.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return ((obj is XRAsyncCameraImageConversion) && Equals((XRAsyncCameraImageConversion)obj));
        }

        public bool Equals(XRAsyncCameraImageConversion other)
        {
            return
                (conversionParams.Equals(other.conversionParams)) &&
                (m_RequestId == other.m_RequestId) &&
                (m_CameraSubsystem == other.m_CameraSubsystem);
        }

        public static bool operator ==(XRAsyncCameraImageConversion lhs, XRAsyncCameraImageConversion rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRAsyncCameraImageConversion lhs, XRAsyncCameraImageConversion rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return string.Format("ConversionParams: {0}", conversionParams);
        }
    }
}
