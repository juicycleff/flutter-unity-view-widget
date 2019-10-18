using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Timeline;
using MenuEntryPair = System.Collections.Generic.KeyValuePair<UnityEngine.GUIContent, UnityEditor.Timeline.TimelineAction>;

namespace UnityEditor.Timeline
{
    [ActiveInMode(TimelineModes.Default)]
    abstract class TimelineAction : MenuItemActionBase
    {
        public abstract bool Execute(WindowState state);

        public virtual MenuActionDisplayState GetDisplayState(WindowState state)
        {
            return MenuActionDisplayState.Visible;
        }

        public virtual bool IsChecked(WindowState state)
        {
            return false;
        }

        protected string GetDisplayName(WindowState state)
        {
            return menuName;
        }

        bool CanExecute(WindowState state)
        {
            return GetDisplayState(state) == MenuActionDisplayState.Visible;
        }

        public static void Invoke<T>(WindowState state) where T : TimelineAction
        {
            var action = AllActions.FirstOrDefault(x => x.GetType() == typeof(T));
            if (action != null && action.CanExecute(state))
                action.Execute(state);
        }

        // an instance of all TimelineActions
        public static readonly TimelineAction[] AllActions = GetActionsOfType(typeof(TimelineAction)).Select(x => (TimelineAction)x.GetConstructors()[0].Invoke(null)).ToArray();

        // an instance of all TimelineActions that should appear in a regular contextMenu
        public static readonly TimelineAction[] MenuActions = AllActions.Where(a => a.showInMenu && !(a is MarkerHeaderAction)).ToArray();

        public static void GetMenuEntries(IEnumerable<TimelineAction> actions, Vector2? mousePos, List<MenuActionItem> items)
        {
            var state = TimelineWindow.instance.state;
            var mode = TimelineWindow.instance.currentMode.mode;

            foreach (var action in actions)
            {
                var actionItem = action;
                action.mousePosition = mousePos;
                items.Add(
                    new MenuActionItem()
                    {
                        category =  action.category,
                        entryName = action.GetDisplayName(state),
                        shortCut = action.shortCut,
                        isChecked = action.IsChecked(state),
                        isActiveInMode = IsActionActiveInMode(action, mode),
                        priority = action.priority,
                        state = action.GetDisplayState(state),
                        callback = () =>
                        {
                            actionItem.mousePosition = mousePos;
                            actionItem.Execute(state);
                            actionItem.mousePosition = null;
                        }
                    }
                );
                action.mousePosition = null;
            }
        }

        public static bool HandleShortcut(WindowState state, Event evt)
        {
            if (EditorGUI.IsEditingTextField())
                return false;

            foreach (var action in AllActions)
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

                        var handled = action.Execute(state);
                        if (handled)
                            return true;
                    }
                }
            }

            return false;
        }

        protected static bool DoInternal(Type t, WindowState state)
        {
            var action = (TimelineAction)t.GetConstructors()[0].Invoke(null);

            if (action.CanExecute(state))
                return action.Execute(state);

            return false;
        }
    }

    // indicates the action only applies to the marker header menu
    abstract class MarkerHeaderAction : TimelineAction
    {
    }


    [MenuEntry("Copy", MenuOrder.TimelineAction.Copy)]
    [Shortcut("Main Menu/Edit/Copy", EventCommandNames.Copy)]
    class CopyAction : TimelineAction
    {
        public static bool Do(WindowState state)
        {
            return DoInternal(typeof(CopyAction), state);
        }

        public override MenuActionDisplayState GetDisplayState(WindowState state)
        {
            return SelectionManager.Count() > 0 ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state)
        {
            TimelineEditor.clipboard.Clear();

            var clips = SelectionManager.SelectedClips().ToArray();
            if (clips.Length > 0)
            {
                ItemAction<TimelineClip>.Invoke<CopyClipsToClipboard>(state, clips);
            }
            var markers = SelectionManager.SelectedMarkers().ToArray();
            if (markers.Length > 0)
            {
                ItemAction<IMarker>.Invoke<CopyMarkersToClipboard>(state, markers);
            }
            var tracks = SelectionManager.SelectedTracks().ToArray();
            if (tracks.Length > 0)
            {
                CopyTracksToClipboard.Do(state, tracks);
            }

            return true;
        }
    }

    [MenuEntry("Paste", MenuOrder.TimelineAction.Paste)]
    [Shortcut("Main Menu/Edit/Paste", EventCommandNames.Paste)]
    class PasteAction : TimelineAction
    {
        public static bool Do(WindowState state)
        {
            return DoInternal(typeof(PasteAction), state);
        }

        public override MenuActionDisplayState GetDisplayState(WindowState state)
        {
            return CanPaste(state) ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state)
        {
            if (!CanPaste(state))
                return false;

            PasteItems(state, mousePosition);
            PasteTracks(state);

            state.Refresh();

            mousePosition = null;
            return true;
        }

        bool CanPaste(WindowState state)
        {
            var copiedItems = TimelineEditor.clipboard.GetCopiedItems().ToList();

            if (!copiedItems.Any())
                return TimelineEditor.clipboard.GetTracks().Any();

            return CanPasteItems(copiedItems, state, mousePosition);
        }

        static bool CanPasteItems(ICollection<ItemsPerTrack> itemsGroups, WindowState state, Vector2? mousePosition)
        {
            var hasItemsCopiedFromMultipleTracks = itemsGroups.Count > 1;
            var allItemsCopiedFromCurrentAsset = itemsGroups.All(x => x.targetTrack.timelineAsset == state.editSequence.asset);
            var hasUsedShortcut = mousePosition == null;
            var anySourceLocked = itemsGroups.Any(x => x.targetTrack != null && x.targetTrack.lockedInHierarchy);

            //do not paste if the user copied items from another timeline
            //if the copied items comes from > 1 track (since we do not know where to paste the copied items)
            //or if a keyboard shortcut was used (since the user will not see the paste result)
            if (!allItemsCopiedFromCurrentAsset)
            {
                if (hasItemsCopiedFromMultipleTracks || hasUsedShortcut)
                    return false;
            }

            if (hasUsedShortcut)
                return !anySourceLocked; // copy/paste to same track

            var targetTrack = GetPickedTrack();
            if (targetTrack == null)
                targetTrack = SelectionManager.SelectedTracks().FirstOrDefault();

            if (hasItemsCopiedFromMultipleTracks)
            {
                //do not paste if the track which received the paste action does not contain a copied clip
                return !anySourceLocked && itemsGroups.Select(x => x.targetTrack).Contains(targetTrack);
            }

            var copiedItems = itemsGroups.SelectMany(i => i.items);
            return IsTrackValidForItems(targetTrack, copiedItems);
        }

        static void PasteItems(WindowState state, Vector2? mousePosition)
        {
            var copiedItems = TimelineEditor.clipboard.GetCopiedItems().ToList();
            var numberOfUniqueParentsInClipboard = copiedItems.Count();

            if (numberOfUniqueParentsInClipboard == 0) return;
            List<ITimelineItem> newItems;

            //if the copied items were on a single parent, then use the mouse position to get the parent OR the original parent
            if (numberOfUniqueParentsInClipboard == 1)
            {
                var itemsGroup = copiedItems.First();
                TrackAsset target = null;
                if (mousePosition.HasValue)
                    target = GetPickedTrack();
                if (target == null)
                    target = FindSuitableParentForSingleTrackPasteWithoutMouse(itemsGroup);

                var candidateTime = TimelineHelpers.GetCandidateTime(state, mousePosition, target);
                newItems = TimelineHelpers.DuplicateItemsUsingCurrentEditMode(state, TimelineEditor.clipboard.exposedPropertyTable, TimelineEditor.inspectedDirector, itemsGroup, target, candidateTime, "Paste Items").ToList();
            }
            //if copied items were on multiple parents, then the destination parents are the same as the original parents
            else
            {
                var time = TimelineHelpers.GetCandidateTime(state, mousePosition, copiedItems.Select(c => c.targetTrack).ToArray());
                newItems = TimelineHelpers.DuplicateItemsUsingCurrentEditMode(state, TimelineEditor.clipboard.exposedPropertyTable, TimelineEditor.inspectedDirector, copiedItems, time, "Paste Items").ToList();
            }

            TimelineHelpers.FrameItems(state, newItems);
            SelectionManager.RemoveTimelineSelection();
            foreach (var item in newItems)
            {
                SelectionManager.Add(item);
            }
        }

        static TrackAsset FindSuitableParentForSingleTrackPasteWithoutMouse(ItemsPerTrack itemsGroup)
        {
            var groupParent = itemsGroup.targetTrack; //set a main parent in the clipboard
            var selectedTracks = SelectionManager.SelectedTracks();

            if (selectedTracks.Contains(groupParent))
            {
                return groupParent;
            }

            //find a selected track suitable for all items
            var itemsToPaste = itemsGroup.items;
            var compatibleTrack = selectedTracks.FirstOrDefault(t => IsTrackValidForItems(t, itemsToPaste));
            return compatibleTrack != null ? compatibleTrack : groupParent;
        }

        static bool IsTrackValidForItems(TrackAsset track, IEnumerable<ITimelineItem> items)
        {
            if (track == null || track.lockedInHierarchy) return false;
            return items.All(i => i.IsCompatibleWithTrack(track));
        }

        static TrackAsset GetPickedTrack()
        {
            var rowGUI = PickerUtils.pickedElements.OfType<IRowGUI>().FirstOrDefault();
            if (rowGUI != null)
                return rowGUI.asset;

            return null;
        }

        static void PasteTracks(WindowState state)
        {
            var trackData = TimelineEditor.clipboard.GetTracks().ToList();
            if (trackData.Any())
            {
                SelectionManager.RemoveTimelineSelection();
            }

            foreach (var track in trackData)
            {
                var newTrack = track.item.Duplicate(TimelineEditor.clipboard.exposedPropertyTable, TimelineEditor.inspectedDirector, TimelineEditor.inspectedAsset);
                SelectionManager.Add(newTrack);
                foreach (var childTrack in newTrack.GetFlattenedChildTracks())
                {
                    SelectionManager.Add(childTrack);
                }

                if (track.parent != null && track.parent.timelineAsset == state.editSequence.asset)
                {
                    TrackExtensions.ReparentTracks(new List<TrackAsset> { newTrack }, track.parent, track.item);
                }
            }
        }
    }

    [MenuEntry("Duplicate", MenuOrder.TimelineAction.Duplicate)]
    [Shortcut("Main Menu/Edit/Duplicate", EventCommandNames.Duplicate)]
    class DuplicateAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return Execute(state, (item1, item2) => ItemsUtils.TimeGapBetweenItems(item1, item2, state));
        }

        internal bool Execute(WindowState state, Func<ITimelineItem, ITimelineItem, double> gapBetweenItems)
        {
            var selectedItems = SelectionManager.SelectedItems().ToItemsPerTrack().ToList();
            if (selectedItems.Any())
            {
                var requestedTime = CalculateDuplicateTime(selectedItems, gapBetweenItems);
                var duplicatedItems = TimelineHelpers.DuplicateItemsUsingCurrentEditMode(state, TimelineEditor.inspectedDirector,TimelineEditor.inspectedDirector, selectedItems, requestedTime, "Duplicate Items");

                TimelineHelpers.FrameItems(state, duplicatedItems);
                SelectionManager.RemoveTimelineSelection();
                foreach (var item in duplicatedItems)
                    SelectionManager.Add(item);
            }

            var tracks = SelectionManager.SelectedTracks().ToArray();
            if (tracks.Length > 0)
                TrackAction.Invoke<DuplicateTracks>(state, tracks);

            state.Refresh();
            return true;
        }

        static double CalculateDuplicateTime(IEnumerable<ItemsPerTrack> duplicatedItems, Func<ITimelineItem, ITimelineItem, double> gapBetweenItems)
        {
            //Find the end time of the rightmost item
            var itemsOnTracks = duplicatedItems.SelectMany(i => i.targetTrack.GetItems()).ToList();
            var time = itemsOnTracks.Max(i => i.end);

            //From all the duplicated items, select the leftmost items
            var firstDuplicatedItems = duplicatedItems.Select(i => i.leftMostItem);
            var leftMostDuplicatedItems = firstDuplicatedItems.OrderBy(i => i.start).GroupBy(i => i.start).FirstOrDefault();
            if (leftMostDuplicatedItems == null) return 0.0;

            foreach (var leftMostItem in leftMostDuplicatedItems)
            {
                var siblings = leftMostItem.parentTrack.GetItems();
                var rightMostSiblings = siblings.OrderByDescending(i => i.end).GroupBy(i => i.end).FirstOrDefault();
                if (rightMostSiblings == null) continue;

                foreach (var sibling in rightMostSiblings)
                    time = Math.Max(time, sibling.end + gapBetweenItems(leftMostItem, sibling));
            }

            return time;
        }
    }

    [MenuEntry("Delete", MenuOrder.TimelineAction.Delete)]
    [Shortcut("Main Menu/Edit/Delete", EventCommandNames.Delete)]
    [ShortcutPlatformOverride(RuntimePlatform.OSXEditor, KeyCode.Backspace, ShortcutModifiers.Action)]
    [ActiveInMode(TimelineModes.Default)]
    class DeleteAction : TimelineAction
    {
        public override MenuActionDisplayState GetDisplayState(WindowState state)
        {
            return CanDelete(state) ? MenuActionDisplayState.Visible : MenuActionDisplayState.Disabled;
        }

        static bool CanDelete(WindowState state)
        {
            if (state.editSequence.isReadOnly)
                return false;
            // All() returns true when empty
            return SelectionManager.SelectedTracks().All(x => !x.lockedInHierarchy) &&
                SelectionManager.SelectedItems().All(x => x.parentTrack == null || !x.parentTrack.lockedInHierarchy);
        }

        public override bool Execute(WindowState state)
        {
            if (SelectionManager.GetCurrentInlineEditorCurve() != null)
                return false;

            if (!CanDelete(state))
                return false;

            var selectedItems = SelectionManager.SelectedItems();
            DeleteItems(selectedItems);

            var tracks = SelectionManager.SelectedTracks().ToArray();
            if (tracks.Any())
                TrackAction.Invoke<DeleteTracks>(state, tracks);

            state.Refresh();
            return selectedItems.Any() ||  tracks.Length > 0;
        }

        internal static void DeleteItems(IEnumerable<ITimelineItem> items)
        {
            var tracks = items.GroupBy(c => c.parentTrack);

            foreach (var track in tracks)
                TimelineUndo.PushUndo(track.Key, "Delete Items");

            TimelineAnimationUtilities.UnlinkAnimationWindowFromClips(items.OfType<ClipItem>().Select(i => i.clip));

            EditMode.PrepareItemsDelete(ItemsUtils.ToItemsPerTrack(items));
            EditModeUtils.Delete(items);

            SelectionManager.RemoveAllClips();
        }
    }

    [MenuEntry("Match Content", MenuOrder.TimelineAction.MatchContent)]
    [Shortcut(Shortcuts.Timeline.matchContent)]
    class MatchContent : TimelineAction
    {
        public override MenuActionDisplayState GetDisplayState(WindowState state)
        {
            var clips = SelectionManager.SelectedClips().ToArray();

            if (!clips.Any() || SelectionManager.GetCurrentInlineEditorCurve() != null)
                return MenuActionDisplayState.Hidden;

            return clips.Any(TimelineHelpers.HasUsableAssetDuration)
                ? MenuActionDisplayState.Visible
                : MenuActionDisplayState.Disabled;
        }

        public override bool Execute(WindowState state)
        {
            if (SelectionManager.GetCurrentInlineEditorCurve() != null)
                return false;

            var clips = SelectionManager.SelectedClips().ToArray();
            return clips.Length > 0 && ClipModifier.MatchContent(clips);
        }
    }

    [Shortcut(Shortcuts.Timeline.play)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class PlayTimelineAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            var currentState = state.playing;
            state.SetPlaying(!currentState);
            return true;
        }
    }

    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectAllAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            // otherwise select all tracks.
            SelectionManager.Clear();
            state.GetWindow().allTracks.ForEach(x => SelectionManager.Add(x.track));

            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.previousFrame)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class PreviousFrameAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            state.editSequence.frame--;
            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.nextFrame)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class NextFrameAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            state.editSequence.frame++;
            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.frameAll)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class FrameAllAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            var inlineCurveEditor = SelectionManager.GetCurrentInlineEditorCurve();
            if (inlineCurveEditor != null && inlineCurveEditor.inlineCurvesSelected)
            {
                FrameSelectedAction.FrameInlineCurves(inlineCurveEditor, state, false);
                return true;
            }

            if (state.IsEditingASubItem())
                return false;

            var w = state.GetWindow();
            if (w == null || w.treeView == null)
                return false;

            var visibleTracks = w.treeView.visibleTracks.ToList();
            if (state.editSequence.asset != null && state.editSequence.asset.markerTrack != null)
                visibleTracks.Add(state.editSequence.asset.markerTrack);

            if (visibleTracks.Count == 0)
                return false;

            var startTime = float.MaxValue;
            var endTime = float.MinValue;

            foreach (var t in visibleTracks)
            {
                if (t == null)
                    continue;

                double trackStart, trackEnd;
                t.GetItemRange(out trackStart, out trackEnd);
                startTime = Mathf.Min(startTime, (float)trackStart);
                endTime = Mathf.Max(endTime, (float)(trackEnd));
            }

            if (startTime != float.MinValue)
            {
                FrameSelectedAction.FrameRange(startTime, endTime, state);
                return true;
            }

            return false;
        }
    }

    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class FrameSelectedAction : TimelineAction
    {
        public static void FrameRange(float startTime, float endTime, WindowState state)
        {
            if (startTime > endTime)
            {
                return;
            }

            var halfDuration = endTime - Math.Max(0.0f, startTime);

            if (halfDuration > 0.0f)
            {
                state.SetTimeAreaShownRange(Mathf.Max(-10.0f, startTime - (halfDuration * 0.1f)),
                    endTime + (halfDuration * 0.1f));
            }
            else
            {
                // start == end
                // keep the zoom level constant, only pan the time area to center the item
                var currentRange = state.timeAreaShownRange.y - state.timeAreaShownRange.x;
                state.SetTimeAreaShownRange(startTime - currentRange / 2, startTime + currentRange / 2);
            }

            TimelineZoomManipulator.InvalidateWheelZoom();
            state.Evaluate();
        }

        public override bool Execute(WindowState state)
        {
            var inlineCurveEditor = SelectionManager.GetCurrentInlineEditorCurve();
            if (inlineCurveEditor != null && inlineCurveEditor.inlineCurvesSelected)
            {
                FrameInlineCurves(inlineCurveEditor, state, true);
                return true;
            }

            if (state.IsEditingASubItem())
                return false;

            if (SelectionManager.Count() == 0)
                return false;

            var startTime = float.MaxValue;
            var endTime = float.MinValue;

            var clips = SelectionManager.SelectedClipGUI();
            var markers = SelectionManager.SelectedMarkers();
            if (!clips.Any() && !markers.Any())
                return false;

            foreach (var c in clips)
            {
                startTime = Mathf.Min(startTime, (float)c.clip.start);
                endTime = Mathf.Max(endTime, (float)c.clip.end);
                if (c.clipCurveEditor != null)
                {
                    c.clipCurveEditor.FrameClip();
                }
            }

            foreach (var marker in markers)
            {
                startTime = Mathf.Min(startTime, (float)marker.time);
                endTime = Mathf.Max(endTime, (float)marker.time);
            }

            FrameRange(startTime, endTime, state);

            return true;
        }

        public static void FrameInlineCurves(IClipCurveEditorOwner curveEditorOwner, WindowState state, bool selectionOnly)
        {
            var curveEditor = curveEditorOwner.clipCurveEditor.curveEditor;
            var frameBounds = selectionOnly ? curveEditor.GetSelectionBounds() : curveEditor.GetClipBounds();

            var clipGUI = curveEditorOwner as TimelineClipGUI;
            var areaOffset = 0.0f;

            if (clipGUI != null)
            {
                areaOffset = (float)Math.Max(0.0, clipGUI.clip.FromLocalTimeUnbound(0.0));

                var timeScale = (float)clipGUI.clip.timeScale;  // Note: The getter for clip.timeScale is guaranteed to never be zero.

                // Apply scaling
                var newMin = frameBounds.min.x / timeScale;
                var newMax = (frameBounds.max.x - frameBounds.min.x) / timeScale + newMin;

                frameBounds.SetMinMax(
                    new Vector3(newMin, frameBounds.min.y, frameBounds.min.z),
                    new Vector3(newMax, frameBounds.max.y, frameBounds.max.z));
            }

            curveEditor.Frame(frameBounds, true, true);

            var area = curveEditor.shownAreaInsideMargins;
            area.x += areaOffset;

            FrameRange(area.x, area.x + area.width, state);
        }
    }

    [Shortcut(Shortcuts.Timeline.previousKey)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class PrevKeyAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            var keyTraverser = new Utilities.KeyTraverser(state.editSequence.asset, 0.01f / state.referenceSequence.frameRate);
            var time = keyTraverser.GetPrevKey((float)state.editSequence.time, state.dirtyStamp);
            if (time != state.editSequence.time)
            {
                state.editSequence.time = time;
            }

            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.nextKey)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class NextKeyAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            var keyTraverser = new Utilities.KeyTraverser(state.editSequence.asset, 0.01f / state.referenceSequence.frameRate);
            var time = keyTraverser.GetNextKey((float)state.editSequence.time, state.dirtyStamp);
            if (time != state.editSequence.time)
            {
                state.editSequence.time = time;
            }

            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.goToStart)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class GotoStartAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            state.editSequence.time = 0.0f;
            state.EnsurePlayHeadIsVisible();

            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.goToEnd)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class GotoEndAction : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            state.editSequence.time = state.editSequence.duration;
            state.EnsurePlayHeadIsVisible();

            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.zoomIn)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class ZoomIn : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            TimelineZoomManipulator.Instance.DoZoom(1.15f, state);
            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.zoomOut)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class ZoomOut : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            TimelineZoomManipulator.Instance.DoZoom(0.85f, state);
            return true;
        }
    }

    [Shortcut(Shortcuts.Timeline.collapseGroup)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class CollapseGroup : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.CollapseGroup(state);
        }
    }

    [Shortcut(Shortcuts.Timeline.unCollapseGroup)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class UnCollapseGroup : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.UnCollapseGroup(state);
        }
    }

    [Shortcut(Shortcuts.Timeline.selectLeftItem)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectLeftClip : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            // Switches to track header if no left track exists
            return KeyboardNavigation.SelectLeftItem(state);
        }
    }

    [Shortcut(Shortcuts.Timeline.selectRightItem)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectRightClip : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectRightItem(state);
        }
    }

    [Shortcut(Shortcuts.Timeline.selectUpItem)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectUpClip : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectUpItem(state);
        }
    }

    [Shortcut(Shortcuts.Timeline.selectUpTrack)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectUpTrack : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectUpTrack();
        }
    }

    [Shortcut(Shortcuts.Timeline.selectDownItem)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectDownClip : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectDownItem(state);
        }
    }

    [Shortcut(Shortcuts.Timeline.selectDownTrack)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class SelectDownTrack : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            if (!KeyboardNavigation.ClipAreaActive() && !KeyboardNavigation.TrackHeadActive())
                return KeyboardNavigation.FocusFirstVisibleItem(state);
            else
                return KeyboardNavigation.SelectDownTrack();
        }
    }

    [Shortcut(Shortcuts.Timeline.multiSelectLeft)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class MultiselectLeftClip : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectLeftItem(state, true);
        }
    }

    [Shortcut(Shortcuts.Timeline.multiSelectRight)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class MultiselectRightClip : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectRightItem(state, true);
        }
    }

    [Shortcut(Shortcuts.Timeline.multiSelectUp)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class MultiselectUpTrack : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectUpTrack(true);
        }
    }

    [Shortcut(Shortcuts.Timeline.multiSelectDown)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class MultiselectDownTrack : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            return KeyboardNavigation.SelectDownTrack(true);
        }
    }

    [Shortcut(Shortcuts.Timeline.toggleClipTrackArea)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class ToggleClipTrackArea : TimelineAction
    {
        public override bool Execute(WindowState state)
        {
            if (KeyboardNavigation.TrackHeadActive())
                return KeyboardNavigation.FocusFirstVisibleItem(state, SelectionManager.SelectedTracks());

            if (!KeyboardNavigation.ClipAreaActive())
                return KeyboardNavigation.FocusFirstVisibleItem(state);

            var item = KeyboardNavigation.GetVisibleSelectedItems().LastOrDefault();
            if (item != null)
                SelectionManager.SelectOnly(item.parentTrack);
            return true;
        }
    }

    [MenuEntry("Mute", MenuOrder.TrackAction.MuteTrack)]
    class ToggleMuteMarkersOnTimeline : MarkerHeaderAction
    {
        public override bool IsChecked(WindowState state)
        {
            return IsMarkerTrackValid(state) && state.editSequence.asset.markerTrack.muted;
        }

        public override bool Execute(WindowState state)
        {
            if (state.showMarkerHeader)
                ToggleMute(state);
            return true;
        }

        static void ToggleMute(WindowState state)
        {
            var timeline = state.editSequence.asset;
            timeline.CreateMarkerTrack();

            TimelineUndo.PushUndo(timeline.markerTrack, "Toggle Mute");
            timeline.markerTrack.muted = !timeline.markerTrack.muted;
        }

        static bool IsMarkerTrackValid(WindowState state)
        {
            var timeline = state.editSequence.asset;
            return timeline != null && timeline.markerTrack != null;
        }
    }

    [MenuEntry("Show Markers", MenuOrder.TrackAction.ShowHideMarkers)]
    [ActiveInMode(TimelineModes.Default | TimelineModes.ReadOnly)]
    class ToggleShowMarkersOnTimeline : MarkerHeaderAction
    {
        public override bool IsChecked(WindowState state)
        {
            return state.showMarkerHeader;
        }

        public override bool Execute(WindowState state)
        {
            ToggleShow(state);
            return true;
        }

        static void ToggleShow(WindowState state)
        {
            state.GetWindow().SetShowMarkerHeader(!state.showMarkerHeader);
        }
    }
}
