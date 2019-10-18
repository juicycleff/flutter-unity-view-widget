using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Runtime clip customized for 'infinite' tracks playables.
    /// Used for clips whose time needs to match the timelines exactly
    /// </summary>
    class InfiniteRuntimeClip : RuntimeElement
    {
        private Playable m_Playable;
        private static readonly Int64 kIntervalEnd = DiscreteTime.GetNearestTick(TimelineClip.kMaxTimeValue);

        public InfiniteRuntimeClip(Playable playable)
        {
            m_Playable = playable;
        }

        public override Int64 intervalStart
        {
            get { return 0; }
        }

        public override Int64 intervalEnd
        {
            get { return kIntervalEnd; }
        }

        public override bool enable
        {
            set
            {
                if (value)
                    m_Playable.Play();
                else
                    m_Playable.Pause();
            }
        }

        public override void EvaluateAt(double localTime, FrameData frameData)
        {
            m_Playable.SetTime(localTime);
        }
    }
}
