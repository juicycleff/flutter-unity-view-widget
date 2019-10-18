using System;
using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Represents the data (arrays of positions, confidence values, and identifiers) associated with a point cloud.
    /// </summary>
    public struct XRPointCloudData : IEquatable<XRPointCloudData>, IDisposable
    {
        /// <summary>
        /// Positions for each point in the point cloud. This array is parallel
        /// to <see cref="confidenceValues"/> and <see cref="identifiers"/>.
        /// Use <c>positions.IsCreated</c> to check for existence.
        /// </summary>
        public NativeArray<Vector3> positions
        {
            get => m_Positions;
            set => m_Positions = value;
        }
        NativeArray<Vector3> m_Positions;

        /// <summary>
        /// Confidence values for each point in the point cloud. This array is parallel
        /// to <see cref="positions"/> and <see cref="identifiers"/>.
        /// Use <c>confidenceValues.IsCreated</c> to check for existence.
        /// </summary>
        public NativeArray<float> confidenceValues
        {
            get => m_ConfidenceValues;
            set => m_ConfidenceValues = value;
        }
        NativeArray<float> m_ConfidenceValues;

        /// <summary>
        /// Identifiers for each point in the point cloud. This array is parallel
        /// to <see cref="positions"/> and <see cref="confidenceValues"/>.
        /// Use <c>identifiers.IsCreated</c> to check for existence.
        /// </summary>
        /// <remarks>
        /// Identifiers are unique to a particular session, which means you can use
        /// the identifier to match a particular point in the point cloud with a
        /// previously detected point.
        /// </remarks>
        public NativeArray<ulong> identifiers
        {
            get => m_Identifiers;
            set => m_Identifiers = value;
        }
        NativeArray<ulong> m_Identifiers;

        /// <summary>
        /// Disposes of the <c>NativeArray</c>s, checking for existence first.
        /// </summary>
        public void Dispose()
        {
            if (m_Positions.IsCreated)
                m_Positions.Dispose();
            if (m_ConfidenceValues.IsCreated)
                m_ConfidenceValues.Dispose();
            if (m_Identifiers.IsCreated)
                m_Identifiers.Dispose();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_Positions.GetHashCode();
                hash = hash * 486187739 + m_ConfidenceValues.GetHashCode();
                hash = hash * 486187739 + m_Identifiers.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj) => obj is XRPointCloudData && Equals((XRPointCloudData)obj);

        public override string ToString()
        {
            return string.Format("XRPointCloudData: {0} positions {1} confidence values {2} identifiers",
                m_Positions.Length, m_ConfidenceValues.Length, m_Identifiers.Length);
        }

        public bool Equals(XRPointCloudData other)
        {
            return
                m_Positions.Equals(other.m_Positions) &&
                m_ConfidenceValues.Equals(other.m_ConfidenceValues) &&
                m_Identifiers.Equals(other.m_Identifiers);
        }

        public static bool operator ==(XRPointCloudData lhs, XRPointCloudData rhs) => lhs.Equals(rhs);

        public static bool operator !=(XRPointCloudData lhs, XRPointCloudData rhs) => !lhs.Equals(rhs);
    }
}
