using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    enum ManipulateEdges
    {
        Left,
        Right,
        Both
    }

    class SnapEngine
    {
        static readonly float k_MagnetInfluenceInPixels = 10.0f;

        class SnapInfo
        {
            public double time { get; set; }

            public bool showSnapHint { get; set; }

            public bool IsInInfluenceZone(double currentTime, WindowState state)
            {
                var pos = state.TimeToPixel(currentTime);
                var magnetPos = state.TimeToPixel(time);

                return Math.Abs(pos - magnetPos) < k_MagnetInfluenceInPixels;
            }
        }

        struct TimeBoundaries
        {
            public TimeBoundaries(double l, double r)
            {
                left = l;
                right = r;
            }

            public readonly double left;
            public readonly double right;

            public TimeBoundaries Translate(double d)
            {
                return new TimeBoundaries(left + d, right + d);
            }
        }

        public static bool displayDebugLayout;

        readonly IAttractable m_Attractable;
        readonly IAttractionHandler m_AttractionHandler;
        readonly ManipulateEdges m_ManipulateEdges;

        readonly WindowState m_State;

        double m_GrabbedTime;
        TimeBoundaries m_GrabbedTimes;

        TimeBoundaries m_CurrentTimes;

        readonly List<SnapInfo> m_Magnets = new List<SnapInfo>();

        bool m_SnapEnabled;

        public SnapEngine(IAttractable attractable, IAttractionHandler attractionHandler, ManipulateEdges manipulateEdges, WindowState state,
                          Vector2 mousePosition, IEnumerable<ISnappable> snappables = null)
        {
            m_Attractable = attractable;
            m_ManipulateEdges = manipulateEdges;

            m_AttractionHandler = attractionHandler;
            m_State = state;

            m_CurrentTimes = m_GrabbedTimes = new TimeBoundaries(m_Attractable.start, m_Attractable.end);
            m_GrabbedTime = m_State.PixelToTime(mousePosition.x);

            // Add Time zero as Magnet
            AddMagnet(0.0, true, state);

            // Add current Time as Magnet
            // case1157280 only add current time as magnet if visible
            if (TimelineWindow.instance.currentMode.ShouldShowTimeCursor(m_State))
                AddMagnet(state.editSequence.time, true, state);

            if (state.IsEditingASubTimeline())
            {
                // Add start and end of evaluable range as Magnets
                // This includes the case where the master timeline has a fixed length
                var range = state.editSequence.GetEvaluableRange();
                AddMagnet(range.start, true, state);
                AddMagnet(range.end, true, state);
            }
            else if (state.masterSequence.asset.durationMode == TimelineAsset.DurationMode.FixedLength)
            {
                // Add end sequence Time as Magnet
                AddMagnet(state.masterSequence.asset.duration, true, state);
            }


            if (snappables == null)
                snappables = GetVisibleSnappables(m_State);

            foreach (var snappable in snappables)
            {
                if (!attractable.ShouldSnapTo(snappable))
                    continue;

                var edges = snappable.SnappableEdgesFor(attractable, manipulateEdges);
                foreach (var edge in edges)
                    AddMagnet(edge.time, edge.showSnapHint, state);
            }
        }

        public static IEnumerable<ISnappable> GetVisibleSnappables(WindowState state)
        {
            Rect rect = TimelineWindow.instance.state.timeAreaRect;
            rect.height = float.MaxValue;
            return state.spacePartitioner.GetItemsInArea<ISnappable>(rect).ToArray();
        }

        void AddMagnet(double magnetTime, bool showSnapHint, WindowState state)
        {
            var magnet = m_Magnets.FirstOrDefault(m => m.time.Equals(magnetTime));
            if (magnet == null)
            {
                if (IsMagnetInShownArea(magnetTime, state))
                    m_Magnets.Add(new SnapInfo { time = magnetTime, showSnapHint = showSnapHint });
            }
            else
            {
                magnet.showSnapHint |= showSnapHint;
            }
        }

        static bool IsMagnetInShownArea(double time, WindowState state)
        {
            var shownArea = state.timeAreaShownRange;
            return time >= shownArea.x && time <= shownArea.y;
        }

        SnapInfo GetMagnetAt(double time)
        {
            return m_Magnets.FirstOrDefault(m => m.time.Equals(time));
        }

        SnapInfo ClosestMagnet(double time)
        {
            SnapInfo candidate = null;
            var min = double.MaxValue;
            foreach (var magnetInfo in m_Magnets)
            {
                var m = Math.Abs(magnetInfo.time - time);
                if (m < min)
                {
                    candidate = magnetInfo;
                    min = m;
                }
            }

            if (candidate != null && candidate.IsInInfluenceZone(time, m_State))
                return candidate;

            return null;
        }

        public void Snap(Vector2 currentMousePosition, EventModifiers modifiers)
        {
            var d = m_State.PixelToTime(currentMousePosition.x) - m_GrabbedTime;

            m_CurrentTimes = m_GrabbedTimes.Translate(d);

            bool isLeft = m_ManipulateEdges == ManipulateEdges.Left || m_ManipulateEdges == ManipulateEdges.Both;
            bool isRight = m_ManipulateEdges == ManipulateEdges.Right || m_ManipulateEdges == ManipulateEdges.Both;

            bool attracted = false;

            m_SnapEnabled = modifiers == ManipulatorsUtils.actionModifier ? !m_State.edgeSnaps : m_State.edgeSnaps;

            if (m_SnapEnabled)
            {
                SnapInfo leftActiveMagnet = null;
                SnapInfo rightActiveMagnet = null;

                if (isLeft)
                    leftActiveMagnet = ClosestMagnet(m_CurrentTimes.left);

                if (isRight)
                    rightActiveMagnet = ClosestMagnet(m_CurrentTimes.right);

                if (leftActiveMagnet != null || rightActiveMagnet != null)
                {
                    attracted = true;

                    bool leftAttraction = false;

                    if (rightActiveMagnet == null)
                    {
                        // Attracted by a left magnet only.
                        leftAttraction = true;
                    }
                    else
                    {
                        if (leftActiveMagnet != null)
                        {
                            // Attracted by both magnets, choose the closest one.
                            var leftDistance = Math.Abs(leftActiveMagnet.time - m_CurrentTimes.left);
                            var rightDistance = Math.Abs(rightActiveMagnet.time - m_CurrentTimes.right);

                            leftAttraction = leftDistance <= rightDistance;
                        }
                        // else, Attracted by right magnet only
                    }

                    if (leftAttraction)
                    {
                        m_AttractionHandler.OnAttractedEdge(m_Attractable, m_ManipulateEdges, AttractedEdge.Left, leftActiveMagnet.time);
                    }
                    else
                    {
                        m_AttractionHandler.OnAttractedEdge(m_Attractable, m_ManipulateEdges, AttractedEdge.Right, rightActiveMagnet.time);
                    }
                }
            }

            if (!attracted)
            {
                var time = isLeft ? m_CurrentTimes.left : m_CurrentTimes.right;

                time = m_State.SnapToFrameIfRequired(time);

                m_AttractionHandler.OnAttractedEdge(m_Attractable, m_ManipulateEdges, AttractedEdge.None, time);
            }
        }

        public void OnGUI(bool showLeft = true, bool showRight = true)
        {
            if (displayDebugLayout)
            {
                // Display Magnet influence zone
                foreach (var m in m_Magnets)
                {
                    var window = TimelineWindow.instance;
                    var rect = new Rect(m_State.TimeToPixel(m.time) - k_MagnetInfluenceInPixels, window.state.timeAreaRect.yMax, 2f * k_MagnetInfluenceInPixels, m_State.windowHeight);
                    EditorGUI.DrawRect(rect, new Color(1f, 0f, 0f, 0.4f));
                }

                // Display Cursor position
                var mousePos = Event.current.mousePosition;
                var time = m_State.PixelToTime(mousePos.x);
                var p = new Vector2(m_State.TimeToPixel(time), TimelineWindow.instance.state.timeAreaRect.yMax);
                var s = new Vector2(1f, m_State.windowHeight);
                EditorGUI.DrawRect(new Rect(p, s), Color.blue);

                p = new Vector2(m_State.TimeToPixel(m_GrabbedTime), TimelineWindow.instance.state.timeAreaRect.yMax);
                s = new Vector2(1f, m_State.windowHeight);
                EditorGUI.DrawRect(new Rect(p, s), Color.red);

                p = new Vector2(m_State.TimeToPixel(m_CurrentTimes.left), TimelineWindow.instance.state.timeAreaRect.yMax);
                s = new Vector2(1f, m_State.windowHeight);
                EditorGUI.DrawRect(new Rect(p, s), Color.yellow);

                p = new Vector2(m_State.TimeToPixel(m_CurrentTimes.right), TimelineWindow.instance.state.timeAreaRect.yMax);
                EditorGUI.DrawRect(new Rect(p, s), Color.yellow);
            }

            if (m_SnapEnabled)
            {
                if (showLeft)
                    DrawMagnetLineAt(m_Attractable.start);

                if (showRight)
                    DrawMagnetLineAt(m_Attractable.end);
            }
        }

        void DrawMagnetLineAt(double time)
        {
            var magnet = GetMagnetAt(time);

            if (magnet != null && magnet.showSnapHint)
                Graphics.DrawLineAtTime(m_State, magnet.time, Color.white);
        }
    }
}
