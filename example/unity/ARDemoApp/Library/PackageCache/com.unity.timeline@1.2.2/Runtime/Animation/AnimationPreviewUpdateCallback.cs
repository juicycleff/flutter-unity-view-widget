using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    class AnimationPreviewUpdateCallback : ITimelineEvaluateCallback
    {
        AnimationPlayableOutput m_Output;
        PlayableGraph m_Graph;
        List<IAnimationWindowPreview> m_PreviewComponents;

        public AnimationPreviewUpdateCallback(AnimationPlayableOutput output)
        {
            m_Output = output;

            Playable playable = m_Output.GetSourcePlayable();
            if (playable.IsValid())
            {
                m_Graph = playable.GetGraph();
            }
        }

        public void Evaluate()
        {
            if (!m_Graph.IsValid())
                return;

            if (m_PreviewComponents == null)
                FetchPreviewComponents();

            foreach (var component in m_PreviewComponents)
            {
                if (component != null)
                {
                    component.UpdatePreviewGraph(m_Graph);
                }
            }
        }

        private void FetchPreviewComponents()
        {
            m_PreviewComponents = new List<IAnimationWindowPreview>();

            var animator = m_Output.GetTarget();
            if (animator == null)
                return;

            var gameObject = animator.gameObject;
            m_PreviewComponents.AddRange(gameObject.GetComponents<IAnimationWindowPreview>());
        }
    }
}
