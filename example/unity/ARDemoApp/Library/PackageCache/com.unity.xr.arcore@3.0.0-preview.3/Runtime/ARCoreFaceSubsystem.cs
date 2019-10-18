using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Implementation of <c>XRFaceSubsystem</c> for ARCore. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public class ARCoreFaceSubsystem : XRFaceSubsystem
    {
        /// <summary>
        /// Get all the available <see cref="ARCoreFaceRegion"/>s.
        /// </summary>
        /// <param name="trackableId">The id associated with the face to query.</param>
        /// <param name="allocator">The allocator to use if <paramref name="regions"/> requires a resize. C# Jobs are used, so the allocator
        /// must be either <c>Allocator.TempJob</c> or <c>Allocator.Persistent</c>.</param>
        /// <param name="regions">An array of <see cref="ARCoreFaceRegionData"/>s. If <paramref name="regions"/> is allocated
        /// and the correct size, then its memory is reused. Otherwise, it is reallocated.</param>
        /// <exception cref="System.InvalidOperationException">Thrown if <paramref name="allocator"/> is <c>Allocator.Temp</c>.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <paramref name="allocator"/> is <c>Allocator.None</c>.</exception>
        public unsafe void GetRegionPoses(
            TrackableId trackableId,
            Allocator allocator,
            ref NativeArray<ARCoreFaceRegionData> regions)
        {
            if (allocator == Allocator.Temp)
                throw new InvalidOperationException("Allocator.Temp is not supported. Use Allocator.TempJob if you wish to use a temporary allocator.");

            if (allocator == Allocator.None)
                throw new InvalidOperationException("Allocator.None is not a valid allocator.");

            int count;
            var regionPtr = UnityARCore_faceTracking_acquireRegions(trackableId, out count);

            if (regionPtr == null)
            {
                if (regions.IsCreated)
                    regions.Dispose();
                regions = default(NativeArray<ARCoreFaceRegionData>);
                return;
            }

            try
            {
                // Resize regions if necessary
                if (regions.IsCreated)
                {
                    if (regions.Length != count)
                    {
                        regions.Dispose();
                        regions = new NativeArray<ARCoreFaceRegionData>(count, allocator);
                    }
                }
                else
                {
                    regions = new NativeArray<ARCoreFaceRegionData>(count, allocator);
                }

                new TransformPoseJob
                {
                    regionsIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<FaceRegionWithARCorePose>(regionPtr, count, Allocator.None),
                    regionsOut = regions
                }.Schedule(count, 1).Complete();
            }
            finally
            {
                UnityARCore_faceTracking_deallocateTemp(regionPtr);
            }
        }

        // ARCore stores its poses as (rotation, position) whereas a
        // UnityEngine.Pose is (position, rotation).
        struct ARCorePose
        {
            public Quaternion rotation;
            public Vector3 position;

            public ARCorePose(Vector3 position, Quaternion rotation)
            {
                this.position = position;
                this.rotation = rotation;
            }
        }

        struct FaceRegionWithARCorePose
        {
            public ARCoreFaceRegion regionType;
            public ARCorePose pose;

            public FaceRegionWithARCorePose(ARCoreFaceRegion regionType, ARCorePose pose)
            {
                this.regionType = regionType;
                this.pose = pose;
            }
        }

        struct TransformPoseJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<FaceRegionWithARCorePose> regionsIn;

            [WriteOnly]
            public NativeArray<ARCoreFaceRegionData> regionsOut;

            public void Execute(int index)
            {
                var arcorePose = regionsIn[index].pose;
                var position = arcorePose.position;
                var rotation = arcorePose.rotation;

                // Flip handedness
                var pose = new Pose(
                    new Vector3(position.x, position.y, -position.z),
                    new Quaternion(-rotation.x, -rotation.y, rotation.z, rotation.w));

                regionsOut[index] = new ARCoreFaceRegionData(
                    regionsIn[index].regionType, pose);
            }
        }

        protected override Provider CreateProvider() => new ARCoreProvider();

        class ARCoreProvider : Provider
        {
            public override void Start() => UnityARCore_faceTracking_Start();

            public override void Stop() => UnityARCore_faceTracking_Stop();

            public override void Destroy() => UnityARCore_faceTracking_Destroy();

            public unsafe override void GetFaceMesh(
                TrackableId faceId,
                Allocator allocator,
                ref XRFaceMesh faceMesh)
            {
                int vertexCount, triangleCount;
                void* vertexPtr, normalPtr, indexPtr, uvPtr;
                if (!UnityARCore_faceTracking_TryGetFaceData(
                    faceId,
                    out vertexPtr, out normalPtr, out uvPtr, out vertexCount,
                    out indexPtr, out triangleCount))
                {
                    faceMesh.Dispose();
                    faceMesh = default(XRFaceMesh);
                    return;
                }

                faceMesh.Resize(
                    vertexCount,
                    triangleCount,
                    XRFaceMesh.Attributes.Normals | XRFaceMesh.Attributes.UVs,
                    allocator);

                var vertexJobHandle = new TransformVerticesJob
                {
                    verticesIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector3>(vertexPtr, vertexCount, Allocator.None),
                    verticesOut = faceMesh.vertices
                }.Schedule(vertexCount, 32);

                var normalJobHandle = new TransformVerticesJob
                {
                    verticesIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector3>(normalPtr, vertexCount, Allocator.None),
                    verticesOut = faceMesh.normals
                }.Schedule(vertexCount, 32);

                var uvJobHandle = new TransformUVsJob
                {
                    uvsIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector2>(uvPtr, vertexCount, Allocator.None),
                    uvsOut = faceMesh.uvs
                }.Schedule(vertexCount, 32);

                var indexJobHandle = new TransformIndicesJob
                {
                    triangleIndicesIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Triangle<ushort>>(indexPtr, triangleCount, Allocator.None),
                    // "cast" it to an array of Triangles
                    triangleIndicesOut = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Triangle<int>>(faceMesh.indices.GetUnsafePtr(), triangleCount, Allocator.None)
                }.Schedule(triangleCount, 32);

                // Wait on all four
                JobHandle.CombineDependencies(
                    JobHandle.CombineDependencies(vertexJobHandle, normalJobHandle),
                    indexJobHandle, uvJobHandle).Complete();
            }

            struct TransformVerticesJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Vector3> verticesIn;

                [WriteOnly]
                public NativeArray<Vector3> verticesOut;

                public void Execute(int i)
                {
                    verticesOut[i] = new Vector3(
                         verticesIn[i].x,
                         verticesIn[i].y,
                        -verticesIn[i].z);
                }
            }

            struct TransformUVsJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Vector2> uvsIn;

                [WriteOnly]
                public NativeArray<Vector2> uvsOut;

                public void Execute(int i)
                {
                    uvsOut[i] = new Vector2(
                        uvsIn[i].x,
                        uvsIn[i].y);
                }
            }

            struct Triangle<T> where T : struct
            {
                public T a;
                public T b;
                public T c;

                public Triangle(T a, T b, T c)
                {
                    this.a = a;
                    this.b = c;
                    this.c = b;
                }
            }

            struct TransformIndicesJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Triangle<ushort>> triangleIndicesIn;

                [WriteOnly]
                public NativeArray<Triangle<int>> triangleIndicesOut;

                public void Execute(int i)
                {
                    triangleIndicesOut[i] = new Triangle<int>(
                        triangleIndicesIn[i].a,
                        triangleIndicesIn[i].b,
                        triangleIndicesIn[i].c);
                }
            }

            public unsafe override TrackableChanges<XRFace> GetChanges(
                XRFace defaultFace,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedLength, updatedLength, removedLength, elementSize;
                var context = UnityARCore_faceTracking_AcquireChanges(
                    out addedPtr, out addedLength,
                    out updatedPtr, out updatedLength,
                    out removedPtr, out removedLength,
                    out elementSize);

                try
                {
                    return new TrackableChanges<XRFace>(
                        addedPtr, addedLength,
                        updatedPtr, updatedLength,
                        removedPtr, removedLength,
                        defaultFace, elementSize,
                        allocator);
                }
                finally
                {
                    UnityARCore_faceTracking_ReleaseChanges(context);
                }
            }
        }

        [DllImport("UnityARCore")]
        static extern void UnityARCore_faceTracking_Start();

        [DllImport("UnityARCore")]
        static extern void UnityARCore_faceTracking_Stop();

        [DllImport("UnityARCore")]
        static extern void UnityARCore_faceTracking_Destroy();

        [DllImport("UnityARCore")]
        static extern unsafe bool UnityARCore_faceTracking_TryGetFaceData(
            TrackableId faceId,
            out void* vertexPtr, out void* normalPtr, out void* uvPtr, out int vertexCount,
            out void* indexPtr, out int triangleCount);

        [DllImport("UnityARCore")]
       static extern unsafe void* UnityARCore_faceTracking_AcquireChanges(
           out void* addedPtr, out int addedCount,
           out void* updatedPtr, out int updatedCount,
           out void* removedPtr, out int removedCount,
           out int elementSize);

        [DllImport("UnityARCore")]
        static extern unsafe void UnityARCore_faceTracking_ReleaseChanges(void* changes);

        [DllImport("UnityARCore")]
        static extern unsafe void* UnityARCore_faceTracking_acquireRegions(
            TrackableId trackableId,
            out int count);

        [DllImport("UnityARCore")]
        static extern unsafe void UnityARCore_faceTracking_deallocateTemp(void* regions);

        // this method is run on startup of the app to register this provider with XR Subsystem Manager
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var descriptorParams = new FaceSubsystemParams
            {
                supportsFacePose = true,
                supportsFaceMeshVerticesAndIndices = true,
                supportsFaceMeshUVs = true,
                supportsFaceMeshNormals = true,
                id = "ARCore-Face",
                subsystemImplementationType = typeof(ARCoreFaceSubsystem)
            };

            XRFaceSubsystemDescriptor.Create(descriptorParams);
#endif
        }
    }
}
