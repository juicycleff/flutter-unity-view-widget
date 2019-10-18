namespace UnityEditor.Timeline
{
    class TimelineActiveMode : TimelineMode
    {
        public TimelineActiveMode()
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
                editAsAssetButton = TimelineModeGUIState.Hidden
            };
            mode = TimelineModes.Active;
        }

        public override bool ShouldShowTimeCursor(WindowState state)
        {
            return true;
        }

        public override bool ShouldShowPlayRange(WindowState state)
        {
            return state.playRangeEnabled;
        }

        public override TimelineModeGUIState ToolbarState(WindowState state)
        {
            return TimelineModeGUIState.Enabled;
        }

        public override TimelineModeGUIState TrackState(WindowState state)
        {
            return TimelineModeGUIState.Enabled;
        }
    }
}
