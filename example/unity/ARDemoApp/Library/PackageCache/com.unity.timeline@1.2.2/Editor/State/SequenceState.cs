using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class SequenceState : ISequenceState
    {
        readonly WindowState m_WindowState;
        readonly SequenceState m_ParentSequence;

        double m_Time;
        Range? m_CachedEvaluableRange;

        public TimelineAsset asset { get; }
        public PlayableDirector director { get; }
        public TimelineClip hostClip { get; }
        public double start { get; }
        public double timeScale { get; }

        public double duration
        {
            get
            {
                if (asset == null)
                    return 0.0;

                var assetDuration = asset.durationMode == TimelineAsset.DurationMode.FixedLength ? asset.fixedDuration : asset.duration;
                return hostClip == null ? assetDuration : Math.Min(hostClip.duration, assetDuration);
            }
        }

        bool? m_IsReadOnly;
        public bool isReadOnly
        {
            get
            {
                if (!m_IsReadOnly.HasValue)
                    m_IsReadOnly = FileUtil.IsReadOnly(asset);
                return m_IsReadOnly.Value;
            }
        }

        public void ResetIsReadOnly()
        {
            m_IsReadOnly = null;
        }

        public TimelineAssetViewModel viewModel
        {
            get
            {
                return TimelineWindowViewPrefs.GetOrCreateViewModel(asset);
            }
        }

        public double time
        {
            get
            {
                if (m_ParentSequence != null)
                    return hostClip.ToLocalTimeUnbound(m_ParentSequence.time);

                return GetLocalTime();
            }
            set
            {
                var correctedValue = Math.Min(value, TimeUtility.k_MaxTimelineDurationInSeconds);
                viewModel.windowTime = correctedValue;

                if (m_ParentSequence != null)
                    m_ParentSequence.time = hostClip.FromLocalTimeUnbound(correctedValue);
                else
                    SetLocalTime(correctedValue);
            }
        }

        public int frame
        {
            get { return TimeUtility.ToFrames(time, frameRate); }
            set { time = TimeUtility.FromFrames(Mathf.Max(0, value), frameRate); }
        }

        public float frameRate
        {
            get
            {
                if (asset != null)
                    return asset.editorSettings.fps;

                return TimelineAsset.EditorSettings.kDefaultFps;
            }
            set
            {
                TimelineAsset.EditorSettings settings = asset.editorSettings;
                if (Math.Abs(settings.fps - value) > TimeUtility.kFrameRateEpsilon)
                {
                    settings.fps = Mathf.Max(value, (float)TimeUtility.kFrameRateEpsilon);
                    EditorUtility.SetDirty(asset);
                }
            }
        }

        public SequenceState(WindowState windowState, TimelineAsset asset, PlayableDirector director, TimelineClip hostClip, SequenceState parentSequence)
        {
            m_WindowState = windowState;
            m_ParentSequence = parentSequence;

            this.asset = asset;
            this.director = director;
            this.hostClip = hostClip;

            start = hostClip == null ? 0.0 : hostClip.start;
            timeScale = hostClip == null ? 1.0 : hostClip.timeScale * parentSequence.timeScale;
        }

        public Range GetEvaluableRange()
        {
            if (hostClip == null)
                return new Range
                {
                    start = 0.0,
                    end = duration
                };

            if (!m_CachedEvaluableRange.HasValue)
            {
                var globalRange = GetGlobalEvaluableRange();
                m_CachedEvaluableRange = new Range
                {
                    start = ToLocalTime(globalRange.start),
                    end = ToLocalTime(globalRange.end)
                };
            }

            return m_CachedEvaluableRange.Value;
        }

        public string TimeAsString(double timeValue, string format = "F2")
        {
            if (viewModel.timeInFrames)
                return TimeUtility.TimeAsFrames(timeValue, frameRate, format);

            return TimeUtility.TimeAsTimeCode(timeValue, frameRate, format);
        }

        public double ToGlobalTime(double t)
        {
            if (hostClip == null)
                return t;

            return m_ParentSequence.ToGlobalTime(hostClip.FromLocalTimeUnbound(t));
        }

        public double ToLocalTime(double t)
        {
            if (hostClip == null)
                return t;

            return hostClip.ToLocalTimeUnbound(m_ParentSequence.ToLocalTime(t));
        }

        double GetLocalTime()
        {
            if (!m_WindowState.previewMode && !Application.isPlaying)
                return viewModel.windowTime;

            // the time needs to always be synchronized with the director
            if (director != null)
                m_Time = director.time;

            return m_Time;
        }

        void SetLocalTime(double newTime)
        {
            // do this prior to the calback, because the callback pulls from the get
            if (director != null)
                director.time = newTime;

            if (Math.Abs(m_Time - newTime) > TimeUtility.kTimeEpsilon)
            {
                m_Time = newTime;
                m_WindowState.InvokeTimeChangeCallback();
            }
        }

        Range GetGlobalEvaluableRange()
        {
            if (hostClip == null)
                return new Range
                {
                    start = 0.0,
                    end = duration
                };

            var currentRange = new Range
            {
                start = hostClip.ToLocalTimeUnbound(ToGlobalTime(hostClip.start)),
                end = hostClip.ToLocalTimeUnbound(ToGlobalTime(hostClip.end))
            };

            return Range.Intersection(currentRange, m_ParentSequence.GetGlobalEvaluableRange());
        }

        public void Dispose()
        {
            TimelineWindowViewPrefs.SaveViewModel(asset);
        }
    }
}
