using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// The ARCore implementation of the <c>XRReferencePointSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARCoreReferencePointSubsystem : XRReferencePointSubsystem
    {
        protected override Provider CreateProvider() => new ARCoreProvider();

        class ARCoreProvider : Provider
        {
            public override void Start() => UnityARCore_refPoints_start();

            public override void Stop() => UnityARCore_refPoints_stop();

            public override void Destroy() => UnityARCore_refPoints_onDestroy();

            public override unsafe TrackableChanges<XRReferencePoint> GetChanges(
                XRReferencePoint defaultReferencePoint,
                Allocator allocator)
            {
                int addedCount, updatedCount, removedCount, elementSize;
                void* addedPtr, updatedPtr, removedPtr;
                var context = UnityARCore_refPoints_acquireChanges(
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
                    UnityARCore_refPoints_releaseChanges(context);
                }

            }

            public override bool TryAddReferencePoint(
                Pose pose,
                out XRReferencePoint referencePoint)
            {
                return UnityARCore_refPoints_tryAdd(pose, out referencePoint);
            }

            public override bool TryAttachReferencePoint(
                TrackableId trackableToAffix,
                Pose pose,
                out XRReferencePoint referencePoint)
            {
                return UnityARCore_refPoints_tryAttach(trackableToAffix, pose, out referencePoint);
            }

            public override bool TryRemoveReferencePoint(TrackableId referencePointId)
            {
                return UnityARCore_refPoints_tryRemove(referencePointId);
            }

            [DllImport("UnityARCore")]
            static extern void UnityARCore_refPoints_start();

            [DllImport("UnityARCore")]
            static extern void UnityARCore_refPoints_stop();

            [DllImport("UnityARCore")]
            static extern void UnityARCore_refPoints_onDestroy();

            [DllImport("UnityARCore")]
            static extern unsafe void* UnityARCore_refPoints_acquireChanges(
                out void* addedPtr, out int addedCount,
                out void* updatedPtr, out int updatedCount,
                out void* removedPtr, out int removedCount,
                out int elementSize);

            [DllImport("UnityARCore")]
            static extern unsafe void UnityARCore_refPoints_releaseChanges(
                void* changes);

            [DllImport("UnityARCore")]
            static extern bool UnityARCore_refPoints_tryAdd(
                Pose pose,
                out XRReferencePoint referencePoint);

            [DllImport("UnityARCore")]
            static extern bool UnityARCore_refPoints_tryAttach(
                TrackableId trackableToAffix,
                Pose pose,
                out XRReferencePoint referencePoint);

            [DllImport("UnityARCore")]
            static extern bool UnityARCore_refPoints_tryRemove(TrackableId referencePointId);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var cinfo = new XRReferencePointSubsystemDescriptor.Cinfo
            {
                id = "ARCore-ReferencePoint",
                subsystemImplementationType = typeof(ARCoreReferencePointSubsystem),
                supportsTrackableAttachments = true
            };

            XRReferencePointSubsystemDescriptor.Create(cinfo);
#endif
        }
    }
}
