using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    internal static class EnvironmentProbeApi
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        internal static extern void UnityARKit_EnvironmentProbeProvider_Construct();

        [DllImport("__Internal")]
        internal static extern void UnityARKit_EnvironmentProbeProvider_Destruct();

        [DllImport("__Internal")]
        internal static extern void UnityARKit_EnvironmentProbeProvider_Start();

        [DllImport("__Internal")]
        internal static extern void UnityARKit_EnvironmentProbeProvider_Stop();

        [DllImport("__Internal")]
        internal static extern void UnityARKit_EnvironmentProbeProvider_SetAutomaticPlacementEnabled(bool enabled);

        [DllImport("__Internal")]
        internal static extern bool UnityARKit_EnvironmentProbeProvider_TrySetEnvironmentTextureHDREnabled(bool enabled);

        [DllImport("__Internal")]
        internal static extern bool UnityARKit_EnvironmentProbeProvider_TryAddEnvironmentProbe(Pose pose,
                                                                                               Vector3 scale,
                                                                                               Vector3 size,
                                                                                               out XREnvironmentProbe environmentProbe);

        [DllImport("__Internal")]
        internal static extern bool UnityARKit_EnvironmentProbeProvider_TryRemoveEnvironmentProbe(TrackableId id);

        [DllImport("__Internal")]
        internal static extern IntPtr UnityARKit_EnvironmentProbeProvider_AcquireChanges(out int numAddedEnvironmentProbes, out IntPtr addedEnvironmentProbes,
                                                                                         out int numUpdatedEnvironmentProbes, out IntPtr updatedEnvironmentProbes,
                                                                                         out int numRemovedEnvironmentProbeIds, out IntPtr removedEnvironmentProbeIds,
                                                                                         out int stride);

        [DllImport("__Internal")]
        internal static extern void UnityARKit_EnvironmentProbeProvider_ReleaseChanges(IntPtr context);

        [DllImport("__Internal")]
        internal static extern bool UnityARKit_EnvironmentProbeProvider_IsSupported();
#else
        internal static void UnityARKit_EnvironmentProbeProvider_Construct() {}

        internal static void UnityARKit_EnvironmentProbeProvider_Destruct() {}

        internal static void UnityARKit_EnvironmentProbeProvider_Start() {}

        internal static void UnityARKit_EnvironmentProbeProvider_Stop() {}

        internal static void UnityARKit_EnvironmentProbeProvider_SetAutomaticPlacementEnabled(bool enabled) {}

        internal static bool UnityARKit_EnvironmentProbeProvider_TrySetEnvironmentTextureHDREnabled(bool enabled) { return false; }

        internal static bool UnityARKit_EnvironmentProbeProvider_TryAddEnvironmentProbe(Pose pose,
                                                                                        Vector3 scale,
                                                                                        Vector3 size,
                                                                                        out XREnvironmentProbe environmentProbe)
        {
            environmentProbe = XREnvironmentProbe.defaultValue;
            return false;
        }

        internal static bool UnityARKit_EnvironmentProbeProvider_TryRemoveEnvironmentProbe(TrackableId id) { return false; }

        internal static IntPtr UnityARKit_EnvironmentProbeProvider_AcquireChanges(out int numAddedEnvironmentProbes, out IntPtr addedEnvironmentProbes,
                                                                                  out int numUpdatedEnvironmentProbes, out IntPtr updatedEnvironmentProbes,
                                                                                  out int numRemovedEnvironmentProbeIds, out IntPtr removedEnvironmentProbeIds,
                                                                                  out int stride)
        {
            numAddedEnvironmentProbes = 0;
            addedEnvironmentProbes = IntPtr.Zero;

            numUpdatedEnvironmentProbes = 0;
            updatedEnvironmentProbes = IntPtr.Zero;

            numRemovedEnvironmentProbeIds = 0;
            removedEnvironmentProbeIds = IntPtr.Zero;

            stride = 0;
            return IntPtr.Zero;
        }

        internal static void UnityARKit_EnvironmentProbeProvider_ReleaseChanges(IntPtr context) {}

        internal static bool UnityARKit_EnvironmentProbeProvider_IsSupported() { return false; }
#endif
    }
}
