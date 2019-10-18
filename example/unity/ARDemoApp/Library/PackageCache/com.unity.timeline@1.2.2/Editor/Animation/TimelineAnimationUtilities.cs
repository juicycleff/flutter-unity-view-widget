using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngineInternal;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class TimelineAnimationUtilities
    {
        public enum OffsetEditMode
        {
            None = -1,
            Translation = 0,
            Rotation = 1
        }

        public static bool ValidateOffsetAvailabitity(PlayableDirector director, Animator animator)
        {
            if (director == null || animator == null)
                return false;

            return true;
        }

        public static TimelineClip GetPreviousClip(TimelineClip clip)
        {
            TimelineClip previousClip = null;
            foreach (var c in clip.parentTrack.clips)
            {
                if (c.start < clip.start && (previousClip == null || c.start >= previousClip.start))
                    previousClip = c;
            }
            return previousClip;
        }

        public static TimelineClip GetNextClip(TimelineClip clip)
        {
            return clip.parentTrack.clips.Where(c => c.start > clip.start).OrderBy(c => c.start).FirstOrDefault();
        }

        public struct RigidTransform
        {
            public Vector3 position;
            public Quaternion rotation;

            public static RigidTransform Compose(Vector3 pos, Quaternion rot)
            {
                RigidTransform ret;
                ret.position = pos;
                ret.rotation = rot;
                return ret;
            }

            public static RigidTransform Mul(RigidTransform a, RigidTransform b)
            {
                RigidTransform ret;
                ret.rotation = a.rotation * b.rotation;
                ret.position = a.position + a.rotation * b.position;
                return ret;
            }

            public static RigidTransform Inverse(RigidTransform a)
            {
                RigidTransform ret;
                ret.rotation = Quaternion.Inverse(a.rotation);
                ret.position = ret.rotation * (-a.position);
                return ret;
            }

            public static RigidTransform identity
            {
                get { return Compose(Vector3.zero, Quaternion.identity); }
            }
        }


        private static Matrix4x4 GetTrackMatrix(Transform transform, AnimationTrack track)
        {
            Matrix4x4 trackMatrix = Matrix4x4.TRS(track.position, track.rotation, Vector3.one);

            // in scene off mode, the track offsets are set to the preview position which is stored in the track
            if (track.trackOffset == TrackOffset.ApplySceneOffsets)
            {
                trackMatrix = Matrix4x4.TRS(track.sceneOffsetPosition, Quaternion.Euler(track.sceneOffsetRotation), Vector3.one);
            }

            // put the parent transform on to the track matrix
            if (transform.parent != null)
            {
                trackMatrix = transform.parent.localToWorldMatrix * trackMatrix;
            }

            return trackMatrix;
        }

        // Given a world space position and rotation, updates the clip offsets to match that
        public static RigidTransform UpdateClipOffsets(AnimationPlayableAsset asset, AnimationTrack track, Transform transform, Vector3 globalPosition, Quaternion globalRotation)
        {
            Matrix4x4 worldToLocal = transform.worldToLocalMatrix;
            Matrix4x4 clipMatrix = Matrix4x4.TRS(asset.position, asset.rotation, Vector3.one);
            Matrix4x4 trackMatrix = GetTrackMatrix(transform, track);


            // Use the transform to find the proper goal matrix with scale taken into account
            var oldPos = transform.position;
            var oldRot = transform.rotation;
            transform.position = globalPosition;
            transform.rotation = globalRotation;
            Matrix4x4 goal = transform.localToWorldMatrix;
            transform.position = oldPos;
            transform.rotation = oldRot;

            // compute the new clip matrix.
            Matrix4x4 newClip = trackMatrix.inverse * goal * worldToLocal * trackMatrix * clipMatrix;
            return RigidTransform.Compose(newClip.GetColumn(3), MathUtils.QuaternionFromMatrix(newClip));
        }

        public static RigidTransform GetTrackOffsets(AnimationTrack track, Transform transform)
        {
            Vector3 position = track.position;
            Quaternion rotation = track.rotation;
            if (transform != null && transform.parent != null)
            {
                position = transform.parent.TransformPoint(position);
                rotation = transform.parent.rotation * rotation;
                MathUtils.QuaternionNormalize(ref rotation);
            }

            return RigidTransform.Compose(position, rotation);
        }

        public static void UpdateTrackOffset(AnimationTrack track, Transform transform, RigidTransform offsets)
        {
            if (transform != null && transform.parent != null)
            {
                offsets.position = transform.parent.InverseTransformPoint(offsets.position);
                offsets.rotation = Quaternion.Inverse(transform.parent.rotation) * offsets.rotation;
                MathUtils.QuaternionNormalize(ref offsets.rotation);
            }

            track.position = offsets.position;
            track.eulerAngles = AnimationUtility.GetClosestEuler(offsets.rotation, track.eulerAngles, RotationOrder.OrderZXY);
            track.UpdateClipOffsets();
        }

        static MatchTargetFields GetMatchFields(TimelineClip clip)
        {
            var track = clip.parentTrack as AnimationTrack;
            if (track == null)
                return MatchTargetFieldConstants.None;

            var asset = clip.asset as AnimationPlayableAsset;
            var fields = track.matchTargetFields;
            if (asset != null && !asset.useTrackMatchFields)
                fields = asset.matchTargetFields;
            return fields;
        }

        static void WriteMatchFields(AnimationPlayableAsset asset, RigidTransform result, MatchTargetFields fields)
        {
            Vector3 position = asset.position;

            position.x = fields.HasAny(MatchTargetFields.PositionX) ? result.position.x : position.x;
            position.y = fields.HasAny(MatchTargetFields.PositionY) ? result.position.y : position.y;
            position.z = fields.HasAny(MatchTargetFields.PositionZ) ? result.position.z : position.z;

            asset.position = position;

            // check first to avoid unnecessary conversion errors
            if (fields.HasAny(MatchTargetFieldConstants.Rotation))
            {
                Vector3 eulers = asset.eulerAngles;
                Vector3 resultEulers = result.rotation.eulerAngles;

                eulers.x = fields.HasAny(MatchTargetFields.RotationX) ? resultEulers.x : eulers.x;
                eulers.y = fields.HasAny(MatchTargetFields.RotationY) ? resultEulers.y : eulers.y;
                eulers.z = fields.HasAny(MatchTargetFields.RotationZ) ? resultEulers.z : eulers.z;

                asset.eulerAngles = AnimationUtility.GetClosestEuler(Quaternion.Euler(eulers), asset.eulerAngles, RotationOrder.OrderZXY);
            }
        }

        public static void MatchPrevious(TimelineClip currentClip, Transform matchPoint, PlayableDirector director)
        {
            const double timeEpsilon = 0.00001;
            MatchTargetFields matchFields = GetMatchFields(currentClip);
            if (matchFields == MatchTargetFieldConstants.None || matchPoint == null)
                return;

            double cachedTime = director.time;

            // finds previous clip
            TimelineClip previousClip = GetPreviousClip(currentClip);
            if (previousClip == null || currentClip == previousClip)
                return;

            // make sure the transform is properly updated before modifying the graph
            director.Evaluate();

            var parentTrack = currentClip.parentTrack as AnimationTrack;

            var blendIn = currentClip.blendInDuration;
            currentClip.blendInDuration = 0;
            var blendOut = previousClip.blendOutDuration;
            previousClip.blendOutDuration = 0;

            //evaluate previous without current
            parentTrack.RemoveClip(currentClip);
            director.RebuildGraph();
            double previousEndTime = currentClip.start > previousClip.end ? previousClip.end : currentClip.start;
            director.time = previousEndTime - timeEpsilon;
            director.Evaluate(); // add port to evaluate only track

            var targetPosition = matchPoint.position;
            var targetRotation = matchPoint.rotation;

            // evaluate current without previous
            parentTrack.AddClip(currentClip);
            parentTrack.RemoveClip(previousClip);
            director.RebuildGraph();
            director.time = currentClip.start + timeEpsilon;
            director.Evaluate();

            //////////////////////////////////////////////////////////////////////
            //compute offsets

            var animationPlayable = currentClip.asset as AnimationPlayableAsset;
            var match = UpdateClipOffsets(animationPlayable, parentTrack, matchPoint, targetPosition, targetRotation);
            WriteMatchFields(animationPlayable, match, matchFields);

            //////////////////////////////////////////////////////////////////////

            currentClip.blendInDuration = blendIn;
            previousClip.blendOutDuration = blendOut;

            parentTrack.AddClip(previousClip);
            director.RebuildGraph();
            director.time = cachedTime;
            director.Evaluate();
        }

        public static void MatchNext(TimelineClip currentClip, Transform matchPoint, PlayableDirector director)
        {
            const double timeEpsilon = 0.00001;
            MatchTargetFields matchFields = GetMatchFields(currentClip);
            if (matchFields == MatchTargetFieldConstants.None || matchPoint == null)
                return;

            double cachedTime = director.time;

            // finds next clip
            TimelineClip nextClip = GetNextClip(currentClip);
            if (nextClip == null || currentClip == nextClip)
                return;

            // make sure the transform is properly updated before modifying the graph
            director.Evaluate();

            var parentTrack = currentClip.parentTrack as AnimationTrack;

            var blendOut = currentClip.blendOutDuration;
            var blendIn = nextClip.blendInDuration;
            currentClip.blendOutDuration = 0;
            nextClip.blendInDuration = 0;

            //evaluate previous without current
            parentTrack.RemoveClip(currentClip);
            director.RebuildGraph();
            director.time = nextClip.start + timeEpsilon;
            director.Evaluate(); // add port to evaluate only track

            var targetPosition = matchPoint.position;
            var targetRotation = matchPoint.rotation;

            // evaluate current without next
            parentTrack.AddClip(currentClip);
            parentTrack.RemoveClip(nextClip);
            director.RebuildGraph();
            director.time = Math.Min(nextClip.start, currentClip.end - timeEpsilon);
            director.Evaluate();

            //////////////////////////////////////////////////////////////////////
            //compute offsets

            var animationPlayable = currentClip.asset as AnimationPlayableAsset;
            var match = UpdateClipOffsets(animationPlayable, parentTrack, matchPoint, targetPosition, targetRotation);
            WriteMatchFields(animationPlayable, match, matchFields);

            //////////////////////////////////////////////////////////////////////

            currentClip.blendOutDuration = blendOut;
            nextClip.blendInDuration = blendIn;

            parentTrack.AddClip(nextClip);
            director.RebuildGraph();
            director.time = cachedTime;
            director.Evaluate();
        }

        public static TimelineWindowTimeControl CreateTimeController(WindowState state, TimelineClip clip)
        {
            var animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            var timeController = ScriptableObject.CreateInstance<TimelineWindowTimeControl>();
            timeController.Init(animationWindow.state, clip);
            return timeController;
        }

        public static TimelineWindowTimeControl CreateTimeController(WindowState state, TimelineWindowTimeControl.ClipData clipData)
        {
            var animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            var timeController = ScriptableObject.CreateInstance<TimelineWindowTimeControl>();
            timeController.Init(animationWindow.state, clipData);
            return timeController;
        }

        public static void EditAnimationClipWithTimeController(AnimationClip animationClip, TimelineWindowTimeControl timeController, Object sourceObject)
        {
            var animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            animationWindow.EditSequencerClip(animationClip, sourceObject, timeController);
        }

        public static void UnlinkAnimationWindowFromTracks(IEnumerable<TrackAsset> tracks)
        {
            var clips = new List<AnimationClip>();
            foreach (var track in tracks)
            {
                var animationTrack = track as AnimationTrack;
                if (animationTrack != null && animationTrack.infiniteClip != null)
                    clips.Add(animationTrack.infiniteClip);

                GetAnimationClips(track.GetClips(), clips);
            }
            UnlinkAnimationWindowFromAnimationClips(clips);
        }

        public static void UnlinkAnimationWindowFromClips(IEnumerable<TimelineClip> timelineClips)
        {
            var clips = new List<AnimationClip>();
            GetAnimationClips(timelineClips, clips);
            UnlinkAnimationWindowFromAnimationClips(clips);
        }

        public static void UnlinkAnimationWindowFromAnimationClips(ICollection<AnimationClip> clips)
        {
            if (clips.Count == 0)
                return;

            UnityEngine.Object[] windows = Resources.FindObjectsOfTypeAll(typeof(AnimationWindow));
            foreach (var animWindow in windows.OfType<AnimationWindow>())
            {
                if (animWindow != null && animWindow.state != null && animWindow.state.linkedWithSequencer && clips.Contains(animWindow.state.activeAnimationClip))
                    animWindow.UnlinkSequencer();
            }
        }

        public static void UnlinkAnimationWindow()
        {
            UnityEngine.Object[] windows = Resources.FindObjectsOfTypeAll(typeof(AnimationWindow));
            foreach (var animWindow in windows.OfType<AnimationWindow>())
            {
                if (animWindow != null && animWindow.state != null && animWindow.state.linkedWithSequencer)
                    animWindow.UnlinkSequencer();
            }
        }

        private static void GetAnimationClips(IEnumerable<TimelineClip> timelineClips, List<AnimationClip> clips)
        {
            foreach (var timelineClip in timelineClips)
            {
                if (timelineClip.curves != null)
                    clips.Add(timelineClip.curves);
                AnimationPlayableAsset apa = timelineClip.asset as AnimationPlayableAsset;
                if (apa != null && apa.clip != null)
                    clips.Add(apa.clip);
            }
        }

        public static int GetAnimationWindowCurrentFrame()
        {
            var animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            if (animationWindow)
                return animationWindow.state.currentFrame;
            return -1;
        }

        public static void SetAnimationWindowCurrentFrame(int frame)
        {
            var animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            if (animationWindow)
                animationWindow.state.currentFrame = frame;
        }

        public static void ConstrainCurveToBooleanValues(AnimationCurve curve)
        {
            // Clamp the values first
            var keys = curve.keys;
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                key.value = key.value < 0.5f ? 0.0f : 1.0f;
                keys[i] = key;
            }
            curve.keys = keys;

            // Update the tangents once all the values are clamped
            for (var i = 0; i < curve.length; i++)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
            }
        }

        public static void ConstrainCurveToRange(AnimationCurve curve, float minValue, float maxValue)
        {
            var keys = curve.keys;
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                key.value = Mathf.Clamp(key.value, minValue, maxValue);
                keys[i] = key;
            }
            curve.keys = keys;
        }

        public static bool IsAnimationClip(TimelineClip clip)
        {
            return clip != null && (clip.asset as AnimationPlayableAsset) != null;
        }
    }
}
