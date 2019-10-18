using System;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomTrackDrawer(typeof(AnimationTrack)), UsedImplicitly]
    class AnimationTrackDrawer : TrackDrawer
    {
        internal static class Styles
        {
            public static readonly GUIContent AnimationButtonOnTooltip = EditorGUIUtility.TrTextContent("", "Avatar Mask enabled\nClick to disable");
            public static readonly GUIContent AnimationButtonOffTooltip = EditorGUIUtility.TrTextContent("", "Avatar Mask disabled\nClick to enable");
        }

        public override bool DrawTrackHeaderButton(Rect rect, TrackAsset track, WindowState state)
        {
            var animTrack = track as AnimationTrack;
            bool hasAvatarMask = animTrack != null && animTrack.avatarMask != null;
            if (hasAvatarMask)
            {
                var style = animTrack.applyAvatarMask
                    ? DirectorStyles.Instance.avatarMaskOn
                    : DirectorStyles.Instance.avatarMaskOff;
                var tooltip = animTrack.applyAvatarMask
                    ? Styles.AnimationButtonOnTooltip
                    : Styles.AnimationButtonOffTooltip;
                if (GUI.Button(rect, tooltip, style))
                {
                    animTrack.applyAvatarMask = !animTrack.applyAvatarMask;
                    if (state != null)
                        state.rebuildGraph = true;
                }
            }
            return hasAvatarMask;
        }

        public override void DrawRecordingBackground(Rect trackRect, TrackAsset trackAsset, Vector2 visibleTime, WindowState state)
        {
            base.DrawRecordingBackground(trackRect, trackAsset, visibleTime, state);
            DrawBorderOfAddedRecordingClip(trackRect, trackAsset, visibleTime, (WindowState)state);
        }

        static void DrawBorderOfAddedRecordingClip(Rect trackRect, TrackAsset trackAsset, Vector2 visibleTime, WindowState state)
        {
            if (!state.IsArmedForRecord(trackAsset))
                return;

            AnimationTrack animTrack = trackAsset as AnimationTrack;
            if (animTrack == null || !animTrack.inClipMode)
                return;

            // make sure there is no clip but we can add one
            TimelineClip clip = null;
            if (trackAsset.FindRecordingClipAtTime(state.editSequence.time, out clip) || clip != null)
                return;

            float yMax = trackRect.yMax;
            float yMin = trackRect.yMin;

            double startGap = 0;
            double endGap = 0;

            trackAsset.GetGapAtTime(state.editSequence.time, out startGap, out endGap);
            if (double.IsInfinity(endGap))
                endGap = visibleTime.y;

            if (startGap > visibleTime.y || endGap < visibleTime.x)
                return;


            startGap = Math.Max(startGap, visibleTime.x);
            endGap = Math.Min(endGap, visibleTime.y);

            float xMin = state.TimeToPixel(startGap);
            float xMax = state.TimeToPixel(endGap);

            Rect r = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
            ClipDrawer.DrawBorder(r, ClipBorder.kRecording, ClipBlends.kNone);
        }
    }
}
