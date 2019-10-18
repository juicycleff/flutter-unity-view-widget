using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [MenuEntry("Add Override Track", MenuOrder.CustomTrackAction.AnimAddOverrideTrack), UsedImplicitly]
    class AddOverrideTrackAction : TrackAction
    {
        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            foreach (var animTrack in tracks.OfType<AnimationTrack>())
            {
                TimelineHelpers.CreateTrack(typeof(AnimationTrack), animTrack, "Override " + animTrack.GetChildTracks().Count());
            }

            return true;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(t => t.isSubTrack || !t.GetType().IsAssignableFrom(typeof(AnimationTrack))))
                return MenuActionDisplayState.Hidden;

            if (tracks.Any(t => t.lockedInHierarchy))
                return MenuActionDisplayState.Disabled;

            return MenuActionDisplayState.Visible;
        }
    }

    [MenuEntry("Convert To Clip Track", MenuOrder.CustomTrackAction.AnimConvertToClipMode), UsedImplicitly]
    class ConvertToClipModeAction : TrackAction
    {
        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            foreach (var animTrack in tracks.OfType<AnimationTrack>())
                animTrack.ConvertToClipMode();

            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

            return true;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(t => !t.GetType().IsAssignableFrom(typeof(AnimationTrack))))
                return MenuActionDisplayState.Hidden;

            if (tracks.Any(t => t.lockedInHierarchy))
                return MenuActionDisplayState.Disabled;

            if (tracks.OfType<AnimationTrack>().All(a => a.CanConvertToClipMode()))
                return MenuActionDisplayState.Visible;

            return MenuActionDisplayState.Hidden;
        }
    }

    [MenuEntry("Convert To Infinite Clip", MenuOrder.CustomTrackAction.AnimConvertFromClipMode), UsedImplicitly]
    class ConvertFromClipTrackAction : TrackAction
    {
        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            foreach (var animTrack in tracks.OfType<AnimationTrack>())
                animTrack.ConvertFromClipMode(state.editSequence.asset);

            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

            return true;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(t => !t.GetType().IsAssignableFrom(typeof(AnimationTrack))))
                return MenuActionDisplayState.Hidden;

            if (tracks.Any(t => t.lockedInHierarchy))
                return MenuActionDisplayState.Disabled;

            if (tracks.OfType<AnimationTrack>().All(a => a.CanConvertFromClipMode()))
                return MenuActionDisplayState.Visible;

            return MenuActionDisplayState.Hidden;
        }
    }

    abstract class TrackOffsetBaseAction : TrackAction
    {
        public abstract TrackOffset trackOffset { get; }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(t => !t.GetType().IsAssignableFrom(typeof(AnimationTrack))))
                return MenuActionDisplayState.Hidden;

            if (tracks.Any(t => t.lockedInHierarchy))
                return MenuActionDisplayState.Disabled;

            return MenuActionDisplayState.Visible;
        }

        protected override bool IsChecked(WindowState state, TrackAsset[] tracks)
        {
            return tracks.OfType<AnimationTrack>().All(t => t.trackOffset == trackOffset);
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            foreach (var animTrack in tracks.OfType<AnimationTrack>())
            {
                state.UnarmForRecord(animTrack);
                TimelineUndo.PushUndo(animTrack, "Set Transform Offsets");
                animTrack.trackOffset = trackOffset;
            }

            TimelineEditor.Refresh(RefreshReason.ContentsModified);
            return true;
        }
    }


    [MenuEntry("Track Offsets/Apply Transform Offsets", MenuOrder.CustomTrackAction.AnimApplyTrackOffset), UsedImplicitly]
    class ApplyTransformOffsetAction : TrackOffsetBaseAction
    {
        public override TrackOffset trackOffset
        {
            get { return TrackOffset.ApplyTransformOffsets; }
        }
    }

    [MenuEntry("Track Offsets/Apply Scene Offsets", MenuOrder.CustomTrackAction.AnimApplySceneOffset), UsedImplicitly]
    class ApplySceneOffsetAction : TrackOffsetBaseAction
    {
        public override TrackOffset trackOffset
        {
            get { return TrackOffset.ApplySceneOffsets; }
        }
    }

    [MenuEntry("Track Offsets/Auto (Deprecated)", MenuOrder.CustomTrackAction.AnimApplyAutoOffset), UsedImplicitly]
    class ApplyAutoAction : TrackOffsetBaseAction
    {
        public override TrackOffset trackOffset
        {
            get { return TrackOffset.Auto; }
        }
    }
}
