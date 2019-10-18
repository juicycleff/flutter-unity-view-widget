# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.0-preview.3] - 2019-09-26
### Improvements
- Build compiled binaries with Xcode 10.3 (10G8) and Xcode 11 (11A420a)

## [3.0.0-preview.2] - 2019-09-05
### Improvements
- Update [ARSubsystems](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@3.0) dependency to 3.0.0-preview.2

## [3.0.0-preview.1] - 2019-08-27
### New
- This package now supports bulding with Xcode 10, and 11 beta 7.

## [1.1.0-preview.4]
### New
- Add support for eye tracking.

### Fixes
- Recompile static library with XCode 11 beta 5.

## [1.1.0-preview.3] - 2019-07-18
### Fixes
- Recompile static library with Xcode 11 beta 4.

## [1.1.0-preview.2] - 2019-07-15
- Recompile static library with Xcode 11 beta 3.
- Update Unity dependency to 2019.1

## [1.1.0-preview.1] - 2019-06-05
- Adding support for ARKit 3 functionality: multiple face tracking and tracking a face (with front camera) while in world tracking (with rear camera).

## [1.0.0-preview.6] - 2019-05-31
### Fixes
- Fix documentation links.

## [1.0.0-preview.4] - 2019-05-06

### This is the first release of *ARKit Face Tracking*.

Provides runtime support for Face Tracking on ARKit. This is a separate package from com.unity.xr.arkit due to security concerns: apps that contain certain face-tracking related symbols in their compiled binaries will fail App Store validation unless additional documentation explaning the uses of face tracking are documented. This allows face tracking support to be added separately.
