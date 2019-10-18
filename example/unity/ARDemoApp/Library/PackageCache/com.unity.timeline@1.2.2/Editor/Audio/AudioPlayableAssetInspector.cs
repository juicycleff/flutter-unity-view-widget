using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    [CustomEditor(typeof(AudioPlayableAsset))]
    class AudioPlayableAssetInspector : BasicAssetInspector
    {
        public override void ApplyChanges()
        {
            // At this point, we are guaranteed that the Timeline window is focused on
            // the correct asset and that a single clip is selected (see ClipInspector)

            if (TimelineEditor.inspectedDirector == null)
                // Do nothing if in asset mode
                return;

            var asset = (AudioPlayableAsset)target;

            if (TimelineEditor.inspectedDirector.state == PlayState.Playing)
                asset.LiveLink();
            else
                TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }
    }
}
