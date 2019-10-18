using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class InlineCurveResize : Manipulator
    {
        bool m_Captured;

        float m_CapturedHeight;
        float m_CaptureMouseYPos;

        InlineCurveResizeHandle m_Target;

        protected override bool MouseDown(Event evt, WindowState state)
        {
            m_Target = PickerUtils.PickedInlineCurveResizer();
            if (m_Target == null)
                return false;

            m_Captured = true;
            m_CapturedHeight = TimelineWindowViewPrefs.GetInlineCurveHeight(m_Target.trackGUI.track);
            m_CaptureMouseYPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y;
            state.AddCaptured(this);

            return true;
        }

        protected override bool MouseDrag(Event evt, WindowState state)
        {
            if (!m_Captured || m_Target == null)
                return false;

            var trackGUI = m_Target.trackGUI;

            float inlineTrackHeight = m_CapturedHeight +
                (GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y - m_CaptureMouseYPos);

            TimelineWindowViewPrefs.SetInlineCurveHeight(trackGUI.track, Mathf.Max(inlineTrackHeight, 60.0f));

            state.GetWindow().treeView.CalculateRowRects();

            return true;
        }

        protected override bool MouseUp(Event evt, WindowState state)
        {
            if (!m_Captured)
                return false;

            state.RemoveCaptured(this);
            m_Captured = false;

            return true;
        }
    }

    class TrackDoubleClick : Manipulator
    {
        protected override bool DoubleClick(Event evt, WindowState state)
        {
            if (evt.button != 0)
                return false;

            var trackGUI = PickerUtils.PickedTrackBaseGUI();

            if (trackGUI == null)
                return false;

            // Double-click is only available for AnimationTracks: it conflicts with selection mechanics on other tracks
            if ((trackGUI.track as AnimationTrack) == null)
                return false;

            return EditTrackInAnimationWindow.Do(state, trackGUI.track);
        }
    }

    class TrackShortcutManipulator : Manipulator
    {
        protected override bool ExecuteCommand(Event evt, WindowState state)
        {
            if (state.IsCurrentEditingASequencerTextField())
                return false;

            var tracks = SelectionManager.SelectedTracks().ToList();

            var itemGUIs = SelectionManager.SelectedClipGUI();

            foreach (var itemGUI in itemGUIs)
            {
                var trackGUI = itemGUI.parent == null ? null : itemGUI.parent as TimelineTrackBaseGUI;
                if (trackGUI == null)
                    continue;

                if (!tracks.Contains(trackGUI.track))
                    tracks.Add(trackGUI.track);
            }

            return TrackAction.HandleShortcut(state, evt, tracks.ToArray());
        }
    }
}
