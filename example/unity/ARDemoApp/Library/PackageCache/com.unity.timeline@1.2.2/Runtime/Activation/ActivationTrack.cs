using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Track that can be used to control the active state of a GameObject.
    /// </summary>
    [Serializable]
    [TrackClipType(typeof(ActivationPlayableAsset))]
    [TrackBindingType(typeof(GameObject))]
    public class ActivationTrack : TrackAsset
    {
        [SerializeField]
        PostPlaybackState m_PostPlaybackState = PostPlaybackState.LeaveAsIs;
        ActivationMixerPlayable m_ActivationMixer;

        /// <summary>
        /// Specify what state to leave the GameObject in after the Timeline has finished playing.
        /// </summary>
        public enum PostPlaybackState
        {
            /// <summary>
            /// Set the GameObject to active.
            /// </summary>
            Active,

            /// <summary>
            /// Set the GameObject to Inactive.
            /// </summary>
            Inactive,

            /// <summary>
            /// Revert the GameObject to the state in was in before the Timeline was playing.
            /// </summary>
            Revert,

            /// <summary>
            /// Leave the GameObject in the state it was when the Timeline was stopped.
            /// </summary>
            LeaveAsIs
        }

        internal override bool CanCompileClips()
        {
            return !hasClips || base.CanCompileClips();
        }

        /// <summary>
        /// Specifies what state to leave the GameObject in after the Timeline has finished playing.
        /// </summary>
        public PostPlaybackState postPlaybackState
        {
            get { return m_PostPlaybackState; }
            set { m_PostPlaybackState = value; UpdateTrackMode(); }
        }

        /// <inheritdoc/>
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ActivationMixerPlayable.Create(graph, inputCount);
            m_ActivationMixer = mixer.GetBehaviour();

            UpdateTrackMode();

            return mixer;
        }

        internal void UpdateTrackMode()
        {
            if (m_ActivationMixer != null)
                m_ActivationMixer.postPlaybackState = m_PostPlaybackState;
        }

        /// <inheritdoc/>
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var gameObject = GetGameObjectBinding(director);
            if (gameObject != null)
            {
                driver.AddFromName(gameObject, "m_IsActive");
            }
        }

        /// <inheritdoc/>
        protected override void OnCreateClip(TimelineClip clip)
        {
            clip.displayName = "Active";
            base.OnCreateClip(clip);
        }
    }
}
