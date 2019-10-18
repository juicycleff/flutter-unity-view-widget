using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Containter for the human body joint data.
    /// </summary>
    public struct XRHumanBodyJoint : IEquatable<XRHumanBodyJoint>
    {
        /// <summary>
        /// The index for the joint in the skeleton hierachy.
        /// </summary>
        /// <value>
        /// The index for the joint in the skeleton hierachy.
        /// </value>
        /// <remarks>
        /// All indices will be non-negative.
        /// </remarks>
        public int index
        {
            get { return m_Index; }
        }
        int m_Index;

        /// <summary>
        /// The index for the parent joint in the skeleton hierachy.
        /// </summary>
        /// <value>
        /// The index for the parent joint in the skeleton hierachy.
        /// </value>
        /// <remarks>
        /// A parent index that is negative represents that the joint has no parent in the hierachy.
        /// </remarks>
        public int parentIndex
        {
            get { return m_ParentIndex; }
        }
        int m_ParentIndex;

        /// <summary>
        /// The scale relative to the parent joint.
        /// </summary>
        /// <value>
        /// The scale relative to the parent joint.
        /// </value>
        public Vector3 localScale
        {
            get { return m_LocalScale; }
        }
        Vector3 m_LocalScale;

        /// <summary>
        /// The pose relative to the parent joint.
        /// </summary>
        /// <value>
        /// The pose relative to the parent joint.
        /// </value>
        public Pose localPose
        {
            get { return m_LocalPose; }
        }
        Pose m_LocalPose;

        /// <summary>
        /// The scale relative to the human body origin.
        /// </summary>
        /// <value>
        /// The scale relative to the human body origin.
        /// </value>
        public Vector3 anchorScale
        {
            get { return m_AnchorScale; }
        }
        Vector3 m_AnchorScale;

        /// <summary>
        /// The pose relative to the human body origin.
        /// </summary>
        /// <value>
        /// The pose relative to the human body origin.
        /// </value>
        public Pose anchorPose
        {
            get { return m_AnchorPose; }
        }
        Pose m_AnchorPose;

        /// <summary>
        /// Whether the joint is tracked.
        /// </summary>
        /// <value>
        /// <c>true</c> if the joint is tracked. Otherwise, <c>false</c>.
        /// </value>
        public bool tracked
        {
            get { return (m_Tracked != 0); }
        }
        int m_Tracked;

        /// <summary>
        /// Construct the human body joint.
        /// </summary>
        /// <param name="index">The index for the joint in the skeleton.</param>
        /// <param name="parentIndex">The index for the parent joint in the skeleton.</param>
        /// <param name="localScale">The scale relative to the parent joint.</param>
        /// <param name="localPose">The pose relative to the parent joint.</param>
        /// <param name="anchorScale">The scale relative to the human body origin.</param>
        /// <param name="anchorPose">The pose relative to the human body origin.</param>
        /// <param name="tracked">Whether the joint is tracked.</param>
        public XRHumanBodyJoint(int index, int parentIndex, Vector3 localScale, Pose localPose, Vector3 anchorScale, Pose anchorPose, bool tracked)
        {
            m_Index = index;
            m_ParentIndex = parentIndex;
            m_LocalScale = localScale;
            m_LocalPose = localPose;
            m_AnchorScale = anchorScale;
            m_AnchorPose = anchorPose;
            m_Tracked = tracked ? 1 : 0;
        }

        public bool Equals(XRHumanBodyJoint other)
        {
            return (m_Index.Equals(other.m_Index) && m_ParentIndex.Equals(other.m_ParentIndex)
                    && m_LocalScale.Equals(other.m_LocalScale) && m_LocalPose.Equals(other.m_LocalPose)
                    && m_AnchorScale.Equals(other.m_AnchorScale) && m_AnchorPose.Equals(other.m_AnchorPose)
                    && m_Tracked.Equals(other.m_Tracked));
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is XRHumanBodyJoint) && Equals((XRHumanBodyJoint)obj));
        }

        public static bool operator ==(XRHumanBodyJoint lhs, XRHumanBodyJoint rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRHumanBodyJoint lhs, XRHumanBodyJoint rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_Index.GetHashCode();
                hashCode = (hashCode * 486187739) + m_ParentIndex.GetHashCode();
                hashCode = (hashCode * 486187739) + m_LocalScale.GetHashCode();
                hashCode = (hashCode * 486187739) + m_LocalPose.GetHashCode();
                hashCode = (hashCode * 486187739) + m_AnchorScale.GetHashCode();
                hashCode = (hashCode * 486187739) + m_AnchorPose.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Tracked.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return ToString("F5");
        }

        public string ToString(string format)
        {
            return String.Format("joint [{0}] -> [{1}] localScale:{2} localPose:{3} anchorScale:{4} anchorPose:{5} tracked:{6}",
                                 m_Index, m_ParentIndex, m_LocalScale.ToString(format), m_LocalPose.ToString(format),
                                 m_AnchorScale.ToString(format), m_AnchorPose.ToString(format), tracked.ToString());
        }
    }
}
