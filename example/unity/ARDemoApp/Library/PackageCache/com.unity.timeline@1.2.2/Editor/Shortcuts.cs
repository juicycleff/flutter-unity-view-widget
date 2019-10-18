using System;
using JetBrains.Annotations;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace UnityEditor.Timeline
{
    static class Shortcuts
    {
        public static class Clip
        {
            public const string split = "Timeline/Editing/Split";
            public const string trimStart = "Timeline/Editing/TrimStart";
            public const string trimEnd = "Timeline/Editing/TrimEnd";

            [UsedImplicitly, ShortcutManagement.Shortcut(split, typeof(TimelineWindow), KeyCode.S)]
            static void Split(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(split, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(trimStart, typeof(TimelineWindow), KeyCode.I)]
            static void TrimStart(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(trimStart, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(trimEnd, typeof(TimelineWindow), KeyCode.O)]
            static void TrimEnd(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(trimEnd, args.context);
            }
        }

        public static class Timeline
        {
            public const string play = "Timeline/Play";
            public const string previousFrame = "Timeline/PrevFrame";
            public const string nextFrame = "Timeline/NextFrame";
            public const string frameAll = "Timeline/FrameAll";
            public const string previousKey = "Timeline/PrevKey";
            public const string nextKey = "Timeline/NextKey";
            public const string goToStart = "Timeline/GotoStart";
            public const string goToEnd = "Timeline/GotoEnd";
            public const string zoomIn = "Timeline/ZoomIn";
            public const string zoomOut = "Timeline/ZoomOut";
            public const string collapseGroup = "Timeline/CollapseGroup";
            public const string unCollapseGroup = "Timeline/UnCollapseGroup";
            public const string selectLeftItem = "Timeline/SelectLeftItem";
            public const string selectRightItem = "Timeline/SelectRightItem";
            public const string selectUpItem = "Timeline/SelectUpItem";
            public const string selectUpTrack = "Timeline/SelectUpTrack";
            public const string selectDownItem = "Timeline/SelectDownItem";
            public const string selectDownTrack = "Timeline/SelectDownTrack";
            public const string multiSelectLeft = "Timeline/SelectLeft";
            public const string multiSelectRight = "Timeline/SelectRight";
            public const string multiSelectUp = "Timeline/SelectUp";
            public const string multiSelectDown = "Timeline/SelectDown";
            public const string toggleClipTrackArea = "Timeline/ToggleClipTrackArea";
            public const string matchContent = "Timeline/MatchContent";
            public const string toggleLock = "Timeline/ToggleLock";
            public const string toggleMute = "Timeline/ToggleMute";

            public const string moveLeft = "Timeline/MoveLeft";
            public const string moveRight = "Timeline/MoveRight";
            public const string moveUp = "Timeline/MoveUp";
            public const string moveDown = "Timeline/MoveDown";

            [UsedImplicitly, ShortcutManagement.Shortcut(play, typeof(TimelineWindow), KeyCode.Space)]
            static void Play(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(play, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(previousFrame, typeof(TimelineWindow), KeyCode.Comma)]
            static void PreviousFrame(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(previousFrame, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(nextFrame, typeof(TimelineWindow), KeyCode.Period)]
            static void NextFrame(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(nextFrame, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(frameAll, typeof(TimelineWindow), KeyCode.A)]
            static void FrameAll(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(frameAll, args.context);
            }

            #if UNITY_EDITOR_OSX
            [UsedImplicitly, ShortcutManagement.Shortcut(previousKey, typeof(TimelineWindow), KeyCode.Comma, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
            #else
            [UsedImplicitly, ShortcutManagement.Shortcut(previousKey, typeof(TimelineWindow), KeyCode.Comma, ShortcutModifiers.Action)]
            #endif
            static void PrevKey(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(previousKey, args.context);
            }

            #if UNITY_EDITOR_OSX
            [UsedImplicitly, ShortcutManagement.Shortcut(nextKey, typeof(TimelineWindow), KeyCode.Period, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
            #else
            [UsedImplicitly, ShortcutManagement.Shortcut(nextKey, typeof(TimelineWindow), KeyCode.Period, ShortcutModifiers.Action)]
            #endif
            static void NextKey(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(nextKey, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(goToStart, typeof(TimelineWindow), KeyCode.Comma, ShortcutModifiers.Shift)]
            static void GoToStart(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(goToStart, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(goToEnd, typeof(TimelineWindow), KeyCode.Period, ShortcutModifiers.Shift)]
            static void GoToEnd(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(goToEnd, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(zoomIn, typeof(TimelineWindow), KeyCode.Equals)]
            static void ZoomIn(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(zoomIn, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(zoomOut, typeof(TimelineWindow), KeyCode.Minus)]
            static void ZoomOut(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(zoomOut, args.context);
            }

            [UsedImplicitly]
            [ShortcutManagement.Shortcut(moveLeft, typeof(TimelineWindow), KeyCode.LeftArrow)]
            static void SelectLeft(ShortcutManagement.ShortcutArguments args)
            {
                if (KeyboardNavigation.ClipAreaActive())
                {
                    SendEventToInvokeShortcut(selectLeftItem, args.context);
                }
                else if (KeyboardNavigation.TrackHeadActive())
                {
                    SendEventToInvokeShortcut(collapseGroup, args.context);
                }
            }

            [UsedImplicitly]
            [ShortcutManagement.Shortcut(moveRight, typeof(TimelineWindow), KeyCode.RightArrow)]
            static void SelectRight(ShortcutManagement.ShortcutArguments args)
            {
                if (KeyboardNavigation.ClipAreaActive())
                {
                    SendEventToInvokeShortcut(selectRightItem, args.context);
                }
                else if (KeyboardNavigation.TrackHeadActive())
                {
                    SendEventToInvokeShortcut(unCollapseGroup, args.context);
                }
            }

            [UsedImplicitly]
            [ShortcutManagement.Shortcut(moveUp, typeof(TimelineWindow), KeyCode.UpArrow)]
            static void SelectUp(ShortcutManagement.ShortcutArguments args)
            {
                if (KeyboardNavigation.ClipAreaActive())
                {
                    SendEventToInvokeShortcut(selectUpItem, args.context);
                }
                else if (KeyboardNavigation.TrackHeadActive())
                {
                    SendEventToInvokeShortcut(selectUpTrack, args.context);
                }
                else
                {
                    KeyboardNavigation.FocusFirstVisibleItem(GetState(args));
                }
            }

            [UsedImplicitly]
            [ShortcutManagement.Shortcut(moveDown, typeof(TimelineWindow), KeyCode.DownArrow)]
            static void SelectDown(ShortcutManagement.ShortcutArguments args)
            {
                if (KeyboardNavigation.ClipAreaActive())
                {
                    SendEventToInvokeShortcut(selectDownItem, args.context);
                }
                else if (KeyboardNavigation.TrackHeadActive())
                {
                    SendEventToInvokeShortcut(selectDownTrack, args.context);
                }
                else
                {
                    KeyboardNavigation.FocusFirstVisibleItem(GetState(args));
                }
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(multiSelectLeft, typeof(TimelineWindow), KeyCode.LeftArrow, ShortcutModifiers.Shift)]
            static void MultiSelectLeft(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(multiSelectLeft, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(multiSelectRight, typeof(TimelineWindow), KeyCode.RightArrow, ShortcutModifiers.Shift)]
            static void MultiSelectRight(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(multiSelectRight, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(multiSelectUp , typeof(TimelineWindow), KeyCode.UpArrow, ShortcutModifiers.Shift)]
            static void MultiSelectUp(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(multiSelectUp, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(multiSelectDown, typeof(TimelineWindow), KeyCode.DownArrow, ShortcutModifiers.Shift)]
            static void MultiSelectDown(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(multiSelectDown, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(toggleClipTrackArea, typeof(TimelineWindow), KeyCode.Tab)]
            static void ToggleClipTrackArea(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(toggleClipTrackArea, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(matchContent, typeof(TimelineWindow), KeyCode.C)]
            static void Shortcut(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(matchContent, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(toggleLock, typeof(TimelineWindow), KeyCode.L)]
            static void Lock(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(toggleLock, args.context);
            }

            [UsedImplicitly, ShortcutManagement.Shortcut(toggleMute, typeof(TimelineWindow), KeyCode.M)]
            static void Mute(ShortcutManagement.ShortcutArguments args)
            {
                SendEventToInvokeShortcut(toggleMute, args.context);
            }
        }

        static WindowState GetState(ShortcutManagement.ShortcutArguments args)
        {
            return ((TimelineWindow)args.context).state;
        }

        static void SendEventToInvokeShortcut(string timelineShortcutId, object context)
        {
            var e = new Event
            {
                type = EventType.ExecuteCommand,
                commandName = timelineShortcutId
            };

            (context as EditorWindow).SendEvent(e);
        }
    }
}
