using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// This class is deprecated. It is recommended to use Playable Asset and Playable Behaviour derived classes instead.
    /// </summary>
    [Serializable]
    [Obsolete("For best performance use PlayableAsset and PlayableBehaviour.")]
    public class BasicPlayableBehaviour : ScriptableObject, IPlayableAsset, IPlayableBehaviour
    {
        public BasicPlayableBehaviour() {}

        /// <summary>
        /// The playback duration in seconds of the instantiated Playable.
        /// </summary>
        public virtual double duration { get { return PlayableBinding.DefaultDuration; } }

        /// <summary>
        ///A description of the outputs of the instantiated Playable.
        /// </summary>
        public virtual IEnumerable<PlayableBinding> outputs { get { return PlayableBinding.None; } }

        /// <summary>
        ///   <para>This function is called when the PlayableGraph that owns this PlayableBehaviour starts.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        public virtual void OnGraphStart(Playable playable) {}

        /// <summary>
        ///   <para>This function is called when the PlayableGraph that owns this PlayableBehaviour stops.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        public virtual void OnGraphStop(Playable playable)  {}

        /// <summary>
        ///   <para>This function is called when the Playable that owns the PlayableBehaviour is created.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        public virtual void OnPlayableCreate(Playable playable) {}

        /// <summary>
        ///   <para>This function is called when the Playable that owns the PlayableBehaviour is destroyed.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        public virtual void OnPlayableDestroy(Playable playable) {}

        /// <summary>
        ///   <para>This function is called when the Playable play state is changed to Playables.PlayState.Playing.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public virtual void OnBehaviourPlay(Playable playable, FrameData info) {}

        /// <summary>
        ///   <para>This function is called when the Playable play state is changed to Playables.PlayState.Paused.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public virtual void OnBehaviourPause(Playable playable, FrameData info) {}

        /// <summary>
        ///   <para>This function is called during the PrepareFrame phase of the PlayableGraph.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public virtual void PrepareFrame(Playable playable, FrameData info) {}

        /// <summary>
        ///   <para>This function is called during the ProcessFrame phase of the PlayableGraph.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        /// <param name="playerData">The user data of the ScriptPlayableOutput that initiated the process pass.</param>
        public virtual void ProcessFrame(Playable playable, FrameData info, object playerData) {}

        /// <summary>
        /// Implement this method to have your asset inject playables into the given graph.
        /// </summary>
        /// <param name="graph">The graph to inject playables into.</param>
        /// <param name="owner">The game object which initiated the build.</param>
        /// <returns>The playable injected into the graph, or the root playable if multiple playables are injected.</returns>
        public virtual Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<BasicPlayableBehaviour>.Create(graph, this);
        }
    }
}
