using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    partial class TimelineWindow
    {
        PlayableLookup m_PlayableLookup = new PlayableLookup();

        class PlayableLookup
        {
            const int k_InitialDictionarySize = 10;

            readonly Dictionary<AnimationClip, Playable> m_AnimationClipToPlayable =
                new Dictionary<AnimationClip, Playable>(k_InitialDictionarySize);
            readonly Dictionary<AnimationClip, TimelineClip> m_AnimationClipToTimelineClip =
                new Dictionary<AnimationClip, TimelineClip>(k_InitialDictionarySize);

            public void UpdatePlayableLookup(TimelineClip clip, GameObject go, Playable p)
            {
                if (clip == null || go == null || !p.IsValid())
                    return;

                if (clip.curves != null)
                    m_AnimationClipToTimelineClip[clip.curves] = clip;

                UpdatePlayableLookup(clip.parentTrack.timelineAsset, clip, go, p);
            }

            public void UpdatePlayableLookup(TrackAsset track, GameObject go, Playable p)
            {
                if (track == null || go == null || !p.IsValid())
                    return;

                UpdatePlayableLookup(track.timelineAsset, track, go, p);
            }

            void UpdatePlayableLookup(TimelineAsset timelineAsset, ICurvesOwner curvesOwner, GameObject go, Playable p)
            {
                var director = go.GetComponent<PlayableDirector>();
                var editingDirector = instance.state.editSequence.director;
                // No Asset mode update
                if (curvesOwner.curves != null && director != null && director == editingDirector &&
                    timelineAsset == instance.state.editSequence.asset)
                {
                    m_AnimationClipToPlayable[curvesOwner.curves] = p;
                }
            }

            public bool GetPlayableFromAnimClip(AnimationClip clip, out Playable p)
            {
                if (clip == null)
                {
                    p = Playable.Null;
                    return false;
                }

                return m_AnimationClipToPlayable.TryGetValue(clip, out p);
            }

            public TimelineClip GetTimelineClipFromCurves(AnimationClip clip)
            {
                if (clip == null)
                    return null;

                TimelineClip timelineClip = null;
                m_AnimationClipToTimelineClip.TryGetValue(clip, out timelineClip);
                return timelineClip;
            }

            public void ClearPlayableLookup()
            {
                m_AnimationClipToPlayable.Clear();
            }
        }
    }
}
