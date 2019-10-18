using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;
using System.Globalization;

namespace UnityEditor.Timeline
{
    // Methods and data for handling recording to monobehaviours
    static partial class TimelineRecording
    {
        internal class RecordingState : IAnimationRecordingState
        {
            public GameObject activeGameObject { get; set; }
            public GameObject activeRootGameObject { get; set; }
            public AnimationClip activeAnimationClip { get; set; }

            public void SaveCurve(AnimationWindowCurve curve)
            {
                Undo.RegisterCompleteObjectUndo(activeAnimationClip, "Edit Curve");
                AnimationWindowUtility.SaveCurve(activeAnimationClip, curve);
            }

            public void AddPropertyModification(EditorCurveBinding binding, PropertyModification propertyModification, bool keepPrefabOverride)
            {
                AnimationMode.AddPropertyModification(binding, propertyModification, keepPrefabOverride);
            }

            public bool addZeroFrame
            {
                get { return false; }
            }

            public int currentFrame { get; set; }

            public bool DiscardModification(PropertyModification modification)
            {
                return false;
            }
        }

        static readonly RecordingState s_RecordState = new RecordingState();
        static readonly AnimationTrackRecorder s_TrackRecorder = new AnimationTrackRecorder();
        static readonly List<UndoPropertyModification> s_UnprocessedMods = new List<UndoPropertyModification>();
        static readonly List<UndoPropertyModification> s_ModsToProcess = new List<UndoPropertyModification>();
        static AnimationTrack s_LastTrackWarning;

        public const string kLocalPosition = "m_LocalPosition";
        public const string kLocalRotation = "m_LocalRotation";
        public const string kLocalEulerHint = "m_LocalEulerAnglesHint";
        const string kRotationWarning = "You are recording with an initial rotation offset. This may result in a misrepresentation of euler angles. When recording transform properties, it is recommended to reset rotation prior to recording";


        public static bool IsRecordingAnimationTrack { get; private set; }


        internal static UndoPropertyModification[] ProcessMonoBehaviourModification(UndoPropertyModification[] modifications, WindowState state)
        {
            if (state == null || state.editSequence.director == null)
                return modifications;

            s_UnprocessedMods.Clear();

            s_TrackRecorder.PrepareForRecord(state);

            s_ModsToProcess.Clear();
            s_ModsToProcess.AddRange(modifications.Reverse());

            while (s_ModsToProcess.Count > 0)
            {
                var modification = s_ModsToProcess[s_ModsToProcess.Count - 1];
                s_ModsToProcess.RemoveAt(s_ModsToProcess.Count - 1);

                // grab the clip we need to apply to
                var modifiedGO = GetGameObjectFromModification(modification);
                var track = GetTrackForGameObject(modifiedGO, state);
                if (track != null)
                {
                    IsRecordingAnimationTrack = true;

                    double startTime = 0;
                    var clip = s_TrackRecorder.PrepareTrack(track, state, modifiedGO, out startTime);
                    if (clip == null)
                    {
                        s_ModsToProcess.Reverse();
                        return s_ModsToProcess.ToArray();
                    }
                    s_RecordState.activeAnimationClip = clip;
                    s_RecordState.activeRootGameObject = state.GetSceneReference(track);
                    s_RecordState.activeGameObject = modifiedGO;
                    s_RecordState.currentFrame = Mathf.RoundToInt((float)startTime);

                    EditorUtility.SetDirty(clip);
                    var toProcess = GatherRelatedModifications(modification, s_ModsToProcess);

                    var animator = s_RecordState.activeRootGameObject.GetComponent<Animator>();
                    var animTrack = track as AnimationTrack;

                    // update preview mode before recording so the correct values get placed (in case we modify offsets)
                    // Case 900624
                    UpdatePreviewMode(toProcess, modifiedGO);

                    // if this is the first position/rotation recording, copy the current position / rotation to the track offset
                    AddTrackOffset(animTrack, toProcess, clip, animator);

                    // same for clip mod clips being created
                    AddClipOffset(animTrack, toProcess, s_TrackRecorder.recordClip, animator);

                    // Check if we need to handle position/rotation offsets
                    var handleOffsets = animator != null && modification.currentValue != null &&
                        modification.currentValue.target == s_RecordState.activeRootGameObject.transform &&
                        HasOffsets(animTrack, s_TrackRecorder.recordClip);
                    if (handleOffsets)
                    {
                        toProcess = HandleEulerModifications(animTrack, s_TrackRecorder.recordClip, clip, s_RecordState.currentFrame * clip.frameRate, toProcess);
                        RemoveOffsets(modification, animTrack, s_TrackRecorder.recordClip, toProcess);
                    }

                    var remaining = AnimationRecording.Process(s_RecordState, toProcess);
                    if (remaining != null && remaining.Length != 0)
                    {
                        s_UnprocessedMods.AddRange(remaining);
                    }

                    if (handleOffsets)
                    {
                        ReapplyOffsets(modification, animTrack, s_TrackRecorder.recordClip, toProcess);
                    }

                    s_TrackRecorder.FinializeTrack(track, state);

                    IsRecordingAnimationTrack = false;
                }
                else
                {
                    s_UnprocessedMods.Add(modification);
                }
            }


            s_TrackRecorder.FinalizeRecording(state);

            return s_UnprocessedMods.ToArray();
        }

        internal static bool IsPosition(UndoPropertyModification modification)
        {
            if (modification.currentValue != null)
                return modification.currentValue.propertyPath.StartsWith(kLocalPosition);
            else if (modification.previousValue != null)
                return modification.previousValue.propertyPath.StartsWith(kLocalPosition);
            return false;
        }

        internal static bool IsRotation(UndoPropertyModification modification)
        {
            if (modification.currentValue != null)
                return modification.currentValue.propertyPath.StartsWith(kLocalRotation) ||
                    modification.currentValue.propertyPath.StartsWith(kLocalEulerHint);
            if (modification.previousValue != null)
                return modification.previousValue.propertyPath.StartsWith(kLocalRotation) ||
                    modification.previousValue.propertyPath.StartsWith(kLocalEulerHint);
            return false;
        }

        // Test if this modification position or rotation
        internal static bool IsPositionOrRotation(UndoPropertyModification modification)
        {
            return IsPosition(modification) || IsRotation(modification);
        }

        internal static void UpdatePreviewMode(UndoPropertyModification[] mods, GameObject go)
        {
            if (mods.Any(x => IsPositionOrRotation(x) && IsRootModification(x)))
            {
                bool hasPosition = false;
                bool hasRotation = false;

                foreach (var mod in mods)
                {
                    EditorCurveBinding binding = new EditorCurveBinding();
                    if (AnimationUtility.PropertyModificationToEditorCurveBinding(mod.previousValue, go, out binding) != null)
                    {
                        hasPosition |= IsPosition(mod);
                        hasRotation |= IsRotation(mod);
                        AnimationMode.AddPropertyModification(binding, mod.previousValue, true);
                    }
                }

                // case 931859 - if we are only changing one field, all fields must be registered before
                // any recording modifications
                var driver = WindowState.previewDriver;
                if (driver != null && AnimationMode.InAnimationMode(driver))
                {
                    if (hasPosition)
                    {
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalPosition + ".x");
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalPosition + ".y");
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalPosition + ".z");
                    }
                    else if (hasRotation)
                    {
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalRotation + ".x");
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalRotation + ".y");
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalRotation + ".z");
                        DrivenPropertyManager.RegisterProperty(driver, go.transform, kLocalRotation + ".w");
                    }
                }
            }
        }

        internal static bool IsRootModification(UndoPropertyModification modification)
        {
            string path = string.Empty;
            if (modification.currentValue != null)
                path = modification.currentValue.propertyPath;
            else if (modification.previousValue != null)
                path = modification.previousValue.propertyPath;

            return !path.Contains('/') && !path.Contains('\\');
        }

        // test if the clip has any position or rotation bindings
        internal static bool ClipHasPositionOrRotation(AnimationClip clip)
        {
            if (clip == null || clip.empty)
                return false;

            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            for (var i = 0; i < info.bindings.Length; i++)
            {
                bool isPositionOrRotation =
                    info.bindings[i].type != null &&
                    typeof(Transform).IsAssignableFrom(info.bindings[i].type) &&
                    (
                        info.bindings[i].propertyName.StartsWith(kLocalPosition) ||
                        info.bindings[i].propertyName.StartsWith(kLocalRotation) ||
                        info.bindings[i].propertyName.StartsWith("localEuler")
                    );

                if (isPositionOrRotation)
                    return true;
            }

            return false;
        }

        internal static TimelineAnimationUtilities.RigidTransform ComputeInitialClipOffsets(AnimationTrack track, UndoPropertyModification[] mods, Animator animator)
        {
            // take into account the track transform
            var target = GetInitialTransform(mods, animator);
            var trackToClip = TimelineAnimationUtilities.RigidTransform.identity;
            if (track.trackOffset == TrackOffset.ApplyTransformOffsets)
                trackToClip = TimelineAnimationUtilities.RigidTransform.Compose(track.position, track.rotation);
            else if (track.trackOffset == TrackOffset.ApplySceneOffsets)
                trackToClip = TimelineAnimationUtilities.RigidTransform.Compose(track.sceneOffsetPosition, Quaternion.Euler(track.sceneOffsetRotation));

            target = TimelineAnimationUtilities.RigidTransform.Mul(TimelineAnimationUtilities.RigidTransform.Inverse(trackToClip), target);

            // set the previous position in case the animation system adds a default key
            SetPreviousPositionAndRotation(mods, animator, trackToClip.position, trackToClip.rotation);
            return target;
        }

        internal static TimelineAnimationUtilities.RigidTransform GetInitialTransform(UndoPropertyModification[] mods, Animator animator)
        {
            var pos = Vector3.zero;
            var rot = Quaternion.identity;

            // if we are operating on the root, grab the transform from the undo
            if (mods[0].previousValue.target == animator.transform)
            {
                GetPreviousPositionAndRotation(mods, ref pos, ref rot);
            }
            // otherwise we need to grab it from the root object, which is the one with the actual animator
            else
            {
                pos = animator.transform.localPosition;
                rot = animator.transform.localRotation;
            }

            // take into account the track transform
            return TimelineAnimationUtilities.RigidTransform.Compose(pos, rot);
        }

        internal static void SetPreviousPositionAndRotation(UndoPropertyModification[] mods, Animator animator, Vector3 pos, Quaternion rot)
        {
            if (mods[0].previousValue.target == animator.transform)
            {
                SetPreviousPositionAndRotation(mods, pos, rot);
            }
        }

        // If we are adding to an infinite clip, strip the objects position and rotation and set it as the clip offset
        internal static void AddTrackOffset(AnimationTrack track, UndoPropertyModification[] mods, AnimationClip clip, Animator animator)
        {
            var copyTrackOffset = !track.inClipMode &&
                !ClipHasPositionOrRotation(clip) &&
                mods.Any(x => IsPositionOrRotation(x) && IsRootModification(x)) &&
                animator != null;
            if (copyTrackOffset)
            {
                // in scene offset mode, makes sure we have the correct initial transform set
                if (track.trackOffset == TrackOffset.ApplySceneOffsets)
                {
                    var rigidTransform = GetInitialTransform(mods, animator);
                    track.sceneOffsetPosition = rigidTransform.position;
                    track.sceneOffsetRotation = rigidTransform.rotation.eulerAngles;
                    SetPreviousPositionAndRotation(mods, animator, rigidTransform.position, rigidTransform.rotation);
                }
                else
                {
                    var rigidTransform = ComputeInitialClipOffsets(track, mods, animator);
                    track.infiniteClipOffsetPosition = rigidTransform.position;
                    track.infiniteClipOffsetEulerAngles = rigidTransform.rotation.eulerAngles;
                }
            }
        }

        internal static void AddClipOffset(AnimationTrack track, UndoPropertyModification[] mods, TimelineClip clip, Animator animator)
        {
            if (clip == null || clip.asset == null)
                return;

            var clipAsset = clip.asset as AnimationPlayableAsset;
            var copyClipOffset = track.inClipMode &&
                clipAsset != null && !ClipHasPositionOrRotation(clipAsset.clip) &&
                mods.Any(x => IsPositionOrRotation(x) && IsRootModification(x)) &&
                animator != null;
            if (copyClipOffset)
            {
                var rigidTransform = ComputeInitialClipOffsets(track, mods, animator);

                clipAsset.position = rigidTransform.position;
                clipAsset.rotation = rigidTransform.rotation;
            }
        }

        internal static TimelineAnimationUtilities.RigidTransform GetLocalToTrack(AnimationTrack track, TimelineClip clip)
        {
            if (track == null)
                return TimelineAnimationUtilities.RigidTransform.Compose(Vector3.zero, Quaternion.identity);

            var trackPos = track.position;
            var trackRot = track.rotation;

            if (track.trackOffset == TrackOffset.ApplySceneOffsets)
            {
                trackPos = track.sceneOffsetPosition;
                trackRot = Quaternion.Euler(track.sceneOffsetRotation);
            }

            var clipWrapper = clip == null ? null : clip.asset as AnimationPlayableAsset;
            var clipTransform = TimelineAnimationUtilities.RigidTransform.Compose(Vector3.zero, Quaternion.identity);
            if (clipWrapper != null)
            {
                clipTransform = TimelineAnimationUtilities.RigidTransform.Compose(clipWrapper.position, clipWrapper.rotation);
            }
            else
            {
                clipTransform = TimelineAnimationUtilities.RigidTransform.Compose(track.infiniteClipOffsetPosition, track.infiniteClipOffsetRotation);
            }

            var trackTransform = TimelineAnimationUtilities.RigidTransform.Compose(trackPos, trackRot);

            return TimelineAnimationUtilities.RigidTransform.Mul(trackTransform, clipTransform);
        }

        // Checks whether there are any offsets applied to a clip
        internal static bool HasOffsets(AnimationTrack track, TimelineClip clip)
        {
            if (track == null)
                return false;

            bool hasClipOffsets = false;
            bool hasTrackOffsets = false;

            var clipWrapper = clip == null ? null : clip.asset as AnimationPlayableAsset;
            if (clipWrapper != null)
                hasClipOffsets |= clipWrapper.position != Vector3.zero || clipWrapper.rotation != Quaternion.identity;

            if (track.trackOffset == TrackOffset.ApplySceneOffsets)
            {
                hasTrackOffsets = track.sceneOffsetPosition != Vector3.zero || track.sceneOffsetRotation != Vector3.zero;
            }
            else
            {
                hasTrackOffsets = (track.position != Vector3.zero || track.rotation != Quaternion.identity);
                if (!track.inClipMode)
                    hasClipOffsets |= track.infiniteClipOffsetPosition != Vector3.zero || track.infiniteClipOffsetRotation != Quaternion.identity;
            }

            return hasTrackOffsets || hasClipOffsets;
        }

        internal static void RemoveOffsets(UndoPropertyModification modification, AnimationTrack track, TimelineClip clip, UndoPropertyModification[] mods)
        {
            if (IsPositionOrRotation(modification))
            {
                var modifiedGO = GetGameObjectFromModification(modification);
                var target = TimelineAnimationUtilities.RigidTransform.Compose(modifiedGO.transform.localPosition, modifiedGO.transform.localRotation);
                var localToTrack = GetLocalToTrack(track, clip);
                var trackToLocal = TimelineAnimationUtilities.RigidTransform.Inverse(localToTrack);
                var localSpace = TimelineAnimationUtilities.RigidTransform.Mul(trackToLocal, target);

                // Update the undo call values
                var prevPos = modifiedGO.transform.localPosition;
                var prevRot = modifiedGO.transform.localRotation;
                GetPreviousPositionAndRotation(mods, ref prevPos, ref prevRot);
                var previousRigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(trackToLocal, TimelineAnimationUtilities.RigidTransform.Compose(prevPos, prevRot));
                SetPreviousPositionAndRotation(mods, previousRigidTransform.position, previousRigidTransform.rotation);

                var currentPos = modifiedGO.transform.localPosition;
                var currentRot = modifiedGO.transform.localRotation;
                GetCurrentPositionAndRotation(mods, ref currentPos, ref currentRot);
                var currentRigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(trackToLocal, TimelineAnimationUtilities.RigidTransform.Compose(currentPos, currentRot));
                SetCurrentPositionAndRotation(mods, currentRigidTransform.position, currentRigidTransform.rotation);

                modifiedGO.transform.localPosition = localSpace.position;
                modifiedGO.transform.localRotation = localSpace.rotation;
            }
        }

        internal static void ReapplyOffsets(UndoPropertyModification modification, AnimationTrack track, TimelineClip clip, UndoPropertyModification[] mods)
        {
            if (IsPositionOrRotation(modification))
            {
                var modifiedGO = GetGameObjectFromModification(modification);
                var target = TimelineAnimationUtilities.RigidTransform.Compose(modifiedGO.transform.localPosition, modifiedGO.transform.localRotation);
                var localToTrack = GetLocalToTrack(track, clip);
                var trackSpace = TimelineAnimationUtilities.RigidTransform.Mul(localToTrack, target);

                // Update the undo call values
                var prevPos = modifiedGO.transform.localPosition;
                var prevRot = modifiedGO.transform.localRotation;
                GetPreviousPositionAndRotation(mods, ref prevPos, ref prevRot);
                var previousRigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(localToTrack, TimelineAnimationUtilities.RigidTransform.Compose(prevPos, prevRot));
                SetPreviousPositionAndRotation(mods, previousRigidTransform.position, previousRigidTransform.rotation);

                var currentPos = modifiedGO.transform.localPosition;
                var currentRot = modifiedGO.transform.localRotation;
                GetCurrentPositionAndRotation(mods, ref currentPos, ref currentRot);
                var currentRigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(localToTrack, TimelineAnimationUtilities.RigidTransform.Compose(currentPos, currentRot));
                SetCurrentPositionAndRotation(mods, currentRigidTransform.position, currentRigidTransform.rotation);

                modifiedGO.transform.localPosition = trackSpace.position;
                modifiedGO.transform.localRotation = trackSpace.rotation;
            }
        }

        // This will gather the modifications that modify the same property on the same object (rgba of a color, xyzw of a vector)
        //  Note: This will modify the list, removing any elements that match
        static UndoPropertyModification[] GatherRelatedModifications(UndoPropertyModification toMatch, List<UndoPropertyModification> list)
        {
            var matching = new List<UndoPropertyModification> {toMatch};

            for (var i = list.Count - 1; i >= 0; i--)
            {
                var undo = list[i];
                if (undo.previousValue.target == toMatch.previousValue.target &&
                    DoesPropertyPathMatch(undo.previousValue.propertyPath, toMatch.previousValue.propertyPath))
                {
                    matching.Add(undo);
                    list.RemoveAt(i);
                }
            }

            return matching.ToArray();
        }

        // Grab the game object out of the modification object
        static GameObject GetGameObjectFromModification(UndoPropertyModification mod)
        {
            // grab the GO this is modifying
            GameObject modifiedGO = null;
            if (mod.previousValue.target is GameObject)
                modifiedGO = mod.previousValue.target as GameObject;
            else if (mod.previousValue.target is Component)
                modifiedGO = (mod.previousValue.target as Component).gameObject;

            return modifiedGO;
        }

        // returns the level of the child in the hierarchy relative to the parent,
        //  or -1 if the child is not the parent or a descendent of it
        static int GetChildLevel(GameObject parent, GameObject child)
        {
            var level = 0;
            while (child != null)
            {
                if (parent == child)
                    break;
                if (child.transform.parent == null)
                    return -1;
                child = child.transform.parent.gameObject;
                level++;
            }

            if (child != null)
                return level;
            return -1;
        }

        static bool DoesPropertyPathMatch(string a, string b)
        {
            return AnimationWindowUtility.GetPropertyGroupName(a).Equals(AnimationWindowUtility.GetPropertyGroupName(a));
        }

        internal static void GetPreviousPositionAndRotation(UndoPropertyModification[] mods, ref Vector3 position, ref Quaternion rotation)
        {
            var t = mods[0].previousValue.target as Transform;
            if (t == null)
                t = (Transform)mods[0].currentValue.target;

            position = t.localPosition;
            rotation = t.localRotation;

            foreach (var mod in mods)
            {
                switch (mod.previousValue.propertyPath)
                {
                    case kLocalPosition + ".x":
                        position.x = ParseFloat(mod.previousValue.value, position.x);
                        break;
                    case kLocalPosition + ".y":
                        position.y = ParseFloat(mod.previousValue.value, position.y);
                        break;
                    case kLocalPosition + ".z":
                        position.z = ParseFloat(mod.previousValue.value, position.z);
                        break;
                    case kLocalRotation + ".x":
                        rotation.x = ParseFloat(mod.previousValue.value, rotation.x);
                        break;
                    case kLocalRotation + ".y":
                        rotation.y = ParseFloat(mod.previousValue.value, rotation.y);
                        break;
                    case kLocalRotation + ".z":
                        rotation.z = ParseFloat(mod.previousValue.value, rotation.z);
                        break;
                    case kLocalRotation + ".w":
                        rotation.w = ParseFloat(mod.previousValue.value, rotation.w);
                        break;
                }
            }
        }

        internal static void GetCurrentPositionAndRotation(UndoPropertyModification[] mods, ref Vector3 position, ref Quaternion rotation)
        {
            var t = (Transform)mods[0].currentValue.target;
            position = t.localPosition;
            rotation = t.localRotation;

            foreach (var mod in mods)
            {
                switch (mod.currentValue.propertyPath)
                {
                    case kLocalPosition + ".x":
                        position.x = ParseFloat(mod.currentValue.value, position.x);
                        break;
                    case kLocalPosition + ".y":
                        position.y = ParseFloat(mod.currentValue.value, position.y);
                        break;
                    case kLocalPosition + ".z":
                        position.z = ParseFloat(mod.currentValue.value, position.z);
                        break;
                    case kLocalRotation + ".x":
                        rotation.x = ParseFloat(mod.currentValue.value, rotation.x);
                        break;
                    case kLocalRotation + ".y":
                        rotation.y = ParseFloat(mod.currentValue.value, rotation.y);
                        break;
                    case kLocalRotation + ".z":
                        rotation.z = ParseFloat(mod.currentValue.value, rotation.z);
                        break;
                    case kLocalRotation + ".w":
                        rotation.w = ParseFloat(mod.currentValue.value, rotation.w);
                        break;
                }
            }
        }

        // when making the previous position and rotation
        internal static void SetPreviousPositionAndRotation(UndoPropertyModification[] mods, Vector3 pos, Quaternion rot)
        {
            foreach (var mod in mods)
            {
                switch (mod.previousValue.propertyPath)
                {
                    case kLocalPosition + ".x":
                        mod.previousValue.value = pos.x.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalPosition + ".y":
                        mod.previousValue.value = pos.y.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalPosition + ".z":
                        mod.previousValue.value = pos.z.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".x":
                        mod.previousValue.value = rot.x.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".y":
                        mod.previousValue.value = rot.y.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".z":
                        mod.previousValue.value = rot.z.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".w":
                        mod.previousValue.value = rot.w.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                }
            }
        }

        internal static void SetCurrentPositionAndRotation(UndoPropertyModification[] mods, Vector3 pos, Quaternion rot)
        {
            foreach (var mod in mods)
            {
                switch (mod.previousValue.propertyPath)
                {
                    case kLocalPosition + ".x":
                        mod.currentValue.value = pos.x.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalPosition + ".y":
                        mod.currentValue.value = pos.y.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalPosition + ".z":
                        mod.currentValue.value = pos.z.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".x":
                        mod.currentValue.value = rot.x.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".y":
                        mod.currentValue.value = rot.y.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".z":
                        mod.currentValue.value = rot.z.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                    case kLocalRotation + ".w":
                        mod.currentValue.value = rot.w.ToString(EditorGUI.kFloatFieldFormatString);
                        break;
                }
            }
        }

        internal static float ParseFloat(string str, float defaultVal)
        {
            float temp = 0.0f;
            if (float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out temp))
                return temp;
            return defaultVal;
        }

        internal static UndoPropertyModification[] HandleEulerModifications(AnimationTrack track, TimelineClip clip, AnimationClip animClip, float time, UndoPropertyModification[] mods)
        {
            if (mods.Any(x => x.currentValue.propertyPath.StartsWith(kLocalEulerHint) || x.currentValue.propertyPath.StartsWith(kLocalRotation)))
            {
                // if there is a rotational offsets, we need to strip the euler hints, since they are used by the animation recording system
                //  over the quaternion.
                var localToTrack = GetLocalToTrack(track, clip);
                if (localToTrack.rotation != Quaternion.identity)
                {
                    if (s_LastTrackWarning != track)
                    {
                        s_LastTrackWarning = track;
                        Debug.LogWarning(kRotationWarning);
                    }

                    Transform transform = mods[0].currentValue.target as Transform;
                    if (transform != null)
                    {
                        var trackToLocal = TimelineAnimationUtilities.RigidTransform.Inverse(localToTrack);
                        // since the euler angles are going to be transformed, we do a best guess at a euler that gives the shortest path
                        var quatMods = mods.Where(x => !x.currentValue.propertyPath.StartsWith(kLocalEulerHint));
                        var eulerMods = FindBestEulerHint(trackToLocal.rotation * transform.localRotation, animClip, time, transform);
                        return quatMods.Union(eulerMods).ToArray();
                    }
                    return mods.Where(x => !x.currentValue.propertyPath.StartsWith(kLocalEulerHint)).ToArray();
                }
            }
            return mods;
        }

        internal static IEnumerable<UndoPropertyModification> FindBestEulerHint(Quaternion rotation, AnimationClip clip, float time, Transform transform)
        {
            Vector3 euler = rotation.eulerAngles;

            var xCurve = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "localEulerAnglesRaw.x"));
            var yCurve = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "localEulerAnglesRaw.y"));
            var zCurve = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "localEulerAnglesRaw.z"));

            if (xCurve != null)
                euler.x = xCurve.Evaluate(time);
            if (yCurve != null)
                euler.y = yCurve.Evaluate(time);
            if (zCurve != null)
                euler.z = zCurve.Evaluate(time);

            euler = QuaternionCurveTangentCalculation.GetEulerFromQuaternion(rotation, euler);

            return new[]
            {
                PropertyModificationToUndoPropertyModification(new PropertyModification {target = transform, propertyPath = kLocalEulerHint + ".x", value = euler.x.ToString() }),
                PropertyModificationToUndoPropertyModification(new PropertyModification {target = transform, propertyPath = kLocalEulerHint + ".y", value = euler.y.ToString() }),
                PropertyModificationToUndoPropertyModification(new PropertyModification {target = transform, propertyPath = kLocalEulerHint + ".z", value = euler.z.ToString() })
            };
        }
    }
}
