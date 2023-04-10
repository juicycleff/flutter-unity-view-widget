# Unitypackages
Documentation of the unitypackage for `flutter_unity_widget`.

These packages are generated from the `FlutterUnityIntegration`folder in the example project.
Using the files from the example project ensures you have the latest version.


### Which one do I pick?
Try the newest one first.

Package versions do not indicate supported unity versions.
For example `fuw-2022.1.7f1.unitypackage` will work in Unity 2021 and even Unity 2019.

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
