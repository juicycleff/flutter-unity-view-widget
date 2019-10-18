using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.MemoryProfiler;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    static class TimelineHelpers
    {
        static List<Type> s_SubClassesOfTrackDrawer;

        // check whether the exposed reference is explicitly named
        static bool IsExposedReferenceExplicitlyNamed(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            
            GUID guid;
            return !GUID.TryParse(name, out guid);
        }

        static string GenerateExposedReferenceName()
        {
            return UnityEditor.GUID.Generate().ToString();
        }
        
        
        public static void CloneExposedReferences(ScriptableObject clone, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable)
        {
            var cloneObject = new SerializedObject(clone);
            SerializedProperty prop = cloneObject.GetIterator();
            while (prop.Next(true))
            {
                if (prop.propertyType == SerializedPropertyType.ExposedReference)
                {
                    var exposedNameProp = prop.FindPropertyRelative("exposedName");
                    var sourceKey = exposedNameProp.stringValue;
                    var destKey = sourceKey;

                    if (!IsExposedReferenceExplicitlyNamed(sourceKey))
                        destKey = GenerateExposedReferenceName();

                    exposedNameProp.stringValue = destKey;

                    var requiresCopy = sourceTable != destTable || sourceKey != destKey;
                    if (requiresCopy && sourceTable != null && destTable != null)
                    {
                        var valid = false;
                        var target = sourceTable.GetReferenceValue(sourceKey, out valid);
                        if (valid && target != null)
                        {
                            var existing = destTable.GetReferenceValue(destKey, out valid);
                            if (!valid || existing != target)
                            {
                                var destTableObj = destTable as UnityEngine.Object;
                                if (destTableObj != null)
                                    TimelineUndo.PushUndo(destTableObj, "Create Clip");
                                destTable.SetReferenceValue(destKey, target);
                            }
                        }
                    }
                }
            }
            cloneObject.ApplyModifiedPropertiesWithoutUndo();
        }
        
        
        public static ScriptableObject CloneReferencedPlayableAsset(ScriptableObject original, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, Object newOwner)
        {
            var clone = Object.Instantiate(original);
            SaveCloneToAsset(clone, newOwner);
            if (clone == null || (clone as IPlayableAsset) == null)
            {
                throw new InvalidCastException("could not cast instantiated object into IPlayableAsset");
            }
            CloneExposedReferences(clone, sourceTable, destTable);
            TimelineUndo.RegisterCreatedObjectUndo(clone, "Create clip");
            
            return clone;
        }

        static void SaveCloneToAsset(Object clone, Object newOwner)
        {
            if (newOwner == null)
                return;
            
            var containerPath = AssetDatabase.GetAssetPath(newOwner);
            var containerAsset = AssetDatabase.LoadAssetAtPath<Object>(containerPath);
            if (containerAsset != null)
            {
                TimelineCreateUtilities.SaveAssetIntoObject(clone, containerAsset);
                EditorUtility.SetDirty(containerAsset);
            }
        }

        static AnimationClip CloneAnimationClip(AnimationClip clip, Object owner)
        {
            if (clip == null)
                return null;

            var newClip = Object.Instantiate(clip);
            newClip.name = AnimationTrackRecorder.GetUniqueRecordedClipName(owner, clip.name);

            TimelineUndo.RegisterCreatedObjectUndo(newClip, "Create clip");
            SaveAnimClipIntoObject(newClip, owner);

            return newClip;
        }

        public static TimelineClip Clone(TimelineClip clip, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, double time, PlayableAsset newOwner = null)
        {
            if (newOwner == null)
                newOwner = clip.parentTrack;

            TimelineClip newClip = DuplicateClip(clip, sourceTable, destTable, newOwner);
            newClip.start = time;
            var track = newClip.parentTrack;
            track.SortClips();
            TrackExtensions.ComputeBlendsFromOverlaps(track.clips);
            return newClip;
        }

        // Creates a complete clone of a track and returns it.
        // Does not parent, or add the track to the sequence
        public static TrackAsset Clone(PlayableAsset parent, TrackAsset trackAsset, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, PlayableAsset assetOwner = null)
        {
            if (trackAsset == null)
                return null;

            var timelineAsset = trackAsset.timelineAsset;
            if (timelineAsset == null)
                return null;

            if (assetOwner == null)
                assetOwner = parent;

            // create a duplicate, then clear the clips and subtracks
            var newTrack = Object.Instantiate(trackAsset);
            newTrack.name = trackAsset.name;
            newTrack.ClearClipsInternal();
            newTrack.parent = parent;
            newTrack.ClearSubTracksInternal();

            if (trackAsset.hasCurves)
                newTrack.curves = CloneAnimationClip(trackAsset.curves, assetOwner);

            var animTrack = trackAsset as AnimationTrack;
            if (animTrack != null && animTrack.infiniteClip != null)
                ((AnimationTrack)newTrack).infiniteClip = CloneAnimationClip(animTrack.infiniteClip, assetOwner);

            foreach (var clip in trackAsset.clips)
            {
                var newClip = DuplicateClip(clip, sourceTable, destTable, assetOwner);
                newClip.parentTrack = newTrack;
            }

            newTrack.ClearMarkers();
            foreach (var e in trackAsset.GetMarkersRaw())
            {
                var newMarker = Object.Instantiate(e);
                newTrack.AddMarker(newMarker);
                SaveCloneToAsset(newMarker, assetOwner);
                if (newMarker is IMarker)
                {
                    (newMarker as IMarker).Initialize(newTrack);
                }
            }

            newTrack.SetCollapsed(trackAsset.GetCollapsed());

            // calling code is responsible for adding to asset, adding to sequence, and parenting,
            // and duplicating subtracks
            return newTrack;
        }

        public static IEnumerable<ITimelineItem> DuplicateItemsUsingCurrentEditMode(WindowState state, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, ItemsPerTrack items, TrackAsset targetParent, double candidateTime, string undoOperation)
        {
            if (targetParent != null)
            {
                var aTrack = targetParent as AnimationTrack;
                if (aTrack != null)
                    aTrack.ConvertToClipMode();

                var duplicatedItems = DuplicateItems(items, targetParent, sourceTable, destTable, undoOperation);
                FinalizeInsertItemsUsingCurrentEditMode(state, new[] {duplicatedItems}, candidateTime);
                return duplicatedItems.items;
            }

            return Enumerable.Empty<ITimelineItem>();
        }
        
        public static IEnumerable<ITimelineItem> DuplicateItemsUsingCurrentEditMode(WindowState state, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, IEnumerable<ItemsPerTrack> items, double candidateTime, string undoOperation)
        {
            var duplicatedItemsGroups = new List<ItemsPerTrack>();
            foreach (var i in items)
                duplicatedItemsGroups.Add(DuplicateItems(i, i.targetTrack, sourceTable, destTable, undoOperation));

            FinalizeInsertItemsUsingCurrentEditMode(state, duplicatedItemsGroups, candidateTime);
            return duplicatedItemsGroups.SelectMany(i => i.items);
        }

        internal static ItemsPerTrack DuplicateItems(ItemsPerTrack items, TrackAsset target, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, string undoOperation)
        {
            var duplicatedItems = new List<ITimelineItem>();
            var clips = items.clips.ToList();
            if (clips.Any())
            {
                TimelineUndo.PushUndo(target, undoOperation);
                duplicatedItems.AddRange(DuplicateClips(clips, sourceTable, destTable, target).ToItems());
                TimelineUndo.PushUndo(target, undoOperation); // second undo causes reference fixups on redo (case 1063868)
            }

            var markers = items.markers.ToList();
            if (markers.Any())
            {
                duplicatedItems.AddRange(MarkerModifier.CloneMarkersToParent(markers, target).ToItems());
            }

            return new ItemsPerTrack(target, duplicatedItems.ToArray());
        }

        static void FinalizeInsertItemsUsingCurrentEditMode(WindowState state, IList<ItemsPerTrack> itemsGroups, double candidateTime)
        {
            EditMode.FinalizeInsertItemsAtTime(itemsGroups, candidateTime);

            SelectionManager.Clear();
            foreach (var itemsGroup in itemsGroups)
            {
                var track = itemsGroup.targetTrack;
                var items = itemsGroup.items;

                EditModeUtils.SetParentTrack(items, track);

                track.SortClips();

                TrackExtensions.ComputeBlendsFromOverlaps(track.clips);
                track.CalculateExtrapolationTimes();

                foreach (var item in items)
                    if (item.gui != null) item.gui.Select();
            }

            var allItems = itemsGroups.SelectMany(x => x.items).ToList();
            foreach (var item in allItems)
            {
                SelectionManager.Add(item);
            }

            FrameItems(state, allItems);
        }

        internal static TimelineClip Clone(TimelineClip clip, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, PlayableAsset newOwner)
        {
            var editorClip = EditorClipFactory.GetEditorClip(clip);
            // Workaround for Clips not being unity object, assign it to a editor clip wrapper, clone it, and pull the clip back out
            var newClip = Object.Instantiate(editorClip).clip;

            // perform fix ups for what Instantiate cannot properly detect
            SelectionManager.Remove(newClip);
            newClip.parentTrack = null;
            newClip.curves = null; // instantiate might copy the reference, we need to clear it

            // curves are explicitly owned by the clip
            if (clip.curves != null)
            {
                newClip.CreateCurves(AnimationTrackRecorder.GetUniqueRecordedClipName(newOwner, clip.curves.name));
                EditorUtility.CopySerialized(clip.curves, newClip.curves);
                TimelineCreateUtilities.SaveAssetIntoObject(newClip.curves, newOwner);
            }

            ScriptableObject playableAsset = newClip.asset as ScriptableObject;
            if (playableAsset != null && newClip.asset is IPlayableAsset)
            {
                var clone = CloneReferencedPlayableAsset(playableAsset, sourceTable, destTable, newOwner);
                newClip.asset = clone;

                // special case to make the name match the recordable clips, but only if they match on the original clip
                var originalRecordedAsset = clip.asset as AnimationPlayableAsset;
                if (clip.recordable && originalRecordedAsset != null && originalRecordedAsset.clip != null)
                {
                    AnimationPlayableAsset clonedAnimationAsset = clone as AnimationPlayableAsset;
                    if (clonedAnimationAsset != null && clonedAnimationAsset.clip != null)
                    {
                        clonedAnimationAsset.clip = CloneAnimationClip(originalRecordedAsset.clip, newOwner);
                        if (clip.displayName == originalRecordedAsset.clip.name && newClip.recordable)
                        {
                            clonedAnimationAsset.name = clonedAnimationAsset.clip.name;
                            newClip.displayName = clonedAnimationAsset.name;
                        }
                    }
                }
            }

            return newClip;
        }

        static TimelineClip[] DuplicateClips(IEnumerable<TimelineClip> clips, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, PlayableAsset newOwner)
        {
            var newClips = new TimelineClip[clips.Count()];

            int i = 0;

            foreach (var clip in clips)
            {
                var newParent = newOwner == null ? clip.parentTrack : newOwner;
                var newClip = DuplicateClip(clip, sourceTable, destTable, newParent);
                newClip.parentTrack = null;
                newClips[i++] = newClip;
            }

            return newClips;
        }

        static TimelineClip DuplicateClip(TimelineClip clip, IExposedPropertyTable sourceTable, IExposedPropertyTable destTable, PlayableAsset newOwner)
        {
            var newClip = Clone(clip, sourceTable, destTable, newOwner);

            var track = clip.parentTrack;
            if (track != null)
            {
                newClip.parentTrack = track;
                track.AddClip(newClip);
            }

            var editor = CustomTimelineEditorCache.GetClipEditor(clip);
            try
            {
                editor.OnCreate(newClip, track, clip);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return newClip;
        }

        // Given a track type, return all the playable asset types that should be
        //  visible to the user via menus

        // Given a track type, return all the playable asset types


        public static Type GetCustomDrawer(Type trackType)
        {
            if (s_SubClassesOfTrackDrawer == null)
            {
                s_SubClassesOfTrackDrawer = TypeCache.GetTypesDerivedFrom<TrackDrawer>().ToList();
            }

            foreach (var drawer in s_SubClassesOfTrackDrawer)
            {
                var attr = Attribute.GetCustomAttribute(drawer, typeof(CustomTrackDrawerAttribute), false) as CustomTrackDrawerAttribute;
                if (attr != null && attr.assetType.IsAssignableFrom(trackType))
                    return drawer;
            }

            return typeof(TrackDrawer);
        }

        public static bool HaveSameContainerAsset(Object assetA, Object assetB)
        {
            if (assetA == null || assetB == null)
                return false;

            if ((assetA.hideFlags & HideFlags.DontSave) != 0  && (assetB.hideFlags & HideFlags.DontSave) != 0)
                return true;

            return AssetDatabase.GetAssetPath(assetA) == AssetDatabase.GetAssetPath(assetB);
        }

        public static void SaveAnimClipIntoObject(AnimationClip clip, Object asset)
        {
            if (asset != null)
            {
                clip.hideFlags = asset.hideFlags & ~HideFlags.HideInHierarchy; // show animation clips, even if the parent track isn't
                if ((clip.hideFlags & HideFlags.DontSave) == 0)
                {
                    AssetDatabase.AddObjectToAsset(clip, asset);
                }
            }
        }

        // Make sure a gameobject has all the required component for the given TrackAsset
        public static Component AddRequiredComponent(GameObject go, TrackAsset asset)
        {
            if (go == null || asset == null)
                return null;

            var bindings = asset.outputs;
            if (!bindings.Any())
                return null;

            var binding = bindings.First();
            if (binding.outputTargetType == null || !typeof(Component).IsAssignableFrom(binding.outputTargetType))
                return null;

            var component = go.GetComponent(binding.outputTargetType);
            if (component == null)
            {
                component = Undo.AddComponent(go, binding.outputTargetType);
            }
            return component;
        }

        public static string GetTrackCategoryName(System.Type trackType)
        {
            if (trackType == null)
                return string.Empty;

            string s = GetItemCategoryName(trackType);
            if (!String.IsNullOrEmpty(s))
                return s;

            if (trackType.Namespace == null || trackType.Namespace.Contains("UnityEngine"))
                return string.Empty;

            return trackType.Namespace + "/";
        }

        public static string GetItemCategoryName(System.Type itemType)
        {
            if (itemType == null)
                return string.Empty;

            var attribute = itemType.GetCustomAttribute(typeof(MenuCategoryAttribute)) as MenuCategoryAttribute;
            if (attribute != null)
            {
                var s = attribute.category;
                if (!s.EndsWith("/"))
                    s += "/";
                return s;
            }

            return string.Empty;
        }

        public static string GetTrackMenuName(System.Type trackType)
        {
            return ObjectNames.NicifyVariableName(trackType.Name);
        }

        // retrieve the duration of a single loop, taking into account speed
        public static double GetLoopDuration(TimelineClip clip)
        {
            double length = clip.clipAssetDuration;
            if (double.IsNegativeInfinity(length) || double.IsNaN(length))
                return TimelineClip.kMinDuration;

            if (length == double.MaxValue || double.IsInfinity(length))
            {
                return double.MaxValue;
            }

            return Math.Max(TimelineClip.kMinDuration, length / clip.timeScale);
        }

        public static double GetClipAssetEndTime(TimelineClip clip)
        {
            var d = GetLoopDuration(clip);
            if (d < double.MaxValue)
                d = clip.FromLocalTimeUnbound(d);

            return d;
        }

        // Checks if the underlying asset duration is usable. This means the clip
        //  can loop or hold
        public static bool HasUsableAssetDuration(TimelineClip clip)
        {
            double length = clip.clipAssetDuration;
            return (length < TimelineClip.kMaxTimeValue) && !double.IsInfinity(length) && !double.IsNaN(length);
        }

        // Retrieves the starting point of each loop of a clip, relative to the start of the clip
        //  Note that if clip-in is bigger than the loopDuration, negative times will be added
        public static double[] GetLoopTimes(TimelineClip clip)
        {
            if (!HasUsableAssetDuration(clip))
                return new[] {-clip.clipIn / clip.timeScale};

            var times = new List<double>();
            double loopDuration = GetLoopDuration(clip);

            if (loopDuration <= TimeUtility.kTimeEpsilon)
                return new double[] {};


            double start = -clip.clipIn / clip.timeScale;
            double end = start + loopDuration;

            times.Add(start);
            while (end < clip.duration - WindowState.kTimeEpsilon)
            {
                times.Add(end);
                end += loopDuration;
            }

            return times.ToArray();
        }

        public static double GetCandidateTime(WindowState state, Vector2? mousePosition, params TrackAsset[] trackAssets)
        {
            // Right-Click
            if (mousePosition != null)
                return state.GetSnappedTimeAtMousePosition(mousePosition.Value);

            // Playhead
            if (state != null && state.editSequence.director != null)
                return state.SnapToFrameIfRequired(state.editSequence.time);

            // Specific tracks end
            if (trackAssets != null && trackAssets.Any())
            {
                var items = trackAssets.SelectMany(t => t.GetItems()).ToList();
                return items.Any() ? items.Max(i => i.end) : 0;
            }

            // Timeline tracks end
            if (state != null && state.editSequence.asset != null)
                return state.editSequence.asset.flattenedTracks.Any() ? state.editSequence.asset.flattenedTracks.Max(t => t.end) : 0;

            return 0.0;
        }

        public static TimelineClip CreateClipOnTrack(Object asset, TrackAsset parentTrack, WindowState state)
        {
            return CreateClipOnTrack(asset, parentTrack, GetCandidateTime(state, null, parentTrack), state);
        }

        public static TimelineClip CreateClipOnTrack(Object asset, TrackAsset parentTrack, double candidateTime)
        {
            WindowState state = null;
            if (TimelineWindow.instance != null)
                state = TimelineWindow.instance.state;

            return CreateClipOnTrack(asset, parentTrack, candidateTime, state);
        }

        public static TimelineClip CreateClipOnTrack(Type playableAssetType, TrackAsset parentTrack, WindowState state)
        {
            return CreateClipOnTrack(playableAssetType, null, parentTrack, GetCandidateTime(state, null, parentTrack), state);
        }

        public static TimelineClip CreateClipOnTrack(Type playableAssetType, TrackAsset parentTrack, double candidateTime)
        {
            return CreateClipOnTrack(playableAssetType, null, parentTrack, candidateTime);
        }

        public static TimelineClip CreateClipOnTrack(Object asset, TrackAsset parentTrack, double candidateTime, WindowState state)
        {
            if (parentTrack == null)
                return null;

            // pick the first clip type available, unless there is one that matches the asset
            var clipType = TypeUtility.GetPlayableAssetsHandledByTrack(parentTrack.GetType()).FirstOrDefault();
            if (asset != null)
                clipType = TypeUtility.GetAssetTypesForObject(parentTrack.GetType(), asset).FirstOrDefault();

            if (clipType == null)
                return null;

            return CreateClipOnTrack(clipType, asset, parentTrack, candidateTime, state);
        }

        public static TimelineClip CreateClipOnTrack(Type playableAssetType, Object assignableObject, TrackAsset parentTrack, double candidateTime)
        {
            WindowState state = null;
            if (TimelineWindow.instance != null)
                state = TimelineWindow.instance.state;

            return CreateClipOnTrack(playableAssetType, assignableObject, parentTrack, candidateTime, state);
        }

        public static TimelineClip CreateClipOnTrack(Type playableAssetType, Object assignableObject, TrackAsset parentTrack, double candidateTime, WindowState state)
        {
            if (parentTrack == null)
                return null;

            bool revertClipMode = false;

            // Ideally this is done automatically by the animation track,
            // but it's editor only because it does animation clip manipulation
            var animTrack = parentTrack as AnimationTrack;
            if (animTrack != null && animTrack.CanConvertToClipMode())
            {
                animTrack.ConvertToClipMode();
                revertClipMode = true;
            }

            TimelineClip newClip = null;
            if (TypeUtility.IsConcretePlayableAsset(playableAssetType))
            {
                try
                {
                    newClip = parentTrack.CreateClipOfType(playableAssetType);
                }
                catch (InvalidOperationException) {}    // expected on a mismatch
            }

            if (newClip == null)
            {
                if (revertClipMode)
                    animTrack.ConvertFromClipMode(animTrack.timelineAsset);

                Debug.LogWarningFormat("Cannot create a clip of type {0} on a track of type {1}", playableAssetType.Name, parentTrack.GetType().Name);
                return null;
            }

            AddClipOnTrack(newClip, parentTrack, candidateTime, assignableObject, state);

            return newClip;
        }

        /// <summary>
        /// Create a clip on track from an existing PlayableAsset
        /// </summary>
        public static TimelineClip CreateClipOnTrackFromPlayableAsset(IPlayableAsset asset, TrackAsset parentTrack, double candidateTime)
        {
            if (parentTrack == null || asset == null || !TypeUtility.IsConcretePlayableAsset(asset.GetType()))
                return null;

            TimelineClip newClip = null;
            try
            {
                newClip = parentTrack.CreateClipFromPlayableAsset(asset);
            }
            catch
            {
                return null;
            }

            WindowState state = null;
            if (TimelineWindow.instance != null)
                state = TimelineWindow.instance.state;

            AddClipOnTrack(newClip, parentTrack, candidateTime, null, state);

            return newClip;
        }

        public static void CreateClipsFromObjects(Type assetType, TrackAsset targetTrack, double candidateTime, IEnumerable<Object> objects)
        {
            foreach (var obj in objects)
            {
                if (ObjectReferenceField.FindObjectReferences(assetType).Any(f => f.IsAssignable(obj)))
                {
                    var clip = CreateClipOnTrack(assetType, obj, targetTrack, candidateTime);
                    candidateTime += clip.duration;
                }
            }
        }

        public static void CreateMarkersFromObjects(Type assetType, TrackAsset targetTrack, double candidateTime, IEnumerable<Object> objects)
        {
            var mList = new List<ITimelineItem>();
            foreach (var obj in objects)
            {
                if (ObjectReferenceField.FindObjectReferences(assetType).Any(f => f.IsAssignable(obj)))
                {
                    var marker = CreateMarkerOnTrack(assetType, obj, targetTrack, candidateTime);
                    mList.Add(marker.ToItem());
                }
            }

            var state = TimelineWindow.instance.state;
            for (var i = 1; i < mList.Count; ++i)
            {
                var delta = ItemsUtils.TimeGapBetweenItems(mList[i - 1], mList[i], state);
                mList[i].start += delta;
            }

            FinalizeInsertItemsUsingCurrentEditMode(state, new[] {new ItemsPerTrack(targetTrack, mList)}, candidateTime);
            state.Refresh();
        }

        public static IMarker CreateMarkerOnTrack(Type markerType, Object assignableObject, TrackAsset parentTrack, double candidateTime)
        {
            WindowState state = null;
            if (TimelineWindow.instance != null)
                state = TimelineWindow.instance.state;

            var newMarker = parentTrack.CreateMarker(markerType, candidateTime); //Throws if marker is not an object
            var obj = newMarker as ScriptableObject;
            if (obj != null)
                obj.name = TypeUtility.GetDisplayName(markerType);

            if (assignableObject != null)
            {
                var director = state != null ? state.editSequence.director : null;
                foreach (var field in ObjectReferenceField.FindObjectReferences(markerType))
                {
                    if (field.IsAssignable(assignableObject))
                    {
                        field.Assign(newMarker as ScriptableObject, assignableObject, director);
                        break;
                    }
                }
            }

            try
            {
                CustomTimelineEditorCache.GetMarkerEditor(newMarker).OnCreate(newMarker, null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return newMarker;
        }

        public static void CreateClipsFromTypes(IEnumerable<Type> assetTypes, TrackAsset targetTrack, double candidateTime)
        {
            foreach (var assetType in assetTypes)
            {
                var clip = CreateClipOnTrack(assetType, targetTrack, candidateTime);
                candidateTime += clip.duration;
            }
        }

        public static void FrameItems(WindowState state, IEnumerable<ITimelineItem> items)
        {
            if (items == null || !items.Any() || state == null)
                return;

            // if this is called before a repaint, the timeArea can be null
            var window = state.editorWindow as TimelineWindow;
            if (window == null || window.timeArea == null)
                return;

            var start = (float)items.Min(x => x.start);
            var end = (float)items.Max(x => x.end);
            var timeRange = state.timeAreaShownRange;

            // nothing to do
            if (timeRange.x <= start && timeRange.y >= end)
                return;

            var ds = start - timeRange.x;
            var de = end - timeRange.y;

            var padding = state.PixelDeltaToDeltaTime(15);
            var d = Math.Abs(ds) < Math.Abs(de) ? ds - padding : de + padding;

            state.SetTimeAreaShownRange(timeRange.x + d, timeRange.y + d);
        }

        public static void Frame(WindowState state, double start, double end)
        {
            var timeRange = state.timeAreaShownRange;

            // nothing to do
            if (timeRange.x <= start && timeRange.y >= end)
                return;

            var ds = (float)start - timeRange.x;
            var de = (float)end - timeRange.y;

            var padding = state.PixelDeltaToDeltaTime(15);
            var d = Math.Abs(ds) < Math.Abs(de) ? ds - padding : de + padding;

            state.SetTimeAreaShownRange(timeRange.x + d, timeRange.y + d);
        }

        public static void RangeSelect<T>(IList<T> totalCollection, IList<T> currentSelection, T clickedItem, Action<T> selector, Action<T> remover) where T : class
        {
            var firstSelect = currentSelection.FirstOrDefault();
            if (firstSelect == null)
            {
                selector(clickedItem);
                return;
            }

            var idxFirstSelect = totalCollection.IndexOf(firstSelect);
            var idxLastSelect = totalCollection.IndexOf(currentSelection.Last());
            var idxClicked = totalCollection.IndexOf(clickedItem);

            //case 927807: selection is invalid
            if (idxFirstSelect < 0)
            {
                SelectionManager.Clear();
                selector(clickedItem);
                return;
            }

            // Expand the selection between the first selected clip and clicked clip (insertion order is important)
            if (idxFirstSelect < idxClicked)
                for (var i = idxFirstSelect; i <= idxClicked; ++i)
                    selector(totalCollection[i]);
            else
                for (var i = idxFirstSelect; i >= idxClicked; --i)
                    selector(totalCollection[i]);

            // If clicked inside the selected range, shrink the selection between the the click and last selected clip
            if (Math.Min(idxFirstSelect, idxLastSelect) < idxClicked && idxClicked < Math.Max(idxFirstSelect, idxLastSelect))
                for (var i = Math.Min(idxLastSelect, idxClicked); i <= Math.Max(idxLastSelect, idxClicked); ++i)
                    remover(totalCollection[i]);

            // Ensure clicked clip is selected
            selector(clickedItem);
        }

        public static void Bind(TrackAsset track, Object obj, PlayableDirector director)
        {
            if (director != null && track != null)
            {
                var bindType = TypeUtility.GetTrackBindingAttribute(track.GetType());
                if (bindType == null || bindType.type == null)
                    return;

                if (obj == null || bindType.type.IsInstanceOfType(obj))
                {
                    TimelineUndo.PushUndo(director, "Bind Track");
                    director.SetGenericBinding(track, obj);
                }
                else if (obj is GameObject && typeof(Component).IsAssignableFrom(bindType.type))
                {
                    var component = (obj as GameObject).GetComponent(bindType.type);
                    if (component == null)
                        component = Undo.AddComponent(obj as GameObject, bindType.type);

                    TimelineUndo.PushUndo(director, "Bind Track");
                    director.SetGenericBinding(track, component);
                }
            }
        }

        /// <summary>
        /// Shared code for adding a clip to a track
        /// </summary>
        static void AddClipOnTrack(TimelineClip newClip, TrackAsset parentTrack, double candidateTime, Object assignableObject, WindowState state)
        {
            var playableAsset = newClip.asset as IPlayableAsset;

            newClip.parentTrack = null;
            newClip.timeScale = 1.0;
            newClip.mixInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            newClip.mixOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

            var playableDirector = state != null ? state.editSequence.director : null;

            if (assignableObject != null)
            {
                foreach (var field in ObjectReferenceField.FindObjectReferences(playableAsset.GetType()))
                {
                    if (field.IsAssignable(assignableObject))
                    {
                        newClip.displayName = assignableObject.name;
                        field.Assign(newClip.asset as PlayableAsset, assignableObject, playableDirector);
                        break;
                    }
                }
            }

            // get the clip editor
            try
            {
                CustomTimelineEditorCache.GetClipEditor(newClip).OnCreate(newClip, parentTrack, null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }


            // reset the duration as the newly assigned values may have changed the default
            if (playableAsset != null)
            {
                var candidateDuration = playableAsset.duration;

                if (!double.IsInfinity(candidateDuration) && candidateDuration > 0)
                    newClip.duration = Math.Min(Math.Max(candidateDuration, TimelineClip.kMinDuration), TimelineClip.kMaxTimeValue);
            }

            var newClipsByTracks = new[] { new ItemsPerTrack(parentTrack, new[] {newClip.ToItem()}) };

            FinalizeInsertItemsUsingCurrentEditMode(state, newClipsByTracks, candidateTime);

            if (state != null)
                state.Refresh();
        }

        public static TrackAsset CreateTrack(TimelineAsset asset, Type type, TrackAsset parent = null, string name = null)
        {
            if (asset == null)
                return null;

            var track = asset.CreateTrack(type, parent, name);
            if (track != null)
            {
                if (parent != null)
                    parent.SetCollapsed(false);

                var editor = CustomTimelineEditorCache.GetTrackEditor(track);
                try
                {
                    editor.OnCreate(track, null);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            }

            return track;
        }

        public static TrackAsset CreateTrack(Type type, TrackAsset parent = null, string name = null)
        {
            return CreateTrack(TimelineEditor.inspectedAsset, type, parent, name);
        }

        public static T CreateTrack<T>(TimelineAsset asset, TrackAsset parent = null, string name = null) where T : TrackAsset
        {
            return (T)CreateTrack(asset, typeof(T), parent, name);
        }

        public static T CreateTrack<T>(TrackAsset parent = null, string name = null) where T : TrackAsset
        {
            return (T)CreateTrack(TimelineEditor.inspectedAsset, typeof(T), parent, name);
        }
    }
}
