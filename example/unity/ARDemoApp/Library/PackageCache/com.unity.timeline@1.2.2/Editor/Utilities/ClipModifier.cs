using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    static class ClipModifier
    {
        public static bool Delete(TimelineAsset timeline, TimelineClip clip)
        {
            return timeline.DeleteClip(clip);
        }

        public static bool Tile(TimelineClip[] clips)
        {
            if (clips.Length < 2)
                return false;

            var clipsByTracks = clips.GroupBy(x => x.parentTrack)
                .Select(track => new {track.Key, Items = track.OrderBy(c => c.start)});

            foreach (var track in clipsByTracks)
            {
                TimelineUndo.PushUndo(track.Key, "Tile");
            }

            foreach (var track in clipsByTracks)
            {
                double newStart = track.Items.First().start;
                foreach (var c in track.Items)
                {
                    c.start = newStart;
                    newStart += c.duration;
                }
            }

            return true;
        }

        public static bool TrimStart(TimelineClip[] clips, double trimTime)
        {
            var result = false;

            foreach (var clip in clips)
                result |= TrimStart(clip, trimTime);

            return result;
        }

        public static bool TrimStart(TimelineClip clip, double trimTime)
        {
            if (clip.asset == null)
                return false;

            if (clip.start > trimTime)
                return false;

            if (clip.end < trimTime)
                return false;

            TimelineUndo.PushUndo(clip.parentTrack, "Trim Clip Start");

            // Note: We are NOT using edit modes in this case because we want the same result
            // regardless of the selected EditMode: split at cursor and delete left part
            SetStart(clip, trimTime);

            return true;
        }

        public static bool TrimEnd(TimelineClip[] clips, double trimTime)
        {
            var result = false;

            foreach (var clip in clips)
                result |= TrimEnd(clip, trimTime);

            return result;
        }

        public static bool TrimEnd(TimelineClip clip, double trimTime)
        {
            if (clip.asset == null)
                return false;

            if (clip.start > trimTime)
                return false;

            if (clip.end < trimTime)
                return false;

            TimelineUndo.PushUndo(clip.parentTrack, "Trim Clip End");
            TrimClipWithEditMode(clip, TrimEdge.End, trimTime);

            return true;
        }

        public static bool MatchDuration(TimelineClip[] clips)
        {
            double referenceDuration = clips[0].duration;
            foreach (var clip in clips)
            {
                TimelineUndo.PushUndo(clip.parentTrack, "Match Clip Duration");

                var newEnd = clip.start + referenceDuration;
                TrimClipWithEditMode(clip, TrimEdge.End, newEnd);
            }

            return true;
        }

        public static bool Split(TimelineClip[] clips, double splitTime, PlayableDirector director)
        {
            var result = false;

            foreach (var clip in clips)
            {
                if (clip.start >= splitTime)
                    continue;

                if (clip.end <= splitTime)
                    continue;

                TimelineUndo.PushUndo(clip.parentTrack, "Split Clip");

                TimelineClip newClip = TimelineHelpers.Clone(clip, director, director, clip.start);

                SetStart(clip, splitTime);
                SetEnd(newClip, splitTime, false);

                // Sort produced by cloning clips on top of each other is unpredictable (it varies between mono runtimes)
                clip.parentTrack.SortClips();

                result = true;
            }

            return result;
        }

        public static void SetStart(TimelineClip clip, double time)
        {
            var supportsClipIn = clip.SupportsClipIn();
            var supportsPadding = TimelineUtility.IsRecordableAnimationClip(clip);

            // treat empty recordable clips as not supporting clip in (there are no keys to modify)
            if (supportsPadding && (clip.animationClip == null || clip.animationClip.empty))
            {
                supportsClipIn = false;
            }

            if (supportsClipIn && !supportsPadding)
            {
                var minStart = clip.FromLocalTimeUnbound(0.0);
                if (time < minStart)
                    time = minStart;
            }

            var maxStart = clip.end - TimelineClip.kMinDuration;
            if (time > maxStart)
                time = maxStart;

            var timeOffset = time - clip.start;
            var duration = clip.duration - timeOffset;

            if (supportsClipIn)
            {
                if (supportsPadding)
                {
                    double clipInGlobal = clip.clipIn / clip.timeScale;
                    double keyShift = -timeOffset;
                    if (timeOffset < 0) // left drag, eliminate clipIn before shifting
                    {
                        double clipInDelta = Math.Max(-clipInGlobal, timeOffset);
                        keyShift = -Math.Min(0, timeOffset - clipInDelta);
                        clip.clipIn += clipInDelta * clip.timeScale;
                    }
                    else if (timeOffset > 0) // right drag, elimate padding in animation clip before adding clip in
                    {
                        var clipInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip.animationClip);
                        double keyDelta = clip.FromLocalTimeUnbound(clipInfo.keyTimes.Min()) - clip.start;
                        keyShift = -Math.Max(0, Math.Min(timeOffset, keyDelta));
                        clip.clipIn += Math.Max(timeOffset + keyShift, 0) * clip.timeScale;
                    }
                    if (keyShift != 0)
                    {
                        AnimationTrackRecorder.ShiftAnimationClip(clip.animationClip, (float)(keyShift * clip.timeScale));
                    }
                }
                else
                {
                    clip.clipIn += timeOffset * clip.timeScale;
                }
            }

            clip.start = time;
            clip.duration = duration;
        }

        public static void SetEnd(TimelineClip clip, double time, bool affectTimeScale)
        {
            var duration = Math.Max(time - clip.start, TimelineClip.kMinDuration);

            if (affectTimeScale && clip.SupportsSpeedMultiplier())
            {
                var f = clip.duration / duration;
                clip.timeScale *= f;
            }

            clip.duration = duration;
        }

        public static bool ResetEditing(TimelineClip[] clips)
        {
            var result = false;

            foreach (var clip in clips)
                result = result || ResetEditing(clip);

            return result;
        }

        public static bool ResetEditing(TimelineClip clip)
        {
            if (clip.asset == null)
                return false;

            TimelineUndo.PushUndo(clip.parentTrack, "Reset Clip Editing");

            clip.clipIn = 0.0;

            if (clip.clipAssetDuration < double.MaxValue)
            {
                var duration = clip.clipAssetDuration / clip.timeScale;
                TrimClipWithEditMode(clip, TrimEdge.End, clip.start + duration);
            }

            return true;
        }

        public static bool MatchContent(TimelineClip[] clips)
        {
            var result = false;

            foreach (var clip in clips)
                result = result || MatchContent(clip);

            return result;
        }

        public static bool MatchContent(TimelineClip clip)
        {
            if (clip.asset == null)
                return false;

            TimelineUndo.PushUndo(clip.parentTrack, "Match Clip Content");

            var newStartCandidate = clip.start - clip.clipIn / clip.timeScale;
            var newStart = newStartCandidate < 0.0 ? 0.0 : newStartCandidate;

            TrimClipWithEditMode(clip, TrimEdge.Start, newStart);

            // In case resetting the start was blocked by edit mode or timeline start, we do the best we can
            clip.clipIn = (clip.start - newStartCandidate) * clip.timeScale;
            if (TimelineHelpers.HasUsableAssetDuration(clip))
            {
                var duration = TimelineHelpers.GetLoopDuration(clip);
                var offset = (clip.clipIn / clip.timeScale) % duration;
                TrimClipWithEditMode(clip, TrimEdge.End, clip.start - offset + duration);
            }

            return true;
        }

        public static void TrimClipWithEditMode(TimelineClip clip, TrimEdge edge, double time)
        {
            var clipItem = ItemsUtils.ToItem(clip);
            EditMode.BeginTrim(clipItem, edge);
            if (edge == TrimEdge.Start)
                EditMode.TrimStart(clipItem, time);
            else
                EditMode.TrimEnd(clipItem, time, false);
            EditMode.FinishTrim();
        }

        public static bool CompleteLastLoop(TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                CompleteLastLoop(clip);
            }

            return true;
        }

        public static void CompleteLastLoop(TimelineClip clip)
        {
            FixLoops(clip, true);
        }

        public static bool TrimLastLoop(TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                TrimLastLoop(clip);
            }

            return true;
        }

        public static void TrimLastLoop(TimelineClip clip)
        {
            FixLoops(clip, false);
        }

        static void FixLoops(TimelineClip clip, bool completeLastLoop)
        {
            if (!TimelineHelpers.HasUsableAssetDuration(clip))
                return;

            var loopDuration = TimelineHelpers.GetLoopDuration(clip);
            var firstLoopDuration = loopDuration - clip.clipIn * (1.0 / clip.timeScale);

            // Making sure we don't trim to zero
            if (!completeLastLoop && firstLoopDuration > clip.duration)
                return;

            var numLoops = (clip.duration - firstLoopDuration) / loopDuration;
            var numCompletedLoops = Math.Floor(numLoops);

            if (!(numCompletedLoops < numLoops))
                return;

            if (completeLastLoop)
                numCompletedLoops += 1;

            var newEnd = clip.start + firstLoopDuration + loopDuration * numCompletedLoops;

            TimelineUndo.PushUndo(clip.parentTrack, "Trim Clip Last Loop");

            TrimClipWithEditMode(clip, TrimEdge.End, newEnd);
        }

        public static bool DoubleSpeed(TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                if (clip.SupportsSpeedMultiplier())
                {
                    TimelineUndo.PushUndo(clip.parentTrack, "Double Clip Speed");
                    clip.timeScale = clip.timeScale * 2.0f;
                }
            }

            return true;
        }

        public static bool HalfSpeed(TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                if (clip.SupportsSpeedMultiplier())
                {
                    TimelineUndo.PushUndo(clip.parentTrack, "Half Clip Speed");
                    clip.timeScale = clip.timeScale * 0.5f;
                }
            }

            return true;
        }

        public static bool ResetSpeed(TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                if (clip.timeScale != 1.0)
                {
                    TimelineUndo.PushUndo(clip.parentTrack, "Reset Clip Speed");
                    clip.timeScale = 1.0;
                }
            }

            return true;
        }
    }
}
