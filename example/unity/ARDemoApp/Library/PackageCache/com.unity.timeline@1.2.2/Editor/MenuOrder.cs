using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    static class MenuOrder
    {
        // by default, adds at the end, before 'add'
        public const int DefaultPriority = 9000;
        public const int SeparatorAt = 1000;

        public static class TimelineAction
        {
            public const int Start = 1000;
            public const int Copy = Start + 100;
            public const int Paste = Start + 200;
            public const int Duplicate = Start + 300;
            public const int Delete = Start + 400;
            public const int MatchContent = Start + 500;
        }

        public static class TrackAction
        {
            public const int Start = TimelineAction.Start + SeparatorAt;

            public const int LockTrack = Start + 100;
            public const int LockSelected = Start + 150;
            public const int MuteTrack = Start + 200;
            public const int MuteSelected = Start + 250;
            public const int ShowHideMarkers = Start + 300;
            public const int RemoveInvalidMarkers = Start + 400;
            public const int EditInAnimationWindow = Start + 800;
        }

        public static class TrackAddMenu
        {
            public const int Start = TrackAction.Start + SeparatorAt;
            public const int AddLayerTrack = Start;
        }

        public static class ClipEditAction
        {
            public const int Start = TrackAddMenu.Start + SeparatorAt;
            public const int EditInAnimationWindow = Start + 100;
            public const int EditSubTimeline = Start + 200;
        }

        public static class ClipAction
        {
            public const int Start = ClipEditAction.Start + SeparatorAt;

            public const int TrimStart = Start + 100;
            public const int TrimEnd = Start + 110;
            public const int Split = Start + 120;
            public const int CompleteLastLoop = Start + SeparatorAt;
            public const int TrimLastLoop = Start + SeparatorAt + 110;
            public const int MatchDuration = Start + SeparatorAt + 120;
            public const int DoubleSpeed = Start + 2 * SeparatorAt;
            public const int HalfSpeed = Start + 2 * SeparatorAt + 110;
            public const int ResetDuration = Start + 3 * SeparatorAt;
            public const int ResetSpeed = Start + 3 * SeparatorAt + 110;
            public const int ResetAll = Start + 3 * SeparatorAt + 120;

            public const int Tile = Start + 300;
            public const int FindSourceAsset = Start + 400;
        }

        public static class MarkerAction
        {
            public const int Start = ClipAction.Start + SeparatorAt;
        }

        public static class CustomTrackAction
        {
            public const int Start = MarkerAction.Start + SeparatorAt;

            public const int AnimConvertToClipMode = Start + 100;
            public const int AnimConvertFromClipMode = Start + 200;
            public const int AnimApplyTrackOffset = Start + 300;
            public const int AnimApplySceneOffset = Start + 310;
            public const int AnimApplyAutoOffset = Start + 320;
            public const int AnimAddOverrideTrack = Start + 500;
        }

        public static class CustomClipAction
        {
            public const int Start = CustomTrackAction.Start + SeparatorAt;
            public const int AnimClipMatchPrevious = Start + 100;
            public const int AnimClipMatchNext = Start + 110;
            public const int AnimClipResetOffset = Start + 120;
        }


        public const int AddGroupItemStart = DefaultPriority + SeparatorAt;
        public const int AddTrackItemStart = AddGroupItemStart + SeparatorAt;
        public const int AddCustomTrackItemStart = AddTrackItemStart + SeparatorAt;
        public const int AddClipItemStart = AddCustomTrackItemStart + SeparatorAt;
        public const int AddCustomClipItemStart = AddClipItemStart + SeparatorAt;
        public const int AddMarkerItemStart = AddCustomClipItemStart + SeparatorAt;
        public const int AddCustomMarkerItemStart = AddMarkerItemStart + SeparatorAt;
    }
}
