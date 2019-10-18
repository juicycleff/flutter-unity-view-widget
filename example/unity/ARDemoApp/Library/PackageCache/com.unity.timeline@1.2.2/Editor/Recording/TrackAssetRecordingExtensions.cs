using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class TrackAssetRecordingExtensions
    {
        static readonly Dictionary<TrackAsset, AnimationClip> s_ActiveClips = new Dictionary<TrackAsset, AnimationClip>();

        internal static void OnRecordingArmed(this TrackAsset track, PlayableDirector director)
        {
            if (track == null)
                return;

            var animClip = track.FindRecordingAnimationClipAtTime(director.time);
            if (animClip == null)
                return;

            s_ActiveClips[track] = animClip;
        }

        internal static void OnRecordingTimeChanged(this TrackAsset track, PlayableDirector director)
        {
            if (track == null)
                return;

            var animClip = track.FindRecordingAnimationClipAtTime(director.time);
            AnimationClip prevClip = track.GetActiveRecordingAnimationClip();
            if (prevClip != animClip)
            {
                s_ActiveClips[track] = animClip;
            }
        }

        internal static void OnRecordingUnarmed(this TrackAsset track, PlayableDirector director)
        {
            s_ActiveClips.Remove(track);
        }

        internal static bool CanRecordAtTime(this TrackAsset track, double time)
        {
            // Animation Track
            var animTrack = track as AnimationTrack;
            if (animTrack != null)
            {
                if (!animTrack.inClipMode)
                    return true;

                TimelineClip clip = null;
                return FindRecordingClipAtTime(track, time, out clip);
            }

            // Custom track
            return track.clips.Any(x => x.start < time + TimeUtility.kTimeEpsilon && x.HasAnyAnimatableParameters());
        }

        internal static AnimationClip GetActiveRecordingAnimationClip(this TrackAsset track)
        {
            AnimationClip clip = null;
            s_ActiveClips.TryGetValue(track, out clip);
            return clip;
        }

        internal static bool IsRecordingToClip(this TrackAsset track, TimelineClip clip)
        {
            if (track == null || clip == null)
                return false;
            var animClip = track.GetActiveRecordingAnimationClip();
            if (animClip == null)
                return false;
            if (animClip == clip.curves)
                return true;

            var animAsset = clip.asset as AnimationPlayableAsset;
            return animAsset != null && animClip == animAsset.clip;
        }

        // Finds the clip at the given time that recording should use
        //    returns whether recording at this particular point is valid
        // The target clip will be returned, even if recording at that time is invalid
        // in case of recording in a blend OR recording to a non-timeline clip
        internal static bool FindRecordingClipAtTime(this TrackAsset track, double time, out TimelineClip target)
        {
            target = null;
            if (track == null)
            {
                return false;
            }

            // only animation tracks require the recordable flag as they are recording
            //  to an animation clip
            bool requiresRecordable = (track as AnimationTrack) != null;
            if (requiresRecordable)
            {
                track.SortClips();
                var sortedByStartTime = track.clips;
                int i = 0;
                for (i = 0; i < sortedByStartTime.Length; i++)
                {
                    var clip = sortedByStartTime[i];
                    if (clip.start <= time && clip.end >= time)
                    {
                        target = clip;
                        // not recordable
                        if (!clip.recordable)
                            return false;

                        // in a blend
                        if (!Mathf.Approximately(1.0f, clip.EvaluateMixIn(time) * clip.EvaluateMixOut(time)))
                            return false;

                        return true;
                    }

                    if (clip.start > time)
                    {
                        break;
                    }
                }

                return false;
            }


            // Recordable playable assets -- takes the last clip that matches
            track.SortClips();
            for (int i = 0; i < track.clips.Length; i++)
            {
                var clip = track.clips[i];
                if (clip.start <= time && clip.end >= time && clip.HasAnyAnimatableParameters())
                    target = clip;

                if (clip.start > time)
                    break;
            }

            return target != null;
        }

        // Given a track, return the animation clip
        internal static AnimationClip FindRecordingAnimationClipAtTime(this TrackAsset trackAsset, double time)
        {
            if (trackAsset == null)
                return null;

            AnimationTrack animTrack = trackAsset as AnimationTrack;
            if (animTrack != null && !animTrack.inClipMode)
            {
                return animTrack.infiniteClip;
            }

            TimelineClip displayBackground;
            trackAsset.FindRecordingClipAtTime(time, out displayBackground);
            if (displayBackground != null)
            {
                if (displayBackground.recordable)
                {
                    AnimationPlayableAsset asset = displayBackground.asset as AnimationPlayableAsset;
                    if (asset != null)
                        return asset.clip;
                }
                else if (animTrack == null)
                {
                    if (displayBackground.curves == null)
                        displayBackground.CreateCurves(AnimationTrackRecorder.GetUniqueRecordedClipName(displayBackground.parentTrack, TimelineClip.kDefaultCurvesName));

                    return displayBackground.curves;
                }
            }
            else if (trackAsset.HasAnyAnimatableParameters())
            {
                if (trackAsset.curves == null)
                    trackAsset.CreateCurves(AnimationTrackRecorder.GetUniqueRecordedClipName(trackAsset.timelineAsset, TrackAsset.kDefaultCurvesName));

                return trackAsset.curves;
            }

            return null;
        }

        internal static void ClearRecordingState()
        {
            s_ActiveClips.Clear();
        }
    }
}
