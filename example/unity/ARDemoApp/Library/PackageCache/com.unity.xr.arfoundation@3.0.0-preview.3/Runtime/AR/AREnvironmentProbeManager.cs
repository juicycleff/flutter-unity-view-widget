using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

using Object = UnityEngine.Object;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// This class creates, maintains, and destroys environment probe game object components as the
    /// <c>XREnvironmentProbeSubsystem</c> provides updates from environment probes as they are detected in the
    /// environment.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_EnvironmentProbeManager)]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.AREnvironmentProbeManager.html")]
    public sealed class AREnvironmentProbeManager : ARTrackableManager<
        XREnvironmentProbeSubsystem,
        XREnvironmentProbeSubsystemDescriptor,
        XREnvironmentProbe,
        AREnvironmentProbe>
    {
        /// <summary>
        /// A property of the environment probe subsystem that, if enabled, automatically generates environment probes
        /// for the scene.
        /// </summary>
        /// <value>
        /// <c>true</c> if automatic environment probe placement is enabled. Otherwise, <c>false</c>.
        /// </value>
        public bool automaticPlacement
        {
            get { return m_AutomaticPlacement; }
            set
            {
                m_AutomaticPlacement = value;
                SetAutomaticPlacementStateOnSubsystem();
             }
        }
        [SerializeField]
        [Tooltip("Whether environment probes should be automatically placed in the environment (if supported).")]
        bool m_AutomaticPlacement = true;

        /// <summary>
        /// Specifies the texture filter mode to be used with the environment texture.
        /// </summary>
        /// <value>
        /// The texture filter mode to be used with the environment texture.
        /// </value>
        public FilterMode environmentTextureFilterMode
        {
            get { return m_EnvironmentTextureFilterMode; }
            set { m_EnvironmentTextureFilterMode = value; }
        }
        [SerializeField]
        [Tooltip("The texture filter mode to be used with the reflection probe environment texture.")]
        FilterMode m_EnvironmentTextureFilterMode = FilterMode.Trilinear;

        /// <summary>
        /// Specifies whether the environment textures should be returned as HDR textures.
        /// </summary>
        /// <value>
        /// <c>true</c> if the environment textures should be returned as HDR textures. Otherwise, <c>false</c>.
        /// </value>
        public bool environmentTextureHDR
        {
            get { return m_EnvironmentTextureHDR; }
            set
            {
                m_EnvironmentTextureHDR = value;
                SetEnvironmentTextureHDRStateOnSubsystem();
            }
        }
        [SerializeField]
        [Tooltip("Whether the environment textures should be returned as HDR textures.")]
        bool m_EnvironmentTextureHDR = true;

        /// <summary>
        /// Specifies a debug prefab that will be attached to all environment probes.
        /// </summary>
        /// <value>
        /// A debug prefab that will be attached to all environment probes.
        /// </value>
        /// <remarks>
        /// Setting a debug prefab allows for these environment probes to be more readily visualized but is not
        /// required for normal operation of this manager. This script will automatically create reflection probes for
        /// all environment probes reported by the <c>XREnvironmentProbeSubsystem</c>.
        /// </remarks>
        public GameObject debugPrefab
        {
            get { return m_DebugPrefab; }
            set { m_DebugPrefab = value; }
        }
        [SerializeField]
        [Tooltip("A debug prefab that allows for these environment probes to be more readily visualized.")]
        GameObject m_DebugPrefab;

        /// <summary>
        /// Invoked once per frame with lists of environment probes that have been added, updated, and removed since the last frame.
        /// </summary>
        public event Action<AREnvironmentProbesChangedEvent> environmentProbesChanged;

        /// <summary>
        /// Attempts to find the environment probe matching the trackable ID currently in the scene.
        /// </summary>
        /// <param name='trackableId'>The trackable ID of an environment probe for which to search.</param>
        /// <returns>
        /// Environment probe in the scene matching the <paramref name="trackableId"/> or <c>null</c> if no matching
        /// environment probe is found.
        /// </returns>
        public AREnvironmentProbe GetEnvironmentProbe(TrackableId trackableId)
        {
            AREnvironmentProbe environmentProbe;
            if (m_Trackables.TryGetValue(trackableId, out environmentProbe))
                return environmentProbe;

            return null;
        }

        /// <summary>
        /// Creates a new environment probe at <paramref name="pose"/> with <paramref name="scale"/> and <paramref name="size"/>
        /// if supported by the subsystem. Use <see cref="subsystem"/><c>.SubsystemDescriptor.supportsManualPlacement</c> to determine
        /// support for this feature. If successful, a new <c>GameObject</c> with an <see cref="AREnvironmentProbe"/> will be created
        /// immediately; however, the provider may not report the environment probe as added until a future frame. Check the
        /// status of the probe by inspecting its
        /// <see cref="ARTrackableManager{TSubsystem, TSubsystemDescriptor, TSessionRelativeData, TTrackable}.pending"/>
        /// property.
        /// </summary>
        /// <param name="pose">The position and rotation at which to create the new environment probe.</param>
        /// <param name="scale">The scale of the new environment probe.</param>
        /// <param name="size">The size (dimensions) of the new environment probe.</param>
        /// <returns>A new <see cref="AREnvironmentProbe"/> if successful, otherwise <c>null</c>.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if this manager is not enabled</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if this manager has no subsystem.</exception>
        /// <exception cref="System.NotSupportedException">Thrown if manual placement is not supported by this subsystem.
        /// Check for support with <see cref="subsystem"/><c>.SubsystemDescriptor.supportsManualPlacement</c></exception>
        public AREnvironmentProbe AddEnvironmentProbe(Pose pose, Vector3 scale, Vector3 size)
        {
            if (!enabled)
                throw new InvalidOperationException("Cannot create an environment probe from a disabled environment probe manager.");

            if (subsystem == null)
                throw new InvalidOperationException("Environment probe manager has no subsystem. Enable the manager first.");

            if (!subsystem.SubsystemDescriptor.supportsManualPlacement)
                throw new NotSupportedException("Manual environment probe placement is not supported by this subsystem.");

            var sessionRelativePose = sessionOrigin.trackablesParent.InverseTransformPose(pose);
            XREnvironmentProbe sessionRelativeData;
            if (subsystem.TryAddEnvironmentProbe(pose, scale, size, out sessionRelativeData))
            {
                var probe = CreateTrackableImmediate(sessionRelativeData);
                probe.placementType = AREnvironmentProbePlacementType.Manual;
                return probe;
            }

            return null;
        }

        /// <summary>
        /// Remove an existing environment probe. Support for this feature is provider-specific. Check for support with
        /// <see cref="subsystem"/><c>.SubsystemDescriptor.supportsRemovalOfManual</c> and
        /// <see cref="subsystem"/><c>.SubsystemDescriptor.supportsRemovalOfAutomatic</c>.
        /// </summary>
        /// <param name="probe">The environment probe to remove</param>
        /// <returns><c>true</c> if the environment probe was removed, otherwise <c>false</c>.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if this manager is not enabled.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="subsystem"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="probe"/> is <c>null</c>.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the environment probe was manually placed, but removal of manually placed probes is not supported.
        /// You can check for this case with <see cref="AREnvironmentProbe.placementType"/> and
        /// <see cref="subsystem"/><c>.SubsystemDescriptor.supportsRemovalOfManual</c>
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the environment probe was automatically placed, but removal of automatically placed probes is not supported.
        /// You can check for this case with <see cref="AREnvironmentProbe.placementType"/> and
        /// <see cref="subsystem"/><c>.SubsystemDescriptor.supportsRemovalOfAutomatic</c>
        /// </exception>
        public bool RemoveEnvironmentProbe(AREnvironmentProbe probe)
        {
            if (!enabled)
                throw new InvalidOperationException("Cannot remove an environment probe from a disabled reference point manager.");

            if (subsystem == null)
                throw new InvalidOperationException("Environment probe manager has no subsystem. Enable the manager first.");

            if (probe == null)
                throw new ArgumentNullException("probe");

            if ((probe.placementType == AREnvironmentProbePlacementType.Manual) && !subsystem.SubsystemDescriptor.supportsRemovalOfManual)
                throw new InvalidOperationException("Removal of manually placed environment probes are not supported by this subsystem.");

            if ((probe.placementType == AREnvironmentProbePlacementType.Automatic) && !subsystem.SubsystemDescriptor.supportsRemovalOfAutomatic)
                throw new InvalidOperationException("Removal of automatically placed environment probes are not supported by this subsystem.");

            if (subsystem.RemoveEnvironmentProbe(probe.trackableId))
            {
                DestroyPendingTrackable(probe.trackableId);
                return true;
            }

            return false;
        }

        protected override string gameObjectName
        {
            get { return "AREnvironmentProbe"; }
        }

        protected override GameObject GetPrefab()
        {
            return m_DebugPrefab;
        }

        /// <summary>
        /// Enables the environment probe functionality by registering listeners for the environment probe events, if
        /// the <c>XREnvironmentProbeSubsystem</c> exists, and enabling environment probes in the AR subsystem manager.
        /// </summary>
        protected override void OnBeforeStart()
        {
            SetAutomaticPlacementStateOnSubsystem();
            SetEnvironmentTextureHDRStateOnSubsystem();
        }

        /// <summary>
        /// Destroys any game objects created by this environment probe manager for each environment probe, and clears
        /// the mapping of environment probes.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var kvp in m_Trackables)
            {
                var environmentProbe = kvp.Value;
                Object.Destroy(environmentProbe.gameObject);
            }
        }

        protected override void OnTrackablesChanged(
            List<AREnvironmentProbe> added,
            List<AREnvironmentProbe> updated,
            List<AREnvironmentProbe> removed)
        {
            if (environmentProbesChanged != null)
            {
                environmentProbesChanged(new AREnvironmentProbesChangedEvent(added, updated, removed));
            }
        }

        protected override void OnCreateTrackable(AREnvironmentProbe probe)
        {
            probe.environmentTextureFilterMode = m_EnvironmentTextureFilterMode;
        }

        /// <summary>
        /// Sets the current state of the <see cref="automaticPlacement"/> property to the
        /// <c>XREnvironmentProbeSubsystem</c>, if the subsystem exists and supports automatic placement.
        /// </summary>
        void SetAutomaticPlacementStateOnSubsystem()
        {
            if ((subsystem != null) && subsystem.SubsystemDescriptor.supportsAutomaticPlacement)
            {
                subsystem.automaticPlacement = m_AutomaticPlacement;
            }
        }

        /// <summary>
        /// Sets the current state of the <see cref="environmentTextureHDR"/> property to the
        /// <c>XREnvironmentProbeSubsystem</c>, if the subsystem exists and supports HDR environment textures.
        /// </summary>
        void SetEnvironmentTextureHDRStateOnSubsystem()
        {
            if ((subsystem != null) && subsystem.SubsystemDescriptor.supportsEnvironmentTextureHDR)
            {
                subsystem.environmentTextureHDR = m_EnvironmentTextureHDR;
            }
        }
    }
}
