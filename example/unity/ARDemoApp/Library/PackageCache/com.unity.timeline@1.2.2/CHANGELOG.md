# Changelog
All notable changes to this package will be documented in this file. The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)

## [1.2.2] - 2019-08-20

## Fixed
- Fixed issue where fields for custom clips were not responding to Add Key commands. (1174416)
- Fixed issue where a different track's bound GameObject is highlighted when clicking a track's bound GameObject box. (1141836)
- Fixed issue where a clip locks to the playhead's position when moving it. (1157280)

## [1.2.1] - 2019-08-01

## Fixed
- Fixed appearance of a selected clip's border. 
- Fixed non-transform properties from AnimationClips not being correctly put into preview mode when the avatar root does not contain the animator component. ([1162334](https://issuetracker.unity3d.com/product/unity/issues/guid/1162334/))
- Fixed an issue where the context menu for inline curves keys would not open on MacOS. ([1158584](https://issuetracker.unity3d.com/product/unity/issues/guid/1158584/))
- Fixed recording state being incorrect after toggling preview mode (1146551)
- Fixed copying clips without ExposedReferences causing the scene to dirty (1144469)

## [1.2.0] - 2019-07-16
*Compatible with Unity 2019.3*

### Added
- Added ILayerable interface. Implementing this interface on a custom track will enable support for multiple layers, similar to the AnimationTracks override tracks.
- Added "Pan" autoscrolling option in the Timeline window.
- Enabled rectangle tool for inline curves.

### Changed
- Scrolling horizontally with the mouse wheel or trackpad now pans the timeline view horizontally, instead of zooming.
- Scrolling vertically with the mouse wheel or trackpad on the track headers or on the vertical scroll bar now pans the timeline view vertically, instead of zooming.

### Fixed
- Fixed an issue causing info text to overlap when displaying multiple lines (1150863).
- Fixed duration mode not reverting from "Fixed Length" to "Based On Clips" properly. (1154034)
- Fixed playrange markers being drawn over horizontal scrollbar (1156023)
- Fixed an issue where a hotkey does not autofit all when Marker is present (1158704)
- Fixed an issue where an exception was thrown when overwriting a Signal Asset through the Signal Emitter inspector. (1152202)
- Fixed Control Tracks not updating instances when source prefab change. (case 1158592)
- An exception will be thrown when calling TrackAsset.CreateMarker() with a marker that implements INotification if the track does not support notifications. (1150248)
- Fixed preview mode being reenabled when warnings change on tracks. (case 1151381)
- Fixed minimum clip duration to be frame aligned. (case 1156602)
- Fixed playhead being moved when applying undo while recording.(case 1154802)
- Fixed warnings about localEulerAnglesRaw when using RectTransform. (case 1151100)
- Fixed precision error on the duration of infinite tracks. (case 1156723)
- Fixed issue where two GatherProperties call were made when switching between two PlayableDirectors. (1159036)
- Fixed issue where inspectors for clips, tracks and markers would get incorrectly displayed when no Timeline Window is opened. (1158242, 1158283)
- Fixed issue with clip connectors that were incorrectly drawn when the timeline was panned or zoomed. (1141960)
- Fixed issue where evaluating a Playable Graph inside a Notification Receiver would cause an infinite recursion. ([1149930](https://issuetracker.unity3d.com/product/unity/issues/guid/1149930/))
- Fixed Trim and Move operations to ensure playable duration is updated upon completion. ([1151894](https://issuetracker.unity3d.com/product/unity/issues/guid/1151894/))
- Fixed options menu icon that was blurry on high-dpi screens. (1154623)
- Track binding field is now larger. (1153446)
- Fixed issue where an empty Timeline window would create new objects on each repaint. (1142894)
- Fixed an issue causing info text to overlap when displaying multiple lines (when trimming + time scaling, for example). (1150863)
- Fixed duration mode not reverting from "Fixed Length" to "Based On Clips" properly. ([1154034](https://issuetracker.unity3d.com/product/unity/issues/guid/1154034/))
- Prevented the PlayableGraph from being created twice when playing a timeline in play mode with the Timeline window opened. (1147247)
- Fixed issue where an exception was thrown when clicking on a SignalEmitter with the Timeline window in asset mode. (1146261)
- A timeline will now be played correctly when building a player with Mono and Managed Stripping Level set higher than Low. ([1133182](https://issuetracker.unity3d.com/product/unity/issues/guid/1133182/))
- The Signal Asset creation dialog will no longer throw exceptions when canceled on macOS. ([1141959](https://issuetracker.unity3d.com/product/unity/issues/guid/1141959/))
- Fixed issue where the Emit Signal property on a Signal Emitter would not get saved correctly. ([1148709](https://issuetracker.unity3d.com/product/unity/issues/guid/1148709/))
- Fixed issue where a Signal Emitter placed at the start of a timeline would be fired twice. ([1149653](https://issuetracker.unity3d.com/product/unity/issues/guid/1149653/))
- Fixed record button state not updating when offset modes are changed. ([1142747](https://issuetracker.unity3d.com/product/unity/issues/guid/1142747/))
- Cleared invalid assets from the Timeline Clipboard when going into or out of PlayMode. (1144473)
- Copying a Control Clip during play mode no longer throws exceptions. (1141581)
- Going to Play Mode while inspecting a Track Asset will no longer throw exceptions. (1141958)
- Resizing Timeline's window no longer affects the zoom value. ([1147150](https://issuetracker.unity3d.com/product/unity/issues/guid/1147150/))
- Snap relaxing now responds to Command on Mac, instead of Control. (1149144)
- Clips will no longer randomly disappear when showing or hiding inline curves. (1141661)
- The global/local time referential button will no longer be shown for a top-level timeline. (1080872)
- Playhead will not be drawn above the bottom scrollbar anymore. (1134016)
- Fixed moving a marker on an Infinite Track will keep the track in infinite mode (1141190)
- Fixed zooming in/out will keep the padding at the beginning of the timeline (1030689)
- Fixed marker UI is the same color and size on infinite track (1139370)
- Fixed Disable the possibility to add Markers to tracks of a Timeline that is ReadOnly (1134463)
- Fixed wrong context menu being shown when right-clicking a marker (1133592)
- Fixed creation of override track to work with multiselection (1133592)

## [1.1.0] - 2019-02-14
*Compatible with Unity 2019.2*
### Added
- ClipEditor, TrackEditor and MarkerEditor classes users can derive from to control visual appearance of custom timeline clips, tracks and markers using the CustomTimelineEditor attribute.
- ClipEditor.GetSubTimelines to allow user created clips that support sub-timelines in editor
- TimelineEditor.selectedClip and TimelineEditor.selectedClips to set and retrieve the currently selected timeline clips
- IPropertyCollector.AddFromName override that takes a component.
- Warning icons to SignalEmitters when they do not reference an asset
- Ability to mute/unmute a Group Track.
- Mute/Unmute only selected track command added for tracks with multiple layers.
- Animate-able Properties on Tracks and Clips can now be edited through inline curves.
- Added loop override on AnimationTrack clips (1140766)
- ReadOnly/Source Control Lock support for Timeline Scene

### Changed
- Control Track display to show a particle system icon when particle systems are being controlled
- Animate-able Properties for clips are no longer edited using by "recording"; they are edited through the inline curves just like tracks.
- AudioTrack properties can now be animated through inline curves.
- Changed Marker show/hide to be undoable. Hide will also unselect markers. (1124661)
- Changed SignalReceivers show their enabled state in the inspector. (1131163)
- Changed Track Context Menu to show "Add Signal Emitter" at the top of the list of Marker commands. (1131166)
- Moved "Add Signal Emitter" and "Add Signal Emitter From Asset" commands out of their sub-menu. (1131166)

### Fixed
- Fixed markers being drawn outside their pane. (1124381)
- Fixed non-public tracks not being recognized by the Timeline Editor. (1122803)
- Fixed keyboard shortcuts for _Frame All_ (default: A) and _Frame Selected_ (default: F) to also apply horizontally ([1126623](https://issuetracker.unity3d.com/product/unity/issues/guid/1126623/))
- Fixed recording getting disabled when selecting a different GameObject while the Timeline Window is not locked. (1123119)
- Fixed time sync between Animation and Timeline windows when clips have non-default timescale or clip-in values. ([930909](https://issuetracker.unity3d.com/product/unity/issues/guid/930909/))
- Fixed animation window link not releasing when deleting the timeline asset. (1127425)
- Fixed an exception being raised when selecting both a Track marker and a Timeline marker at the same time. ([1113006](https://issuetracker.unity3d.com/product/unity/issues/guid/1113006/))
- Fixed the header marker area will so it no longer opens its context menu if it's hidden. (1124351)
- Fixed Signal emitters to show the Signals list when created on override tracks. (1102913)
- Fixed a crash on IL2CPP platforms when the VideoPlayer component is not used. (1129572)
- Fixed Timeline Duration changes in editor not being undoable. (1109279)
- Fixed _Match Offsets_ commands causing improper animation defaults to be applied. (911678)
- Fixed Timeline Inspectors leaving _EditorGUI.showMixedValue_ in the wrong state. ([1123895](https://issuetracker.unity3d.com/product/unity/issues/guid/1123895/))
- Fixed issue where performing undo after moving items on multiple tracks would not undo some items. (1131071)
- Fixed cog icon in the Signal Receiver inspector being blurry. (1130320)
- Fixed Timeline marker track hamburger icon not being centered vertically. (1131112)
- Fixed detection of signal receivers when track is in a group. (1131811)
- Fixed exception being thrown when deleting Signal entries. (1131065)
- Fixed Markers blocking against Clips when moving both Clips and Markers in Ripple mode. (1102594)
- Fixed NullReferenceException being thrown when muting an empty marker track. (1131106)
- Fixed SignalEmitter Inspector losing the Receiver UI when it is locked and another object is selected. (1116041)
- Fixed Marker and Clip appearing to be allowed to move to another track in Ripple mode. (1131123)
- Fixed issue where the Signal Emitter inspector did not show the Signal Receiver UI when placed on the timeline marker track. (1131811)
- Fixed Replace mode not drawing clips when moved together with a Marker. (1132605)
- Fixed inline curves to retain their state when performing undo/redo or keying from the inspector. ([1125443](https://issuetracker.unity3d.com/product/unity/issues/guid/1125443))
- Fixed an issue preventing Timeline from entering preview mode when an Audio Track is present an a full assembly reload is performed. (1132243)
- Fixed an issue where the Marker context menu would show a superfluous line at the bottom. (1132662)
- Fixed an issue preventing Timeline asset to be removed from a locked Timeline Window when a new scene is loaded. (1135073)
- Fixed EaseIn/Out shortcut for clips

## [1.0.0] - 2019-01-28
*Compatible with Unity 2019.1*
### Added
- This is the first release of Timeline, as a Package
- Added API calls to access all AnimationClips used by Timeline.
- Added support in the runtime API to Animate Properties used by template-style PlayableBehaviours used as Mixers.
- Added Markers. Markers are abstract types that represent a single point in time.
- Added Signal Emitters and Signal Assets. Signal Emitters are markers that send a notification, indicated by a SignalAsset, to a GameObject indicating an event has occurred during playback of the Timeline.
- Added Signal Receiver Components. Signal Receivers are MonoBehaviour that listen for Signals from Timeline and respond by invoking UnityEvents.
- Added Signal Tracks. Signal Tracks are Timeline Tracks that are used only for Signal Emitters.

### Fixed
- Signal Receiver will no longer throw exceptions when its inspector is locked ([1114526](https://issuetracker.unity3d.com/product/unity/issues/guid/1114526/))
- Context menu operations will now be applied on all selected tracks (1089820)
- Clip edit mode clutch keys will not get stuck when holding multiple keys at the same time (1097216)
- Marker inspector will be disabled when the marker is collapsed (1102860)
- Clip inspector will no longer throw exceptions when changing values when the inspector is locked (1115984)
- Fixed appearance of muted tracks (1018643)
- Fixed multiple issues where clips and markers were selectable when located under the time ruler and the marker header track ([1117925](https://issuetracker.unity3d.com/product/unity/issues/guid/1117925/), 1102598)
- A marker aligned with the edge of a clip is now easier to select (1102591)
- Changed behaviour of the Timeline Window to apply modifications immediately during Playmode ([922846](https://issuetracker.unity3d.com/product/unity/issues/guid/922846/), 1111908)
- PlayableDirector.played event is now called after entering or exiting Playmode ([1088918](https://issuetracker.unity3d.com/product/unity/issues/guid/1088918/))
- Undoing a paste track operation in a group will no longer corrupt the timeline (1116052)
- The correct context menu will now be displayed on the marker header track (1120857)
- Fixed an issue where a circular reference warning appeared in the Control Clip inspector even if there was no circular reference (1116520)
- Fixed preview mode when animation clips with root curves are used (case 1116297, case 1116007)
- Added option to disable foot IK on animation playable assets (case 1115652)
- Fixed unevaluated animation tracks causing default pose (case 1109118)
- Fixed drawing of Group Tracks when header is off-screen (case 876340)
- Fixed drag and drop of objects inside a group being inserted outside (case 1011381, case 1014774)
