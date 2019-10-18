using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
    enum TimelineModeGUIState
    {
        Disabled,
        Hidden,
        Enabled
    }

    abstract class TimelineMode
    {
        public struct HeaderState
        {
            public TimelineModeGUIState breadCrumb;
            public TimelineModeGUIState sequenceSelector;
            public TimelineModeGUIState options;
        }

        public struct TrackOptionsState
        {
            public TimelineModeGUIState newButton;
            public TimelineModeGUIState editAsAssetButton;
        }

        public HeaderState headerState { get; protected set; }
        public TrackOptionsState trackOptionsState { get; protected set; }
        public TimelineModes mode { get; protected set; }

        public abstract bool ShouldShowPlayRange(WindowState state);
        public abstract bool ShouldShowTimeCursor(WindowState state);

        public virtual bool ShouldShowTrackBindings(WindowState state)
        {
            return ShouldShowTimeCursor(state);
        }

        public virtual bool ShouldShowTimeArea(WindowState state)
        {
            return !state.IsEditingAnEmptyTimeline();
        }

        public abstract TimelineModeGUIState TrackState(WindowState state);
        public abstract TimelineModeGUIState ToolbarState(WindowState state);

        public virtual TimelineModeGUIState PreviewState(WindowState state)
        {
            return Application.isPlaying ? TimelineModeGUIState.Disabled : TimelineModeGUIState.Enabled;
        }

        public virtual TimelineModeGUIState EditModeButtonsState(WindowState state)
        {
            return TimelineModeGUIState.Enabled;
        }
    }

    [Flags]
    internal enum TimelineModes
    {
        None = 0,
        Active = 1,
        ReadOnly = 2,
        Inactive = 4,
        Disabled = 8,
        AssetEdition = 16,
        All = Active | ReadOnly | Inactive | Disabled,
        Default = Active | AssetEdition
    }
}
