# flutter_unity_widget
[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors-)

[![version][version-badge]][package]
[![MIT License][license-badge]][license]
[![PRs Welcome][prs-badge]](http://makeapullrequest.com)

[![Watch on GitHub][github-watch-badge]][github-watch]
[![Star on GitHub][github-star-badge]][github-star]

Flutter unity 3D widget for embedding unity in flutter. Add a Flutter widget to show unity. Works on Android and iOS.

## Installation
 First depend on the library by adding this to your packages `pubspec.yaml`:

```yaml
dependencies:
  flutter_unity_widget: ^0.1.5
```

Now inside your Dart code you can import it.

```dart
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
```
<br />

## Preview

30 fps gifs, showcasing communication between Flutter and Unity:

![gif](https://github.com/snowballdigital/flutter-unity-view-widget/blob/master/preview_android.gif?raw=true)
![gif](https://github.com/snowballdigital/flutter-unity-view-widget/blob/master/preview_ios.gif?raw=true)

<br />

## Setup Project
For this, there is also a video tutorial, which you can find a [here](https://www.youtube.com/watch?v=exNPmv_7--Q).

### Add Unity Project

1. Create an unity project, Example: 'UnityDemoApp'.
2. Create a folder named `unity` in flutter project folder.
2. Move unity project folder to `unity` folder.

Now your project files should look like this.

```
.
├── android
├── ios
├── lib
├── test
├── unity
│   └── <Your Unity Project>    // Example: UnityDemoApp
├── pubspec.yml
├── README.md
```

### Configure Player Settings

1. First Open Unity Project.

2. Click Menu: File => Build Settings

Be sure you have at least one scene added to your build.

3. => Player Settings

   **Android Platform**:
    1. Make sure your `Graphics APIs` are set to OpenGLES3 with a fallback to OpenGLES2 (no Vulkan)
   
    2. Change `Scripting Backend` to IL2CPP.

    3. Mark the following `Target Architectures` :
        - ARMv7        ✅
        - ARM64        ✅
        - x86          ✅

<img src="https://raw.githubusercontent.com/snowballdigital/flutter-unity-view-widget/master/Screenshot%202019-03-27%2007.31.55.png" width="400" />

   **iOS Platform**:
    1. This only works with Unity version >=2019.3 because uses Unity as a library!
    2. Other Settings find the Rendering part, uncheck the `Auto   Graphics API` and select only `OpenGLES3`.
    3. Depending on where you want to test or run your app, (simulator or physical device), you should select the appropriate SDK on `Target SDK`.
      <br />


      

<br />

### Add Unity Build Scripts and Export

Copy [`Build.cs`](https://github.com/snowballdigital/flutter-unity-view-widget/tree/master/scripts/Editor/Build.cs) and [`XCodePostBuild.cs`](https://github.com/snowballdigital/flutter-unity-view-widget/tree/master/scripts/Editor/XCodePostBuild.cs) to `unity/<Your Unity Project>/Assets/Scripts/Editor/`

Open your unity project in Unity Editor. Now you can export the Unity project with `Flutter/Export Android` (for Unity versions up to 2019.2), `Flutter/Export Android (Unity 2019.3.*)` (for Unity versions 2019.3 and up, which uses the new [Unity as a Library](https://blogs.unity3d.com/2019/06/17/add-features-powered-by-unity-to-native-mobile-apps/) export format), or `Flutter/Export IOS` menu.

<img src="https://github.com/snowballdigital/flutter-unity-view-widget/blob/master/Screenshot%202019-03-27%2008.13.08.png?raw=true" width="400" />

Android will export unity project to `android/UnityExport`.

IOS will export unity project to `ios/UnityExport`.

<br />

 **Android Platform Only**

  1. After exporting the unity game, open Android Studio and and add the `Unity Classes` Java `.jar` file as a module to the unity project. You just need to do this once if you are exporting from the same version of Unity everytime. The `.jar` file is located in the ```<Your Flutter Project>/android/UnityExport/lib``` folder
  2. If using Unity 2019.2 or older, open `build.gradle` of `UnityExport` module and replace the dependencies with
```gradle
    dependencies {
        implementation project(':unity-classes') // the unity classes module you added from step 1
    }
```
  3. If using Unity 2019.2 or older, open `build.gradle` of `UnityExport` module and remove these
```gradle
    bundle {
        language {
            enableSplit = false
        }
        density {
            enableSplit = false
        }
        abi {
            enableSplit = true
        }
    }
```

**iOS Platform Only**

  1. open your ios/Runner.xcworkspace (workspace!, not the project) in Xcode and add the exported project in the workspace root (with a right click in the Navigator, not on an item -> Add Files to “Runner” -> add the UnityExport/Unity-Iphone.xcodeproj file
  <img src="workspace.png" width="400" />
  2. Select the Unity-iPhone/Data folder and change the Target Membership for Data folder to UnityFramework
  <img src="change_target_membership_data_folder.png" width="400" />
  3. Add this to your Runner/Runner/Runner-Bridging-Header.h

```c
#import "UnityUtils.h"
```
  4. Add to AppDelegate.swift before the GeneratePluginRegistrant call:

```swift
InitArgs(CommandLine.argc, CommandLine.unsafeArgv)
```
  5. Opt-in to the embedded views preview by adding a boolean property to the app's `Info.plist` file with the key `io.flutter.embedded_views_preview` and the value `YES`.

<br />

### AR Foundation (ANDROID only at the moment)
If you want to use Unity for integrating Augmented Reality in your Flutter app, a few more changes are required:
  1. Export the Unity Project as previously stated (using the Editor Build script).
  2. Check if the exported project includes all required Unity libraries (.so) files (`lib/\<architecture\>/libUnityARCore.so` and `libarpresto_api.so`). There seems to be a bug where a Unity export does not include all lib files. If they are missing, use Unity to build a standalone .apk of your AR project, unzip the resulting apk, and copy over the missing .lib files to the `UnityExport` module.
  3. Similar to how you've created the `unity-classes` module in Android Studio, create similar modules for all exported .aar and .jar files in the `UnityExport/libs` folder (`arcore_client.aar`, `unityandroidpermissions.aar`, `UnityARCore.aar`).
  4. Update the build.gradle script of the `UnityExport` module to depend on the new modules (again, similar to how it depends on `unity-classes`).
  5. Finally, update your Dart code build method where you include the `UnityWidget` and add `isARScene: true,`.
  Sadly, this does have the side effect of making your Flutter activity act in full screen, as Unity requires control of your Activity for running in AR, and it makes several modifications to your Activity as a result (including setting it to full screen).

 
### Add UnityMessageManager Support

Copy [`UnityMessageManager.cs`](https://github.com/snowballdigital/flutter-unity-view-widget/blob/master/example/Unity/Assets/UnityMessageManager.cs) to your unity project.

Copy this folder [`JsonDotNet`](https://github.com/snowballdigital/flutter-unity-view-widget/tree/master/example/Unity/Assets/JsonDotNet) to your unity project.

Copy [`link.xml`](https://github.com/snowballdigital/flutter-unity-view-widget/blob/master/example/Unity/Assets/link.xml) to your unity project.

<br />

## Examples
### Simple Example

```dart
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

class UnityDemoScreen extends StatefulWidget {

  UnityDemoScreen({Key key}) : super(key: key);

  @override
  _UnityDemoScreenState createState() => _UnityDemoScreenState();
}

class _UnityDemoScreenState extends State<UnityDemoScreen>{
  static final GlobalKey<ScaffoldState> _scaffoldKey =
      GlobalKey<ScaffoldState>();
  UnityWidgetController _unityWidgetController;

  Widget build(BuildContext context) {

    return Scaffold(
      key: _scaffoldKey,
      body: SafeArea(
        bottom: false,
        child: WillPopScope(
          onWillPop: () {
            // Pop the category page if Android back button is pressed.
          },
          child: Container(
            color: colorYellow,
            child: UnityWidget(
              onUnityViewCreated: onUnityCreated,
            ),
          ),
        ),
      ),
    );
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    this._unityWidgetController = controller;
  }
}
```
<br />

### Communicating with and from Unity

```dart
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

class UnityDemoScreen extends StatefulWidget {

  UnityDemoScreen({Key key}) : super(key: key);

  @override
  _UnityDemoScreenState createState() => _UnityDemoScreenState();
}

class _UnityDemoScreenState extends State<UnityDemoScreen>{
  static final GlobalKey<ScaffoldState> _scaffoldKey =
      GlobalKey<ScaffoldState>();
  UnityWidgetController _unityWidgetController;
  bool paused = false;


  Widget build(BuildContext context) {

    return Scaffold(
      key: _scaffoldKey,
      body: Scaffold(
        key: _scaffoldKey,
        appBar: AppBar(
          title: const Text('Unity Flutter Demo'),
        ),
        body: Container(
            child: Stack(
          children: <Widget>[
            UnityWidget(
              onUnityViewCreated: onUnityCreated,
            ),
            Positioned(
              bottom: 40.0,
              left: 80.0,
              right: 80.0,
              child: MaterialButton(
                onPressed: () {

                  if(paused) {
                    _unityWidgetController.resume();
                    setState(() {
                      paused = false;
                    });
                  } else {
                    _unityWidgetController.pause();
                    setState(() {
                      paused = true;
                    });
                  }
                },
                color: Colors.blue[500],
                child: Text(paused ? 'Start Game' : 'Pause Game'),
              ),
            ),
          ],
        )),
      ),
    );
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    this._unityWidgetController = controller;
  }
}

```

## API
 - `pause()` (Use this to pause unity player)
 - `resume()` (Use this to resume unity player)

## Known issues
 - Android Export requires several manual changes
 - Using AR will make the activity run in full screen (hiding status and navigation bar).


[version-badge]: https://img.shields.io/pub/v/flutter_unity_widget.svg?style=flat-square
[package]: https://pub.dartlang.org/packages/flutter_unity_widget/
[license-badge]: https://img.shields.io/github/license/snowballdigital/flutter-unity-view-widget.svg?style=flat-square
[license]: https://github.com/snowballdigital/flutter-unity-view-widget/blob/master/LICENSE
[prs-badge]: https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square
[prs]: http://makeapullrequest.com
[github-watch-badge]: https://img.shields.io/github/watchers/snowballdigital/flutter-unity-view-widget.svg?style=social
[github-watch]: https://github.com/snowballdigital/flutter-unity-view-widget/watchers
[github-star-badge]: https://img.shields.io/github/stars/snowballdigital/flutter-unity-view-widget.svg?style=social
[github-star]: https://github.com/snowballdigital/flutter-unity-view-widget/stargazers

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="http://rexraphael.com"><img src="https://avatars2.githubusercontent.com/u/11243590?v=4" width="100px;" alt="Rex Raphael"/><br /><sub><b>Rex Raphael</b></sub></a><br /><a href="https://github.com/snowballdigital/flutter-unity-view-widget/commits?author=juicycleff" title="Code">💻</a></td>
    <td align="center"><a href="https://stockxit.com"><img src="https://avatars1.githubusercontent.com/u/1475368?v=4" width="100px;" alt="Thomas"/><br /><sub><b>Thomas</b></sub></a><br /><a href="https://github.com/snowballdigital/flutter-unity-view-widget/commits?author=thomas-stockx" title="Code">💻</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!