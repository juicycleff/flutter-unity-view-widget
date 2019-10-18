using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    // Does a post processing of the weights on an animation track to properly normalize
    // the mixer weights so that blending does not bring default poses and subtracks, layers and
    // layer graphs blend correctly
    class AnimationOutputWeightProcessor : ITimelineEvaluateCallback
    {
        struct WeightInfo
        {
            public Playable mixer;
            public Playable parentMixer;
            public int port;
        }

        AnimationPlayableOutput m_Output;
        AnimationMotionXToDeltaPlayable m_MotionXPlayable;
        readonly List<WeightInfo> m_Mixers = new List<WeightInfo>();

        public AnimationOutputWeightProcessor(AnimationPlayableOutput output)
        {
            m_Output = output;
            output.SetWeight(0);
            FindMixers();
        }

        void FindMixers()
        {
            var playable = m_Output.GetSourcePlayable();
            var outputPort = m_Output.GetSourceOutputPort();

            m_Mixers.Clear();
            // only write the final output in playmode. it should always be 1 in editor because we blend to the defaults
            FindMixers(playable, outputPort, playable.GetInput(outputPort));
        }

        // Recursively accumulates mixers.
        void FindMixers(Playable parent, int port, Playable node)
        {
            if (!node.IsValid())
                return;

            var type = node.GetPlayableType();
            if (type == typeof(AnimationMixerPlayable) || type == typeof(AnimationLayerMixerPlayable))
            {
                // use post fix traversal so children come before parents
                int subCount = node.GetInputCount();
                for (int j = 0; j < subCount; j++)
                {
                    FindMixers(node, j, node.GetInput(j));
                }

                // if we encounter a layer mixer, we assume there is nesting occuring
                //  and we modulate the weight instead of overwriting it.
                var weightInfo = new WeightInfo
                {
                    parentMixer = parent,
                    mixer = node,
                    port = port,
                };
                m_Mixers.Add(weightInfo);
            }
            else
            {
                var count = node.GetInputCount();
                for (var i = 0; i < count; i++)
                {
                    FindMixers(parent, port, node.GetInput(i));
                }
            }
        }

        public void Evaluate()
        {
            float weight = 1;
            m_Output.SetWeight(1);
            for (int i = 0; i < m_Mixers.Count; i++)
            {
                var mixInfo = m_Mixers[i];
                weight = WeightUtility.NormalizeMixer(mixInfo.mixer);
                mixInfo.parentMixer.SetInputWeight(mixInfo.port, weight);
            }
            
            // only write the final weight in player/playmode. In editor, we are blending to the appropriate defaults
            // the last mixer in the list is the final blend, since the list is composed post-order.
            if (Application.isPlaying)
                m_Output.SetWeight(weight);
        }
    }
}
