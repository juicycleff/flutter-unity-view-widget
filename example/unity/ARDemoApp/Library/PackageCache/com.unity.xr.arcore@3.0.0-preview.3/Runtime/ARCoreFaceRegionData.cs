using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Holds data associated with a face "region".
    /// </summary>
    /// <seealso cref="ARCoreFaceSubsystem.GetRegionPoses(ARSubsystems.TrackableId, Unity.Collections.Allocator, ref Unity.Collections.NativeArray{ARCoreFaceRegionData})"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARCoreFaceRegionData : IEquatable<ARCoreFaceRegionData>
    {
        ARCoreFaceRegion m_Region;
        Pose m_Pose;

        /// <summary>
        /// The region this data describes.
        /// </summary>
        public ARCoreFaceRegion region { get { return m_Region; } }

        /// <summary>
        /// The pose associated with the face <see cref="region"/>.
        /// </summary>
        public Pose pose { get { return m_Pose; } }

        /// <summary>
        /// Constructs an <see cref="ARCoreFaceRegionData"/>.
        /// </summary>
        /// <param name="region">The region this data describes.</param>
        /// <param name="pose">The pose associated with the given <paramref name="region"/>.</param>
        public ARCoreFaceRegionData(ARCoreFaceRegion region, Pose pose)
        {
            m_Region = region;
            m_Pose = pose;
        }

        public bool Equals(ARCoreFaceRegionData other)
        {
            return
                (m_Region == other.m_Region) &&
                m_Pose.Equals(other.m_Pose);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = ((int)m_Region).GetHashCode();
                hash = hash * 486187739 + m_Pose.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARCoreFaceRegionData))
                return false;

            return Equals((ARCoreFaceRegionData)obj);
        }

        public override string ToString()
        {
            return string.Format("Region: {0}, Pose: {1}", m_Region, m_Pose);
        }

        public static bool operator ==(ARCoreFaceRegionData lhs, ARCoreFaceRegionData rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARCoreFaceRegionData lhs, ARCoreFaceRegionData rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
