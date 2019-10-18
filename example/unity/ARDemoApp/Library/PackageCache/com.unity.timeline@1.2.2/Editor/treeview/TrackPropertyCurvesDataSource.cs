using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TrackPropertyCurvesDataSource : BasePropertyKeyDataSource
    {
        protected override AnimationClip animationClip { get; }

        public TrackPropertyCurvesDataSource(TrackAsset track)
        {
            animationClip = track != null ? track.curves : null;
        }
    }
}
