using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A manager for <see cref="ARTrackedObject"/>s. Uses the <c>XRDepthSubsystem</c>
    /// to recognize and track depth data in the physical environment.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_PointCloudManager)]
    [RequireComponent(typeof(ARSessionOrigin))]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARPointCloudManager.html")]
    public class ARPointCloudManager : ARTrackableManager<
        XRDepthSubsystem,
        XRDepthSubsystemDescriptor,
        XRPointCloud,
        ARPointCloud>, IRaycaster
    {
        [SerializeField]
        [Tooltip("If not null, instantiates this prefab for each point cloud.")]
        GameObject m_PointCloudPrefab;

        /// <summary>
        /// Getter/setter for the Point Cloud Prefab.
        /// </summary>
        public GameObject pointCloudPrefab
        {
            get { return m_PointCloudPrefab; }
            set { m_PointCloudPrefab = value; }
        }

        /// Invoked once per frame with information about the <see cref="ARTrackedObject"/>s that have changed, i.e., been added, updated, or removed.
        /// This happens just before <see cref="ARTrackedObject"/>s are destroyed, so you can set <c>ARTrackedObject.destroyOnRemoval</c> to <c>false</c>
        /// from this event to suppress this behavior.
        public event Action<ARPointCloudChangedEventArgs> pointCloudsChanged;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (subsystem != null)
            {
                var raycastManager = GetComponent<ARRaycastManager>();
                if (raycastManager != null)
                    raycastManager.RegisterRaycaster(this);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            var raycastManager = GetComponent<ARRaycastManager>();
            if (raycastManager != null)
                raycastManager.UnregisterRaycaster(this);
        }

        /// <summary>
        /// ARTrackableManager interface.
        /// </summary>
        protected override GameObject GetPrefab()
        {
            return m_PointCloudPrefab;
        }

        /// <summary>
        /// The name to be used for the <c>GameObject</c> whenever a new Object is detected.
        /// </summary>
        protected override string gameObjectName
        {
            get { return "ARPointCloud"; }
        }

        protected override void OnAfterSetSessionRelativeData(
            ARPointCloud pointCloud,
            XRPointCloud sessionRelativeData)
        {
            pointCloud.UpdateData(subsystem);
        }

        /// <summary>
        /// Invokes the <see cref="pointCloudsChanged"/> event.
        /// </summary>
        /// <param name="added">A list of objects added this frame.</param>
        /// <param name="updated">A list of objects updated this frame.</param>
        /// <param name="removed">A list of objects removed this frame.</param>
        protected override void OnTrackablesChanged(
            List<ARPointCloud> added,
            List<ARPointCloud> updated,
            List<ARPointCloud> removed)
        {
            if (pointCloudsChanged != null)
                pointCloudsChanged(
                    new ARPointCloudChangedEventArgs(
                        added,
                        updated,
                        removed));
        }

        /// <summary>
        /// Implementation for the <c>IRaycaster</c> interface. Raycasts against every point cloud.
        /// </summary>
        /// <param name="rayInSessionSpace">A <c>Ray</c>, in session space.</param>
        /// <param name="trackableTypeMask">The type of trackables to raycast against.
        /// If <c>TrackableType.FeaturePoint</c> is not set, this method returns an empty array.</param>
        /// <param name="allocator">The allocator to use for the returned <c>NativeArray</c>.</param>
        /// <returns>A new <c>NativeArray</c>, allocated using <paramref name="allocator"/>, containing
        /// a list of <c>XRRaycastHit</c>s of points hit by the raycast.</returns>
        public NativeArray<XRRaycastHit> Raycast(
            Ray rayInSessionSpace,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            if ((trackableTypeMask & TrackableType.FeaturePoint) == TrackableType.None)
                return new NativeArray<XRRaycastHit>(0, allocator);

            // TODO: Expose this as a property
            float raycastAngleInRadians = Mathf.Deg2Rad * 5f;

            var trackableCollection = trackables;

            var allHits = new NativeArray<XRRaycastHit>(0, allocator);

            foreach (var pointCloud in trackableCollection)
            {
                // Collect the points in the point cloud
                if (!pointCloud.positions.HasValue)
                    continue;

                var points = pointCloud.positions.Value;

                var sessionSpacePose = new Pose(
                    pointCloud.transform.localPosition,
                    pointCloud.transform.localRotation);

                var invRotation = Quaternion.Inverse(sessionSpacePose.rotation);

                // Get the ray in "point cloud space", i.e., relative to the point cloud's local transform
                var ray = new Ray(
                    invRotation * (rayInSessionSpace.origin - sessionSpacePose.position),
                    invRotation * rayInSessionSpace.direction);

                // Perform the raycast against each point
                var infos = new NativeArray<PointCloudRaycastInfo>(points.Length, Allocator.TempJob);
                var raycastJob = new PointCloudRaycastJob
                {
                    points = points,
                    ray = ray,
                    infoOut = infos
                };
                var raycastHandle = raycastJob.Schedule(infos.Length, 1);

                // Collect the hits
                using (var hitBuffer = new NativeArray<XRRaycastHit>(infos.Length, Allocator.TempJob))
                using (infos)
                using (var count = new NativeArray<int>(1, Allocator.TempJob))
                {
                    var collectResultsJob = new PointCloudRaycastCollectResultsJob
                    {
                        points = points,
                        infos = infos,
                        hits = hitBuffer,
                        cosineThreshold = Mathf.Cos(raycastAngleInRadians * .5f),
                        pose = sessionSpacePose,
                        trackableId = pointCloud.trackableId,
                        count = count
                    };
                    var collectResultsHandle = collectResultsJob.Schedule(raycastHandle);

                    // Wait for it to finish
                    collectResultsHandle.Complete();

                    // Copy out the results
                    Append(ref allHits, hitBuffer, count[0], allocator);
                }
            }

            return allHits;
        }

        static void Append<T>(
            ref NativeArray<T> currentArray,
            NativeArray<T> arrayToAppend,
            int lengthToCopy,
            Allocator allocator) where T : struct
        {
            var dstArray = new NativeArray<T>(currentArray.Length + lengthToCopy, allocator);
            NativeArray<T>.Copy(currentArray, dstArray);
            NativeArray<T>.Copy(arrayToAppend, 0, dstArray, currentArray.Length, lengthToCopy);
            currentArray.Dispose();
            currentArray = dstArray;
        }

        struct PointCloudRaycastInfo
        {
            public float distance;
            public float cosineAngleWithRay;
        }

        struct PointCloudRaycastJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeSlice<Vector3> points;

            [WriteOnly]
            public NativeArray<PointCloudRaycastInfo> infoOut;

            public Ray ray;

            public void Execute(int i)
            {
                var originToPoint = points[i] - ray.origin;
                float distance = originToPoint.magnitude;
                var info = new PointCloudRaycastInfo
                {
                    distance = distance,
                    cosineAngleWithRay = Vector3.Dot(originToPoint, ray.direction) / distance
                };

                infoOut[i] = info;
            }
        }

        struct PointCloudRaycastCollectResultsJob : IJob
        {
            [ReadOnly]
            public NativeSlice<Vector3> points;

            [ReadOnly]
            public NativeArray<PointCloudRaycastInfo> infos;

            [WriteOnly]
            public NativeArray<XRRaycastHit> hits;

            [WriteOnly]
            public NativeArray<int> count;

            public float cosineThreshold;

            public Pose pose;

            public TrackableId trackableId;

            public void Execute()
            {
                var hitIndex = 0;
                for (int i = 0; i < points.Length; ++i)
                {
                    if (infos[i].cosineAngleWithRay >= cosineThreshold)
                    {
                        hits[hitIndex++] = new XRRaycastHit(
                            trackableId,
                            new Pose(pose.rotation * points[i] + pose.position, Quaternion.identity),
                            infos[i].distance,
                            TrackableType.FeaturePoint);
                    }
                }

                count[0] = hitIndex;
            }
        }
    }
}
