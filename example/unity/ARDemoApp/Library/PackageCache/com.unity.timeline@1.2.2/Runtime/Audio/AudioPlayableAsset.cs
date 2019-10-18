using System;
using System.Collections.Generic;
using UnityEngine.Audio;
#if UNITY_EDITOR
using System.ComponentModel;
#endif
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// PlayableAsset wrapper for an AudioClip in Timeline.
    /// </summary>
    [Serializable]
#if UNITY_EDITOR
    [DisplayName("Audio Clip")]
#endif
    public class AudioPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] AudioClip m_Clip;
#pragma warning disable 649 //Field is never assigned to and will always have its default value
        [SerializeField] bool m_Loop;
        [SerializeField, HideInInspector] float m_bufferingTime = 0.1f;
        [SerializeField] AudioClipProperties m_ClipProperties = new AudioClipProperties();

        // the amount of time to give the clip to load prior to it's start time
        internal float bufferingTime
        {
            get { return m_bufferingTime;  }
            set { m_bufferingTime = value; }
        }

#if UNITY_EDITOR
        Playable m_LiveClipPlayable = Playable.Null;

#endif

        /// <summary>
        /// The audio clip to be played
        /// </summary>
        public AudioClip clip
        {
            get { return m_Clip; }
            set { m_Clip = value; }
        }

        /// <summary>
        /// Whether the audio clip loops.
        /// </summary>
        /// <remarks>
        /// Use this to loop the audio clip when the duration of the timeline clip exceeds that of the audio clip.
        /// </remarks>
        public bool loop
        {
            get { return m_Loop; }
            set { m_Loop = value; }
        }

        /// <summary>
        /// Returns the duration required to play the audio clip exactly once
        /// </summary>
        public override double duration
        {
            get
            {
                if (m_Clip == null)
                    return base.duration;

                // use this instead of length to avoid rounding precision errors,
                return (double)m_Clip.samples / m_Clip.frequency;
            }
        }

        /// <summary>
        /// Returns a description of the PlayableOutputs that may be created for this asset.
        /// </summary>
        public override IEnumerable<PlayableBinding> outputs
        {
            get { yield return AudioPlayableBinding.Create(name, this); }
        }

        /// <summary>
        /// Creates the root of a Playable subgraph to play the audio clip.
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable</param>
        /// <param name="go">The GameObject that triggered the graph build</param>
        /// <returns>The root playable of the subgraph</returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            if (m_Clip == null)
                return Playable.Null;

            var audioClipPlayable = AudioClipPlayable.Create(graph, m_Clip, m_Loop);
            audioClipPlayable.GetHandle().SetScriptInstance(m_ClipProperties.Clone());

#if UNITY_EDITOR
            m_LiveClipPlayable = audioClipPlayable;
#endif

            return audioClipPlayable;
        }

        /// <summary>
        /// Returns the capabilities of TimelineClips that contain an AudioPlayableAsset
        /// </summary>
        public ClipCaps clipCaps
        {
            get
            {
                return ClipCaps.ClipIn |
                    ClipCaps.SpeedMultiplier |
                    ClipCaps.Blending |
                    (m_Loop ? ClipCaps.Looping : ClipCaps.None);
            }
        }

#if UNITY_EDITOR
        internal void LiveLink()
        {
            if (!m_LiveClipPlayable.IsValid())
                return;

            var audioMixerProperties = m_LiveClipPlayable.GetHandle().GetObject<AudioClipProperties>();

            if (audioMixerProperties == null)
                return;

            audioMixerProperties.volume = m_ClipProperties.volume;
        }

#endif
    }
}
