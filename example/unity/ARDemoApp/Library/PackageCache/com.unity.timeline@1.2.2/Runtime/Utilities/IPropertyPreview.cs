using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Implement this interface in a PlayableAsset to specify which properties will be modified when Timeline is in preview mode.
    /// </summary>
    public interface IPropertyPreview
    {
        /// <summary>
        /// Called by the Timeline Editor to gather properties requiring preview.
        /// </summary>
        /// <param name="director">The PlayableDirector invoking the preview</param>
        /// <param name="driver">PropertyCollector used to gather previewable properties</param>
        void GatherProperties(PlayableDirector director, IPropertyCollector driver);
    }
}
