using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A group track is a container that allows tracks to be arranged in a hierarchical manner.
    /// </summary>
    [Serializable]
    [TrackClipType(typeof(TrackAsset))]
    [SupportsChildTracks]
    public class GroupTrack : TrackAsset
    {
        internal override bool CanCompileClips()
        {
            return false;
        }

        /// <inheritdoc />
        public override IEnumerable<PlayableBinding> outputs
        {
            get { return PlayableBinding.None; }
        }
    }
}
