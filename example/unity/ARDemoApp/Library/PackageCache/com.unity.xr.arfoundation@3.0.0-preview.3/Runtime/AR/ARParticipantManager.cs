using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A manager for <see cref="ARParticipant"/>s. Creates, updates, and removes
    /// <c>GameObject</c>s in response to other users in a multi-user collaborative session.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_ParticipantManager)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ARSessionOrigin))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARParticipantManager.html")]
    public sealed class ARParticipantManager : ARTrackableManager<
        XRParticipantSubsystem,
        XRParticipantSubsystemDescriptor,
        XRParticipant,
        ARParticipant>
    {
        [SerializeField]
        [Tooltip("(Optional) Instantiates this prefab for each participant.")]
        GameObject m_ParticipantPrefab;

        /// <summary>
        /// (Optional) Instantiates this prefab for each participant. If <c>null</c>, an empty <c>GameObject</c>
        /// with a <see cref="ARParticipant"/> component is instantiated instead.
        /// </summary>
        public GameObject participantPrefab
        {
            get { return m_ParticipantPrefab; }
            set { m_ParticipantPrefab = value; }
        }

        /// <summary>
        /// Invoked when participants have changed (been added, updated, or removed).
        /// </summary>
        public event Action<ARParticipantsChangedEventArgs> participantsChanged;

        /// <summary>
        /// Attempt to retrieve an existing <see cref="ARParticipant"/> by <paramref name="trackableId"/>.
        /// </summary>
        /// <param name="trackableId">The <see cref="TrackableId"/> of the participant to retrieve.</param>
        /// <returns>The <see cref="ARParticipant"/> with <paramref name="trackableId"/>, or <c>null</c> if it does not exist.</returns>
        public ARParticipant GetParticipant(TrackableId trackableId)
        {
            if (m_Trackables.TryGetValue(trackableId, out ARParticipant participant))
                return participant;

            return null;
        }

        protected override GameObject GetPrefab()
        {
            return m_ParticipantPrefab;
        }

        protected override void OnTrackablesChanged(
            List<ARParticipant> added,
            List<ARParticipant> updated,
            List<ARParticipant> removed)
        {
            if (participantsChanged != null)
            {
                participantsChanged(
                    new ARParticipantsChangedEventArgs(
                        added,
                        updated,
                        removed));
            }
        }

        /// <summary>
        /// The name to be used for the <c>GameObject</c> whenever a new participant is detected.
        /// </summary>
        protected override string gameObjectName
        {
            get { return "ARParticipant"; }
        }
    }
}
