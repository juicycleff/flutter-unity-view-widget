using System;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A PlayableTrack is a track whose clips are custom playables.
    /// </summary>
    /// <remarks>
    /// This is a track that can contain PlayableAssets that are found in the project and do not have their own specified track type.
    /// </remarks>
    [Serializable]
    public class PlayableTrack : TrackAsset
    {
        /// <inheritdoc />
        protected override void OnCreateClip(TimelineClip clip)
        {
            if (clip.asset != null)
                clip.displayName = clip.asset.GetType().Name;
        }
    }
}
