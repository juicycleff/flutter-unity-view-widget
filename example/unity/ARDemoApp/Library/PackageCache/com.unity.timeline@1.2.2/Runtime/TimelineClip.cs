using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Implement this interface to support advanced features of timeline clips.
    /// </summary>
    public interface ITimelineClipAsset
    {
        /// <summary>
        /// Returns a description of the features supported by clips with PlayableAssets implementing this interface.
        /// </summary>
        ClipCaps clipCaps { get; }
    }

    /// <summary>
    /// Represents a clip on the timeline.
    /// </summary>
    [Serializable]
    public partial class TimelineClip : ICurvesOwner, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The default capabilities for a clip
        /// </summary>
        public static readonly ClipCaps kDefaultClipCaps = ClipCaps.Blending;

        /// <summary>
        /// The default length of a clip in seconds.
        /// </summary>
        public static readonly float kDefaultClipDurationInSeconds = 5;

        /// <summary>
        /// The minimum timescale allowed on a clip
        /// </summary>
        public static readonly double kTimeScaleMin = 1.0 / 1000;

        /// <summary>
        /// The maximum timescale allowed on a clip
        /// </summary>
        public static readonly double kTimeScaleMax = 1000;

        internal static readonly string kDefaultCurvesName = "Clip Parameters";

        internal static readonly double kMinDuration = 1 / 60.0;

        // constant representing the longest possible sequence duration
        internal static readonly double kMaxTimeValue = 1000000; // more than a week's time, and within numerical precision boundaries

        /// <summary>
        /// How the clip handles time outside its start and end range.
        /// </summary>
        public enum ClipExtrapolation
        {
            /// <summary>
            /// No extrapolation is applied.
            /// </summary>
            None,

            /// <summary>
            /// Hold the time at the end value of the clip.
            /// </summary>
            Hold,

            /// <summary>
            /// Repeat time values outside the start/end range.
            /// </summary>
            Loop,

            /// <summary>
            /// Repeat time values outside the start/end range, reversing direction at each loop
            /// </summary>
            PingPong,

            /// <summary>
            /// Time values are passed in without modification, extending beyond the clips range
            /// </summary>
            Continue
        };

        /// <summary>
        /// How blend curves are treated in an overlap
        /// </summary>
        public enum BlendCurveMode
        {
            /// <summary>
            /// The curve is normalized against the opposing clip
            /// </summary>
            Auto,

            /// <summary>
            /// The blend curve is fixed.
            /// </summary>
            Manual
        };

        internal TimelineClip(TrackAsset parent)
        {
            // parent clip into track
            parentTrack = parent;
        }

        [SerializeField] double m_Start;
        [SerializeField] double m_ClipIn;
        [SerializeField] Object m_Asset;
        [SerializeField][FormerlySerializedAs("m_HackDuration")] double m_Duration;
        [SerializeField] double m_TimeScale = 1.0;
        [SerializeField] TrackAsset m_ParentTrack;

        // for mixing out scripts - default is no mix out (i.e. flat)
        [SerializeField] double m_EaseInDuration;
        [SerializeField] double m_EaseOutDuration;

        // the blend durations override ease in / out durations
        [SerializeField] double m_BlendInDuration = -1.0f;
        [SerializeField] double m_BlendOutDuration = -1.0f;

        // doubles as ease in/out and blend in/out curves
        [SerializeField] AnimationCurve m_MixInCurve;
        [SerializeField] AnimationCurve m_MixOutCurve;

        [SerializeField] BlendCurveMode m_BlendInCurveMode = BlendCurveMode.Auto;
        [SerializeField] BlendCurveMode m_BlendOutCurveMode = BlendCurveMode.Auto;

        [SerializeField] List<string> m_ExposedParameterNames;
        [SerializeField] AnimationClip m_AnimationCurves;

        [SerializeField] bool m_Recordable;

        // extrapolation
        [SerializeField] ClipExtrapolation m_PostExtrapolationMode;
        [SerializeField] ClipExtrapolation m_PreExtrapolationMode;
        [SerializeField] double m_PostExtrapolationTime;
        [SerializeField] double m_PreExtrapolationTime;

        [SerializeField] string m_DisplayName;

        /// <summary>
        /// Is the clip being extrapolated before its start time?
        /// </summary>
        public bool hasPreExtrapolation
        {
            get { return m_PreExtrapolationMode != ClipExtrapolation.None && m_PreExtrapolationTime > 0; }
        }

        /// <summary>
        /// Is the clip being extrapolated past its end time?
        /// </summary>
        public bool hasPostExtrapolation
        {
            get { return m_PostExtrapolationMode != ClipExtrapolation.None && m_PostExtrapolationTime > 0; }
        }

        /// <summary>
        /// A speed multiplier for the clip;
        /// </summary>
        public double timeScale
        {
            get { return clipCaps.HasAny(ClipCaps.SpeedMultiplier) ? Math.Max(kTimeScaleMin, Math.Min(m_TimeScale, kTimeScaleMax)) : 1.0; }
            set
            {
                UpdateDirty(m_TimeScale, value);
                m_TimeScale = clipCaps.HasAny(ClipCaps.SpeedMultiplier) ? Math.Max(kTimeScaleMin, Math.Min(value, kTimeScaleMax)) : 1.0;
            }
        }

        /// <summary>
        /// The start time, in seconds, of the clip
        /// </summary>
        public double start
        {
            get { return m_Start; }
            set
            {
                UpdateDirty(value, m_Start);
                var newValue = Math.Max(SanitizeTimeValue(value, m_Start), 0);
                if (m_ParentTrack != null && m_Start != newValue)
                {
                    m_ParentTrack.OnClipMove();
                }
                m_Start = newValue;
            }
        }

        /// <summary>
        /// The length, in seconds, of the clip
        /// </summary>
        public double duration
        {
            get { return m_Duration; }
            set
            {
                UpdateDirty(m_Duration, value);
                m_Duration = Math.Max(SanitizeTimeValue(value, m_Duration), double.Epsilon);
            }
        }

        /// <summary>
        /// The end time, in seconds of the clip
        /// </summary>
        public double end
        {
            get { return m_Start + m_Duration; }
        }

        /// <summary>
        /// Local offset time of the clip.
        /// </summary>
        public double clipIn
        {
            get { return clipCaps.HasAny(ClipCaps.ClipIn) ? m_ClipIn : 0; }
            set
            {
                UpdateDirty(m_ClipIn, value);
                m_ClipIn = clipCaps.HasAny(ClipCaps.ClipIn) ? Math.Max(Math.Min(SanitizeTimeValue(value, m_ClipIn), kMaxTimeValue), 0.0) : 0;
            }
        }

        /// <summary>
        /// The name displayed on the clip
        /// </summary>
        public string displayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; }
        }


        /// <summary>
        /// The length, in seconds, of the PlayableAsset attached to the clip.
        /// </summary>
        public double clipAssetDuration
        {
            get
            {
                var playableAsset = m_Asset as IPlayableAsset;
                return playableAsset != null ? playableAsset.duration : double.MaxValue;
            }
        }

        /// <summary>
        /// An animation clip containing animated properties of the attached PlayableAsset
        /// </summary>
        /// <remarks>
        /// This is where animated clip properties are stored.
        /// </remarks>
        public AnimationClip curves
        {
            get { return m_AnimationCurves; }
            internal set { m_AnimationCurves = value; }
        }

        string ICurvesOwner.defaultCurvesName
        {
            get { return kDefaultCurvesName; }
        }

        /// <summary>
        /// Whether this clip contains animated properties for the attached PlayableAsset.
        /// </summary>
        /// <remarks>
        /// This property is false if the curves property is null or if it contains no information.
        /// </remarks>
        public bool hasCurves
        {
            get { return m_AnimationCurves != null && !m_AnimationCurves.empty; }
        }

        /// <summary>
        /// The PlayableAsset attached to the clip.
        /// </summary>
        public Object asset
        {
            get { return m_Asset; }
            set { m_Asset = value; }
        }

        Object ICurvesOwner.assetOwner
        {
            get { return parentTrack; }
        }

        TrackAsset ICurvesOwner.targetTrack
        {
            get { return parentTrack; }
        }

        [Obsolete("underlyingAsset property is obsolete. Use asset property instead", true)]
        public Object underlyingAsset
        {
            get { return null; }
            set {}
        }

        /// <summary>
        /// Returns the TrackAsset to which this clip is attached.
        /// </summary>
        public TrackAsset parentTrack
        {
            get { return m_ParentTrack; }
            set
            {
                if (m_ParentTrack == value)
                    return;

                if (m_ParentTrack != null)
                    m_ParentTrack.RemoveClip(this);

                m_ParentTrack = value;

                if (m_ParentTrack != null)
                    m_ParentTrack.AddClip(this);
            }
        }

        /// <summary>
        /// The ease in duration of the timeline clip in seconds. This only applies if the start of the clip is not overlapping.
        /// </summary>
        public double easeInDuration
        {
            get { return clipCaps.HasAny(ClipCaps.Blending) ? Math.Min(Math.Max(m_EaseInDuration, 0), duration * 0.49) : 0; }
            set { m_EaseInDuration = clipCaps.HasAny(ClipCaps.Blending) ? Math.Max(0, Math.Min(SanitizeTimeValue(value, m_EaseInDuration), duration * 0.49)) : 0; }
        }

        /// <summary>
        /// The ease out duration of the timeline clip in seconds. This only applies if the end of the clip is not overlapping.
        /// </summary>
        public double easeOutDuration
        {
            get { return clipCaps.HasAny(ClipCaps.Blending) ? Math.Min(Math.Max(m_EaseOutDuration, 0), duration * 0.49) : 0; }
            set { m_EaseOutDuration = clipCaps.HasAny(ClipCaps.Blending) ? Math.Max(0, Math.Min(SanitizeTimeValue(value, m_EaseOutDuration), duration * 0.49)) : 0; }
        }

        [Obsolete("Use easeOutTime instead (UnityUpgradable) -> easeOutTime", true)]
        public double eastOutTime
        {
            get { return duration - easeOutDuration + m_Start; }
        }

        /// <summary>
        /// The time in seconds that the ease out begins
        /// </summary>
        public double easeOutTime
        {
            get { return duration - easeOutDuration + m_Start; }
        }

        /// <summary>
        /// The amount of overlap in seconds on the start of a clip.
        /// </summary>
        public double blendInDuration
        {
            get { return clipCaps.HasAny(ClipCaps.Blending) ? m_BlendInDuration : 0; }
            set { m_BlendInDuration = clipCaps.HasAny(ClipCaps.Blending) ? SanitizeTimeValue(value, m_BlendInDuration) : 0; }
        }

        /// <summary>
        /// The amount of overlap in seconds at the end of a clip.
        /// </summary>
        public double blendOutDuration
        {
            get { return clipCaps.HasAny(ClipCaps.Blending) ? m_BlendOutDuration : 0; }
            set { m_BlendOutDuration = clipCaps.HasAny(ClipCaps.Blending) ? SanitizeTimeValue(value, m_BlendOutDuration) : 0; }
        }

        /// <summary>
        /// The mode for calculating the blend curve of the overlap at the start of the clip
        /// </summary>
        public BlendCurveMode blendInCurveMode
        {
            get { return m_BlendInCurveMode; }
            set { m_BlendInCurveMode = value; }
        }

        /// <summary>
        /// The mode for calculating the blend curve of the overlap at the end of the clip
        /// </summary>
        public BlendCurveMode blendOutCurveMode
        {
            get { return m_BlendOutCurveMode; }
            set { m_BlendOutCurveMode = value; }
        }

        /// <summary>
        /// Returns whether the clip is blending in
        /// </summary>
        public bool hasBlendIn { get { return clipCaps.HasAny(ClipCaps.Blending) && m_BlendInDuration > 0; } }

        /// <summary>
        /// Returns whether the clip is blending out
        /// </summary>
        public bool hasBlendOut { get { return clipCaps.HasAny(ClipCaps.Blending) && m_BlendOutDuration > 0; } }

        /// <summary>
        /// The animation curve used for calculating weights during an ease in or a blend in.
        /// </summary>
        public AnimationCurve mixInCurve
        {
            get
            {
                // auto fix broken curves
                if (m_MixInCurve == null || m_MixInCurve.length < 2)
                    m_MixInCurve = GetDefaultMixInCurve();

                return m_MixInCurve;
            }
            set { m_MixInCurve = value; }
        }

        /// <summary>
        /// The amount of the clip being used for ease or blend in as a percentage
        /// </summary>
        public float mixInPercentage
        {
            get { return (float)(mixInDuration / duration); }
        }

        /// <summary>
        /// The amount of the clip blending or easing in, in seconds
        /// </summary>
        public double mixInDuration
        {
            get { return hasBlendIn ? blendInDuration : easeInDuration; }
        }

        /// <summary>
        /// The animation curve used for calculating weights during an ease out or a blend out.
        /// </summary>
        public AnimationCurve mixOutCurve
        {
            get
            {
                if (m_MixOutCurve == null || m_MixOutCurve.length < 2)
                    m_MixOutCurve = GetDefaultMixOutCurve();
                return m_MixOutCurve;
            }
            set { m_MixOutCurve = value; }
        }

        /// <summary>
        /// The time in seconds that an ease out or blend out starts
        /// </summary>
        public double mixOutTime
        {
            get { return duration - mixOutDuration + m_Start; }
        }

        /// <summary>
        /// The amount of the clip blending or easing out, in seconds
        /// </summary>
        public double mixOutDuration
        {
            get { return hasBlendOut ? blendOutDuration : easeOutDuration; }
        }

        /// <summary>
        /// The amount of the clip being used for ease or blend out as a percentage
        /// </summary>
        public float mixOutPercentage
        {
            get { return (float)(mixOutDuration / duration); }
        }

        /// <summary>
        /// Returns whether this clip is recordable in editor
        /// </summary>
        public bool recordable
        {
            get { return m_Recordable; }
            internal set { m_Recordable = value; }
        }

        [Obsolete("exposedParameter is deprecated and will be removed in a future release", true)]
        public List<string> exposedParameters
        {
            get { return m_ExposedParameterNames ?? (m_ExposedParameterNames = new List<string>()); }
        }

        /// <summary>
        /// Returns the capabilities supported by this clip.
        /// </summary>
        public ClipCaps clipCaps
        {
            get
            {
                var clipAsset = asset as ITimelineClipAsset;
                return (clipAsset != null) ? clipAsset.clipCaps : kDefaultClipCaps;
            }
        }

        internal int Hash()
        {
            return HashUtility.CombineHash(m_Start.GetHashCode(),
                m_Duration.GetHashCode(),
                m_TimeScale.GetHashCode(),
                m_ClipIn.GetHashCode(),
                ((int)m_PreExtrapolationMode).GetHashCode(),
                ((int)m_PostExtrapolationMode).GetHashCode());
        }

        /// <summary>
        /// Given a time, returns the weight from the mix out
        /// </summary>
        /// <param name="time">Time (relative to the timeline)</param>
        /// <returns></returns>
        public float EvaluateMixOut(double time)
        {
            if (!clipCaps.HasAny(ClipCaps.Blending))
                return 1.0f;

            if (mixOutDuration > Mathf.Epsilon)
            {
                var perc = (float)(time - mixOutTime) / (float)mixOutDuration;
                perc = Mathf.Clamp01(mixOutCurve.Evaluate(perc));
                return perc;
            }
            return 1.0f;
        }

        /// <summary>
        /// Given a time, returns the weight from the mix in
        /// </summary>
        /// <param name="time">Time (relative to the timeline)</param>
        /// <returns></returns>
        public float EvaluateMixIn(double time)
        {
            if (!clipCaps.HasAny(ClipCaps.Blending))
                return 1.0f;

            if (mixInDuration > Mathf.Epsilon)
            {
                var perc = (float)(time - m_Start) / (float)mixInDuration;
                perc = Mathf.Clamp01(mixInCurve.Evaluate(perc));
                return perc;
            }
            return 1.0f;
        }

        static AnimationCurve GetDefaultMixInCurve()
        {
            return AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        static AnimationCurve GetDefaultMixOutCurve()
        {
            return AnimationCurve.EaseInOut(0, 1, 1, 0);
        }

        /// <summary>
        /// Converts from global time to a clips local time.
        /// </summary>
        /// <param name="time">time relative to the timeline</param>
        /// <returns>
        /// The local time with extrapolation applied
        /// </returns>
        public double ToLocalTime(double time)
        {
            if (time < 0)
                return time;

            // handle Extrapolation
            if (IsPreExtrapolatedTime(time))
                time = GetExtrapolatedTime(time - m_Start, m_PreExtrapolationMode, m_Duration);
            else if (IsPostExtrapolatedTime(time))
                time = GetExtrapolatedTime(time - m_Start, m_PostExtrapolationMode, m_Duration);
            else
                time -= m_Start;

            // handle looping and time scale within the clip
            time *= timeScale;
            time += clipIn;

            return time;
        }

        /// <summary>
        /// Converts from global time to local time of the clip
        /// </summary>
        /// <param name="time">The time relative to the timeline</param>
        /// <returns>The local time, ignoring any extrapolation or bounds</returns>
        public double ToLocalTimeUnbound(double time)
        {
            return (time - m_Start) * timeScale + clipIn;
        }

        /// <summary>
        /// Converts from local time of the clip to global time
        /// </summary>
        /// <param name="time">Time relative to the clip</param>
        /// <returns>The time relative to the timeline</returns>
        internal double FromLocalTimeUnbound(double time)
        {
            return (time - clipIn) / timeScale + m_Start;
        }

        /// <summary>
        /// If this contains an animation asset, returns the animation clip attached. Otherwise returns null.
        /// </summary>
        public AnimationClip animationClip
        {
            get
            {
                if (m_Asset == null)
                    return null;

                var playableAsset = m_Asset as AnimationPlayableAsset;
                return playableAsset != null ? playableAsset.clip : null;
            }
        }

        static double SanitizeTimeValue(double value, double defaultValue)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                Debug.LogError("Invalid time value assigned");
                return defaultValue;
            }

            return Math.Max(-kMaxTimeValue, Math.Min(kMaxTimeValue, value));
        }

        /// <summary>
        /// Returns whether the clip is being extrapolated past the end time.
        /// </summary>
        public ClipExtrapolation postExtrapolationMode
        {
            get { return clipCaps.HasAny(ClipCaps.Extrapolation) ? m_PostExtrapolationMode : ClipExtrapolation.None; }
            internal set { m_PostExtrapolationMode = clipCaps.HasAny(ClipCaps.Extrapolation) ? value : ClipExtrapolation.None; }
        }

        /// <summary>
        /// Returns whether the clip is being extrapolated before the start time.
        /// </summary>
        public ClipExtrapolation preExtrapolationMode
        {
            get { return clipCaps.HasAny(ClipCaps.Extrapolation) ? m_PreExtrapolationMode : ClipExtrapolation.None; }
            internal set { m_PreExtrapolationMode = clipCaps.HasAny(ClipCaps.Extrapolation) ? value : ClipExtrapolation.None; }
        }

        internal void SetPostExtrapolationTime(double time)
        {
            m_PostExtrapolationTime = time;
        }

        internal void SetPreExtrapolationTime(double time)
        {
            m_PreExtrapolationTime = time;
        }

        /// <summary>
        /// Given a time, returns whether it falls within the clips extrapolation
        /// </summary>
        /// <param name="sequenceTime">The time relative to the timeline</param>
        public bool IsExtrapolatedTime(double sequenceTime)
        {
            return IsPreExtrapolatedTime(sequenceTime) || IsPostExtrapolatedTime(sequenceTime);
        }

        /// <summary>
        /// Given a time, returns whether it falls within the clip pre-extrapolation
        /// </summary>
        /// <param name="sequenceTime">The time relative to the timeline</param>
        public bool IsPreExtrapolatedTime(double sequenceTime)
        {
            return preExtrapolationMode != ClipExtrapolation.None &&
                sequenceTime < m_Start && sequenceTime >= m_Start - m_PreExtrapolationTime;
        }

        /// <summary>
        /// Given a time, returns whether it falls within the clip post-extrapolation
        /// </summary>
        /// <param name="sequenceTime">The time relative to the timeline</param>
        public bool IsPostExtrapolatedTime(double sequenceTime)
        {
            return postExtrapolationMode != ClipExtrapolation.None &&
                (sequenceTime > end) && (sequenceTime - end < m_PostExtrapolationTime);
        }

        /// <summary>
        /// The start time of the clip, accounting for pre-extrapolation
        /// </summary>
        public double extrapolatedStart
        {
            get
            {
                if (m_PreExtrapolationMode != ClipExtrapolation.None)
                    return m_Start - m_PreExtrapolationTime;

                return m_Start;
            }
        }

        /// <summary>
        /// The length of the clip in seconds, including extrapolation.
        /// </summary>
        public double extrapolatedDuration
        {
            get
            {
                double length = m_Duration;

                if (m_PostExtrapolationMode != ClipExtrapolation.None)
                    length += Math.Min(m_PostExtrapolationTime, kMaxTimeValue);

                if (m_PreExtrapolationMode != ClipExtrapolation.None)
                    length += m_PreExtrapolationTime;

                return length;
            }
        }

        static double GetExtrapolatedTime(double time, ClipExtrapolation mode, double duration)
        {
            if (duration == 0)
                return 0;

            switch (mode)
            {
                case ClipExtrapolation.None:
                    break;

                case ClipExtrapolation.Loop:
                    if (time < 0)
                        time = duration - (-time % duration);
                    else if (time > duration)
                        time %= duration;
                    break;

                case ClipExtrapolation.Hold:
                    if (time < 0)
                        return 0;
                    if (time > duration)
                        return duration;
                    break;

                case ClipExtrapolation.PingPong:
                    if (time < 0)
                    {
                        time = duration * 2 - (-time % (duration * 2));
                        time = duration - Math.Abs(time - duration);
                    }
                    else
                    {
                        time = time % (duration * 2.0);
                        time = duration - Math.Abs(time - duration);
                    }
                    break;

                case ClipExtrapolation.Continue:
                    break;
            }
            return time;
        }

        /// <summary>
        /// Creates an AnimationClip to store animated properties for the attached PlayableAsset.
        /// </summary>
        /// <remarks>
        /// If curves already exists for this clip, this method produces no result regardless of the
        /// value specified for curvesClipName.
        /// </remarks>
        /// <remarks>
        /// When used from the editor, this method attempts to save the created curves clip to the TimelineAsset.
        /// The TimelineAsset must already exist in the AssetDatabase to save the curves clip. If the TimelineAsset
        /// does not exist, the curves clip is still created but it is not saved.
        /// </remarks>
        /// <param name="curvesClipName">
        /// The name of the AnimationClip to create.
        /// This method does not ensure unique names. If you want a unique clip name, you must provide one.
        /// See ObjectNames.GetUniqueName for information on a method that creates unique names.
        /// </param>
        public void CreateCurves(string curvesClipName)
        {
            if (m_AnimationCurves != null)
                return;

            m_AnimationCurves = TimelineCreateUtilities.CreateAnimationClipForTrack(string.IsNullOrEmpty(curvesClipName) ? kDefaultCurvesName : curvesClipName, parentTrack, true);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            m_Version = k_LatestVersion;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_Version < k_LatestVersion)
            {
                UpgradeToLatestVersion();
            }
        }

        /// <summary>
        /// Outputs a more readable representation of the timeline clip as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UnityString.Format("{0} ({1:F2}, {2:F2}):{3:F2} | {4}", displayName, start, end, clipIn, parentTrack);
        }

#if UNITY_EDITOR
        internal int DirtyIndex { get; private set; }
        internal void MarkDirty()
        {
            DirtyIndex++;
        }

        void UpdateDirty(double oldValue, double newValue)
        {
            if (oldValue != newValue)
                DirtyIndex++;
        }

#else
        void UpdateDirty(double oldValue, double newValue) {}
#endif
    };
}
