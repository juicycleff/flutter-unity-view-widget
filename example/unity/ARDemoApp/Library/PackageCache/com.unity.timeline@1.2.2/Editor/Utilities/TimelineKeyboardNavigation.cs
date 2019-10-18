using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class KeyboardNavigation
    {
        public static void FrameTrackHeader(TreeViewItem treeItem = null)
        {
            if (TrackHeadActive())
                treeItem = treeItem ?? SelectionManager.SelectedTrackGUI().Last();
            else
            {
                var item = GetVisibleSelectedItems().LastOrDefault();
                treeItem = TimelineWindow.instance.allTracks.FirstOrDefault(
                    x => item != null && x.track == item.parentTrack);
            }

            if (treeItem != null)
                TimelineWindow.instance.treeView.FrameItem(treeItem);
        }

        public static bool TrackHeadActive()
        {
            return SelectionManager.SelectedTracks().Any(x => x.IsVisibleRecursive()) && !ClipAreaActive();
        }

        public static bool ClipAreaActive()
        {
            return GetVisibleSelectedItems().Any();
        }

        public static IEnumerable<ITimelineItem> GetVisibleSelectedItems()
        {
            return SelectionManager.SelectedItems().Where(x => x.parentTrack.IsVisibleRecursive());
        }

        public static IEnumerable<TimelineTrackBaseGUI> GetVisibleTracks()
        {
            return TimelineWindow.instance.allTracks.Where(x => x.track.IsVisibleRecursive());
        }

        static TrackAsset PreviousTrack(this TrackAsset track)
        {
            var uiOrderTracks = GetVisibleTracks().Select(t => t.track).ToList();
            var selIdx = uiOrderTracks.IndexOf(track);
            return selIdx > 0 ? uiOrderTracks[selIdx - 1] : null;
        }

        static TrackAsset NextTrack(this TrackAsset track)
        {
            var uiOrderTracks = GetVisibleTracks().Select(t => t.track).ToList();
            var selIdx = uiOrderTracks.IndexOf(track);
            return selIdx < uiOrderTracks.Count - 1  && selIdx != -1 ? uiOrderTracks[selIdx + 1] : null;
        }

        static ITimelineItem PreviousItem(this ITimelineItem item, bool clipOnly)
        {
            var items = item.parentTrack.GetItems().ToArray();
            if (clipOnly)
            {
                items = items.Where(x => x is ClipItem).ToArray();
            }
            else
            {
                items =  items.Where(x => x is MarkerItem).ToArray();
            }

            var idx = Array.IndexOf(items, item);
            return idx > 0 ? items[idx - 1] : null;
        }

        static ITimelineItem NextItem(this ITimelineItem item, bool clipOnly)
        {
            var items = item.parentTrack.GetItems().ToArray();
            if (clipOnly)
            {
                items = items.Where(x => x is ClipItem).ToArray();
            }
            else
            {
                items =  items.Where(x => x is MarkerItem).ToArray();
            }

            var idx = Array.IndexOf(items, item);
            return idx < items.Length - 1 ? items[idx + 1] : null;
        }

        static bool FilterItems(ref List<ITimelineItem> items)
        {
            var clipOnly = false;
            if (items.Any(x => x is ClipItem))
            {
                items = items.Where(x => x is ClipItem).ToList();
                clipOnly = true;
            }

            return clipOnly;
        }

        static ITimelineItem GetClosestItem(TrackAsset track, ITimelineItem refItem)
        {
            var start = refItem.start;
            var end = refItem.end;
            var items = track.GetItems().ToList();

            if (refItem is ClipItem)
            {
                items = items.Where(x => x is ClipItem).ToList();
            }
            else
            {
                items =  items.Where(x => x is MarkerItem).ToList();
            }

            if (!items.Any())
                return null;
            ITimelineItem ret = null;
            var scoreToBeat = double.NegativeInfinity;

            foreach (var item in items)
            {
                // test for overlap
                var low = Math.Max(item.start, start);
                var high = Math.Min(item.end, end);
                if (low <= high)
                {
                    var score = high - low;
                    if (score >= scoreToBeat)
                    {
                        scoreToBeat = score;
                        ret = item;
                    }
                }
            }

            return ret;
        }

        public static bool FocusFirstVisibleItem(WindowState state,
            IEnumerable<TrackAsset> focusTracks = null)
        {
            var timeRange = state.timeAreaShownRange;

            var tracks = focusTracks ?? TimelineWindow.instance.treeView.visibleTracks.Where(x => x.IsVisibleRecursive() && x.GetItems().Any());
            var items = tracks.SelectMany(t => t.GetItems().OfType<ClipItem>().Where(x => x.end >= timeRange.x && x.end <= timeRange.y ||
                x.start >= timeRange.x && x.start <= timeRange.y)).ToList();
            var itemFullyInView = items.Where(x => x.end >= timeRange.x && x.end <= timeRange.y &&
                x.start >= timeRange.x && x.start <= timeRange.y);
            var itemToSelect = itemFullyInView.FirstOrDefault() ?? items.FirstOrDefault();
            if (itemToSelect != null)
            {
                SelectionManager.SelectOnly(itemToSelect);
                return true;
            }
            return false;
        }

        public static bool CollapseGroup(WindowState state)
        {
            if (TrackHeadActive())
            {
                var quit = false;
                foreach (var track in SelectionManager.SelectedTracks())
                {
                    if (!track.GetChildTracks().Any())
                        continue;
                    if (!quit && !track.GetCollapsed())
                        quit = true;
                    track.SetCollapsed(true);
                }
                if (quit)
                {
                    state.Refresh();
                    return true;
                }

                var selectedTrack = SelectionManager.SelectedTracks().LastOrDefault();
                var parent = selectedTrack != null ? selectedTrack.parent as TrackAsset : null;
                if (parent)
                {
                    SelectionManager.SelectOnly(parent);
                    FrameTrackHeader(GetVisibleTracks().First(x => x.track == parent));
                    return true;
                }
            }
            return false;
        }

        public static bool SelectLeftItem(WindowState state, bool shift = false)
        {
            if (ClipAreaActive())
            {
                var items = SelectionManager.SelectedItems().ToList();
                var clipOnly = FilterItems(ref items);

                var item = items.Last();
                var prev = item.PreviousItem(clipOnly);
                if (prev != null)
                {
                    if (shift)
                    {
                        if (SelectionManager.Contains(prev))
                            SelectionManager.Remove(item);
                        SelectionManager.Add(prev);
                    }
                    else
                        SelectionManager.SelectOnly(prev);
                    TimelineHelpers.FrameItems(state, new[] {prev});
                }
                else if (item != null && !shift && item.parentTrack != state.editSequence.asset.markerTrack)
                    SelectionManager.SelectOnly(item.parentTrack);
                return true;
            }
            return false;
        }

        public static bool SelectRightItem(WindowState state, bool shift = false)
        {
            if (ClipAreaActive())
            {
                var items = SelectionManager.SelectedItems().ToList();
                var clipOnly = FilterItems(ref items);

                var item = items.Last();
                var next = item.NextItem(clipOnly);
                if (next != null)
                {
                    if (shift)
                    {
                        if (SelectionManager.Contains(next))
                            SelectionManager.Remove(item);
                        SelectionManager.Add(next);
                    }
                    else
                        SelectionManager.SelectOnly(next);
                    TimelineHelpers.FrameItems(state, new[] {next});
                    return true;
                }
            }
            return false;
        }

        public static bool UnCollapseGroup(WindowState state)
        {
            if (TrackHeadActive())
            {
                var quit = false;
                foreach (var track in SelectionManager.SelectedTracks())
                {
                    if (!track.GetChildTracks().Any()) continue;

                    if (!quit && track.GetCollapsed())
                        quit = true;
                    track.SetCollapsed(false);
                }

                if (quit)
                {
                    state.Refresh();
                    return true;
                }

                // Transition to Clip area
                var visibleTracks = GetVisibleTracks().Select(x => x.track).ToList();
                var idx = visibleTracks.IndexOf(SelectionManager.SelectedTracks().Last());
                ITimelineItem item = null;
                for (var i = idx; i < visibleTracks.Count; ++i)
                {
                    var items = visibleTracks[i].GetItems().OfType<ClipItem>();
                    if (!items.Any())
                        continue;
                    item = items.First();
                    break;
                }

                if (item != null)
                {
                    SelectionManager.SelectOnly(item);
                    TimelineHelpers.FrameItems(state, new[] {item});
                    return true;
                }
            }
            return false;
        }

        public static bool SelectUpTrack(bool shift = false)
        {
            if (TrackHeadActive())
            {
                var prevTrack = PreviousTrack(SelectionManager.SelectedTracks().Last());
                if (prevTrack != null)
                {
                    if (shift)
                    {
                        if (SelectionManager.Contains(prevTrack))
                            SelectionManager.Remove(SelectionManager.SelectedTracks().Last());
                        SelectionManager.Add(prevTrack);
                    }
                    else
                        SelectionManager.SelectOnly(prevTrack);
                    FrameTrackHeader(GetVisibleTracks().First(x => x.track == prevTrack));
                }
                return true;
            }
            return false;
        }

        public static bool SelectUpItem(WindowState state)
        {
            if (ClipAreaActive())
            {
                var refItem = SelectionManager.SelectedItems().Last();
                var prevTrack = refItem.parentTrack.PreviousTrack();
                while (prevTrack != null)
                {
                    var selectionItem = GetClosestItem(prevTrack, refItem);
                    if (selectionItem == null)
                    {
                        prevTrack = prevTrack.PreviousTrack();
                        continue;
                    }

                    SelectionManager.SelectOnly(selectionItem);
                    TimelineHelpers.FrameItems(state, new[] {selectionItem});
                    FrameTrackHeader(GetVisibleTracks().First(x => x.track == selectionItem.parentTrack));
                    break;
                }
                return true;
            }

            return false;
        }

        public static bool SelectDownTrack(bool shift = false)
        {
            if (TrackHeadActive())
            {
                var nextTrack = SelectionManager.SelectedTracks().Last().NextTrack();
                if (nextTrack != null)
                {
                    if (shift)
                    {
                        if (SelectionManager.Contains(nextTrack))
                            SelectionManager.Remove(SelectionManager.SelectedTracks().Last());
                        SelectionManager.Add(nextTrack);
                    }
                    else
                        SelectionManager.SelectOnly(nextTrack);

                    FrameTrackHeader(GetVisibleTracks().First(x => x.track == nextTrack));
                }
                return true;
            }

            return false;
        }

        public static bool SelectDownItem(WindowState state)
        {
            if (ClipAreaActive())
            {
                var refItem = SelectionManager.SelectedItems().Last();
                var nextTrack = refItem.parentTrack.NextTrack();
                while (nextTrack != null)
                {
                    var selectionItem = GetClosestItem(nextTrack, refItem);
                    if (selectionItem == null)
                    {
                        nextTrack = nextTrack.NextTrack();
                        continue;
                    }

                    SelectionManager.SelectOnly(selectionItem);
                    TimelineHelpers.FrameItems(state, new[] {selectionItem});
                    FrameTrackHeader(GetVisibleTracks().First(x => x.track == selectionItem.parentTrack));
                    break;
                }
                return true;
            }
            return false;
        }
    }
}
