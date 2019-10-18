using System;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents an asynchronous world map request.
    /// Use this to determine the status of the request,
    /// and get the <see cref="ARWorldMap"/> once the request is complete.
    /// </summary>
    public struct ARWorldMapRequest : IDisposable, IEquatable<ARWorldMapRequest>
    {
        /// <summary>
        /// Get the status of the request.
        /// </summary>
        public ARWorldMapRequestStatus status
        {
            get
            {
                return Api.UnityARKit_getWorldMapRequestStatus(m_RequestId);
            }
        }

        /// <summary>
        /// Retrieve the <see cref="ARWorldMap"/>.
        /// It is an error to call this method when <see cref="status"/> is
        /// not <see cref="ARWorldMapRequestStatus.Success"/>.
        /// </summary>
        /// <returns>An <see cref="ARWorldMap"/> representing the state of the session at the time the request was made.</returns>
        public ARWorldMap GetWorldMap()
        {
            if (status != ARWorldMapRequestStatus.Success)
                throw new InvalidOperationException("Cannot GetWorldMap unless status is ARWorldMapRequestStatus.Success.");

            var worldMapId = Api.UnityARKit_getWorldMapIdFromRequestId(m_RequestId);
            if (worldMapId == ARWorldMap.k_InvalidHandle)
                throw new InvalidOperationException("Internal error.");

            return new ARWorldMap(worldMapId);
        }

        /// <summary>
        /// Dispose of the request. You must dispose of the request to avoid
        /// leaking resources.
        /// </summary>
        public void Dispose()
        {
            Api.UnityARKit_disposeWorldMapRequest(m_RequestId);
        }

        public override int GetHashCode()
        {
            return m_RequestId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARWorldMapRequest))
                return false;

            return Equals((ARWorldMapRequest)obj);
        }

        public bool Equals(ARWorldMapRequest other)
        {
            return (m_RequestId == other.m_RequestId);
        }

        public static bool operator ==(ARWorldMapRequest lhs, ARWorldMapRequest rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARWorldMapRequest lhs, ARWorldMapRequest rhs)
        {
            return !lhs.Equals(rhs);
        }

        internal ARWorldMapRequest(int requestId)
        {
            m_RequestId = requestId;
        }

        int m_RequestId;
    }
}
