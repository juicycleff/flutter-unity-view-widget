# Unitypackages
Documentation of the unitypackage for `flutter_unity_widget`.

These packages are generated from the `FlutterUnityIntegration`folder in the example project.
Using the files from the example project ensures you have the latest version.


### Which one do I pick?
Try the newest one first.

Most changes are backwards compatible and package versions do not indicate required unity versions.
For example `fuw-2022.1.7f1.unitypackage` will still work in Unity 2021 and even Unity 2019.
And the `6000.0.1` package is backwards compatible with Unity 2022.3 and earlier.


If you really can't get it to work try to match version numbers.
e.g if you use an older plugin version in the 4.x range, you might have to try unitypackages with 4.x versions.


### NewtonSoft errors
You might run into one of the following errors:

- `The type or namespace name 'Newtonsoft' could not be found`
 You need to add or enable the dll file.
- `Multiple precompiled assemblies`.
You need to remove or disable the dll file.

For Android, iOS and Web this file shoud be
`Assets\FlutterUnityIntegration\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll.txt`

Using a wrong extension like `.dll.txt` will disable it.

# CHANGELOG
Changes for `2022.1.7f1` and earlier were collected retroactively and might not be complete.

## Pending
> Example Unity project, not in a unitypackage yet.
* None

## 6000.0.2
> fuw-6000.0.2.unitypackage
* (Android) Fix generation of duplicate include entries in settings.gradle.kts.

## 2022.3.2
> fuw-2022.3.2.unitypackage
* This is a duplicate of 6000.0.2, for those overlooking the fact that it is backwards compatiple.

## 6000.0.1
> fuw-6000.0.0.unitypackage
* (Android) Handle missing `"game_view_content_description"` string in Unity output strings.xml.

## 6000.0.0
> fuw-6000.0.0.unitypackage
* Fix Android exports for Unity 6000.0 breaking changes.  
* Fix iOS exports for Unity 6000.0 breaking changes.   
**Note** 
* This package remains backwards compatible with previous Unity versions like 2022.3.
* Android exports with Unity 6 are not compatible with the Flutter plugin 2022.x or earlier.

## 2022.3.0
>fuw-2022.3.0.unitypackage
* Avoid invalid iOS export when current build target is not iOS. [#838](https://github.com/juicycleff/flutter-unity-view-widget/pull/838)
* (Android) Disable absolute ndk path from Unity export. (Unity 2022.3.x and newer) 
* (Android) Add missing namespace in unityLibrary build.gradle for Android Gradle plugin (AGP) 8.x.
* (Web) Fix Javascript error on Play and Pause.
* (Android) Fix build error `resource style/UnityThemeSelector not found` in the example project.
* Use Il2CppCodeGeneration.OptimizeSpeed in Android and iOS release exports.
* (Android) Handle new .gradle.kts files in Flutter 3.29+.


## 2022.2.0
>fuw-2022.2.0.unitypackage
* Restore newtonsoft.json.dll import. 
* Improve file appending during iOS export.
* Disable bitcode for Xcode 14
* (Android) Fix proguard linebreak bug
* Fix a debugger crash in Unity 2022
* Demo: Fix float parsing for localizations not using a dot as separator.

## 2022.1.7f1
>fuw-2022.1.7f1.unitypackage
* Add separate Debug and Release exports options.

## 2022.1.1-v2
>fuw-2022.1.1-v2.unitypackage
* Add missing using statements in `NativeAPI.cs`.
* Add success logs at the end of an export.
* Add Android proguard rule `-keep class com.unity3d.plugin.* { *; }";`
* Improve web exports.

## 2022.1.1
>fuw-2022.1.1.unitypackage
* Rename newtonsoft.json `.dll` to `.dll.txt` to avoid assembly errors.

## 2022.1.0
>fuw-2022.1.0.unitypackage
* iOS export fixes.
* WIP webGL export

## v4.1
 >FlutterUnityIntegration-v4.1.0.unitypackage
* Enable bitcode for iOS.
