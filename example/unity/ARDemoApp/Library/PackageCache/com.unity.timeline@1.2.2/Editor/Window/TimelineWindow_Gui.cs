using System;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        struct MarkerOverlay
        {
            public IMarker marker;
            public Rect    rect;
            public bool    isSelected;
            public bool    isCollapsed;
            public MarkerEditor editor;
        }


        enum TimelineItemArea
        {
            Header,
            Lines
        }

        [SerializeField] float m_HierarchySplitterPerc = WindowConstants.hierarchySplitterDefaultPercentage;

        static internal readonly TimelineMode s_ActiveMode = new TimelineActiveMode();
        static internal readonly TimelineMode s_EditAssetMode = new TimelineAssetEditionMode();
        static internal readonly TimelineMode s_InactiveMode = new TimelineInactiveMode();
        static internal readonly TimelineMode s_DisabledMode = new TimelineDisabledMode();
        static internal readonly TimelineMode s_PrefabOutOfContextMode = new TimelineAssetEditionMode();
        static internal readonly TimelineMode s_ReadonlyMode = new TimelineReadOnlyMode();

        int m_SplitterCaptured;
        float m_VerticalScrollBarSize, m_HorizontalScrollBarSize;

        List<MarkerOverlay> m_OverlayQueue = new List<MarkerOverlay>(100);


        float headerHeight
        {
            get
            {
                return WindowConstants.markerRowYPosition + (state.showMarkerHeader ? WindowConstants.markerRowHeight : 0.0f);
            }
        }

        public Rect markerHeaderRect
        {
            get { return new Rect(0.0f, WindowConstants.markerRowYPosition, state.sequencerHeaderWidth, WindowConstants.markerRowHeight); }
        }

        public Rect markerContentRect
        {
            get { return Rect.MinMaxRect(state.sequencerHeaderWidth, WindowConstants.markerRowYPosition, position.width, WindowConstants.markerRowYPosition + WindowConstants.markerRowHeight); }
        }

        Rect trackRect
        {
            get
            {
                var yMinHeight = headerHeight;
                return new Rect(0, yMinHeight, position.width, position.height - yMinHeight - horizontalScrollbarHeight);
            }
        }

        public Rect sequenceRect
        {
            get { return new Rect(0.0f, WindowConstants.markerRowYPosition, position.width - WindowConstants.sliderWidth, position.height - WindowConstants.timeAreaYPosition); }
        }

        public Rect sequenceHeaderRect
        {
            get { return new Rect(0.0f, WindowConstants.markerRowYPosition, state.sequencerHeaderWidth, position.height - WindowConstants.timeAreaYPosition); }
        }

        public Rect sequenceContentRect
        {
            get
            {
                return new Rect(
                    state.sequencerHeaderWidth,
                    WindowConstants.markerRowYPosition,
                    position.width - state.sequencerHeaderWidth - (treeView != null && treeView.showingVerticalScrollBar ? WindowConstants.sliderWidth : 0),
                    position.height - WindowConstants.markerRowYPosition - horizontalScrollbarHeight);
            }
        }

        public float verticalScrollbarWidth
        {
            get
            {
                return m_VerticalScrollBarSize;
            }
        }

        public float horizontalScrollbarHeight
        {
            get { return m_HorizontalScrollBarSize; }
        }

        float breadCrumbAreaWidth
        {
            get
            {
                return state.timeAreaRect.width - WindowConstants.selectorWidth - WindowConstants.cogButtonWidth - WindowConstants.cogButtonPadding;
            }
        }

        internal TimelineMode currentMode
        {
            get
            {
                if (state == null || state.editSequence.asset == null)
                    return s_InactiveMode;
                if (state.editSequence.isReadOnly)
                    return s_ReadonlyMode;
                if (state.editSequence.director == null || state.masterSequence.director == null)
                    return s_EditAssetMode;

                if (PrefabUtility.IsPartOfPrefabAsset(state.editSequence.director))
                {
                    var stage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (stage == null || !stage.IsPartOfPrefabContents(state.editSequence.director.gameObject))
                        return s_PrefabOutOfContextMode;
                }

                if (!state.masterSequence.director.isActiveAndEnabled)
                    return s_DisabledMode;

                return s_ActiveMode;
            }
        }

        void DoLayout()
        {
            var rawType = Event.current.rawType; // TODO: rawType seems to be broken after calling Use(), use this Hack and remove it once it's fixed.
            var mousePosition = Event.current.mousePosition; // mousePosition is also affected by this bug and does not reflect the original position after a Use()

            Initialize();
            HandleSplitterResize();

            var processManipulators = Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout;

            if (processManipulators)
            {
                // Update what's under mouse the cursor
                PickerUtils.DoPick(state, mousePosition);

                if (state.editSequence.asset != null)
                    m_PreTreeViewControl.HandleManipulatorsEvents(state);
            }

            SequencerGUI();

            if (processManipulators)
            {
                if (state.editSequence.asset != null)
                    m_PostTreeViewControl.HandleManipulatorsEvents(state);
            }

            m_RectangleSelect.OnGUI(state, rawType, mousePosition);
            m_RectangleZoom.OnGUI(state, rawType, mousePosition);
        }

        void TimelineSectionGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(position.width - state.sequencerHeaderWidth));
                {
                    DoSequenceSelectorGUI();
                    DoBreadcrumbGUI();
                    OptionsGUI();
                }
                GUILayout.EndHorizontal();

                TimelineGUI();
            }
            GUILayout.EndVertical();
        }

        void SplitterGUI()
        {
            if (!state.IsEditingAnEmptyTimeline())
            {
                var splitterLineRect = new Rect(state.sequencerHeaderWidth - 1.0f, 0.0f, 2.0f, clientArea.height);
                EditorGUI.DrawRect(splitterLineRect, DirectorStyles.Instance.customSkin.colorTopOutline3);
            }
        }

        void TrackViewsGUI()
        {
            using (new GUIViewportScope(trackRect))
            {
                TracksGUI(trackRect, state, currentMode.TrackState(state));
            }
        }

        void UserOverlaysGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            // the rect containing the time area plus the time ruler
            var screenRect = new Rect(
                state.sequencerHeaderWidth,
                WindowConstants.timeAreaYPosition,
                position.width - state.sequencerHeaderWidth - (treeView != null && treeView.showingVerticalScrollBar ? WindowConstants.sliderWidth : 0),
                position.height - WindowConstants.timeAreaYPosition - horizontalScrollbarHeight);

            var startTime = state.PixelToTime(screenRect.xMin);
            var endTime = state.PixelToTime(screenRect.xMax);

            using (new GUIViewportScope(screenRect))
            {
                foreach (var entry in m_OverlayQueue)
                {
                    var uiState = MarkerUIStates.None;
                    if (entry.isCollapsed)
                        uiState |= MarkerUIStates.Collapsed;
                    if (entry.isSelected)
                        uiState |= MarkerUIStates.Selected;
                    var region = new MarkerOverlayRegion(GUIClip.Clip(entry.rect), screenRect, startTime, endTime);
                    try
                    {
                        entry.editor.DrawOverlay(entry.marker, uiState, region);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            m_OverlayQueue.Clear();
        }

        void DrawHeaderBackground()
        {
            var rect = state.timeAreaRect;
            rect.xMin = 0.0f;
            EditorGUI.DrawRect(rect, DirectorStyles.Instance.customSkin.colorTimelineBackground);
        }

        void HandleBottomFillerDragAndDrop(Rect rect)
        {
            if (Event.current.type != EventType.DragUpdated &&
                Event.current.type != EventType.DragExited &&
                Event.current.type != EventType.DragPerform)
                return;

            if (instance.treeView == null || instance.treeView.timelineDragging == null)
                return;

            if (!rect.Contains(Event.current.mousePosition))
                return;

            instance.treeView.timelineDragging.DragElement(null, new Rect(), -1);
        }

        void DrawHeaderBackgroundBottomFiller()
        {
            var rect = sequenceRect;
            rect.yMin = rect.yMax;
            rect.yMax = rect.yMax + WindowConstants.sliderWidth;
            if (state.editSequence.asset != null && !state.IsEditingAnEmptyTimeline())
            {
                rect.width = state.sequencerHeaderWidth;
            }
            using (new GUIViewportScope(rect))
            {
                Graphics.DrawBackgroundRect(state, rect);
            }

            HandleBottomFillerDragAndDrop(rect);
        }

        void SequencerGUI()
        {
            var duration = state.editSequence.duration;

            DrawHeaderBackground();
            DurationGUI(TimelineItemArea.Header, duration);

            GUILayout.BeginHorizontal();
            {
                SequencerHeaderGUI();
                TimelineSectionGUI();
            }
            GUILayout.EndHorizontal();

            TrackViewsGUI();
            MarkerHeaderGUI();
            UserOverlaysGUI();

            DurationGUI(TimelineItemArea.Lines, duration);
            PlayRangeGUI(TimelineItemArea.Lines);
            TimeCursorGUI(TimelineItemArea.Lines);
            DrawHeaderBackgroundBottomFiller();

            SubTimelineRangeGUI();

            PlayRangeGUI(TimelineItemArea.Header);
            TimeCursorGUI(TimelineItemArea.Header);

            SplitterGUI();
        }

        void SubTimelineRangeGUI()
        {
            if (!state.IsEditingASubTimeline() || state.IsEditingAnEmptyTimeline()) return;

            var subTimelineOverlayColor = DirectorStyles.Instance.customSkin.colorSubSequenceOverlay;

            var range = state.editSequence.GetEvaluableRange();
            var area = new Vector2(state.TimeToPixel(range.start), state.TimeToPixel(range.end));

            var fullRect = sequenceContentRect;
            fullRect.yMin -= state.timeAreaRect.height;

            if (fullRect.xMin < area.x)
            {
                var before = fullRect;
                before.xMin = fullRect.xMin;
                before.xMax = Mathf.Min(area.x, fullRect.xMax);
                EditorGUI.DrawRect(before, subTimelineOverlayColor);
            }

            if (fullRect.xMax > area.y)
            {
                var after = fullRect;
                after.xMin = Mathf.Max(area.y, fullRect.xMin);
                after.xMax = fullRect.xMax;
                EditorGUI.DrawRect(after, subTimelineOverlayColor);

                // Space above the vertical scrollbar
                after.xMin = after.xMax;
                after.width = verticalScrollbarWidth;
                after.yMax = state.timeAreaRect.y + state.timeAreaRect.height + (state.showMarkerHeader ? WindowConstants.markerRowHeight : 0.0f);
                EditorGUI.DrawRect(after, subTimelineOverlayColor);
            }
        }

        void HandleSplitterResize()
        {
            state.mainAreaWidth = position.width;

            if (state.editSequence.asset == null)
                return;

            // Sequencer Header Splitter : The splitter has 6 pixels wide,center it around m_State.sequencerHeaderWidth. That's why there's this -3.
            Rect sequencerHeaderSplitterRect = new Rect(state.sequencerHeaderWidth - 3.0f, 0.0f, 6.0f, clientArea.height);
            EditorGUIUtility.AddCursorRect(sequencerHeaderSplitterRect, MouseCursor.SplitResizeLeftRight);

            if (Event.current.type == EventType.MouseDown)
            {
                if (sequencerHeaderSplitterRect.Contains(Event.current.mousePosition))
                    m_SplitterCaptured = 1;
            }

            if (m_SplitterCaptured > 0)
            {
                if (Event.current.type == EventType.MouseUp)
                {
                    m_SplitterCaptured = 0;
                    Event.current.Use();
                }

                if (Event.current.type == EventType.MouseDrag)
                {
                    if (m_SplitterCaptured == 1)
                    {
                        var percInc = Event.current.delta.x / position.width;
                        m_HierarchySplitterPerc = Mathf.Clamp(m_HierarchySplitterPerc + percInc, WindowConstants.minHierarchySplitter, WindowConstants.maxHierarchySplitter);
                        state.sequencerHeaderWidth += Event.current.delta.x;
                    }

                    Event.current.Use();
                }
            }
        }

        void OptionsGUI()
        {
            if (currentMode.headerState.options == TimelineModeGUIState.Hidden || state.editSequence.asset == null)
                return;

            using (new EditorGUI.DisabledScope(currentMode.headerState.options == TimelineModeGUIState.Disabled))
            {
                GUILayout.FlexibleSpace();
                if (EditorGUILayout.DropdownButton(DirectorStyles.cogIcon, FocusType.Keyboard, EditorStyles.toolbarButton))
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(EditorGUIUtility.TrTextContent("Seconds"), !state.timeInFrames, ChangeTimeCode, "seconds");
                    menu.AddItem(EditorGUIUtility.TrTextContent("Frames"), state.timeInFrames, ChangeTimeCode, "frames");
                    menu.AddSeparator("");

                    TimeAreaContextMenu.AddTimeAreaMenuItems(menu, state);

                    menu.AddSeparator("");

                    bool standardFrameRate = false;
                    standardFrameRate |= AddStandardFrameRateMenu(menu, "Frame Rate/Film (24)", 24.0f);
                    standardFrameRate |= AddStandardFrameRateMenu(menu, "Frame Rate/PAL (25)", 25.0f);
                    standardFrameRate |= AddStandardFrameRateMenu(menu, "Frame Rate/NTSC (29.97)", 29.97f);
                    standardFrameRate |= AddStandardFrameRateMenu(menu, "Frame Rate/30", 30.0f);
                    standardFrameRate |= AddStandardFrameRateMenu(menu, "Frame Rate/50", 50.0f);
                    standardFrameRate |= AddStandardFrameRateMenu(menu, "Frame Rate/60", 60.0f);

                    if (standardFrameRate)
                        menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Frame Rate/Custom"));
                    else
                        menu.AddItem(EditorGUIUtility.TrTextContent("Frame Rate/Custom (" + state.editSequence.frameRate + ")"), true, () => {});

                    menu.AddSeparator("");
                    if (state.playRangeEnabled)
                    {
                        menu.AddItem(EditorGUIUtility.TrTextContent("Play Range Mode/Loop"), state.playRangeLoopMode, () => state.playRangeLoopMode = true);
                        menu.AddItem(EditorGUIUtility.TrTextContent("Play Range Mode/Once"), !state.playRangeLoopMode, () => state.playRangeLoopMode = false);
                    }
                    else
                    {
                        menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Play Range Mode"));
                    }
                    menu.AddItem(EditorGUIUtility.TrTextContent("Playback Scrolling mode/None"), state.autoScrollMode == PlaybackScrollMode.None, () => state.autoScrollMode = PlaybackScrollMode.None);
                    menu.AddItem(EditorGUIUtility.TrTextContent("Playback Scrolling mode/Smooth"), state.autoScrollMode == PlaybackScrollMode.Smooth, () => state.autoScrollMode = PlaybackScrollMode.Smooth);
                    menu.AddItem(EditorGUIUtility.TrTextContent("Playback Scrolling mode/Pan"), state.autoScrollMode == PlaybackScrollMode.Pan, () => state.autoScrollMode = PlaybackScrollMode.Pan);
                    menu.AddSeparator("");
                    menu.AddItem(EditorGUIUtility.TrTextContent("Show Audio Waveforms"), state.showAudioWaveform, () =>
                    {
                        state.showAudioWaveform = !state.showAudioWaveform;
                    });
                    menu.AddItem(EditorGUIUtility.TrTextContent("Enable Audio Scrubbing"), !state.muteAudioScrubbing, () => state.muteAudioScrubbing = !state.muteAudioScrubbing);

                    menu.AddSeparator("");
                    menu.AddItem(EditorGUIUtility.TrTextContent("Snap to Frame"), state.frameSnap, () => state.frameSnap = !state.frameSnap);
                    menu.AddItem(EditorGUIUtility.TrTextContent("Edge Snap"), state.edgeSnaps, () => state.edgeSnaps = !state.edgeSnaps);

                    if (Unsupported.IsDeveloperMode())
                    {
                        menu.AddItem(EditorGUIUtility.TrTextContent("Show Snapping Debug"), SnapEngine.displayDebugLayout,
                            () => SnapEngine.displayDebugLayout = !SnapEngine.displayDebugLayout);

                        menu.AddItem(EditorGUIUtility.TrTextContent("Debug TimeArea"), false,
                            () =>
                                Debug.LogFormat("translation: {0}   scale: {1}   rect: {2}   shownRange: {3}", m_TimeArea.translation, m_TimeArea.scale, m_TimeArea.rect, m_TimeArea.shownArea));

                        menu.AddItem(EditorGUIUtility.TrTextContent("Edit Skin"), false, () => Selection.activeObject = DirectorStyles.Instance.customSkin);

                        menu.AddItem(EditorGUIUtility.TrTextContent("Show QuadTree Debugger"), state.showQuadTree,
                            () => state.showQuadTree = !state.showQuadTree);
                    }

                    menu.ShowAsContext();
                }
            }
        }

        bool AddStandardFrameRateMenu(GenericMenu menu, string name, float value)
        {
            bool on = state.editSequence.frameRate.Equals(value);
            if (state.editSequence.isReadOnly)
            {
                menu.AddDisabledItem(EditorGUIUtility.TextContent(name), on);
            }
            else
            {
                menu.AddItem(EditorGUIUtility.TextContent(name), on, r =>
                {
                    state.editSequence.frameRate = value;
                }, value);
            }
            return on;
        }

        void ChangeTimeCode(object obj)
        {
            string format = obj.ToString();
            if (format == "frames")
            {
                state.timeInFrames = true;
            }
            else
            {
                state.timeInFrames = false;
            }
        }

        public void AddUserOverlay(IMarker marker, Rect rect, MarkerEditor editor, bool collapsed, bool selected)
        {
            if (marker == null)
                throw new ArgumentNullException("marker");
            if (editor == null)
                throw new ArgumentNullException("editor");

            m_OverlayQueue.Add(new MarkerOverlay()
            {
                isCollapsed = collapsed,
                isSelected = selected,
                marker = marker,
                rect = rect,
                editor = editor
            }
            );
        }
    }
}
