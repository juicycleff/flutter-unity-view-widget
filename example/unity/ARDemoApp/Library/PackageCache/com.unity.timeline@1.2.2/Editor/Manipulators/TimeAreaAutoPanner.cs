using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimeAreaAutoPanner
    {
        readonly WindowState m_State;
        readonly TimelineWindow m_Window;
        readonly Rect m_ViewRect;

        const float k_PixelDistanceToMaxSpeed = 100.0f;
        const float k_MaxPanSpeed = 30.0f;

        public TimeAreaAutoPanner(WindowState state)
        {
            m_State = state;
            m_Window = m_State.GetWindow();

            var shownRange = m_State.timeAreaShownRange;
            var trackViewBounds = m_Window.sequenceRect;
            m_ViewRect = Rect.MinMaxRect(m_State.TimeToPixel(shownRange.x), trackViewBounds.yMin,
                m_State.TimeToPixel(shownRange.y), trackViewBounds.yMax);
        }

        public void OnGUI(Event evt)
        {
            if (evt.type != EventType.Layout)
                return;

            var hFactor = 0.0f;
            var vFactor = 0.0f;

            bool horizontalPan = GetPanFactor(evt.mousePosition.x, m_ViewRect.xMin, m_ViewRect.xMax, out hFactor);
            bool verticalPan = GetPanFactor(evt.mousePosition.y, m_ViewRect.yMin, m_ViewRect.yMax, out vFactor);

            if (horizontalPan)
            {
                var translation = m_State.timeAreaTranslation;
                translation.x += hFactor * k_MaxPanSpeed;

                m_State.SetTimeAreaTransform(translation, m_State.timeAreaScale);
            }

            if (verticalPan)
            {
                var translation = m_Window.treeView.scrollPosition;
                translation.y -= vFactor * k_MaxPanSpeed;

                m_Window.treeView.scrollPosition = translation;
            }
        }

        static bool GetPanFactor(float v, float min, float max, out float factor)
        {
            factor = 0.0f;

            if (v < min)
            {
                factor = Mathf.Clamp01((min - v) / k_PixelDistanceToMaxSpeed);
                return true;
            }

            if (v > max)
            {
                factor = -Mathf.Clamp01((v - max) / k_PixelDistanceToMaxSpeed);
                return true;
            }

            return false;
        }
    }
}
