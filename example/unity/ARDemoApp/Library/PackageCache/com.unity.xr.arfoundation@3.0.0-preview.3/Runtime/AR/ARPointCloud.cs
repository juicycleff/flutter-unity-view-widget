using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a detected point cloud, aka feature points.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_PointCloud)]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARPointCloud.html")]
    public class ARPointCloud : ARTrackable<XRPointCloud, ARPointCloud>
    {
        /// <summary>
        /// Invoked whenever the point cloud is updated.
        /// </summary>
        public event Action<ARPointCloudUpdatedEventArgs> updated;

        /// <summary>
        /// An array of positions for each point in the point cloud.
        /// This array is parallel to <see cref="identifiers"/> and
        /// <see cref="confidenceValues"/>. Positions are provided in
        /// point cloud space, that is, relative to this <see cref="ARPointCloud"/>'s
        /// local position and rotation.
        /// </summary>
        public NativeSlice<Vector3>? positions
        {
            get
            {
                if (m_Data.positions.IsCreated)
                {
                    return m_Data.positions;
                }

                return null;
            }
        }

        /// <summary>
        /// An array of identifiers for each point in the point cloud.
        /// This array is parallel to <see cref="positions"/> and
        /// <see cref="confidenceValues"/>.
        /// </summary>
        public NativeSlice<ulong>? identifiers
        {
            get
            {
                if (m_Data.identifiers.IsCreated)
                {
                    return m_Data.identifiers;
                }

                return null;
            }
        }

        /// <summary>
        /// An array of confidence values for each point in the point cloud
        /// ranging from 0..1.
        /// This array is parallel to <see cref="positions"/> and
        /// <see cref="identifiers"/>. Check for existence with
        /// <c>confidenceValues.IsCreated</c>.
        /// </summary>
        public NativeArray<float>? confidenceValues
        {
            get
            {
                if (m_Data.confidenceValues.IsCreated)
                {
                    return m_Data.confidenceValues;
                }

                return null;
            }
        }

        void Update()
        {
            if (m_PointsUpdated && updated != null)
            {
                m_PointsUpdated = false;
                updated(new ARPointCloudUpdatedEventArgs());
            }
        }

        void OnDestroy()
        {
            m_Data.Dispose();
        }

        internal void UpdateData(XRDepthSubsystem subsystem)
        {
            m_Data.Dispose();
            m_Data = subsystem.GetPointCloudData(trackableId, Allocator.Persistent);
            m_PointsUpdated = m_Data.positions.IsCreated;
        }

        XRPointCloudData m_Data;

        bool m_PointsUpdated = false;
    }
}
