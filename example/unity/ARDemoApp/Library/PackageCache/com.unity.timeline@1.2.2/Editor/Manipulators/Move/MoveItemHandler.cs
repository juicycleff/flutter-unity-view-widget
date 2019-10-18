using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class MoveItemHandler : IAttractable, IAttractionHandler
    {
        bool m_Grabbing;

        MovingItems m_LeftMostMovingItems;
        MovingItems m_RightMostMovingItems;

        HashSet<TimelineItemGUI> m_ItemGUIs;
        ItemsGroup m_ItemsGroup;

        public TrackAsset targetTrack { get; private set; }

        public bool allowTrackSwitch { get; private set; }

        int m_GrabbedModalUndoGroup = -1;

        readonly WindowState m_State;

        public MovingItems[] movingItems { get; private set; }

        public MoveItemHandler(WindowState state)
        {
            m_State = state;
        }

        public void Grab(IEnumerable<ITimelineItem> items, TrackAsset referenceTrack)
        {
            Grab(items, referenceTrack, Vector2.zero);
        }

        public void Grab(IEnumerable<ITimelineItem> items, TrackAsset referenceTrack, Vector2 mousePosition)
        {
            if (items == null) return;

            items = items.ToArray(); // Cache enumeration result

            if (!items.Any()) return;

            m_GrabbedModalUndoGroup = Undo.GetCurrentGroup();

            var trackItems = items.GroupBy(c => c.parentTrack).ToArray();
            var trackItemsCount = trackItems.Length;
            var tracks = items.Select(c => c.parentTrack).Where(x => x != null).Distinct();

            movingItems = new MovingItems[trackItemsCount];

            allowTrackSwitch = trackItemsCount == 1 && !trackItems.SelectMany(x => x).Any(x => x is MarkerItem); // For now, track switch is only supported when all items are on the same track and there are no items
            foreach (var sourceTrack in tracks)
            {
                // one push per track handles all the clips on the track
                TimelineUndo.PushUndo(sourceTrack, "Move Items");

                // push all markers on the track because of ripple
                foreach (var marker in sourceTrack.GetMarkers().OfType<ScriptableObject>())
                    TimelineUndo.PushUndo(marker, "Move Items");
            }

            for (var i = 0; i < trackItemsCount; ++i)
            {
                var track = trackItems[i].Key;
                var grabbedItems = new MovingItems(m_State, track, trackItems[i].ToArray(), referenceTrack, mousePosition, allowTrackSwitch);
                movingItems[i] = grabbedItems;
            }

            m_LeftMostMovingItems = null;
            m_RightMostMovingItems = null;

            foreach (var grabbedTrackItems in movingItems)
            {
                if (m_LeftMostMovingItems == null || m_LeftMostMovingItems.start > grabbedTrackItems.start)
                    m_LeftMostMovingItems = grabbedTrackItems;

                if (m_RightMostMovingItems == null || m_RightMostMovingItems.end < grabbedTrackItems.end)
                    m_RightMostMovingItems = grabbedTrackItems;
            }

            m_ItemGUIs = new HashSet<TimelineItemGUI>();
            m_ItemsGroup = new ItemsGroup(items);

            foreach (var item in items)
                m_ItemGUIs.Add(item.gui);

            targetTrack = referenceTrack;

            EditMode.BeginMove(this);
            m_Grabbing = true;
        }

        public void Drop()
        {
            if (IsValidDrop())
            {
                foreach (var grabbedItems in movingItems)
                {
                    var track = grabbedItems.targetTrack;
                    TimelineUndo.PushUndo(track, "Move Items");

                    if (EditModeUtils.IsInfiniteTrack(track) && grabbedItems.clips.Any())
                        ((AnimationTrack)track).ConvertToClipMode();
                }

                EditMode.FinishMove();

                Done();
            }
            else
            {
                Cancel();
            }

            EditMode.ClearEditMode();
        }

        bool IsValidDrop()
        {
            return movingItems.All(g => g.canDrop);
        }

        void Cancel()
        {
            if (!m_Grabbing)
                return;

            // TODO fix undo reselection persistency
            // identify the clips by their playable asset, since that reference will survive the undo
            // This is a workaround, until a more persistent fix for selection of clips across Undo can be found
            var assets = movingItems.SelectMany(x => x.clips).Select(x => x.asset);

            Undo.RevertAllDownToGroup(m_GrabbedModalUndoGroup);

            // reselect the clips from the original clip
            var clipsToSelect = movingItems.Select(x => x.originalTrack).SelectMany(x => x.GetClips()).Where(x => assets.Contains(x.asset)).ToArray();
            SelectionManager.RemoveTimelineSelection();

            foreach (var c in clipsToSelect)
                SelectionManager.Add(c);

            Done();
        }

        void Done()
        {
            foreach (var movingItem in movingItems)
            {
                foreach (var item in movingItem.items)
                {
                    if (item.gui != null)
                        item.gui.isInvalid = false;
                }
            }

            movingItems = null;
            m_LeftMostMovingItems = null;
            m_RightMostMovingItems = null;
            m_Grabbing = false;

            m_State.Refresh();
        }

        public double start { get { return m_ItemsGroup.start; } }

        public double end { get { return m_ItemsGroup.end; } }

        public bool ShouldSnapTo(ISnappable snappable)
        {
            var itemGUI = snappable as TimelineItemGUI;
            return itemGUI != null && !m_ItemGUIs.Contains(itemGUI);
        }

        public void UpdateTrackTarget(TrackAsset track)
        {
            if (!EditMode.AllowTrackSwitch())
                return;

            targetTrack = track;

            var targetTracksChanged = false;

            foreach (var grabbedItem in movingItems)
            {
                var prevTrackGUI = grabbedItem.targetTrack;

                grabbedItem.SetReferenceTrack(track);

                targetTracksChanged = grabbedItem.targetTrack != prevTrackGUI;
            }

            if (targetTracksChanged)
                EditMode.HandleTrackSwitch(movingItems);

            RefreshPreviewItems();

            m_State.rebuildGraph |= targetTracksChanged;
        }

        public void OnGUI(Event evt)
        {
            if (!m_Grabbing)
                return;

            if (evt.type != EventType.Repaint)
                return;

            var isValid = IsValidDrop();

            using (new GUIViewportScope(m_State.GetWindow().sequenceContentRect))
            {
                foreach (var grabbedClip in movingItems)
                {
                    grabbedClip.RefreshBounds(m_State, evt.mousePosition);

                    if (!grabbedClip.HasAnyDetachedParents())
                        continue;

                    grabbedClip.Draw(isValid);
                }

                if (isValid)
                {
                    EditMode.DrawMoveGUI(m_State, movingItems);
                }
                else
                {
                    TimelineCursors.ClearCursor();
                }
            }
        }

        public void OnAttractedEdge(IAttractable attractable, ManipulateEdges manipulateEdges, AttractedEdge edge, double time)
        {
            double offset;

            if (edge == AttractedEdge.Right)
            {
                var duration = end - start;
                var startTime = time - duration;
                startTime = EditMode.AdjustStartTime(m_State, m_RightMostMovingItems, startTime);

                offset = startTime + duration - end;
            }
            else
            {
                if (edge == AttractedEdge.Left)
                    time = EditMode.AdjustStartTime(m_State, m_LeftMostMovingItems, time);

                offset = time - start;
            }

            if (start + offset < 0.0)
                offset = -start;

            if (!offset.Equals(0.0))
            {
                foreach (var grabbedClips in movingItems)
                    grabbedClips.start += offset;

                EditMode.UpdateMove();

                RefreshPreviewItems();
            }
        }

        public void RefreshPreviewItems()
        {
            foreach (var movingItemsGroup in movingItems)
            {
                // Check validity
                var valid = ValidateItemDrag(movingItemsGroup);

                foreach (var item in movingItemsGroup.items)
                {
                    if (item.gui != null)
                        item.gui.isInvalid = !valid;
                }

                movingItemsGroup.canDrop = valid;
            }
        }

        static bool ValidateItemDrag(ItemsPerTrack itemsGroup)
        {
            //TODO-marker: this is to prevent the drag operation from being canceled when moving only markers
            if (itemsGroup.clips.Any())
            {
                if (itemsGroup.targetTrack == null)
                    return false;

                if (itemsGroup.targetTrack.lockedInHierarchy)
                    return false;

                if (itemsGroup.items.Any(i => !i.IsCompatibleWithTrack(itemsGroup.targetTrack)))
                    return false;

                return EditMode.ValidateDrag(itemsGroup);
            }

            return true;
        }

        public void OnTrackDetach()
        {
            EditMode.OnTrackDetach(movingItems);
        }
    }
}
