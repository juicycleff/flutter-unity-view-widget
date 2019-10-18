using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

using Object = UnityEngine.Object;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// The manager for the human body subsystem.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_HumanBodyManager)]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@2.2/api/UnityEngine.XR.ARFoundation.ARHumanBodyManager.html")]
    public sealed class ARHumanBodyManager : ARTrackableManager<XRHumanBodySubsystem, XRHumanBodySubsystemDescriptor, XRHumanBody, ARHumanBody>
    {
        /// <summary>
        /// The texture information for any human stencil image.
        /// </summary>
        /// <value>
        /// The texture information for any human stencil image.
        /// </value>
        ARTextureInfo m_HumanStencilInfo;

        /// <summary>
        /// The texture information for any human depth image.
        /// </summary>
        /// <value>
        /// The texture information for any human depth image.
        /// </value>
        ARTextureInfo m_HumanDepthInfo;

        /// <summary>
        /// Whether 2D human pose estimation is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if 2D human pose estimation is enabled. Otherwise, <c>false</c>.
        /// </value>
        public bool humanBodyPose2DEstimationEnabled
        {
            get { return m_HumanBodyPose2DEstimationEnabled; }
            set
            {
                m_HumanBodyPose2DEstimationEnabled = value;
                if (enabled && subsystem != null)
                {
                    subsystem.humanBodyPose2DEstimationEnabled = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("Whether to estimate the 2D pose for any human bodies detected.")]
        bool m_HumanBodyPose2DEstimationEnabled = false;

        /// <summary>
        /// Whether 3D human pose estimation is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if 3D human pose estimation is enabled. Otherwise, <c>false</c>.
        /// </value>
        public bool humanBodyPose3DEstimationEnabled
        {
            get { return m_HumanBodyPose3DEstimationEnabled; }
            set
            {
                m_HumanBodyPose3DEstimationEnabled = value;
                if (enabled && subsystem != null)
                {
                    subsystem.humanBodyPose3DEstimationEnabled = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("Whether to estimate the 3D pose for any human bodies detected.")]
        bool m_HumanBodyPose3DEstimationEnabled = false;

        /// <summary>
        /// Whether 3D human body scale estimation is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if 3D human body scale estimation is enabled. Otherwise, <c>false</c>.
        /// </value>
        public bool humanBodyPose3DScaleEstimationEnabled
        {
            get { return m_HumanBodyPose3DScaleEstimationEnabled; }
            set
            {
                m_HumanBodyPose3DScaleEstimationEnabled = value;
                if (enabled && subsystem != null)
                {
                    subsystem.humanBodyPose3DScaleEstimationEnabled = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("Whether to estimate the 3D pose for any human bodies detected.")]
        bool m_HumanBodyPose3DScaleEstimationEnabled = false;

        /// <summary>
        /// The mode for generating the human segmentation stencil texture.
        /// </summary>
        /// <value>
        /// The mode for generating the human segmentation stencil texture.
        /// </value>
        /// <summary>
        /// Specifies the human segmentation stencil mode.
        /// </summary>
        /// <value>
        /// The human segmentation stencil mode.
        /// </value>
        public HumanSegmentationMode humanSegmentationStencilMode
        {
            get { return m_HumanSegmentationStencilMode; }
            set
            {
                m_HumanSegmentationStencilMode = value;
                if (enabled && subsystem != null)
                {
                    subsystem.humanSegmentationStencilMode = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("The mode for generating human segmentation stencil texture.")]
        HumanSegmentationMode m_HumanSegmentationStencilMode = HumanSegmentationMode.Disabled;

        /// <summary>
        /// The mode for generating the human segmentation depth texture.
        /// </summary>
        /// <value>
        /// The mode for generating the human segmentation depth texture.
        /// </value>
        /// <remarks>
        /// Setting the value for the depth mode to <c>HumanSegmentationMode.HalfScreenResolution</c> or
        /// <c>HumanSegmentationMode.FullScreenResolution</c> do improve the quality of the depth texture content by
        /// executing an image filter over the texture, but neither setting increases the resolution of the texture
        /// currently. <c>HumanSegmentationMode.HalfScreenResolution</c> and
        /// <c>HumanSegmentationMode.FullScreenResolution</c> result in the equivalent depth texture quality in the
        /// current implementation.
        /// </remarks>
        public HumanSegmentationMode humanSegmentationDepthMode
        {
            get { return m_HumanSegmentationDepthMode; }
            set
            {
                m_HumanSegmentationDepthMode = value;
                if (enabled && subsystem != null)
                {
                    subsystem.humanSegmentationDepthMode = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("The mode for generating human segmentation depth texture. See note in ARCameraManager.cs for additional information on the limitation of the current implementation.")]
        HumanSegmentationMode m_HumanSegmentationDepthMode = HumanSegmentationMode.Disabled;

        /// <summary>
        /// The prefab object to instantiate at the location of the human body origin.
        /// </summary>
        /// <value>
        /// The prefab object to instantiate at the location of the human body origin.
        /// </value>
        public GameObject humanBodyPrefab
        {
            get { return m_HumanBodyPrefab; }
            set { m_HumanBodyPrefab = value; }
        }

        [SerializeField]
        [Tooltip("The prefab to instantiate at the origin for the detected human body if human body pose estimation is enabled.")]
        GameObject m_HumanBodyPrefab;

        /// <summary>
        /// The name for any generated game objects.
        /// </summary>
        /// <value>
        /// The name for any generated game objects.
        /// </value>
        protected override string gameObjectName
        {
            get { return "ARHumanBody"; }
        }

        /// <summary>
        /// The human segmentation stencil texture.
        /// </summary>
        /// <value>
        /// The human segmentation stencil texture, if any. Otherwise, <c>null</c>.
        /// </value>
        public Texture2D humanStencilTexture
        {
            get { return m_HumanStencilInfo.texture; }
        }

        /// <summary>
        /// The human segmentation depth texture.
        /// </summary>
        /// <value>
        /// The human segmentation depth texture, if any. Otherwise, <c>null</c>.
        /// </value>
        public Texture2D humanDepthTexture
        {
            get { return m_HumanDepthInfo.texture; }
        }

        /// <summary>
        /// The event that is fired when a change to the detected human bodies is reported.
        /// </summary>
        public event Action<ARHumanBodiesChangedEventArgs> humanBodiesChanged;

        /// <summary>
        /// Gets the prefab object to instantiate at the location of the trackable.
        /// </summary>
        /// <returns>
        /// A game object to instantiate at the location of the trackable, or <c>null</c>.
        /// </returns>
        protected override GameObject GetPrefab()
        {
            return m_HumanBodyPrefab;
        }

        /// <summary>
        /// Get the human body matching the trackable identifier.
        /// </summary>
        /// <param name="trackableId">The trackable identifier for querying a human body trackable.</param>
        /// <returns>
        /// The human body trackable, if found. Otherwise, <c>null</c>.
        /// </returns>
        public ARHumanBody GetHumanBody(TrackableId trackableId)
        {
            ARHumanBody humanBody;
            if (m_Trackables.TryGetValue(trackableId, out humanBody))
            {
                return humanBody;
            }

            return null;
        }

        /// <summary>
        /// Gets the human body pose 2D joints for the current frame.
        /// </summary>
        /// <param name="allocator">The allocator to use for the returned array memory.</param>
        /// <returns>
        /// The array of body pose 2D joints.
        /// </returns>
        /// <remarks>
        /// The returned array may be empty if the system is not enabled for human body pose 2D or if the system
        /// does not detect a human in the camera image.
        /// </remarks>
        /// <exception cref="System.NotSupportedException">Thrown if the implementation does not support human body
        /// pose 2D.</exception>
        public NativeArray<XRHumanBodyPose2DJoint> GetHumanBodyPose2DJoints(Allocator allocator)
        {
            return ((subsystem == null) ? new NativeArray<XRHumanBodyPose2DJoint>(0, allocator)
                    : subsystem.GetHumanBodyPose2DJoints(allocator));
        }

        /// <summary>
        /// Callback before the subsystem is started (but after it is created).
        /// </summary>
        protected override void OnBeforeStart()
        {
            subsystem.humanBodyPose2DEstimationEnabled = m_HumanBodyPose2DEstimationEnabled;
            subsystem.humanBodyPose3DEstimationEnabled = m_HumanBodyPose3DEstimationEnabled;
            subsystem.humanBodyPose3DScaleEstimationEnabled = m_HumanBodyPose3DScaleEstimationEnabled;
            subsystem.humanSegmentationStencilMode = m_HumanSegmentationStencilMode;
            subsystem.humanSegmentationDepthMode = m_HumanSegmentationDepthMode;
        }

        /// <summary>
        /// Callback as the manager is being updated.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (subsystem == null)
            {
                return;
            }

            UpdateTexturesInfos();
        }

        /// <summary>
        /// Callback as the manager is being destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_Trackables.Clear();
        }

        /// <summary>
        /// Callback after the session relative data has been set to update the skeleton for the human body.
        /// </summary>
        /// <param name="arBody">The human body trackable being updated.</param>
        /// <param name="xrBody">The raw human body data from the subsystem.</param>
        protected override void OnAfterSetSessionRelativeData(ARHumanBody arBody, XRHumanBody xrBody)
        {
            arBody.UpdateSkeleton(subsystem);
        }

        /// <summary>
        /// Callback when the trackable deltas are being reported.
        /// </summary>
        /// <param name="added">The list of human bodies added to the set of trackables.</param>
        /// <param name="updated">The list of human bodies updated in the set of trackables.</param>
        /// <param name="removed">The list of human bodies removed to the set of trackables.</param>
        protected override void OnTrackablesChanged(List<ARHumanBody> added, List<ARHumanBody> updated, List<ARHumanBody> removed)
        {
            if ((humanBodiesChanged != null) &&
                ((added.Count > 0) || (updated.Count > 0) || (removed.Count > 0)))
            {
                humanBodiesChanged(new ARHumanBodiesChangedEventArgs(added, updated, removed));
            }
        }

        /// <summary>
        /// Update the human segmentation image information if the subsystem has human segmentation images.
        /// </summary>
        void UpdateTexturesInfos()
        {
            XRTextureDescriptor humanStencilDescriptor;
            if (subsystem.TryGetHumanStencil(out humanStencilDescriptor))
            {
                m_HumanStencilInfo = ARTextureInfo.GetUpdatedTextureInfo(m_HumanStencilInfo, humanStencilDescriptor);
            }
            else
            {
                m_HumanStencilInfo.Reset();
            }

            XRTextureDescriptor humanDepthDescriptor;
            if (subsystem.TryGetHumanDepth(out humanDepthDescriptor))
            {
                m_HumanDepthInfo = ARTextureInfo.GetUpdatedTextureInfo(m_HumanDepthInfo, humanDepthDescriptor);
            }
            else
            {
                m_HumanDepthInfo.Reset();
            }
        }
    }
}
