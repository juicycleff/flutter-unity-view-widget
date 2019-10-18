using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using Graphics = UnityEditor.Timeline.Graphics;

namespace UnityEditor
{
    class ClipCurveEditor
    {
        internal readonly CurveEditor m_CurveEditor;
        static readonly CurveEditorSettings s_CurveEditorSettings = new CurveEditorSettings();

        static readonly float s_GridLabelWidth = 40.0f;

        readonly BindingSelector m_BindingHierarchy;
        public BindingSelector bindingHierarchy
        {
            get { return m_BindingHierarchy; }
        }

        public Rect shownAreaInsideMargins
        {
            get { return m_CurveEditor != null ? m_CurveEditor.shownAreaInsideMargins : new Rect(1, 1, 1, 1); }
        }

        Vector2 m_ScrollPosition = Vector2.zero;

        readonly CurveDataSource m_DataSource;

        float m_LastFrameRate = 30.0f;
        int m_LastClipVersion = -1;
        int m_LastCurveCount = -1;
        TrackViewModelData m_ViewModel;

        bool isNewSelection
        {
            get
            {
                if (m_ViewModel == null || m_DataSource == null)
                    return true;

                return m_ViewModel.lastInlineCurveDataID != m_DataSource.id;
            }
        }

        internal CurveEditor curveEditor
        {
            get { return m_CurveEditor; }
        }

        public ClipCurveEditor(CurveDataSource dataSource, TimelineWindow parentWindow, TrackAsset hostTrack)
        {
            m_DataSource = dataSource;

            m_CurveEditor = new CurveEditor(new Rect(0, 0, 1000, 100), new CurveWrapper[0], false);

            s_CurveEditorSettings.hSlider = false;
            s_CurveEditorSettings.vSlider = false;
            s_CurveEditorSettings.hRangeLocked = false;
            s_CurveEditorSettings.vRangeLocked = false;
            s_CurveEditorSettings.scaleWithWindow = true;
            s_CurveEditorSettings.hRangeMin = 0.0f;
            s_CurveEditorSettings.showAxisLabels = true;
            s_CurveEditorSettings.allowDeleteLastKeyInCurve = true;
            s_CurveEditorSettings.rectangleToolFlags = CurveEditorSettings.RectangleToolFlags.MiniRectangleTool;

            s_CurveEditorSettings.vTickStyle = new TickStyle
            {
                tickColor = { color = DirectorStyles.Instance.customSkin.colorInlineCurveVerticalLines },
                distLabel = 20,
                stubs = true
            };

            s_CurveEditorSettings.hTickStyle = new TickStyle
            {
                // hide horizontal lines by giving them a transparent color
                tickColor = { color = new Color(0.0f, 0.0f, 0.0f, 0.0f) },
                distLabel = 0
            };

            m_CurveEditor.settings = s_CurveEditorSettings;

            m_ViewModel = TimelineWindowViewPrefs.GetTrackViewModelData(hostTrack);

            if (isNewSelection)
                m_CurveEditor.shownArea = new Rect(1, 1, 1, 1);
            else
                m_CurveEditor.shownAreaInsideMargins = m_ViewModel.inlineCurvesShownAreaInsideMargins;

            m_CurveEditor.ignoreScrollWheelUntilClicked = true;
            m_CurveEditor.curvesUpdated = OnCurvesUpdated;

            m_BindingHierarchy = new BindingSelector(parentWindow, m_CurveEditor, m_ViewModel.inlineCurvesState);
        }

        public void SelectAllKeys()
        {
            m_CurveEditor.SelectAll();
        }

        public void FrameClip()
        {
            m_CurveEditor.InvalidateBounds();
            m_CurveEditor.FrameClip(false, true);
        }

        public bool HasSelection()
        {
            return m_CurveEditor.hasSelection;
        }

        public Vector2 GetSelectionRange()
        {
            Bounds b = m_CurveEditor.selectionBounds;
            return new Vector2(b.min.x, b.max.x);
        }

        public CurveDataSource dataSource
        {
            get { return m_DataSource; }
        }

        internal void OnCurvesUpdated()
        {
            if (m_DataSource == null)
                return;

            if (m_CurveEditor == null)
                return;

            if (m_CurveEditor.animationCurves.Length == 0)
                return;

            List<CurveWrapper> curvesToUpdate = m_CurveEditor.animationCurves.Where(c => c.changed).ToList();

            // nothing changed, return.
            if (curvesToUpdate.Count == 0)
                return;

            AnimationClip clip = m_DataSource.animationClip;

            // something changed, manage the undo properly.
            Undo.RegisterCompleteObjectUndo(clip, "Edit Clip Curve");

            foreach (CurveWrapper c in curvesToUpdate)
            {
                AnimationUtility.SetEditorCurve(clip, c.binding, c.curve);
                c.changed = false;
            }

            m_DataSource.UpdateCurves(curvesToUpdate);
        }

        public void DrawHeader(Rect headerRect)
        {
            m_BindingHierarchy.InitIfNeeded(headerRect, m_DataSource, isNewSelection);

            try
            {
                GUILayout.BeginArea(headerRect);
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
                m_BindingHierarchy.OnGUI(new Rect(0, 0, headerRect.width, headerRect.height));
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        class FrameFormatCurveEditorState : ICurveEditorState
        {
            public TimeArea.TimeFormat timeFormat
            {
                get { return TimeArea.TimeFormat.Frame; }
            }
            public Vector2 timeRange { get { return new Vector2(0, 1); } }
            public bool rippleTime { get { return false; } }
        }

        class UnformattedCurveEditorState : ICurveEditorState
        {
            public TimeArea.TimeFormat timeFormat
            {
                get { return TimeArea.TimeFormat.None; }
            }
            public Vector2 timeRange { get { return new Vector2(0, 1); } }
            public bool rippleTime { get { return false; } }
        }

        void UpdateCurveEditorIfNeeded(WindowState state)
        {
            if ((Event.current.type != EventType.Layout) || (m_DataSource == null) || (m_BindingHierarchy == null) || (m_DataSource.animationClip == null))
                return;

            AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(m_DataSource.animationClip);
            int version = curveInfo.version;
            if (version != m_LastClipVersion)
            {
                // tree has changed
                if (m_LastCurveCount != curveInfo.curves.Length)
                {
                    m_BindingHierarchy.RefreshTree();
                    m_LastCurveCount = curveInfo.curves.Length;
                }
                else
                {
                    // update just the curves
                    m_BindingHierarchy.RefreshCurves();
                }

                if (isNewSelection)
                    FrameClip();

                m_LastClipVersion = version;
            }

            if (state.timeInFrames)
                m_CurveEditor.state = new FrameFormatCurveEditorState();
            else
                m_CurveEditor.state = new UnformattedCurveEditorState();

            m_CurveEditor.invSnap = state.referenceSequence.frameRate;
        }

        public void DrawCurveEditor(Rect animEditorRect, WindowState state, Vector2 activeRange, bool loop, bool selected)
        {
            var curveStart = state.TimeToPixel(m_DataSource.start);
            var minCurveStart = animEditorRect.xMax - 1.0f;

            if (curveStart > minCurveStart) // Prevent the curve from drawing inside small rect
                animEditorRect.xMax += curveStart - minCurveStart;

            UpdateCurveEditorIfNeeded(state);

            DrawCurveEditorBackground(animEditorRect);

            float localCurveStart = curveStart - animEditorRect.xMin;

            // adjust the top margin so smaller rectangle have smaller top / bottom margins.
            m_CurveEditor.topmargin = m_CurveEditor.bottommargin = CalculateTopMargin(animEditorRect.height);
            // calculate the margin needed to align the curve with the clip.
            m_CurveEditor.rightmargin = 0.0f;
            m_CurveEditor.leftmargin = localCurveStart;

            m_CurveEditor.rect = new Rect(0.0f, 0.0f, animEditorRect.width, animEditorRect.height);

            m_CurveEditor.SetShownHRangeInsideMargins(0.0f, (state.PixelToTime(animEditorRect.xMax) - m_DataSource.start) * m_DataSource.timeScale);

            if (m_LastFrameRate != state.referenceSequence.frameRate)
            {
                m_CurveEditor.hTicks.SetTickModulosForFrameRate(state.referenceSequence.frameRate);
                m_LastFrameRate = state.referenceSequence.frameRate;
            }

            foreach (CurveWrapper cw in m_CurveEditor.animationCurves)
                cw.renderer.SetWrap(WrapMode.Default, loop ? WrapMode.Loop : WrapMode.Default);

            m_CurveEditor.BeginViewGUI();

            Color oldColor = GUI.color;
            GUI.color = Color.white;

            GUI.BeginGroup(animEditorRect);

            // Draw a line at 0
            Graphics.DrawLine(new Vector2(localCurveStart, 0.0f), new Vector2(localCurveStart, animEditorRect.height), new Color(1.0f, 1.0f, 1.0f, 0.5f));

            float rangeStart = activeRange.x - animEditorRect.x;
            float rangeWidth = activeRange.y - activeRange.x;

            // draw selection outline underneath the curves.
            if (selected)
            {
                var selectionRect = new Rect(rangeStart, 0.0f, rangeWidth, animEditorRect.height);
                DrawOutline(selectionRect);
            }

            EditorGUI.BeginChangeCheck();

            Event evt = Event.current;
            if ((evt.type == EventType.Layout) || (evt.type == EventType.Repaint) || selected)
                m_CurveEditor.CurveGUI();

            m_CurveEditor.EndViewGUI();

            if (EditorGUI.EndChangeCheck())
                OnCurvesUpdated();

            // draw overlays on top of curves
            var overlayColor = DirectorStyles.Instance.customSkin.colorInlineCurveOutOfRangeOverlay;

            var leftSide = new Rect(localCurveStart, 0.0f, rangeStart - localCurveStart, animEditorRect.height);
            EditorGUI.DrawRect(leftSide, overlayColor);

            var rightSide = new Rect(rangeStart + rangeWidth, 0.0f, animEditorRect.width - rangeStart - rangeWidth, animEditorRect.height);
            EditorGUI.DrawRect(rightSide, overlayColor);

            GUI.color = oldColor;

            GUI.EndGroup();

            // draw the grid labels last
            Rect gridRect = animEditorRect;
            gridRect.width = s_GridLabelWidth;
            float offset = localCurveStart - s_GridLabelWidth;
            if (offset > 0.0f)
                gridRect.x = animEditorRect.x + offset;

            GUI.BeginGroup(gridRect);

            m_CurveEditor.GridGUI();

            GUI.EndGroup();
        }

        static void DrawCurveEditorBackground(Rect animEditorRect)
        {
            if (EditorGUIUtility.isProSkin)
                return;

            var animEditorBackgroundRect = Rect.MinMaxRect(
                0.0f, animEditorRect.yMin, animEditorRect.xMax, animEditorRect.yMax);

            // Curves are not legible in Personal Skin so we need to darken the background a bit.
            EditorGUI.DrawRect(animEditorBackgroundRect, DirectorStyles.Instance.customSkin.colorInlineCurvesBackground);
        }

        float CalculateTopMargin(float height)
        {
            return Mathf.Clamp(0.15f * height, 10.0f, 40.0f);
        }

        // todo move this in a utility class?
        static void DrawOutline(Rect rect, float thickness = 2.0f)
        {
            // Draw top selected lines.
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), Color.white);

            // Draw bottom selected lines.
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), Color.white);

            // Draw Left Selected Lines
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), Color.white);

            // Draw Right Selected Lines
            EditorGUI.DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), Color.white);
        }
    }
}
