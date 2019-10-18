using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    abstract class CurveDataSource
    {
        public static CurveDataSource Create(IRowGUI trackGUI)
        {
            if (trackGUI.asset is AnimationTrack)
                return new InfiniteClipCurveDataSource(trackGUI);

            return new TrackParametersCurveDataSource(trackGUI);
        }

        public static CurveDataSource Create(TimelineClipGUI clipGUI)
        {
            if (clipGUI.clip.animationClip != null)
                return new ClipAnimationCurveDataSource(clipGUI);

            return new ClipParametersCurveDataSource(clipGUI);
        }

        int? m_ID = null;
        public int id
        {
            get
            {
                if (!m_ID.HasValue)
                    m_ID = CreateHashCode();

                return m_ID.Value;
            }
        }

        readonly IRowGUI m_TrackGUI;
        protected IRowGUI trackGUI { get { return m_TrackGUI; } }

        protected CurveDataSource(IRowGUI trackGUI)
        {
            m_TrackGUI = trackGUI;
        }

        public abstract AnimationClip animationClip { get; }
        public abstract float start { get; }
        public abstract float timeScale { get; }
        public abstract string groupingName { get; }
        public virtual void UpdateCurves(List<CurveWrapper> updatedCurves) {}
        public virtual void RebuildCurves() {}  // Only necessary when using proxies

        public Rect GetBackgroundRect(WindowState state)
        {
            var trackRect = m_TrackGUI.boundingRect;
            return new Rect(
                state.timeAreaTranslation.x + trackRect.xMin,
                trackRect.y,
                (float)state.editSequence.asset.duration * state.timeAreaScale.x,
                trackRect.height
            );
        }

        public List<CurveWrapper> GenerateWrappers(List<EditorCurveBinding> bindings)
        {
            var wrappers = new List<CurveWrapper>(bindings.Count);
            int curveWrapperId = 0;

            foreach (EditorCurveBinding b in bindings)
            {
                // General configuration
                var wrapper = new CurveWrapper
                {
                    id = curveWrapperId++,
                    binding = b,
                    groupId = -1,
                    hidden = false,
                    readOnly = false,
                    getAxisUiScalarsCallback = () => new Vector2(1, 1)
                };

                // Specific configuration
                ConfigureCurveWrapper(wrapper);

                wrappers.Add(wrapper);
            }

            return wrappers;
        }

        protected virtual void ConfigureCurveWrapper(CurveWrapper wrapper)
        {
            wrapper.color = CurveUtility.GetPropertyColor(wrapper.binding.propertyName);
            wrapper.renderer = new NormalCurveRenderer(AnimationUtility.GetEditorCurve(animationClip, wrapper.binding));
            wrapper.renderer.SetCustomRange(0.0f, animationClip.length);
        }

        protected virtual int CreateHashCode()
        {
            return m_TrackGUI.asset.GetHashCode();
        }
    }

    class ClipAnimationCurveDataSource : CurveDataSource
    {
        static readonly string k_GroupingName = L10n.Tr("Animated Values");

        readonly TimelineClipGUI m_ClipGUI;

        public ClipAnimationCurveDataSource(TimelineClipGUI clipGUI) : base(clipGUI.parent)
        {
            m_ClipGUI = clipGUI;
        }

        public override AnimationClip animationClip
        {
            get { return m_ClipGUI.clip.animationClip; }
        }

        public override float start
        {
            get { return (float)m_ClipGUI.clip.FromLocalTimeUnbound(0.0); }
        }

        public override float timeScale
        {
            get { return (float)m_ClipGUI.clip.timeScale; }
        }

        public override string groupingName
        {
            get { return k_GroupingName; }
        }

        protected override int CreateHashCode()
        {
            return base.CreateHashCode().CombineHash(m_ClipGUI.clip.GetHashCode());
        }
    }

    class ClipParametersCurveDataSource : CurveDataSource
    {
        static readonly string k_GroupingName = L10n.Tr("Clip Properties");

        readonly TimelineClipGUI m_ClipGUI;
        readonly CurvesProxy m_CurvesProxy;

        public ClipParametersCurveDataSource(TimelineClipGUI clipGUI) : base(clipGUI.parent)
        {
            m_ClipGUI = clipGUI;
            m_CurvesProxy = new CurvesProxy(clipGUI.clip);
        }

        public override AnimationClip animationClip
        {
            get { return m_CurvesProxy.curves; }
        }

        public override float start
        {
            get { return (float)m_ClipGUI.clip.FromLocalTimeUnbound(0.0); }
        }

        public override float timeScale
        {
            get { return (float)m_ClipGUI.clip.timeScale; }
        }

        public override string groupingName
        {
            get { return k_GroupingName; }
        }

        public override void UpdateCurves(List<CurveWrapper> updatedCurves)
        {
            m_CurvesProxy.UpdateCurves(updatedCurves);
        }

        public override void RebuildCurves()
        {
            m_CurvesProxy.RebuildCurves();
        }

        protected override void ConfigureCurveWrapper(CurveWrapper wrapper)
        {
            m_CurvesProxy.ConfigureCurveWrapper(wrapper);
        }

        protected override int CreateHashCode()
        {
            return base.CreateHashCode().CombineHash(m_ClipGUI.clip.GetHashCode());
        }
    }

    class InfiniteClipCurveDataSource : CurveDataSource
    {
        static readonly string k_GroupingName = L10n.Tr("Animated Values");

        readonly AnimationTrack m_AnimationTrack;

        public InfiniteClipCurveDataSource(IRowGUI trackGui) : base(trackGui)
        {
            m_AnimationTrack = trackGui.asset as AnimationTrack;
        }

        public override AnimationClip animationClip
        {
            get { return m_AnimationTrack.infiniteClip; }
        }

        public override float start
        {
            get { return 0.0f; }
        }

        public override float timeScale
        {
            get { return 1.0f; }
        }

        public override string groupingName
        {
            get { return k_GroupingName; }
        }
    }

    class TrackParametersCurveDataSource : CurveDataSource
    {
        static readonly string k_GroupingName = L10n.Tr("Track Properties");

        readonly CurvesProxy m_CurvesProxy;

        public TrackParametersCurveDataSource(IRowGUI trackGui) : base(trackGui)
        {
            m_CurvesProxy = new CurvesProxy(trackGui.asset);
        }

        public override AnimationClip animationClip
        {
            get { return m_CurvesProxy.curves; }
        }

        public override float start
        {
            get { return 0.0f; }
        }

        public override float timeScale
        {
            get { return 1.0f; }
        }

        public override string groupingName
        {
            get { return k_GroupingName; }
        }

        public override void UpdateCurves(List<CurveWrapper> updatedCurves)
        {
            m_CurvesProxy.UpdateCurves(updatedCurves);
        }

        public override void RebuildCurves()
        {
            m_CurvesProxy.RebuildCurves();
        }

        protected override void ConfigureCurveWrapper(CurveWrapper wrapper)
        {
            m_CurvesProxy.ConfigureCurveWrapper(wrapper);
        }
    }
}
