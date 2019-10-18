using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        private TimelineAsset m_PreviousMasterSequence;

        public void ClearCurrentTimeline()
        {
            SetCurrentTimeline(null, null, null, true);
        }

        public void SetCurrentTimeline(TimelineAsset seq)
        {
            SetCurrentTimeline(seq, null, null);
        }

        public void SetCurrentTimeline(PlayableDirector director, TimelineClip hostClip = null)
        {
            var asset = director != null ? director.playableAsset as TimelineAsset : null;
            SetCurrentTimeline(asset, director, hostClip);
        }

        void SetCurrentTimeline(TimelineAsset seq, PlayableDirector instanceOfDirector, TimelineClip hostClip, bool force = false)
        {
            if (state == null)
                return;

            if (!force &&
                state.editSequence.hostClip == hostClip &&
                state.editSequence.director == instanceOfDirector &&
                state.editSequence.asset == seq)
                return;

            state.SetCurrentSequence(seq, instanceOfDirector, hostClip);
        }

        void OnBeforeSequenceChange()
        {
            treeView = null;
            m_MarkerHeaderGUI = null;
            m_TimeAreaDirty = true;

            state.Reset();
            m_PlayableLookup.ClearPlayableLookup();

            // clear old editors to caches, like audio previews, get flushed
            CustomTimelineEditorCache.ClearCache<ClipEditor>();
            CustomTimelineEditorCache.ClearCache<MarkerEditor>();
            CustomTimelineEditorCache.ClearCache<TrackEditor>();

            m_PreviousMasterSequence = state.masterSequence.asset;
        }

        void OnAfterSequenceChange()
        {
            Repaint();

            m_SequencePath = state.GetCurrentSequencePath();

            m_LastFrameHadSequence = state.editSequence.asset != null;
            TimelineWindowViewPrefs.SaveAll();

            // this prevent clearing the animation window when going in/out of playmode, but
            // clears it when we switch master timelines
            // the cast to a object will handle the case where the sequence has been deleted.
            object previousMasterSequence = m_PreviousMasterSequence;
            bool isDeleted = previousMasterSequence != null && m_PreviousMasterSequence == null;
            bool hasChanged = m_PreviousMasterSequence != null && m_PreviousMasterSequence != state.masterSequence.asset;
            if (isDeleted  || hasChanged)
                TimelineAnimationUtilities.UnlinkAnimationWindow();
        }
    }
}
