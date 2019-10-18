using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    static class TrackModifier
    {
        public static bool DeleteTrack(TimelineAsset timeline, TrackAsset track)
        {
            if (TimelineEditor.inspectedDirector != null)
            {
                TimelineUndo.PushUndo(TimelineEditor.inspectedDirector, "Delete Track");
                TimelineEditor.inspectedDirector.ClearGenericBinding(track);
            }

            return timeline.DeleteTrack(track);
        }
    }
}
