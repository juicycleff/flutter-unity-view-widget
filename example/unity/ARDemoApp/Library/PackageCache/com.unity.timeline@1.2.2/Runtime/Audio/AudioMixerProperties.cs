using System;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    [Serializable]
    class AudioMixerProperties : PlayableBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;

        [Range(-1.0f, 1.0f)]
        public float stereoPan = 0.0f;

        [Range(0.0f, 1.0f)]
        public float spatialBlend = 0.0f;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (!playable.IsValid() || !playable.IsPlayableOfType<AudioMixerPlayable>())
                return;

            var inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; ++i)
            {
                if (playable.GetInputWeight(i) > 0.0f)
                {
                    var input = playable.GetInput(i);

                    if (input.IsValid() && input.IsPlayableOfType<AudioClipPlayable>())
                    {
                        var audioClipPlayable = (AudioClipPlayable)input;
                        var audioClipProperties = input.GetHandle().GetObject<AudioClipProperties>();

                        audioClipPlayable.SetVolume(Mathf.Clamp01(volume * audioClipProperties.volume));
                        audioClipPlayable.SetStereoPan(Mathf.Clamp(stereoPan, -1.0f, 1.0f));
                        audioClipPlayable.SetSpatialBlend(Mathf.Clamp01(spatialBlend));
                    }
                }
            }
        }
    }
}
