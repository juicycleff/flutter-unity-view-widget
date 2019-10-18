using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class AnimatedParameterExtensions
    {
        public static bool HasAnyAnimatableParameters(this ICurvesOwner curvesOwner)
        {
            return AnimatedParameterUtility.HasAnyAnimatableParameters(curvesOwner.asset);
        }

        public static IEnumerable<SerializedProperty> GetAllAnimatableParameters(this ICurvesOwner curvesOwner)
        {
            return AnimatedParameterUtility.GetAllAnimatableParameters(curvesOwner.asset);
        }

        public static bool IsParameterAnimatable(this ICurvesOwner curvesOwner, string parameterName)
        {
            return AnimatedParameterUtility.IsParameterAnimatable(curvesOwner.asset, parameterName);
        }

        public static bool IsParameterAnimated(this ICurvesOwner curvesOwner, string parameterName)
        {
            return AnimatedParameterUtility.IsParameterAnimated(curvesOwner.asset, curvesOwner.curves, parameterName);
        }

        public static EditorCurveBinding GetCurveBinding(this ICurvesOwner curvesOwner, string parameterName)
        {
            return AnimatedParameterUtility.GetCurveBinding(curvesOwner.asset, parameterName);
        }

        public static string GetUniqueRecordedClipName(this ICurvesOwner curvesOwner)
        {
            return AnimationTrackRecorder.GetUniqueRecordedClipName(curvesOwner.assetOwner, curvesOwner.defaultCurvesName);
        }

        public static AnimationCurve GetAnimatedParameter(this ICurvesOwner curvesOwner, string bindingName)
        {
            return AnimatedParameterUtility.GetAnimatedParameter(curvesOwner.asset, curvesOwner.curves, bindingName);
        }

        public static bool AddAnimatedParameterValueAt(this ICurvesOwner curvesOwner, string parameterName, float value, float time)
        {
            if (!curvesOwner.IsParameterAnimatable(parameterName))
                return false;

            if (curvesOwner.curves == null)
                curvesOwner.CreateCurves(curvesOwner.GetUniqueRecordedClipName());

            var binding = curvesOwner.GetCurveBinding(parameterName);
            var curve = AnimationUtility.GetEditorCurve(curvesOwner.curves, binding) ?? new AnimationCurve();

            var serializedObject = AnimatedParameterUtility.GetSerializedPlayableAsset(curvesOwner.asset);
            var property = serializedObject.FindProperty(parameterName);

            bool isStepped = property.propertyType == SerializedPropertyType.Boolean ||
                property.propertyType == SerializedPropertyType.Integer ||
                property.propertyType == SerializedPropertyType.Enum;

            CurveEditUtility.AddKeyFrameToCurve(curve, time, curvesOwner.curves.frameRate, value, isStepped);
            AnimationUtility.SetEditorCurve(curvesOwner.curves, binding, curve);

            return true;
        }

        public static void SanitizeCurvesData(this ICurvesOwner curvesOwner)
        {
            var curves = curvesOwner.curves;
            if (curves == null)
                return;

            // Remove any 0-length curves
            foreach (var binding in AnimationUtility.GetCurveBindings(curves))
            {
                var curve = AnimationUtility.GetEditorCurve(curves, binding);
                if (curve.length == 0)
                    AnimationUtility.SetEditorCurve(curves, binding, null);
            }

            // If no curves remain, delete the curves asset
            if (curves.empty)
            {
                var track = curvesOwner.targetTrack;
                var timeline = track != null ? track.timelineAsset : null;
                TimelineUndo.PushDestroyUndo(timeline, track, curves, "Delete Parameter Curves");
            }
        }

        public static bool AddAnimatedParameter(this ICurvesOwner curvesOwner, string parameterName)
        {
            var newBinding = new EditorCurveBinding();

            SerializedProperty property;
            if (!InternalAddParameter(curvesOwner, parameterName, ref newBinding, out property))
                return false;

            var duration = (float)curvesOwner.duration;
            CurveEditUtility.AddKey(curvesOwner.curves, newBinding, property, 0);
            CurveEditUtility.AddKey(curvesOwner.curves, newBinding, property, duration);
            return true;
        }

        public static bool RemoveAnimatedParameter(this ICurvesOwner curvesOwner, string parameterName)
        {
            if (!curvesOwner.IsParameterAnimated(parameterName) || curvesOwner.curves == null)
                return false;

            var binding = curvesOwner.GetCurveBinding(parameterName);
            AnimationUtility.SetEditorCurve(curvesOwner.curves, binding, null);
            return true;
        }

        // Set an animated parameter. Requires the field identifier 'position.x', but will add default curves to all fields
        public static bool SetAnimatedParameter(this ICurvesOwner curvesOwner, string parameterName, AnimationCurve curve)
        {
            // this will add a basic curve for all the related parameters
            if (!curvesOwner.IsParameterAnimated(parameterName) && !curvesOwner.AddAnimatedParameter(parameterName))
                return false;

            var binding = curvesOwner.GetCurveBinding(parameterName);
            AnimationUtility.SetEditorCurve(curvesOwner.curves, binding, curve);
            return true;
        }

        static bool InternalAddParameter([NotNull] ICurvesOwner curvesOwner, string parameterName, ref EditorCurveBinding binding, out SerializedProperty property)
        {
            property = null;

            if (curvesOwner.IsParameterAnimated(parameterName))
                return false;

            var serializedObject = AnimatedParameterUtility.GetSerializedPlayableAsset(curvesOwner.asset);
            if (serializedObject == null)
                return false;

            property = serializedObject.FindProperty(parameterName);
            if (property == null || !AnimatedParameterUtility.IsTypeAnimatable(property.propertyType))
                return false;

            if (curvesOwner.curves == null)
                curvesOwner.CreateCurves(curvesOwner.GetUniqueRecordedClipName());

            binding = curvesOwner.GetCurveBinding(parameterName);
            return true;
        }
    }
}
