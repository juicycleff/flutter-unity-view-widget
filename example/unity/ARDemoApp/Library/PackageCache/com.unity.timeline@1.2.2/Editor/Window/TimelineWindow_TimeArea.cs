using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        [NonSerialized] TimelineTimeArea m_TimeArea;
        public TimeArea timeArea { get { return m_TimeArea; } }

        internal static class Styles
        {
            public static string DurationModeText = L10n.Tr("Duration Mode/{0}");
        }

        float m_LastFrameRate;
        bool m_TimeAreaDirty = true;

        void InitializeTimeArea()
        {
            if (m_TimeArea == null)
            {
                m_TimeArea = new TimelineTimeArea(state, false)
                {
                    hRangeLocked = false,
                    vRangeLocked = true,
                    margin = 10,
                    scaleWithWindow = true,
                    hSlider = true,
                    vSlider = false,
                    hBaseRangeMin = 0.0f,
                    hBaseRangeMax = WindowState.kMaxShownTime,
                    hRangeMin = 0.0f,
                    hScaleMax = WindowConstants.maxTimeAreaScaling,
                    rect = state.timeAreaRect
                };

                m_TimeAreaDirty = true;
                InitTimeAreaFrameRate();
                SyncTimeAreaShownRange();
            }
        }

        void TimelineGUI()
        {
            if (!currentMode.ShouldShowTimeArea(state))
                return;

            Rect rect = state.timeAreaRect;
            m_TimeArea.rect = new Rect(rect.x, rect.y, rect.width, clientArea.height - rect.y);

            if (m_LastFrameRate != state.referenceSequence.frameRate)
                InitTimeAreaFrameRate();

            SyncTimeAreaShownRange();

            m_TimeArea.BeginViewGUI();
            m_TimeArea.TimeRuler(rect, state.referenceSequence.frameRate, true, false, 1.0f, state.timeInFrames ? TimeArea.TimeFormat.Frame : TimeArea.TimeFormat.TimeFrame);
            m_TimeArea.EndViewGUI();
        }

        void InitTimeAreaFrameRate()
        {
            m_LastFrameRate = state.referenceSequence.frameRate;
            m_TimeArea.hTicks.SetTickModulosForFrameRate(m_LastFrameRate);
        }

        void SyncTimeAreaShownRange()
        {
            var range = state.timeAreaShownRange;
            if (!Mathf.Approximately(range.x, m_TimeArea.shownArea.x) || !Mathf.Approximately(range.y, m_TimeArea.shownArea.xMax))
            {
                // set view data onto the time area
                if (m_TimeAreaDirty)
                {
                    m_TimeArea.SetShownHRange(range.x, range.y);
                    m_TimeAreaDirty = false;
                }
                else
                {
                    // set time area data onto the view data
                    state.TimeAreaChanged();
                }
            }

            m_TimeArea.hBaseRangeMax = (float)state.editSequence.duration;
        }

        class TimelineTimeArea : TimeArea
        {
            readonly WindowState m_State;

            public TimelineTimeArea(WindowState state, bool minimalGUI) : base(minimalGUI)
            {
                m_State = state;
            }

            public override string FormatTickTime(float time, float frameRate, TimeFormat timeFormat)
            {
                time = m_State.timeReferenceMode == TimeReferenceMode.Global ?
                    (float)m_State.editSequence.ToGlobalTime(time) : time;

                return FormatTime(time, frameRate, timeFormat);
            }
        }
    }
}
