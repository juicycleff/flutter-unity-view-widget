namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The status of a session serialization request.
    /// See also <see cref="AsyncSessionSaveRequest"/> and <see cref="AsyncSessionLoadRequest"/>.
    /// </summary>
    public enum ARWorldMapRequestStatus
    {
        /// <summary>
        /// The request is not valid.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The request is pending.
        /// </summary>
        Pending,

        /// <summary>
        /// The request succeeded successfully.
        /// </summary>
        Success,

        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        ErrorUnknown,

        /// <summary>
        /// The request failed because serialization is not supported.
        /// </summary>
        ErrorNotSupported,

        /// <summary>
        /// The request failed because the supplied data was not valid.
        /// </summary>
        ErrorBadData,

        /// <summary>
        /// The request failed because there are insufficient features to
        /// serialize a session. The API requires more data; try scanning more of
        /// the environment.
        /// </summary>
        ErrorInsufficientFeatures
    }

    /// <summary>
    /// Extension methods for the <see cref="ARWorldMapRequestStatus"/> enum.
    /// </summary>
    public static class ARWorldMapRequestStatusExtensions
    {
        /// <summary>
        /// Whether the serialization request is complete. <c>IsDone</c> does not mean the
        /// request completed successfully. See <see cref="HasError(ARWorldMapRequestStatus)"/>.
        /// </summary>
        /// <param name="status">The <see cref="ARWorldMapRequestStatus"/> being extended.</param>
        /// <returns><c>true</c> if <paramref name="status"/> indicates the request has completed.</returns>
        public static bool IsDone(this ARWorldMapRequestStatus status)
        {
            return status != ARWorldMapRequestStatus.Pending;
        }

        /// <summary>
        /// Whether the serialization request completed with an error.
        /// </summary>
        /// <param name="status">The <see cref="ARWorldMapRequestStatus"/> being extended.</param>
        /// <returns><c>true</c> if <paramref name="status"/> indicates an error status.</returns>
        public static bool IsError(this ARWorldMapRequestStatus status)
        {
            switch (status)
            {
                case ARWorldMapRequestStatus.Pending:
                case ARWorldMapRequestStatus.Success:
                    return false;
                default:
                    return true;
            }
        }
    }
}
