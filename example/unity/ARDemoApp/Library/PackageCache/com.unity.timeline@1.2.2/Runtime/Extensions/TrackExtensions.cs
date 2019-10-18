using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Extension methods for TrackAssets
    /// </summary>
    public static class TrackAssetExtensions
    {
        /// <summary>
        /// Gets the GroupTrack this track belongs to.
        /// </summary>
        /// <param name="asset">The track asset to find the group of</param>
        /// <returns>The parent GroupTrack or null if the Track is an override track, or root track.</returns>
        public static GroupTrack GetGroup(this TrackAsset asset)
        {
            if (asset == null)
                return null;

            return asset.parent as GroupTrack;
        }

        /// <summary>
        /// Assigns the track to the specified group track.
        /// </summary>
        /// <param name="asset">The track to assign.</param>
        /// <param name="group">The GroupTrack to assign the track to.</param>
        /// <remarks>
        /// Does not support assigning to a group in a different timeline.
        /// </remarks>
        public static void SetGroup(this TrackAsset asset, GroupTrack group)
        {
            const string undoString = "Reparent";

            if (asset == null || asset == group || asset.parent == group)
                return;

            if (group != null && asset.timelineAsset != group.timelineAsset)
                throw new InvalidOperationException("Cannot assign to a group in a different timeline");


            TimelineUndo.PushUndo(asset, undoString);

            var timeline = asset.timelineAsset;
            var parentTrack = asset.parent as TrackAsset;
            var parentTimeline = asset.parent as TimelineAsset;
            if (parentTrack != null || parentTimeline != null)
            {
                TimelineUndo.PushUndo(asset.parent, undoString);
                if (parentTimeline != null)
                {
                    parentTimeline.RemoveTrack(asset);
                }
                else
                {
                    parentTrack.RemoveSubTrack(asset);
                }
            }

            if (group == null)
            {
                TimelineUndo.PushUndo(timeline, undoString);
                asset.parent = asset.timelineAsset;
                timeline.AddTrackInternal(asset);
            }
            else
            {
                TimelineUndo.PushUndo(group, undoString);
                group.AddChild(asset);
            }
        }
    }
}
