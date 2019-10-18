using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
    class EditModeInputHandler
    {
        readonly MoveInputHandler m_MoveHandler;
        readonly TrimInputHandler m_TrimHandler;

        public EditModeInputHandler()
        {
            m_MoveHandler = new MoveInputHandler();
            m_TrimHandler = new TrimInputHandler();
        }

        public void ProcessMove(InputEvent action, double value)
        {
            if (TimelineWindow.instance != null && TimelineWindow.instance.state != null)
                ProcessInputAction(m_MoveHandler, action, value, TimelineWindow.instance.state);
        }

        public void ProcessTrim(InputEvent action, double value, bool stretch)
        {
            if (TimelineWindow.instance != null && TimelineWindow.instance.state != null)
            {
                m_TrimHandler.stretch = stretch;
                ProcessInputAction(m_TrimHandler, action, value, TimelineWindow.instance.state);
            }
        }

        public void SetValueForEdge(IEnumerable<ITimelineItem> items, AttractedEdge edge, double value)
        {
            if (TimelineWindow.instance != null && TimelineWindow.instance.state != null)
                MoveInputHandler.SetValueForEdge(items, edge, value, TimelineWindow.instance.state);
        }

        public void OnGUI(WindowState state, Event evt)
        {
            if (TimelineWindow.instance != null && TimelineWindow.instance.state != null)
            {
                m_MoveHandler.OnGUI(evt);
                m_TrimHandler.OnGUI(state);
            }
        }

        static void ProcessInputAction(IInputHandler handler, InputEvent action, double value, WindowState state)
        {
            var items = SelectionManager.SelectedItems();
            switch (action)
            {
                case InputEvent.None:
                    return;
                case InputEvent.DragEnter:
                    handler.OnEnterDrag(items, state);
                    break;
                case InputEvent.Drag:
                    handler.OnDrag(value, state);
                    break;
                case InputEvent.DragExit:
                    handler.OnExitDrag();
                    break;
                case InputEvent.KeyboardInput:
                    handler.OnSetValue(items, value, state);
                    break;
                default:
                    return;
            }
        }

        interface IInputHandler
        {
            void OnEnterDrag(IEnumerable<ITimelineItem> items, WindowState state);
            void OnDrag(double value, WindowState state);
            void OnExitDrag();
            void OnSetValue(IEnumerable<ITimelineItem> items, double value, WindowState state);
        }

        class TrimInputHandler : IInputHandler
        {
            bool isDragging { get; set; }
            public bool stretch { get; set; }

            IEnumerable<ITimelineItem> grabbedItems { get; set; }

            public void OnEnterDrag(IEnumerable<ITimelineItem> items, WindowState state)
            {
                grabbedItems = items.OfType<ITrimmable>().ToArray();
                foreach (var item in grabbedItems)
                {
                    EditMode.BeginTrim(item, TrimEdge.End);
                }

                isDragging = true;
            }

            public void OnDrag(double endValue, WindowState state)
            {
                var trimValue = endValue;
                trimValue = TimelineWindow.instance.state.SnapToFrameIfRequired(trimValue);

                foreach (var item in grabbedItems)
                {
                    EditMode.TrimEnd(item, trimValue, stretch);
                }
                state.UpdateRootPlayableDuration(state.editSequence.duration);
            }

            public void OnExitDrag()
            {
                isDragging = false;
                EditMode.FinishTrim();
                TimelineWindow.instance.Repaint();
            }

            public void OnSetValue(IEnumerable<ITimelineItem> items, double endValue, WindowState state)
            {
                foreach (var item in items.OfType<ITrimmable>())
                {
                    EditMode.BeginTrim(item, TrimEdge.End);
                    EditMode.TrimEnd(item, endValue, stretch);
                    EditMode.FinishTrim();
                }
                state.UpdateRootPlayableDuration(state.editSequence.duration);
            }

            public void OnGUI(WindowState state)
            {
                if (!isDragging) return;

                foreach (var item in grabbedItems)
                {
                    EditMode.DrawTrimGUI(state, item.gui, TrimEdge.End);
                }
            }
        }

        class MoveInputHandler : IInputHandler
        {
            MoveItemHandler m_MoveItemHandler;

            public void OnEnterDrag(IEnumerable<ITimelineItem> items, WindowState state)
            {
                if (items.Any())
                {
                    m_MoveItemHandler = new MoveItemHandler(state);
                    m_MoveItemHandler.Grab(items, items.First().parentTrack);
                }
            }

            public void OnDrag(double value, WindowState state)
            {
                if (m_MoveItemHandler == null) return;

                var startValue = value;
                startValue = state.SnapToFrameIfRequired(startValue);
                m_MoveItemHandler.OnAttractedEdge(null, ManipulateEdges.Both, AttractedEdge.None, startValue);
            }

            public void OnExitDrag()
            {
                if (m_MoveItemHandler == null) return;

                m_MoveItemHandler.Drop();
                m_MoveItemHandler = null;
                GUIUtility.ExitGUI();
            }

            public void OnSetValue(IEnumerable<ITimelineItem> items, double value, WindowState state)
            {
                if (!items.Any()) return;

                m_MoveItemHandler = new MoveItemHandler(state);
                m_MoveItemHandler.Grab(items, items.First().parentTrack);
                m_MoveItemHandler.OnAttractedEdge(null, ManipulateEdges.Both, AttractedEdge.None, value);
                m_MoveItemHandler.Drop();
                m_MoveItemHandler = null;
            }

            public void OnGUI(Event evt)
            {
                if (m_MoveItemHandler != null)
                    m_MoveItemHandler.OnGUI(evt);
            }

            public static void SetValueForEdge(IEnumerable<ITimelineItem> items, AttractedEdge edge, double value, WindowState state)
            {
                var handler = new MoveItemHandler(state);
                foreach (var item in items)
                {
                    handler.Grab(new[] {item}, item.parentTrack);
                    handler.OnAttractedEdge(null, ManipulateEdges.Both, edge, value);
                    handler.Drop();
                }
            }
        }
    }
}
