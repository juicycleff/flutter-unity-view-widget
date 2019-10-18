using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    // Handles Undo animated properties on Monobehaviours to create track clips
    static partial class TimelineRecording
    {
        static readonly List<PropertyModification> s_TempPropertyModifications = new List<PropertyModification>(6);

        internal static UndoPropertyModification[] ProcessUndoModification(UndoPropertyModification[] modifications, WindowState state)
        {
            if (HasAnyPlayableAssetModifications(modifications))
                return ProcessPlayableAssetModification(modifications, state);
            return ProcessMonoBehaviourModification(modifications, state);
        }

        static UnityEngine.Object GetTarget(UndoPropertyModification undo)
        {
            if (undo.currentValue != null)
                return undo.currentValue.target;
            if (undo.previousValue != null)
                return undo.previousValue.target;
            return null;
        }

        // Gets the appropriate track for a given game object
        static TrackAsset GetTrackForGameObject(GameObject gameObject, WindowState state)
        {
            if (gameObject == null)
                return null;

            var director = state.editSequence.director;
            if (director == null)
                return null;

            var level = int.MaxValue;

            TrackAsset result = null;

            // search the output tracks
            var outputTracks = state.editSequence.asset.flattenedTracks;
            foreach (var track in outputTracks)
            {
                if (track.GetType() != typeof(AnimationTrack))
                    continue;
                if (!state.IsTrackRecordable(track))
                    continue;

                var obj = TimelineUtility.GetSceneGameObject(director, track);
                if (obj != null)
                {
                    // checks if the effected gameobject is our child
                    var childLevel = GetChildLevel(obj, gameObject);
                    if (childLevel != -1 && childLevel < level)
                    {
                        result = track;
                        level = childLevel;
                    }
                }
            }

            // the resulting track is not armed. checking here avoids accidentally recording objects with their own
            // tracks
            if (result && !state.IsTrackRecordable(result))
            {
                result = null;
            }

            return result;
        }

        // Gets the track this property would record to.
        // Returns null if there is a track, but it's not currently active for recording
        public static TrackAsset GetRecordingTrack(SerializedProperty property, WindowState state)
        {
            var serializedObject = property.serializedObject;
            var component = serializedObject.targetObject as Component;
            if (component == null)
                return null;

            var gameObject = component.gameObject;
            return GetTrackForGameObject(gameObject, state);
        }

        // Given a serialized property, gathers all animatable properties
        static void GatherModifications(SerializedProperty property, List<PropertyModification> modifications)
        {
            // handles child properties (Vector3 is 3 recordable properties)
            if (property.hasChildren)
            {
                var iter = property.Copy();
                var end = property.GetEndProperty(false);

                // recurse over all children properties
                while (iter.Next(true) && !SerializedProperty.EqualContents(iter, end))
                {
                    GatherModifications(iter, modifications);
                }
            }

            var isObject = property.propertyType == SerializedPropertyType.ObjectReference;
            var isFloat = property.propertyType == SerializedPropertyType.Float ||
                property.propertyType == SerializedPropertyType.Boolean ||
                property.propertyType == SerializedPropertyType.Integer;

            if (isObject || isFloat)
            {
                var serializedObject = property.serializedObject;
                var modification = new PropertyModification();

                modification.target = serializedObject.targetObject;
                modification.propertyPath = property.propertyPath;
                if (isObject)
                {
                    modification.value = string.Empty;
                    modification.objectReference = property.objectReferenceValue;
                }
                else
                {
                    modification.value = TimelineUtility.PropertyToString(property);
                }

                // Path for monobehaviour based - better to grab the component to get the curvebinding to allow validation
                if (serializedObject.targetObject is Component)
                {
                    EditorCurveBinding temp;
                    var go = ((Component)serializedObject.targetObject).gameObject;
                    if (AnimationUtility.PropertyModificationToEditorCurveBinding(modification, go, out temp) != null)
                    {
                        modifications.Add(modification);
                    }
                }
                else
                {
                    modifications.Add(modification);
                }
            }
        }

        public static bool CanRecord(SerializedProperty property, WindowState state)
        {
            if (IsPlayableAssetProperty(property))
                return AnimatedParameterUtility.IsTypeAnimatable(property.propertyType);

            if (GetRecordingTrack(property, state) == null)
                return false;

            s_TempPropertyModifications.Clear();
            GatherModifications(property, s_TempPropertyModifications);
            return s_TempPropertyModifications.Any();
        }

        public static void AddKey(SerializedProperty prop, WindowState state)
        {
            s_TempPropertyModifications.Clear();
            GatherModifications(prop, s_TempPropertyModifications);
            if (s_TempPropertyModifications.Any())
            {
                AddKey(s_TempPropertyModifications, state);
            }
        }

        public static void AddKey(IEnumerable<PropertyModification> modifications, WindowState state)
        {
            var undos = modifications.Select(PropertyModificationToUndoPropertyModification).ToArray();
            ProcessUndoModification(undos, state);
        }

        static UndoPropertyModification PropertyModificationToUndoPropertyModification(PropertyModification prop)
        {
            return new UndoPropertyModification
            {
                previousValue = prop,
                currentValue = new PropertyModification
                {
                    objectReference = prop.objectReference,
                    propertyPath = prop.propertyPath,
                    target = prop.target,
                    value = prop.value
                },
                keepPrefabOverride = true
            };
        }

        // Given an animation track, return the clip that we are currently recording to
        static AnimationClip GetRecordingClip(TrackAsset asset, WindowState state, out double startTime, out double timeScale)
        {
            startTime = 0;
            timeScale = 1;

            TimelineClip displayBackground = null;
            asset.FindRecordingClipAtTime(state.editSequence.time, out displayBackground);
            var animClip = asset.FindRecordingAnimationClipAtTime(state.editSequence.time);

            if (displayBackground != null)
            {
                startTime =  displayBackground.start;
                timeScale =  displayBackground.timeScale;
            }

            return animClip;
        }

        // Helper that finds the animation clip we are recording and the relative time to that clip
        static bool GetClipAndRelativeTime(UnityEngine.Object target, WindowState state,
            out AnimationClip outClip, out double keyTime, out bool keyInRange)
        {
            const float floatToDoubleError = 0.00001f;
            outClip = null;
            keyTime = 0;
            keyInRange = false;

            double startTime = 0;
            double timeScale = 1;
            AnimationClip clip = null;

            IPlayableAsset playableAsset = target as IPlayableAsset;
            Component component = target as Component;

            // Handle recordable playable assets
            if (playableAsset != null)
            {
                var curvesOwner = AnimatedParameterUtility.ToCurvesOwner(playableAsset, state.editSequence.asset);
                if (curvesOwner != null && state.IsTrackRecordable(curvesOwner.targetTrack))
                {
                    if (curvesOwner.curves == null)
                        curvesOwner.CreateCurves(curvesOwner.GetUniqueRecordedClipName());

                    clip = curvesOwner.curves;

                    var timelineClip = curvesOwner as TimelineClip;
                    if (timelineClip != null)
                    {
                        startTime = timelineClip.start;
                        timeScale = timelineClip.timeScale;
                    }
                }
            }
            // Handle recording components, including infinite clip
            else if (component != null)
            {
                var asset = GetTrackForGameObject(component.gameObject, state);
                if (asset != null)
                {
                    clip = GetRecordingClip(asset, state, out startTime, out timeScale);
                }
            }

            if (clip == null)
                return false;

            keyTime = (state.editSequence.time - startTime) * timeScale;
            outClip = clip;
            keyInRange = keyTime >= 0 && keyTime <= (clip.length * timeScale + floatToDoubleError);

            return true;
        }

        public static bool HasCurve(IEnumerable<PropertyModification> modifications, UnityEngine.Object target,
            WindowState state)
        {
            return GetKeyTimes(target, modifications, state).Any();
        }

        public static bool HasKey(IEnumerable<PropertyModification> modifications, UnityEngine.Object target,
            WindowState state)
        {
            AnimationClip clip;
            double keyTime;
            bool inRange;
            if (!GetClipAndRelativeTime(target, state, out clip, out keyTime, out inRange))
                return false;

            return GetKeyTimes(target, modifications, state).Any(t => (CurveEditUtility.KeyCompare((float)state.editSequence.time, (float)t, clip.frameRate) == 0));
        }

        // Checks if a key already exists for this property
        static bool HasBinding(UnityEngine.Object target, PropertyModification modification, AnimationClip clip, out EditorCurveBinding binding)
        {
            var component = target as Component;
            var playableAsset = target as IPlayableAsset;

            if (component != null)
            {
                var type = AnimationUtility.PropertyModificationToEditorCurveBinding(modification, component.gameObject, out binding);
                binding = RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(binding, clip);
                return type != null;
            }

            if (playableAsset != null)
            {
                binding = EditorCurveBinding.FloatCurve(string.Empty, target.GetType(),
                    AnimatedParameterUtility.GetAnimatedParameterBindingName(target, modification.propertyPath));
            }
            else
            {
                binding = new EditorCurveBinding();
                return false;
            }

            return true;
        }

        public static void RemoveKey(UnityEngine.Object target, IEnumerable<PropertyModification> modifications,
            WindowState state)
        {
            AnimationClip clip;
            double keyTime;
            bool inRange;
            if (!GetClipAndRelativeTime(target, state, out clip, out keyTime, out inRange) || !inRange)
                return;
            var refreshPreview = false;
            TimelineUndo.PushUndo(clip, "Remove Key");
            foreach (var mod in modifications)
            {
                EditorCurveBinding temp;
                if (HasBinding(target, mod, clip, out temp))
                {
                    if (temp.isPPtrCurve)
                    {
                        CurveEditUtility.RemoveObjectKey(clip, temp, keyTime);
                        if (CurveEditUtility.GetObjectKeyCount(clip, temp) == 0)
                        {
                            refreshPreview = true;
                        }
                    }
                    else
                    {
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, temp);
                        if (curve != null)
                        {
                            CurveEditUtility.RemoveKeyFrameFromCurve(curve, (float)keyTime, clip.frameRate);
                            AnimationUtility.SetEditorCurve(clip, temp, curve);
                            if (curve.length == 0)
                            {
                                AnimationUtility.SetEditorCurve(clip, temp, null);
                                refreshPreview = true;
                            }
                        }
                    }
                }
            }

            if (refreshPreview)
            {
                state.ResetPreviewMode();
            }
        }

        static HashSet<double> GetKeyTimes(UnityEngine.Object target, IEnumerable<PropertyModification> modifications, WindowState state)
        {
            var keyTimes = new HashSet<double>();

            AnimationClip animationClip;
            double keyTime;
            bool inRange;
            GetClipAndRelativeTime(target, state, out animationClip, out keyTime, out inRange);
            if (animationClip == null)
                return keyTimes;

            var component = target as Component;
            var playableAsset = target as IPlayableAsset;
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(animationClip);

            TimelineClip clip = null;
            if (component != null)
            {
                GetTrackForGameObject(component.gameObject, state).FindRecordingClipAtTime(state.editSequence.time, out clip);
            }
            else if (playableAsset != null)
            {
                clip = FindClipWithAsset(state.editSequence.asset, playableAsset);
            }

            foreach (var mod in modifications)
            {
                EditorCurveBinding temp;
                if (HasBinding(target, mod, animationClip, out temp))
                {
                    IEnumerable<double> keys = new HashSet<double>();
                    if (temp.isPPtrCurve)
                    {
                        var curve = info.GetObjectCurveForBinding(temp);
                        if (curve != null)
                        {
                            keys = curve.Select(x => (double)x.time);
                        }
                    }
                    else
                    {
                        var curve = info.GetCurveForBinding(temp);
                        if (curve != null)
                        {
                            keys = curve.keys.Select(x => (double)x.time);
                        }
                    }

                    // Transform the times in to 'global' space using the clip
                    if (clip != null)
                    {
                        foreach (var k in keys)
                        {
                            var time = clip.FromLocalTimeUnbound(k);
                            const double eps = 1e-5;
                            if (time >= clip.start - eps && time <= clip.end + eps)
                            {
                                keyTimes.Add(time);
                            }
                        }
                    }
                    // infinite clip mode, global == local space
                    else
                    {
                        keyTimes.UnionWith(keys);
                    }
                }
            }

            return keyTimes;
        }

        public static void NextKey(UnityEngine.Object target, IEnumerable<PropertyModification> modifications, WindowState state)
        {
            const double eps = 1e-5;
            var keyTimes = GetKeyTimes(target, modifications, state);
            if (keyTimes.Count == 0)
                return;
            var nextKeys = keyTimes.Where(x => x > state.editSequence.time + eps);
            if (nextKeys.Any())
            {
                state.editSequence.time = nextKeys.Min();
            }
        }

        public static void PrevKey(UnityEngine.Object target, IEnumerable<PropertyModification> modifications, WindowState state)
        {
            const double eps = 1e-5;
            var keyTimes = GetKeyTimes(target, modifications, state);
            if (keyTimes.Count == 0)
                return;
            var prevKeys = keyTimes.Where(x => x < state.editSequence.time - eps);
            if (prevKeys.Any())
            {
                state.editSequence.time = prevKeys.Max();
            }
        }

        public static void RemoveCurve(UnityEngine.Object target, IEnumerable<PropertyModification> modifications, WindowState state)
        {
            AnimationClip clip = null;
            double keyTime = 0;
            var inRange = false; // not used for curves
            if (!GetClipAndRelativeTime(target, state, out clip, out keyTime, out inRange))
                return;

            TimelineUndo.PushUndo(clip, "Remove Curve");
            foreach (var mod in modifications)
            {
                EditorCurveBinding temp;
                if (HasBinding(target, mod, clip, out temp))
                {
                    if (temp.isPPtrCurve)
                        AnimationUtility.SetObjectReferenceCurve(clip, temp, null);
                    else
                        AnimationUtility.SetEditorCurve(clip, temp, null);
                }
            }

            state.ResetPreviewMode();
        }

        public static IEnumerable<GameObject> GetRecordableGameObjects(WindowState state)
        {
            if (state == null || state.editSequence.asset == null || state.editSequence.director == null)
                yield break;

            var outputTracks = state.editSequence.asset.GetOutputTracks();
            foreach (var track in outputTracks)
            {
                if (track.GetType() != typeof(AnimationTrack))
                    continue;
                if (!state.IsTrackRecordable(track) && !track.GetChildTracks().Any(state.IsTrackRecordable))
                    continue;

                var obj = TimelineUtility.GetSceneGameObject(state.editSequence.director, track);
                if (obj != null)
                {
                    yield return obj;
                }
            }
        }
    }
}
