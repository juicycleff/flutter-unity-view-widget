using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    static class WeightUtility
    {
        // Given a mixer, normalizes the mixer if required
        //  returns the output weight that should be applied to the mixer as input
        public static float NormalizeMixer(Playable mixer)
        {
            if (!mixer.IsValid())
                return 0;
            int count = mixer.GetInputCount();
            float weight = 0.0f;
            for (int c = 0; c < count; c++)
            {
                weight += mixer.GetInputWeight(c);
            }

            if (weight > Mathf.Epsilon && weight < 1)
            {
                for (int c = 0; c < count; c++)
                {
                    mixer.SetInputWeight(c, mixer.GetInputWeight(c) / weight);
                }
            }
            return Mathf.Clamp01(weight);
        }
    }
}
