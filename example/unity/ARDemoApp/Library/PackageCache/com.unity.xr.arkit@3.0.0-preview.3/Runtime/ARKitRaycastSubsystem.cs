using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// ARKit implementation of the <c>XRRaycastSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARKitRaycastSubsystem : XRRaycastSubsystem
    {
        protected override Provider CreateProvider() => new ARKitProvider();

        class ARKitProvider : XRRaycastSubsystem.Provider
        {
            public override unsafe NativeArray<XRRaycastHit> Raycast(
                XRRaycastHit defaultRaycastHit,
                Vector2 screenPoint,
                TrackableType trackableTypeMask,
                Allocator allocator)
            {
                void* hitResults;
                int count;
                NativeApi.UnityARKit_raycast_acquireHitResults(
                    screenPoint,
                    trackableTypeMask,
                    out hitResults,
                    out count);

                var results = new NativeArray<XRRaycastHit>(count, allocator);
                NativeApi.UnityARKit_raycast_copyAndReleaseHitResults(
                    UnsafeUtility.AddressOf(ref defaultRaycastHit),
                    UnsafeUtility.SizeOf<XRRaycastHit>(),
                    hitResults,
                    results.GetUnsafePtr());

                return results;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_IOS && !UNITY_EDITOR
            XRRaycastSubsystemDescriptor.RegisterDescriptor(new XRRaycastSubsystemDescriptor.Cinfo
            {
                id = "ARKit-Raycast",
                subsystemImplementationType = typeof(ARKitRaycastSubsystem),
                supportsViewportBasedRaycast = true,
                supportsWorldBasedRaycast = false,
                supportedTrackableTypes =
                    TrackableType.Planes |
                    TrackableType.FeaturePoint
            });
#endif
        }

        static class NativeApi
        {
            [DllImport("__Internal")]
            public static unsafe extern void UnityARKit_raycast_acquireHitResults(
                Vector2 screenPoint,
                TrackableType filter,
                out void* hitResults,
                out int hitCount);

            [DllImport("__Internal")]
            public static unsafe extern void UnityARKit_raycast_copyAndReleaseHitResults(
                void* defaultHit,
                int stride,
                void* hitResults,
                void* dstBuffer);
        }
    }
}
