using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class ClearSelection : Manipulator
    {
        protected override bool MouseDown(Event evt, WindowState state)
        {
            // If we hit this point this means no one used the mouse down events. We can safely clear the selection if needed
            if (evt.button != 0)
                return false;

            var window = state.GetWindow();

            if (!window.sequenceRect.Contains(evt.mousePosition))
                return false;

            if (ItemSelection.CanClearSelection(evt))
            {
                SelectionManager.Clear();
                return true;
            }

            return false;
        }
    }

    static class ItemSelection
    {
        public static bool CanClearSelection(Event evt)
        {
            return !evt.control && !evt.command && !evt.shift;
        }

        public static void RangeSelectItems(ITimelineItem lastItemToSelect)
        {
            var selectSorted = SelectionManager.SelectedItems().ToList();
            var firstSelect = selectSorted.FirstOrDefault();
            if (firstSelect == null)
            {
                SelectionManager.Add(lastItemToSelect);
                return;
            }

            var allTracks = TimelineEditor.inspectedAsset.flattenedTracks;
            var allItems = allTracks.SelectMany(ItemsUtils.GetItems).ToList();
            TimelineHelpers.RangeSelect(allItems, selectSorted, lastItemToSelect, SelectionManager.Add, SelectionManager.Remove);
        }

        public static ISelectable HandleSingleSelection(Event evt)
        {
            var item = PickerUtils.PickedLayerableOfType<ISelectable>();

            if (item != null)
            {
                var selected = item.IsSelected();
                if (!selected && CanClearSelection(evt))
                    SelectionManager.Clear();

                if (evt.modifiers == EventModifiers.Shift)
                {
                    if (!selected)
                        RangeSelectItems((item as TimelineItemGUI).item);
                }
                else
                {
                    HandleItemSelection(evt, item);
                }
            }

            return item;
        }

        public static void HandleItemSelection(Event evt, ISelectable item)
        {
            if (evt.modifiers == ManipulatorsUtils.actionModifier)
            {
                if (item.IsSelected())
                    item.Deselect();
                else
                    item.Select();
            }
            else
            {
                if (!item.IsSelected())
                    item.Select();
            }
        }
    }

    class SelectAndMoveItem : Manipulator
    {
        bool m_Dragged;
        SnapEngine m_SnapEngine;
        TimeAreaAutoPanner m_TimeAreaAutoPanner;
        Vector2 m_MouseDownPosition;

        bool m_HorizontalMovementDone;
        bool m_VerticalMovementDone;

        MoveItemHandler m_MoveItemHandler;
        bool m_CycleMarkersPending;

        protected override bool MouseDown(Event evt, WindowState state)
        {
            if (evt.alt || evt.button != 0)
                return false;

            m_Dragged = false;

            // Cycling markers and selection are mutually exclusive operations
            if (!HandleMarkerCycle() && !HandleSingleSelection(evt))
                return false;

            m_MouseDownPosition = evt.mousePosition;
            m_VerticalMovementDone = false;
            m_HorizontalMovementDone = false;

            return true;
        }

        protected override bool MouseUp(Event evt, WindowState state)
        {
            if (!m_Dragged)
            {
                var item = PickerUtils.PickedLayerableOfType<ISelectable>();

                if (item == null)
                    return false;

                if (!item.IsSelected())
                    return false;

                // Re-selecting an item part of a multi-selection should only keep this item selected.
                if (SelectionManager.Count() > 1 && ItemSelection.CanClearSelection(evt))
                {
                    SelectionManager.Clear();
                    item.Select();
                    return true;
                }

                if (m_CycleMarkersPending)
                {
                    m_CycleMarkersPending = false;
                    TimelineMarkerClusterGUI.CycleMarkers();
                    return true;
                }

                return false;
            }

            m_TimeAreaAutoPanner = null;

            DropItems();

            m_SnapEngine = null;
            m_MoveItemHandler = null;

            state.Evaluate();
            state.RemoveCaptured(this);
            m_Dragged = false;
            TimelineCursors.ClearCursor();

            return true;
        }

        protected override bool DoubleClick(Event evt, WindowState state)
        {
            return MouseDown(evt, state) && MouseUp(evt, state);
        }

        protected override bool MouseDrag(Event evt, WindowState state)
        {
            if (state.editSequence.isReadOnly)
                return false;

            // case 1099285 - ctrl-click can cause no clips to be selected
            var selectedItemsGUI = SelectionManager.SelectedItems();
            if (!selectedItemsGUI.Any())
            {
                m_Dragged = false;
                return false;
            }

            const float hDeadZone = 5.0f;
            const float vDeadZone = 5.0f;

            bool vDone = m_VerticalMovementDone || Math.Abs(evt.mousePosition.y - m_MouseDownPosition.y) > vDeadZone;
            bool hDone = m_HorizontalMovementDone || Math.Abs(evt.mousePosition.x - m_MouseDownPosition.x) > hDeadZone;

            m_CycleMarkersPending = false;

            if (!m_Dragged)
            {
                var canStartMove = vDone || hDone;

                if (canStartMove)
                {
                    state.AddCaptured(this);
                    m_Dragged = true;

                    var referenceTrack = GetTrackDropTargetAt(state, m_MouseDownPosition);

                    foreach (var item in selectedItemsGUI)
                        item.gui.StartDrag();

                    m_MoveItemHandler = new MoveItemHandler(state);

                    m_MoveItemHandler.Grab(selectedItemsGUI, referenceTrack, m_MouseDownPosition);

                    m_SnapEngine = new SnapEngine(m_MoveItemHandler, m_MoveItemHandler, ManipulateEdges.Both,
                        state, m_MouseDownPosition);

                    m_TimeAreaAutoPanner = new TimeAreaAutoPanner(state);
                }
            }

            if (!m_VerticalMovementDone)
            {
                m_VerticalMovementDone = vDone;

                if (m_VerticalMovementDone)
                    m_MoveItemHandler.OnTrackDetach();
            }

            if (!m_HorizontalMovementDone)
            {
                m_HorizontalMovementDone = hDone;
            }

            if (m_Dragged)
            {
                if (m_HorizontalMovementDone)
                    m_SnapEngine.Snap(evt.mousePosition, evt.modifiers);

                if (m_VerticalMovementDone)
                {
                    var track = GetTrackDropTargetAt(state, evt.mousePosition);
                    m_MoveItemHandler.UpdateTrackTarget(track);
                }

                state.Evaluate();
            }

            return true;
        }

        public override void Overlay(Event evt, WindowState state)
        {
            if (!m_Dragged)
                return;

            if (m_TimeAreaAutoPanner != null)
                m_TimeAreaAutoPanner.OnGUI(evt);

            m_MoveItemHandler.OnGUI(evt);

            if (!m_MoveItemHandler.allowTrackSwitch || m_MoveItemHandler.targetTrack != null)
            {
                TimeIndicator.Draw(state, m_MoveItemHandler.start, m_MoveItemHandler.end);
                m_SnapEngine.OnGUI();
            }
        }

        bool HandleMarkerCycle()
        {
            m_CycleMarkersPending = TimelineMarkerClusterGUI.CanCycleMarkers();
            return m_CycleMarkersPending;
        }

        bool HandleSingleSelection(Event evt)
        {
            return ItemSelection.HandleSingleSelection(evt) != null;
        }

        void DropItems()
        {
            // Order matters here: m_MoveItemHandler.movingItems is destroyed during call to Drop()
            foreach (var movingItem in m_MoveItemHandler.movingItems)
            {
                foreach (var item in movingItem.items)
                    item.gui.StopDrag();
            }

            m_MoveItemHandler.Drop();
        }

        static TrackAsset GetTrackDropTargetAt(WindowState state, Vector2 point)
        {
            var track = state.spacePartitioner.GetItemsAtPosition<IRowGUI>(point).FirstOrDefault();
            return track != null ? track.asset : null;
        }
    }
}
