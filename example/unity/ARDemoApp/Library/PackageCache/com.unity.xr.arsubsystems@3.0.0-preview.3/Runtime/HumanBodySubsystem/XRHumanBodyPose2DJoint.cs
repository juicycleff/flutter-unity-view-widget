using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Container for a human body pose 2D joint as part of a AR detected screen space skeleton.
    /// </summary>
    public struct XRHumanBodyPose2DJoint : IEquatable<XRHumanBodyPose2DJoint>
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
        /// The position of the joint in 2D screenspace.
        /// </summary>
        /// <value>
        /// The position of the joint in 2D screenspace.
        /// </value>
        public Vector2 position
        {
            get { return m_Position; }
        }
        Vector2 m_Position;

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
        /// Constructs a <c>XRHumanBodyPose2DJoint</c> with the given parameters.
        /// </summary>
        /// <param name="index">The index of the joint in the skeleton hierachy.</param>
        /// <param name="parentIndex">The index of the parent joint in the skeleton hierarchy.</param>
        /// <param name="position">The position of the joint in 2D screenspace.</param>
        /// <param name="tracked">Whether the joint is tracked.</param>
        public XRHumanBodyPose2DJoint(int index, int parentIndex, Vector2 position, bool tracked)
        {
            m_Index = index;
            m_ParentIndex = parentIndex;
            m_Position = position;
            m_Tracked = tracked ? 1 : 0;
        }

        public bool Equals(XRHumanBodyPose2DJoint other)
        {
            return (m_Index.Equals(other.m_Index) && m_ParentIndex.Equals(other.m_ParentIndex)
                    && m_Position.Equals(other.m_Position) && m_Tracked.Equals(other.m_Tracked));
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is XRHumanBodyPose2DJoint) && Equals((XRHumanBodyPose2DJoint)obj));
        }

        public static bool operator ==(XRHumanBodyPose2DJoint lhs, XRHumanBodyPose2DJoint rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRHumanBodyPose2DJoint lhs, XRHumanBodyPose2DJoint rhs)
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
                hashCode = (hashCode * 486187739) + m_Position.GetHashCode();
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
            return String.Format("joint [{0}] -> [{1}] {2}", m_Index, m_ParentIndex,
                                 tracked ? m_Position.ToString(format) : "<not tracked>");
        }
    }
}
