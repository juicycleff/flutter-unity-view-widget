using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace UnityEditor.Timeline
{
    struct CurveBindingPair
    {
        public EditorCurveBinding binding;
        public AnimationCurve curve;
        public ObjectReferenceKeyframe[] objectCurve;
    }

    class CurveBindingGroup
    {
        public CurveBindingPair[] curveBindingPairs { get; set; }
        public Vector2 timeRange { get; set; }
        public Vector2 valueRange { get; set; }

        public bool isFloatCurve
        {
            get
            {
                return curveBindingPairs != null && curveBindingPairs.Length > 0 &&
                    curveBindingPairs[0].curve != null;
            }
        }

        public bool isObjectCurve
        {
            get
            {
                return curveBindingPairs != null && curveBindingPairs.Length > 0 &&
                    curveBindingPairs[0].objectCurve != null;
            }
        }

        public int count
        {
            get
            {
                if (curveBindingPairs == null)
                    return 0;
                return curveBindingPairs.Length;
            }
        }
    }

    class AnimationClipCurveInfo
    {
        bool m_CurveDirty = true;
        bool m_KeysDirty = true;

        public bool dirty
        {
            get { return m_CurveDirty; }
            set
            {
                m_CurveDirty = value;
                if (m_CurveDirty)
                {
                    m_KeysDirty = true;
                    if (m_groupings != null)
                        m_groupings.Clear();
                }
            }
        }

        public AnimationCurve[] curves;
        public EditorCurveBinding[] bindings;

        public EditorCurveBinding[] objectBindings;
        public List<ObjectReferenceKeyframe[]> objectCurves;

        Dictionary<string, CurveBindingGroup> m_groupings;

        // to tell whether the cache has changed
        public int version { get; private set; }

        float[] m_KeyTimes;

        Dictionary<EditorCurveBinding, float[]> m_individualBindinsKey;

        public float[] keyTimes
        {
            get
            {
                if (m_KeysDirty || m_KeyTimes == null)
                {
                    RebuildKeyCache();
                }
                return m_KeyTimes;
            }
        }

        public float[] GetCurveTimes(EditorCurveBinding curve)
        {
            return GetCurveTimes(new[] { curve });
        }

        public float[] GetCurveTimes(EditorCurveBinding[] curves)
        {
            if (m_KeysDirty || m_KeyTimes == null)
            {
                RebuildKeyCache();
            }

            var keyTimes = new List<float>();
            for (int i = 0; i < curves.Length; i++)
            {
                var c = curves[i];
                if (m_individualBindinsKey.ContainsKey(c))
                {
                    keyTimes.AddRange(m_individualBindinsKey[c]);
                }
            }
            return keyTimes.ToArray();
        }

        void RebuildKeyCache()
        {
            m_individualBindinsKey = new Dictionary<EditorCurveBinding, float[]>();

            List<float> keys = curves.SelectMany(y => y.keys).Select(z => z.time).ToList();
            for (int i = 0; i < objectCurves.Count; i++)
            {
                var kf = objectCurves[i];
                keys.AddRange(kf.Select(x => x.time));
            }

            for (int b = 0; b < bindings.Count(); b++)
            {
                m_individualBindinsKey.Add(bindings[b], curves[b].keys.Select(k => k.time).Distinct().ToArray());
            }

            m_KeyTimes = keys.OrderBy(x => x).Distinct().ToArray();
            m_KeysDirty = false;
        }

        public void Update(AnimationClip clip)
        {
            List<EditorCurveBinding> postfilter = new List<EditorCurveBinding>();
            var clipBindings = AnimationUtility.GetCurveBindings(clip);
            for (int i = 0; i < clipBindings.Length; i++)
            {
                var bind = clipBindings[i];
                if (!bind.propertyName.Contains("LocalRotation.w"))
                    postfilter.Add(RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(bind, clip));
            }
            bindings = postfilter.ToArray();

            curves = new AnimationCurve[bindings.Length];
            for (int i = 0; i < bindings.Length; i++)
            {
                curves[i] = AnimationUtility.GetEditorCurve(clip, bindings[i]);
            }

            objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            objectCurves = new List<ObjectReferenceKeyframe[]>(objectBindings.Length);
            for (int i = 0; i < objectBindings.Length; i++)
            {
                objectCurves.Add(AnimationUtility.GetObjectReferenceCurve(clip, objectBindings[i]));
            }

            m_CurveDirty = false;
            m_KeysDirty = true;

            version = version + 1;
        }

        public bool GetBindingForCurve(AnimationCurve curve, ref EditorCurveBinding binding)
        {
            for (int i = 0; i < curves.Length; i++)
            {
                if (curve == curves[i])
                {
                    binding = bindings[i];
                    return true;
                }
            }
            return false;
        }

        public AnimationCurve GetCurveForBinding(EditorCurveBinding binding)
        {
            for (int i = 0; i < curves.Length; i++)
            {
                if (binding.Equals(bindings[i]))
                {
                    return curves[i];
                }
            }
            return null;
        }

        public ObjectReferenceKeyframe[] GetObjectCurveForBinding(EditorCurveBinding binding)
        {
            if (objectCurves == null)
                return null;

            for (int i = 0; i < objectCurves.Count; i++)
            {
                if (binding.Equals(objectBindings[i]))
                {
                    return objectCurves[i];
                }
            }
            return null;
        }

        // given a groupID, get the list of curve bindings
        public CurveBindingGroup GetGroupBinding(string groupID)
        {
            if (m_groupings == null)
                m_groupings = new Dictionary<string, CurveBindingGroup>();

            CurveBindingGroup result = null;
            if (!m_groupings.TryGetValue(groupID, out result))
            {
                result = new CurveBindingGroup();
                result.timeRange = new Vector2(float.MaxValue, float.MinValue);
                result.valueRange = new Vector2(float.MaxValue, float.MinValue);
                List<CurveBindingPair> found = new List<CurveBindingPair>();
                for (int i = 0; i < bindings.Length; i++)
                {
                    if (bindings[i].GetGroupID() == groupID)
                    {
                        CurveBindingPair pair = new CurveBindingPair();
                        pair.binding = bindings[i];
                        pair.curve = curves[i];
                        found.Add(pair);

                        for (int k = 0; k < curves[i].keys.Length; k++)
                        {
                            var key = curves[i].keys[k];
                            result.timeRange = new Vector2(Mathf.Min(key.time, result.timeRange.x), Mathf.Max(key.time, result.timeRange.y));
                            result.valueRange = new Vector2(Mathf.Min(key.value, result.valueRange.x), Mathf.Max(key.value, result.valueRange.y));
                        }
                    }
                }
                for (int i = 0; i < objectBindings.Length; i++)
                {
                    if (objectBindings[i].GetGroupID() == groupID)
                    {
                        CurveBindingPair pair = new CurveBindingPair();
                        pair.binding = objectBindings[i];
                        pair.objectCurve = objectCurves[i];
                        found.Add(pair);

                        for (int k = 0; k < objectCurves[i].Length; k++)
                        {
                            var key = objectCurves[i][k];
                            result.timeRange = new Vector2(Mathf.Min(key.time, result.timeRange.x), Mathf.Max(key.time, result.timeRange.y));
                        }
                    }
                }

                result.curveBindingPairs = found.OrderBy(x => AnimationWindowUtility.GetComponentIndex(x.binding.propertyName)).ToArray();

                m_groupings.Add(groupID, result);
            }
            return result;
        }
    }

    // Cache for storing the animation clip data
    class AnimationClipCurveCache
    {
        static AnimationClipCurveCache s_Instance;
        Dictionary<AnimationClip, AnimationClipCurveInfo> m_ClipCache = new Dictionary<AnimationClip, AnimationClipCurveInfo>();
        bool m_IsEnabled;


        public static AnimationClipCurveCache Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new AnimationClipCurveCache();
                }

                return s_Instance;
            }
        }

        public void OnEnable()
        {
            if (!m_IsEnabled)
            {
                AnimationUtility.onCurveWasModified += OnCurveWasModified;
                m_IsEnabled = true;
            }
        }

        public void OnDisable()
        {
            if (m_IsEnabled)
            {
                AnimationUtility.onCurveWasModified -= OnCurveWasModified;
                m_IsEnabled = false;
            }
        }

        // callback when a curve is edited. Force the cache to update next time it's accessed
        void OnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType modification)
        {
            if (modification == AnimationUtility.CurveModifiedType.CurveDeleted)
            {
                m_ClipCache.Remove(clip);
            }
            else
            {
                AnimationClipCurveInfo data;
                if (m_ClipCache.TryGetValue(clip, out data))
                {
                    data.dirty = true;
                }
            }
        }

        public AnimationClipCurveInfo GetCurveInfo(AnimationClip clip)
        {
            AnimationClipCurveInfo data;
            if (clip == null)
                return null;
            if (!m_ClipCache.TryGetValue(clip, out data))
            {
                data = new AnimationClipCurveInfo();
                data.dirty = true;
                m_ClipCache[clip] = data;
            }
            if (data.dirty)
            {
                data.Update(clip);
            }
            return data;
        }
    }

    static class EditorCurveBindingExtension
    {
        // identifier to generate an id thats the same for all curves in the same group
        public static string GetGroupID(this EditorCurveBinding binding)
        {
            return binding.type + AnimationWindowUtility.GetPropertyGroupName(binding.propertyName);
        }
    }


    static class CurveBindingGroupExtensions
    {
        // Extentions to determine curve types
        public static bool IsEnableGroup(this CurveBindingGroup curves)
        {
            return curves.isFloatCurve && curves.count == 1 && curves.curveBindingPairs[0].binding.propertyName == "m_Enabled";
        }

        public static bool IsVectorGroup(this CurveBindingGroup curves)
        {
            if (!curves.isFloatCurve)
                return false;
            if (curves.count <= 1 || curves.count > 4)
                return false;
            char l = curves.curveBindingPairs[0].binding.propertyName.Last();
            return l == 'x' || l == 'y' || l == 'z' || l == 'w';
        }

        public static bool IsColorGroup(this CurveBindingGroup curves)
        {
            if (!curves.isFloatCurve)
                return false;
            if (curves.count != 3 && curves.count != 4)
                return false;
            char l = curves.curveBindingPairs[0].binding.propertyName.Last();
            return l == 'r' || l == 'g' || l == 'b' || l == 'a';
        }

        public static string GetDescription(this CurveBindingGroup group, float t)
        {
            string result = string.Empty;
            if (group.isFloatCurve)
            {
                if (group.count > 1)
                {
                    result += "(" + group.curveBindingPairs[0].curve.Evaluate(t).ToString("0.##");
                    for (int j = 1; j < group.curveBindingPairs.Length; j++)
                    {
                        result += "," + group.curveBindingPairs[j].curve.Evaluate(t).ToString("0.##");
                    }
                    result += ")";
                }
                else
                {
                    result = group.curveBindingPairs[0].curve.Evaluate(t).ToString("0.##");
                }
            }
            else if (group.isObjectCurve)
            {
                Object obj = null;
                if (group.curveBindingPairs[0].objectCurve.Length > 0)
                    obj = CurveEditUtility.Evaluate(group.curveBindingPairs[0].objectCurve, t);
                result = (obj == null ? "None" : obj.name);
            }

            return result;
        }
    }
}
