using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Implement this interface on a TrackAsset derived class to support layers
    /// </summary>
    public interface ILayerable
    {
        /// <summary>
        /// Creates a mixer that blends track mixers.
        /// </summary>
        /// <param name="graph">The graph where the mixer playable will be added.</param>
        /// <param name="go">The GameObject that requested the graph.</param>
        /// <param name="inputCount">The number of inputs on the mixer. There should be an input for each playable from each clip.</param>
        /// <returns>Returns a playable that is used as a mixer. If this method returns Playable.Null, it indicates that a layer mixer is not needed. In this case, a single track mixer blends all playables generated from all layers.</returns>
        Playable CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount);
    }
}
