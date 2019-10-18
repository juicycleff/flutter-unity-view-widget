// Copyright 2017 Google Inc. All rights reserved.
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

// Modified by Unity from originals located
// https://github.com/googlevr/daydream-elements/blob/master/Assets/DaydreamElements/Elements/ArmModels/Scripts/ArmModels/SwingArmModel.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

namespace UnityEngine.XR.LegacyInputHelpers
{
    public class SwingArmModel : ArmModel
    {
        [Tooltip("Portion of controller rotation applied to the shoulder joint.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        float m_ShoulderRotationRatio = 0.5f;
        /// <summary>
        /// Portion of controller rotation applied to the shoulder joint.
        /// </summary>
        public float shoulderRotationRatio
        {
            get { return m_ShoulderRotationRatio; }
            set { m_ShoulderRotationRatio = value; }
        }

        [Tooltip("Portion of controller rotation applied to the elbow joint.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float m_ElbowRotationRatio = 0.3f;
        /// <summary>
        /// Portion of controller rotation applied to the elbow joint.
        /// </summary>
        public float elbowRotationRatio
        {
            get { return m_ElbowRotationRatio; }
            set { m_ElbowRotationRatio = value; }
        }

        [Tooltip("Portion of controller rotation applied to the wrist joint.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float m_WristRotationRatio = 0.2f;
        /// <summary>
        /// Portion of controller rotation applied to the wrist joint.
        /// </summary>
        public float wristRotationRatio
        {
            get { return m_WristRotationRatio; }
            set { m_WristRotationRatio = value; }
        }
      
        [SerializeField]
        Vector2 m_JointShiftAngle = new Vector2(160.0f, 180.0f);
        /// <summary>
        /// Min angle of the controller before starting to lerp towards the shifted joint ratios.
        /// </summary>
        public float minJointShiftAngle
        {
            get { return m_JointShiftAngle.x; }
            set { m_JointShiftAngle.x = value; }
        }
        /// <summary>
        /// Max angle of the controller before starting to lerp towards the shifted joint ratios.
        /// </summary>
        public float maxJointShiftAngle
        {
            get { return m_JointShiftAngle.y; }
            set { m_JointShiftAngle.y = value; }
        }

        [Tooltip("Exponent applied to the joint shift ratio to control the curve of the shift.")]
        [Range(1.0f, 20.0f)]
        [SerializeField]
        float m_JointShiftExponent = 6.0f;
        /// <summary>
        /// Exponent applied to the joint shift ratio to control the curve of the shift.
        /// </summary>
        public float jointShiftExponent
        {
            get { return m_JointShiftExponent; }
            set { m_JointShiftExponent = value; }
        }

        [Tooltip("Portion of controller rotation applied to the shoulder joint when the controller is backwards.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float m_ShiftedShoulderRotationRatio = 0.1f;
        /// <summary>
        /// Portion of controller rotation applied to the shoulder joint when the controller is backwards.
        /// </summary>
        public float shiftedShoulderRotationRatio
        {
            get { return m_ShiftedShoulderRotationRatio; }
            set { m_ShiftedShoulderRotationRatio = value; }
        }

        [Tooltip("Portion of controller rotation applied to the elbow joint when the controller is backwards.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float m_ShiftedElbowRotationRatio = 0.4f;
        /// <summary>
        /// Portion of controller rotation applied to the elbow joint when the controller is backwards.
        /// </summary>
        public float shiftedElbowRotationRatio
        {
            get { return m_ShiftedElbowRotationRatio; }
            set { m_ShiftedElbowRotationRatio = value; }
        }

        [Tooltip("Portion of controller rotation applied to the wrist joint when the controller is backwards.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float m_ShiftedWristRotationRatio = 0.5f;
        /// <summary>
        /// Portion of controller rotation applied to the wrist joint when the controller is backwards.
        /// </summary>
        public float shiftedWristRotationRatio
        {
            get { return m_ShiftedWristRotationRatio; }
            set { m_ShiftedWristRotationRatio = value; }
        }

        protected override void CalculateFinalJointRotations(Quaternion controllerOrientation, Quaternion xyRotation, Quaternion lerpRotation)
        {
            // As the controller angle increases the ratio of the rotation applied to each joint shifts.
            float totalAngle = Quaternion.Angle(xyRotation, Quaternion.identity);
            float jointShiftAngleRange = maxJointShiftAngle - minJointShiftAngle;
            float angleRatio = Mathf.Clamp01((totalAngle - minJointShiftAngle) / jointShiftAngleRange);
            float jointShiftRatio = Mathf.Pow(angleRatio, m_JointShiftExponent);

            // Calculate what portion of the rotation is applied to each joint.
            float finalShoulderRatio = Mathf.Lerp(m_ShoulderRotationRatio, m_ShiftedShoulderRotationRatio, jointShiftRatio);
            float finalElbowRatio = Mathf.Lerp(m_ElbowRotationRatio, m_ShiftedElbowRotationRatio, jointShiftRatio);
            float finalWristRatio = Mathf.Lerp(m_WristRotationRatio, m_ShiftedWristRotationRatio, jointShiftRatio);

            // Calculate relative rotations for each joint.
            Quaternion swingShoulderRot = Quaternion.Lerp(Quaternion.identity, xyRotation, finalShoulderRatio);
            Quaternion swingElbowRot = Quaternion.Lerp(Quaternion.identity, xyRotation, finalElbowRatio);
            Quaternion swingWristRot = Quaternion.Lerp(Quaternion.identity, xyRotation, finalWristRatio);

            // Calculate final rotations.
            Quaternion shoulderRotation = m_TorsoRotation * swingShoulderRot;
            m_ElbowRotation = shoulderRotation * swingElbowRot;
            m_WristRotation = elbowRotation * swingWristRot;
            m_ControllerRotation = m_TorsoRotation * controllerOrientation;
            m_TorsoRotation = shoulderRotation;
        }
    }
}