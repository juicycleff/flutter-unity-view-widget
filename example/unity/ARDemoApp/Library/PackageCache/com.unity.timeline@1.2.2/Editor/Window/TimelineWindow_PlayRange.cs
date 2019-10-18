using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        TimeAreaItem m_PlayRangeEnd;
        TimeAreaItem m_PlayRangeStart;

        void PlayRangeGUI(TimelineItemArea area)
        {
            if (!currentMode.ShouldShowPlayRange(state) || treeView == null)
                return;

            if (state.masterSequence.asset != null && !state.masterSequence.asset.GetRootTracks().Any())
                return;

            // left Time Cursor
            if (m_PlayRangeStart == null || m_PlayRangeStart.style != styles.playTimeRangeStart)
            {
                m_PlayRangeStart = new TimeAreaItem(styles.playTimeRangeStart, OnTrackHeadMinSelectDrag);
                Vector2 offset = new Vector2(-2.0f, 0);
                m_PlayRangeStart.boundOffset = offset;
            }

            // right Time Cursor
            if (m_PlayRangeEnd == null || m_PlayRangeEnd.style != styles.playTimeRangeEnd)
            {
                m_PlayRangeEnd = new TimeAreaItem(styles.playTimeRangeEnd, OnTrackHeadMaxSelectDrag);
                Vector2 offset = new Vector2(2.0f, 0);
                m_PlayRangeEnd.boundOffset = offset;
            }

            if (area == TimelineItemArea.Header)
                DrawPlayRange(true, false);
            else if (area == TimelineItemArea.Lines)
                DrawPlayRange(false, true);
        }

        void DrawPlayRange(bool drawHeads, bool drawLines)
        {
            Rect timeCursorRect = state.timeAreaRect;
            timeCursorRect.height = clientArea.height;

            m_PlayRangeEnd.HandleManipulatorsEvents(state);
            m_PlayRangeStart.HandleManipulatorsEvents(state);

            // The first time a user enable the play range, we put the play range 75% around the current time...
            if (state.playRange == TimelineAssetViewModel.NoPlayRangeSet)
            {
                float minimumPlayRangeTime = 0.01f;
                float t0 = Mathf.Max(0.0f, state.PixelToTime(state.timeAreaRect.xMin));
                float t1 = Mathf.Min((float)state.masterSequence.duration, state.PixelToTime(state.timeAreaRect.xMax));

                if (Mathf.Abs(t1 - t0) <= minimumPlayRangeTime)
                {
                    state.playRange = new Vector2(t0, t1);
                    return;
                }

                float deltaT = (t1 - t0) * 0.25f / 2.0f;

                t0 += deltaT;
                t1 -= deltaT;

                if (t1 < t0)
                {
                    float temp = t0;
                    t0 = t1;
                    t1 = temp;
                }

                if (Mathf.Abs(t1 - t0) < minimumPlayRangeTime)
                {
                    if (t0 - minimumPlayRangeTime > 0.0f)
                        t0 -= minimumPlayRangeTime;
                    else if (t1 + minimumPlayRangeTime < state.masterSequence.duration)
                        t1 += minimumPlayRangeTime;
                }

                state.playRange = new Vector2(t0, t1);
            }

            // Draw the head or the lines according to the parameters..
            m_PlayRangeStart.drawHead = drawHeads;
            m_PlayRangeStart.drawLine = drawLines;

            m_PlayRangeEnd.drawHead = drawHeads;
            m_PlayRangeEnd.drawLine = drawLines;

            var playRangeTime = state.playRange;
            m_PlayRangeStart.Draw(sequenceContentRect, state, playRangeTime.x);
            m_PlayRangeEnd.Draw(sequenceContentRect, state, playRangeTime.y);

            // Draw Time Range Box from Start to End...
            if (state.playRangeEnabled && m_PlayHead != null)
            {
                Rect rect =
                    Rect.MinMaxRect(
                        Mathf.Clamp(state.TimeToPixel(playRangeTime.x), state.timeAreaRect.xMin, state.timeAreaRect.xMax),
                        m_PlayHead.bounds.yMax,
                        Mathf.Clamp(state.TimeToPixel(playRangeTime.y), state.timeAreaRect.xMin, state.timeAreaRect.xMax),
                        sequenceContentRect.height + state.timeAreaRect.height + timeCursorRect.y
                    );


                EditorGUI.DrawRect(rect, DirectorStyles.Instance.customSkin.colorRange);

                rect.height = 3f;
                EditorGUI.DrawRect(rect, Color.white);
            }
        }

        void OnTrackHeadMinSelectDrag(double newTime)
        {
            Vector2 range = state.playRange;
            range.x = (float)newTime;
            state.playRange = range;
            m_PlayRangeStart.showTooltip = true;
        }

        void OnTrackHeadMaxSelectDrag(double newTime)
        {
            Vector2 range = state.playRange;
            range.y = (float)newTime;
            state.playRange = range;
            m_PlayRangeEnd.showTooltip = true;
        }
    }
}
