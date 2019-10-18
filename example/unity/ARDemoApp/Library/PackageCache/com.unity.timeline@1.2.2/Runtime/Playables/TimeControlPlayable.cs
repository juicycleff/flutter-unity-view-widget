using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A PlayableBehaviour that manages a component that implements the ITimeControl interface
    /// </summary>
    public class TimeControlPlayable : PlayableBehaviour
    {
        ITimeControl m_timeControl;

        bool m_started;

        /// <summary>
        /// Creates a Playable with a TimeControlPlayable behaviour attached
        /// </summary>
        /// <param name="graph">The PlayableGraph to inject the Playable into.</param>
        /// <param name="timeControl"></param>
        /// <returns></returns>
        public static ScriptPlayable<TimeControlPlayable> Create(PlayableGraph graph, ITimeControl timeControl)
        {
            if (timeControl == null)
                return ScriptPlayable<TimeControlPlayable>.Null;

            var handle = ScriptPlayable<TimeControlPlayable>.Create(graph);
            handle.GetBehaviour().Initialize(timeControl);
            return handle;
        }

        /// <summary>
        /// Initializes the behaviour
        /// </summary>
        /// <param name="timeControl">Component that implements the ITimeControl interface</param>
        public void Initialize(ITimeControl timeControl)
        {
            m_timeControl = timeControl;
        }

        /// <summary>
        /// This function is called during the PrepareFrame phase of the PlayableGraph.
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            Debug.Assert(m_started, "PrepareFrame has been called without OnControlTimeStart being called first.");
            if (m_timeControl != null)
                m_timeControl.SetTime(playable.GetTime());
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to Playables.PlayState.Playing.
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (m_timeControl == null)
                return;

            if (!m_started)
            {
                m_timeControl.OnControlTimeStart();
                m_started = true;
            }
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to PlayState.Paused.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (m_timeControl == null)
                return;

            if (m_started)
            {
                m_timeControl.OnControlTimeStop();
                m_started = false;
            }
        }
    }
}
