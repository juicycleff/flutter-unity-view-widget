using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [ActiveInMode(TimelineModes.Default)]
    abstract class TrackAction : MenuItemActionBase
    {
        public abstract bool Execute(WindowState state, TrackAsset[] tracks);

        protected virtual MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            return tracks.Length > 0 ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        protected virtual bool IsChecked(WindowState state, TrackAsset[] tracks)
        {
            return false;
        }

        protected virtual string GetDisplayName(TrackAsset[] tracks)
        {
            return menuName;
        }

        public static void Invoke<T>(WindowState state, TrackAsset[] tracks) where T : TrackAction
        {
            actions.First(x => x.GetType() == typeof(T)).Execute(state, tracks);
        }

        static List<TrackAction> s_ActionClasses;

        static List<TrackAction> actions
        {
            get
            {
                if (s_ActionClasses == null)
                    s_ActionClasses =
                        GetActionsOfType(typeof(TrackAction))
                            .Select(x => (TrackAction)x.GetConstructors()[0].Invoke(null))
                            .OrderBy(x => x.priority).ThenBy(x => x.category)
                            .ToList();

                return s_ActionClasses;
            }
        }

        public static void GetMenuEntries(WindowState state, Vector2? mousePos, TrackAsset[] tracks, List<MenuActionItem> items)
        {
            var mode = TimelineWindow.instance.currentMode.mode;
            foreach (var action in actions)
            {
                if (!action.showInMenu)
                    continue;

                var actionItem = action;
                items.Add(
                    new MenuActionItem()
                    {
                        category =  action.category,
                        entryName = action.GetDisplayName(tracks),
                        shortCut = action.shortCut,
                        isChecked = action.IsChecked(state, tracks),
                        isActiveInMode = IsActionActiveInMode(action, mode),
                        priority = action.priority,
                        state = action.GetDisplayState(state, tracks),
                        callback = () =>
                        {
                            actionItem.mousePosition = mousePos;
                            actionItem.Execute(state, tracks);
                            actionItem.mousePosition = null;
                        }
                    }
                );
            }
        }

        public static bool HandleShortcut(WindowState state, Event evt, TrackAsset[] tracks)
        {
            foreach (var action in actions)
            {
                var attr = action.GetType().GetCustomAttributes(typeof(ShortcutAttribute), true);

                foreach (ShortcutAttribute shortcut in attr)
                {
                    if (shortcut.MatchesEvent(evt))
                    {
                        if (s_ShowActionTriggeredByShortcut)
                            Debug.Log(action.GetType().Name);

                        if (!IsActionActiveInMode(action, TimelineWindow.instance.currentMode.mode))
                            return false;

                        return action.Execute(state, tracks);
                    }
                }
            }

            return false;
        }

        // For testing
        internal MenuActionDisplayState InternalGetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            return GetDisplayState(state, tracks);
        }
    }

    [MenuEntry("Edit in Animation Window", MenuOrder.TrackAction.EditInAnimationWindow)]
    class EditTrackInAnimationWindow : TrackAction
    {
        public static bool Do(WindowState state, TrackAsset track)
        {
            AnimationClip clipToEdit = null;

            AnimationTrack animationTrack = track as AnimationTrack;
            if (animationTrack != null)
            {
                if (!animationTrack.CanConvertToClipMode())
                    return false;

                clipToEdit = animationTrack.infiniteClip;
            }
            else if (track.hasCurves)
            {
                clipToEdit = track.curves;
            }

            if (clipToEdit == null)
                return false;

            var gameObject = state.GetSceneReference(track);
            var timeController = TimelineAnimationUtilities.CreateTimeController(state, CreateTimeControlClipData(track));
            TimelineAnimationUtilities.EditAnimationClipWithTimeController(clipToEdit, timeController, gameObject);

            return true;
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Length == 0)
                return MenuActionDisplayState.Hidden;

            if (tracks[0] is AnimationTrack)
            {
                var animTrack = tracks[0] as AnimationTrack;
                if (animTrack.CanConvertToClipMode())
                    return MenuActionDisplayState.Visible;
            }
            else if (tracks[0].hasCurves)
            {
                return MenuActionDisplayState.Visible;
            }

            return MenuActionDisplayState.Hidden;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            return Do(state, tracks[0]);
        }

        static TimelineWindowTimeControl.ClipData CreateTimeControlClipData(TrackAsset track)
        {
            var data = new TimelineWindowTimeControl.ClipData();
            data.track = track;
            data.start = track.start;
            data.duration = track.duration;
            return data;
        }
    }

    [MenuEntry("Lock selected track only", MenuOrder.TrackAction.LockSelected)]
    class LockSelectedTrack : TrackAction
    {
        public static readonly string LockSelectedTrackOnlyText = L10n.Tr("Lock selected track only");
        public static readonly string UnlockSelectedTrackOnlyText = L10n.Tr("Unlock selected track only");

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(track => TimelineUtility.IsLockedFromGroup(track) || track is GroupTrack ||
                !track.subTracksObjects.Any()))
                return MenuActionDisplayState.Hidden;
            return MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            if (!tracks.Any()) return false;

            var hasUnlockedTracks = tracks.Any(x => !x.locked);
            Lock(state, tracks.Where(p => !(p is GroupTrack)).ToArray(), hasUnlockedTracks);
            return true;
        }

        protected override string GetDisplayName(TrackAsset[] tracks)
        {
            return tracks.All(t => t.locked) ? UnlockSelectedTrackOnlyText : LockSelectedTrackOnlyText;
        }

        public static void Lock(WindowState state, TrackAsset[] tracks, bool shouldlock)
        {
            if (tracks.Length == 0)
                return;

            foreach (var track in tracks.Where(t => !TimelineUtility.IsLockedFromGroup(t)))
            {
                TimelineUndo.PushUndo(track, "Lock Tracks");
                track.locked = shouldlock;
            }
            TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw);
        }
    }

    [MenuEntry("Lock", MenuOrder.TrackAction.LockTrack)]
    [Shortcut(Shortcuts.Timeline.toggleLock)]
    class LockTrack : TrackAction
    {
        public static readonly string UnlockText = L10n.Tr("Unlock");

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            bool hasUnlockableTracks = tracks.Any(x => TimelineUtility.IsLockedFromGroup(x));
            if (hasUnlockableTracks)
                return MenuActionDisplayState.Disabled;
            return MenuActionDisplayState.Visible;
        }

        protected override string GetDisplayName(TrackAsset[] tracks)
        {
            return tracks.Any(x => !x.locked) ? base.GetDisplayName(tracks) : UnlockText;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            if (!tracks.Any()) return false;

            var hasUnlockedTracks = tracks.Any(x => !x.locked);
            SetLockState(tracks, hasUnlockedTracks, state);
            return true;
        }

        public static void SetLockState(TrackAsset[] tracks, bool shouldLock, WindowState state = null)
        {
            if (tracks.Length == 0)
                return;

            foreach (var track in tracks)
            {
                if (TimelineUtility.IsLockedFromGroup(track))
                    continue;

                if (track as GroupTrack == null)
                    SetLockState(track.GetChildTracks().ToArray(), shouldLock, state);

                TimelineUndo.PushUndo(track, "Lock Tracks");
                track.locked = shouldLock;
            }

            if (state != null)
            {
                // find the tracks we've locked. unselect anything locked and remove recording.
                foreach (var track in tracks)
                {
                    if (TimelineUtility.IsLockedFromGroup(track) || !track.locked)
                        continue;

                    var flattenedChildTracks = track.GetFlattenedChildTracks();
                    foreach (var i in track.clips)
                        SelectionManager.Remove(i);
                    state.UnarmForRecord(track);
                    foreach (var child in flattenedChildTracks)
                    {
                        SelectionManager.Remove(child);
                        state.UnarmForRecord(child);
                        foreach (var clip in child.GetClips())
                            SelectionManager.Remove(clip);
                    }
                }

                // no need to rebuild, just repaint (including inspectors)
                InspectorWindow.RepaintAllInspectors();
                state.editorWindow.Repaint();
            }
        }
    }

    [UsedImplicitly]
    [MenuEntry("Show Markers", MenuOrder.TrackAction.ShowHideMarkers)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class ShowHideMarkers : TrackAction
    {
        protected override bool IsChecked(WindowState state, TrackAsset[] tracks)
        {
            return tracks.All(x => x.GetShowMarkers());
        }

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(x => x is GroupTrack) || tracks.Any(t => t.GetMarkerCount() == 0))
                return MenuActionDisplayState.Hidden;

            if (tracks.Any(t => t.lockedInHierarchy))
                return MenuActionDisplayState.Disabled;

            return MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            if (!tracks.Any()) return false;

            var hasUnlockedTracks = tracks.Any(x => !x.GetShowMarkers());
            ShowHide(state, tracks, hasUnlockedTracks);
            return true;
        }

        static void ShowHide(WindowState state, TrackAsset[] tracks, bool shouldLock)
        {
            if (tracks.Length == 0)
                return;

            var window = state.GetWindow();
            foreach (var track in tracks)
            {
                window.SetShowTrackMarkers(track, shouldLock);
            }

            TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw);
        }
    }

    [MenuEntry("Mute selected track only", MenuOrder.TrackAction.MuteSelected), UsedImplicitly]
    class MuteSelectedTrack : TrackAction
    {
        public static readonly string UnmuteSelectedText = L10n.Tr("Unmute selected track only");
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(track => TimelineUtility.IsParentMuted(track) || track is GroupTrack ||
                !track.subTracksObjects.Any()))
                return MenuActionDisplayState.Hidden;
            return MenuActionDisplayState.Visible;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            if (!tracks.Any())
                return false;

            var hasUnmutedTracks = tracks.Any(x => !x.muted);
            Mute(state, tracks.Where(p => !(p is GroupTrack)).ToArray(), hasUnmutedTracks);
            return true;
        }

        protected override string GetDisplayName(TrackAsset[] tracks)
        {
            return tracks.All(t => t.muted) ?  UnmuteSelectedText : base.GetDisplayName(tracks);
        }

        public static void Mute(WindowState state, TrackAsset[] tracks, bool shouldMute)
        {
            if (tracks.Length == 0)
                return;

            foreach (var track in tracks.Where(t => !TimelineUtility.IsParentMuted(t)))
            {
                TimelineUndo.PushUndo(track, "Mute Tracks");
                track.muted = shouldMute;
            }

            state.Refresh();
        }
    }

    [MenuEntry("Mute", MenuOrder.TrackAction.MuteTrack)]
    [Shortcut(Shortcuts.Timeline.toggleMute)]
    class MuteTrack : TrackAction
    {
        public static readonly string UnMuteText = L10n.Tr("Unmute");

        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(track => TimelineUtility.IsParentMuted(track)))
                return MenuActionDisplayState.Disabled;
            return MenuActionDisplayState.Visible;
        }

        protected override string GetDisplayName(TrackAsset[] tracks)
        {
            return tracks.Any(x => !x.muted) ? base.GetDisplayName(tracks) : UnMuteText;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            if (!tracks.Any() || tracks.Any(track => TimelineUtility.IsParentMuted(track)))
                return false;

            var hasUnmutedTracks = tracks.Any(x => !x.muted);
            Mute(state, tracks, hasUnmutedTracks);
            return true;
        }

        public static void Mute(WindowState state, TrackAsset[] tracks, bool shouldMute)
        {
            if (tracks.Length == 0)
                return;

            foreach (var track in tracks)
            {
                if (track as GroupTrack == null)
                    Mute(state, track.GetChildTracks().ToArray(), shouldMute);
                TimelineUndo.PushUndo(track, "Mute Tracks");
                track.muted = shouldMute;
            }

            state.Refresh();
        }
    }

    class DeleteTracks : TrackAction
    {
        public static void Do(TimelineAsset timeline, TrackAsset track)
        {
            SelectionManager.Remove(track);
            TrackModifier.DeleteTrack(timeline, track);
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            // disable preview mode so deleted tracks revert to default state
            // Case 956129: Disable preview mode _before_ deleting the tracks, since clip data is still needed
            state.previewMode = false;

            TimelineAnimationUtilities.UnlinkAnimationWindowFromTracks(tracks);

            foreach (var track in tracks)
                Do(state.editSequence.asset, track);

            state.Refresh();

            return true;
        }
    }

    class CopyTracksToClipboard : TrackAction
    {
        public static bool Do(WindowState state, TrackAsset[] tracks)
        {
            var action = new CopyTracksToClipboard();

            return action.Execute(state, tracks);
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            TimelineEditor.clipboard.CopyTracks(tracks);

            return true;
        }
    }

    class DuplicateTracks : TrackAction
    {
        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any())
            {
                SelectionManager.RemoveTimelineSelection();
            }

            foreach (var track in TrackExtensions.FilterTracks(tracks))
            {
                var newTrack = track.Duplicate(TimelineEditor.inspectedDirector, TimelineEditor.inspectedDirector);
                SelectionManager.Add(newTrack);
                foreach (var childTrack in newTrack.GetFlattenedChildTracks())
                {
                    SelectionManager.Add(childTrack);
                }
            }

            state.Refresh();

            return true;
        }
    }

    [MenuEntry("Remove Invalid Markers", MenuOrder.TrackAction.RemoveInvalidMarkers), UsedImplicitly]
    class RemoveInvalidMarkersAction : TrackAction
    {
        protected override MenuActionDisplayState GetDisplayState(WindowState state, TrackAsset[] tracks)
        {
            if (tracks.Any(target => target != null && target.GetMarkerCount() != target.GetMarkersRaw().Count()))
                return MenuActionDisplayState.Visible;

            return MenuActionDisplayState.Hidden;
        }

        public override bool Execute(WindowState state, TrackAsset[] tracks)
        {
            bool anyRemoved = false;
            foreach (var target in tracks)
            {
                var invalids = target.GetMarkersRaw().Where(x => !(x is IMarker)).ToList();
                foreach (var m in invalids)
                {
                    anyRemoved = true;
                    target.DeleteMarkerRaw(m);
                }
            }

            if (anyRemoved)
                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

            return anyRemoved;
        }
    }
}
