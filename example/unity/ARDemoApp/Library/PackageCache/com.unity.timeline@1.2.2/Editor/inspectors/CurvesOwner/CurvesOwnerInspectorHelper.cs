using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class CurvesOwnerInspectorHelper
    {
        // Because what is animated is not the asset, but the instanced playable,
        // we apply the animation clip here to preview what is being shown
        // This could be improved doing something more inline with animation mode,
        // and reverting values that aren't be recorded later to avoid dirtying the asset
        public static void PreparePlayableAsset(ICurvesOwnerInspectorWrapper wrapper)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (wrapper.serializedPlayableAsset == null)
                return;

            var curvesOwner = wrapper.curvesOwner;
            if (curvesOwner == null || curvesOwner.curves == null)
                return;

            var timelineWindow = TimelineWindow.instance;
            if (timelineWindow == null || timelineWindow.state == null)
                return;

            // requires preview mode. reset the eval time so previous value is correct value is displayed while toggling
            if (!timelineWindow.state.previewMode)
            {
                wrapper.lastEvalTime = -1;
                return;
            }

            var time = wrapper.ToLocalTime(timelineWindow.state.editSequence.time);

            // detect if the time has changed, or if the curves have changed
            if (Math.Abs(wrapper.lastEvalTime - time) < TimeUtility.kTimeEpsilon)
            {
                int curveVersion = AnimationClipCurveCache.Instance.GetCurveInfo(curvesOwner.curves).version;
                if (curveVersion == wrapper.lastCurveVersion)
                    return;

                wrapper.lastCurveVersion = curveVersion;
            }

            wrapper.lastEvalTime = time;

            var clipInfo = AnimationClipCurveCache.Instance.GetCurveInfo(curvesOwner.curves);
            int count = clipInfo.bindings.Length;
            if (count == 0)
                return;

            wrapper.serializedPlayableAsset.Update();

            var prop = wrapper.serializedPlayableAsset.GetIterator();
            while (prop.NextVisible(true))
            {
                if (curvesOwner.IsParameterAnimated(prop.propertyPath))
                {
                    var curve = curvesOwner.GetAnimatedParameter(prop.propertyPath);
                    switch (prop.propertyType)
                    {
                        case SerializedPropertyType.Boolean:
                            prop.boolValue = curve.Evaluate((float)time) > 0;
                            break;
                        case SerializedPropertyType.Float:
                            prop.floatValue = curve.Evaluate((float)time);
                            break;
                        case SerializedPropertyType.Integer:
                            prop.intValue = Mathf.FloorToInt(curve.Evaluate((float)time));
                            break;
                        case SerializedPropertyType.Color:
                            SetAnimatedValue(curvesOwner, prop, "r", time);
                            SetAnimatedValue(curvesOwner, prop, "g", time);
                            SetAnimatedValue(curvesOwner, prop, "b", time);
                            SetAnimatedValue(curvesOwner, prop, "a", time);
                            break;
                        case SerializedPropertyType.Quaternion:
                        case SerializedPropertyType.Vector4:
                            SetAnimatedValue(curvesOwner, prop, "w", time);
                            goto case SerializedPropertyType.Vector3;
                        case SerializedPropertyType.Vector3:
                            SetAnimatedValue(curvesOwner, prop, "z", time);
                            goto case SerializedPropertyType.Vector2;
                        case SerializedPropertyType.Vector2:
                            SetAnimatedValue(curvesOwner, prop, "x", time);
                            SetAnimatedValue(curvesOwner, prop, "y", time);
                            break;
                    }
                }
            }

            wrapper.serializedPlayableAsset.ApplyModifiedPropertiesWithoutUndo();
        }

        static void SetAnimatedValue(ICurvesOwner clip, SerializedProperty property, string path, double localTime)
        {
            var prop = property.FindPropertyRelative(path);
            if (prop != null)
            {
                var curve = clip.GetAnimatedParameter(prop.propertyPath);
                if (curve != null)
                    prop.floatValue = curve.Evaluate((float)localTime);
            }
        }
    }
}
