using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class EditMode
    {
        public enum EditType
        {
            None = -1,
            Mix = 0,
            Ripple = 1,
            Replace = 2
        }

        interface ISubEditMode
        {
            IMoveItemMode moveItemMode { get; }
            IMoveItemDrawer moveItemDrawer { get; }
            ITrimItemMode trimItemMode { get; }
            ITrimItemDrawer trimItemDrawer { get; }
            IAddDeleteItemMode addDeleteItemMode { get; }

            Color color { get; }
            KeyCode clutchKey { get; }

            void Reset();
        }

        class SubEditMode<TMoveMode, TTrimMode, TAddDeleteMode>: ISubEditMode
            where TMoveMode : class, IMoveItemMode, IMoveItemDrawer, new()
            where TTrimMode : class, ITrimItemMode, ITrimItemDrawer, new()
            where TAddDeleteMode : class, IAddDeleteItemMode, new()
        {
            public SubEditMode(Color guiColor, KeyCode key)
            {
                color = guiColor;
                clutchKey = key;
                Reset();
            }

            public void Reset()
            {
                m_MoveItemMode = new TMoveMode();
                m_TrimItemMode = new TTrimMode();
                m_AddDeleteItemMode = new TAddDeleteMode();
            }

            TMoveMode m_MoveItemMode;
            TTrimMode m_TrimItemMode;
            TAddDeleteMode m_AddDeleteItemMode;

            public IMoveItemMode moveItemMode            { get { return m_MoveItemMode; } }
            public IMoveItemDrawer moveItemDrawer        { get { return m_MoveItemMode; } }
            public ITrimItemMode trimItemMode            { get { return m_TrimItemMode; } }
            public ITrimItemDrawer trimItemDrawer        { get { return m_TrimItemMode; } }
            public IAddDeleteItemMode addDeleteItemMode  { get { return m_AddDeleteItemMode; } }
            public Color color { get; }
            public KeyCode clutchKey { get; }
        }

        static readonly Dictionary<EditType, ISubEditMode> k_EditModes = new Dictionary<EditType, ISubEditMode>
        {
            { EditType.Mix,     new SubEditMode<MoveItemModeMix, TrimItemModeMix, AddDeleteItemModeMix>(DirectorStyles.kMixToolColor, KeyCode.Alpha1) },
            { EditType.Ripple,  new SubEditMode<MoveItemModeRipple, TrimItemModeRipple, AddDeleteItemModeRipple>(DirectorStyles.kRippleToolColor, KeyCode.Alpha2) },
            { EditType.Replace, new SubEditMode<MoveItemModeReplace, TrimItemModeReplace, AddDeleteItemModeReplace>(DirectorStyles.kReplaceToolColor, KeyCode.Alpha3) }
        };

        static EditType s_CurrentEditType = EditType.Mix;
        static EditType s_OverrideEditType = EditType.None;

        static ITrimmable s_CurrentTrimItem;
        static TrimEdge s_CurrentTrimDirection;
        static MoveItemHandler s_CurrentMoveItemHandler;
        static EditModeInputHandler s_InputHandler = new EditModeInputHandler();

        static ITrimItemMode trimMode
        {
            get { return GetSubEditMode(editType).trimItemMode; }
        }

        static ITrimItemDrawer trimDrawer
        {
            get { return GetSubEditMode(editType).trimItemDrawer; }
        }

        static IMoveItemMode moveMode
        {
            get { return GetSubEditMode(editType).moveItemMode; }
        }

        static IMoveItemDrawer moveDrawer
        {
            get { return GetSubEditMode(editType).moveItemDrawer; }
        }

        static IAddDeleteItemMode addDeleteMode
        {
            get { return GetSubEditMode(editType).addDeleteItemMode; }
        }

        public static EditModeInputHandler inputHandler
        {
            get { return s_InputHandler; }
        }

        static Color modeColor
        {
            get { return GetSubEditMode(editType).color; }
        }

        public static EditType editType
        {
            get
            {
                if (s_OverrideEditType != EditType.None)
                    return s_OverrideEditType;

                var window = TimelineWindow.instance;
                if (window != null)
                    s_CurrentEditType = window.state.editType;

                return s_CurrentEditType;
            }
            set
            {
                s_CurrentEditType = value;

                var window = TimelineWindow.instance;
                if (window != null)
                    window.state.editType = value;

                s_OverrideEditType = EditType.None;
            }
        }

        static ISubEditMode GetSubEditMode(EditType type)
        {
            var subEditMode = k_EditModes[type];
            if (subEditMode != null)
                return subEditMode;

            Debug.LogError("Unsupported editmode type");
            return null;
        }

        static EditType GetSubEditType(KeyCode key)
        {
            foreach (var subEditMode in k_EditModes)
            {
                if (subEditMode.Value.clutchKey == key)
                    return subEditMode.Key;
            }
            return EditType.None;
        }

        public static void ClearEditMode()
        {
            k_EditModes[editType].Reset();
        }

        public static void BeginTrim(ITimelineItem item, TrimEdge trimDirection)
        {
            var itemToTrim = item as ITrimmable;
            if (itemToTrim == null) return;

            s_CurrentTrimItem = itemToTrim;
            s_CurrentTrimDirection = trimDirection;
            trimMode.OnBeforeTrim(itemToTrim, trimDirection);
            TimelineUndo.PushUndo(itemToTrim.parentTrack, "Trim Clip");
        }

        public static void TrimStart(ITimelineItem item, double time)
        {
            var itemToTrim = item as ITrimmable;
            if (itemToTrim == null) return;

            trimMode.TrimStart(itemToTrim, time);
        }

        public static void TrimEnd(ITimelineItem item, double time, bool affectTimeScale)
        {
            var itemToTrim = item as ITrimmable;
            if (itemToTrim == null) return;

            trimMode.TrimEnd(itemToTrim, time, affectTimeScale);
        }

        public static void DrawTrimGUI(WindowState state, TimelineItemGUI item, TrimEdge edge)
        {
            trimDrawer.DrawGUI(state, item.rect, modeColor, edge);
        }

        public static void FinishTrim()
        {
            s_CurrentTrimItem = null;

            TimelineCursors.ClearCursor();
            ClearEditMode();

            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        public static void BeginMove(MoveItemHandler moveItemHandler)
        {
            s_CurrentMoveItemHandler = moveItemHandler;
            moveMode.BeginMove(s_CurrentMoveItemHandler.movingItems);
        }

        public static void UpdateMove()
        {
            moveMode.UpdateMove(s_CurrentMoveItemHandler.movingItems);
        }

        public static void OnTrackDetach(IEnumerable<ItemsPerTrack> grabbedTrackItems)
        {
            moveMode.OnTrackDetach(grabbedTrackItems);
        }

        public static void HandleTrackSwitch(IEnumerable<ItemsPerTrack> grabbedTrackItems)
        {
            moveMode.HandleTrackSwitch(grabbedTrackItems);
        }

        public static bool AllowTrackSwitch()
        {
            return moveMode.AllowTrackSwitch();
        }

        public static double AdjustStartTime(WindowState state, ItemsPerTrack itemsGroup, double time)
        {
            return moveMode.AdjustStartTime(state, itemsGroup, time);
        }

        public static bool ValidateDrag(ItemsPerTrack itemsGroup)
        {
            return moveMode.ValidateMove(itemsGroup);
        }

        public static void DrawMoveGUI(WindowState state, IEnumerable<MovingItems> movingItems)
        {
            moveDrawer.DrawGUI(state, movingItems, modeColor);
        }

        public static void FinishMove()
        {
            var manipulatedItemsList = s_CurrentMoveItemHandler.movingItems;
            moveMode.FinishMove(manipulatedItemsList);

            foreach (var itemsGroup in manipulatedItemsList)
                foreach (var item in itemsGroup.items)
                    item.parentTrack = itemsGroup.targetTrack;

            s_CurrentMoveItemHandler = null;

            TimelineCursors.ClearCursor();
            ClearEditMode();

            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        public static void FinalizeInsertItemsAtTime(IEnumerable<ItemsPerTrack> newItems, double requestedTime)
        {
            addDeleteMode.InsertItemsAtTime(newItems, requestedTime);
        }

        public static void PrepareItemsDelete(IEnumerable<ItemsPerTrack> newItems)
        {
            addDeleteMode.RemoveItems(newItems);
        }

        public static void HandleModeClutch()
        {
            if (Event.current.type == EventType.KeyDown && EditorGUI.IsEditingTextField())
                return;

            var prevType = editType;

            if (Event.current.type == EventType.KeyDown)
            {
                var clutchEditType = GetSubEditType(Event.current.keyCode);
                if (clutchEditType != EditType.None)
                {
                    s_OverrideEditType = clutchEditType;
                    Event.current.Use();
                }
            }
            else if (Event.current.type == EventType.KeyUp)
            {
                var clutchEditType = GetSubEditType(Event.current.keyCode);
                if (clutchEditType == s_OverrideEditType)
                {
                    s_OverrideEditType = EditType.None;
                    Event.current.Use();
                }
            }

            if (prevType != editType)
            {
                if (s_CurrentTrimItem != null)
                {
                    trimMode.OnBeforeTrim(s_CurrentTrimItem, s_CurrentTrimDirection);
                }
                else if (s_CurrentMoveItemHandler != null)
                {
                    if (s_CurrentMoveItemHandler.movingItems == null)
                    {
                        s_CurrentMoveItemHandler = null;
                        return;
                    }

                    foreach (var movingItems in s_CurrentMoveItemHandler.movingItems)
                    {
                        if (movingItems != null && movingItems.HasAnyDetachedParents())
                        {
                            foreach (var items in movingItems.items)
                            {
                                items.parentTrack = movingItems.originalTrack;
                            }
                        }
                    }

                    var movingSelection = s_CurrentMoveItemHandler.movingItems;

                    // Handle clutch key transition if needed
                    GetSubEditMode(prevType).moveItemMode.OnModeClutchExit(movingSelection);
                    moveMode.OnModeClutchEnter(movingSelection);

                    moveMode.BeginMove(movingSelection);
                    moveMode.HandleTrackSwitch(movingSelection);

                    UpdateMove();
                    s_CurrentMoveItemHandler.RefreshPreviewItems();

                    TimelineWindow.instance.state.rebuildGraph = true; // TODO Rebuild only if parent changed
                }

                TimelineWindow.instance.Repaint(); // TODO Refresh the toolbar without doing a full repaint?
            }
        }
    }
}
