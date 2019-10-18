using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        TimeAreaItem m_PlayHead;

        void TimeCursorGUI(TimelineItemArea area)
        {
            DrawTimeOnSlider();
            if (!CanDrawTimeCursor(area))
                return;

            if (m_PlayHead == null || m_PlayHead.style != styles.timeCursor)
            {
                m_PlayHead = new TimeAreaItem(styles.timeCursor, OnTrackHeadDrag);
                m_PlayHead.AddManipulator(new PlayheadContextMenu(m_PlayHead));
            }

            var headerMode = area == TimelineItemArea.Header;
            DrawTimeCursor(headerMode, !headerMode);
        }

        bool CanDrawTimeCursor(TimelineItemArea area)
        {
            if (!currentMode.ShouldShowTimeCursor(state))
                return false;

            if (treeView == null || state.editSequence.asset == null || (state.editSequence.asset != null && state.IsEditingAnEmptyTimeline()))
                return false;

            if (area == TimelineItemArea.Lines && !state.TimeIsInRange((float)state.editSequence.time))
                return false;

            return true;
        }

        void DrawTimeOnSlider()
        {
            if (currentMode.ShouldShowTimeCursor(state))
            {
                var colorDimFactor = EditorGUIUtility.isProSkin ? 0.7f : 0.9f;
                var c = styles.timeCursor.normal.textColor * colorDimFactor;

                float time = Mathf.Max((float)state.editSequence.time, 0);
                float duration = (float)state.editSequence.duration;

                m_TimeArea.DrawTimeOnSlider(time, c, duration, DirectorStyles.kDurationGuiThickness);
            }
        }

        void DrawTimeCursor(bool drawHead, bool drawline)
        {
            m_PlayHead.HandleManipulatorsEvents(state);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (state.timeAreaRect.Contains(Event.current.mousePosition))
                {
                    state.SetPlaying(false);
                    m_PlayHead.HandleManipulatorsEvents(state);
                    state.editSequence.time = Math.Max(0.0, state.GetSnappedTimeAtMousePosition(Event.current.mousePosition));
                }
            }

            state.isClipSnapping = false;

            m_PlayHead.drawLine = drawline;
            m_PlayHead.drawHead = drawHead;
            m_PlayHead.Draw(sequenceContentRect, state, state.editSequence.time);
        }

        void OnTrackHeadDrag(double newTime)
        {
            state.editSequence.time = Math.Max(0.0, newTime);
            m_PlayHead.showTooltip = true;
        }
    }
}
