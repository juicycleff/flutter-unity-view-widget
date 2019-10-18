using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class Gaps
    {
        public static void Insert(TimelineAsset asset, double at, double amount, float tolerance)
        {
            // gather all clips
            var clips = asset.flattenedTracks.SelectMany(x => x.clips).Where(x => (x.start - at) >= -tolerance);

            foreach (var t in clips.Select(x => x.parentTrack).Distinct())
            {
                TimelineUndo.PushUndo(t, "Insert Time");
            }

            foreach (var clip in clips)
            {
                clip.start += amount;
            }
        }
    }

    class PlayheadContextMenu : Manipulator
    {
        readonly TimeAreaItem m_TimeAreaItem;

        public PlayheadContextMenu(TimeAreaItem timeAreaItem)
        {
            m_TimeAreaItem = timeAreaItem;
        }

        protected override bool ContextClick(Event evt, WindowState state)
        {
            if (!m_TimeAreaItem.bounds.Contains(evt.mousePosition))
                return false;

            var tolerance = 0.25f / state.referenceSequence.frameRate;
            var menu = new GenericMenu();

            if (!TimelineWindow.instance.state.editSequence.isReadOnly)
            {
                menu.AddItem(EditorGUIUtility.TrTextContent("Insert/Frame/Single"), false, () =>
                {
                    Gaps.Insert(state.editSequence.asset, state.editSequence.time, 1.0f / state.referenceSequence.frameRate, tolerance);
                    state.Refresh();
                }
                );

                int[] values = {5, 10, 25, 100};

                for (var i = 0; i != values.Length; ++i)
                {
                    float f = values[i];
                    menu.AddItem(EditorGUIUtility.TrTextContent("Insert/Frame/" + values[i] + " Frames"), false, () =>
                    {
                        Gaps.Insert(state.editSequence.asset, state.editSequence.time, f / state.referenceSequence.frameRate, tolerance);
                        state.Refresh();
                    }
                    );
                }

                var playRangeTime = state.playRange;
                if (playRangeTime.y > playRangeTime.x)
                {
                    menu.AddItem(EditorGUIUtility.TrTextContent("Insert/Selected Time"), false, () =>
                    {
                        Gaps.Insert(state.editSequence.asset, playRangeTime.x, playRangeTime.y - playRangeTime.x, tolerance);
                        state.Refresh();
                    }
                    );
                }
            }

            menu.AddItem(EditorGUIUtility.TrTextContent("Select/Clips Ending Before"), false, () => SelectMenuCallback(x => x.end < state.editSequence.time + tolerance, state));
            menu.AddItem(EditorGUIUtility.TrTextContent("Select/Clips Starting Before"), false, () => SelectMenuCallback(x => x.start < state.editSequence.time + tolerance, state));
            menu.AddItem(EditorGUIUtility.TrTextContent("Select/Clips Ending After"), false, () => SelectMenuCallback(x => x.end - state.editSequence.time >= -tolerance, state));
            menu.AddItem(EditorGUIUtility.TrTextContent("Select/Clips Starting After"), false, () => SelectMenuCallback(x => x.start - state.editSequence.time >= -tolerance, state));
            menu.AddItem(EditorGUIUtility.TrTextContent("Select/Clips Intersecting"), false, () => SelectMenuCallback(x => x.start <= state.editSequence.time && state.editSequence.time <= x.end, state));
            menu.AddItem(EditorGUIUtility.TrTextContent("Select/Blends Intersecting"), false, () => SelectMenuCallback(x => SelectBlendingIntersecting(x, state.editSequence.time), state));
            menu.ShowAsContext();
            return true;
        }

        static bool SelectBlendingIntersecting(TimelineClip clip, double time)
        {
            return clip.start <= time && time <= clip.end && (
                (time <= clip.start + clip.blendInDuration) ||
                (time >= clip.end - clip.blendOutDuration)
            );
        }

        static void SelectMenuCallback(Func<TimelineClip, bool> selector, WindowState state)
        {
            var allClips = state.GetWindow().treeView.allClipGuis;
            if (allClips == null)
                return;

            SelectionManager.Clear();
            for (var i = 0; i != allClips.Count; ++i)
            {
                var c = allClips[i];

                if (c != null && c.clip != null && selector(c.clip))
                {
                    SelectionManager.Add(c.clip);
                }
            }
        }
    }

    class TimeAreaContextMenu : Manipulator
    {
        protected override bool ContextClick(Event evt, WindowState state)
        {
            if (state.timeAreaRect.Contains(Event.current.mousePosition))
            {
                var menu = new GenericMenu();
                AddTimeAreaMenuItems(menu, state);
                menu.ShowAsContext();
                return true;
            }
            return false;
        }

        internal static void AddTimeAreaMenuItems(GenericMenu menu, WindowState state)
        {
            foreach (var value in Enum.GetValues(typeof(TimelineAsset.DurationMode)))
            {
                var mode = (TimelineAsset.DurationMode)value;
                var item = EditorGUIUtility.TextContent(string.Format(TimelineWindow.Styles.DurationModeText, L10n.Tr(ObjectNames.NicifyVariableName(mode.ToString()))));

                if (state.recording || state.IsEditingASubTimeline() || state.editSequence.asset == null
                    || state.editSequence.isReadOnly)
                    menu.AddDisabledItem(item);
                else
                    menu.AddItem(item, state.editSequence.asset.durationMode == mode, () => SelectDurationCallback(state, mode));

                menu.AddItem(DirectorStyles.showMarkersOnTimeline, state.showMarkerHeader, () => new ToggleShowMarkersOnTimeline().Execute(state));
            }
        }

        static void SelectDurationCallback(WindowState state, TimelineAsset.DurationMode mode)
        {
            if (mode == state.editSequence.asset.durationMode)
                return;

            TimelineUndo.PushUndo(state.editSequence.asset, "Duration Mode");


            // if we switched from Auto to Fixed, use the auto duration as the new fixed duration so the end marker stay in the same position.
            if (state.editSequence.asset.durationMode == TimelineAsset.DurationMode.BasedOnClips && mode == TimelineAsset.DurationMode.FixedLength)
            {
                state.editSequence.asset.fixedDuration = state.editSequence.duration;
            }

            state.editSequence.asset.durationMode = mode;
            state.UpdateRootPlayableDuration(state.editSequence.duration);
        }
    }

    class Scrub : Manipulator
    {
        readonly Func<Event, WindowState, bool> m_OnMouseDown;
        readonly Action<double> m_OnMouseDrag;
        readonly Action m_OnMouseUp;

        bool m_IsCaptured;

        public Scrub(Func<Event, WindowState, bool> onMouseDown, Action<double> onMouseDrag, Action onMouseUp)
        {
            m_OnMouseDown = onMouseDown;
            m_OnMouseDrag = onMouseDrag;
            m_OnMouseUp = onMouseUp;
        }

        protected override bool MouseDown(Event evt, WindowState state)
        {
            if (evt.button != 0)
                return false;

            if (!m_OnMouseDown(evt, state))
                return false;

            state.AddCaptured(this);
            m_IsCaptured = true;

            return true;
        }

        protected override bool MouseUp(Event evt, WindowState state)
        {
            if (!m_IsCaptured)
                return false;

            m_IsCaptured = false;
            state.RemoveCaptured(this);

            m_OnMouseUp();

            return true;
        }

        protected override bool MouseDrag(Event evt, WindowState state)
        {
            if (!m_IsCaptured)
                return false;

            m_OnMouseDrag(state.GetSnappedTimeAtMousePosition(evt.mousePosition));

            return true;
        }
    }

    class TimeAreaItem : Control
    {
        public Color        headColor   { get; set; }
        public Color        lineColor   { get; set; }
        public bool         drawLine    { get; set; }
        public bool         drawHead    { get; set; }
        public bool         canMoveHead { get; set; }
        public string       tooltip     { get; set; }
        public Vector2      boundOffset { get; set; }

        readonly GUIContent m_HeaderContent = new GUIContent();
        readonly GUIStyle m_Style;
        readonly Tooltip m_Tooltip;

        Rect m_BoundingRect;

        float widgetHeight { get { return m_Style.fixedHeight; } }
        float widgetWidth { get { return m_Style.fixedWidth; } }

        public Rect bounds
        {
            get
            {
                Rect r = m_BoundingRect;
                r.y = TimelineWindow.instance.state.timeAreaRect.yMax - widgetHeight;
                r.position += boundOffset;

                return r;
            }
        }

        public GUIStyle style
        {
            get { return m_Style;  }
        }


        public bool showTooltip { get; set; }

        // is this the first frame the drag callback is being invoked
        public bool firstDrag { get; private set; }

        public TimeAreaItem(GUIStyle style, Action<double> onDrag)
        {
            m_Style = style;
            headColor = Color.white;
            var scrub = new Scrub(
                (evt, state) =>
                {
                    firstDrag = true;
                    return state.timeAreaRect.Contains(evt.mousePosition) && bounds.Contains(evt.mousePosition);
                },
                (d) =>
                {
                    if (onDrag != null)
                        onDrag(d);
                    firstDrag = false;
                },
                () =>
                {
                    showTooltip = false;
                    firstDrag = false;
                }
            );
            AddManipulator(scrub);
            lineColor = m_Style.normal.textColor;
            drawLine = true;
            drawHead = true;
            canMoveHead = false;
            tooltip = string.Empty;
            boundOffset = Vector2.zero;
            m_Tooltip = new Tooltip(DirectorStyles.Instance.displayBackground, DirectorStyles.Instance.tinyFont);
        }

        public void Draw(Rect rect, WindowState state, double time)
        {
            var clipRect = new Rect(0.0f, 0.0f, TimelineWindow.instance.position.width, TimelineWindow.instance.position.height);
            clipRect.xMin += state.sequencerHeaderWidth;

            using (new GUIViewportScope(clipRect))
            {
                Vector2 windowCoordinate = rect.min;
                windowCoordinate.y += 4.0f;

                windowCoordinate.x = state.TimeToPixel(time);

                m_BoundingRect = new Rect((windowCoordinate.x - widgetWidth / 2.0f), windowCoordinate.y, widgetWidth, widgetHeight);

                // Do not paint if the time cursor goes outside the timeline bounds...
                if (Event.current.type == EventType.Repaint)
                {
                    if (m_BoundingRect.xMax < state.timeAreaRect.xMin)
                        return;
                    if (m_BoundingRect.xMin > state.timeAreaRect.xMax)
                        return;
                }

                var top = new Vector3(windowCoordinate.x, rect.y - DirectorStyles.kDurationGuiThickness);
                var bottom = new Vector3(windowCoordinate.x, rect.yMax);

                if (drawLine)
                {
                    Rect lineRect = Rect.MinMaxRect(top.x - 0.5f, top.y, bottom.x + 0.5f, bottom.y);
                    EditorGUI.DrawRect(lineRect, lineColor);
                }

                if (drawHead)
                {
                    Color c = GUI.color;
                    GUI.color = headColor;
                    GUI.Box(bounds, m_HeaderContent, m_Style);
                    GUI.color = c;

                    if (canMoveHead)
                        EditorGUIUtility.AddCursorRect(bounds, MouseCursor.MoveArrow);
                }

                if (showTooltip)
                {
                    m_Tooltip.text = TimeReferenceUtility.ToTimeString(time);

                    Vector2 position = bounds.position;
                    position.y = state.timeAreaRect.y;
                    position.y -= m_Tooltip.bounds.height;
                    position.x -= Mathf.Abs(m_Tooltip.bounds.width - bounds.width) / 2.0f;

                    Rect tooltipBounds = bounds;
                    tooltipBounds.position = position;
                    m_Tooltip.bounds = tooltipBounds;

                    m_Tooltip.Draw();
                }
            }
        }
    }
}
