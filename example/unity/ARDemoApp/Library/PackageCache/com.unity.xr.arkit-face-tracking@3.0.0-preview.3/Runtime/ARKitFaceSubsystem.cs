using System;
using System.Runtime.InteropServices;

using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    [Preserve]
    public class ARKitFaceSubsystem : XRFaceSubsystem
    {
        /// <summary>
        /// calls available to native code, linked via extern C symbols
        /// </summary>
        [DllImport("__Internal")]
        static extern void UnityARKit_FaceProvider_Initialize();

        [DllImport("__Internal")]
        static extern void UnityARKit_FaceProvider_Shutdown();

        [DllImport("__Internal")]
        static extern void UnityARKit_FaceProvider_Start();

        [DllImport("__Internal")]
        static extern void UnityARKit_FaceProvider_Stop();

        [DllImport("__Internal")]
        static extern bool UnityARKit_FaceProvider_TryAcquireFaceBlendCoefficients(TrackableId faceId, out IntPtr ptrBlendCoefficientData, out int numArrayBlendCoefficients);

        [DllImport("__Internal")]
        static extern bool UnityARKit_FaceProvider_IsSupported();

        [DllImport("__Internal")]
        static extern bool UnityARKit_FaceProvider_IsEyeTrackingSupported();

        [DllImport("__Internal")]
        static extern void UnityARKit_FaceProvider_DeallocateTempMemory(IntPtr ptr);

        [DllImport("__Internal")]
        static extern unsafe void* UnityARKit_FaceProvider_AcquireChanges(
            out void* addedPtr, out int addedLength,
            out void* updatedPtr, out int updatedLength,
            out void* removedPtr, out int removedLength,
            out int elementSize);

        [DllImport("__Internal")]
        static extern unsafe void UnityARKit_FaceProvider_ReleaseChanges(void* context);

        [DllImport("__Internal")]
        static extern unsafe void* UnityARKit_FaceProvider_AcquireFaceAnchor(
            TrackableId faceId,
            out void* vertexPtr, out void* uvPtr, out int vertexCount,
            out void* indexPtr, out int triangleCount);

        [DllImport("__Internal")]
        static extern unsafe void UnityARKit_FaceProvider_ReleaseFaceAnchor(
            void* faceAnchor);

        [DllImport("__Internal")]
        static extern int UnityARKit_FaceProvider_GetSupportedFaceCount();

        [DllImport("__Internal")]
        static extern int UnityARKit_FaceProvider_GetMaximumFaceCount();

        [DllImport("__Internal")]
        static extern void UnityARKit_FaceProvider_SetMaximumFaceCount(int count);

        /// <summary>
        /// Get the blend shape coefficients for the face. Blend shapes describe a number of facial
        /// features on a scale of 0..1. For example, how closed is the left eye, how open is the mouth.
        /// See <see cref="ARKitBlendShapeCoefficient"/> for more details.
        /// </summary>
        /// <param name="faceId">The <c>TrackableId</c> associated with the face to query.</param>
        /// <param name="allocator">The allocator to use for the returned <c>NativeArray</c>.</param>
        /// <returns>A new <c>NativeArray</c> allocated with <paramref name="allocator"/> describing
        /// the blend shapes for the face with id <paramref name="faceId"/>. The caller owns the
        /// <c>NativeArray</c> and is responsible for calling <c>Dispose</c> on it.</returns>
        public unsafe NativeArray<ARKitBlendShapeCoefficient> GetBlendShapeCoefficients(
            TrackableId faceId,
            Allocator allocator)
        {
            IntPtr ptrNativeCoefficientsArray;
            int coefficientsCount;
            if (!UnityARKit_FaceProvider_TryAcquireFaceBlendCoefficients(faceId, out ptrNativeCoefficientsArray, out coefficientsCount) || coefficientsCount <= 0)
            {
                return new NativeArray<ARKitBlendShapeCoefficient>(0, allocator);
            }

            try
            {
                // Points directly to the native memory
                var nativeCoefficientsArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ARKitBlendShapeCoefficient>(
                    (void*)ptrNativeCoefficientsArray, coefficientsCount, Allocator.None);

                var coefficients = new NativeArray<ARKitBlendShapeCoefficient>(coefficientsCount, allocator);
                coefficients.CopyFrom(nativeCoefficientsArray);

                return coefficients;
            }
            finally
            {
                UnityARKit_FaceProvider_DeallocateTempMemory(ptrNativeCoefficientsArray);
            }
        }

        protected override Provider CreateProvider()
        {
            return new ARKitProvider();
        }

        class ARKitProvider : Provider
        {
            public ARKitProvider() => UnityARKit_FaceProvider_Initialize();

            public override void Start() => UnityARKit_FaceProvider_Start();

            public override void Stop() => UnityARKit_FaceProvider_Stop();

            public override void Destroy() => UnityARKit_FaceProvider_Shutdown();

            public unsafe override void GetFaceMesh(
                TrackableId faceId,
                Allocator allocator,
                ref XRFaceMesh faceMesh)
            {
                int vertexCount, triangleCount;
                void* vertexPtr, indexPtr, uvPtr;
                var faceAnchor = UnityARKit_FaceProvider_AcquireFaceAnchor(
                    faceId,
                    out vertexPtr, out uvPtr, out vertexCount,
                    out indexPtr, out triangleCount);

                if (faceAnchor == null)
                {
                    faceMesh.Dispose();
                    faceMesh = default(XRFaceMesh);
                    return;
                }

                try
                {
                    faceMesh.Resize(
                        vertexCount,
                        triangleCount,
                        XRFaceMesh.Attributes.UVs,
                        allocator);

                    var vertexJob = new TransformVerticesJob
                    {
                        // Use a Vector4 b/c the data is a simd primitive,
                        // so we need something that consists of 4 floats
                        verticesIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector4>(vertexPtr, vertexCount, Allocator.None),
                        verticesOut = faceMesh.vertices
                    };
                    var vertexJobHandle = vertexJob.Schedule(vertexCount, 32);

                    var uvJob = new TransformUVsJob
                    {
                        uvsIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector2>(uvPtr, vertexCount, Allocator.None),
                        uvsOut = faceMesh.uvs
                    };
                    var uvJobHandle = uvJob.Schedule(vertexCount, 32);

                    var indexJob = new TransformIndicesJob
                    {
                        triangleIndicesIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Triangle<short>>(indexPtr, triangleCount, Allocator.None),
                        // "cast" it to an array of Triangles
                        triangleIndicesOut = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Triangle<int>>(faceMesh.indices.GetUnsafePtr(), triangleCount, Allocator.None)
                    };
                    var indexJobHandle = indexJob.Schedule(triangleCount, 32);

                    // Wait on all three
                    JobHandle.CombineDependencies(vertexJobHandle, indexJobHandle, uvJobHandle).Complete();
                }
                finally
                {
                    UnityARKit_FaceProvider_ReleaseFaceAnchor(faceAnchor);
                }
            }

            struct TransformVerticesJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Vector4> verticesIn;

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
                        -uvsIn[i].y);
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
                    this.b = b;
                    this.c = c;
                }
            }

            struct TransformIndicesJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Triangle<short>> triangleIndicesIn;

                [WriteOnly]
                public NativeArray<Triangle<int>> triangleIndicesOut;

                public void Execute(int i)
                {
                    triangleIndicesOut[i] = new Triangle<int>(
                        // Flip triangle winding
                        triangleIndicesIn[i].a,
                        triangleIndicesIn[i].c,
                        triangleIndicesIn[i].b);
                }
            }

            /// <summary>
            /// Get the changes (added, updated, and removed) faces since the last call to <see cref="GetChanges(Allocator)"/>.
            /// </summary>
            /// <param name="defaultFace">
            /// The default face. This should be used to initialize the returned <c>NativeArray</c>s for backwards compatibility.
            /// See <see cref="TrackableChanges{T}.TrackableChanges(void*, int, void*, int, void*, int, T, int, Allocator)"/>.
            /// </param>
            /// <param name="allocator">An <c>Allocator</c> to use when allocating the returned <c>NativeArray</c>s.</param>
            /// <returns>
            /// <see cref="TrackableChanges{T}"/> describing the faces that have been added, updated, and removed
            /// since the last call to <see cref="GetChanges(Allocator)"/>. The changes should be allocated using
            /// <paramref name="allocator"/>.
            /// </returns>
            public unsafe override TrackableChanges<XRFace> GetChanges(
                XRFace defaultFace,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedLength, updatedLength, removedLength, elementSize;
                var context = UnityARKit_FaceProvider_AcquireChanges(
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
                    UnityARKit_FaceProvider_ReleaseChanges(context);
                }
            }

            public override int supportedFaceCount => UnityARKit_FaceProvider_GetSupportedFaceCount();

            public override int maximumFaceCount
            {
                get { return UnityARKit_FaceProvider_GetMaximumFaceCount(); }
                set { UnityARKit_FaceProvider_SetMaximumFaceCount(value); }
            }
        }

        // this method is run on startup of the app to register this provider with XR Subsystem Manager
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_IOS && !UNITY_EDITOR
            if (!UnityARKit_FaceProvider_IsSupported())
                return;

            var descriptorParams = new FaceSubsystemParams
            {
                supportsFacePose = true,
                supportsFaceMeshVerticesAndIndices = true,
                supportsFaceMeshUVs = true,
                supportsEyeTracking = UnityARKit_FaceProvider_IsEyeTrackingSupported(),
                id = "ARKit-Face",
                subsystemImplementationType = typeof(ARKitFaceSubsystem)
            };

            XRFaceSubsystemDescriptor.Create(descriptorParams);
#endif
        }
    }
}

