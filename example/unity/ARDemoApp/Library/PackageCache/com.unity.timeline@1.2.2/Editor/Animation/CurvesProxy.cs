using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    class CurvesProxy : ICurvesOwner
    {
        public AnimationClip curves
        {
            get { return proxyCurves != null ? proxyCurves : m_OriginalOwner.curves; }
        }

        public bool hasCurves
        {
            get { return m_IsAnimatable || m_OriginalOwner.hasCurves; }
        }

        public double duration
        {
            get { return m_OriginalOwner.duration; }
        }

        public string defaultCurvesName
        {
            get { return m_OriginalOwner.defaultCurvesName; }
        }

        public UnityObject asset
        {
            get { return m_OriginalOwner.asset; }
        }

        public UnityObject assetOwner
        {
            get { return m_OriginalOwner.assetOwner; }
        }

        public TrackAsset targetTrack
        {
            get { return m_OriginalOwner.targetTrack; }
        }

        readonly ICurvesOwner m_OriginalOwner;
        readonly bool m_IsAnimatable;
        readonly Dictionary<EditorCurveBinding, SerializedProperty> m_PropertiesMap = new Dictionary<EditorCurveBinding, SerializedProperty>();
        int m_ProxyIsRebuilding = 0;

        AnimationClip m_ProxyCurves;
        AnimationClip proxyCurves
        {
            get
            {
                if (!m_IsAnimatable) return null;

                if (m_ProxyCurves == null)
                    RebuildProxyCurves();

                return m_ProxyCurves;
            }
        }

        List<SerializedProperty> m_AllAnimatableParameters;
        List<SerializedProperty> allAnimatableParameters
        {
            get
            {
                var so = AnimatedParameterUtility.GetSerializedPlayableAsset(m_OriginalOwner.asset);
                if (so == null)
                    return null;

                so.UpdateIfRequiredOrScript();

                if (m_AllAnimatableParameters == null)
                    m_AllAnimatableParameters = m_OriginalOwner.GetAllAnimatableParameters().ToList();

                return m_AllAnimatableParameters;
            }
        }

        public CurvesProxy([NotNull] ICurvesOwner originalOwner)
        {
            m_OriginalOwner = originalOwner;
            m_IsAnimatable = originalOwner.HasAnyAnimatableParameters();

            RebuildProxyCurves();
        }

        public void CreateCurves(string curvesClipName)
        {
            m_OriginalOwner.CreateCurves(curvesClipName);
        }

        public void ConfigureCurveWrapper(CurveWrapper wrapper)
        {
            var color = CurveUtility.GetPropertyColor(wrapper.binding.propertyName);
            wrapper.color = color;

            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            wrapper.wrapColorMultiplier = Color.HSVToRGB(h, s * 0.33f, v * 1.15f);

            var curve = AnimationUtility.GetEditorCurve(proxyCurves, wrapper.binding);

            wrapper.renderer = new NormalCurveRenderer(curve);

            // Use curve length instead of animation clip length
            wrapper.renderer.SetCustomRange(0.0f, curve.keys.Last().time);
        }

        public void RebuildCurves()
        {
            RebuildProxyCurves();
        }

        public void UpdateCurves(List<CurveWrapper> updatedCurves)
        {
            if (m_ProxyIsRebuilding > 0)
                return;

            Undo.RegisterCompleteObjectUndo(m_OriginalOwner.asset, "Edit Clip Curve");

            if (m_OriginalOwner.curves != null)
                Undo.RegisterCompleteObjectUndo(m_OriginalOwner.curves, "Edit Clip Curve");

            foreach (var curve in updatedCurves)
            {
                UpdateCurve(curve.binding, curve.curve);
            }

            AnimatedParameterUtility.UpdateSerializedPlayableAsset(m_OriginalOwner.asset);
        }

        void UpdateCurve(EditorCurveBinding binding, AnimationCurve curve)
        {
            ApplyConstraints(binding, curve);

            if (curve.length == 0)
            {
                HandleAllKeysDeleted(binding);
            }
            else if (curve.length == 1)
            {
                HandleConstantCurveValueChanged(binding, curve);
            }
            else
            {
                HandleCurveUpdated(binding, curve);
            }
        }

        void ApplyConstraints(EditorCurveBinding binding, AnimationCurve curve)
        {
            if (curve.length == 0)
                return;

            var curveUpdated = false;

            var property = m_PropertiesMap[binding];
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                TimelineAnimationUtilities.ConstrainCurveToBooleanValues(curve);
                curveUpdated = true;
            }
            else
            {
                var range = AnimatedParameterUtility.GetAttributeForProperty<RangeAttribute>(property);
                if (range != null)
                {
                    TimelineAnimationUtilities.ConstrainCurveToRange(curve, range.min, range.max);
                    curveUpdated = true;
                }
            }

            if (!curveUpdated)
                return;

            using (new RebuildGuard(this))
            {
                AnimationUtility.SetEditorCurve(m_ProxyCurves, binding, curve);
            }
        }

        void HandleCurveUpdated(EditorCurveBinding binding, AnimationCurve updatedCurve)
        {
            if (!m_OriginalOwner.hasCurves)
                m_OriginalOwner.CreateCurves(null);

            AnimationUtility.SetEditorCurve(m_OriginalOwner.curves, binding, updatedCurve);
        }

        void HandleConstantCurveValueChanged(EditorCurveBinding binding, AnimationCurve updatedCurve)
        {
            var prop = m_PropertiesMap[binding];
            if (prop == null)
                return;

            Undo.RegisterCompleteObjectUndo(prop.serializedObject.targetObject, "Edit Clip Curve");
            prop.serializedObject.UpdateIfRequiredOrScript();
            CurveEditUtility.SetFromKeyValue(prop, updatedCurve.keys[0].value);
            prop.serializedObject.ApplyModifiedProperties();
        }

        void HandleAllKeysDeleted(EditorCurveBinding binding)
        {
            if (m_OriginalOwner.hasCurves)
            {
                // Remove curve from original asset
                AnimationUtility.SetEditorCurve(m_OriginalOwner.curves, binding, null);
                m_OriginalOwner.SanitizeCurvesData();
            }

            // Ensure proxy still has constant value
            RebuildProxyCurves();
        }

        void RebuildProxyCurves()
        {
            if (!m_IsAnimatable)
                return;

            using (new RebuildGuard(this))
            {
                if (m_ProxyCurves == null)
                {
                    m_ProxyCurves = new AnimationClip
                    {
                        legacy = true,
                        name = "Constant Curves",
                        hideFlags = HideFlags.HideAndDontSave,
                        frameRate = m_OriginalOwner.targetTrack.timelineAsset == null
                            ? TimelineAsset.EditorSettings.kDefaultFps
                            : m_OriginalOwner.targetTrack.timelineAsset.editorSettings.fps
                    };
                }
                else
                {
                    m_ProxyCurves.ClearCurves();
                }

                m_OriginalOwner.SanitizeCurvesData();
                AnimatedParameterUtility.UpdateSerializedPlayableAsset(m_OriginalOwner.asset);

                foreach (var param in allAnimatableParameters)
                    CreateProxyCurve(param, m_ProxyCurves, m_OriginalOwner.asset, param.propertyPath);

                AnimationClipCurveCache.Instance.GetCurveInfo(m_ProxyCurves).dirty = true;
            }
        }

        void CreateProxyCurve(SerializedProperty prop, AnimationClip clip, UnityObject owner, string propertyName)
        {
            var binding = AnimatedParameterUtility.GetCurveBinding(owner, propertyName);

            var originalCurve = m_OriginalOwner.hasCurves
                ? AnimationUtility.GetEditorCurve(m_OriginalOwner.curves, binding)
                : null;

            if (originalCurve != null)
            {
                AnimationUtility.SetEditorCurve(clip, binding, originalCurve);
            }
            else
            {
                var curve = new AnimationCurve();

                CurveEditUtility.AddKeyFrameToCurve(
                    curve, 0.0f, clip.frameRate, CurveEditUtility.GetKeyValue(prop),
                    prop.propertyType == SerializedPropertyType.Boolean);

                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }

            m_PropertiesMap[binding] = prop;
        }

        class RebuildGuard : IDisposable
        {
            CurvesProxy m_Owner;

            public RebuildGuard(CurvesProxy owner)
            {
                m_Owner = owner;
                m_Owner.m_ProxyIsRebuilding++;
            }

            public void Dispose()
            {
                m_Owner.m_ProxyIsRebuilding--;
                m_Owner = null;
            }
        }
    }
}
