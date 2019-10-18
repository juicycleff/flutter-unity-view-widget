using System;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// An internal class with only static methods to register the environment probe subsystem before the scene is
    /// loaded.
    /// </summary>
    internal static class ARKitEnvironmentProbeRegistration
    {
        /// <summary>
        /// Create and register the environment probe subsystem descriptor to advertise a providing implementation for
        /// environment probe functionality.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
#if UNITY_IOS && !UNITY_EDITOR
            var iOSVersion = OSVersion.Parse(UnityEngine.iOS.Device.systemVersion);
            if (iOSVersion < new OSVersion(12))
                return;

            const string subsystemId = "ARKit-EnvironmentProbe";
            XREnvironmentProbeSubsystemCinfo environmentProbeSubsystemInfo = new XREnvironmentProbeSubsystemCinfo()
            {
                id = subsystemId,
                implementationType = typeof(ARKitEnvironmentProbeSubsystem),
                supportsManualPlacement = true,
                supportsRemovalOfManual = true,
                supportsAutomaticPlacement = true,
                supportsRemovalOfAutomatic = true,
                supportsEnvironmentTexture = true,
                supportsEnvironmentTextureHDR = iOSVersion >= new OSVersion(13),
            };

            if (!XREnvironmentProbeSubsystem.Register(environmentProbeSubsystemInfo))
            {
                Debug.LogErrorFormat("Cannot register the {0} subsystem", subsystemId);
            }
#endif
        }
    }

    /// <summary>
    /// This subsystem provides implementing functionality for the <c>XREnvironmentProbeSubsystem</c> class.
    /// </summary>
    [Preserve]
    class ARKitEnvironmentProbeSubsystem : XREnvironmentProbeSubsystem
    {
        protected override Provider CreateProvider() => new ARKitProvider();

        class ARKitProvider : Provider
        {
            public ARKitProvider() => EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_Construct();

            public override void Start() => EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_Start();

            /// <summary>
            /// Stops the environment probe subsystem by disabling the environment probe state.
            /// </summary>
            public override void Stop() => EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_Stop();

            /// <summary>
            /// Destroy the environment probe subsystem by first ensuring that the subsystem has been stopped and then
            /// destroying the provider.
            /// </summary>
            public override void Destroy() => EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_Destruct();

            /// <summary>
            /// Enable or disable automatic placement of environment probes by the provider.
            /// </summary>
            /// <param name='value'><c>true</c> if the provider should automatically place environment probes in the scene.
            /// Otherwise, <c>false</c></param>.
            public override void SetAutomaticPlacement(bool value)
            {
                EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_SetAutomaticPlacementEnabled(value);
            }

            /// <summary>
            /// Set the state of HDR environment texture generation.
            /// </summary>
            /// <param name="value">Whether HDR environment texture generation is enabled (<c>true</c>) or disabled
            /// (<c>false</c>).</param>
            /// <returns>
            /// Whether the HDR environment texture generation state was set.
            /// </returns>
            public override bool TrySetEnvironmentTextureHDREnabled(bool value)
            {
                return EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_TrySetEnvironmentTextureHDREnabled(value);
            }

            public override bool TryAddEnvironmentProbe(Pose pose, Vector3 scale, Vector3 size, out XREnvironmentProbe environmentProbe)
            {
                return EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_TryAddEnvironmentProbe(pose, scale, size, out environmentProbe);
            }

            /// <summary>
            /// Remove the environment probe matching the trackable ID from the AR session..
            /// </summary>
            /// <param name='trackableId'>The trackable ID for the environment probe to be removed.</param>
            /// <returns>
            /// <c>true</c> if an environment probe matching the trackable ID is found and will be removed from the AR
            /// session. Otherwise, <c>false</c>.
            /// </returns>
            public override bool RemoveEnvironmentProbe(TrackableId trackableId)
            {
                return EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_TryRemoveEnvironmentProbe(trackableId);
            }

            public override TrackableChanges<XREnvironmentProbe> GetChanges(XREnvironmentProbe defaultEnvironmentProbe,
                                                                            Allocator allocator)
            {
                int numAddedEnvironmentProbes;
                IntPtr addedEnvironmentProbesPointer;

                int numUpdatedEnvironmentProbes;
                IntPtr updatedEnvironmentProbesPointer;

                int numRemovedEnvironmentProbeIds;
                IntPtr removedEnvironmentProbeIdsPointer;

                int stride;

                var context = EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_AcquireChanges(out numAddedEnvironmentProbes, out addedEnvironmentProbesPointer,
                                                                                                     out numUpdatedEnvironmentProbes, out updatedEnvironmentProbesPointer,
                                                                                                     out numRemovedEnvironmentProbeIds, out removedEnvironmentProbeIdsPointer,
                                                                                                     out stride);

                try
                {
                    unsafe
                    {
                        // Wrap the navite pointers into a native array and then copy them into a separate native array enabled
                        // with temporary allocations.
                        return new TrackableChanges<XREnvironmentProbe>(
                            (void*)addedEnvironmentProbesPointer, numAddedEnvironmentProbes,
                            (void*)updatedEnvironmentProbesPointer, numUpdatedEnvironmentProbes,
                            (void*)removedEnvironmentProbeIdsPointer, numRemovedEnvironmentProbeIds,
                            defaultEnvironmentProbe, stride,
                            allocator);
                    }
                }
                finally
                {
                    EnvironmentProbeApi.UnityARKit_EnvironmentProbeProvider_ReleaseChanges(context);
                }
            }
        }
    }
}
