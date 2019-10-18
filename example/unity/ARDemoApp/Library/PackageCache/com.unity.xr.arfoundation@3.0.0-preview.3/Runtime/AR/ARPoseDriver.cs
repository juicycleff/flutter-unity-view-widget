using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// The ARPoseDriver component applies the current Pose value of an AR device to the transform of the GameObject.
    /// </summary>
    public class ARPoseDriver : MonoBehaviour
    {
        internal struct NullablePose
        {
            internal Vector3? position;
            internal Quaternion? rotation;
        }

        protected void OnEnable()
        {
            Application.onBeforeRender += OnBeforeRender;
        }

        protected void OnDisable()
        {
            Application.onBeforeRender -= OnBeforeRender;
        }

        protected void Update()
        {
            PerformUpdate();
        }

        protected void OnBeforeRender()
        {
            PerformUpdate();
        }

        protected void PerformUpdate()
        {
            if (!enabled)
                return;

            var updatedPose = GetNodePoseData(XR.XRNode.CenterEye);

            if (updatedPose.position.HasValue)
                transform.localPosition = updatedPose.position.Value;
            if (updatedPose.rotation.HasValue)
                transform.localRotation = updatedPose.rotation.Value;
        }

        static internal List<XR.XRNodeState> nodeStates = new List<XR.XRNodeState>();
        static internal NullablePose GetNodePoseData(XR.XRNode currentNode)
        {
            NullablePose resultPose = new NullablePose();
            XR.InputTracking.GetNodeStates(nodeStates);
            foreach (var nodeState in nodeStates)
            {
                if (nodeState.nodeType == currentNode)
                {
                    var pose = Pose.identity;
                    var positionSuccess = nodeState.TryGetPosition(out pose.position);
                    var rotationSuccess = nodeState.TryGetRotation(out pose.rotation);
                    
                    if (positionSuccess)
                        resultPose.position = pose.position;
                    if (rotationSuccess)
                        resultPose.rotation = pose.rotation;

                    return resultPose;
                }
            }
            return resultPose;
        }
    }
}