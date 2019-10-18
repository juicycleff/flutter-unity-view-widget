using UnityEngine;

namespace UnityEditor.Timeline
{
    class TimelineDisabledMode : TimelineMode
    {
        public TimelineDisabledMode()
        {
            headerState = new HeaderState
            {
                breadCrumb = TimelineModeGUIState.Enabled,
                options = TimelineModeGUIState.Enabled,
                sequenceSelector = TimelineModeGUIState.Enabled
            };

            trackOptionsState = new TrackOptionsState
            {
                newButton = TimelineModeGUIState.Enabled,
                editAsAssetButton = TimelineModeGUIState.Enabled
            };
            mode = TimelineModes.Disabled;
        }

        public override bool ShouldShowPlayRange(WindowState state)
        {
            return false;
        }

        public override bool ShouldShowTimeCursor(WindowState state)
        {
            return true;
        }

        public override TimelineModeGUIState ToolbarState(WindowState state)
        {
            return TimelineModeGUIState.Disabled;
        }

        public override TimelineModeGUIState TrackState(WindowState state)
        {
            return TimelineModeGUIState.Enabled;
        }
    }
}
