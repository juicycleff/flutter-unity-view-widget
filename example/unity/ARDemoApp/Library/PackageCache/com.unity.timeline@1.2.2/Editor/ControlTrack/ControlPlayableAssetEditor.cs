using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    [CustomTimelineEditor(typeof(ControlPlayableAsset))]
    class ControlPlayableAssetEditor : ClipEditor
    {
        static readonly Texture2D[] s_ParticleSystemIcon = {AssetPreview.GetMiniTypeThumbnail(typeof(ParticleSystem))};

        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var asset = (ControlPlayableAsset)clip.asset;
            var options = base.GetClipOptions(clip);
            if (asset.updateParticle && TimelineEditor.inspectedDirector != null && asset.controllingParticles)
                options.icons = s_ParticleSystemIcon;
            return options;
        }

        public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
            var asset = (ControlPlayableAsset)clip.asset;
            GameObject sourceObject = null;

            // go by sourceObject first, then by prefab
            if (TimelineEditor.inspectedDirector != null)
                sourceObject = asset.sourceGameObject.Resolve(TimelineEditor.inspectedDirector);

            if (sourceObject == null && asset.prefabGameObject != null)
                sourceObject = asset.prefabGameObject;

            if (sourceObject)
            {
                var directors = asset.GetComponent<PlayableDirector>(sourceObject);
                var particleSystems = asset.GetComponent<ParticleSystem>(sourceObject);

                // update the duration and loop values (used for UI purposes) here
                // so they are tied to the latest gameObject bound
                asset.UpdateDurationAndLoopFlag(directors, particleSystems);

                clip.displayName = sourceObject.name;
            }
        }

        public override void GetSubTimelines(TimelineClip clip, PlayableDirector director, List<PlayableDirector> subTimelines)
        {
            var asset = (ControlPlayableAsset)clip.asset;

            // If there is a prefab, it will override the source GameObject
            if (!asset.updateDirector || asset.prefabGameObject != null || director == null)
                return;

            var go = asset.sourceGameObject.Resolve(director);
            if (go == null)
                return;

            foreach (var subTimeline in asset.GetComponent<PlayableDirector>(go))
            {
                if (subTimeline.playableAsset is TimelineAsset)
                    subTimelines.Add(subTimeline);
            }
        }
    }
}
