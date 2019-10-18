// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Modified by Unity from original:
// https://github.com/googlevr/gvr-unity-sdk/blob/master/Assets/GoogleVR/Scripts/Controller/ArmModel/GvrArmModel.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.SpatialTracking;

namespace UnityEngine.XR.LegacyInputHelpers
{
    public class ArmModel : BasePoseProvider
    {

        /// <summary> Gets the Pose value from the calculated arm model. as the model returns both position and rotation in all cases, we set both flags on return if successful.</summary>
        public override PoseDataFlags GetPoseFromProvider(out Pose output)
        {
            if (OnControllerInputUpdated())
            {
                output = finalPose;
                return PoseDataFlags.Position | PoseDataFlags.Rotation;
            }
            output = Pose.identity;
            return PoseDataFlags.NoData;
        }

        Pose m_FinalPose;
        /// <summary>
        /// the pose which represents the final tracking result of the arm model
        /// </summary>        
        public Pose finalPose
        {
            get { return m_FinalPose; }
            set { m_FinalPose = value; }
        }

#if ENABLE_VR
        [SerializeField]
        XRNode m_PoseSource = XRNode.LeftHand;
        /// <summary>
        /// the pose to use as the input 3DOF position
        /// </summary>        
        public XRNode poseSource
        {
            get { return m_PoseSource; }
            set { m_PoseSource = value; }
        }

        [SerializeField]
        XRNode m_HeadPoseSource = XRNode.CenterEye;
        /// <summary>
        /// The game object which represents the "head" position of the user
        /// </summary>       
        public XRNode headGameObject
        {
            get { return m_HeadPoseSource; }
            set { m_HeadPoseSource = value; }
        }
#endif

        /// Standard implementation for a mathematical model to make the virtual controller approximate the
        /// physical location of the Daydream controller.

        [SerializeField]
        Vector3 m_ElbowRestPosition = DEFAULT_ELBOW_REST_POSITION;
        /// <summary>
        /// Position of the elbow joint relative to the head before the arm model is applied.
        /// </summary>
        public Vector3 elbowRestPosition
        {
            get { return m_ElbowRestPosition; }
            set { m_ElbowRestPosition = value; }
        }

        [SerializeField]
        Vector3 m_WristRestPosition = DEFAULT_WRIST_REST_POSITION;
        /// <summary>
        /// Position of the wrist joint relative to the elbow before the arm model is applied.
        /// </summary>
        public Vector3 wristRestPosition
        {
            get { return m_WristRestPosition; }
            set { m_WristRestPosition = value; }
        }

        [SerializeField]
        Vector3 m_ControllerRestPosition = DEFAULT_CONTROLLER_REST_POSITION;
        /// <summary>
        /// Position of the controller joint relative to the wrist before the arm model is applied.
        /// </summary>
        public Vector3 controllerRestPosition
        {
            get { return m_ControllerRestPosition; }
            set { m_ControllerRestPosition = value; }
        }

        [SerializeField]
        Vector3 m_ArmExtensionOffset = DEFAULT_ARM_EXTENSION_OFFSET;
        /// <summary>
        /// Offset applied to the elbow position as the controller is rotated upwards.
        /// </summary>
        public Vector3 armExtensionOffset
        {
            get { return m_ArmExtensionOffset; }
            set { m_ArmExtensionOffset = value; }
        }

        [Range(0.0f, 1.0f)]
        [SerializeField]
        float m_ElbowBendRatio = DEFAULT_ELBOW_BEND_RATIO;
        /// <summary>
        /// Ratio of the controller's rotation to apply to the rotation of the elbow.
        /// The remaining rotation is applied to the wrist's rotation.
        /// </summary>
        public float elbowBendRatio
        {
            get { return m_ElbowBendRatio; }
            set { m_ElbowBendRatio = value; }
        }
        
        [SerializeField]
        bool m_IsLockedToNeck = true;
        /// <summary>
        /// If true, the root of the pose is locked to the local position of the player's neck.
        /// </summary>
        public bool isLockedToNeck
        {
            get { return m_IsLockedToNeck; }
            set { m_IsLockedToNeck = value; }
        }

        /// Represent the neck's position relative to the user's head.
        /// If isLockedToNeck is true, this will be the InputTracking position of the Head node modified
        /// by an inverse neck model to approximate the neck position.
        /// Otherwise, it is always zero.
        public Vector3 neckPosition
        {
            get
            {
                return m_NeckPosition;
            }
        }

        /// Represent the shoulder's position relative to the user's head.
        /// This is not actually used as part of the arm model calculations, and exists for debugging.
        public Vector3 shoulderPosition
        {
            get
            {
                Vector3 retVal = m_NeckPosition + m_TorsoRotation * Vector3.Scale(SHOULDER_POSITION, m_HandedMultiplier);
                return retVal;
            }
        }

        /// Represent the shoulder's rotation relative to the user's head.
        /// This is not actually used as part of the arm model calculations, and exists for debugging.
        public Quaternion shoulderRotation
        {
            get
            {
                return m_TorsoRotation;
            }
        }

        /// Represent the elbow's position relative to the user's head.
        public Vector3 elbowPosition
        {
            get
            {
                return m_ElbowPosition;
            }
        }

        /// Represent the elbow's rotation relative to the user's head.
        public Quaternion elbowRotation
        {
            get
            {
                return m_ElbowRotation;
            }
        }

        /// Represent the wrist's position relative to the user's head.
        public Vector3 wristPosition
        {
            get
            {
                return m_WristPosition;
            }
        }

        /// Represent the wrist's rotation relative to the user's head.
        public Quaternion wristRotation
        {
            get
            {
                return m_WristRotation;
            }
        }

        /// Represent the controller's position relative to the head pose
        public Vector3 controllerPosition
        {
            get
            {
                return m_ControllerPosition;
            }
        }

        /// Represent the controllers rotation relative to the user's head.
        public Quaternion controllerRotation
        {
            get
            {
                return m_ControllerRotation;
            }
        }

#if UNITY_EDITOR

        /// Editor only API to allow querying the torso forward direction
        public Vector3 torsoDirection
        {
            get { return m_TorsoDirection; }
        }

        /// Editor only API to allow querying the torso rotation
        public Quaternion torsoRotation
        {
            get { return m_TorsoRotation; }
        }
#endif

        protected Vector3 m_NeckPosition;
        protected Vector3 m_ElbowPosition;
        protected Quaternion m_ElbowRotation;
        protected Vector3 m_WristPosition;
        protected Quaternion m_WristRotation;
        protected Vector3 m_ControllerPosition;
        protected Quaternion m_ControllerRotation;

        /// Multiplier for handedness such that 1 = Right, 0 = Center, -1 = left.
        protected Vector3 m_HandedMultiplier;

        /// Forward direction of user's torso.
        protected Vector3 m_TorsoDirection;

        /// Orientation of the user's torso.
        protected Quaternion m_TorsoRotation;

        // Default values for tuning variables.
        protected static readonly Vector3 DEFAULT_ELBOW_REST_POSITION = new Vector3(0.195f, -0.5f, 0.005f);
        protected static readonly Vector3 DEFAULT_WRIST_REST_POSITION = new Vector3(0.0f, 0.0f, 0.25f);
        protected static readonly Vector3 DEFAULT_CONTROLLER_REST_POSITION = new Vector3(0.0f, 0.0f, 0.05f);
        protected static readonly Vector3 DEFAULT_ARM_EXTENSION_OFFSET = new Vector3(-0.13f, 0.14f, 0.08f);
        protected const float DEFAULT_ELBOW_BEND_RATIO = 0.6f;

        /// Increases elbow bending as the controller moves up (unitless).
        protected const float EXTENSION_WEIGHT = 0.4f;

        /// Rest position for shoulder joint.
        protected static readonly Vector3 SHOULDER_POSITION = new Vector3(0.17f, -0.2f, -0.03f);

        /// Neck offset used to apply the inverse neck model when locked to the head.
        protected static readonly Vector3 NECK_OFFSET = new Vector3(0.0f, 0.075f, 0.08f);

        /// Angle ranges the for arm extension offset to start and end (degrees).
        protected const float MIN_EXTENSION_ANGLE = 7.0f;
        protected const float MAX_EXTENSION_ANGLE = 60.0f;

        protected virtual void OnEnable()
        {
            // Force the torso direction to match the gaze direction immediately.
            // Otherwise, the controller will not be positioned correctly if the ArmModel was enabled
            // when the user wasn't facing forward.
            UpdateTorsoDirection(true);

            // Update immediately to avoid a frame delay before the arm model is applied.
            OnControllerInputUpdated();
        }

        protected virtual void OnDisable()
        {

        }

        public virtual bool OnControllerInputUpdated()
        {
            UpdateHandedness();
            if (UpdateTorsoDirection(false))
            {
                if (UpdateNeckPosition())
                {
                    if (ApplyArmModel())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual void UpdateHandedness()
        {
            // Determine handedness multiplier.
            m_HandedMultiplier.Set(0, 1, 1);
#if ENABLE_VR
            if (m_PoseSource == XRNode.RightHand || m_PoseSource == XRNode.TrackingReference)
            {
                m_HandedMultiplier.x = 1.0f;
            }
            else if (m_PoseSource == XRNode.LeftHand)
            {
                m_HandedMultiplier.x = -1.0f;
            }
#endif
        }

        protected virtual bool UpdateTorsoDirection(bool forceImmediate)
        {
            // Determine the gaze direction horizontally.
#if ENABLE_VR
            Vector3 gazeDirection = new Vector3();
            if (TryGetForwardVector(m_HeadPoseSource, out gazeDirection))
            {
                gazeDirection.y = 0.0f;
                gazeDirection.Normalize();

                // Use the gaze direction to update the forward direction.
                if (forceImmediate)
                {
                    m_TorsoDirection = gazeDirection;
                }
                else
                {
                    Vector3 angAccel;
                    if (TryGetAngularAcceleration(poseSource, out angAccel))
                    {
                        float angularVelocity = angAccel.magnitude;
                        float gazeFilterStrength = Mathf.Clamp((angularVelocity - 0.2f) / 45.0f, 0.0f, 0.1f);
                        m_TorsoDirection = Vector3.Slerp(m_TorsoDirection, gazeDirection, gazeFilterStrength);
                    }
                }

                // Calculate the torso rotation.
                m_TorsoRotation = Quaternion.FromToRotation(Vector3.forward, m_TorsoDirection);
                return true;
            }
#endif
            return false;
        }

        protected virtual bool UpdateNeckPosition()
        {
#if ENABLE_VR
            if (m_IsLockedToNeck && TryGetPosition(m_HeadPoseSource, out m_NeckPosition))
            {
                // Find the approximate neck position by Applying an inverse neck model.
                // This transforms the head position to the center of the head and also accounts
                // for the head's rotation so that the motion feels more natural.
                return ApplyInverseNeckModel(m_NeckPosition, out m_NeckPosition);
            }
#endif

            m_NeckPosition = Vector3.zero;
            return true;
        }

        protected virtual bool ApplyArmModel()
        {
            // Set the starting positions of the joints before they are transformed by the arm model.
            SetUntransformedJointPositions();

            // Get the controller's orientation.
            Quaternion controllerOrientation;
            Quaternion xyRotation;
            float xAngle;
            if (GetControllerRotation(out controllerOrientation, out xyRotation, out xAngle))
            {

                // Offset the elbow by the extension offset.
                float extensionRatio = CalculateExtensionRatio(xAngle);
                ApplyExtensionOffset(extensionRatio);

                // Calculate the lerp rotation, which is used to control how much the rotation of the
                // controller impacts each joint.
                Quaternion lerpRotation = CalculateLerpRotation(xyRotation, extensionRatio);

                CalculateFinalJointRotations(controllerOrientation, xyRotation, lerpRotation);
                ApplyRotationToJoints();
                m_FinalPose.position = m_ControllerPosition;
                m_FinalPose.rotation = m_ControllerRotation;
                return true;
            }
            return false;
        }

        /// Set the starting positions of the joints before they are transformed by the arm model.
        protected virtual void SetUntransformedJointPositions()
        {
            m_ElbowPosition = Vector3.Scale(m_ElbowRestPosition, m_HandedMultiplier);
            m_WristPosition = Vector3.Scale(m_WristRestPosition, m_HandedMultiplier);
            m_ControllerPosition = Vector3.Scale(m_ControllerRestPosition, m_HandedMultiplier);
        }

        /// Calculate the extension ratio based on the angle of the controller along the x axis.
        protected virtual float CalculateExtensionRatio(float xAngle)
        {
            float normalizedAngle = (xAngle - MIN_EXTENSION_ANGLE) / (MAX_EXTENSION_ANGLE - MIN_EXTENSION_ANGLE);
            float extensionRatio = Mathf.Clamp(normalizedAngle, 0.0f, 1.0f);
            return extensionRatio;
        }

        /// Offset the elbow by the extension offset.
        protected virtual void ApplyExtensionOffset(float extensionRatio)
        {
            Vector3 extensionOffset = Vector3.Scale(m_ArmExtensionOffset, m_HandedMultiplier);
            m_ElbowPosition += extensionOffset * extensionRatio;
        }

        /// Calculate the lerp rotation, which is used to control how much the rotation of the
        /// controller impacts each joint.
        protected virtual Quaternion CalculateLerpRotation(Quaternion xyRotation, float extensionRatio)
        {
            float totalAngle = Quaternion.Angle(xyRotation, Quaternion.identity);
            float lerpSuppresion = 1.0f - Mathf.Pow(totalAngle / 180.0f, 6.0f);
            float inverseElbowBendRatio = 1.0f - m_ElbowBendRatio;
            float lerpValue = inverseElbowBendRatio + m_ElbowBendRatio * extensionRatio * EXTENSION_WEIGHT;
            lerpValue *= lerpSuppresion;
            return Quaternion.Lerp(Quaternion.identity, xyRotation, lerpValue);
        }

        /// Determine the final joint rotations relative to the head.
        protected virtual void CalculateFinalJointRotations(Quaternion controllerOrientation, Quaternion xyRotation, Quaternion lerpRotation)
        {
            m_ElbowRotation = m_TorsoRotation * Quaternion.Inverse(lerpRotation) * xyRotation;
            m_WristRotation = m_ElbowRotation * lerpRotation;
            m_ControllerRotation = m_TorsoRotation * controllerOrientation;
        }

        /// Apply the joint rotations to the positions of the joints to determine the final pose.
        protected virtual void ApplyRotationToJoints()
        {
            m_ElbowPosition = m_NeckPosition + m_TorsoRotation * m_ElbowPosition;
            m_WristPosition = m_ElbowPosition + m_ElbowRotation * m_WristPosition;
            m_ControllerPosition = m_WristPosition + m_WristRotation * m_ControllerPosition;
        }

        /// Transform the head position into an approximate neck position.
        protected virtual bool ApplyInverseNeckModel(Vector3 headPosition, out Vector3 calculatedPosition)
        {
            // Determine the gaze direction horizontally.
#if ENABLE_VR
            Quaternion headRotation = new Quaternion();
            if (TryGetRotation(m_HeadPoseSource, out headRotation))
            {
                Vector3 rotatedNeckOffset =
                    headRotation * NECK_OFFSET - NECK_OFFSET.y * Vector3.up;
                headPosition -= rotatedNeckOffset;

                calculatedPosition = headPosition;
                return true;
            }
#endif

            calculatedPosition = Vector3.zero;
            return false;
        }

#if ENABLE_VR
        protected bool TryGetForwardVector(XRNode node, out Vector3 forward)
        {
            Pose tmpPose = new Pose();
            if (TryGetRotation(node, out tmpPose.rotation) &&
                TryGetPosition(node, out tmpPose.position))
            {
                forward = tmpPose.forward;
                return true;
            }

            forward = Vector3.zero;            
            return false;
        }

        List<XR.XRNodeState> xrNodeStateListOrientation = new List<XRNodeState>();
        protected bool TryGetRotation(XRNode node, out Quaternion rotation)
        {
            XR.InputTracking.GetNodeStates(xrNodeStateListOrientation);
            var length = xrNodeStateListOrientation.Count;           
            XRNodeState nodeState;
            for (int i = 0; i < length; ++i)
            {
                nodeState = xrNodeStateListOrientation[i];
                if (nodeState.nodeType == node)
                {
                    if (nodeState.TryGetRotation(out rotation))
                    {
                        return true;
                    }
                }
            }                          
            rotation = Quaternion.identity;            
            return false;
        }

        List<XR.XRNodeState> xrNodeStateListPosition = new List<XRNodeState>();
        protected bool TryGetPosition(XRNode node, out Vector3 position)
        {
            XR.InputTracking.GetNodeStates(xrNodeStateListPosition);
            var length = xrNodeStateListPosition.Count;      
            XRNodeState nodeState;
            for (int i = 0; i < length; ++i)
            {
                nodeState = xrNodeStateListPosition[i];
                if (nodeState.nodeType == node)
                {
                    if (nodeState.TryGetPosition(out position))
                    {
                        return true;
                    }
                }
            }                            
            position = Vector3.zero;            
            return false;
        }
        
        List<XR.XRNodeState> xrNodeStateListAngularAcceleration = new List<XRNodeState>();
        protected bool TryGetAngularAcceleration(XRNode node, out Vector3 angularAccel)
        {
            XR.InputTracking.GetNodeStates(xrNodeStateListAngularAcceleration);
            var length = xrNodeStateListAngularAcceleration.Count;          
            XRNodeState nodeState;
            for (int i = 0; i < length; ++i)
            {
                nodeState = xrNodeStateListAngularAcceleration[i];
                if (nodeState.nodeType == node)
                {
                    if (nodeState.TryGetAngularAcceleration(out angularAccel))
                    {
                        return true;
                    }
                }
            }
            angularAccel = Vector3.zero;            
            return false;
        }

        List<XR.XRNodeState> xrNodeStateListAngularVelocity = new List<XRNodeState>();
        protected bool TryGetAngularVelocity(XRNode node, out Vector3 angVel)
        {
            XR.InputTracking.GetNodeStates(xrNodeStateListAngularVelocity);
            var length = xrNodeStateListAngularVelocity.Count;
            XRNodeState nodeState;
            for (int i = 0; i < length; ++i)
            {
                nodeState = xrNodeStateListAngularVelocity[i];
                if (nodeState.nodeType == node)
                {
                    if (nodeState.TryGetAngularVelocity(out angVel))
                    {
                        return true;
                    }
                }
            }
            angVel = Vector3.zero;
            return false;
        }
#endif

        /// Get the controller's orientation.
        protected bool GetControllerRotation(out Quaternion rotation, out Quaternion xyRotation, out float xAngle)
        {
#if ENABLE_VR
            // Find the controller's orientation relative to the player.
            if (TryGetRotation(poseSource, out rotation))
            {
                rotation = Quaternion.Inverse(m_TorsoRotation) * rotation;

                // Extract just the x rotation angle.
                Vector3 controllerForward = rotation * Vector3.forward;
                xAngle = 90.0f - Vector3.Angle(controllerForward, Vector3.up);

                // Remove the z rotation from the controller.
                xyRotation = Quaternion.FromToRotation(Vector3.forward, controllerForward);
                return true;
            }
#endif

            rotation = Quaternion.identity;
            xyRotation = Quaternion.identity;
            xAngle = 0.0f;
            return false;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor only API to draw debug gizmos to help visualize the arm model
        /// </summary>
        public virtual void OnDrawGizmos()
        {
            if (!enabled)
            {
                return;
            }

            if (transform.parent == null) {                
                return;
            }
           
            Vector3 worldShoulder = transform.parent.TransformPoint(shoulderPosition);
            Vector3 worldElbow = transform.parent.TransformPoint(elbowPosition);
            Vector3 worldwrist = transform.parent.TransformPoint(wristPosition);
            Vector3 worldcontroller = transform.parent.TransformPoint(controllerPosition);
    
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldShoulder, 0.02f);
            Gizmos.DrawLine(worldShoulder, worldElbow);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(worldElbow, 0.02f);
            Gizmos.DrawLine(worldElbow, worldwrist);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(worldwrist, 0.02f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(worldcontroller, 0.02f);
        }
#endif // UNITY_EDITOR
    }
}