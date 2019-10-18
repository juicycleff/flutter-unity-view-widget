using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A session-unique identifier for trackables in the real-world environment, e.g., planes and feature points.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ids are generally unique to a particular session, but multiple sessions may produce
    /// identical ids for different trackables.
    /// </para><para>
    /// A trackable id is a 128 bit number, stored as two ulongs. This makes it large enough to hold a <c>Guid</c>.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct TrackableId : IEquatable<TrackableId>
    {
        /// <summary>
        /// Get the invalid id.
        /// </summary>
        public static TrackableId invalidId { get { return s_InvalidId; } }

        /// <summary>
        /// The first half of the id.
        /// </summary>
        public ulong subId1
        {
            get { return m_SubId1; }
            set { m_SubId1 = value; }
        }

        /// <summary>
        /// The second half of the id.
        /// </summary>
        public ulong subId2
        {
            get { return m_SubId2; }
            set { m_SubId2 = value; }
        }

        /// <summary>
        /// Constructs a <c>TrackableId</c> from two <c>ulong</c>s.
        /// </summary>
        /// <param name="subId1">The first half of the id.</param>
        /// <param name="subId2">The second half of the id.</param>
        public TrackableId(ulong subId1, ulong subId2)
        {
            m_SubId1 = subId1;
            m_SubId2 = subId2;
        }

        /// <summary>
        /// Generates a string representation of the id suitable for debugging.
        /// </summary>
        /// <returns>A string representation of the id.</returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}",
                m_SubId1.ToString("X16"),
                m_SubId2.ToString("X16"));
        }

        /// <summary>
        /// Generates a hash code suitable for use in a <c>Dictionary</c> or <c>Set</c>.
        /// </summary>
        /// <returns>A hash code for participation in certain collections.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = m_SubId1.GetHashCode();
                return hash * 486187739 + m_SubId2.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is TrackableId && Equals((TrackableId)obj);
        }

        public bool Equals(TrackableId other)
        {
            return (m_SubId1 == other.m_SubId1) && (m_SubId2 == other.m_SubId2);
        }

        public static bool operator==(TrackableId id1, TrackableId id2)
        {
            return
                (id1.m_SubId1 == id2.m_SubId1) &&
                (id1.m_SubId2 == id2.m_SubId2);
        }

        public static bool operator!=(TrackableId id1, TrackableId id2)
        {
            return
                (id1.m_SubId1 != id2.m_SubId1) ||
                (id1.m_SubId2 != id2.m_SubId2);
        }

        static TrackableId s_InvalidId = new TrackableId(0, 0);

        ulong m_SubId1;

        ulong m_SubId2;
    }
}
