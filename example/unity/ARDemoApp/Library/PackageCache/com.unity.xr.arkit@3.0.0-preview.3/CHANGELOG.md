# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.0-preview.3] - 2019-09-26
### New
- Build compiled binaries with Xcode 10.3 (10G8) and Xcode 11 (11A420a)
- Added support for both linear and gamma color spaces.
- Register AR tracking inputs with the new [Input System](https://github.com/Unity-Technologies/InputSystem)

### Fixes
- Exclude tvOS as a supported platform.
- The ["match frame rate"](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@3.0/api/UnityEngine.XR.ARFoundation.ARSession.html#UnityEngine_XR_ARFoundation_ARSession_matchFrameRate) option could incorrectly cause execution to be blocked while waiting for a new frame, leading to long frame times. This has been fixed.
- The ["match frame rate"](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@3.0/api/UnityEngine.XR.ARFoundation.ARSession.html#UnityEngine_XR_ARFoundation_ARSession_matchFrameRate) option did not account for thermal throttling, which can put ARKit into a 30 frames per second update mode while Unity would keep trying to update at 60 frames per second. This could lead to visual artifacts like judder. The calculated frame rate now takes the thermal state into account and will do a better job matching ARKit's update rate.

## [3.0.0-preview.2] - 2019-09-05
### New
- Added support for [ARCoachingOverlayView](https://developer.apple.com/documentation/arkit/arcoachingoverlayview)
- Added tracking input support for the [Input System](https://github.com/Unity-Technologies/InputSystem)

### Fixes
- 3.0.0-preview.1 was not compatible with some older versions of Xcode. This has been fixed.

### Breaking changes
- `ARKitSessionSubsystem.worldMapSupported` was previously an instance method; now it is a `static` method as it does not require an instance to perform this check.

## [3.0.0-preview.1] - 2019-08-27
### New
- Add support for [XR Management](https://docs.unity3d.com/Packages/com.unity.xr.management@3.0/manual/index.html).
- Add support for the [XRParticipantSubsystem](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0/manual/participant-subsystem.html), which can track other users in a multi-user collaborative session.
- Add support for [exposureDuration](https://developer.apple.com/documentation/arkit/arcamera/3182986-exposureduration?language=objc)
- Add support for [exposureOffset](https://developer.apple.com/documentation/arkit/arcamera/3194569-exposureoffset?language=objc)
- Add support for Lightweight Render Pipeline and Universal Render Pipeline.
- Add support for height scale estimatation for the 3D human body subsystem.
- This package now supports bulding with Xcode ~~9,~~ 10 and 11 beta 7.

### Fixes
- Enforce minimum target iOS version of 11.0 whenever ARKit is required.
- Setting the `ARHumanBodyManager.humanSegmentationDepthMode` value to either `HalfScreenResolution` or `FullScreenResolution` resulted in an invalid human segmentation depth image. This has been fixed.

## [2.2.0-preview.4] - 2019-07-29
### Fixes
- Update ARKit 3 compatibility for Xcode 11 beta 5.

## [2.2.0-preview.3] - 2019-07-18
### Fixes
- Update ARKit 3 compatibility for Xcode 11 beta 4.

## [2.2.0-preview.2] - 2019-07-16
### New
- Add support for `NotTrackingReason`.
- Add support for matching the ARCore framerate with the Unity one. See `XRSessionSubsystem.matchFrameRate`.
- Expose the [priority](https://docs.unity3d.com/Packages/com.unity.xr.arkit@2.2/api/UnityEngine.XR.ARKit.ARCollaborationData.html#UnityEngine_XR_ARKit_ARCollaborationData_priority) property on the `ARCollaborationData`.
- Add support for getting the ambient light intensity in lumens.

### Fixes
- Update ARKit 3 compatibility for Xcode 11 beta 3. This fixes
  - [Collaborative sessions](https://docs.unity3d.com/Packages/com.unity.xr.arkit@2.2/api/UnityEngine.XR.ARKit.ARCollaborationData.html)
  - Human body tracking

## [2.2.0-preview.1] - 2019-06-05
- Adding support for ARKit 3 functionality: Human pose estimation, human segmentation images, session collaboration, multiple face tracking, and tracking a face (with front camera) while in world tracking (with rear camera).

## [2.1.0-preview.6] - 2019-06-03
- Use relative paths for Xcode asset catalogs. This allows the generated Xcode project to be moved to a different directory, or even a different machine. Previously, we used full paths, which prevented this.
- Conditionally compile subsystem registrations. This means the subsystems wont't register themselves in the Editor (and won't generate warnings if there are other subsystems for other platforms).

## [2.1.0-preview.5] - 2019-05-21
### Fixes
- Fix documentation links
- Fix iOS version number parsing. This caused
  - Editor Play Mode exceptions (trying to parse a desktop OS string)
  - Incorrect handling of iOS point releases (e.g., 12.1.3)

## [2.1.0-preview.3] - 2019-05-14
### New
- Add [image tracking](https://developer.apple.com/documentation/arkit/recognizing_images_in_an_ar_experience) support.
- Add [environment probe](https://developer.apple.com/documentation/arkit/adding_realistic_reflections_to_an_ar_experience) support.
- Add [face tracking](https://developer.apple.com/documentation/arkit/creating_face-based_ar_experiences) support.
- Add [object tracking](https://developer.apple.com/documentation/arkit/scanning_and_detecting_3d_objects) support.

## [1.0.0-preview.23] - 2019-01-04
### Fixes
- Refactor the way ARKit face tracking is in the build. Face tracking has been moved to a separate static lib so that it can be removed from the build when face tracking is not enabled. This was preventing apps from passing App Store validation, as face tracking types may not appear in the binary unless you include a privacy policy describing to users how you intend to use face tracking and face data.

### New
- Support the `CameraIntrinsics` API in ARExtensions.

### Fixes
- Fixed linker errors when linking `UnityARKit.a` with Xcode 9.x

## [1.0.0-preview.20] - 2018-12-13

- Fix package dependency.

## [1.0.0-preview.19] - 2018-12-13
- Add C header file necessary to interpret native pointers. See `Includes~/UnityXRNativePtrs.h`
- Add support for setting the camera focus mode.
- Add a build check to ensure only ARM64 is selected as the only target architecture.
- Implement `CameraConfiguration` support, allowing you to enumerate and set the resolution used by the hardware camera.

## [1.0.0-preview.18] - 2018-11-21
### New
- Added ARKit Face Tracking support via `com.unity.xr.facesubsystem`.
- Plane detection modes: Add ability to selectively enable detection for horizontal, vertical, or both types of planes.

## [1.0.0-preview.17] - 2018-10-06
### Fixes
- Fixed an issue where toggling plane detection or light estimation would momentarily pause the ARSession, causing tracking to become temporarily unstable.
- Fixed the (new) CameraImage API to work with the 2018.3 betas.
- ARKit's `ARTrackingStateLimited` was reported as `TrackingState.Tracking`. It is now reported as `TrackingState.Unavailable`.

### Improvements
- Add support for native pointer access for several ARSession-related native objects.
- Add [ARWorldMap](https://developer.apple.com/documentation/arkit/arworldmap) support.
- Add linker validation when building with the IL2CPP scripting backend to avoid stripping the Unity.XR.ARKit assembly.

## [1.0.0-preview.16] - 2018-10-10
### New
- Added support for `XRCameraExtensions` API to get the raw camera image data on the CPU. See the [ARFoundation manual documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.0/manual/cpu-camera-image.html) for more information.

## [1.0.0-preview.15] - 2018-09-18
### Fixes
- Fix memory leak when destroying the ARSession.

## [1.0.0-preview.14] - 2018-08-10
- Add a pre build check to make sure Metal is the first selected Graphics API in Player Settings.
- Remove restriction on symlinking Unity libraries in Build Settings if using Unity 2018.3 or newer.
- Change plugin entry point in UnityARKit.a to avoid name collisions with other libraries (was `UnityPluginLoad`).

## [1.0.0-preview.13] - 2018-07-17
- Update plugin to be compatible with Unity 2018.3
- `ARPlane.trackingState` reports the session `TrackingState` for ARKit planes (previously it returned `TrackingState.Unknown`). ARKit planes do not have per-plane tracking states, so if they exist and the session is tracking, then the SDK will now report that the planes are tracked.

## [1.0.0-preview.12] - 2018-06-20
- Add -fembed-bitcode flag to UnityARKit.a to support archiving.
- Fail the build if "Symlink Unity libraries" is checked.

## [1.0.0-preview.11] - 2018-06-14
- Fail the build if Camera Usage Description is blank

## [1.0.0-preview.10] - 2018-06-08
- Do not include build postprocessor when not on iOS
- Add support for reference points attached to planes

## [1.0.0-preview.9] - 2018-06-06
- Remove extraneous debug log

## [1.0.0-preview.8] - 2018-05-07

### Added
-Created a Legacy XRInput interface to automate the switch between 2018.1 and 2018.2 XRInput versions.

## [1.0.0-preview.8] - 2018-05-24
### Added
- Availability check to determine runtime support for ARKit.
- Normalize average brightness reading from 0..1

## [1.0.0-preview.5] - 2018-03-26

### This is the first release of the ARKit package for multi-platform AR.

In this release we are shipping a working iteration of the ARKit package for
Unity's native multi-platform AR support.
Included in the package are static libraries, configuration files, binaries
and project files needed to adapt ARKit to the Unity multi-platform AR API.
