using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    [UsedImplicitly]
    [CustomTimelineEditor(typeof(ActivationTrack))]
    class ActivationTrackEditor : TrackEditor
    {
        static readonly string ClipText = LocalizationDatabase.GetLocalizedString("Active");

        static readonly string k_ErrorParentString = LocalizationDatabase.GetLocalizedString("The bound GameObject is a parent of the PlayableDirector.");
        static readonly string k_ErrorString = LocalizationDatabase.GetLocalizedString("The bound GameObject contains the PlayableDirector.");

        public override TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            var options = base.GetTrackOptions(track, binding);
            options.errorText = GetErrorText(track, binding);
            return options;
        }

        string GetErrorText(TrackAsset track, Object binding)
        {
            var gameObject = binding as GameObject;
            var currentDirector = TimelineEditor.inspectedDirector;
            if (gameObject != null && currentDirector != null)
            {
                var director = gameObject.GetComponent<PlayableDirector>();
                if (currentDirector == director)
                {
                    return k_ErrorString;
                }

                if (currentDirector.gameObject.transform.IsChildOf(gameObject.transform))
                {
                    return k_ErrorParentString;
                }
            }

            return base.GetErrorText(track, binding, TrackBindingErrors.PrefabBound);
        }

        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            // Add a default clip to the newly created track
            if (copiedFrom == null)
            {
                var clip = track.CreateClip(0);
                clip.displayName = ClipText;
                clip.duration = System.Math.Max(clip.duration, track.timelineAsset.duration * 0.5f);
            }
        }
    }
}
