namespace UnityEditor.Timeline
{
    class TimelineInactiveMode : TimelineMode
    {
        public TimelineInactiveMode()
        {
            headerState = new HeaderState
            {
                breadCrumb = TimelineModeGUIState.Disabled,
                options = TimelineModeGUIState.Enabled,
                sequenceSelector = TimelineModeGUIState.Enabled
            };

            trackOptionsState = new TrackOptionsState
            {
                newButton = TimelineModeGUIState.Disabled,
                editAsAssetButton = TimelineModeGUIState.Enabled
            };
            mode = TimelineModes.Inactive;
        }

        public override bool ShouldShowPlayRange(WindowState state)
        {
            return false;
        }

        public override bool ShouldShowTimeCursor(WindowState state)
        {
            return false;
        }

        public override TimelineModeGUIState ToolbarState(WindowState state)
        {
            return TimelineModeGUIState.Disabled;
        }

        public override TimelineModeGUIState TrackState(WindowState state)
        {
            return TimelineModeGUIState.Disabled;
        }

        public override TimelineModeGUIState PreviewState(WindowState state)
        {
            return TimelineModeGUIState.Disabled;
        }
    }
}
