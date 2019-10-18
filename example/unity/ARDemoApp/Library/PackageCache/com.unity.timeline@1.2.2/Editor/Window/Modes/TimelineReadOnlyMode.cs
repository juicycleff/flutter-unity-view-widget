namespace UnityEditor.Timeline
{
    class TimelineReadOnlyMode : TimelineMode
    {
        public TimelineReadOnlyMode()
        {
            headerState = new HeaderState()
            {
                breadCrumb = TimelineModeGUIState.Enabled,
                options =  TimelineModeGUIState.Enabled,
                sequenceSelector = TimelineModeGUIState.Enabled,
            };

            trackOptionsState = new TrackOptionsState()
            {
                newButton =  TimelineModeGUIState.Disabled,
                editAsAssetButton = TimelineModeGUIState.Disabled,
            };
            mode = TimelineModes.ReadOnly;
        }

        public override bool ShouldShowPlayRange(WindowState state)
        {
            return state.editSequence.director != null && state.playRangeEnabled;
        }

        public override bool ShouldShowTimeCursor(WindowState state)
        {
            return state.editSequence.director != null;
        }

        public override TimelineModeGUIState TrackState(WindowState state)
        {
            return TimelineModeGUIState.Disabled;
        }

        public override TimelineModeGUIState ToolbarState(WindowState state)
        {
            return state.editSequence.director == null ? TimelineModeGUIState.Disabled : TimelineModeGUIState.Enabled;
        }

        public override TimelineModeGUIState PreviewState(WindowState state)
        {
            return state.editSequence.director == null ? TimelineModeGUIState.Disabled : TimelineModeGUIState.Enabled;
        }

        public override TimelineModeGUIState EditModeButtonsState(WindowState state)
        {
            return TimelineModeGUIState.Disabled;
        }
    }
}
