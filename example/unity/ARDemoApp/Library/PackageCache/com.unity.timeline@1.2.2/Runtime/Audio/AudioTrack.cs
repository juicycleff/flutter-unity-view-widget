using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A Timeline track that can play AudioClips.
    /// </summary>
    [Serializable]
    [TrackClipType(typeof(AudioPlayableAsset), false)]
    [TrackBindingType(typeof(AudioSource))]
    public class AudioTrack : TrackAsset
    {
        [SerializeField]
        AudioMixerProperties m_TrackProperties = new AudioMixerProperties();

#if UNITY_EDITOR
        Playable m_LiveMixerPlayable = Playable.Null;

#endif

        /// <summary>
        /// Create an TimelineClip for playing an AudioClip on this track.
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        /// <returns>A TimelineClip with an AudioPlayableAsset asset.</returns>
        public TimelineClip CreateClip(AudioClip clip)
        {
            if (clip == null)
                return null;

            var newClip = CreateDefaultClip();

            var audioAsset = newClip.asset as AudioPlayableAsset;
            if (audioAsset != null)
                audioAsset.clip = clip;

            newClip.duration = clip.length;
            newClip.displayName = clip.name;

            return newClip;
        }

        internal override Playable CompileClips(PlayableGraph graph, GameObject go, IList<TimelineClip> timelineClips, IntervalTree<RuntimeElement> tree)
        {
            var clipBlender = AudioMixerPlayable.Create(graph, timelineClips.Count);

#if UNITY_EDITOR
            clipBlender.GetHandle().SetScriptInstance(m_TrackProperties.Clone());
            m_LiveMixerPlayable = clipBlender;
#else
            if (hasCurves)
                clipBlender.GetHandle().SetScriptInstance(m_TrackProperties.Clone());
#endif

            for (int i = 0; i < timelineClips.Count; i++)
            {
                var c = timelineClips[i];
                var asset = c.asset as PlayableAsset;
                if (asset == null)
                    continue;

                var buffer = 0.1f;
                var audioAsset = c.asset as AudioPlayableAsset;
                if (audioAsset != null)
                    buffer = audioAsset.bufferingTime;

                var source = asset.CreatePlayable(graph, go);
                if (!source.IsValid())
                    continue;

                if (source.IsPlayableOfType<AudioClipPlayable>())
                {
                    // Enforce initial values on all clips
                    var audioClipPlayable = (AudioClipPlayable)source;
                    var audioClipProperties = audioClipPlayable.GetHandle().GetObject<AudioClipProperties>();

                    audioClipPlayable.SetVolume(Mathf.Clamp01(m_TrackProperties.volume * audioClipProperties.volume));
                    audioClipPlayable.SetStereoPan(Mathf.Clamp(m_TrackProperties.stereoPan, -1.0f, 1.0f));
                    audioClipPlayable.SetSpatialBlend(Mathf.Clamp01(m_TrackProperties.spatialBlend));
                }

                tree.Add(new ScheduleRuntimeClip(c, source, clipBlender, buffer));
                graph.Connect(source, 0, clipBlender, i);
                source.SetSpeed(c.timeScale);
                source.SetDuration(c.extrapolatedDuration);
                clipBlender.SetInputWeight(source, 1.0f);
            }

            ConfigureTrackAnimation(tree, go, clipBlender);

            return clipBlender;
        }

        /// <inheritdoc/>
        public override IEnumerable<PlayableBinding> outputs
        {
            get { yield return AudioPlayableBinding.Create(name, this); }
        }

#if UNITY_EDITOR
        internal void LiveLink()
        {
            if (!m_LiveMixerPlayable.IsValid())
                return;

            var audioMixerProperties = m_LiveMixerPlayable.GetHandle().GetObject<AudioMixerProperties>();

            if (audioMixerProperties == null)
                return;

            audioMixerProperties.volume = m_TrackProperties.volume;
            audioMixerProperties.stereoPan = m_TrackProperties.stereoPan;
            audioMixerProperties.spatialBlend = m_TrackProperties.spatialBlend;
        }

#endif

        void OnValidate()
        {
            m_TrackProperties.volume = Mathf.Clamp01(m_TrackProperties.volume);
            m_TrackProperties.stereoPan = Mathf.Clamp(m_TrackProperties.stereoPan, -1.0f, 1.0f);
            m_TrackProperties.spatialBlend = Mathf.Clamp01(m_TrackProperties.spatialBlend);
        }
    }
}
