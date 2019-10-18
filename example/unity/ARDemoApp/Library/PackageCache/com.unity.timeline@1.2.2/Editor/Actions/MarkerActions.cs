using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using MarkerAction = UnityEditor.Timeline.ItemAction<UnityEngine.Timeline.IMarker>;

namespace UnityEditor.Timeline
{
    [UsedImplicitly]
    class CopyMarkersToClipboard : MarkerAction
    {
        public override bool Execute(WindowState state, IMarker[] markers)
        {
            TimelineEditor.clipboard.CopyItems(markers.ToItems());
            return true;
        }
    }
}
