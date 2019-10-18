using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    // Special runtime clip implementation that handles playables that use a scheduling system
    //  such as Audio
    internal class ScheduleRuntimeClip : RuntimeClipBase
    {
        private TimelineClip m_Clip;
        private Playable m_Playable;
        private Playable m_ParentMixer;
        private double m_StartDelay;
        private double m_FinishTail;
        private bool m_Started = false;

        // represents the start point when we want to start getting updated
        public override double start
        {
            get { return Math.Max(0, m_Clip.start - m_StartDelay); }
        }

        public override double duration
        {
            get { return m_Clip.duration + m_FinishTail + m_Clip.start - start; }
        }

        public void SetTime(double time)
        {
            m_Playable.SetTime(time);
        }

        public TimelineClip clip { get { return m_Clip; } }
        public Playable mixer { get { return m_ParentMixer; } }
        public Playable playable { get { return m_Playable; } }

        public ScheduleRuntimeClip(TimelineClip clip, Playable clipPlayable,
                                   Playable parentMixer, double startDelay = 0.2, double finishTail = 0.1)
        {
            Create(clip, clipPlayable, parentMixer, startDelay, finishTail);
        }

        private void Create(TimelineClip clip, Playable clipPlayable, Playable parentMixer,
            double startDelay, double finishTail)
        {
            m_Clip = clip;
            m_Playable = clipPlayable;
            m_ParentMixer = parentMixer;
            m_StartDelay = startDelay;
            m_FinishTail = finishTail;
            clipPlayable.Pause();
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

                m_Started &= value;
            }
        }

        public override void EvaluateAt(double localTime, FrameData frameData)
        {
            if (frameData.timeHeld)
            {
                enable = false;
                return;
            }


            bool forceSeek = frameData.seekOccurred || frameData.timeLooped || frameData.evaluationType == FrameData.EvaluationType.Evaluate;

            // If we are in the tail region of the clip, then dont do anything
            if (localTime > start + duration - m_FinishTail)
                return;

            // this may set the weight to 1 in a delay, but it will avoid missing the start
            float weight = clip.EvaluateMixIn(localTime) * clip.EvaluateMixOut(localTime);
            if (mixer.IsValid())
                mixer.SetInputWeight(playable, weight);

            // localTime of the sequence to localtime of the clip
            if (!m_Started || forceSeek)
            {
                // accounts for clip in and speed
                double clipTime = clip.ToLocalTime(Math.Max(localTime, clip.start));
                // multiply by the time scale so the delay is local to the clip
                //  Audio will rescale based on it's effective time scale (which includes the parent)
                double startDelay = Math.Max(clip.start - localTime, 0) * clip.timeScale;
                double durationLocal = m_Clip.duration * clip.timeScale;
                if (m_Playable.IsPlayableOfType<AudioClipPlayable>())
                    ((AudioClipPlayable)m_Playable).Seek(clipTime, startDelay, durationLocal);

                m_Started = true;
            }
        }
    }
}
