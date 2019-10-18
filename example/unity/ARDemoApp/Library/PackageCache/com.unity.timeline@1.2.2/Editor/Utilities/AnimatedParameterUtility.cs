using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    static class AnimatedParameterUtility
    {
        static readonly Type k_DefaultAnimationType = typeof(TimelineAsset);
        static SerializedObject s_CachedObject;

        public static ICurvesOwner ToCurvesOwner(IPlayableAsset playableAsset, TimelineAsset timeline)
        {
            if (playableAsset == null)
                return null;

            var curvesOwner = playableAsset as ICurvesOwner;
            if (curvesOwner == null)
            {
                // If the asset is not directly an ICurvesOwner, it might be the asset for a TimelineClip
                curvesOwner = TimelineRecording.FindClipWithAsset(timeline, playableAsset);
            }

            return curvesOwner;
        }

        public static bool TryGetSerializedPlayableAsset(UnityObject asset, out SerializedObject serializedObject)
        {
            serializedObject = null;
            if (asset == null || Attribute.IsDefined(asset.GetType(), typeof(NotKeyableAttribute)) || !HasScriptPlayable(asset))
                return false;

            serializedObject = GetSerializedPlayableAsset(asset);
            return serializedObject != null;
        }

        public static SerializedObject GetSerializedPlayableAsset(UnityObject asset)
        {
            if (!(asset is IPlayableAsset))
                return null;

            var scriptObject = asset as ScriptableObject;
            if (scriptObject == null)
                return null;

            if (s_CachedObject == null || s_CachedObject.targetObject != asset)
            {
                s_CachedObject = new SerializedObject(scriptObject);
            }

            return s_CachedObject;
        }

        public static void UpdateSerializedPlayableAsset(UnityObject asset)
        {
            var so = GetSerializedPlayableAsset(asset);
            if (so != null)
                so.UpdateIfRequiredOrScript();
        }

        public static bool HasScriptPlayable(UnityObject asset)
        {
            if (asset == null)
                return false;

            var scriptPlayable = asset as IPlayableBehaviour;
            return scriptPlayable != null || GetScriptPlayableFields(asset as IPlayableAsset).Any();
        }

        public static FieldInfo[] GetScriptPlayableFields(IPlayableAsset asset)
        {
            if (asset == null)
                return new FieldInfo[0];

            FieldInfo[] scriptPlayableFields;
            if (!AnimatedParameterCache.TryGetScriptPlayableFields(asset.GetType(), out scriptPlayableFields))
            {
                scriptPlayableFields = GetScriptPlayableFields_Internal(asset);
                AnimatedParameterCache.SetScriptPlayableFields(asset.GetType(), scriptPlayableFields);
            }

            return scriptPlayableFields;
        }

        static FieldInfo[] GetScriptPlayableFields_Internal(IPlayableAsset asset)
        {
            return asset.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(
                    f => typeof(IPlayableBehaviour).IsAssignableFrom(f.FieldType) &&                                        // The field is an IPlayableBehaviour
                    (f.IsPublic || f.GetCustomAttributes(typeof(SerializeField), false).Any()) &&       // The field is either public or marked with [SerializeField]
                    !f.GetCustomAttributes(typeof(NotKeyableAttribute), false).Any() &&                 // The field is not marked with [NotKeyable]
                    !f.GetCustomAttributes(typeof(HideInInspector), false).Any() &&                     // The field is not marked with [HideInInspector]
                    !f.FieldType.GetCustomAttributes(typeof(NotKeyableAttribute), false).Any())         // The field is not of a type marked with [NotKeyable]
                .ToArray();
        }

        public static bool HasAnyAnimatableParameters(UnityObject asset)
        {
            return GetAllAnimatableParameters(asset).Any();
        }

        public static IEnumerable<SerializedProperty> GetAllAnimatableParameters(UnityObject asset)
        {
            SerializedObject serializedObject;
            if (!TryGetSerializedPlayableAsset(asset, out serializedObject))
                yield break;

            var prop = serializedObject.GetIterator();

            // We need to keep this variable because prop starts invalid
            var outOfBounds = false;
            while (!outOfBounds && prop.NextVisible(true))
            {
                foreach (var property in SelectAnimatableProperty(prop))
                    yield return property;

                // We can become out of bounds by calling SelectAnimatableProperty, if the last iterated property is a color.
                outOfBounds = !prop.isValid;
            }
        }

        static IEnumerable<SerializedProperty> SelectAnimatableProperty(SerializedProperty prop)
        {
            // We're only interested by animatable leaf parameters
            if (!prop.hasChildren && IsParameterAnimatable(prop))
                yield return prop.Copy();

            // Color type is not considered "visible" when iterating
            if (prop.propertyType == SerializedPropertyType.Color)
            {
                var end = prop.GetEndProperty();

                // For some reasons, if the last 2+ serialized properties are of type Color, prop becomes invalid and
                // Next() throws an exception. This is not the case when only the last serialized property is a Color.
                while (!SerializedProperty.EqualContents(prop, end) && prop.isValid && prop.Next(true))
                {
                    foreach (var property in SelectAnimatableProperty(prop))
                        yield return property;
                }
            }
        }

        public static bool IsParameterAnimatable(UnityObject asset, string parameterName)
        {
            SerializedObject serializedObject;
            if (!TryGetSerializedPlayableAsset(asset, out serializedObject))
                return false;

            var prop = serializedObject.FindProperty(parameterName);
            return IsParameterAnimatable(prop);
        }

        public static bool IsParameterAnimatable(SerializedProperty property)
        {
            if (property == null)
                return false;

            bool isAnimatable;
            if (!AnimatedParameterCache.TryGetIsPropertyAnimatable(property, out isAnimatable))
            {
                isAnimatable = IsParameterAnimatable_Internal(property);
                AnimatedParameterCache.SetIsPropertyAnimatable(property, isAnimatable);
            }

            return isAnimatable;
        }

        static bool IsParameterAnimatable_Internal(SerializedProperty property)
        {
            if (property == null)
                return false;

            var asset = property.serializedObject.targetObject;

            // Currently not supported
            if (asset is AnimationTrack)
                return false;

            if (IsParameterKeyable(property))
                return asset is IPlayableBehaviour || IsParameterAtPathAnimatable(asset, property.propertyPath);

            return false;
        }

        static bool IsParameterKeyable(SerializedProperty property)
        {
            return IsTypeAnimatable(property.propertyType) && IsKeyableInHierarchy(property);
        }

        static bool IsKeyableInHierarchy(SerializedProperty property)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var pathSegments = property.propertyPath.Split('.');
            var type = property.serializedObject.targetObject.GetType();
            foreach (var segment in pathSegments)
            {
                if (type.GetCustomAttributes(typeof(NotKeyableAttribute), false).Any())
                {
                    return false;
                }

                var fieldInfo = type.GetField(segment, bindingFlags);

                if (fieldInfo == null ||
                    fieldInfo.GetCustomAttributes(typeof(NotKeyableAttribute), false).Any() ||
                    fieldInfo.GetCustomAttributes(typeof(HideInInspector), false).Any())
                {
                    return false;
                }

                type = fieldInfo.FieldType;
            }

            return true;
        }

        static bool IsParameterAtPathAnimatable(UnityObject asset, string path)
        {
            if (asset == null)
                return false;

            return GetScriptPlayableFields(asset as IPlayableAsset)
                .Any(
                f => path.StartsWith(f.Name, StringComparison.Ordinal) &&
                path.Length > f.Name.Length &&
                path[f.Name.Length] == '.');
        }

        public static bool IsTypeAnimatable(SerializedPropertyType type)
        {
            // Note: Integer is not currently supported by the animated property system
            switch (type)
            {
                case SerializedPropertyType.Boolean:
                case SerializedPropertyType.Float:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Color:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.Vector4:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsParameterAnimated(UnityObject asset, AnimationClip animationData, string parameterName)
        {
            if (asset == null || animationData == null)
                return false;

            var binding = GetCurveBinding(asset, parameterName);
            var bindings = AnimationClipCurveCache.Instance.GetCurveInfo(animationData).bindings;
            return bindings.Any(x => BindingMatchesParameterName(x, binding.propertyName));
        }

        // Retrieve an animated parameter curve. parameter name is required to include the appropriate field for vectors
        // e.g.: position
        public static AnimationCurve GetAnimatedParameter(UnityObject asset, AnimationClip animationData, string parameterName)
        {
            if (!(asset is ScriptableObject) || animationData == null)
                return null;

            var binding = GetCurveBinding(asset, parameterName);
            return AnimationUtility.GetEditorCurve(animationData, binding);
        }

        // get an animatable curve binding for this parameter
        public static EditorCurveBinding GetCurveBinding(UnityObject asset, string parameterName)
        {
            var animationName = GetAnimatedParameterBindingName(asset, parameterName);
            return EditorCurveBinding.FloatCurve(string.Empty, GetValidAnimationType(asset), animationName);
        }

        public static string GetAnimatedParameterBindingName(UnityObject asset, string parameterName)
        {
            if (asset == null)
                return parameterName;

            string bindingName;
            if (!AnimatedParameterCache.TryGetBindingName(asset.GetType(), parameterName, out bindingName))
            {
                bindingName = GetAnimatedParameterBindingName_Internal(asset, parameterName);
                AnimatedParameterCache.SetBindingName(asset.GetType(), parameterName, bindingName);
            }

            return bindingName;
        }

        static string GetAnimatedParameterBindingName_Internal(UnityObject asset, string parameterName)
        {
            if (asset is IPlayableBehaviour)
                return parameterName;

            // strip the IScript playable field name
            var fields = GetScriptPlayableFields(asset as IPlayableAsset);
            foreach (var f in fields)
            {
                if (parameterName.StartsWith(f.Name, StringComparison.Ordinal))
                {
                    if (parameterName.Length > f.Name.Length && parameterName[f.Name.Length] == '.')
                        return parameterName.Substring(f.Name.Length + 1);
                }
            }

            return parameterName;
        }

        public static bool BindingMatchesParameterName(EditorCurveBinding binding, string parameterName)
        {
            if (binding.propertyName == parameterName)
                return true;

            var indexOfDot = binding.propertyName.IndexOf('.');
            return indexOfDot > 0 && parameterName.Length == indexOfDot &&
                binding.propertyName.StartsWith(parameterName, StringComparison.Ordinal);
        }

        // the animated type must be a non-abstract instantiable object.
        public static Type GetValidAnimationType(UnityObject asset)
        {
            return asset != null ? asset.GetType() : k_DefaultAnimationType;
        }

        public static FieldInfo GetFieldInfoForProperty(SerializedProperty property)
        {
            FieldInfo fieldInfo;

            if (!AnimatedParameterCache.TryGetFieldInfoForProperty(property, out fieldInfo))
            {
                Type _;
                fieldInfo = ScriptAttributeUtility.GetFieldInfoFromProperty(property, out _);
                AnimatedParameterCache.SetFieldInfoForProperty(property, fieldInfo);
            }

            return fieldInfo;
        }

        public static T GetAttributeForProperty<T>(SerializedProperty property) where T : Attribute
        {
            var fieldInfo = GetFieldInfoForProperty(property);
            return fieldInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
    }
}
