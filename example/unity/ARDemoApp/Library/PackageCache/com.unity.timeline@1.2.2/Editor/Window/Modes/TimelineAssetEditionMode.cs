namespace UnityEditor.Timeline
{
    class TimelineAssetEditionMode : TimelineInactiveMode
    {
        public override TimelineModeGUIState TrackState(WindowState state)
        {
            return TimelineModeGUIState.Enabled;
        }

        public TimelineAssetEditionMode()
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
            mode = TimelineModes.AssetEdition;
        }
    }
}
