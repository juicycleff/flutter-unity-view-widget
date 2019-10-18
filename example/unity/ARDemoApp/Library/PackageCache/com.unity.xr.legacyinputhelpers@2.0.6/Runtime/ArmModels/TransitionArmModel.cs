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

// Modified by Unity from original:
// https://github.com/googlevr/daydream-elements/blob/master/Assets/DaydreamElements/Elements/ArmModels/Scripts/ArmModels/TransitionArmModel.cs

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.SpatialTracking;

[assembly: InternalsVisibleTo("UnityEditor.XR.LegacyInputHelpers")]

namespace UnityEngine.XR.LegacyInputHelpers
{
    [Serializable]
    public class ArmModelTransition
    {
        [SerializeField]
        String m_KeyName;
        /// <summary>
        /// the string name that will be used to trigger a transition
        /// </summary>       
        public string transitionKeyName
        {
            get { return m_KeyName; }
            set { m_KeyName = value; }
        }

        [SerializeField]
        ArmModel m_ArmModel; 
        /// <summary>
        /// the arm model that will be transitioned to on receiving this event.
        /// </summary>
        public ArmModel armModel
        {
            get { return m_ArmModel; }
            set { m_ArmModel = value; }
        }
    }

    public class TransitionArmModel : ArmModel
    {
        [SerializeField]
        ArmModel m_CurrentArmModelComponent = null;
        /// <summary>
        /// This field contains the current active arm model that will be used as the input to the tracked pose driver which is
        /// using the transitional arm model.
        /// </summary>
        public ArmModel currentArmModelComponent
        {
            get { return m_CurrentArmModelComponent; }
            set { m_CurrentArmModelComponent = value;  }
        }

        [SerializeField]
        public List<ArmModelTransition> m_ArmModelTransitions = new List<ArmModelTransition>();

        /// Max number of active transitions that can be going on at one time.
        /// Transitions are only completed when the controller rotates, so if TransitionToArmModel
        /// is called several times without the controller moving, the number of active transitions can
        /// add up.
        private const int MAX_ACTIVE_TRANSITIONS = 10;

        /// When transitioning to a new arm model, drop any old transitions that have barely begun.
        private const float DROP_TRANSITION_THRESHOLD = 0.035f;

        /// Threshold for clamping transitions that have been completed.
        private const float LERP_CLAMP_THRESHOLD = 0.95f;

        /// Minimum amount of angular velocity on the controller before transitioning occurs.
        private const float MIN_ANGULAR_VELOCITY = 0.2f;

        /// Unit less weight for how much the angular velocity impacts the transition.
        private const float ANGULAR_VELOCITY_DIVISOR = 45.0f;

        internal struct ArmModelBlendData
        {
            public ArmModel armModel;
            public float currentBlendAmount;                  
        }


        internal List<ArmModelBlendData> armModelBlendData = new List<ArmModelBlendData>(MAX_ACTIVE_TRANSITIONS);
        ArmModelBlendData currentBlendingArmModel;
        
        public bool Queue(string key)
        {
            // attempt to find the arm model to blend to using the supplied key.
            foreach(var am in m_ArmModelTransitions)
            {
                if(am.transitionKeyName == key)
                {
                    Queue(am.armModel);
                    return true;
                }
            }
            return false;
        }

        public void Queue(ArmModel newArmModel)
        {
            if(newArmModel == null)
            {
                return;
            }
            if(m_CurrentArmModelComponent == null)
            {
                m_CurrentArmModelComponent = newArmModel;
            }

            RemoveJustStartingTransitions();            
            if (armModelBlendData.Count == MAX_ACTIVE_TRANSITIONS)
            {
                RemoveOldestTransition();
            }

            var ambd = new ArmModelBlendData();
            ambd.armModel = newArmModel;
            ambd.currentBlendAmount = 0.0f;            

            armModelBlendData.Add(ambd);
        }
                
        void RemoveJustStartingTransitions()
        {
            for( int i = 0; i < armModelBlendData.Count; ++i)
            {
                ArmModelBlendData ambd = armModelBlendData[i];
                if (ambd.currentBlendAmount < DROP_TRANSITION_THRESHOLD)
                {
                    armModelBlendData.RemoveAt(i);
                }
            }
        }

        void RemoveOldestTransition()
        {
            armModelBlendData.RemoveAt(0);
        }

        public override PoseDataFlags GetPoseFromProvider(out Pose output)
        {
            if (UpdateBlends())
            {
                output = finalPose;               
                return PoseDataFlags.Position | PoseDataFlags.Rotation;
            }
            output = Pose.identity;
            return PoseDataFlags.NoData;
        }

        bool UpdateBlends()
        {
            if (currentArmModelComponent == null)
            {               
                return false;
            }

            if (m_CurrentArmModelComponent.OnControllerInputUpdated())
            {

                m_NeckPosition = m_CurrentArmModelComponent.neckPosition;
                m_ElbowPosition = m_CurrentArmModelComponent.elbowPosition;
                m_WristPosition = m_CurrentArmModelComponent.wristPosition;
                m_ControllerPosition = m_CurrentArmModelComponent.controllerPosition;

                m_ElbowRotation = m_CurrentArmModelComponent.elbowRotation;
                m_WristRotation = m_CurrentArmModelComponent.wristRotation;
                m_ControllerRotation = m_CurrentArmModelComponent.controllerRotation;

#if UNITY_EDITOR
                m_TorsoDirection = m_CurrentArmModelComponent.torsoDirection;
                m_TorsoRotation = m_CurrentArmModelComponent.torsoRotation;
#endif

#if ENABLE_VR
                Vector3 angVel;
                if (TryGetAngularVelocity(poseSource, out angVel) && armModelBlendData.Count > 0)
                {

                    float angularVelocity = angVel.magnitude;
                    float lerpValue = Mathf.Clamp(((angularVelocity) - MIN_ANGULAR_VELOCITY) / ANGULAR_VELOCITY_DIVISOR, 0.0f, 0.1f);
          
                    for (int i = 0; i < armModelBlendData.Count; ++i)
                    {
                        ArmModelBlendData ambd = armModelBlendData[i];
                        ambd.currentBlendAmount = Mathf.Lerp(ambd.currentBlendAmount, 1.0f, lerpValue);
                        if (ambd.currentBlendAmount > LERP_CLAMP_THRESHOLD)
                        {
                            ambd.currentBlendAmount = 1.0f;
                        }
                        else
                        {
                            ambd.armModel.OnControllerInputUpdated();

                            m_NeckPosition = Vector3.Slerp(neckPosition, ambd.armModel.neckPosition, ambd.currentBlendAmount);
                            m_ElbowPosition = Vector3.Slerp(elbowPosition, ambd.armModel.elbowPosition, ambd.currentBlendAmount);
                            m_WristPosition = Vector3.Slerp(wristPosition, ambd.armModel.wristPosition, ambd.currentBlendAmount);
                            m_ControllerPosition = Vector3.Slerp(controllerPosition, ambd.armModel.controllerPosition, ambd.currentBlendAmount);

                            m_ElbowRotation = Quaternion.Slerp(elbowRotation, ambd.armModel.elbowRotation, ambd.currentBlendAmount);
                            m_WristRotation = Quaternion.Slerp(wristRotation, ambd.armModel.wristRotation, ambd.currentBlendAmount);
                            m_ControllerRotation = Quaternion.Slerp(controllerRotation, ambd.armModel.controllerRotation, ambd.currentBlendAmount);

                        }
                        // write back.
                        armModelBlendData[i] = ambd;

                        if (ambd.currentBlendAmount >= 1.0f)
                        {
                            m_CurrentArmModelComponent = ambd.armModel;
                            armModelBlendData.RemoveRange(0, i + 1);
                        }
                    }
                }
                else if (armModelBlendData.Count > 0)
                {
                    Debug.LogErrorFormat(this.gameObject, "Unable to get angular acceleration for node");
                    return false;
                }
#endif

                finalPose = new Pose(controllerPosition, controllerRotation);
                return true;
            }
            else
            {
                return false;
            }
        }

#if UNITY_EDITOR
        internal List<ArmModelBlendData> GetActiveBlends()
        {
            return armModelBlendData;
        }
#endif
    }
}
