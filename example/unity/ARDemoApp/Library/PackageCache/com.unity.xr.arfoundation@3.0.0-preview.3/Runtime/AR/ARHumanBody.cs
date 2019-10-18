using System;
using System.Text;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

using Object = UnityEngine.Object;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Trackable human body containing the base pose for the body and the body skeleton.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_HumanBody)]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@2.2/api/UnityEngine.XR.ARFoundation.ARHumanBody.html")]
    public class ARHumanBody : ARTrackable<XRHumanBody, ARHumanBody>, IDisposable
    {
        /// <summary>
        /// The pose for the human body origin.
        /// </summary>
        /// <value>
        /// The pose for the human body origin.
        /// </value>
        public Pose pose
        {
            get { return sessionRelativeData.pose; }
        }

        /// <summary>
        /// The scale factor that relates the implementation's default body height to the estimated height.
        /// </summary>
        /// <value>
        /// The scale factor that relates the implementation's default body height to the estimated height.
        /// </value>
        public float estimatedHeightScaleFactor
        {
            get => sessionRelativeData.estimatedHeightScaleFactor;
        }

        /// <summary>
        /// The array of joints making up the human body skeleton.
        /// </summary>
        /// <value>
        /// The array of joints making up the human body skeleton.
        /// </value>
        public NativeArray<XRHumanBodyJoint> joints
        {
            get { return m_Joints; }
        }
        NativeArray<XRHumanBodyJoint> m_Joints;

        /// <summary>
        /// Update the skeleton for the human body from the subsystem.
        /// </summary>
        /// <param name="bodySubsystem">The human body subsystem from which to query the skeleton.</param>
        internal void UpdateSkeleton(XRHumanBodySubsystem bodySubsystem)
        {
            bodySubsystem.GetSkeleton(trackableId, Allocator.Persistent, ref m_Joints);
        }

        public override string ToString()
        {
            return ToString("0.000");
        }

        public string ToString(string floatingPointFormat)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} [trackableId:{1} pose:{2} numJoints:{3} state:{4}]", name, trackableId.ToString(),
                                 pose.ToString(floatingPointFormat), joints.Length, trackingState.ToString());
            foreach (var joint in m_Joints)
            {
                sb.AppendFormat("\n         {0}", joint.ToString(floatingPointFormat));
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            if (m_Joints.IsCreated)
            {
                m_Joints.Dispose();
            }
        }
    }
}
