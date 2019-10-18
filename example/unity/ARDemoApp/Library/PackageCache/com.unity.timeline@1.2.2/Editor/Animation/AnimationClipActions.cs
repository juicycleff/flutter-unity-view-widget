using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using ClipAction = UnityEditor.Timeline.ItemAction<UnityEngine.Timeline.TimelineClip>;

namespace UnityEditor.Timeline
{
    [MenuEntry("Match Offsets To Previous Clip", MenuOrder.CustomClipAction.AnimClipMatchPrevious), UsedImplicitly]
    class MatchOffsetsPreviousAction : ClipAction
    {
        public override bool Execute(WindowState state, TimelineClip[] items)
        {
            AnimationOffsetMenu.MatchClipsToPrevious(state, items.Where(x => IsValidClip(x, TimelineEditor.inspectedDirector)).ToArray());
            return true;
        }

        private static bool IsValidClip(TimelineClip clip, PlayableDirector director)
        {
            return clip != null &&
                clip.parentTrack != null &&
                (clip.asset as AnimationPlayableAsset) != null &&
                clip.parentTrack.clips.Any(x => x.start < clip.start) &&
                TimelineUtility.GetSceneGameObject(director, clip.parentTrack) != null;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] items)
        {
            if (!items.All(TimelineAnimationUtilities.IsAnimationClip))
                return MenuActionDisplayState.Hidden;

            var director = TimelineEditor.inspectedDirector;
            if (TimelineEditor.inspectedDirector == null)
                return MenuActionDisplayState.Hidden;

            if (items.Any(c => IsValidClip(c, director)))
                return MenuActionDisplayState.Visible;

            return MenuActionDisplayState.Hidden;
        }
    }

    [MenuEntry("Match Offsets To Next Clip", MenuOrder.CustomClipAction.AnimClipMatchNext), UsedImplicitly]
    class MatchOffsetsNextAction : ClipAction
    {
        public override bool Execute(WindowState state, TimelineClip[] items)
        {
            AnimationOffsetMenu.MatchClipsToNext(state, items.Where(x => IsValidClip(x, TimelineEditor.inspectedDirector)).ToArray());
            return true;
        }

        private static bool IsValidClip(TimelineClip clip, PlayableDirector director)
        {
            return clip != null &&
                clip.parentTrack != null &&
                (clip.asset as AnimationPlayableAsset) != null &&
                clip.parentTrack.clips.Any(x => x.start > clip.start) &&
                TimelineUtility.GetSceneGameObject(director, clip.parentTrack) != null;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] items)
        {
            if (!items.All(TimelineAnimationUtilities.IsAnimationClip))
                return MenuActionDisplayState.Hidden;

            var director = TimelineEditor.inspectedDirector;
            if (TimelineEditor.inspectedDirector == null)
                return MenuActionDisplayState.Hidden;

            if (items.Any(c => IsValidClip(c, director)))
                return MenuActionDisplayState.Visible;

            return MenuActionDisplayState.Hidden;
        }
    }

    [MenuEntry("Reset Offsets", MenuOrder.CustomClipAction.AnimClipResetOffset), UsedImplicitly]
    class ResetOffsets : ClipAction
    {
        public override bool Execute(WindowState state, TimelineClip[] items)
        {
            AnimationOffsetMenu.ResetClipOffsets(state, items.Where(TimelineAnimationUtilities.IsAnimationClip).ToArray());
            return true;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] items)
        {
            if (!items.All(TimelineAnimationUtilities.IsAnimationClip))
                return MenuActionDisplayState.Hidden;

            return MenuActionDisplayState.Visible;
        }
    }
}
