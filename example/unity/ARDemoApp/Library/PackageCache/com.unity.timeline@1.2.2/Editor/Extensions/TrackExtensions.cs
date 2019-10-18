using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    // Editor-only extension methods on track assets
    static class TrackExtensions
    {
        public static readonly double kMinOverlapTime = TimeUtility.kTimeEpsilon * 1000;

        public static AnimationClip GetOrCreateClip(this AnimationTrack track)
        {
            if (track.infiniteClip == null && !track.inClipMode)
                track.CreateInfiniteClip(AnimationTrackRecorder.GetUniqueRecordedClipName(track, AnimationTrackRecorder.kRecordClipDefaultName));

            return track.infiniteClip;
        }

        public static TimelineClip CreateClip(this TrackAsset track, double time)
        {
            var attr = track.GetType().GetCustomAttributes(typeof(TrackClipTypeAttribute), true);

            if (attr.Length == 0)
                return null;

            if (TimelineWindow.instance.state == null)
                return null;

            if (attr.Length == 1)
            {
                var clipClass = (TrackClipTypeAttribute)attr[0];

                var clip =  TimelineHelpers.CreateClipOnTrack(clipClass.inspectedType, track, TimelineWindow.instance.state);
                clip.start = time;
                return clip;
            }

            return null;
        }

        static bool Overlaps(TimelineClip blendOut, TimelineClip blendIn)
        {
            if (blendIn == blendOut)
                return false;

            if (Math.Abs(blendIn.start - blendOut.start) < TimeUtility.kTimeEpsilon)
            {
                return blendIn.duration >= blendOut.duration;
            }

            return blendIn.start >= blendOut.start && blendIn.start < blendOut.end;
        }

        public static void ComputeBlendsFromOverlaps(this TrackAsset asset)
        {
            ComputeBlendsFromOverlaps(asset.clips);
        }

        internal static void ComputeBlendsFromOverlaps(TimelineClip[] clips)
        {
            foreach (var clip in clips)
            {
                clip.blendInDuration = -1;
                clip.blendOutDuration = -1;
            }

            foreach (var clip in clips)
            {
                var blendIn = clip;
                var blendOut = clips
                    .Where(c => Overlaps(c, blendIn))
                    .OrderBy(c => c.start)
                    .FirstOrDefault();

                if (blendOut != null)
                {
                    UpdateClipIntersection(blendOut, blendIn);
                }
            }
        }

        internal static void UpdateClipIntersection(TimelineClip blendOutClip, TimelineClip blendInClip)
        {
            if (!blendOutClip.SupportsBlending() || !blendInClip.SupportsBlending())
                return;

            if (blendInClip.end < blendOutClip.end)
                return;

            double duration = Math.Max(0, blendOutClip.start + blendOutClip.duration - blendInClip.start);
            duration = duration <= kMinOverlapTime ? 0 : duration;
            blendOutClip.blendOutDuration = duration;
            blendInClip.blendInDuration = duration;

            var blendInMode = blendInClip.blendInCurveMode;
            var blendOutMode = blendOutClip.blendOutCurveMode;

            if (blendInMode == TimelineClip.BlendCurveMode.Manual && blendOutMode == TimelineClip.BlendCurveMode.Auto)
            {
                blendOutClip.mixOutCurve = CurveEditUtility.CreateMatchingCurve(blendInClip.mixInCurve);
            }
            else if (blendInMode == TimelineClip.BlendCurveMode.Auto && blendOutMode == TimelineClip.BlendCurveMode.Manual)
            {
                blendInClip.mixInCurve = CurveEditUtility.CreateMatchingCurve(blendOutClip.mixOutCurve);
            }
            else if (blendInMode == TimelineClip.BlendCurveMode.Auto && blendOutMode == TimelineClip.BlendCurveMode.Auto)
            {
                blendInClip.mixInCurve = null; // resets to default curves
                blendOutClip.mixOutCurve = null;
            }
        }

        internal static void RecursiveSubtrackClone(TrackAsset source, TrackAsset duplicate, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, PlayableAsset assetOwner)
        {
            var subtracks = source.GetChildTracks();
            foreach (var sub in subtracks)
            {
                var newSub = TimelineHelpers.Clone(duplicate, sub, sourceTable, destTable, assetOwner);
                duplicate.AddChild(newSub);
                RecursiveSubtrackClone(sub, newSub, sourceTable, destTable, assetOwner);

                // Call the custom editor on Create
                var customEditor = CustomTimelineEditorCache.GetTrackEditor(newSub);
                try
                {
                    customEditor.OnCreate(newSub, sub);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                // registration has to happen AFTER recursion
                TimelineCreateUtilities.SaveAssetIntoObject(newSub, assetOwner);
                TimelineUndo.RegisterCreatedObjectUndo(newSub, "Duplicate");
            }
        }

        internal static TrackAsset Duplicate(this TrackAsset track, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable,
            TimelineAsset destinationTimeline = null)
        {
            if (track == null)
                return null;

            // if the destination is us, clear to avoid bad parenting (case 919421)
            if (destinationTimeline == track.timelineAsset)
                destinationTimeline = null;

            var timelineParent = track.parent as TimelineAsset;
            var trackParent = track.parent as TrackAsset;
            if (timelineParent == null && trackParent == null)
            {
                Debug.LogWarning("Cannot duplicate track because it is not parented to known type");
                return null;
            }

            // Determine who the final parent is. If we are pasting into another track, it's always the timeline.
            //  Otherwise it's the original parent
            PlayableAsset finalParent = destinationTimeline != null ? destinationTimeline : track.parent;

            // grab the list of tracks to generate a name from (923360) to get the list of names
            // no need to do this part recursively
            var finalTrackParent = finalParent as TrackAsset;
            var finalTimelineAsset = finalParent as TimelineAsset;
            var otherTracks = (finalTimelineAsset != null) ? finalTimelineAsset.trackObjects : finalTrackParent.subTracksObjects;

            // Important to create the new objects before pushing the original undo, or redo breaks the
            //  sequence
            var newTrack = TimelineHelpers.Clone(finalParent, track, sourceTable, destTable, finalParent);
            newTrack.name = TimelineCreateUtilities.GenerateUniqueActorName(otherTracks, newTrack.name);

            RecursiveSubtrackClone(track, newTrack, sourceTable, destTable, finalParent);
            TimelineCreateUtilities.SaveAssetIntoObject(newTrack, finalParent);
            TimelineUndo.RegisterCreatedObjectUndo(newTrack, "Duplicate");
            TimelineUndo.PushUndo(finalParent, "Duplicate");

            if (destinationTimeline != null) // other timeline
                destinationTimeline.AddTrackInternal(newTrack);
            else if (timelineParent != null) // this timeline, no parent
                ReparentTracks(new List<TrackAsset> { newTrack }, timelineParent, timelineParent.GetRootTracks().Last(), false);
            else // this timeline, with parent
                trackParent.AddChild(newTrack);

            // Call the custom editor. this check prevents the call when copying to the clipboard
            if (destinationTimeline == null || destinationTimeline == TimelineEditor.inspectedAsset)
            {
                var customEditor = CustomTimelineEditorCache.GetTrackEditor(newTrack);
                try
                {
                    customEditor.OnCreate(newTrack, track);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return newTrack;
        }

        // Reparents a list of tracks to a new parent
        //  the new parent cannot be null (has to be track asset or sequence)
        //  the insertAfter can be null (will not reorder)
        internal static bool ReparentTracks(List<TrackAsset> tracksToMove, PlayableAsset targetParent,
            TrackAsset insertMarker = null, bool insertBefore = false)
        {
            var targetParentTrack = targetParent as TrackAsset;
            var targetSequenceTrack = targetParent as TimelineAsset;

            if (tracksToMove == null || tracksToMove.Count == 0 || (targetParentTrack == null && targetSequenceTrack == null))
                return false;

            // invalid parent type on a track
            if (targetParentTrack != null && tracksToMove.Any(x => !TimelineCreateUtilities.ValidateParentTrack(targetParentTrack, x.GetType())))
                return false;

            // no valid tracks means this is simply a rearrangement
            var validTracks = tracksToMove.Where(x => x.parent != targetParent).ToList();
            if (insertMarker == null && !validTracks.Any())
                return false;

            var parents = validTracks.Select(x => x.parent).Where(x => x != null).Distinct().ToList();

            // push the current state of the tracks that will change
            foreach (var p in parents)
            {
                TimelineUndo.PushUndo(p, "Reparent");
            }
            foreach (var t in validTracks)
            {
                TimelineUndo.PushUndo(t, "Reparent");
            }
            TimelineUndo.PushUndo(targetParent, "Reparent");

            // need to reparent tracks first, before moving them.
            foreach (var t in validTracks)
            {
                if (t.parent != targetParent)
                {
                    TrackAsset toMoveParent = t.parent as TrackAsset;
                    TimelineAsset toMoveTimeline = t.parent as TimelineAsset;
                    if (toMoveTimeline != null)
                    {
                        toMoveTimeline.RemoveTrack(t);
                    }
                    else if (toMoveParent != null)
                    {
                        toMoveParent.RemoveSubTrack(t);
                    }

                    if (targetParentTrack != null)
                    {
                        targetParentTrack.AddChild(t);
                        targetParentTrack.SetCollapsed(false);
                    }
                    else
                    {
                        targetSequenceTrack.AddTrackInternal(t);
                    }
                }
            }


            if (insertMarker != null)
            {
                // re-ordering track. This is using internal APIs, so invalidation of the tracks must be done manually to avoid
                //  cache mismatches
                var children = targetParentTrack != null ? targetParentTrack.subTracksObjects : targetSequenceTrack.trackObjects;
                TimelineUtility.ReorderTracks(children, tracksToMove, insertMarker, insertBefore);
                if (targetParentTrack != null)
                    targetParentTrack.Invalidate();
                if (insertMarker.timelineAsset != null)
                    insertMarker.timelineAsset.Invalidate();
            }

            return true;
        }

        internal static IEnumerable<TrackAsset> FilterTracks(IEnumerable<TrackAsset> tracks)
        {
            var nTracks = tracks.Count();
            // Duplicate is recursive. If should not have parent and child in the list
            var hash = new HashSet<TrackAsset>(tracks);
            var take = new Dictionary<TrackAsset, bool>(nTracks);

            foreach (var track in tracks)
            {
                var parent = track.parent as TrackAsset;
                var foundParent = false;
                // go up the hierarchy
                while (parent != null && !foundParent)
                {
                    if (hash.Contains(parent))
                    {
                        foundParent = true;
                    }

                    parent = parent.parent as TrackAsset;
                }

                take[track] = !foundParent;
            }

            foreach (var track in tracks)
            {
                if (take[track])
                    yield return track;
            }
        }

        internal static bool IsVisibleRecursive(this TrackAsset track)
        {
            var t = track;
            while ((t = t.parent as TrackAsset) != null)
            {
                if (t.GetCollapsed())
                    return false;
            }

            return true;
        }

        internal static bool GetCollapsed(this TrackAsset track)
        {
            return TimelineWindowViewPrefs.IsTrackCollapsed(track);
        }

        internal static void SetCollapsed(this TrackAsset track, bool collapsed)
        {
            TimelineWindowViewPrefs.SetTrackCollapsed(track, collapsed);
        }

        internal static bool GetShowMarkers(this TrackAsset track)
        {
            return TimelineWindowViewPrefs.IsShowMarkers(track);
        }

        internal static void SetShowMarkers(this TrackAsset track, bool collapsed)
        {
            TimelineWindowViewPrefs.SetTrackShowMarkers(track, collapsed);
        }

        internal static bool GetShowInlineCurves(this TrackAsset track)
        {
            return TimelineWindowViewPrefs.GetShowInlineCurves(track);
        }

        internal static void SetShowInlineCurves(this TrackAsset track, bool inlineOn)
        {
            TimelineWindowViewPrefs.SetShowInlineCurves(track, inlineOn);
        }

        internal static bool ShouldShowInfiniteClipEditor(this TrackAsset track)
        {
            var animationTrack = track as AnimationTrack;
            if (animationTrack != null)
                return animationTrack.ShouldShowInfiniteClipEditor();

            return track.HasAnyAnimatableParameters();
        }

        internal static bool ShouldShowInfiniteClipEditor(this AnimationTrack track)
        {
            return track != null && !track.inClipMode && track.infiniteClip != null;
        }

        // Special method to remove a track that is in a broken state. i.e. the script won't load
        internal static bool RemoveBrokenTrack(PlayableAsset parent, ScriptableObject track)
        {
            var parentTrack = parent as TrackAsset;
            var parentTimeline = parent as TimelineAsset;

            if (parentTrack == null && parentTimeline == null)
                throw new ArgumentException("parent is not a valid parent type", "parent");

            // this object must be a Unity null, but not actually null;
            object trackAsObject = track;
            if (trackAsObject == null || track != null) // yes, this is correct
                throw new ArgumentException("track is not in a broken state");

            // this belongs to a parent track
            if (parentTrack != null)
            {
                int index = parentTrack.subTracksObjects.FindIndex(t => t.GetInstanceID() == track.GetInstanceID());
                if (index >= 0)
                {
                    TimelineUndo.PushUndo(parentTrack, "Remove Track");
                    parentTrack.subTracksObjects.RemoveAt(index);
                    parentTrack.Invalidate();
                    Undo.DestroyObjectImmediate(track);
                    return true;
                }
            }
            else if (parentTimeline != null)
            {
                int index = parentTimeline.trackObjects.FindIndex(t => t.GetInstanceID() == track.GetInstanceID());
                if (index >= 0)
                {
                    TimelineUndo.PushUndo(parentTimeline, "Remove Track");
                    parentTimeline.trackObjects.RemoveAt(index);
                    parentTimeline.Invalidate();
                    Undo.DestroyObjectImmediate(track);
                    return true;
                }
            }

            return false;
        }

        // Find the gap at the given time
        //  return true if there is a gap, false if there is an intersection
        // endGap will be Infinity if the gap has no end
        internal static bool GetGapAtTime(this TrackAsset track, double time, out double startGap, out double endGap)
        {
            startGap = 0;
            endGap = Double.PositiveInfinity;

            if (track == null || !track.GetClips().Any())
            {
                return false;
            }

            track.SortClips();
            var sortedByStartTime = track.clips;
            for (int i = 0; i < sortedByStartTime.Length; i++)
            {
                var clip = sortedByStartTime[i];

                // intersection
                if (time >= clip.start && time <= clip.end)
                {
                    endGap = time;
                    startGap = time;
                    return false;
                }

                if (clip.end < time)
                {
                    startGap = clip.end;
                }
                if (clip.start > time)
                {
                    endGap = clip.start;
                    break;
                }
            }

            if (endGap - startGap < TimelineClip.kMinDuration)
            {
                startGap = time;
                endGap = time;
                return false;
            }

            return true;
        }

        public static bool IsCompatibleWithClip(this TrackAsset track, TimelineClip clip)
        {
            if (track == null || clip == null || clip.asset == null)
                return false;

            return TypeUtility.GetPlayableAssetsHandledByTrack(track.GetType()).Contains(clip.asset.GetType());
        }

        // Get a flattened list of all child tracks
        public static void GetFlattenedChildTracks(this TrackAsset asset, List<TrackAsset> list)
        {
            if (asset == null || list == null)
                return;

            foreach (var track in asset.GetChildTracks())
            {
                list.Add(track);
                GetFlattenedChildTracks(track, list);
            }
        }

        public static IEnumerable<TrackAsset> GetFlattenedChildTracks(this TrackAsset asset)
        {
            if (asset == null || !asset.GetChildTracks().Any())
                return Enumerable.Empty<TrackAsset>();

            var flattenedChildTracks = new List<TrackAsset>();
            GetFlattenedChildTracks(asset, flattenedChildTracks);
            return flattenedChildTracks;
        }
    }
}
