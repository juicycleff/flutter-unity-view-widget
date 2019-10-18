using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    class ActivationMixerPlayable : PlayableBehaviour
    {
        ActivationTrack.PostPlaybackState m_PostPlaybackState;
        bool m_BoundGameObjectInitialStateIsActive;

        private GameObject m_BoundGameObject;


        public static ScriptPlayable<ActivationMixerPlayable> Create(PlayableGraph graph, int inputCount)
        {
            return ScriptPlayable<ActivationMixerPlayable>.Create(graph, inputCount);
        }

        public ActivationTrack.PostPlaybackState postPlaybackState
        {
            get { return m_PostPlaybackState; }
            set { m_PostPlaybackState = value; }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            if (m_BoundGameObject == null)
                return;

            switch (m_PostPlaybackState)
            {
                case ActivationTrack.PostPlaybackState.Active:
                    m_BoundGameObject.SetActive(true);
                    break;
                case ActivationTrack.PostPlaybackState.Inactive:
                    m_BoundGameObject.SetActive(false);
                    break;
                case ActivationTrack.PostPlaybackState.Revert:
                    m_BoundGameObject.SetActive(m_BoundGameObjectInitialStateIsActive);
                    break;
                case ActivationTrack.PostPlaybackState.LeaveAsIs:
                default:
                    break;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_BoundGameObject == null)
            {
                m_BoundGameObject = playerData as GameObject;
                m_BoundGameObjectInitialStateIsActive = m_BoundGameObject != null && m_BoundGameObject.activeSelf;
            }

            if (m_BoundGameObject == null)
                return;

            int inputCount = playable.GetInputCount();
            bool hasInput = false;
            for (int i = 0; i < inputCount; i++)
            {
                if (playable.GetInputWeight(i) > 0)
                {
                    hasInput = true;
                    break;
                }
            }

            m_BoundGameObject.SetActive(hasInput);
        }
    }
}
