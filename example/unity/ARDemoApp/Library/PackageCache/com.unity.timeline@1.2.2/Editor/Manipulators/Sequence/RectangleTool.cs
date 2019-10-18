using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    abstract class RectangleTool
    {
        struct TimelinePoint
        {
            readonly double m_Time;
            readonly float m_YPos;
            readonly float m_YScrollPos;

            readonly WindowState m_State;
            readonly TimelineTreeViewGUI m_TreeViewGUI;

            public TimelinePoint(WindowState state, Vector2 mousePosition)
            {
                m_State = state;
                m_TreeViewGUI = state.GetWindow().treeView;

                m_Time = m_State.PixelToTime(mousePosition.x);
                m_YPos = mousePosition.y;
                m_YScrollPos = m_TreeViewGUI.scrollPosition.y;
            }

            public Vector2 ToPixel()
            {
                return new Vector2(m_State.TimeToPixel(m_Time), m_YPos - (m_TreeViewGUI.scrollPosition.y - m_YScrollPos));
            }
        }

        TimeAreaAutoPanner m_TimeAreaAutoPanner;

        TimelinePoint m_StartPoint;
        Vector2 m_EndPixel = Vector2.zero;

        Rect m_ActiveRect;

        protected abstract bool enableAutoPan { get; }
        protected abstract bool CanStartRectangle(Event evt, Vector2 mousePosition, WindowState state);
        protected abstract bool OnFinish(Event evt, WindowState state, Rect rect);

        int m_Id;

        public void OnGUI(WindowState state, EventType rawType, Vector2 mousePosition)
        {
            if (m_Id == 0)
                m_Id = GUIUtility.GetPermanentControlID();

            if (state == null || state.GetWindow().treeView == null)
                return;

            var evt = Event.current;

            if (rawType == EventType.MouseDown || evt.type == EventType.MouseDown)
            {
                if (state.IsCurrentEditingASequencerTextField())
                    return;

                m_ActiveRect = TimelineWindow.instance.sequenceContentRect;

                if (!m_ActiveRect.Contains(mousePosition))
                    return;

                if (!CanStartRectangle(evt, mousePosition, state))
                    return;

                if (enableAutoPan)
                    m_TimeAreaAutoPanner = new TimeAreaAutoPanner(state);

                m_StartPoint = new TimelinePoint(state, mousePosition);
                m_EndPixel = mousePosition;

                GUIUtility.hotControl = m_Id; //HACK: Because the treeView eats all the events, steal the hotControl if necessary...
                evt.Use();

                return;
            }

            switch (evt.GetTypeForControl(m_Id))
            {
                case EventType.KeyDown:
                {
                    if (GUIUtility.hotControl == m_Id)
                    {
                        if (evt.keyCode == KeyCode.Escape)
                        {
                            m_TimeAreaAutoPanner = null;

                            GUIUtility.hotControl = 0;
                            evt.Use();
                        }
                    }

                    return;
                }

                case EventType.MouseDrag:
                {
                    if (GUIUtility.hotControl != m_Id)
                        return;

                    m_EndPixel = mousePosition;
                    evt.Use();

                    return;
                }

                case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl != m_Id)
                        return;

                    m_TimeAreaAutoPanner = null;

                    var rect = CurrentRectangle();

                    if (IsValidRect(rect))
                        OnFinish(evt, state, rect);

                    GUIUtility.hotControl = 0;
                    evt.Use();

                    return;
                }
            }

            if (GUIUtility.hotControl == m_Id)
            {
                if (evt.type == EventType.Repaint)
                {
                    var r = CurrentRectangle();

                    if (IsValidRect(r))
                    {
                        using (new GUIViewportScope(m_ActiveRect))
                        {
                            DrawRectangle(r);
                        }
                    }
                }

                if (m_TimeAreaAutoPanner != null)
                    m_TimeAreaAutoPanner.OnGUI(evt);
            }
        }

        protected virtual void DrawRectangle(Rect rect)
        {
            EditorStyles.selectionRect.Draw(rect, GUIContent.none, false, false, false, false);
        }

        static bool IsValidRect(Rect rect)
        {
            return rect.width >= 1.0f && rect.height >= 1.0f;
        }

        Rect CurrentRectangle()
        {
            var startPixel = m_StartPoint.ToPixel();
            return Rect.MinMaxRect(
                Math.Min(startPixel.x, m_EndPixel.x),
                Math.Min(startPixel.y, m_EndPixel.y),
                Math.Max(startPixel.x, m_EndPixel.x),
                Math.Max(startPixel.y, m_EndPixel.y));
        }
    }
}
