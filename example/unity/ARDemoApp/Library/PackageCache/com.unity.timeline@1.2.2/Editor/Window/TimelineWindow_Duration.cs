using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        TimeAreaItem m_TimelineDuration;

        void DurationGUI(TimelineItemArea area, double duration)
        {
            // don't show the duration if the time area is not visible for some other reason.
            if (!currentMode.ShouldShowTimeArea(state))
                return;

            bool headerMode = area == TimelineItemArea.Header;

            if (state.IsEditingASubTimeline())
            {
                if (headerMode)
                    HighlightTimeAreaRange(state.editSequence.GetEvaluableRange(), DirectorStyles.Instance.customSkin.colorSubSequenceDurationLine);

                return;
            }

            // don't show the duration if there's none.
            if (state.editSequence.asset.durationMode == TimelineAsset.DurationMode.BasedOnClips && duration <= 0.0f)
                return;

            if (m_TimelineDuration == null || m_TimelineDuration.style != styles.endmarker)
            {
                m_TimelineDuration = new TimeAreaItem(styles.endmarker, OnTrackDurationDrag)
                {
                    tooltip = "End of sequence marker",
                    boundOffset = new Vector2(0.0f, -DirectorStyles.kDurationGuiThickness)
                };
            }

            DrawDuration(headerMode, !headerMode, duration);
        }

        void DrawDuration(bool drawhead, bool drawline, double duration)
        {
            if (state.TimeIsInRange((float)duration))
            {
                // Set the colors based on the mode
                Color lineColor = DirectorStyles.Instance.customSkin.colorEndmarker;
                Color headColor = Color.white;

                bool canMoveHead = !EditorApplication.isPlaying && state.editSequence.asset.durationMode == TimelineAsset.DurationMode.FixedLength;

                if (canMoveHead)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        if (m_TimelineDuration.bounds.Contains(Event.current.mousePosition))
                        {
                            if (m_PlayHead != null && m_PlayHead.bounds.Contains(Event.current.mousePosition))
                            {
                                // ignore duration markers if the mouse is over the TimeCursor.
                                canMoveHead = false;
                            }
                        }
                    }
                }
                else
                {
                    lineColor.a *= 0.66f;
                    headColor = DirectorStyles.Instance.customSkin.colorDuration;
                }

                if (canMoveHead)
                    m_TimelineDuration.HandleManipulatorsEvents(state);

                m_TimelineDuration.lineColor = lineColor;
                m_TimelineDuration.headColor = headColor;
                m_TimelineDuration.drawHead = drawhead;
                m_TimelineDuration.drawLine = drawline;
                m_TimelineDuration.canMoveHead = canMoveHead;

                // Draw the TimeAreaItem
                // Rect trackheadRect = treeviewBounds;
                //trackheadRect.height = clientArea.height;
                m_TimelineDuration.Draw(sequenceRect, state, duration);
            }

            // Draw Blue line in timeline indicating the duration...
            if (state.editSequence.asset != null && drawhead)
            {
                HighlightTimeAreaRange(state.editSequence.GetEvaluableRange(), DirectorStyles.Instance.customSkin.colorDurationLine);
            }
        }

        void HighlightTimeAreaRange(Range range, Color lineColor)
        {
            if (range.length <= 0.0 || !state.RangeIsVisible(range)) return;

            Rect lineRect = Rect.MinMaxRect(
                Math.Max(state.TimeToPixel(range.start), state.timeAreaRect.xMin),
                state.timeAreaRect.y - DirectorStyles.kDurationGuiThickness + state.timeAreaRect.height,
                Math.Min(state.TimeToPixel(range.end), state.timeAreaRect.xMax),
                state.timeAreaRect.y + state.timeAreaRect.height);
            EditorGUI.DrawRect(lineRect, lineColor);
        }

        // Drag handler for the gui
        void OnTrackDurationDrag(double newTime)
        {
            if (state.editSequence.asset.durationMode == TimelineAsset.DurationMode.FixedLength && !state.editSequence.isReadOnly)
            {
                // this is the first call to the drag
                if (m_TimelineDuration.firstDrag)
                {
                    TimelineUndo.PushUndo(state.editSequence.asset, "Change Duration");
                }

                state.editSequence.asset.fixedDuration = newTime;

                // when setting a new length, modify the duration of the timeline playable directly instead of
                //  rebuilding the whole graph
                state.UpdateRootPlayableDuration(newTime);
            }

            m_TimelineDuration.showTooltip = true;
        }
    }
}
