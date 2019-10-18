# Changelog

## [1.0.4] - 2019-05-22

### Changed
- Removed prefab resources from Runtime tests and made them generate through code (fixes warnings in Resource.LoadAll)
- Fix for too long test names which affected some build flows

## [1.0.3] - 2019-05-21

### Changed
- Fixes in test setup phase to make execution faster
- Fixed typo for NetworkTransformChild.

## [1.0.2] - 2019-03-18

### Changed
- Fixed issue with population of Syncvar variable on a class derived from a networkbehaviour base class (case 1066429)
- Fixed issue with IsDynamic not working on .net 3.5 profile (use something else)]
- Fixed file lock error when building under certain conditions (case 1115492)
 
## [1.0.1] - 2019-02-14

### Changed
- Disabled warnings around the usage of the 'new' keyword in the NetworkTransform, it's needed sometimes but can trigger when building (likely because of stripping) and the warning messed with CI/automation

## [1.0.0] - 2019-02-13

### Changed
- Only updating version to 1.0.0 to reflect the status of the package

## [0.2.6-preview] - 2019-02-13

### Changed
- Got rid of all warnings generated when the package is built, so it's CI/automation friendly
- Readme updated

## [0.2.5-preview] - 2019-01-29

### Changed
- Fixed Syncvar variable update issue. Modify both the writing and reading syncvar data as per channel. (Fixed cases 1110031, 1111442 and 1117258)

## [0.2.4-preview] - 2019-01-11

### Changed
- Fixed issue with assembly dependencies not being found by the weaver during certain conditions (like when doing full reimport).

### Added
- Added API documentation, migrated from unity source repo, only some formatting changes, text itself unchanged.

## [0.2.3-preview] - 2018-12-17

### This is the first release of the *Unity Multiplayer HLAPI \<com.unity.multiplayer-hlapi\>*.

Initial release of the Unity Multiplayer HLAPI (or UNet HLAPI) moved into a package. This will
work with Unity 2019.1.0a12 and onwards.

This was previously an extension DLL but the layout has been moved from the extension style to a package format. Also all
parts which existed in the Unity engine (native code) have been moved out and utilize public API instead. Mostly
this involved
- Update bump is now created with a hidden game object, instead of registering in the player loop internally
- Domain reloads are detected with public APIs
- Weaver invocation (used for code generation) registers with the compiliation pipeline API and receives callbacks
  every time compilation finishes so it can parse the DLLs.
- Profiler panel functionality was made possible by making some internal APIs puplic for now.

Also, some runtime tests have been moved to the package as playmode tests.