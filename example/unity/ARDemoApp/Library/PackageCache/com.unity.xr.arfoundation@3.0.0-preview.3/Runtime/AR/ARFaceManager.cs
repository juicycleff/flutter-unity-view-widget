using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Creates, updates, and removes <c>GameObject</c>s with <see cref="ARFace"/> components under the <see cref="ARSessionOrigin"/>'s <see cref="ARSessionOrigin.trackablesParent"/>.
    /// </summary>
    /// <remarks>
    /// When enabled, this component subscribes to <see cref="ARSubsystemManager.faceAdded"/>,
    /// <see cref="ARSubsystemManager.faceUpdated"/>, and <see cref="ARSubsystemManager.faceRemoved"/>.
    /// If this component is disabled, and there are no other subscribers to those events,
    /// face detection will be disabled on the device.
    /// </remarks>
    [RequireComponent(typeof(ARSessionOrigin))]
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_FaceManager)]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARFaceManager.html")]
    public sealed class ARFaceManager : ARTrackableManager<
        XRFaceSubsystem,
        XRFaceSubsystemDescriptor,
        XRFace,
        ARFace>
    {
        [SerializeField]
        [Tooltip("If not null, instantiates this prefab for each created face.")]
        GameObject m_FacePrefab;

        /// <summary>
        /// Getter/setter for the Face Prefab.
        /// </summary>
        public GameObject facePrefab
        {
            get { return m_FacePrefab; }
            set { m_FacePrefab = value; }
        }

        [SerializeField]
        [Tooltip("The maximum number of faces to track simultaneously.")]
        int m_MaximumFaceCount = 1;

        /// <summary>
        /// Get or set the maximum number of faces to track simultaneously
        /// </summary>
        public int maximumFaceCount
        {
            get
            {
                if (subsystem != null)
                {
                    m_MaximumFaceCount = subsystem.maximumFaceCount;
                }

                return m_MaximumFaceCount;
            }
            set
            {
                if (subsystem != null)
                {
                    m_MaximumFaceCount = subsystem.maximumFaceCount = value;
                }
                else
                {
                    m_MaximumFaceCount = value;
                }
            }
        }

        /// <summary>
        /// Get the supported number of faces that can be tracked simultaneously.
        /// </summary>
        public int supportedFaceCount
        {
            get
            {
                if (subsystem == null)
                    throw new InvalidOperationException("Cannot query for supportedFaceCount when subsystem is null.");

                return subsystem.supportedFaceCount;
            }
        }

        /// <summary>
        /// Raised for each new <see cref="ARFace"/> detected in the environment.
        /// </summary>
        public event Action<ARFacesChangedEventArgs> facesChanged;

        /// <summary>
        /// Attempts to retrieve an <see cref="ARFace"/>.
        /// </summary>
        /// <param name="faceId">The <c>TrackableId</c> associated with the <see cref="ARFace"/>.</param>
        /// <returns>The <see cref="ARFace"/>if found. <c>null</c> otherwise.</returns>
        public ARFace TryGetFace(TrackableId faceId)
        {
            ARFace face;
            m_Trackables.TryGetValue(faceId, out face);

            return face;
        }

        protected override void OnBeforeStart()
        {
            subsystem.maximumFaceCount = m_MaximumFaceCount;
        }

        protected override void OnAfterSetSessionRelativeData(
            ARFace face,
            XRFace sessionRelativeData)
        {
            face.UpdateMesh(subsystem);

            if (subsystem.SubsystemDescriptor.supportsEyeTracking)
                face.UpdateEyes();
        }

        protected override void OnTrackablesChanged(
            List<ARFace> added,
            List<ARFace> updated,
            List<ARFace> removed)
        {
            if (facesChanged != null)
            {
                facesChanged(
                    new ARFacesChangedEventArgs(
                        added,
                        updated,
                        removed));
            }
        }

        protected override GameObject GetPrefab()
        {
            return m_FacePrefab;
        }

        protected override string gameObjectName
        {
            get { return "ARFace"; }
        }
    }
}
