using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Playable that controls the active state of a GameObject.
    /// </summary>
    public class ActivationControlPlayable : PlayableBehaviour
    {
        /// <summary>
        /// The state of a GameObject's activeness when a PlayableGraph stops.
        /// </summary>
        public enum PostPlaybackState
        {
            /// <summary>
            /// Set the GameObject to active when the PlayableGraph stops.
            /// </summary>
            Active,

            /// <summary>
            /// Set the GameObject to inactive when the [[PlayableGraph]] stops.
            /// </summary>
            Inactive,

            /// <summary>
            /// Revert the GameObject to the active state it was before the [[PlayableGraph]] started.
            /// </summary>
            Revert
        }

        enum InitialState
        {
            Unset,
            Active,
            Inactive
        }

        public GameObject gameObject = null;
        public PostPlaybackState postPlayback = PostPlaybackState.Revert;
        InitialState m_InitialState;

        /// <summary>
        /// Creates a ScriptPlayable with an ActivationControlPlayable behaviour attached
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable</param>
        /// <param name="gameObject">The GameObject that triggered the graph build</param>
        /// <param name="postPlaybackState">The state to leave the gameObject after the graph is stopped</param>
        /// <returns>Returns a playable that controls activation of a game object</returns>
        public static ScriptPlayable<ActivationControlPlayable> Create(PlayableGraph graph, GameObject gameObject, ActivationControlPlayable.PostPlaybackState postPlaybackState)
        {
            if (gameObject == null)
                return ScriptPlayable<ActivationControlPlayable>.Null;

            var handle = ScriptPlayable<ActivationControlPlayable>.Create(graph);
            var playable = handle.GetBehaviour();
            playable.gameObject = gameObject;
            playable.postPlayback = postPlaybackState;

            return handle;
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to Playables.PlayState.Playing.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        /// <param name="info">The information about this frame</param>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (gameObject == null)
                return;

            gameObject.SetActive(true);
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to PlayState.Paused.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        /// <param name="info">The information about this frame</param>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // OnBehaviourPause can be called if the graph is stopped for a variety of reasons
            //  the effectivePlayState will test if the pause is due to the clip being out of bounds
            if (gameObject != null && info.effectivePlayState == PlayState.Paused)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// This function is called during the ProcessFrame phase of the PlayableGraph.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        /// <param name="userData">unused</param>
        public override void ProcessFrame(Playable playable, FrameData info, object userData)
        {
            if (gameObject != null)// && !gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        /// <summary>
        /// This function is called when the PlayableGraph that owns this PlayableBehaviour starts.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        public override void OnGraphStart(Playable playable)
        {
            if (gameObject != null)
            {
                if (m_InitialState == InitialState.Unset)
                    m_InitialState = gameObject.activeSelf ? InitialState.Active : InitialState.Inactive;
            }
        }

        /// <summary>
        /// This function is called when the Playable that owns the PlayableBehaviour is destroyed.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        public override void OnPlayableDestroy(Playable playable)
        {
            if (gameObject == null || m_InitialState == InitialState.Unset)
                return;

            switch (postPlayback)
            {
                case PostPlaybackState.Active:
                    gameObject.SetActive(true);
                    break;

                case PostPlaybackState.Inactive:
                    gameObject.SetActive(false);
                    break;

                case PostPlaybackState.Revert:
                    gameObject.SetActive(m_InitialState == InitialState.Active);
                    break;
            }
        }
    }
}
