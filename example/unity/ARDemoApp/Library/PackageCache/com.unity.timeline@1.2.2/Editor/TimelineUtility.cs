using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Object = UnityEngine.Object;
using UnityEditor.Experimental.SceneManagement;

namespace UnityEditor.Timeline
{
    static class TimelineUtility
    {
        public static void ReorderTracks(List<ScriptableObject> allTracks, List<TrackAsset> tracks, ScriptableObject insertAfterAsset, bool up)
        {
            foreach (var i in tracks)
                allTracks.Remove(i);

            int index = allTracks.IndexOf(insertAfterAsset);

            index = up ? Math.Max(index, 0) : index + 1;

            allTracks.InsertRange(index, tracks.OfType<ScriptableObject>());
        }

        // Gets the track that holds the game object reference for this track.
        public static TrackAsset GetSceneReferenceTrack(TrackAsset asset)
        {
            if (asset == null)
                return null;
            if (asset.isSubTrack)
                return GetSceneReferenceTrack(asset.parent as TrackAsset);
            return asset;
        }

        public static bool TrackHasAnimationCurves(TrackAsset track)
        {
            if (track.hasCurves)
                return true;

            var animTrack = track as AnimationTrack;
            if (animTrack != null && animTrack.infiniteClip != null)
                return true;

            for (int i = 0; i < track.clips.Length; i++)
            {
                var curveClip = track.clips[i].curves;
                var animationClip = track.clips[i].animationClip;

                // prune out clip with zero curves
                if (curveClip != null && curveClip.empty)
                    curveClip = null;

                if (animationClip != null && animationClip.empty)
                    animationClip = null;

                // prune out clips coming from FBX
                if (animationClip != null && ((animationClip.hideFlags & HideFlags.NotEditable) != 0))
                    animationClip = null;

                if (!track.clips[i].recordable)
                    animationClip = null;

                if ((curveClip != null) || (animationClip != null))
                    return true;
            }

            return false;
        }

        // get the game object reference associated with this
        public static GameObject GetSceneGameObject(PlayableDirector director, TrackAsset asset)
        {
            if (director == null || asset == null)
                return null;

            asset = GetSceneReferenceTrack(asset);

            var gameObject = director.GetGenericBinding(asset) as GameObject;
            var component = director.GetGenericBinding(asset) as Component;
            if (component != null)
                gameObject = component.gameObject;
            return gameObject;
        }

        public static void SetSceneGameObject(PlayableDirector director, TrackAsset asset, GameObject go)
        {
            if (director == null || asset == null)
                return;

            asset = GetSceneReferenceTrack(asset);
            var bindings = asset.outputs;
            if (bindings.Count() == 0)
                return;

            var binding = bindings.First();
            if (binding.outputTargetType == typeof(GameObject))
            {
                BindingUtility.Bind(director, asset, go);
            }
            else
            {
                BindingUtility.Bind(director, asset, TimelineHelpers.AddRequiredComponent(go, asset));
            }
        }

        public static PlayableDirector[] GetDirectorsInSceneUsingAsset(PlayableAsset asset)
        {
            const HideFlags hideFlags =
                HideFlags.HideInHierarchy | HideFlags.HideInInspector |
                HideFlags.DontSaveInEditor | HideFlags.NotEditable;

            var prefabMode = PrefabStageUtility.GetCurrentPrefabStage();

            var inScene = new List<PlayableDirector>();
            var allDirectors = Resources.FindObjectsOfTypeAll(typeof(PlayableDirector)) as PlayableDirector[];
            foreach (var director in allDirectors)
            {
                if ((director.hideFlags & hideFlags) != 0)
                    continue;

                string assetPath = AssetDatabase.GetAssetPath(director.transform.root.gameObject);
                if (!String.IsNullOrEmpty(assetPath))
                    continue;

                if (prefabMode != null && !prefabMode.IsPartOfPrefabContents(director.gameObject))
                    continue;

                if (asset == null || (asset != null && director.playableAsset == asset))
                {
                    inScene.Add(director);
                }
            }
            return inScene.ToArray();
        }

        public static PlayableDirector GetDirectorComponentForGameObject(GameObject gameObject)
        {
            return gameObject != null ? gameObject.GetComponent<PlayableDirector>() : null;
        }

        public static TimelineAsset GetTimelineAssetForDirectorComponent(PlayableDirector director)
        {
            return director != null ? director.playableAsset as TimelineAsset : null;
        }

        public static bool IsPrefabOrAsset(Object obj)
        {
            return EditorUtility.IsPersistent(obj) || (obj.hideFlags & HideFlags.NotEditable) != 0;
        }

        // TODO -- Need to add this to SerializedProperty so we can get replicate the accuracy that exists
        //  in the undo system
        internal static string PropertyToString(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue.ToString();
                case SerializedPropertyType.Float:
                    return property.floatValue.ToString();
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Boolean:
                    return property.boolValue ? "1" : "0";
                case SerializedPropertyType.Color:
                    return property.colorValue.ToString();
                case SerializedPropertyType.ArraySize:
                    return property.intValue.ToString();
                case SerializedPropertyType.Enum:
                    return property.intValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return string.Empty;
                case SerializedPropertyType.LayerMask:
                    return property.intValue.ToString();
                case SerializedPropertyType.Character:
                    return property.intValue.ToString();
                case SerializedPropertyType.AnimationCurve:
                    return property.animationCurveValue.ToString();
                case SerializedPropertyType.Gradient:
                    return property.gradientValue.ToString();
                case SerializedPropertyType.Vector3:
                    return property.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return property.vector4Value.ToString();
                case SerializedPropertyType.Vector2:
                    return property.vector2Value.ToString();
                case SerializedPropertyType.Rect:
                    return property.rectValue.ToString();
                case SerializedPropertyType.Bounds:
                    return property.boundsValue.ToString();
                case SerializedPropertyType.Quaternion:
                    return property.quaternionValue.ToString();
                case SerializedPropertyType.Generic:
                    return string.Empty;
                default:
                    Debug.LogWarning("Unknown Property Type: " + property.propertyType);
                    return string.Empty;
            }
        }

        // Is this a recordable clip on an animation track.
        internal static bool IsRecordableAnimationClip(TimelineClip clip)
        {
            if (!clip.recordable)
                return false;

            AnimationPlayableAsset asset = clip.asset as AnimationPlayableAsset;
            if (asset == null)
                return false;

            return true;
        }

        public static IList<PlayableDirector> GetSubTimelines(TimelineClip clip, IExposedPropertyTable director)
        {
            var editor = CustomTimelineEditorCache.GetClipEditor(clip);
            List<PlayableDirector> directors = new List<PlayableDirector>();
            try
            {
                editor.GetSubTimelines(clip, director as PlayableDirector, directors);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return directors;
        }

        public static bool IsAllSubTrackMuted(TrackAsset asset)
        {
            if (asset is GroupTrack)
                return asset.mutedInHierarchy;

            foreach (TrackAsset t in asset.GetChildTracks())
            {
                if (!t.muted)
                    return false;

                var childMuted = IsAllSubTrackMuted(t);

                if (!childMuted)
                    return false;
            }
            return true;
        }

        public static bool IsParentMuted(TrackAsset asset)
        {
            TrackAsset p = asset.parent as TrackAsset;
            if (p == null) return false;
            return p is GroupTrack ? p.mutedInHierarchy : IsParentMuted(p);
        }

        public static IEnumerable<PlayableDirector> GetAllDirectorsInHierarchy(PlayableDirector mainDirector)
        {
            var directors = new HashSet<PlayableDirector> { mainDirector };
            GetAllDirectorsInHierarchy(mainDirector, directors);
            return directors;
        }

        static void GetAllDirectorsInHierarchy(PlayableDirector director, ISet<PlayableDirector> directors)
        {
            var timelineAsset = director.playableAsset as TimelineAsset;
            if (timelineAsset == null)
                return;

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                foreach (var clip in track.clips)
                {
                    foreach (var subDirector in GetSubTimelines(clip, director))
                    {
                        if (!directors.Contains(subDirector))
                        {
                            directors.Add(subDirector);
                            GetAllDirectorsInHierarchy(subDirector, directors);
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> GetBindingsFromDirectors<T>(IEnumerable<PlayableDirector> directors) where T : Object
        {
            var bindings = new HashSet<T>();
            foreach (var director in directors)
            {
                if (director.playableAsset == null) continue;
                foreach (var output in director.playableAsset.outputs)
                {
                    var binding = director.GetGenericBinding(output.sourceObject) as T;
                    if (binding != null)
                        bindings.Add(binding);
                }
            }
            return bindings;
        }

        public static bool IsLockedFromGroup(TrackAsset asset)
        {
            TrackAsset p = asset.parent as TrackAsset;
            if (p == null) return false;
            return p is GroupTrack ? p.lockedInHierarchy : IsLockedFromGroup(p);
        }

        internal static bool IsCurrentSequenceValid()
        {
            return TimelineWindow.instance != null
                && TimelineWindow.instance.state != null
                && TimelineWindow.instance.state.editSequence != null;
        }
    }
}
