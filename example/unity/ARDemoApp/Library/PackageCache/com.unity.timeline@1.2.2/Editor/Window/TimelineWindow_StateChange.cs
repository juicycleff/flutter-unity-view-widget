namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        void InitializeStateChange()
        {
            state.OnPlayStateChange += OnPreviewPlayModeChanged;
            state.OnDirtyStampChange += OnStateChange;
            state.OnBeforeSequenceChange += OnBeforeSequenceChange;
            state.OnAfterSequenceChange += OnAfterSequenceChange;

            state.OnRebuildGraphChange += () =>
            {
                // called when the graph is rebuild, since the UI tree isn't necessarily rebuilt.
                if (!state.rebuildGraph)
                {
                    // send callbacks to the tacks
                    if (treeView != null)
                    {
                        var allTrackGuis = treeView.allTrackGuis;
                        if (allTrackGuis != null)
                        {
                            for (int i = 0; i < allTrackGuis.Count; i++)
                                allTrackGuis[i].OnGraphRebuilt();
                        }
                    }
                }
            };

            state.OnTimeChange += () =>
            {
                if (EditorApplication.isPlaying == false)
                {
                    state.UpdateRecordingState();
                    EditorApplication.SetSceneRepaintDirty();
                }

                // the time is sync'd prior to the callback
                state.Evaluate();     // will do the repaint

                InspectorWindow.RepaintAllInspectors();
            };

            state.OnRecordingChange += () =>
            {
                if (!state.recording)
                {
                    TrackAssetRecordingExtensions.ClearRecordingState();
                }
            };
        }
    }
}
