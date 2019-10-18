namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// The update order for <c>MonoBehaviour</c>s in ARFoundation.
    /// </summary>
    public static class ARUpdateOrder
    {
        /// <summary>
        /// The <see cref="ARSession"/>'s update order. Should come first.
        /// </summary>
        public const int k_Session = int.MinValue;

        /// <summary>
        /// The <see cref="ARPlaneManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_PlaneManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARPlane"/>'s update order. Should come after the
        /// <see cref="ARPlaneManager"/>.
        /// </summary>
        public const int k_Plane = k_PlaneManager + 1;

        /// <summary>
        /// The <see cref="ARPointCloudManager"/>'s update order. Should come
        /// after the <see cref="ARSession"/>.
        /// </summary>
        public const int k_PointCloudManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARPointCloud"/>'s update order. Should come after
        /// the <see cref="ARPointCloudManager"/>.
        /// </summary>
        public const int k_PointCloud = k_PointCloudManager + 1;

        /// <summary>
        /// The <see cref="ARReferencePointManager"/>'s update order.
        /// Should come after the <see cref="ARSession"/>.
        /// </summary>
        public const int k_ReferencePointManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARReferencePointManager"/>'s update order.
        /// Should come after the <see cref="ARReferencePointManager"/>.
        /// </summary>
        public const int k_ReferencePoint = k_ReferencePointManager + 1;

        /// <summary>
        /// The <see cref="ARInputManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_InputManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARCameraManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_CameraManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARFaceManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_FaceManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARFace"/>'s update order. Should come after
        /// the <see cref="ARFaceManager"/>.
        /// </summary>
        public const int k_Face = k_FaceManager + 1;

        /// <summary>
        /// The <see cref="ARTrackedImageManager"/>'s update order.
        /// Should come after the <see cref="ARSession"/>.
        /// </summary>
        public const int k_TrackedImageManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARTrackedImage"/>'s update order.
        /// Should come after the <see cref="ARTrackedImageManager"/>.
        /// </summary>
        public const int k_TrackedImage = k_TrackedImageManager + 1;

        /// <summary>
        /// The <see cref="AREnvironmentProbeManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_EnvironmentProbeManager = k_Session + 1;

        /// <summary>
        /// The <see cref="AREnvironmentProbe"/>'s update order. Should come after
        /// the <see cref="AREnvironmentProbeManager"/>.
        /// </summary>
        public const int k_EnvironmentProbe = k_EnvironmentProbeManager + 1;

        /// <summary>
        /// The <see cref="ARTrackedObjectManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_TrackedObjectManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARTrackedObject"/>'s update order. Should come after
        /// the <see cref="ARTrackedObjectManager"/>.
        /// </summary>
        public const int k_TrackedObject = k_TrackedObjectManager + 1;

        /// <summary>
        /// The <see cref="ARHumanBodyManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_HumanBodyManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARHumanBody"/>'s update order. Should come after
        /// the <see cref="ARHumanBodyManager"/>.
        /// </summary>
        public const int k_HumanBody = k_HumanBodyManager + 1;

        /// <summary>
        /// The <see cref="ARMeshManager"/>'s update order. Should come after
        /// the <see cref="ARSession"/>.
        /// </summary>
        public const int k_MeshManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARParticipantManager"/>'s update order. Should come after the <see cref="ARSession"/>.
        /// </summary>
        public const int k_ParticipantManager = k_Session + 1;

        /// <summary>
        /// The <see cref="ARParticipant"/>'s update order. Should come after the <see cref="ARParticipantManager"/>.
        /// </summary>
        public const int k_Participant = k_ParticipantManager + 1;
    }
}
