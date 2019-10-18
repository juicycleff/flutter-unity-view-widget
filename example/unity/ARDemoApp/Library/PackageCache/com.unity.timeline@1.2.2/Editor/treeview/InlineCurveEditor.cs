using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    interface IClipCurveEditorOwner
    {
        ClipCurveEditor clipCurveEditor { get; }
        bool inlineCurvesSelected { get; set; }
        bool showLoops { get; }
        TrackAsset owner { get; }
    }

    class InlineCurveResizeHandle : IBounds
    {
        public Rect boundingRect { get; private set; }

        public TimelineTrackGUI trackGUI { get; }

        public InlineCurveResizeHandle(TimelineTrackGUI trackGUI)
        {
            this.trackGUI = trackGUI;
        }

        public void Draw(Rect headerRect, Rect trackRect, WindowState state)
        {
            var rect = new Rect(headerRect.xMax + 4, headerRect.yMax - 5.0f, trackRect.width - 4, 5.0f);

            var handleColor = Handles.color;
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(1.0f,
                new Vector3(rect.x, rect.yMax, 0.0f),
                new Vector3(rect.xMax, rect.yMax, 0.0f));
            Handles.color = handleColor;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeUpDown);

            boundingRect = trackGUI.ToWindowSpace(rect);

            if (Event.current.type == EventType.Repaint)
            {
                state.spacePartitioner.AddBounds(this);

                var dragStyle = new GUIStyle("RL DragHandle");
                dragStyle.Draw(rect, GUIContent.none, false, false, false, false);
            }
        }
    }

    class InlineCurveEditor : IBounds
    {
        Rect m_TrackRect;
        Rect m_HeaderRect;
        readonly TimelineTrackGUI m_TrackGUI;
        readonly InlineCurveResizeHandle m_ResizeHandle;

        bool m_LastSelectionWasClip;
        TimelineClipGUI m_LastSelectedClipGUI;

        Rect IBounds.boundingRect { get { return m_TrackGUI.ToWindowSpace(m_TrackRect); } }

        [UsedImplicitly] // Used in tests
        public TimelineClipGUI currentClipGui
        {
            get { return m_LastSelectedClipGUI; }
        }

        public IClipCurveEditorOwner currentCurveEditor
        {
            get { return m_LastSelectionWasClip ? (IClipCurveEditorOwner)m_LastSelectedClipGUI : (IClipCurveEditorOwner)m_TrackGUI;  }
        }

        public InlineCurveEditor(TimelineTrackGUI trackGUI)
        {
            m_TrackGUI = trackGUI;
            m_ResizeHandle = new InlineCurveResizeHandle(trackGUI);
        }

        static bool MouseOverTrackArea(Rect curveRect, Rect trackRect)
        {
            curveRect.y = trackRect.y;
            curveRect.height = trackRect.height;

            // clamp the curve editor to the track. this allows the menu to scroll properly
            curveRect.xMin = Mathf.Max(curveRect.xMin, trackRect.xMin);
            curveRect.xMax = trackRect.xMax;

            return curveRect.Contains(Event.current.mousePosition);
        }

        static bool MouseOverHeaderArea(Rect headerRect, Rect trackRect)
        {
            headerRect.y = trackRect.y;
            headerRect.height = trackRect.height;

            return headerRect.Contains(Event.current.mousePosition);
        }

        static void DrawCurveEditor(IClipCurveEditorOwner clipCurveEditorOwner, WindowState state, Rect headerRect, Rect trackRect, Vector2 activeRange, bool locked)
        {
            ClipCurveEditor clipCurveEditor = clipCurveEditorOwner.clipCurveEditor;
            CurveDataSource dataSource = clipCurveEditor.dataSource;
            Rect curveRect = dataSource.GetBackgroundRect(state);

            bool newlySelected = false;

            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.ContextClick)
                newlySelected = MouseOverTrackArea(curveRect, trackRect) || MouseOverHeaderArea(headerRect, trackRect);

            // make sure to not use any event before drawing the curve.
            bool prevEnabledState = GUI.enabled;
            GUI.enabled = true;
            clipCurveEditorOwner.clipCurveEditor.DrawHeader(headerRect);
            GUI.enabled = prevEnabledState;

            bool displayAsSelected = !locked && (clipCurveEditorOwner.inlineCurvesSelected || newlySelected);

            using (new EditorGUI.DisabledScope(locked))
            {
                using (new GUIViewportScope(trackRect))
                {
                    Rect animEditorRect = curveRect;
                    animEditorRect.y = trackRect.y;
                    animEditorRect.height = trackRect.height;

                    // clamp the curve editor to the track. this allows the menu to scroll properly
                    animEditorRect.xMin = Mathf.Max(animEditorRect.xMin, trackRect.xMin);
                    animEditorRect.xMax = trackRect.xMax;

                    if (activeRange == Vector2.zero)
                        activeRange = new Vector2(animEditorRect.xMin, animEditorRect.xMax);

                    clipCurveEditor.DrawCurveEditor(animEditorRect, state, activeRange, clipCurveEditorOwner.showLoops, displayAsSelected);
                }
            }

            if (newlySelected && !locked)
            {
                clipCurveEditorOwner.inlineCurvesSelected = true;
                HandleCurrentEvent();
            }
        }

        public void Draw(Rect headerRect, Rect trackRect, WindowState state)
        {
            m_TrackRect = trackRect;
            m_TrackRect.height -= 5.0f;

            if (Event.current.type == EventType.Repaint)
                state.spacePartitioner.AddBounds(this);

            // Remove the indentation of this track to render it properly, otherwise every GUI elements will be offsetted.
            headerRect.x -= DirectorStyles.kBaseIndent;
            headerRect.width += DirectorStyles.kBaseIndent;

            // Remove the width of the color swatch.
            headerRect.x += 4.0f;
            headerRect.width -= 4.0f;

            m_HeaderRect = headerRect;

            EditorGUI.DrawRect(m_HeaderRect, DirectorStyles.Instance.customSkin.colorAnimEditorBinding);

            if (ShouldShowClipCurves(state))
            {
                DrawCurveEditorsForClipsOnTrack(m_HeaderRect, m_TrackRect, state);
            }
            else if (ShouldShowTrackCurves())
            {
                DrawCurveEditorForTrack(m_HeaderRect, m_TrackRect, state);
            }
            else
            {
                DrawCurvesEditorForNothingSelected(m_HeaderRect, m_TrackRect, state);
            }

            m_ResizeHandle.Draw(headerRect, trackRect, state);

            // If MouseDown or ContextClick are not consumed by the curves, use the event to prevent it from going deeper into the treeview.
            if (Event.current.type == EventType.ContextClick)
            {
                var r = Rect.MinMaxRect(m_HeaderRect.xMin, m_HeaderRect.yMin, m_TrackRect.xMax, m_TrackRect.yMax);
                if (r.Contains(Event.current.mousePosition))
                    Event.current.Use();
            }

            UpdateViewModel();
        }

        public void Refresh()
        {
            if (m_LastSelectionWasClip)
                RefreshInlineCurves(m_LastSelectedClipGUI);
            else
                RefreshInlineCurves(m_TrackGUI);
        }

        static void RefreshInlineCurves(IClipCurveEditorOwner guiItem)
        {
            if (guiItem.clipCurveEditor != null && guiItem.clipCurveEditor.dataSource != null)
                guiItem.clipCurveEditor.dataSource.RebuildCurves();
        }

        void DrawCurveEditorForTrack(Rect headerRect, Rect trackRect, WindowState state)
        {
            if (m_TrackGUI.clipCurveEditor == null)
                return;

            DrawCurveEditor(m_TrackGUI, state, headerRect, trackRect, Vector2.zero, m_TrackGUI.locked);
            m_LastSelectionWasClip = false;
        }

        void DrawCurveEditorsForClipsOnTrack(Rect headerRect, Rect trackRect, WindowState state)
        {
            if (m_TrackGUI.clips.Count == 0)
                return;

            if (Event.current.type == EventType.Layout)
            {
                TimelineClipGUI selectedClip = SelectionManager.SelectedClipGUI().FirstOrDefault(x => x.parent == m_TrackGUI);
                if (selectedClip != null)
                {
                    m_LastSelectedClipGUI = selectedClip;
                }
                else if (state.recording && state.IsArmedForRecord(m_TrackGUI.track))
                {
                    if (m_LastSelectedClipGUI == null || !m_TrackGUI.track.IsRecordingToClip(m_LastSelectedClipGUI.clip))
                    {
                        var clip = m_TrackGUI.clips.FirstOrDefault(x => m_TrackGUI.track.IsRecordingToClip(x.clip));
                        if (clip != null)
                            m_LastSelectedClipGUI = clip;
                    }
                }

                if (m_LastSelectedClipGUI == null)
                    m_LastSelectedClipGUI = m_TrackGUI.clips[0];
            }

            if (m_LastSelectedClipGUI == null || m_LastSelectedClipGUI.clipCurveEditor == null || m_LastSelectedClipGUI.isInvalid)
                return;

            var inlineCurveActiveArea = new Vector2(state.TimeToPixel(m_LastSelectedClipGUI.clip.start), state.TimeToPixel(m_LastSelectedClipGUI.clip.end));
            DrawCurveEditor(m_LastSelectedClipGUI, state, headerRect, trackRect, inlineCurveActiveArea, m_TrackGUI.locked);
            m_LastSelectionWasClip = true;
        }

        void DrawCurvesEditorForNothingSelected(Rect headerRect, Rect trackRect, WindowState state)
        {
            if (m_LastSelectionWasClip || !TrackHasCurvesToShow() && m_TrackGUI.clips.Count > 0)
            {
                DrawCurveEditorsForClipsOnTrack(headerRect, trackRect, state);
            }
            else
            {
                DrawCurveEditorForTrack(headerRect, trackRect, state);
            }
        }

        bool ShouldShowClipCurves(WindowState state)
        {
            if (m_TrackGUI.clips.Count == 0)
                return false;

            // Is a clip selected or being recorded to?
            return SelectionManager.SelectedClipGUI().FirstOrDefault(x => x.parent == m_TrackGUI) != null ||
                state.recording && state.IsArmedForRecord(m_TrackGUI.track) && m_TrackGUI.clips.FirstOrDefault(x => m_TrackGUI.track.IsRecordingToClip(x.clip)) != null;
        }

        bool ShouldShowTrackCurves()
        {
            if (m_TrackGUI == null)
                return false;

            var isTrackSelected = SelectionManager.SelectedTrackGUI().FirstOrDefault(x => x == m_TrackGUI) != null;

            if (!isTrackSelected)
                return false;

            return TrackHasCurvesToShow();
        }

        bool TrackHasCurvesToShow()
        {
            var animTrack = m_TrackGUI.track as AnimationTrack;
            if (animTrack != null && !animTrack.inClipMode)
                return true;

            return m_TrackGUI.track.HasAnyAnimatableParameters();
        }

        void UpdateViewModel()
        {
            var curveEditor = currentCurveEditor.clipCurveEditor;
            if (curveEditor == null || curveEditor.bindingHierarchy.treeViewController == null)
                return;

            var vm = TimelineWindowViewPrefs.GetTrackViewModelData(m_TrackGUI.track);
            vm.inlineCurvesState = curveEditor.bindingHierarchy.treeViewController.state;
            vm.inlineCurvesShownAreaInsideMargins = curveEditor.shownAreaInsideMargins;
            vm.lastInlineCurveDataID = curveEditor.dataSource.id;
        }

        static void HandleCurrentEvent()
        {
#if UNITY_EDITOR_OSX
            Event.current.type = EventType.Ignore;
#else
            Event.current.Use();
#endif
        }
    }
}
