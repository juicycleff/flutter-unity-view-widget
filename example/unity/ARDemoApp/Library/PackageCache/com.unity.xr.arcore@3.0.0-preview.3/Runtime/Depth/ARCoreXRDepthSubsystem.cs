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
    /// The ARCore implementation of the <c>XRDepthSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARCoreXRDepthSubsystem : XRDepthSubsystem
    {
        class ARCoreProvider : Provider
        {
            [DllImport("UnityARCore")]
            static extern public void UnityARCore_depth_Initialize();

            [DllImport("UnityARCore")]
            static extern public void UnityARCore_depth_Shutdown();

            [DllImport("UnityARCore")]
            static extern public void UnityARCore_depth_Start(Guid guid);

            [DllImport("UnityARCore")]
            static extern public void UnityARCore_depth_Stop();

            [DllImport("UnityARCore")]
            static extern unsafe public void* UnityARCore_depth_AcquireChanges(
                out void* addedPtr, out int addedLength,
                out void* updatedPtr, out int updatedLength,
                out void* removedPtr, out int removedLength,
                out int elementSize);

            [DllImport("UnityARCore")]
            static extern unsafe public void UnityARCore_depth_ReleaseChanges(void* changes);

            [DllImport("UnityARCore")]
            public static extern unsafe int UnityARCore_depth_getPointCloudPtrs(
                TrackableId trackableId,
                out void* dataPtr, out void* identifierPtr);

            public override unsafe TrackableChanges<XRPointCloud> GetChanges(
                XRPointCloud defaultPointCloud,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedLength, updatedLength, removedLength, elementSize;

                var context = UnityARCore_depth_AcquireChanges(
                    out addedPtr, out addedLength,
                    out updatedPtr, out updatedLength,
                    out removedPtr, out removedLength,
                    out elementSize);

                try
                {
                    return new TrackableChanges<XRPointCloud>(
                        addedPtr, addedLength,
                        updatedPtr, updatedLength,
                        removedPtr, removedLength,
                        defaultPointCloud, elementSize,
                        allocator);
                }
                finally
                {
                    UnityARCore_depth_ReleaseChanges(context);
                }
            }

            public override unsafe XRPointCloudData GetPointCloudData(
                TrackableId trackableId,
                Allocator allocator)
            {
                void* dataPtr, identifierPtr;
                int numPoints = UnityARCore_depth_getPointCloudPtrs(
                    trackableId,
                    out dataPtr, out identifierPtr);

                var data = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Quaternion>(dataPtr, numPoints, Allocator.None);

                var positions = new NativeArray<Vector3>(numPoints, allocator);
                var positionsJob = new TransformPositionsJob
                {
                    positionsIn = data,
                    positionsOut = positions
                };
                var positionsHandle = positionsJob.Schedule(numPoints, 32);

                var confidenceValues = new NativeArray<float>(numPoints, allocator);
                var confidenceJob = new ExtractConfidenceValuesJob
                {
                    confidenceIn = data,
                    confidenceOut = confidenceValues
                };
                var confidenceHandle = confidenceJob.Schedule(numPoints, 32);

                var identifiers = new NativeArray<ulong>(numPoints, allocator);
                var identifiersJob = new CopyIdentifiersJob
                {
                    identifiersIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(identifierPtr, numPoints, Allocator.None),
                    identifiersOut = identifiers
                };
                var identifiersHandle = identifiersJob.Schedule(numPoints, 32);

                JobHandle.CombineDependencies(positionsHandle, confidenceHandle, identifiersHandle).Complete();
                return new XRPointCloudData
                {
                    positions = positions,
                    identifiers = identifiers,
                    confidenceValues = confidenceValues
                };
            }

            struct CopyIdentifiersJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<int> identifiersIn;

                [WriteOnly]
                public NativeArray<ulong> identifiersOut;

                public void Execute(int index)
                {
                    identifiersOut[index] = (ulong)identifiersIn[index];
                }
            }

            struct ExtractConfidenceValuesJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Quaternion> confidenceIn;

                [WriteOnly]
                public NativeArray<float> confidenceOut;

                public void Execute(int index)
                {
                    confidenceOut[index] = confidenceIn[index].w;
                }
            }

            struct TransformPositionsJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Quaternion> positionsIn;

                [WriteOnly]
                public NativeArray<Vector3> positionsOut;

                public void Execute(int index)
                {
                    positionsOut[index] = new Vector3(
                         positionsIn[index].x,
                         positionsIn[index].y,
                        -positionsIn[index].z);
                }
            }

            public override void Destroy() { }

            /// <summary>
            /// Starts the DepthSubsystem provider to begin providing face data via the callback delegates
            /// </summary>
            public override void Start() => UnityARCore_depth_Start(Guid.NewGuid());

            /// <summary>
            /// Stops the DepthSubsystem provider from providing face data
            /// </summary>
            public override void Stop() => UnityARCore_depth_Stop();
        }

        protected override Provider CreateProvider() => new ARCoreProvider();

        // this method is run on startup of the app to register this provider with XR Subsystem Manager
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var descriptorParams = new XRDepthSubsystemDescriptor.Cinfo
            {
                id = "ARCore-Depth",
                implementationType = typeof(ARCoreXRDepthSubsystem),
                supportsFeaturePoints = true,
                supportsUniqueIds = true,
                supportsConfidence = true
            };

            XRDepthSubsystemDescriptor.RegisterDescriptor(descriptorParams);
#endif
        }
    }
}