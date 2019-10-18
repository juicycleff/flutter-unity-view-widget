using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class AnimationTrackRecorder
    {
        public static readonly string kRecordClipDefaultName = L10n.Tr("Recorded");

        AnimationClip m_TargetClip;
        int m_CurveCount = 0;

        double m_ClipTime;
        bool m_needRebuildRects;

        bool m_TrackHasPreviewComponents;

        public TimelineClip recordClip { get; private set; }

        public void PrepareForRecord(WindowState state)
        {
            m_CurveCount = 0;
            m_TargetClip = null;
            m_TrackHasPreviewComponents = false;
        }

        public AnimationClip PrepareTrack(TrackAsset track, WindowState state, GameObject gameObject, out double startTime)
        {
            AnimationClip animationClip = null;

            // if we are not in clip mode, we simply use the track clip
            var animationTrack = (AnimationTrack)track;

            // ignore recording if we are in Legacy auto mode
            startTime = -1;
            var parentTrack = TimelineUtility.GetSceneReferenceTrack(track) as AnimationTrack;
            if (parentTrack != null && parentTrack.trackOffset == TrackOffset.Auto)
                return null;

            if (!animationTrack.inClipMode)
            {
                var trackClip = animationTrack.GetOrCreateClip();
                startTime = trackClip.frameRate * state.editSequence.time;

                // Make the first key be at time 0 of the clip
                if (trackClip.empty)
                {
                    animationTrack.infiniteClipTimeOffset = 0; // state.time;
                    animationTrack.infiniteClipPreExtrapolation = TimelineClip.ClipExtrapolation.Hold;
                    animationTrack.infiniteClipPostExtrapolation = TimelineClip.ClipExtrapolation.Hold;
                }

                animationClip = trackClip;
            }
            else
            {
                TimelineClip activeClip = null;

                // if it fails, but returns no clip, we can add one.
                if (!track.FindRecordingClipAtTime(state.editSequence.time, out activeClip) && activeClip != null)
                {
                    return null;
                }

                if (activeClip == null)
                {
                    activeClip = AddRecordableClip(track, state, state.editSequence.time);
                }

                var clip = activeClip.animationClip;

                // flags this as the clip being recorded for the track
                var clipTime = state.editSequence.time - activeClip.start;

                // if we are in the past
                if (clipTime < 0)
                {
                    Undo.RegisterCompleteObjectUndo(clip, "Record Key");
                    TimelineUndo.PushUndo(track, "Prepend Key");
                    ShiftAnimationClip(clip, (float)-clipTime);
                    activeClip.start = state.editSequence.time;
                    activeClip.duration += -clipTime;
                    clipTime = 0;
                }

                m_ClipTime = clipTime;
                recordClip = activeClip;
                startTime = TimeUtility.ToFrames(recordClip.ToLocalTimeUnbound(state.editSequence.time), clip.frameRate);
                m_needRebuildRects = clip.empty;

                animationClip = clip;
            }

            m_TargetClip = animationClip;
            m_CurveCount = GetCurveCount(animationClip);
            m_TrackHasPreviewComponents = animationTrack.hasPreviewComponents;

            return animationClip;
        }

        static int GetCurveCount(AnimationClip animationClip)
        {
            int count = 0;
            if (animationClip != null)
            {
                var clipCache = AnimationClipCurveCache.Instance.GetCurveInfo(animationClip);
                count = clipCache.curves.Length + clipCache.objectCurves.Count;
            }

            return count;
        }

        public void FinializeTrack(TrackAsset track, WindowState state)
        {
            // make sure we dirty the clip if we are in non clip mode
            var animTrack = track as AnimationTrack;
            if (!animTrack.inClipMode)
            {
                EditorUtility.SetDirty(animTrack.GetOrCreateClip());
            }

            // in clip mode we need to do some extra work
            if (recordClip != null)
            {
                // stretch the clip out to meet the new recording time
                if (m_ClipTime > recordClip.duration)
                {
                    TimelineUndo.PushUndo(track, "Add Key");
                    recordClip.duration = m_ClipTime;
                }

                track.CalculateExtrapolationTimes();
            }

            recordClip = null;
            m_ClipTime = 0;
            if (m_needRebuildRects)
            {
                state.CalculateRowRects();
                m_needRebuildRects = false;
            }
        }

        public void FinalizeRecording(WindowState state)
        {
            // rebuild the graph if we add/remove a clip. Rebuild the graph with an evaluation immediately
            // so previews and scene position is maintained.
            if (m_CurveCount != GetCurveCount(m_TargetClip))
            {
                state.rebuildGraph = true;
                state.GetWindow().RebuildGraphIfNecessary(true);
            }
            else if (m_TrackHasPreviewComponents)
            {
                // Track with preview components potentially has modifications impacting other properties that need
                // to be refreshed before inspector or scene view to not interfere with manipulation.
                state.EvaluateImmediate();
            }
        }

        // For a given track asset get a unique clip name
        public static string GetUniqueRecordedClipName(Object owner, string name)
        {
            // first attempt -- uniquely named in file
            var path = AssetDatabase.GetAssetPath(owner);
            if (!string.IsNullOrEmpty(path))
            {
                var names = AssetDatabase.LoadAllAssetsAtPath(path).Where(x => x != null).Select(x => x.name);
                return ObjectNames.GetUniqueName(names.ToArray(), name);
            }

            TrackAsset asset = owner as TrackAsset;
            if (asset == null || asset.clips.Length == 0)
                return name;

            // final attempt - uniquely named in track
            return ObjectNames.GetUniqueName(asset.clips.Select(x => x.displayName).ToArray(), name);
        }

        // Given an appropriate parent track, create a recordable clip
        public static TimelineClip AddRecordableClip(TrackAsset parentTrack, WindowState state, double atTime)
        {
            var sequenceAsset = state.editSequence.asset;
            if (sequenceAsset == null)
            {
                Debug.LogError("Parent Track needs to be bound to an asset to add a recordable");
                return null;
            }

            var animTrack = parentTrack as AnimationTrack;
            if (animTrack == null)
            {
                Debug.LogError("Recordable clips are only valid on Animation Tracks");
                return null;
            }

            var newClip = animTrack.CreateRecordableClip(GetUniqueRecordedClipName(parentTrack, kRecordClipDefaultName));
            if (newClip == null)
            {
                Debug.LogError("Could not create a recordable clip");
                return null;
            }

            newClip.mixInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            newClip.mixOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

            newClip.preExtrapolationMode = TimelineClip.ClipExtrapolation.Hold;
            newClip.postExtrapolationMode = TimelineClip.ClipExtrapolation.Hold;

            double startTime = 0;
            double endTime = 0;

            GetAddedRecordingClipRange(animTrack, state, atTime, out startTime, out endTime);

            newClip.start = startTime;
            newClip.duration = endTime - startTime;

            state.Refresh();

            return newClip;
        }

        // get the start and end times of what an added recording clip at a given time would be
        internal static void GetAddedRecordingClipRange(TrackAsset track, WindowState state, double atTime, out double start, out double end)
        {
            // size to make the clip in pixels. Reasonably big so that both handles are easily manipulated,
            // and the full title is normally visible
            double defaultDuration = state.PixelDeltaToDeltaTime(100);

            start = atTime;
            end = atTime + defaultDuration;

            double gapStart = 0;
            double gapEnd = 0;

            // no gap, pick are reasonable amount
            if (!track.GetGapAtTime(atTime, out gapStart, out gapEnd))
            {
                start = atTime;
                return;
            }

            if (!double.IsInfinity(gapEnd))
                end = gapEnd;

            start = state.SnapToFrameIfRequired(start);
            end = state.SnapToFrameIfRequired(end);
        }

        // Given a clip, shifts the keys in that clip by the given amount.
        internal static void ShiftAnimationClip(AnimationClip clip, float amount)
        {
            if (clip == null)
                return;

            var curveBindings = AnimationUtility.GetCurveBindings(clip);
            var objectCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach (var binding in curveBindings)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                curve.keys = ShiftKeys(curve.keys, amount);
                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }

            foreach (var binding in objectCurveBindings)
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                keyframes = ShiftObjectKeys(keyframes, amount);
                AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
            }

            EditorUtility.SetDirty(clip);
        }

        // shift all the keys over by the given time, stretching the time 0 key
        static Keyframe[] ShiftKeys(Keyframe[] keys, float time)
        {
            if (keys == null || keys.Length == 0 || time == 0)
                return keys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time += time;
            }

            return keys;
        }

        // Shift object keys over by the appropriate amount
        static ObjectReferenceKeyframe[] ShiftObjectKeys(ObjectReferenceKeyframe[] keys, float time)
        {
            if (keys == null || keys.Length == 0 || time == 0)
                return keys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time += time;
            }

            return keys;
        }
    }
}
