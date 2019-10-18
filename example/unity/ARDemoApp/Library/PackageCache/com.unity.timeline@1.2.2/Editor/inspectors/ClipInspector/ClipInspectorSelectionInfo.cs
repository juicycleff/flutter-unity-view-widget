using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class ClipInspectorSelectionInfo
    {
        public double start, end, duration;
        public double multipleClipStart, multipleClipEnd;
        public double smallestDuration;

        public bool hasMultipleStartValues, hasMultipleEndValues, hasMultipleDurationValues;
        public bool supportsExtrapolation, supportsClipIn, supportsSpeedMultiplier, supportsBlending;
        public bool hasBlendIn, hasBlendOut;
        public bool selectedAssetTypesAreHomogeneous;
        public bool containsAtLeastTwoClipsOnTheSameTrack;

        public HashSet<TrackAsset> uniqueParentTracks = new HashSet<TrackAsset>();
        public ICollection<TimelineClip> clips { get; private set; }

        public ClipInspectorSelectionInfo(ICollection<TimelineClip> selectedClips)
        {
            supportsBlending = supportsClipIn = supportsExtrapolation = supportsSpeedMultiplier = true;
            hasBlendIn = hasBlendOut = true;
            selectedAssetTypesAreHomogeneous = true;
            smallestDuration = TimelineClip.kMaxTimeValue;
            start = end = duration = 0;
            multipleClipStart = multipleClipEnd = 0;
            hasMultipleStartValues = hasMultipleEndValues = hasMultipleDurationValues = false;
            containsAtLeastTwoClipsOnTheSameTrack = false;

            clips = selectedClips;
            Build();
        }

        void Build()
        {
            if (!clips.Any()) return;

            var firstSelectedClip = clips.First();
            if (firstSelectedClip == null) return;

            var firstSelectedClipAssetType = firstSelectedClip.asset != null ? firstSelectedClip.asset.GetType() : null;

            smallestDuration = TimelineClip.kMaxTimeValue;
            InitSelectionBounds(firstSelectedClip);
            InitMultipleClipBounds(firstSelectedClip);

            foreach (var clip in clips)
            {
                if (clip == null) continue;

                uniqueParentTracks.Add(clip.parentTrack);
                selectedAssetTypesAreHomogeneous &= clip.asset.GetType() == firstSelectedClipAssetType;

                UpdateClipCaps(clip);
                UpdateBlends(clip);
                UpdateSmallestDuration(clip);
                UpdateMultipleValues(clip);
                UpdateMultipleValues(clip);
            }
            containsAtLeastTwoClipsOnTheSameTrack = uniqueParentTracks.Count != clips.Count;
        }

        public void Update()
        {
            var firstSelectedClip = clips.First();
            if (firstSelectedClip == null) return;

            hasBlendIn = hasBlendOut = true;
            hasMultipleStartValues = hasMultipleDurationValues = hasMultipleEndValues = false;
            smallestDuration = TimelineClip.kMaxTimeValue;
            InitSelectionBounds(firstSelectedClip);
            InitMultipleClipBounds(firstSelectedClip);

            foreach (var clip in clips)
            {
                if (clip == null) continue;

                UpdateBlends(clip);
                UpdateSmallestDuration(clip);
                UpdateMultipleValues(clip);
            }
        }

        void InitSelectionBounds(TimelineClip clip)
        {
            start = clip.start;
            duration = clip.duration;
            end = clip.start + clip.duration;
        }

        void InitMultipleClipBounds(TimelineClip firstSelectedClip)
        {
            multipleClipStart = firstSelectedClip.start;
            multipleClipEnd = end;
        }

        void UpdateSmallestDuration(TimelineClip clip)
        {
            smallestDuration = Math.Min(smallestDuration, clip.duration);
        }

        void UpdateClipCaps(TimelineClip clip)
        {
            supportsBlending &= clip.SupportsBlending();
            supportsClipIn &= clip.SupportsClipIn();
            supportsExtrapolation &= clip.SupportsExtrapolation();
            supportsSpeedMultiplier &= clip.SupportsSpeedMultiplier();
        }

        void UpdateMultipleValues(TimelineClip clip)
        {
            hasMultipleStartValues |= !Mathf.Approximately((float)clip.start, (float)start);
            hasMultipleDurationValues |= !Mathf.Approximately((float)clip.duration, (float)duration);
            var clipEnd = clip.start + clip.duration;
            hasMultipleEndValues |= !Mathf.Approximately((float)clipEnd, (float)end);

            multipleClipStart = Math.Min(multipleClipStart, clip.start);
            multipleClipEnd = Math.Max(multipleClipEnd, clip.end);
        }

        void UpdateBlends(TimelineClip clip)
        {
            hasBlendIn &= clip.hasBlendIn;
            hasBlendOut &= clip.hasBlendOut;
        }
    }
}
