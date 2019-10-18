using System;
using System.ComponentModel;

namespace UnityEngine.Timeline
{
    partial class AnimationTrack
    {
        // 649 is value is only assigned to. they can be updated from old files being serialized
 #pragma warning disable 649
        //fields that are used for upgrading should be put here, ideally as read-only
        [SerializeField, Obsolete("Use m_InfiniteClipOffsetEulerAngles Instead", false), HideInInspector]
        Quaternion m_OpenClipOffsetRotation = Quaternion.identity;

        [SerializeField, Obsolete("Use m_RotationEuler Instead", false), HideInInspector]
        Quaternion m_Rotation = Quaternion.identity;

        [SerializeField, Obsolete("Use m_RootTransformOffsetMode", false), HideInInspector]
        bool m_ApplyOffsets;
 #pragma warning restore 649

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("openClipOffsetPosition has been deprecated. Use infiniteClipOffsetPosition instead. (UnityUpgradable) -> infiniteClipOffsetPosition", true)]
        public Vector3 openClipOffsetPosition
        {
            get { return infiniteClipOffsetPosition; }
            set { infiniteClipOffsetPosition = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("openClipOffsetRotation has been deprecated. Use infiniteClipOffsetRotation instead. (UnityUpgradable) -> infiniteClipOffsetRotation", true)]
        public Quaternion openClipOffsetRotation
        {
            get { return infiniteClipOffsetRotation; }
            set { infiniteClipOffsetRotation = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("openClipOffsetEulerAngles has been deprecated. Use infiniteClipOffsetEulerAngles instead. (UnityUpgradable) -> infiniteClipOffsetEulerAngles", true)]
        public Vector3 openClipOffsetEulerAngles
        {
            get { return infiniteClipOffsetEulerAngles; }
            set { infiniteClipOffsetEulerAngles = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("openClipPreExtrapolation has been deprecated. Use infiniteClipPreExtrapolation instead. (UnityUpgradable) -> infiniteClipPreExtrapolation", true)]
        public TimelineClip.ClipExtrapolation openClipPreExtrapolation
        {
            get { return infiniteClipPreExtrapolation; }
            set { infiniteClipPreExtrapolation = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("openClipPostExtrapolation has been deprecated. Use infiniteClipPostExtrapolation instead. (UnityUpgradable) -> infiniteClipPostExtrapolation", true)]
        public TimelineClip.ClipExtrapolation openClipPostExtrapolation
        {
            get { return infiniteClipPostExtrapolation; }
            set { infiniteClipPostExtrapolation = value; }
        }

        internal override void OnUpgradeFromVersion(int oldVersion)
        {
            if (oldVersion < (int)Versions.RotationAsEuler)
                AnimationTrackUpgrade.ConvertRotationsToEuler(this);
            if (oldVersion < (int)Versions.RootMotionUpgrade)
                AnimationTrackUpgrade.ConvertRootMotion(this);
            if (oldVersion < (int)Versions.AnimatedTrackProperties)
                AnimationTrackUpgrade.ConvertInfiniteTrack(this);
        }

// 612 is Property is Obsolete
// 618 is Field is Obsolete
#pragma warning disable 612, 618
        static class AnimationTrackUpgrade
        {
            public static void ConvertRotationsToEuler(AnimationTrack track)
            {
                track.m_EulerAngles = track.m_Rotation.eulerAngles;
                track.m_InfiniteClipOffsetEulerAngles = track.m_OpenClipOffsetRotation.eulerAngles;
            }

            public static void ConvertRootMotion(AnimationTrack track)
            {
                track.m_TrackOffset = TrackOffset.Auto; // loaded tracks should use legacy mode

                // reset offsets if not applied
                if (!track.m_ApplyOffsets)
                {
                    track.m_Position = Vector3.zero;
                    track.m_EulerAngles = Vector3.zero;
                }
            }

            public static void ConvertInfiniteTrack(AnimationTrack track)
            {
                track.m_InfiniteClip = track.m_AnimClip;
                track.m_AnimClip = null;
            }
        }
#pragma warning restore 612, 618
    }
}
