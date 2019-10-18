using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class TimeReferenceUtility
    {
        static WindowState state { get { return TimelineWindow.instance.state; } }

        public static double SnapToFrame(double time)
        {
            if (state.timeReferenceMode == TimeReferenceMode.Global)
            {
                time = state.editSequence.ToGlobalTime(time);
                time = TimeUtility.RoundToFrame(time, state.referenceSequence.frameRate);
                return state.editSequence.ToLocalTime(time);
            }

            return TimeUtility.RoundToFrame(time, state.referenceSequence.frameRate);
        }

        public static string ToTimeString(double time, string format = "F2")
        {
            if (state.timeReferenceMode == TimeReferenceMode.Global)
                time = state.editSequence.ToGlobalTime(time);

            return state.editSequence.viewModel.timeInFrames ?
                TimeUtility.TimeAsFrames(time, state.referenceSequence.frameRate, format) :
                TimeUtility.TimeAsTimeCode(time, state.referenceSequence.frameRate, format);
        }

        public static double FromTimeString(string timeString)
        {
            double newTime;

            if (state.timeInFrames)
            {
                double newFrameDouble;
                if (double.TryParse(timeString, out newFrameDouble))
                    newTime = TimeUtility.FromFrames(newFrameDouble, state.referenceSequence.frameRate);
                else
                    newTime = state.editSequence.time;
            }
            else
            {
                newTime = TimeUtility.ParseTimeCode(timeString, state.referenceSequence.frameRate, -1);
            }

            if (newTime >= 0.0)
            {
                return state.timeReferenceMode == TimeReferenceMode.Global ?
                    state.editSequence.ToLocalTime(newTime) : newTime;
            }

            return state.editSequence.time;
        }
    }
}
