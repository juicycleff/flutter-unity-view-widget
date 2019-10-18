using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Registration utility to register the ARKit human body subsystem.
    /// </summary>
    internal static class ARKitHumanBodyRegistration
    {
        /// <summary>
        /// Register the ARKit human body subsystem if iOS and not the editor.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_IOS && !UNITY_EDITOR
            const string k_SubsystemId = "ARKit-HumanBody";

            XRHumanBodySubsystemCinfo humanBodySubsystemCinfo = new XRHumanBodySubsystemCinfo()
            {
                id = k_SubsystemId,
                implementationType = typeof(ARKitHumanBodySubsystem),
                supportsHumanBody2D = NativeApi.UnityARKit_HumanBodyProvider_DoesSupportBodyPose2DEstimation(),
                supportsHumanBody3D = NativeApi.UnityARKit_HumanBodyProvider_DoesSupportBodyPose3DEstimation(),
                supportsHumanBody3DScaleEstimation = NativeApi.UnityARKit_HumanBodyProvider_DoesSupportBodyPose3DScaleEstimation(),
                supportsHumanStencilImage = NativeApi.UnityARKit_HumanBodyProvider_DoesSupportBodySegmentationStencil(),
                supportsHumanDepthImage = NativeApi.UnityARKit_HumanBodyProvider_DoesSupportBodySegmentationDepth(),
            };

            if (!XRHumanBodySubsystem.Register(humanBodySubsystemCinfo))
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", k_SubsystemId);
            }
#endif
        }

        /// <summary>
        /// Container to wrap the native ARKit human body APIs.
        /// </summary>
        static class NativeApi
        {
            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_DoesSupportBodyPose2DEstimation();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_DoesSupportBodyPose3DEstimation();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_DoesSupportBodyPose3DScaleEstimation();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_DoesSupportBodySegmentationStencil();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_DoesSupportBodySegmentationDepth();
        }
    }

    /// <summary>
    /// This subsystem provides implementing functionality for the <c>XRHumanBodySubsystem</c> class.
    /// </summary>
    [Preserve]
    class ARKitHumanBodySubsystem : XRHumanBodySubsystem
    {
        /// <summary>
        /// Create the implementation provider.
        /// </summary>
        /// <returns>
        /// The implementation provider.
        /// </returns>
        protected override Provider CreateProvider()
        {
            return new ARKitProvider();
        }

        /// <summary>
        /// The implementation provider class.
        /// </summary>
        class ARKitProvider : XRHumanBodySubsystem.Provider
        {
            /// <summary>
            /// Construct the implementation provider.
            /// </summary>
            public ARKitProvider() => NativeApi.UnityARKit_HumanBodyProvider_Construct();

            /// <summary>
            /// Start the provider.
            /// </summary>
            public override void Start() => NativeApi.UnityARKit_HumanBodyProvider_Start();

            /// <summary>
            /// Stop the provider.
            /// </summary>
            public override void Stop() => NativeApi.UnityARKit_HumanBodyProvider_Stop();

            /// <summary>
            /// Destroy the human body subsystem by first ensuring that the subsystem has been stopped and then
            /// destroying the provider.
            /// </summary>
            public override void Destroy() => NativeApi.UnityARKit_HumanBodyProvider_Destruct();

            /// <summary>
            /// Sets whether human body pose 2D estimation is enabled.
            /// </summary>
            /// <param name="enabled">Whether the human body pose 2D estimation should be enabled.
            /// </param>
            /// <returns>
            /// <c>true</c> if the human body pose 2D estimation is set to the given value. Otherwise, <c>false</c>.
            /// </returns>
            /// <remarks>
            /// Current restrictions limit either human body pose estimation to be enabled or human segmentation images
            /// to be enabled. At this time, these features are mutually exclusive.
            /// </remarks>
            public override bool TrySetHumanBodyPose2DEstimationEnabled(bool enabled)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TrySetHumanBodyPose2DEstimationEnabled(enabled);
            }

            /// <summary>
            /// Sets whether human body pose 3D estimation is enabled.
            /// </summary>
            /// <param name="enabled">Whether the human body pose 3D estimation should be enabled.
            /// </param>
            /// <returns>
            /// <c>true</c> if the human body pose 3D estimation is set to the given value. Otherwise, <c>false</c>.
            /// </returns>
            /// <remarks>
            /// Current restrictions limit either human body pose estimation to be enabled or human segmentation images
            /// to be enabled. At this time, these features are mutually exclusive.
            /// </remarks>
            public override bool TrySetHumanBodyPose3DEstimationEnabled(bool enabled)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TrySetHumanBodyPose3DEstimationEnabled(enabled);
            }

            /// <summary>
            /// Sets whether 3D human body scale estimation is enabled.
            /// </summary>
            /// <param name="enabled">Whether the 3D human body scale estimation should be enabled.
            /// </param>
            /// <returns>
            /// <c>true</c> if the 3D human body scale estimation is set to the given value. Otherwise, <c>false</c>.
            /// </returns>
            public override bool TrySetHumanBodyPose3DScaleEstimationEnabled(bool enabled)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TrySetHumanBodyPose3DScaleEstimationEnabled(enabled);
            }

            /// <summary>
            /// Set the human segmentation stencil mode.
            /// </summary>
            /// <param name="humanSegmentationStencilMode">The mode for the human segmentation stencil.</param>
            /// <returns>
            /// <c>true</c> if the method successfully set the human segmentation stencil mode. Otherwise,
            /// <c>false</c>.
            /// </returns>
            /// <remarks>
            /// Current restrictions limit either human body pose estimation to be enabled or human segmentation images
            /// to be enabled. At this time, these features are mutually exclusive.
            /// </remarks>
            public override bool TrySetHumanSegmentationStencilMode(HumanSegmentationMode humanSegmentationStencilMode)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TrySetHumanSegmentationStencilMode(humanSegmentationStencilMode);
            }

            /// <summary>
            /// Set the human segmentation depth mode.
            /// </summary>
            /// <param name="humanSegmentationDepthMode">The mode for the human segmentation depth.</param>
            /// <returns>
            /// <c>true</c> if the method successfully set the human segmentation depth mode. Otherwise,
            /// <c>false</c>.
            /// </returns>
            /// <remarks>
            /// Current restrictions limit either human body pose estimation to be enabled or human segmentation images
            /// to be enabled. At this time, these features are mutually exclusive.
            /// </remarks>
            public override bool TrySetHumanSegmentationDepthMode(HumanSegmentationMode humanSegmentationDepthMode)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TrySetHumanSegmentationDepthMode(humanSegmentationDepthMode);
            }

            /// <summary>
            /// Queries for the set of human body changes.
            /// </summary>
            /// <param name="defaultHumanBody">The default human body.</param>
            /// <param name="allocator">The memory allocator to use for the returns trackable changes.</param>
            /// <returns>
            /// The set of human body changes.
            /// </returns>
            public override unsafe TrackableChanges<XRHumanBody> GetChanges(XRHumanBody defaultHumanBody, Allocator allocator)
            {
                int numAddedHumanBodies;
                void* addedHumanBodiesPointer;

                int numUpdatedHumanBodies;
                void* updatedHumanBodiesPointer;

                int numRemovedHumanBodyIds;
                void* removedHumanBodyIdsPointer;

                int stride;

                var context = NativeApi.UnityARKit_HumanBodyProvider_AcquireChanges(out numAddedHumanBodies, out addedHumanBodiesPointer,
                                                                                    out numUpdatedHumanBodies, out updatedHumanBodiesPointer,
                                                                                    out numRemovedHumanBodyIds, out removedHumanBodyIdsPointer,
                                                                                    out stride);

                try
                {
                    // Wrap the navite pointers into a native array and then copy them into a separate native array enabled
                    // with temporary allocations.
                    return new TrackableChanges<XRHumanBody>(
                        addedHumanBodiesPointer, numAddedHumanBodies,
                        updatedHumanBodiesPointer, numUpdatedHumanBodies,
                        removedHumanBodyIdsPointer, numRemovedHumanBodyIds,
                        defaultHumanBody, stride,
                        allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_HumanBodyProvider_ReleaseChanges(context);
                }
            }

            /// <summary>
            /// Get the skeleton joints for the requested trackable identifier.
            /// </summary>
            /// <param name="trackableId">The human body trackable identifier for which to query.</param>
            /// <param name="allocator">The memory allocator to use for the returned arrays.</param>
            /// <param name="skeleton">The array of skeleton joints to update and returns.</param>
            public override unsafe void GetSkeleton(TrackableId trackableId, Allocator allocator, ref NativeArray<XRHumanBodyJoint> skeleton)
            {
                int numJoints;
                void* joints = NativeApi.UnityARKit_HumanBodyProvider_AcquireJoints(trackableId, out numJoints);

                try
                {
                    if (joints == null)
                    {
                        numJoints = 0;
                    }

                    if (!skeleton.IsCreated || (skeleton.Length != numJoints))
                    {
                        if (skeleton.IsCreated)
                        {
                            skeleton.Dispose();
                        }
                        skeleton = new NativeArray<XRHumanBodyJoint>(numJoints, allocator);
                    }

                    if (joints != null)
                    {
                        NativeArray<XRHumanBodyJoint> tmp = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<XRHumanBodyJoint>(joints, numJoints, Allocator.None);
                        skeleton.CopyFrom(tmp);
                    }
                }
                finally
                {
                    NativeApi.UnityARKit_HumanBodyProvider_ReleaseJoints(joints);
                }
            }

            /// <summary>
            /// Get the human body pose 2D joints for the current frame.
            /// </summary>
            /// <param name="defaultBody2DJoint">The default value for the human body pose 2D joint.</param>
            /// <param name="screenOrientation">The orientation of the device so that the joint positions may be
            /// adjusted as required.</param>
            /// <param name="allocator">The allocator to use for the returned array memory.</param>
            /// <returns>
            /// The array of human body pose 2D joints.
            /// </returns>
            /// <remarks>
            /// The returned array may be empty if the system is not enabled or if the system does not detect a human
            /// body.
            /// </remarks>
            public override unsafe NativeArray<XRHumanBodyPose2DJoint> GetHumanBodyPose2DJoints(XRHumanBodyPose2DJoint defaultHumanBodyPose2DJoint,
                                                                                                ScreenOrientation screenOrientation,
                                                                                                Allocator allocator)
            {
                int length, elementSize;
                var joints = NativeApi.UnityARKit_HumanBodyProvider_AcquireHumanBodyPose2DJoints(screenOrientation, out length, out elementSize);

                try
                {
                    var returnJoints = NativeCopyUtility.PtrToNativeArrayWithDefault(defaultHumanBodyPose2DJoint,
                                                                                     joints, elementSize, length,
                                                                                     allocator);

                    return returnJoints;
                }
                finally
                {
                    NativeApi.UnityARKit_HumanBodyProvider_ReleaseHumanBodyPose2DJoints(joints);
                }
            }

            /// <summary>
            /// Gets the human stencil texture descriptor.
            /// </summary>
            /// <param name="humanStencilDescriptor">The human stencil texture descriptor to be populated, if
            /// available.</param>
            /// <returns>
            /// <c>true</c> if the human stencil texture descriptor is available and is returned. Otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool TryGetHumanStencil(out XRTextureDescriptor humanStencilDescriptor)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TryGetHumanStencil(out humanStencilDescriptor);
            }

            /// <summary>
            /// Get the human depth texture descriptor.
            /// </summary>
            /// <param name="humanDepthDescriptor">The human depth texture descriptor to be populated, if available
            /// </param>
            /// <returns>
            /// <c>true</c> if the human depth texture descriptor is available and is returned. Otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool TryGetHumanDepth(out XRTextureDescriptor humanDepthDescriptor)
            {
                return NativeApi.UnityARKit_HumanBodyProvider_TryGetHumanDepth(out humanDepthDescriptor);
            }
        }

        /// <summary>
        /// Container to wrap the native ARKit human body APIs.
        /// </summary>
        static class NativeApi
        {
            [DllImport("__Internal")]
            public static extern void UnityARKit_HumanBodyProvider_Construct();

            [DllImport("__Internal")]
            public static extern void UnityARKit_HumanBodyProvider_Start();

            [DllImport("__Internal")]
            public static extern void UnityARKit_HumanBodyProvider_Stop();

            [DllImport("__Internal")]
            public static extern void UnityARKit_HumanBodyProvider_Destruct();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_TrySetHumanBodyPose2DEstimationEnabled(bool enabled);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_TrySetHumanBodyPose3DEstimationEnabled(bool enabled);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_TrySetHumanBodyPose3DScaleEstimationEnabled(bool enabled);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_TrySetHumanSegmentationStencilMode(HumanSegmentationMode humanSegmentationStencilMode);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_HumanBodyProvider_TrySetHumanSegmentationDepthMode(HumanSegmentationMode humanSegmentationDepthMode);

            [DllImport("__Internal")]
            public static extern unsafe void* UnityARKit_HumanBodyProvider_AcquireChanges(out int numAddedHumanBodies, out void* addedBodys,
                                                                                     out int numUpdatedHumanBodies, out void* updatedBodys,
                                                                                     out int numRemovedHumanBodyIds, out void* removedBodyIds,
                                                                                     out int stride);

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_HumanBodyProvider_ReleaseChanges(void* context);

            [DllImport("__Internal")]
            public static extern unsafe void* UnityARKit_HumanBodyProvider_AcquireJoints(TrackableId trackableId, out int numJoints);

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_HumanBodyProvider_ReleaseJoints(void* joints);

            [DllImport("__Internal")]
            public static unsafe extern void* UnityARKit_HumanBodyProvider_AcquireHumanBodyPose2DJoints(ScreenOrientation screenOrientation,
                                                                                                        out int length,
                                                                                                        out int elementSize);

            [DllImport("__Internal")]
            public static unsafe extern void UnityARKit_HumanBodyProvider_ReleaseHumanBodyPose2DJoints(void* joints);

            [DllImport("__Internal")]
            public static unsafe extern bool UnityARKit_HumanBodyProvider_TryGetHumanStencil(out XRTextureDescriptor humanStencilDescriptor);

            [DllImport("__Internal")]
            public static unsafe extern bool UnityARKit_HumanBodyProvider_TryGetHumanDepth(out XRTextureDescriptor humanDepthDescriptor);
        }
    }
}
