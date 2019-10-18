using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    // Utility class for editing animation clips from serialized properties
    static class CurveEditUtility
    {
        static bool IsRotationKey(EditorCurveBinding binding)
        {
            return binding.propertyName.Contains("localEulerAnglesRaw");
        }

        public static void AddKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
        {
            if (sourceBinding.isPPtrCurve)
            {
                AddObjectKey(clip, sourceBinding, prop, time);
            }
            else if (IsRotationKey(sourceBinding))
            {
                AddRotationKey(clip, sourceBinding, prop, time);
            }
            else
            {
                AddFloatKey(clip, sourceBinding, prop, time);
            }
        }

        static void AddObjectKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
        {
            if (prop.propertyType != SerializedPropertyType.ObjectReference)
                return;

            ObjectReferenceKeyframe[] curve = null;
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            var curveIndex = Array.IndexOf(info.objectBindings, sourceBinding);
            if (curveIndex >= 0)
            {
                curve = info.objectCurves[curveIndex];

                // where in the array does the evaluation land?
                var evalIndex = EvaluateIndex(curve, (float)time);

                if (KeyCompare(curve[evalIndex].time, (float)time, clip.frameRate) == 0)
                {
                    curve[evalIndex].value = prop.objectReferenceValue;
                }
                // check the next key (always return the minimum value)
                else if (evalIndex < curve.Length - 1 && KeyCompare(curve[evalIndex + 1].time, (float)time, clip.frameRate) == 0)
                {
                    curve[evalIndex + 1].value = prop.objectReferenceValue;
                }
                // resize the array
                else
                {
                    if (time > curve[0].time)
                        evalIndex++;
                    var key = new ObjectReferenceKeyframe();
                    key.time = (float)time;
                    key.value = prop.objectReferenceValue;
                    ArrayUtility.Insert(ref curve, evalIndex, key);
                }
            }
            else // curve doesn't exist, add it
            {
                curve = new ObjectReferenceKeyframe[1];
                curve[0].time = (float)time;
                curve[0].value = prop.objectReferenceValue;
            }

            AnimationUtility.SetObjectReferenceCurve(clip, sourceBinding, curve);
            EditorUtility.SetDirty(clip);
        }

        static void AddRotationKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
        {
            if (prop.propertyType != SerializedPropertyType.Quaternion)
            {
                return;
            }

            var updateCurves = new List<AnimationCurve>();
            var updateBindings = new List<EditorCurveBinding>();

            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            for (var i = 0; i < info.bindings.Length; i++)
            {
                if (sourceBind.type != info.bindings[i].type)
                    continue;

                if (info.bindings[i].propertyName.Contains("localEuler"))
                {
                    updateBindings.Add(info.bindings[i]);
                    updateCurves.Add(info.curves[i]);
                }
            }

            // use this instead of serialized properties because the editor will attempt to maintain
            // correct localeulers
            var eulers = ((Transform)prop.serializedObject.targetObject).localEulerAngles;
            if (updateBindings.Count == 0)
            {
                var propName = AnimationWindowUtility.GetPropertyGroupName(sourceBind.propertyName);
                updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".x"));
                updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".y"));
                updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".z"));

                var curveX = new AnimationCurve();
                var curveY = new AnimationCurve();
                var curveZ = new AnimationCurve();
                AddKeyFrameToCurve(curveX, (float)time, clip.frameRate, eulers.x, false);
                AddKeyFrameToCurve(curveY, (float)time, clip.frameRate, eulers.y, false);
                AddKeyFrameToCurve(curveZ, (float)time, clip.frameRate, eulers.z, false);

                updateCurves.Add(curveX);
                updateCurves.Add(curveY);
                updateCurves.Add(curveZ);
            }

            for (var i = 0; i < updateBindings.Count; i++)
            {
                var c = updateBindings[i].propertyName.Last();
                var value = eulers.x;
                if (c == 'y') value = eulers.y;
                else if (c == 'z') value = eulers.z;
                AddKeyFrameToCurve(updateCurves[i], (float)time, clip.frameRate, value, false);
            }

            UpdateEditorCurves(clip, updateBindings, updateCurves);
        }

        // Add a floating point curve key
        static void AddFloatKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
        {
            var updateCurves = new List<AnimationCurve>();
            var updateBindings = new List<EditorCurveBinding>();

            var updated = false;
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            for (var i = 0; i < info.bindings.Length; i++)
            {
                var binding = info.bindings[i];
                if (binding.type != sourceBind.type)
                    continue;

                SerializedProperty valProp = null;
                var curve = info.curves[i];

                // perfect match on property path, editting a float
                if (prop.propertyPath.Equals(binding.propertyName))
                {
                    valProp = prop;
                }
                // this is a child object
                else if (binding.propertyName.Contains(prop.propertyPath))
                {
                    valProp = prop.serializedObject.FindProperty(binding.propertyName);
                }

                if (valProp != null)
                {
                    var value = GetKeyValue(valProp);
                    if (!float.IsNaN(value)) // Nan indicates an error retrieving the property value
                    {
                        updated = true;
                        AddKeyFrameToCurve(curve, (float)time, clip.frameRate, value, valProp.propertyType == SerializedPropertyType.Boolean);
                        updateCurves.Add(curve);
                        updateBindings.Add(binding);
                    }
                }
            }

            // Curves don't exist, add them
            if (!updated)
            {
                var propName = AnimationWindowUtility.GetPropertyGroupName(sourceBind.propertyName);
                if (!prop.hasChildren)
                {
                    var value = GetKeyValue(prop);
                    if (!float.IsNaN(value))
                    {
                        updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, sourceBind.propertyName));
                        var curve = new AnimationCurve();
                        AddKeyFrameToCurve(curve, (float)time, clip.frameRate, value, prop.propertyType == SerializedPropertyType.Boolean);
                        updateCurves.Add(curve);
                    }
                }
                else
                {
                    // special case because subproperties on color aren't 'visible' so you can't iterate over them
                    if (prop.propertyType == SerializedPropertyType.Color)
                    {
                        updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".r"));
                        updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".g"));
                        updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".b"));
                        updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propName + ".a"));

                        var c = prop.colorValue;
                        for (var i = 0; i < 4; i++)
                        {
                            var curve = new AnimationCurve();
                            AddKeyFrameToCurve(curve, (float)time, clip.frameRate, c[i], prop.propertyType == SerializedPropertyType.Boolean);
                            updateCurves.Add(curve);
                        }
                    }
                    else
                    {
                        prop = prop.Copy();
                        foreach (SerializedProperty cp in prop)
                        {
                            updateBindings.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, cp.propertyPath));
                            var curve = new AnimationCurve();
                            AddKeyFrameToCurve(curve, (float)time, clip.frameRate, GetKeyValue(cp), cp.propertyType == SerializedPropertyType.Boolean);
                            updateCurves.Add(curve);
                        }
                    }
                }
            }

            UpdateEditorCurves(clip, updateBindings, updateCurves);
        }

        public static void RemoveKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
        {
            if (sourceBinding.isPPtrCurve)
            {
                RemoveObjectKey(clip, sourceBinding, time);
            }
            else if (IsRotationKey(sourceBinding))
            {
                RemoveRotationKey(clip, sourceBinding, prop, time);
            }
            else
            {
                RemoveFloatKey(clip, sourceBinding, prop, time);
            }
        }

        public static void RemoveObjectKey(AnimationClip clip, EditorCurveBinding sourceBinding, double time)
        {
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            var curveIndex = Array.IndexOf(info.objectBindings, sourceBinding);
            if (curveIndex >= 0)
            {
                var curve = info.objectCurves[curveIndex];
                var evalIndex = GetKeyframeAtTime(curve, (float)time, clip.frameRate);
                if (evalIndex >= 0)
                {
                    ArrayUtility.RemoveAt(ref curve, evalIndex);
                    AnimationUtility.SetObjectReferenceCurve(clip, sourceBinding, curve.Length == 0 ? null : curve);
                    EditorUtility.SetDirty(clip);
                }
            }
        }

        public static int GetObjectKeyCount(AnimationClip clip, EditorCurveBinding sourceBinding)
        {
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            var curveIndex = Array.IndexOf(info.objectBindings, sourceBinding);
            if (curveIndex >= 0)
            {
                var curve = info.objectCurves[curveIndex];
                return curve.Length;
            }

            return 0;
        }

        static void RemoveRotationKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
        {
            if (prop.propertyType != SerializedPropertyType.Quaternion)
            {
                return;
            }

            var updateCurves = new List<AnimationCurve>();
            var updateBindings = new List<EditorCurveBinding>();

            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            for (var i = 0; i < info.bindings.Length; i++)
            {
                if (sourceBind.type != info.bindings[i].type)
                    continue;

                if (info.bindings[i].propertyName.Contains("localEuler"))
                {
                    updateBindings.Add(info.bindings[i]);
                    updateCurves.Add(info.curves[i]);
                }
            }

            foreach (var c in updateCurves)
            {
                RemoveKeyFrameFromCurve(c, (float)time, clip.frameRate);
            }

            UpdateEditorCurves(clip, updateBindings, updateCurves);
        }

        // Removes the float keys from curves
        static void RemoveFloatKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
        {
            var updateCurves = new List<AnimationCurve>();
            var updateBindings = new List<EditorCurveBinding>();

            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            for (var i = 0; i < info.bindings.Length; i++)
            {
                var binding = info.bindings[i];
                if (binding.type != sourceBind.type)
                    continue;

                SerializedProperty valProp = null;
                var curve = info.curves[i];

                // perfect match on property path, editting a float
                if (prop.propertyPath.Equals(binding.propertyName))
                {
                    valProp = prop;
                }
                // this is a child object
                else if (binding.propertyName.Contains(prop.propertyPath))
                {
                    valProp = prop.serializedObject.FindProperty(binding.propertyName);
                }
                if (valProp != null)
                {
                    RemoveKeyFrameFromCurve(curve, (float)time, clip.frameRate);
                    updateCurves.Add(curve);
                    updateBindings.Add(binding);
                }
            }

            // update the curve. Do this last to not mess with the curve caches we are iterating over
            UpdateEditorCurves(clip, updateBindings, updateCurves);
        }

        static void UpdateEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
        {
            if (curve.keys.Length == 0)
                AnimationUtility.SetEditorCurve(clip, binding, null);
            else
                AnimationUtility.SetEditorCurve(clip, binding, curve);
        }

        static void UpdateEditorCurves(AnimationClip clip, List<EditorCurveBinding> bindings, List<AnimationCurve> curves)
        {
            if (curves.Count == 0)
                return;

            for (var i = 0; i < curves.Count; i++)
            {
                UpdateEditorCurve(clip, bindings[i], curves[i]);
            }
            EditorUtility.SetDirty(clip);
        }

        public static void RemoveCurves(AnimationClip clip, SerializedProperty prop)
        {
            if (clip == null || prop == null)
                return;

            var toRemove = new List<EditorCurveBinding>();
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
            for (var i = 0; i < info.bindings.Length; i++)
            {
                var binding = info.bindings[i];

                // check if we match directly, or with a child object
                if (prop.propertyPath.Equals(binding.propertyName) || binding.propertyName.Contains(prop.propertyPath))
                {
                    toRemove.Add(binding);
                }
            }
            for (int i = 0; i < toRemove.Count; i++)
            {
                AnimationUtility.SetEditorCurve(clip, toRemove[i], null);
            }
        }

        // adds a stepped key frame to the given curve
        public static void AddKeyFrameToCurve(AnimationCurve curve, float time, float framerate, float value, bool stepped)
        {
            var key = new Keyframe();

            bool add = true;
            var keyIndex = GetKeyframeAtTime(curve, time, framerate);
            if (keyIndex != -1)
            {
                add = false;
                key = curve[keyIndex]; // retain the tangents and mode
                curve.RemoveKey(keyIndex);
            }

            key.value = value;
            key.time = GetKeyTime(time, framerate);
            keyIndex = curve.AddKey(key);

            if (stepped)
            {
                AnimationUtility.SetKeyBroken(curve, keyIndex, stepped);
                AnimationUtility.SetKeyLeftTangentMode(curve, keyIndex, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyRightTangentMode(curve, keyIndex, AnimationUtility.TangentMode.Constant);
                key.outTangent = Mathf.Infinity;
                key.inTangent = Mathf.Infinity;
            }
            else if (add)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, keyIndex, AnimationUtility.TangentMode.ClampedAuto);
                AnimationUtility.SetKeyRightTangentMode(curve, keyIndex, AnimationUtility.TangentMode.ClampedAuto);
            }

            if (keyIndex != -1 && !stepped)
            {
                AnimationUtility.UpdateTangentsFromModeSurrounding(curve, keyIndex);
                AnimationUtility.SetKeyBroken(curve, keyIndex, false);
            }
        }

        // Removes a keyframe at the given time from the animation curve
        public static bool RemoveKeyFrameFromCurve(AnimationCurve curve, float time, float framerate)
        {
            var keyIndex = GetKeyframeAtTime(curve, time, framerate);
            if (keyIndex == -1)
                return false;

            curve.RemoveKey(keyIndex);
            return true;
        }

        // gets the value of the key
        public static float GetKeyValue(SerializedProperty prop)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return prop.intValue;
                case SerializedPropertyType.Boolean:
                    return prop.boolValue ? 1.0f : 0.0f;
                case SerializedPropertyType.Float:
                    return prop.floatValue;
                default:
                    Debug.LogError("Could not convert property type " + prop.propertyType.ToString() + " to float");
                    break;
            }
            return float.NaN;
        }

        public static void SetFromKeyValue(SerializedProperty prop, float keyValue)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Float:
                {
                    prop.floatValue = keyValue;
                    return;
                }
                case SerializedPropertyType.Integer:
                {
                    prop.intValue = (int)keyValue;
                    return;
                }
                case SerializedPropertyType.Boolean:
                {
                    prop.boolValue = Math.Abs(keyValue) > 0.001f;
                    return;
                }
            }

            Debug.LogError("Could not convert float to property type " + prop.propertyType.ToString());
        }

        // gets the index of the key, -1 if not found
        public static int GetKeyframeAtTime(AnimationCurve curve, float time, float frameRate)
        {
            var range = 0.5f / frameRate;
            var keys = curve.keys;
            for (var i = 0; i < keys.Length; i++)
            {
                var k = keys[i];
                if (k.time >= time - range && k.time < time + range)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int GetKeyframeAtTime(ObjectReferenceKeyframe[] curve, float time, float frameRate)
        {
            if (curve == null || curve.Length == 0)
                return -1;

            var range = 0.5f / frameRate;
            for (var i = 0; i < curve.Length; i++)
            {
                var t = curve[i].time;
                if (t >= time - range && t < time + range)
                {
                    return i;
                }
            }
            return -1;
        }

        public static float GetKeyTime(float time, float frameRate)
        {
            return Mathf.Round(time * frameRate) / frameRate;
        }

        public static int KeyCompare(float timeA, float timeB, float frameRate)
        {
            if (Mathf.Abs(timeA - timeB) <= 0.5f / frameRate)
                return 0;
            return timeA < timeB ? -1 : 1;
        }

        // Evaluates an object (bool curve)
        public static Object Evaluate(ObjectReferenceKeyframe[] curve, float time)
        {
            return curve[EvaluateIndex(curve, time)].value;
        }

        // returns the index from evaluation
        public static int EvaluateIndex(ObjectReferenceKeyframe[] curve, float time)
        {
            if (curve == null || curve.Length == 0)
                throw new InvalidOperationException("Can not evaluate a PPtr curve with no entries");

            // clamp conditions
            if (time <= curve[0].time)
                return 0;
            if (time >= curve.Last().time)
                return curve.Length - 1;

            // binary search
            var max = curve.Length - 1;
            var min = 0;
            while (max - min > 1)
            {
                var imid = (min + max) / 2;
                if (Mathf.Approximately(curve[imid].time, time))
                    return imid;
                if (curve[imid].time < time)
                    min = imid;
                else if (curve[imid].time > time)
                    max = imid;
            }
            return min;
        }

        // Shifts the animation clip so the time start at 0
        public static void ShiftBySeconds(this AnimationClip clip, float time)
        {
            var floatBindings = AnimationUtility.GetCurveBindings(clip);
            var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            // update the float curves
            foreach (var bind in floatBindings)
            {
                var curve = AnimationUtility.GetEditorCurve(clip, bind);
                var keys = curve.keys;
                for (var i = 0; i < keys.Length; i++)
                    keys[i].time += time;
                curve.keys = keys;
                AnimationUtility.SetEditorCurve(clip, bind, curve);
            }

            // update the PPtr curves
            foreach (var bind in objectBindings)
            {
                var curve = AnimationUtility.GetObjectReferenceCurve(clip, bind);
                for (var i = 0; i < curve.Length; i++)
                    curve[i].time += time;
                AnimationUtility.SetObjectReferenceCurve(clip, bind, curve);
            }

            EditorUtility.SetDirty(clip);
        }

        public static void ScaleTime(this AnimationClip clip, float scale)
        {
            var floatBindings = AnimationUtility.GetCurveBindings(clip);
            var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            // update the float curves
            foreach (var bind in floatBindings)
            {
                var curve = AnimationUtility.GetEditorCurve(clip, bind);
                var keys = curve.keys;
                for (var i = 0; i < keys.Length; i++)
                    keys[i].time *= scale;
                curve.keys = keys.OrderBy(x => x.time).ToArray();
                AnimationUtility.SetEditorCurve(clip, bind, curve);
            }

            // update the PPtr curves
            foreach (var bind in objectBindings)
            {
                var curve = AnimationUtility.GetObjectReferenceCurve(clip, bind);
                for (var i = 0; i < curve.Length; i++)
                    curve[i].time *= scale;
                curve = curve.OrderBy(x => x.time).ToArray();
                AnimationUtility.SetObjectReferenceCurve(clip, bind, curve);
            }

            EditorUtility.SetDirty(clip);
        }

        // Creates an opposing blend curve that matches the given curve to make sure the result is normalized
        public static AnimationCurve CreateMatchingCurve(AnimationCurve curve)
        {
            Keyframe[] keys = curve.keys;

            for (var i = 0; i != keys.Length; i++)
            {
                if (!Single.IsPositiveInfinity(keys[i].inTangent))
                    keys[i].inTangent = -keys[i].inTangent;
                if (!Single.IsPositiveInfinity(keys[i].outTangent))
                    keys[i].outTangent = -keys[i].outTangent;
                keys[i].value = 1.0f - keys[i].value;
            }
            return new AnimationCurve(keys);
        }

        // Sanitizes the keys on an animation to force the property to be normalized
        public static Keyframe[] SanitizeCurveKeys(Keyframe[] keys, bool easeIn)
        {
            if (keys.Length < 2)
            {
                if (easeIn)
                    keys = new[] { new Keyframe(0, 0), new Keyframe(1, 1) };
                else
                    keys = new[] { new Keyframe(0, 1), new Keyframe(1, 0) };
            }
            else if (easeIn)
            {
                keys[0].time = 0;
                keys[keys.Length - 1].time = 1;
                keys[keys.Length - 1].value = 1;
            }
            else
            {
                keys[0].time = 0;
                keys[0].value = 1;
                keys[keys.Length - 1].time = 1;
            }
            return keys;
        }
    }
}
