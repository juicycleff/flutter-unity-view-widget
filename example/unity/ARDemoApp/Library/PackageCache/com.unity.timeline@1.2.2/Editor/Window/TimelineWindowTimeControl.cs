using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineWindowTimeControl : IAnimationWindowControl
    {
        [Serializable]
        public struct ClipData
        {
            public double start;
            public double duration;
            public TrackAsset track;
        }

        [SerializeField] ClipData m_ClipData;
        [SerializeField] TimelineClip m_Clip;
        [SerializeField] AnimationWindowState m_AnimWindowState;

        TrackAsset track
        {
            get
            {
                if (m_Clip != null)
                {
                    return m_Clip.parentTrack;
                }
                return m_ClipData.track;
            }
        }

        static TimelineWindow window
        {
            get
            {
                return TimelineWindow.instance;
            }
        }

        static WindowState state
        {
            get
            {
                if (window != null)
                    return window.state;
                return null;
            }
        }

        void OnStateChange()
        {
            if (state != null && state.dirtyStamp > 0 && m_AnimWindowState != null)
                m_AnimWindowState.Repaint();
        }

        public void Init(AnimationWindowState animState, TimelineClip clip)
        {
            m_Clip = clip;
            m_AnimWindowState = animState;
        }

        public void Init(AnimationWindowState animState, ClipData clip)
        {
            m_ClipData = clip;
            m_AnimWindowState = animState;
        }

        public override void OnEnable()
        {
            if (state != null)
                state.OnTimeChange += OnStateChange;

            base.OnEnable();
        }

        public void OnDisable()
        {
            if (state != null)
                state.OnTimeChange -= OnStateChange;
        }

        public override AnimationKeyTime time
        {
            get
            {
                if (state == null)
                    return AnimationKeyTime.Time(0.0f, 0.0f);

                return AnimationKeyTime.Time(ToAnimationClipTime(state.editSequence.time), state.referenceSequence.frameRate);
            }
        }

        void ChangeTime(float newTime)
        {
            if (state != null && state.editSequence.director != null)
            {
                // avoid rounding errors
                var finalTime = ToGlobalTime(newTime);
                if (TimeUtility.OnFrameBoundary(finalTime, state.referenceSequence.frameRate, TimeUtility.kFrameRateEpsilon))
                    finalTime = TimeUtility.RoundToFrame(finalTime, state.referenceSequence.frameRate);
                state.editSequence.time = finalTime;

                window.Repaint();
            }
        }

        static void ChangeFrame(int frame)
        {
            if (state != null)
            {
                state.editSequence.frame = frame;
                window.Repaint();
            }
        }

        public override void GoToTime(float newTime)
        {
            ChangeTime(newTime);
        }

        public override void GoToFrame(int frame)
        {
            ChangeFrame(frame);
        }

        public override void StartScrubTime() {}

        public override void EndScrubTime() {}

        public override void ScrubTime(float newTime)
        {
            ChangeTime(newTime);
        }

        public override void GoToPreviousFrame()
        {
            if (state != null)
                ChangeFrame(state.editSequence.frame - 1);
        }

        public override void GoToNextFrame()
        {
            if (state != null)
                ChangeFrame(state.editSequence.frame + 1);
        }

        AnimationWindowCurve[] GetCurves()
        {
            var curves =
                (m_AnimWindowState.showCurveEditor &&
                    m_AnimWindowState.activeCurves.Count > 0) ? m_AnimWindowState.activeCurves : m_AnimWindowState.allCurves;
            return curves.ToArray();
        }

        public override void GoToPreviousKeyframe()
        {
            var newTime = AnimationWindowUtility.GetPreviousKeyframeTime(GetCurves(), time.time, m_AnimWindowState.clipFrameRate);
            GoToTime(m_AnimWindowState.SnapToFrame(newTime, AnimationWindowState.SnapMode.SnapToClipFrame));
        }

        public override void GoToNextKeyframe()
        {
            var newTime = AnimationWindowUtility.GetNextKeyframeTime(GetCurves(), time.time, m_AnimWindowState.clipFrameRate);
            GoToTime(m_AnimWindowState.SnapToFrame(newTime, AnimationWindowState.SnapMode.SnapToClipFrame));
        }

        public override void GoToFirstKeyframe()
        {
            GoToTime(0);
        }

        public override void GoToLastKeyframe()
        {
            double animClipTime = 0;
            if (m_Clip != null)
            {
                var curves = m_Clip.curves;
                var animAsset = m_Clip.asset as AnimationPlayableAsset;
                if (animAsset != null)
                {
                    animClipTime = animAsset.clip != null ? animAsset.clip.length : 0;
                }
                else if (curves != null)
                {
                    animClipTime = curves.length;
                }
                else
                {
                    animClipTime = m_Clip.clipAssetDuration;
                }
            }
            else
            {
                animClipTime = m_ClipData.duration;
            }

            GoToTime((float)animClipTime);
        }

        public override bool canPlay
        {
            get
            {
                return state != null && state.previewMode;
            }
        }

        public override bool playing
        {
            get
            {
                return state != null && state.playing;
            }
        }

        static void SetPlaybackState(bool playbackState)
        {
            if (state == null || playbackState == state.playing)
                return;

            state.SetPlaying(playbackState);
        }

        public override bool StartPlayback()
        {
            SetPlaybackState(true);
            return state != null && state.playing;
        }

        public override void StopPlayback()
        {
            SetPlaybackState(false);
        }

        public override bool PlaybackUpdate() { return state != null && state.playing; }

        public override bool canRecord
        {
            get { return state != null && state.canRecord; }
        }

        public override bool recording
        {
            get { return state != null && state.recording; }
        }

        public override bool canPreview
        {
            get { return false; }
        }

        public override bool previewing
        {
            get { return false; }
        }

        public override bool StartRecording(Object targetObject)
        {
            if (!canRecord)
                return false;
            if (Application.isPlaying)
                return false;

            if (state != null && track != null)
            {
                state.ArmForRecord(track);
                return state.recording;
            }

            return false;
        }

        public override void StopRecording()
        {
            if (Application.isPlaying)
                return;

            if (state != null && track != null)
                state.UnarmForRecord(track);
        }

        public override void OnSelectionChanged() {}

        public override void ResampleAnimation() {}

        public override bool StartPreview()
        {
            if (state != null)
                state.previewMode = true;
            return state != null && state.previewMode;
        }

        public override void StopPreview()
        {
            if (state != null)
                state.previewMode = false;
        }

        public override void ProcessCandidates() {}
        public override void ClearCandidates() {}

        double durationD
        {
            get
            {
                if (m_Clip != null)
                {
                    return ToAnimationClipTime(m_Clip.end);
                }
                return m_ClipData.duration;
            }
        }

        double ToGlobalTime(float localTime)
        {
            if (m_Clip != null)
                return Math.Max(0, m_Clip.FromLocalTimeUnbound(localTime));
            return Math.Max(0, m_ClipData.start + localTime);
        }

        float ToAnimationClipTime(double globalTime)
        {
            if (m_Clip != null)
                return (float)m_Clip.ToLocalTimeUnbound(globalTime);
            return (float)(globalTime - m_ClipData.start);
        }
    }
}
