using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomTimelineEditor(typeof(MarkerTrack))]
    class MarkerTrackEditor : TrackEditor
    {
        public static readonly float DefaultMarkerTrackHeight = 20;

        public override TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            var options = base.GetTrackOptions(track, binding);
            options.minimumHeight = DefaultMarkerTrackHeight;
            return options;
        }
    }
}
