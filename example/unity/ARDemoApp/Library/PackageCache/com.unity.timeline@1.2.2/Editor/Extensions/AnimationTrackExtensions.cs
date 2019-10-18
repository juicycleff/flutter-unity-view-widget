using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class AnimationTrackExtensions
    {
        public static void ConvertToClipMode(this AnimationTrack track)
        {
            if (!track.CanConvertToClipMode())
                return;

            TimelineUndo.PushUndo(track, "Convert To Clip");

            if (!track.infiniteClip.empty)
            {
                var animClip = track.infiniteClip;
                TimelineUndo.PushUndo(animClip, "Convert To Clip");
                TimelineUndo.PushUndo(track, "Convert To Clip");
                var start = AnimationClipCurveCache.Instance.GetCurveInfo(animClip).keyTimes.FirstOrDefault();
                animClip.ShiftBySeconds(-start);

                track.infiniteClip = null;
                var clip = track.CreateClip(animClip);

                clip.start = start;
                clip.preExtrapolationMode = track.infiniteClipPreExtrapolation;
                clip.postExtrapolationMode = track.infiniteClipPostExtrapolation;
                clip.recordable = true;
                if (Mathf.Abs(animClip.length) < TimelineClip.kMinDuration)
                {
                    clip.duration = 1;
                }

                var animationAsset = clip.asset as AnimationPlayableAsset;
                if (animationAsset)
                {
                    animationAsset.position = track.infiniteClipOffsetPosition;
                    animationAsset.eulerAngles = track.infiniteClipOffsetEulerAngles;

                    // going to / from infinite mode should reset this. infinite mode
                    animationAsset.removeStartOffset = track.infiniteClipRemoveOffset;
                    animationAsset.applyFootIK = track.infiniteClipApplyFootIK;
                    animationAsset.loop = track.infiniteClipLoop;

                    track.infiniteClipOffsetPosition = Vector3.zero;
                    track.infiniteClipOffsetEulerAngles = Vector3.zero;
                }

                track.CalculateExtrapolationTimes();
            }

            track.infiniteClip = null;

            EditorUtility.SetDirty(track);
        }

        public static void ConvertFromClipMode(this AnimationTrack track, TimelineAsset timeline)
        {
            if (!track.CanConvertFromClipMode())
                return;

            TimelineUndo.PushUndo(track, "Convert From Clip");

            var clip = track.clips[0];
            var delta = (float)clip.start;
            track.infiniteClipTimeOffset = 0.0f;
            track.infiniteClipPreExtrapolation = clip.preExtrapolationMode;
            track.infiniteClipPostExtrapolation = clip.postExtrapolationMode;

            var animAsset = clip.asset as AnimationPlayableAsset;
            if (animAsset)
            {
                track.infiniteClipOffsetPosition = animAsset.position;
                track.infiniteClipOffsetEulerAngles = animAsset.eulerAngles;
                track.infiniteClipRemoveOffset = animAsset.removeStartOffset;
                track.infiniteClipApplyFootIK = animAsset.applyFootIK;
                track.infiniteClipLoop = animAsset.loop;
            }

            // clone it, it may not be in the same asset
            var animClip = clip.animationClip;

            float scale = (float)clip.timeScale;
            if (!Mathf.Approximately(scale, 1.0f))
            {
                if (!Mathf.Approximately(scale, 0.0f))
                    scale = 1.0f / scale;
                animClip.ScaleTime(scale);
            }

            TimelineUndo.PushUndo(animClip, "Convert From Clip");
            animClip.ShiftBySeconds(delta);

            // manually delete the clip
            var asset = clip.asset;
            clip.asset = null;

            // Remove the clip, remove old assets
            ClipModifier.Delete(timeline, clip);
            TimelineUndo.PushDestroyUndo(null, track, asset, "Convert From Clip");

            track.infiniteClip = animClip;

            EditorUtility.SetDirty(track);
        }

        public static bool CanConvertToClipMode(this AnimationTrack track)
        {
            if (track == null || track.inClipMode)
                return false;
            return (track.infiniteClip != null && !track.infiniteClip.empty);
        }

        // Requirements to go from clip mode
        //  - one clip, recordable, and animation clip belongs to the same asset as the track
        public static bool CanConvertFromClipMode(this AnimationTrack track)
        {
            if ((track == null) ||
                (!track.inClipMode) ||
                (track.clips.Length != 1) ||
                (track.clips[0].start < 0) ||
                (!track.clips[0].recordable))
                return false;

            var asset = track.clips[0].asset as AnimationPlayableAsset;
            if (asset == null)
                return false;

            return TimelineHelpers.HaveSameContainerAsset(track, asset.clip);
        }
    }
}
