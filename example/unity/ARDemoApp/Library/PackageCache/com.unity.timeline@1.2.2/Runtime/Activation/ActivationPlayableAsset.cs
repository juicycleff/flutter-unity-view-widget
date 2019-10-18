#if UNITY_EDITOR
using System.ComponentModel;
#endif
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Playable Asset class for Activation Tracks
    /// </summary>
#if UNITY_EDITOR
    [DisplayName("Activation Clip")]
#endif
    class ActivationPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        /// <summary>
        /// Returns a description of the features supported by activation clips
        /// </summary>
        public ClipCaps clipCaps { get { return ClipCaps.None; } }

        /// <summary>
        /// Overrides PlayableAsset.CreatePlayable() to inject needed Playables for an activation asset
        /// </summary>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return Playable.Create(graph);
        }
    }
}
