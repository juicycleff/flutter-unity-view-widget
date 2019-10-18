using System;
using UnityEngine;

// Extension methods responsible for managing extrapolation time
namespace UnityEngine.Timeline
{
    static class Extrapolation
    {
        /// <summary>
        /// The minimum amount of extrapolation time to apply
        /// </summary>
        internal static readonly double kMinExtrapolationTime = TimeUtility.kTimeEpsilon * 1000;

        // Calculates the extrapolation times
        internal static void CalculateExtrapolationTimes(this TrackAsset asset)
        {
            TimelineClip[] clips = asset.clips;
            if (clips == null || clips.Length == 0)
                return;

            // extrapolation not supported
            if (!clips[0].SupportsExtrapolation())
                return;

            var orderedClips = SortClipsByStartTime(clips);
            if (orderedClips.Length > 0)
            {
                // post extrapolation is the minimum time to the next clip
                for (int i = 0; i < orderedClips.Length; i++)
                {
                    double minTime = double.PositiveInfinity;
                    for (int j = 0; j < orderedClips.Length; j++)
                    {
                        if (i == j)
                            continue;

                        double deltaTime = orderedClips[j].start - orderedClips[i].end;
                        if (deltaTime >= -TimeUtility.kTimeEpsilon && deltaTime < minTime)
                            minTime = Math.Min(minTime, deltaTime);
                        // check for overlapped clips
                        if (orderedClips[j].start <= orderedClips[i].end && orderedClips[j].end > orderedClips[i].end)
                            minTime = 0;
                    }
                    minTime = minTime <= kMinExtrapolationTime ? 0 : minTime;
                    orderedClips[i].SetPostExtrapolationTime(minTime);
                }

                // the first clip gets pre-extrapolation, then it's only respected if there is no post extrapolation
                orderedClips[0].SetPreExtrapolationTime(Math.Max(0, orderedClips[0].start));
                for (int i = 1; i < orderedClips.Length; i++)
                {
                    double preTime = 0;
                    int prevClip = -1;
                    for (int j = 0; j < i; j++)
                    {
                        // overlap, no pre-time
                        if (orderedClips[j].end > orderedClips[i].start)
                        {
                            prevClip = -1;
                            preTime = 0;
                            break;
                        }

                        double gap = orderedClips[i].start - orderedClips[j].end;
                        if (prevClip == -1 || gap < preTime)
                        {
                            preTime = gap;
                            prevClip = j;
                        }
                    }
                    // check for a post extrapolation time
                    if (prevClip >= 0)
                    {
                        if (orderedClips[prevClip].postExtrapolationMode != TimelineClip.ClipExtrapolation.None)
                            preTime = 0;
                    }

                    preTime = preTime <= kMinExtrapolationTime ? 0 : preTime;
                    orderedClips[i].SetPreExtrapolationTime(preTime);
                }
            }
        }

        static TimelineClip[] SortClipsByStartTime(TimelineClip[] clips)
        {
            var orderedClips = new TimelineClip[clips.Length];
            Array.Copy(clips, orderedClips, clips.Length);
            Array.Sort(orderedClips, (clip1, clip2) => clip1.start.CompareTo(clip2.start));
            return orderedClips;
        }
    }
}
