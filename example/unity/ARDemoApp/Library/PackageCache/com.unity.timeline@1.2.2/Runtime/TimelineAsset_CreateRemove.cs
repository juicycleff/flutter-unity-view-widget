using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngineInternal; // for metro type extensions

namespace UnityEngine.Timeline
{
    public partial class TimelineAsset
    {
        /// <summary>
        /// Allows you to create a track and add it to the Timeline.
        /// </summary>
        /// <param name="type">The type of track to create. Must derive from TrackAsset.</param>
        /// <param name="parent">Track to parent to. This can be null.</param>
        /// <param name="name">Name to give the track.</param>
        /// <returns>The created track.</returns>
        /// <remarks>
        /// This method will throw an InvalidOperationException if the parent is not valid. The parent can be any GroupTrack, or a supported parent type of track. For example, this can be used to create override tracks in AnimationTracks.
        /// </remarks>
        public TrackAsset CreateTrack(Type type, TrackAsset parent, string name)
        {
            if (parent != null && parent.timelineAsset != this)
                throw new InvalidOperationException("Addtrack cannot parent to a track not in the Timeline");

            if (!typeof(TrackAsset).IsAssignableFrom(type))
                throw new InvalidOperationException("Supplied type must be a track asset");

            if (parent != null)
            {
                if (!TimelineCreateUtilities.ValidateParentTrack(parent, type))
                    throw new InvalidOperationException("Cannot assign a child of type " + type.Name + " to a parent of type " + parent.GetType().Name);
            }


            var actualParent = parent != null ? parent as PlayableAsset : this;
            TimelineUndo.PushUndo(actualParent, "Create Track");

            var baseName = name;
            if (string.IsNullOrEmpty(baseName))
            {
                baseName = type.Name;
#if UNITY_EDITOR
                baseName = UnityEditor.ObjectNames.NicifyVariableName(baseName);
#endif
            }

            var trackName = baseName;
            if (parent != null)
                trackName = TimelineCreateUtilities.GenerateUniqueActorName(parent.subTracksObjects, baseName);
            else
                trackName = TimelineCreateUtilities.GenerateUniqueActorName(trackObjects, baseName);

            TrackAsset newTrack = AllocateTrack(parent, trackName, type);
            if (newTrack != null)
            {
                newTrack.name = trackName;
                TimelineCreateUtilities.SaveAssetIntoObject(newTrack, actualParent);
            }
            return newTrack;
        }

        /// <summary>
        /// Creates a track and adds it to the Timeline Asset.
        /// </summary>
        /// <param name="parent">Track to parent to. This can be null.</param>
        /// <param name="trackName">The name of the track being created.</param>
        /// <typeparam name="T">The type of track being created. The track type must be derived from TrackAsset.</typeparam>
        /// <returns>Returns the created track.</returns>
        /// <remarks>
        /// This method will throw an InvalidOperationException if the parent is not valid. The parent can be any GroupTrack, or a supported parent type of track. For example, this can be used to create override tracks in AnimationTracks.
        /// </remarks>
        public T CreateTrack<T>(TrackAsset parent, string trackName) where T : TrackAsset, new()
        {
            return (T)CreateTrack(typeof(T), parent, trackName);
        }

        /// <summary>
        /// Creates a track and adds it to the Timeline Asset.
        /// </summary>
        /// <param name="trackName">The name of the track being created.</param>
        /// <typeparam name="T">The type of track being created. The track type must be derived from TrackAsset.</typeparam>
        /// <returns>Returns the created track.</returns>
        public T CreateTrack<T>(string trackName) where T : TrackAsset, new()
        {
            return (T)CreateTrack(typeof(T), null, trackName);
        }

        /// <summary>
        /// Creates a track and adds it to the Timeline Asset.
        /// </summary>
        /// <typeparam name="T">The type of track being created. The track type must be derived from TrackAsset.</typeparam>
        /// <returns>Returns the created track.</returns>
        public T CreateTrack<T>() where T : TrackAsset, new()
        {
            return (T)CreateTrack(typeof(T), null, null);
        }

        /// <summary>
        /// Delete a clip from this timeline.
        /// </summary>
        /// <param name="clip">The clip to delete.</param>
        /// <returns>Returns true if the removal was successful</returns>
        /// <remarks>
        /// This method will delete a clip and any assets owned by the clip.
        /// </remarks>
        public bool DeleteClip(TimelineClip clip)
        {
            if (clip == null || clip.parentTrack == null)
            {
                return false;
            }
            if (this != clip.parentTrack.timelineAsset)
            {
                Debug.LogError("Cannot delete a clip from this timeline");
                return false;
            }

            TimelineUndo.PushUndo(clip.parentTrack, "Delete Clip");
            if (clip.curves != null)
            {
                TimelineUndo.PushDestroyUndo(this, clip.parentTrack, clip.curves, "Delete Curves");
            }

            // handle wrapped assets
            if (clip.asset != null)
            {
                DeleteRecordedAnimation(clip);

                // TODO -- we should flag assets and owned, instead of this check...
#if UNITY_EDITOR
                string path = UnityEditor.AssetDatabase.GetAssetPath(clip.asset);
                if (path == UnityEditor.AssetDatabase.GetAssetPath(this))
#endif
                {
                    TimelineUndo.PushDestroyUndo(this, clip.parentTrack, clip.asset, "Delete Clip Asset");
                }
            }

            var clipParentTrack = clip.parentTrack;
            clipParentTrack.RemoveClip(clip);
            clipParentTrack.CalculateExtrapolationTimes();

            return true;
        }

        /// <summary>
        /// Deletes a track from a timeline, including all clips and subtracks.
        /// </summary>
        /// <param name="track">The track to delete. It must be owned by this Timeline.</param>
        /// <returns>True if the track was deleted successfully.</returns>
        public bool DeleteTrack(TrackAsset track)
        {
            if (track.timelineAsset != this)
                return false;

            // push before we modify properties
            TimelineUndo.PushUndo(track, "Delete Track");
            TimelineUndo.PushUndo(this, "Delete Track");

            TrackAsset parent = track.parent as TrackAsset;
            if (parent != null)
                TimelineUndo.PushUndo(parent, "Delete Track");

            var children = track.GetChildTracks();
            foreach (var child in children)
            {
                DeleteTrack(child);
            }

            DeleteRecordedAnimation(track);

            var clipsToDelete = new List<TimelineClip>(track.clips);
            foreach (var clip in clipsToDelete)
            {
                DeleteClip(clip);
            }
            RemoveTrack(track);

            TimelineUndo.PushDestroyUndo(this, this, track, "Delete Track");

            return true;
        }

        internal void MoveLastTrackBefore(TrackAsset asset)
        {
            if (m_Tracks == null || m_Tracks.Count < 2 || asset == null)
                return;

            var lastTrack = m_Tracks[m_Tracks.Count - 1];
            if (lastTrack == asset)
                return;

            for (int i = 0; i < m_Tracks.Count - 1; i++)
            {
                if (m_Tracks[i] == asset)
                {
                    for (int j = m_Tracks.Count - 1; j > i; j--)
                        m_Tracks[j] = m_Tracks[j - 1];
                    m_Tracks[i] = lastTrack;
                    Invalidate();
                    break;
                }
            }
        }

        internal TrackAsset AllocateTrack(TrackAsset trackAssetParent, string trackName, Type trackType)
        {
            if (trackAssetParent != null && trackAssetParent.timelineAsset != this)
                throw new InvalidOperationException("Addtrack cannot parent to a track not in the Timeline");

            if (!typeof(TrackAsset).IsAssignableFrom(trackType))
                throw new InvalidOperationException("Supplied type must be a track asset");

            var asset = (TrackAsset)CreateInstance(trackType);
            asset.name = trackName;

            if (trackAssetParent != null)
                trackAssetParent.AddChild(asset);
            else
                AddTrackInternal(asset);

            return asset;
        }

        void DeleteRecordedAnimation(TrackAsset track)
        {
            var animTrack = track as AnimationTrack;
            if (animTrack != null && animTrack.infiniteClip != null)
                TimelineUndo.PushDestroyUndo(this, track, animTrack.infiniteClip, "Delete Track");

            if (track.curves != null)
                TimelineUndo.PushDestroyUndo(this, track, track.curves, "Delete Track Parameters");
        }

        void DeleteRecordedAnimation(TimelineClip clip)
        {
            if (clip == null)
                return;

            if (clip.curves != null)
                TimelineUndo.PushDestroyUndo(this, clip.parentTrack, clip.curves, "Delete Clip Parameters");

            if (!clip.recordable)
                return;

            AnimationPlayableAsset asset = clip.asset as AnimationPlayableAsset;
            if (asset == null || asset.clip == null)
                return;

            TimelineUndo.PushDestroyUndo(this, asset, asset.clip, "Delete Recording");
        }
    }
}
