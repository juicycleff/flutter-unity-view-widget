using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Serialization;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A manager for <see cref="ARPlane"/>s. Creates, updates, and removes
    /// <c>GameObject</c>s in response to detected surfaces in the physical
    /// environment.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_PlaneManager)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ARSessionOrigin))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARPlaneManager.html")]
    public sealed class ARPlaneManager : ARTrackableManager<
        XRPlaneSubsystem,
        XRPlaneSubsystemDescriptor,
        BoundedPlane,
        ARPlane>, IRaycaster
    {
        [SerializeField]
        [Tooltip("If not null, instantiates this prefab for each created plane.")]
        GameObject m_PlanePrefab;

        /// <summary>
        /// Getter/setter for the Plane Prefab.
        /// </summary>
        public GameObject planePrefab
        {
            get { return m_PlanePrefab; }
            set { m_PlanePrefab = value; }
        }

        [SerializeField, PlaneDetectionModeMask]
        [Tooltip("The types of planes to detect.")]
        [FormerlySerializedAs("PlaneDetectionFlags")]
        PlaneDetectionMode m_DetectionMode = k_PlaneDetectionModeEverything;

        /// <summary>
        /// Get or set the <c>PlaneDetectionMode</c> to use for plane detection.
        /// </summary>
        public PlaneDetectionMode detectionMode
        {
            get
            {
                if (m_DetectionMode == k_PlaneDetectionModeEverything)
                    return PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical;

                return m_DetectionMode;
            }
            set
            {
                m_DetectionMode = value;

                if (subsystem != null)
                    subsystem.planeDetectionMode = detectionMode;
            }
        }

        /// <summary>
        /// Invoked when planes have changed (been added, updated, or removed).
        /// </summary>
        public event Action<ARPlanesChangedEventArgs> planesChanged;

        /// <summary>
        /// Attempt to retrieve an existing <see cref="ARPlane"/> by <paramref name="trackableId"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> of the plane to retrieve.</param>
        /// <returns>The <see cref="ARPlane"/> with <paramref name="trackableId"/>, or <c>null</c> if it does not exist.</returns>
        public ARPlane GetPlane(TrackableId trackableId)
        {
            ARPlane plane;
            if (m_Trackables.TryGetValue(trackableId, out plane))
                return plane;

            return null;
        }

        /// <summary>
        /// Performs a raycast against all currently tracked planes.
        /// </summary>
        /// <param name="ray">The ray, in Unity world space, to cast.</param>
        /// <param name="trackableTypeMask">A mask of raycast types to perform.</param>
        /// <param name="allocator">The <c>Allocator</c> to use when creating the returned <c>NativeArray</c>.</param>
        /// <returns>
        /// A new <c>NativeArray</c> of raycast results allocated with <paramref name="allocator"/>.
        /// The caller owns the memory and is responsible for calling <c>Dispose</c> on the <c>NativeArray</c>.
        /// </returns>
        /// <seealso cref="ARRaycastManager.Raycast(Ray, List{ARRaycastHit}, TrackableType)"/>
        /// <seealso cref="ARRaycastManager.Raycast(Vector2, List{ARRaycastHit}, TrackableType)"/>
        public NativeArray<XRRaycastHit> Raycast(
            Ray ray,
            TrackableType trackableTypeMask,
            Allocator allocator)
        {
            // No plane types requested; early out.
            if ((trackableTypeMask & TrackableType.Planes) == TrackableType.None)
                return new NativeArray<XRRaycastHit>(0, allocator);

            var trackableCollection = trackables;

            // Allocate a buffer that is at least large enough to contain a hit against every plane
            var hitBuffer = new NativeArray<XRRaycastHit>(trackableCollection.count, Allocator.Temp);
            try
            {
                int count = 0;
                foreach (var plane in trackableCollection)
                {
                    TrackableType trackableTypes = TrackableType.None;

                    var normal = plane.transform.localRotation * Vector3.up;
                    var infinitePlane = new Plane(normal, plane.transform.localPosition);
                    float distance;
                    if (!infinitePlane.Raycast(ray, out distance))
                        continue;

                    // Pose in session space
                    var pose = new Pose(
                        ray.origin + ray.direction * distance,
                        plane.transform.localRotation);

                    if ((trackableTypeMask & TrackableType.PlaneWithinInfinity) != TrackableType.None)
                        trackableTypes |= TrackableType.PlaneWithinInfinity;

                    // To test the rest, we need the intersection point in plane space
                    var hitPositionPlaneSpace3d = Quaternion.Inverse(plane.transform.localRotation) * (pose.position - plane.transform.localPosition);
                    var hitPositionPlaneSpace = new Vector2(hitPositionPlaneSpace3d.x, hitPositionPlaneSpace3d.z);

                    var estimatedOrWithinBounds = TrackableType.PlaneWithinBounds | TrackableType.PlaneEstimated;
                    if ((trackableTypeMask & estimatedOrWithinBounds) != TrackableType.None)
                    {
                        var differenceFromCenter = hitPositionPlaneSpace - plane.centerInPlaneSpace;
                        if ((Mathf.Abs(differenceFromCenter.x) <= plane.extents.x) &&
                            (Mathf.Abs(differenceFromCenter.y) <= plane.extents.y))
                        {
                            trackableTypes |= (estimatedOrWithinBounds & trackableTypeMask);
                        }
                    }

                    if ((trackableTypeMask & TrackableType.PlaneWithinPolygon) != TrackableType.None)
                    {
                        if (WindingNumber(hitPositionPlaneSpace, plane.boundary) != 0)
                            trackableTypes |= TrackableType.PlaneWithinPolygon;
                    }

                    if (trackableTypes != TrackableType.None)
                    {
                        hitBuffer[count++] = new XRRaycastHit(
                            plane.trackableId,
                            pose,
                            distance,
                            trackableTypes);
                    }
                }

                // Finally, copy to return value
                var hitResults = new NativeArray<XRRaycastHit>(count, allocator);
                NativeArray<XRRaycastHit>.Copy(hitBuffer, hitResults, count);
                return hitResults;
            }
            finally
            {
                hitBuffer.Dispose();
            }
        }

        static float GetCrossDirection(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        // See http://geomalgorithms.com/a03-_inclusion.html
        static int WindingNumber(
            Vector2 positionInPlaneSpace,
            NativeArray<Vector2> boundaryInPlaneSpace)
        {
            int windingNumber = 0;
            Vector2 point = positionInPlaneSpace;
            for (int i = 0; i < boundaryInPlaneSpace.Length; ++i)
            {
                int j = (i + 1) % boundaryInPlaneSpace.Length;
                Vector2 vi = boundaryInPlaneSpace[i];
                Vector2 vj = boundaryInPlaneSpace[j];

                if (vi.y <= point.y)
                {
                    if (vj.y > point.y)                                     // an upward crossing
                    {
                        if (GetCrossDirection(vj - vi, point - vi) < 0f)    // P left of edge
                            ++windingNumber;
                    }
                    // have  a valid up intersect
                }
                else
                {                                                           // y > P.y (no test needed)
                    if (vj.y <= point.y)                                    // a downward crossing
                    {
                        if (GetCrossDirection(vj - vi, point - vi) > 0f)    // P right of edge
                            --windingNumber;
                    }
                    // have  a valid down intersect
                }
            }

            return windingNumber;
        }

        protected override GameObject GetPrefab()
        {
            return m_PlanePrefab;
        }

        protected override void OnBeforeStart()
        {
            subsystem.planeDetectionMode = detectionMode;
        }

        protected override void OnAfterSetSessionRelativeData(
            ARPlane plane,
            BoundedPlane sessionRelativeData)
        {
            ARPlane subsumedByPlane;
            if (m_Trackables.TryGetValue(sessionRelativeData.subsumedById, out subsumedByPlane))
            {
                plane.subsumedBy = subsumedByPlane;
            }
            else
            {
                plane.subsumedBy = null;
            }

            plane.UpdateBoundary(subsystem);
        }

        protected override void OnTrackablesChanged(
            List<ARPlane> added,
            List<ARPlane> updated,
            List<ARPlane> removed)
        {
            if (planesChanged != null)
            {
                planesChanged(
                    new ARPlanesChangedEventArgs(
                        added,
                        updated,
                        removed));
            }
        }

        /// <summary>
        /// The name to be used for the <c>GameObject</c> whenever a new plane is detected.
        /// </summary>
        protected override string gameObjectName
        {
            get { return "ARPlane"; }
        }

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

        static List<Vector2> s_PlaneSpaceBoundary = new List<Vector2>();

        const PlaneDetectionMode k_PlaneDetectionModeEverything = (PlaneDetectionMode)(-1);
    }
}
