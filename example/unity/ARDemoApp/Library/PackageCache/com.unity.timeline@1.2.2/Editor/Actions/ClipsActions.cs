using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using ClipAction = UnityEditor.Timeline.ItemAction<UnityEngine.Timeline.TimelineClip>;

namespace UnityEditor.Timeline
{
    [MenuEntry("Edit in Animation Window", MenuOrder.ClipEditAction.EditInAnimationWindow), UsedImplicitly]
    class EditClipInAnimationWindow : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            if (clips.Length == 1 && clips[0].animationClip != null)
                return MenuActionDisplayState.Visible;
            return MenuActionDisplayState.Hidden;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            var clip = clips[0];

            if (clip.curves != null || clip.animationClip != null)
            {
                var clipToEdit = clip.animationClip != null ? clip.animationClip : clip.curves;
                if (clipToEdit == null)
                    return false;

                var gameObject = state.GetSceneReference(clip.parentTrack);
                var timeController = TimelineAnimationUtilities.CreateTimeController(state, clip);
                TimelineAnimationUtilities.EditAnimationClipWithTimeController(
                    clipToEdit, timeController, clip.animationClip != null  ? gameObject : null);
                return true;
            }

            return false;
        }
    }

    [MenuEntry("Edit Sub-Timeline", MenuOrder.ClipEditAction.EditSubTimeline), UsedImplicitly]
    class EditSubTimeline : ClipAction
    {
        private static readonly string MultiItemPrefix = "Edit Sub-Timelines/";
        private static readonly string SingleItemPrefix = "Edit ";

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            return IsValid(state, clips) ? MenuActionDisplayState.Visible : MenuActionDisplayState.Hidden;
        }

        bool IsValid(WindowState state, TimelineClip[] clips)
        {
            if (clips.Length != 1 || state == null || state.editSequence.director == null) return false;
            var clip = clips[0];

            var directors = TimelineUtility.GetSubTimelines(clip, state.editSequence.director);
            return directors.Any(x => x != null);
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            if (!IsValid(state, clips)) return false;

            var clip = clips[0];

            var directors = TimelineUtility.GetSubTimelines(clip, state.editSequence.director);
            ExecuteInternal(state, directors, 0, clip);

            return true;
        }

        static void ExecuteInternal(WindowState state, IList<PlayableDirector> directors, int directorIndex, TimelineClip clip)
        {
            SelectionManager.Clear();
            state.GetWindow().SetCurrentTimeline(directors[directorIndex], clip);
        }

        protected override void AddMenuItem(WindowState state, TimelineClip[] items, List<MenuActionItem> menuItems)
        {
            if (items == null || items.Length != 1)
                return;

            var mode = TimelineWindow.instance.currentMode.mode;
            var menuItem = new MenuActionItem()
            {
                category = category,
                entryName = GetDisplayName(items),
                shortCut = string.Empty,
                isChecked = false,
                isActiveInMode = IsActionActiveInMode(this, mode),
                priority = priority,
                state = GetDisplayState(state, items),
                callback = null
            };

            var subDirectors = TimelineUtility.GetSubTimelines(items[0], state.editSequence.director);
            if (subDirectors.Count == 1)
            {
                menuItem.entryName = SingleItemPrefix + DisplayNameHelper.GetDisplayName(subDirectors[0]);
                menuItem.callback = () => Execute(state, items);
                menuItems.Add(menuItem);
            }
            else
            {
                for (int i = 0; i < subDirectors.Count; i++)
                {
                    var index = i;
                    menuItem.category = MultiItemPrefix;
                    menuItem.entryName = DisplayNameHelper.GetDisplayName(subDirectors[i]);
                    menuItem.callback = () => ExecuteInternal(state, subDirectors, index, items[0]);
                    menuItems.Add(menuItem);
                }
            }
        }
    }

    [MenuEntry("Editing/Trim Start", MenuOrder.ClipAction.TrimStart)]
    [Shortcut(Shortcuts.Clip.trimStart), UsedImplicitly]
    class TrimStart : ItemAction<TimelineClip>
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            return clips.All(x => state.editSequence.time <= x.start || state.editSequence.time >= x.start + x.duration) ?
                MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.TrimStart(clips, state.editSequence.time);
        }
    }

    [MenuEntry("Editing/Trim End", MenuOrder.ClipAction.TrimEnd), UsedImplicitly]
    [Shortcut(Shortcuts.Clip.trimEnd)]
    class TrimEnd : ItemAction<TimelineClip>
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            return clips.All(x => state.editSequence.time <= x.start || state.editSequence.time >= x.start + x.duration) ?
                MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.TrimEnd(clips, state.editSequence.time);
        }
    }

    [Shortcut(Shortcuts.Clip.split), MenuEntry("Editing/Split", MenuOrder.ClipAction.Split), UsedImplicitly]
    class Split : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            return clips.All(x => state.editSequence.time <= x.start || state.editSequence.time >= x.start + x.duration) ?
                MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            bool success = ClipModifier.Split(clips, state.editSequence.time, state.editSequence.director);
            if (success)
                state.Refresh();
            return success;
        }
    }

    [MenuEntry("Editing/Complete Last Loop", MenuOrder.ClipAction.CompleteLastLoop), UsedImplicitly]
    class CompleteLastLoop : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.Any(TimelineHelpers.HasUsableAssetDuration);
            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.CompleteLastLoop(clips);
        }
    }

    [MenuEntry("Editing/Trim Last Loop", MenuOrder.ClipAction.TrimLastLoop), UsedImplicitly]
    class TrimLastLoop : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.Any(TimelineHelpers.HasUsableAssetDuration);
            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.TrimLastLoop(clips);
        }
    }

    [MenuEntry("Editing/Match Duration", MenuOrder.ClipAction.MatchDuration), UsedImplicitly]
    class MatchDuration : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            return clips.Length > 1 ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.MatchDuration(clips);
        }
    }

    [MenuEntry("Editing/Double Speed", MenuOrder.ClipAction.DoubleSpeed), UsedImplicitly]
    class DoubleSpeed : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.All(x => x.SupportsSpeedMultiplier());

            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.DoubleSpeed(clips);
        }
    }

    [MenuEntry("Editing/Half Speed", MenuOrder.ClipAction.HalfSpeed), UsedImplicitly]
    class HalfSpeed : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.All(x => x.SupportsSpeedMultiplier());

            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.HalfSpeed(clips);
        }
    }

    [MenuEntry("Editing/Reset Duration", MenuOrder.ClipAction.ResetDuration), UsedImplicitly]
    class ResetDuration : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.Any(TimelineHelpers.HasUsableAssetDuration);
            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.ResetEditing(clips);
        }
    }

    [MenuEntry("Editing/Reset Speed", MenuOrder.ClipAction.ResetSpeed), UsedImplicitly]
    class ResetSpeed : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.All(x => x.SupportsSpeedMultiplier());

            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.ResetSpeed(clips);
        }
    }

    [MenuEntry("Editing/Reset All", MenuOrder.ClipAction.ResetAll), UsedImplicitly]
    class ResetAll : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            bool canDisplay = clips.Any(TimelineHelpers.HasUsableAssetDuration) ||
                clips.All(x => x.SupportsSpeedMultiplier());

            return canDisplay ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            var speedResult = ClipModifier.ResetSpeed(clips);
            var editResult = ClipModifier.ResetEditing(clips);
            return speedResult || editResult;
        }
    }

    [MenuEntry("Tile", MenuOrder.ClipAction.Tile), UsedImplicitly]
    class Tile : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TimelineClip[] clips)
        {
            return clips.Length > 1 ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            return ClipModifier.Tile(clips);
        }
    }

    [MenuEntry("Find Source Asset", MenuOrder.ClipAction.FindSourceAsset), UsedImplicitly]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class FindSourceAsset : ClipAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state,
            TimelineClip[] clips)
        {
            if (clips.Length > 1)
                return MenuActionDisplayState.Disabled;

            if (GetUnderlyingAsset(state, clips[0]) == null)
                return MenuActionDisplayState.Disabled;

            return MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            EditorGUIUtility.PingObject(GetUnderlyingAsset(state, clips[0]));
            return true;
        }

        private static UnityEngine.Object GetExternalPlayableAsset(TimelineClip clip)
        {
            if (clip.asset == null)
                return null;

            if ((clip.asset.hideFlags & HideFlags.HideInHierarchy) != 0)
                return null;

            return clip.asset;
        }

        private static UnityEngine.Object GetUnderlyingAsset(WindowState state, TimelineClip clip)
        {
            var asset = clip.asset as ScriptableObject;
            if (asset == null)
                return null;

            var fields = ObjectReferenceField.FindObjectReferences(asset.GetType());
            if (fields.Length == 0)
                return GetExternalPlayableAsset(clip);

            // Find the first non-null field
            foreach (var field in fields)
            {
                // skip scene refs in asset mode
                if (state.editSequence.director == null && field.isSceneReference)
                    continue;
                var obj = field.Find(asset, state.editSequence.director);
                if (obj != null)
                    return obj;
            }

            return GetExternalPlayableAsset(clip);
        }
    }

    class CopyClipsToClipboard : ClipAction
    {
        public override bool Execute(WindowState state, TimelineClip[] clips)
        {
            TimelineEditor.clipboard.CopyItems(clips.ToItems());
            return true;
        }
    }
}
