namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the status of an asynchronous camera image request.
    /// </summary>
    public enum AsyncCameraImageConversionStatus
    {
        /// <summary>
        /// The request is not valid or has been disposed.
        /// </summary>
        Disposed,

        /// <summary>
        /// The request is waiting to be processed.
        /// </summary>
        Pending,

        /// <summary>
        /// The request is currently being processed.
        /// </summary>
        Processing,

        /// <summary>
        /// The request succeeded and image data is ready.
        /// </summary>
        Ready,

        /// <summary>
        /// The request failed. No data is available.
        /// </summary>
        Failed
    }

    /// <summary>
    /// Extension methods for the <see cref="AsyncCameraImageConversionStatus"/> enum.
    /// </summary>
    public static class XRAsyncCameraImageConversionStatusExtensions
    {
        /// <summary>
        /// Whether the request has completed. It may have completed with an error or be
        /// an invalid / disposed request. See <see cref="IsError(AsyncCameraImageConversionStatus)"/>.
        /// </summary>
        /// <param name="status">The <see cref="AsyncCameraImageConversionStatus"/> being extended.</param>
        /// <returns><c>true</c> if the <see cref="AsyncCameraImageConversionStatus"/> has completed.</returns>
        public static bool IsDone(this AsyncCameraImageConversionStatus status)
        {
            switch (status)
            {
                case AsyncCameraImageConversionStatus.Pending:
                case AsyncCameraImageConversionStatus.Processing:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Whether the request status represents an error. If the request has been disposed, <c>IsError</c>
        /// will be <c>true</c>.
        /// </summary>
        /// <param name="status">The <see cref="AsyncCameraImageConversionStatus"/> being extended.</param>
        /// <returns><c>true</c> if the <see cref="AsyncCameraImageConversionStatus"/> represents an error.</returns>
        public static bool IsError(this AsyncCameraImageConversionStatus status)
        {
            switch (status)
            {
                case AsyncCameraImageConversionStatus.Pending:
                case AsyncCameraImageConversionStatus.Processing:
                case AsyncCameraImageConversionStatus.Ready:
                    return false;
                default:
                    return true;
            }
        }
    }
}
