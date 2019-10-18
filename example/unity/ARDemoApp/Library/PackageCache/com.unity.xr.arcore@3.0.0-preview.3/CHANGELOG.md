# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.0-preview.3] - 2019-09-26
### New
- Google's AR Presto library is now distributed as a separate AAR so that it can be overriden.
- Added support for both linear and gamma color spaces.
- Added tracking input support for the [Input System](https://github.com/Unity-Technologies/InputSystem)

### Fixes
- Fixed image tracking when building with IL2CPP.

## [3.0.0-preview.2] - 2019-09-05
### Improvements
- Update [ARSubsystems](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0) dependency to 3.0.0-preview.2

## [3.0.0-preview.1] - 2019-08-21
### New
- Add support for [XR Management](https://docs.unity3d.com/Packages/com.unity.xr.management@3.0/manual/index.html).
- Add support for building image libraries on linux.
- Add support for runtime modifiable reference image libraries. This allows you to add new reference images for image tracking at runtime.

## [2.2.0-preview.2] - 2019-07-19
### Fixes
- Previously, we reported the tracking state of augmented images using an [ARCore API](https://developers.google.com/ar/reference/c/group/trackable#group__trackable_1ga5759851a9b20b4df46d74d4ce4a1376c) that always returned `TrackingState.Tracking`, even when the image had been removed from view. We now use the method [ArAugmentedImage_getTrackingMethod](https://developers.google.com/ar/reference/c/group/augmented-image#group__augmented__image_1ga82c184a0984a95254476696cf57c12a3) instead, which will change the tracking state to `TrackingState.Limited` if the image cannot be seen.

## [2.2.0-preview.2] - 2019-08-09
### New
- Add support for Lightweight Render Pipeline and Universal Render Pipeline.

## [2.2.0-preview.1] - 2019-07-16
### New
- Update ARCore to version 1.10.
- Add support for getting the ambient light intensity in lumens.

## [2.1.0] - 2019-06-25
- 2019.3 verified release

## [2.1.0-preview.7] - 2019-06-18
### New
- Add support for `NotTrackingReason`.

## [2.1.0-preview.6] - 2019-06-03
### Improvements
- Update to ARCore 1.9.
- Add support for matching the ARCore framerate with the Unity one. See `ARSession.matchFrameRate`.

### Fixes
- Conditionally compile subsystem registrations. This means the subsystems wont't register themselves in the Editor (and won't generate warnings if there are other subsystems for other platforms).

## [2.1.0-preview.5] - 2019-05-27
### Fixes
- Enabling managed code stripping or IL2CPP would cause apps to fail to initialize ARCore on the first launch, just after the prompt for camera permissions. This has been fixed.
- Remove debug log from the image tracking subsystem.
- Fix documnentation links
- Reference image library generation would fail if the user's culture settings were not US-standard (e.g., using "," instead of "." for decimal values). This has been fixed.
- Fix `XRSessionSubsystem.Reset`. Previously, this did not properly resume a running session.
- The session subsystem reported that it did not support ARCore APK installation, which meant that consumers of this API (e.g., ARFoundation) would never prompt for installation and report the device as unsupported. This has been fixed.

## [2.1.0-preview.4] - 2019-05-08
### Improvements
- Add support for [image tracking](https://developers.google.com/ar/discover/concepts#augmented_images).
- Add support for [face tracking](https://developers.google.com/ar/develop/unity/augmented-faces/).
- Add support for [multithreaded rendering](https://unity3d.com/learn/tutorials/topics/best-practices/multithreaded-rendering-graphics-jobs#Multithreaded%20Rendering).

## [1.0.0-preview.24] - 2018-12-13
### Improvements
- Support x86, ARMv7, and ARM64 Android architetures (previously was limited to ARMv7).
- Plane detection modes: Add ability to selectively enable detection for horizontal, vertical, or both types of planes.
- Add a build check for the "Graphcis Jobs (Experimental)" player setting, which forces multithreaded rendering and causes ARCore to fail.
- Add a build check for the presence of Google's ARCore SDK for Unity, which cannot be used with this package.
- Add support for setting the camera focus mode.
- Add C header file necessary to interpret native pointers. See `Includes~/UnityXRNativePtrs.h`
- Implement `CameraConfiguration` support, allowing you to enumerate and set the resolution used by the hardware camera.
- Update to ARCore v1.6.0

### Fixes
- Updated background shader to workaround a bug which can cause green and blue color values to appear swapped on some devices when HDR is enabled.

### Changes
- Remove 2018.1 and 2018.2 compatibility.

## [1.0.0-preview.23] - 2018-10-07
### Changes
- Re-add `using` directive needed for 2018.1.

## [1.0.0-preview.22] - 2018-10-06
### Improvements
- Add linker validation when building with the IL2CPP scripting backend to avoid stripping the Unity.XR.ARCore assembly.
- Add native pointer support for native AR objects

## [1.0.0-preview.21] - 2018-10-12
### Fixed
- Fixed a bug which prevented the CameraImage API from working in 2018.3+

## [1.0.0-preview.20] - 2018-10-10
### New
- Added support for `XRCameraExtensions` API to get the raw camera image data on the CPU. See the [ARFoundation manual documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.0/manual/cpu-camera-image.html) for more information.

## [1.0.0-preview.19] - 2018-09-18
### Fixed
- Correctly set camera texture dimensions.
- The background texture was not rendered correctly if a renderable `GameObject` in the scene had negative scale. This has been fixed.
- Fixed issue [AR Camera does not work with video player on ARCore](https://issuetracker.unity3d.com/issues/ar-camera-does-not-work-with-video-player-on-arcore). The pass through video would conflict with the Video Player, producing a flickering effect. This has been fixed.

### Improvements
- Added a pre build step to ensure the Gradle build system is used.
- The build will fail if anything other than the OpenGLES3 graphics API is selected as the primary graphics API.

## [1.0.0-preview.18] - 2018-07-17
### Fixed
- Correctly detect whether the "ARCore Supported" checkbox is checked during player build in 2018.2+
- Updated for compatibility with Unity 2018.3 and later.
- Slinece unused variable warning.
- Plane tracking state would return a cached value if the session was not active. Now, `ARPlane.trackingState` will return `TrackingState.Unavailable` for every plane if the session is not active.

## [1.0.0-preview.17] - 2018-07-03
- Fail the build if x86 or ARM 64 are selected as Target Architectures in the Android Player Settings.

## [1.0.0-preview.16] - 2018-06-20
- Implemented TryGetColorCorrection, which provides light estimation information for RGB color correction.

## [1.0.0-preview.15] - 2018-06-08
- Fixed lack of reporting timestamp to the `ARCameraFrameEventArgs`.
- Do not include Android build pipeline when not on Android
- Add ArAnchor [attachment](https://developers.google.com/ar/develop/developer-guides/anchors) support.

## [1.0.0-preview.14] - 2018-06-07
- Fixed a crash on startup on some devices.
- Throw a build error instead of a warning if using Vulkan (ARCore requires an OpenGL context)
- Camera texture appears as soon as ARCore provides it, rather than waiting for tracking to be established.
- Fix typo in ARCoreSettings (`requirment` => `requirement`)
- Improve usability of ARCoreSettings
    - Remove CreateAssetMenu item -- provide one path to create the asset.
    - xmldoc referred to ARKit instead of ARCore
    - Make currentSettings public so users can override this easily.
- Improve ARCore build error message
    'Error building Player: BuildFailedException: "ARCore Supported" (Player Settings > XR Settings) refers to the built-in ARCore support in Unity and conflicts with the ARCore package.') that doesn't explain that that the "ARCore package" is in fact the "ARCore XR Plugin" package. The package name should match from the package manager window.

## [1.0.0-preview.13] - 2018-06-06
- Fixed a crash following ARCore apk install. There is a (rare) race condition when installing the ARCore apk, where ARCore will try to initialize before the apk is completely ready. This can still happen, but the app no longer crashes. When it does happen, the SDK will report that AR is supported and ready, but AR will not function properly until the app is restarted.

## [1.0.0-preview.12] - 2018-06-01
- Add ARCoreSettings to Player Settings menu. Allows you to select whether ARCore is 'optional' or 'required'.

## [1.0.0-preview.11] - 2018-05-24
- Add Editor as an include platform to ensure ARCore extensions work. This was preventing the availability check from running.
- Fix a bug which prevented the ARSession from restarting once destroyed.

## [1.0.0-preview.10] - 2018-05-23
- Change dependency to `ARExtensions` preview.2


## [1.0.0-preview.9] - 2018-05-09
### Fixed
- Fixed crash when ARCore is not present or otherwise unable to initialize.
- Add support for availability check and apk install

## [1.0.0-preview.8] - 2018-05-07

### Added
- Created a Legacy XRInput interface to automate the switch between 2018.1 and 2018.2 XRInput versions.

### Changed
- Only report display and projection matrices if we actually get them from ARCore.

## [1.0.0-preview.5] - 2018-03-26

### This is the first preview release of the ARCore package for multi-platform AR.

In this release we are shipping a working iteration of the ARCore package for
Unity's native multi-platform AR support.
Included in the package are dynamic libraries, configuration files, binaries
and project files needed to adapt ARCore to the Unity multi-platform AR API.
