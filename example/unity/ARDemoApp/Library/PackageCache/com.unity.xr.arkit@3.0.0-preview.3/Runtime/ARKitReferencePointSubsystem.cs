using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The ARKit implementation of the <c>XRReferencePointSubsystem</c>. Do not create this directly.
    /// Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARKitReferencePointSubsystem : XRReferencePointSubsystem
    {
        protected override Provider CreateProvider() => new ARKitProvider();

        class ARKitProvider : Provider
        {
            public override void Start() => UnityARKit_refPoints_onStart();

            public override void Stop() => UnityARKit_refPoints_onStop();

            public override void Destroy() => UnityARKit_refPoints_onDestroy();

            public override unsafe TrackableChanges<XRReferencePoint> GetChanges(
                XRReferencePoint defaultReferencePoint,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedCount, updatedCount, removedCount, elementSize;
                var context = UnityARKit_refPoints_acquireChanges(
                    out addedPtr, out addedCount,
                    out updatedPtr, out updatedCount,
                    out removedPtr, out removedCount,
                    out elementSize);

                try
                {
                    return new TrackableChanges<XRReferencePoint>(
                        addedPtr, addedCount,
                        updatedPtr, updatedCount,
                        removedPtr, removedCount,
                        defaultReferencePoint, elementSize,
                        allocator);
                }
                finally
                {
                    UnityARKit_refPoints_releaseChanges(context);
                }
            }

            public override bool TryAddReferencePoint(Pose pose, out XRReferencePoint referencePoint)
            {
                return UnityARKit_refPoints_tryAdd(pose, out referencePoint);
            }

            public override bool TryAttachReferencePoint(
                TrackableId trackableToAffix,
                Pose pose,
                out XRReferencePoint referencePoint)
            {
                return UnityARKit_refPoints_tryAttach(trackableToAffix, pose, out referencePoint);
            }

            public override bool TryRemoveReferencePoint(TrackableId referencePointId)
            {
                return UnityARKit_refPoints_tryRemove(referencePointId);
            }

            [DllImport("__Internal")]
            static extern void UnityARKit_refPoints_onStart();

            [DllImport("__Internal")]
            static extern void UnityARKit_refPoints_onStop();

            [DllImport("__Internal")]
            static extern unsafe void UnityARKit_refPoints_onDestroy();

            [DllImport("__Internal")]
            static extern unsafe void* UnityARKit_refPoints_acquireChanges(
                out void* addedPtr, out int addedCount,
                out void* updatedPtr, out int updatedCount,
                out void* removedPtr, out int removedCount,
                out int elementSize);

            [DllImport("__Internal")]
            static extern unsafe void UnityARKit_refPoints_releaseChanges(void* changes);

            [DllImport("__Internal")]
            static extern bool UnityARKit_refPoints_tryAdd(
                Pose pose,
                out XRReferencePoint referencePoint);

            [DllImport("__Internal")]
            static extern bool UnityARKit_refPoints_tryAttach(
                TrackableId trackableToAffix,
                Pose pose,
                out XRReferencePoint referencePoint);

            [DllImport("__Internal")]
            static extern bool UnityARKit_refPoints_tryRemove(TrackableId referencePointId);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_IOS && !UNITY_EDITOR
            var cinfo = new XRReferencePointSubsystemDescriptor.Cinfo
            {
                id = "ARKit-ReferencePoint",
                subsystemImplementationType = typeof(ARKitReferencePointSubsystem),
                supportsTrackableAttachments = true
            };

            XRReferencePointSubsystemDescriptor.Create(cinfo);
#endif
        }
    }
}
