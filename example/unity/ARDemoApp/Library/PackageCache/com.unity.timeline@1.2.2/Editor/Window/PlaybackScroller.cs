using UnityEngine;

namespace UnityEditor.Timeline
{
    enum PlaybackScrollMode
    {
        None,
        Pan,
        Smooth
    }

    static class PlaybackScroller
    {
        public static void AutoScroll(WindowState state)
        {
            if (Event.current.type != EventType.Layout)
                return;

            switch (state.autoScrollMode)
            {
                case PlaybackScrollMode.Pan:
                    DoPanScroll(state);
                    break;
                case PlaybackScrollMode.Smooth:
                    DoSmoothScroll(state);
                    break;
            }
        }

        static void DoSmoothScroll(WindowState state)
        {
            if (state.playing)
                state.SetPlayHeadToMiddle();

            state.UpdateLastFrameTime();
        }

        static void DoPanScroll(WindowState state)
        {
            if (!state.playing)
                return;

            var paddingDeltaTime = state.PixelDeltaToDeltaTime(WindowConstants.autoPanPaddingInPixels);
            var showRange = state.timeAreaShownRange;
            var rightBoundForPan = showRange.y - paddingDeltaTime;
            if (state.editSequence.time > rightBoundForPan)
            {
                var leftBoundForPan = showRange.x + paddingDeltaTime;
                var delta = rightBoundForPan - leftBoundForPan;
                state.SetTimeAreaShownRange(showRange.x + delta, showRange.y + delta);
            }
        }
    }
}
