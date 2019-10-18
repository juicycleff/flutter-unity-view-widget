using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    static class TypeUtility
    {
        private static Type[] s_AllTrackTypes;
        private static Type[] s_AllClipTypes;
        private static Type[] s_MarkerTypes;
        private static Dictionary<Type, Type[]> s_TrackTypeToVisibleClipType = new Dictionary<Type, Type[]>();
        private static Dictionary<Type, Type[]> s_TrackTypeToAllClipType = new Dictionary<Type, Type[]>();
        private static Dictionary<Type, TrackBindingTypeAttribute> s_TrackToBindingCache = new Dictionary<Type, TrackBindingTypeAttribute>();

        public static bool IsConcretePlayableAsset(Type t)
        {
            return typeof(IPlayableAsset).IsAssignableFrom(t)
                && IsConcreteAsset(t);
        }

        private static bool IsConcreteAsset(Type t)
        {
            return typeof(ScriptableObject).IsAssignableFrom(t)
                && !t.IsAbstract
                && !t.IsGenericType
                && !t.IsInterface
                && !typeof(TrackAsset).IsAssignableFrom(t)
                && !typeof(TimelineAsset).IsAssignableFrom(t);
        }

        /// <summary>
        /// List of all PlayableAssets
        /// </summary>
        public static IEnumerable<Type> AllClipTypes()
        {
            if (s_AllClipTypes == null)
            {
                s_AllClipTypes = TypeCache.GetTypesDerivedFrom<IPlayableAsset>().
                    Where(t => IsConcreteAsset(t)).
                    ToArray();
            }
            return s_AllClipTypes;
        }

        public static IEnumerable<Type> AllTrackTypes()
        {
            if (s_AllTrackTypes == null)
            {
                s_AllTrackTypes = TypeCache.GetTypesDerivedFrom<TrackAsset>()
                    .Where(x => !x.IsAbstract)
                    .ToArray();
            }

            return s_AllTrackTypes;
        }

        public static IEnumerable<Type> GetVisiblePlayableAssetsHandledByTrack(Type trackType)
        {
            if (trackType == null || !typeof(TrackAsset).IsAssignableFrom(trackType))
                return Enumerable.Empty<Type>();

            Type[] types;
            if (s_TrackTypeToVisibleClipType.TryGetValue(trackType, out types))
            {
                return types;
            }

            // special case -- the playable track handles all types not handled by other tracks
            if (trackType == typeof(PlayableTrack))
            {
                types = GetUnhandledClipTypes().ToArray();
                s_TrackTypeToVisibleClipType[trackType] = types;
                return types;
            }

            var attributes = trackType.GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
            var baseClasses = attributes.
                OfType<TrackClipTypeAttribute>().
                Where(t => t.allowAutoCreate).
                Select(a => a.inspectedType);

            types = AllClipTypes().Where(t => baseClasses.Any(x => x.IsAssignableFrom(t))).ToArray();
            s_TrackTypeToVisibleClipType[trackType] = types;
            return types;
        }

        public static IEnumerable<Type> GetPlayableAssetsHandledByTrack(Type trackType)
        {
            if (trackType == null || !typeof(TrackAsset).IsAssignableFrom(trackType))
                return Enumerable.Empty<Type>();

            Type[] types;
            if (s_TrackTypeToAllClipType.TryGetValue(trackType, out types))
            {
                return types;
            }

            // special case -- the playable track handles all types not handled by other tracks
            if (trackType == typeof(PlayableTrack))
            {
                types = GetUnhandledClipTypes().ToArray();
                s_TrackTypeToAllClipType[trackType] = types;
                return types;
            }

            var attributes = trackType.GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
            var baseClasses = attributes.
                OfType<TrackClipTypeAttribute>().
                Select(a => a.inspectedType);

            types = AllClipTypes().Where(t => baseClasses.Any(x => x.IsAssignableFrom(t))).ToArray();
            s_TrackTypeToAllClipType[trackType] = types;
            return types;
        }

        /// <summary>
        /// Returns the binding attribute attrached to the track
        /// </summary>
        public static TrackBindingTypeAttribute GetTrackBindingAttribute(Type trackType)
        {
            if (trackType == null || !typeof(TrackAsset).IsAssignableFrom(trackType))
                return null;

            TrackBindingTypeAttribute attribute = null;
            if (!s_TrackToBindingCache.TryGetValue(trackType, out attribute))
            {
                attribute = (TrackBindingTypeAttribute)Attribute.GetCustomAttribute(trackType, typeof(TrackBindingTypeAttribute));
                s_TrackToBindingCache.Add(trackType, attribute);
            }

            return attribute;
        }

        /// <summary>
        /// True if the given track has a clip type that handles the given object
        /// </summary>
        public static bool TrackHasClipForObject(Type trackType, Object obj)
        {
            return GetPlayableAssetsHandledByTrack(trackType)
                .Any(c => ObjectReferenceField.FindObjectReferences(c).Any(o => o.IsAssignable(obj)));
        }

        /// <summary>
        ///  Get the list of markers that have fields for the object
        /// </summary>
        public static IEnumerable<Type> MarkerTypesWithFieldForObject(Object obj)
        {
            return GetAllMarkerTypes().Where(
                c => ObjectReferenceField.FindObjectReferences(c).Any(o => o.IsAssignable(obj))
            );
        }

        /// <summary>
        /// Get the list of tracks that can handle this object as clips
        /// </summary>
        public static IEnumerable<Type> GetTrackTypesForObject(Object obj)
        {
            if (obj == null)
                return Enumerable.Empty<Type>();

            return AllTrackTypes().Where(t => TrackHasClipForObject(t, obj));
        }

        /// <summary>
        /// Given a trackType and an object, does the binding type match
        ///    Takes into account whether creating a missing component is permitted
        /// </summary>
        public static bool IsTrackCreatableFromObject(Object obj, Type trackType)
        {
            if (obj == null || obj.IsPrefab())
                return false;

            var attribute = GetTrackBindingAttribute(trackType);
            if (attribute == null || attribute.type == null)
                return false;

            if (attribute.type.IsAssignableFrom(obj.GetType()))
                return true;

            var gameObject = obj as GameObject;
            if (gameObject != null && typeof(Component).IsAssignableFrom(attribute.type))
            {
                return gameObject.GetComponent(attribute.type) != null ||
                    (attribute.flags & TrackBindingFlags.AllowCreateComponent) != 0;
            }

            return false;
        }

        /// <summary>
        /// Given an object, get the list of track that are creatable from it. Takes
        ///  binding flags into account
        /// </summary>
        public static IEnumerable<Type> GetTracksCreatableFromObject(Object obj)
        {
            if (obj == null)
                return Enumerable.Empty<Type>();

            return AllTrackTypes().Where(t => !IsHiddenInMenu(t) && IsTrackCreatableFromObject(obj, t));
        }

        /// <summary>
        /// Get the list of playable assets that can handle an object for a particular track
        /// </summary>
        /// <param name="trackType">The type of the track</param>
        /// <param name="obj">The object to handle</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssetTypesForObject(Type trackType, Object obj)
        {
            if (obj == null)
                return Enumerable.Empty<Type>();

            return GetPlayableAssetsHandledByTrack(trackType).Where(
                c => ObjectReferenceField.FindObjectReferences(c).Any(o => o.IsAssignable(obj))
            );
        }

        // get the track types for a track from it's attributes
        private static IEnumerable<Type> GetTrackClipTypesFromAttributes(Type trackType)
        {
            if (trackType == null || !typeof(TrackAsset).IsAssignableFrom(trackType))
                return Enumerable.Empty<Type>();

            var attributes = trackType.GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
            var baseClasses = attributes.
                OfType<TrackClipTypeAttribute>().
                Select(a => a.inspectedType);

            return AllClipTypes().Where(t => baseClasses.Any(x => x.IsAssignableFrom(t)));
        }

        // find the playable asset types that are unhandled
        private static IEnumerable<Type> GetUnhandledClipTypes()
        {
            var typesHandledByTrack = AllTrackTypes().SelectMany(t => GetTrackClipTypesFromAttributes(t));

            // exclude anything in the timeline assembly, handled by tracks, has a hide in menu attribute
            // or is explicity ignored
            return AllClipTypes()
                .Except(typesHandledByTrack)
                .Where(t => !TypeUtility.IsBuiltIn(t)) // exclude built-in
                .Where(t => !typeof(TrackAsset).IsAssignableFrom(t))       // exclude track types (they are playable assets)
                .Where(t => !t.IsDefined(typeof(HideInMenuAttribute), false) && !t.IsDefined(typeof(IgnoreOnPlayableTrackAttribute), true))
                .Distinct();
        }

        public static IEnumerable<Type> GetAllMarkerTypes()
        {
            if (s_MarkerTypes == null)
            {
                s_MarkerTypes = TypeCache.GetTypesDerivedFrom<IMarker>()
                    .Where(x =>
                        typeof(ScriptableObject).IsAssignableFrom(x)
                        && !x.IsAbstract
                        && !x.IsGenericType
                        && !x.IsInterface)
                    .ToArray();
            }
            return s_MarkerTypes;
        }

        public static IEnumerable<Type> GetUserMarkerTypes()
        {
            return GetAllMarkerTypes().Where(x => !TypeUtility.IsBuiltIn(x));
        }

        public static IEnumerable<Type> GetBuiltInMarkerTypes()
        {
            return GetAllMarkerTypes().Where(TypeUtility.IsBuiltIn);
        }

        public static bool DoesTrackSupportMarkerType(TrackAsset track, Type type)
        {
            if (track.supportsNotifications)
            {
                return true;
            }

            return !typeof(INotification).IsAssignableFrom(type);
        }

        internal static string GetDisplayName(Type t)
        {
            var displayName = ObjectNames.NicifyVariableName(t.Name);
            var attr = Attribute.GetCustomAttribute(t, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            if (attr != null)
                displayName = attr.DisplayName;
            return displayName;
        }

        public static bool IsHiddenInMenu(Type type)
        {
            var attr = type.GetCustomAttributes(typeof(HideInMenuAttribute), false);
            return attr.Length > 0;
        }

        public struct ObjectReference
        {
            public Type type;
            public bool isSceneReference;
        }

        public static IEnumerable<ObjectReference> ObjectReferencesForType(Type type)
        {
            var objectReferences = ObjectReferenceField.FindObjectReferences(type);
            var uniqueTypes = objectReferences.Select(objRef => objRef.type).Distinct();
            foreach (var refType in uniqueTypes)
            {
                var isSceneReference = objectReferences.Any(objRef => objRef.type == refType && objRef.isSceneReference);
                yield return new ObjectReference { type = refType, isSceneReference = isSceneReference };
            }
        }

        /// <summary>
        /// Checks whether a type has an overridden method with a specific name. This method also checks overridden members in parent classes.
        /// </summary>
        public static bool HasOverrideMethod(System.Type t, string name)
        {
            const MethodAttributes mask = MethodAttributes.Virtual | MethodAttributes.NewSlot;
            const MethodAttributes expectedResult = MethodAttributes.Virtual;

            var method = t.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return method != null && (method.Attributes & mask) == expectedResult;
        }

        /// <summary>
        /// Returns whether the given type resides in the timeline assembly
        /// </summary>
        public static bool IsBuiltIn(System.Type t)
        {
            return t != null && t.Assembly.Equals(typeof(TimelineAsset).Assembly);
        }
    }
}
