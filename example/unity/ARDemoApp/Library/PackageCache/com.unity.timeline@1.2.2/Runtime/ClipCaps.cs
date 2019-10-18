using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Describes the timeline features supported by a clip
    /// </summary>
    [Flags]
    public enum ClipCaps
    {
        /// <summary>
        /// No features are supported.
        /// </summary>
        None            = 0 ,

        /// <summary>
        /// The clip supports loops.
        /// </summary>
        Looping         = 1 << 0,

        /// <summary>
        /// The clip supports clip extrapolation.
        /// </summary>
        Extrapolation   = 1 << 1,

        /// <summary>
        /// The clip supports initial local times greater than zero.
        /// </summary>
        ClipIn          = 1 << 2,

        /// <summary>
        /// The clip supports time scaling.
        /// </summary>
        SpeedMultiplier = 1 << 3,

        /// <summary>
        /// The clip supports blending between clips.
        /// </summary>
        Blending        = 1 << 4,

        /// <summary>
        /// All features are supported.
        /// </summary>
        All = ~None
    }

    static class TimelineClipCapsExtensions
    {
        public static bool SupportsLooping(this TimelineClip clip)
        {
            return clip != null && (clip.clipCaps & ClipCaps.Looping) != ClipCaps.None;
        }

        public static bool SupportsExtrapolation(this TimelineClip clip)
        {
            return clip != null && (clip.clipCaps & ClipCaps.Extrapolation) != ClipCaps.None;
        }

        public static bool SupportsClipIn(this TimelineClip clip)
        {
            return clip != null && (clip.clipCaps & ClipCaps.ClipIn) != ClipCaps.None;
        }

        public static bool SupportsSpeedMultiplier(this TimelineClip clip)
        {
            return clip != null && (clip.clipCaps & ClipCaps.SpeedMultiplier) != ClipCaps.None;
        }

        public static bool SupportsBlending(this TimelineClip clip)
        {
            return clip != null && (clip.clipCaps & ClipCaps.Blending) != ClipCaps.None;
        }

        public static bool HasAll(this ClipCaps caps, ClipCaps flags)
        {
            return (caps & flags) == flags;
        }

        public static bool HasAny(this ClipCaps caps, ClipCaps flags)
        {
            return (caps & flags) != 0;
        }
    }
}
