using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    // The RuntimeClip wraps a single clip in an instanciated sequence.
    // It supports the IInterval interface so that it can be stored in the interval tree
    // It is this class that is returned by an interval tree query.
    class RuntimeClip : RuntimeClipBase
    {
        TimelineClip m_Clip;
        Playable m_Playable;
        Playable m_ParentMixer;

        public override double start
        {
            get { return m_Clip.extrapolatedStart; }
        }

        public override double duration
        {
            get { return m_Clip.extrapolatedDuration; }
        }

        public RuntimeClip(TimelineClip clip, Playable clipPlayable, Playable parentMixer)
        {
            Create(clip, clipPlayable, parentMixer);
        }

        void Create(TimelineClip clip, Playable clipPlayable, Playable parentMixer)
        {
            m_Clip = clip;
            m_Playable = clipPlayable;
            m_ParentMixer = parentMixer;
            clipPlayable.Pause();
        }

        public TimelineClip clip
        {
            get { return m_Clip; }
        }

        public Playable mixer
        {
            get { return m_ParentMixer; }
        }

        public Playable playable
        {
            get { return m_Playable; }
        }

        public override bool enable
        {
            set
            {
                if (value && m_Playable.GetPlayState() != PlayState.Playing)
                {
                    m_Playable.Play();
                }
                else if (!value && m_Playable.GetPlayState() != PlayState.Paused)
                {
                    m_Playable.Pause();
                    if (m_ParentMixer.IsValid())
                        m_ParentMixer.SetInputWeight(m_Playable, 0.0f);
                }
            }
        }

        public void SetTime(double time)
        {
            m_Playable.SetTime(time);
        }

        public void SetDuration(double duration)
        {
            m_Playable.SetDuration(duration);
        }

        public override void EvaluateAt(double localTime, FrameData frameData)
        {
            enable = true;

            float weight = 1.0f;
            if (clip.IsPreExtrapolatedTime(localTime))
                weight = clip.EvaluateMixIn((float)clip.start);
            else if (clip.IsPostExtrapolatedTime(localTime))
                weight = clip.EvaluateMixOut((float)clip.end);
            else
                weight = clip.EvaluateMixIn(localTime) * clip.EvaluateMixOut(localTime);

            if (mixer.IsValid())
                mixer.SetInputWeight(playable, weight);

            // localTime of the sequence to localtime of the clip
            double clipTime = clip.ToLocalTime(localTime);
            if (clipTime.CompareTo(0.0) >= 0)
            {
                SetTime(clipTime);
            }
            SetDuration(clip.extrapolatedDuration);
        }
    }
}
