using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TimelineClipGroup
    {
        readonly TimelineClip[] m_Clips;
        readonly TimelineClip m_LeftMostClip;
        readonly TimelineClip m_RightMostClip;

        public TimelineClip[] clips
        {
            get { return m_Clips; }
        }

        public double start
        {
            get { return m_LeftMostClip.start; }
            set
            {
                var offset = value - m_LeftMostClip.start;

                foreach (var clip in m_Clips)
                    clip.start += offset;
            }
        }

        public double end
        {
            get { return m_RightMostClip.end; }
        }

        public TimelineClipGroup(IEnumerable<TimelineClip> clips)
        {
            Debug.Assert(clips != null && clips.Any());

            m_Clips = clips.ToArray();
            m_LeftMostClip = null;
            m_RightMostClip = null;

            foreach (var clip in m_Clips)
            {
                if (m_LeftMostClip == null || clip.start < m_LeftMostClip.start)
                    m_LeftMostClip = clip;

                if (m_RightMostClip == null || clip.end > m_RightMostClip.end)
                    m_RightMostClip = clip;
            }
        }
    }
}
