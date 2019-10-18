using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// A Track whose clips control time-related elements on a GameObject.
    /// </summary>
    [TrackClipType(typeof(ControlPlayableAsset), false)]
    public class ControlTrack : TrackAsset
    {
    }
}
