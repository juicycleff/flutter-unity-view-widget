# About ARKit Face Tracking

This package provides face tracking support to augment the [ARKit XR Plugin](https://docs.unity3d.com/Packages/com.unity.xr.arkit@3.0).

# Installing ARKit Face Tracking

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

# Using ARKit Face Tracking

This package implements the face tracking subsystem defined in the [AR Subsystems](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0) package. Refer to that package's documentation for instructions on how to use basic face tracking.

This package also provides additional, ARkit-specific face tracking functionality. ARKit provides a series of "blendshapes" to describe different features of a face. Each blendshape is modulated from 0..1. For example, there is a blendshape location describing how closed the mouth is.

## Front Facing Camera

Face tracking requires the use of the front-facing (selfie) camera. When the front-facing camera is active, other tracking subsystems (e.g., plane tracking, image tracking) will not be available. Likewise, if the rear-facing camera is active, face tracking will not be available. If you `Start` other, conflicting subsystems (subsystems that require the rear-facing camera), the ARKit package may decide to use the rear-facing camera instead. Therefore, to use face tracking on ARKit, you should `Stop` any other subsystems which may require the rear-facing camera. These include

* [Plane Tracking](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0/manual/plane-subsystem.html)
* [Image Tracking](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0/manual/image-tracking.html)
* [Object Tracking](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0/manual/object-tracking.html)


# Technical details
## Requirements
This version of ARKit Face Tracking is compatible with the following versions of the Unity Editor:

* 2019.2 and later (recommended)

To use this package, you must have:

- An iOS device with front-facing TrueDepth camera:
  - iPhone X
  - iPhone XS
  - iPhone XS Max
  - iPhone XR
  - iPad Pro (11-inch)
  - iPad Pro (12.9-inch, 3rd generation)
- iOS 11.0 or later
- Xcode 10.0 or later

## Known limitations

None

## Package contents

This version of *ARKit Face Tracking* includes

* A static library which provides an implementation of the [`XRFaceSubsystem`](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0/manual/face-tracking.html).
