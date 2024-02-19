# flutter_unity_widget
[![All Contributors](https://img.shields.io/badge/all_contributors-4-orange.svg?style=flat-square)](#contributors-)

[![version][version-badge]][package]
[![MIT License][license-badge]][license]
[![PRs Welcome][prs-badge]](https://makeapullrequest.com)

[![Watch on GitHub][github-watch-badge]][github-watch]
[![Star on GitHub][github-star-badge]][github-star]

[![Gitter](https://badges.gitter.im/flutter-unity/community.svg)](https://gitter.im/flutter-unity/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Discord](https://img.shields.io/badge/Discord-blue?style=for-the-badge)](https://discord.gg/KmMqD7Sv3K)

Flutter unity 3D widget for embedding unity in flutter. Now you can make awesome gamified features of your app in Unity and get it rendered in a Flutter app both in fullscreen and embeddable mode. Works great on `Android, iPad OS, iOS, Web`.

Windows support is a work in progress.
<br />
### Notes
- Use Windows or Mac to export and build your project.  
  Users on Ubuntu have reported a lot of errors in the Unity export.
- Emulator support is limited and requires special setup. Please use a physical device for Android and iOS.
- Supports Unity 2019.4.3 or later, we recommend the latest LTS.
- Use only OpenGLES3 as Graphics API on Android for AR compatibility.


## Notice
Need me to respond, tag me [Rex Isaac Raphael](https://github.com/juicycleff). 

This plugin expects you to atleast know how to use Unity Engine. If you have issues with how unity widget is presented, you can please modify your unity project build settings as you seem fit.

Moving forward, versioning of the package will change to match unity releases after proper test. Mind you this does not mean the package
is not compatible with other versions, it just mean it's been tested to work with a unity version.

Windows coming soon.

## Installation

First depend on the library by adding this to your packages `pubspec.yaml`:
 -  Flutter 3.0.0
```yaml
dependencies:
  flutter_unity_widget: ^2022.2.0
```


 -  Pre Flutter 3.0.0 (This version will gradually be deprecated)
```yaml
dependencies:
  flutter_unity_widget: ^2022.1.0+7
```

Now inside your Dart code you can import it.

```dart
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
```

You will need to open and export a Unity project, even for running the example. Your build will fail if you only include the widget in Flutter!


## Preview

30 fps gifs, showcasing communication between Flutter and Unity:

![gif](https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/preview_android.gif?raw=true)
![gif](https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/preview_ios.gif?raw=true)

<br />

## Setup 

In the tutorial below, there are steps specific to each platform, denoted by a :information_source: icon followed by
the platform name (Android or iOS). You can click on its icon to expand it.

### Prerequisites

- An existing Flutter project (if there is none, you can [create a new one](https://flutter.dev/docs/get-started/test-drive#create-app))

- An existing Unity project (if there is none, you can [create a new one](https://learn.unity.com/tutorial/create-your-first-unity-project)).

- A `fuw-XXXX.unitypackage` file, found in the [*unitypackages*](https://github.com/juicycleff/flutter-unity-view-widget/tree/master/unitypackages) folder.
Try to use the most recent unitypackage available.


### Unity project setup
These instructions assume you are using a new Unity project. If you open the example project from this repository, you can move on to the next section **Unity Exporting**.

1. Create a folder named *unity* in your Flutter project folder and move the Unity project into there.  
  The Unity export will modify some files in  the `/android` and `/ios` folders of your flutter project. If your Unity project is in a different location the export might (partially) fail.

  > The expected path is *<flutter-project>/unity/__project-name__/...*


2. Make sure you have downloaded a *fuw-XXXX.unitypackage* file mentioned in <b>prerequisites</b>.

3. Using Unity (hub), open the Unity project.
  Go to **Assets > Import Package > Custom Package** and select the downloaded *fuw-XXXX.unitypackage* file. Click on **Import**.

4. Go to **File > Build Settings > Player Settings**
    and change the following under the **Other settings > Configuration** section:

  - In **Scripting Backend**, change to IL2CPP

  - (Android) **Target Architectures**, select ARMv7 and ARM64

  - (Android) For the best compatibility set **Active Input Handling** to `Input Manager (Old)` or `Both`.  
    (The new input system has some issues with touch input on Android)

  - (iOS) Select **Target SDK** depending on where you will run your app (simulator or physical device).  
    We recommend starting with a physical device and the `Device SDK` setting, due to limited simulator support.

  - (Web) Set <b>Publishing settings > Compression format</b> to Brotli or Disabled.  
  Some users report that Unity gets stuck on the loading screen with the Gzip setting, due to MIME type errors.

  <img src="https://raw.githubusercontent.com/juicycleff/flutter-unity-view-widget/master/files/Screenshot%202019-03-27%2007.31.55.png" width="400" />

5. In **File > Build Settings**, make sure to have at least 1 scene added to your build.


Some options in the **Build settings** window get overridden by the plugin's export script.
Attempting to change settings like `Development Build`, `Script Debugging` and `Export project` in this window will not make a difference.
If you end up having to change a build setting that doesn't seem to respond, take a lookat the export script `FlutterUnityIntegration\Editor\Build.cs`.

### Unity exporting

1. After importing the unitypackage, you should now see a **Flutter** option at the top of the Unity editor.

2. Click on **Flutter** and select the appropriate export option:

  - For android use **Export Android Debug** or **Export Android Release**.  
   This will export to *android/unityLibrary*.
  - For iOS use **Export iOS Debug** or **Export iOS Release**.  
   This will export to *ios/UnityLibrary*.
  - Do not use **Flutter > Export _Platform_ plugin** as it was specially added to work with [`flutter_unity_cli`](https://github.com/juicycleff/flutter_unity_cli) for larger projects.
 
  <img src="https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/Unity_Build_Options.png?raw=true" width="400" />
 
   If you use git, you will probably want to add these unityLibrary folders to your gitignore file.
   These folders can get huge and are not guaranteed to work on another computer.





  Proceed to the next section to handle iOS and Android specific setup after the export.

### Platform specific setup (after Unity export)
After exporting Unity, you will need to make some small changes in your iOS or Android project.  
You will likely need to do this **only once**. These changes remain on future Unity exports.

<details>
<summary>:information_source: <b>Android</b></summary>
  
1. Setting the Android NDK

  - If you have Unity and Flutter installed on the same machine, the easiest approach is to use the path of the NDK Unity uses. You can find the path to the NDK in Unity under `Edit -> Preferences -> External Tool`:

  ![NDK Path](files/ndkPath.png)

  - Copy the path and paste it into your flutter project at `android/local.properties` as `ndk.dir=`.  
  (For windows you will need to replace `\` with `\\`.)  
  Don't simply copy and paste this, make sure it the path matches your Unity version!  
```properties
    // mac
    ndk.dir=/Applications/Unity/Hub/Editor/2020.3.19f1/PlaybackEngines/AndroidPlayer/NDK
    // windows
    ndk.dir=C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.13f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\NDK
```


  - With the above setup, you shouldn't have to define any NDK version or setting in gradle files.  
   If you don't have Unity on the device making your Flutter buids, you can instead define it in `android/app/build.gradle`.
```gradle

android {

  ndkVersion "21.3.6528147"
}
```
  - To find the exact version that Unity uses, check `source.properties` at the NDK path described above.  


2. Depending on your gradle version, you might need to make sure the `minSdkVersion` set in `android\app\build.gradle` matches the version that is set in Unity.  
Check the **Minimum API Level** setting in the Unity player settings, and match that version.

3. (optional) Fixing Unity plugins.  
The Unity widget will function without this step, but some Unity plugins like ArFoundation will throw `mUnityPlayer` errors on newer Unity versions.  

    This is needed for Unity 2020.3.46+, 2021.3.19 - 2021.3.20 and 2022.2.4 - 2022.3.18.  
This requires a flutter_unity_widget version that is newer than 2022.2.1.  


- 3.1. Open the `android/app/build.gradle` file and add the following:

```diff
     dependencies {
+        implementation project(':flutter_unity_widget')
     }
```
- 3.2. Edit your android MainActivity file.  
The default location for Flutter is `android/app/src/main/kotlin/<app identifier>/MainActivity.kt`.

  If you use the default flutter activity, change it to inherit `FlutterUnityActivity`:
```diff
// MainActivity.kt

+ import com.xraph.plugin.flutter_unity_widget.FlutterUnityActivity;

+ class MainActivity: FlutterUnityActivity() {
- class MainActivity: FlutterActivity() {
```

- 3.2. (alternative) If you use a custom or modified Activity, implement the `IFlutterUnityActivity` interface instead.

```kotlin
// MainActivity.kt

// only do this if your activity does not inherit FlutterActivity

import com.xraph.plugin.flutter_unity_widget.IFlutterUnityActivity;

class MainActivity: CustomActivity(), IFlutterUnityActivity {
    // unity will try to read this mUnityPlayer property
    @JvmField 
    var mUnityPlayer: java.lang.Object? = null;

    // implement this function so the plugin can set mUnityPlayer
    override fun setUnityPlayer(unityPlayer: java.lang.Object?) {
        mUnityPlayer = unityPlayer;
    }
}
```


4. The Unity export script automatically sets the rest up for you. You are done with the Android setup.  
But if you want to manually set up the changes made by the export, continue.
  
<details> 
<summary> Optional manual Android setup </summary> 

5. Open the *android/settings.gradle* file and change the following:

```diff
+    include ":unityLibrary"
+    project(":unityLibrary").projectDir = file("./unityLibrary")
```

6. Open the *android/app/build.gradle* file and change the following:

```diff
     dependencies {
+        implementation project(':unityLibrary')
     }
```

7. open the *android/build.gradle* file and change the following:

```diff
allprojects {
    repositories {
+       flatDir {
+           dirs "${project(':unityLibrary').projectDir}/libs"
+       }
        google()
        mavenCentral()
    }
}
```

8. If you need to build a release package, open the *android/app/build.gradle* file and change the following:

```diff
     buildTypes {
         release {
             signingConfig signingConfigs.debug
         }
+        debug {
+            signingConfig signingConfigs.debug
+        }
+        profile {
+            signingConfig signingConfigs.debug
+        }
+        innerTest {
+            matchingFallbacks = ['debug', 'release']
+        }
+   }
```

> The code above use the `debug` signConfig for all buildTypes, which can be changed as you well if you need specify signConfig.

9. If you use `minifyEnabled true` in your *android/app/build.gradle* file, open the *android/unityLibrary/proguard-unity.txt* and change the following:

```diff
+    -keep class com.xraph.plugin.** {*;}
```

10. If you want Unity in it's own activity as an alternative, open the *android/app/src/main/AndroidManifest.xml* and change the following:

```diff
+    <activity
+        android:name="com.xraph.plugin.flutter_unity_widget.OverrideUnityActivity"
+        android:theme="@style/UnityThemeSelector"
+        android:screenOrientation="fullSensor"
+        android:launchMode="singleTask"
+        android:configChanges="mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale|layoutDirection|density"
+        android:hardwareAccelerated="false"
+        android:process=":Unity">
+    <meta-data android:name="com.xraph.plugin.flutter_unity_widget.OverrideUnityActivity" android:value="true" />
+    </activity>
```
-----
</details>

-----
</details>


<details>
 <summary>:information_source: <b>iOS</b></summary>
  
  1. Open the *ios/Runner.xcworkspace* (workspace, not the project) file in Xcode, right-click on the Navigator (not on an item), go to **Add Files to "Runner"** and add
  the *ios/UnityLibrary/Unity-Iphone.xcodeproj* file.
  
  <img src="https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/workspace.png" width="400" />
  
  2. (Optional) Select the *Unity-iPhone/Data* folder and change the Target Membership for Data folder to UnityFramework.
  
  <img src="https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/change_target_membership_data_folder.png" width="400" />
  
  3.1. If you're using Swift, open the *ios/Runner/AppDelegate.swift* file and change the following:

```diff
     import UIKit
     import Flutter
+    import flutter_unity_widget

     @UIApplicationMain
     @objc class AppDelegate: FlutterAppDelegate {
         override func application(
             _ application: UIApplication,
             didFinishLaunchingWithOptions launchOptions: [UIApplication.LaunchOptionsKey: Any]?
         ) -> Bool {
+            InitUnityIntegrationWithOptions(argc: CommandLine.argc, argv: CommandLine.unsafeArgv, launchOptions)

             GeneratedPluginRegistrant.register(with: self)
             return super.application(application, didFinishLaunchingWithOptions: launchOptions)
         }
     }
```

   3.2. If you're using Objective-C, open the *ios/Runner/main.m* file and change the following:
```diff
+    #import "flutter_unity_widget.swift.h"

     int main(int argc, char * argv[]) {
          @autoreleasepool {
+             InitUnityIntegration(argc, argv);
              return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
          }
     }
```

  4. Add the *UnityFramework.framework* file as a library to the Runner project.
  
  <img src="https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/libraries.png" width="400" />
  
  5. If you use Xcode 14 or newer, and Unity older than 2021.3.17f1 or 2022.2.2f1, your app might crash when running from Xcode.  
    Disable the `Thread Performance Checker` feature in Xcode to fix this.  
    - In Xcode go to `Product > Scheme > Edit Scheme...`  
    - Now With `Run` selected on the left, got to the `Diagnostics` tab and uncheck the checkbox for `Thread Performance Checker`. 
-----
</details>

### Setup AR

![gif](https://github.com/juicycleff/flutter-unity-view-widget/blob/master/files/ar-demo.gif?raw=true)

 The following setup for AR is done after making an export from Unity.


<b>Warning: The `XR Plugin Management` package version `4.3.1 - 4.3.3` has bug that breaks Android exports. </b>

- The bug accidentally deletes your AndroidManifest.xml file after each build, resulting in a broken export.  
Switch to version `4.2.2` or `4.4` to avoid this.  

<details>
 <summary>:information_source: <b>AR Foundation Android</b></summary>

  7. Open the *lib/__architecture__/* folder and check if there are both *libUnityARCore.so* and *libarpresto_api.so* files.
  There seems to be a bug where a Unity export does not include all lib files. If they are missing, use Unity to build a standalone .apk
  of your AR project, unzip the resulting apk, and copy over the missing .lib files to the `unityLibrary` module. 
  
  8. Repeat steps 5 and 6 from the Android <b>Platform specific setup</b> (editing build.gradle and settings.gradle), replacing `unityLibrary` with `arcore_client`, `unityandroidpermissions` and `UnityARCore`.
  
  9. When using `UnityWidget` in Flutter, set `fullscreen: false` to disable fullscreen.

-----
</details>

<details>
 <summary>:information_source: <b>AR Foundation iOS</b></summary>
7. Open the *ios/Runner/Info.plist* and change the following:

```diff
     <dict>
+        <key>NSCameraUsageDescription</key>
+        <string>$(PRODUCT_NAME) uses Cameras</string>
     </dict>
```
-----
</details>

<details>
 <summary>:information_source: <b>Vuforia Android</b></summary>

Thanks to [@PiotrxKolasinski](https://github.com/PiotrxKolasinski) for writing down the exact steps:

7. Open the *android/unityLibrary/build.gradle* file and change the following: 

```diff
-    implementation(name: 'VuforiaWrapper', ext: 'aar')
+    implementation project(':VuforiaWrapper')
```

8. Using Android Studio, go to **File > Open** and select the *android/* folder. A
    new project will open.
    
> Don't worry if the error message "Project with path ':VuforiaWrapper' could not be 
> found in project ':unityLibrary'" appears. The next step will fix it.

9. In this new project window, go to **File > New > New Module > Import .JAR/.AAR package**
    and select the *android/unityLibrary/libs/VuforiaWrapper.aar* file. A new folder
    named *VuforiaWrapper* will be created inside *android/*. You can now close this
    new project window.
  
-----
  </details>

## Emulators
We recommend using a physical iOS or Android device, as emulator support is limited.  
Below are the limited options to use an emulator.

<details>
<summary> <b>iOS Simulators</b> </summary>

The `Target SDK` option in the Unity player settings is important here.  
- `Device SDK` exports an ARM build. (Which does **NOT** work on ARM simulators)  
- `Simulator SDK` exports and x86 build for simulators.  


If you use ARKit or ARFoundation you are out of luck, iOS simulators do NOT support ARKit.

The rest depends on the type of processor in your mac:  
  
### (Apple Silicon) Run it on mac directly  

1. Export from Unity using `Device SDK` as target.  
2. In Xcode, go to the General tab of Runner.  
3. In Supported destinations add `Mac (Designed for iPhone)` or `Mac (Designed for iPad)`.  
4. Now select this as the target device to run on.  
5. You can now run the app directly on your mac instead of a simulator.  

### (Intel & Apple Silicon) Use an x86 simulator  
1. Set `Simulator SDK` in the Unity player settings.  
2. Make sure there are no AR or XR packages included in the Unity package manager.   
  (You will get the error `symbol not found in flat namespace '_UnityARKitXRPlugin_PluginLoad` otherwise)  
  
3. (Apple Silicon) Get Xcode to use a Rosetta emulator.  
  The next step assumes Xcode 14.3 or newer, if you use an older version look up how to start Xcode using Rosetta instead.
  - In Xcode go to `Product -> Destination -> Destination Architectures` and make sure Rosetta destinations are visible.  
4. Now you need to check the architecture settings in Xcode.
5. Select `Unity-iPhone` and go to `Build settings` -> `Architectures`.  
  If you exported Unity with the Simulator SDK, it should show only `x86_64` for architectures.  
6. Now select `Runner` and change the architecture to exactly match Unity-iPhone.  
  Make sure `x86_64` is the only entry, not one of multiple.
7. Now select `Pods` and click `flutter_unity_widget` in targets.  
  Go to Architectures in build settings again and set `Build Active Architecture Only` to `YES`.  
  (We want this to only use the active x86_64, not fall back to arm.)   
8. Run `Product -> Clean Build Folder` to make sure the new architecture settings are used.  
9. Now you should be able to launch Runner on a Simulator using Rosetta.  
  On Xcode 14.3 or higher the simulator should have `(Rosetta)` in the name.
10. Depending on your Flutter plugins, you might have to change the architecture for other installed Pods as well.  

-----
</details>

<details>
<summary> <b>Android emulators</b></summary>
  
Unity only supports ARM build targets for Android. However most Android emulators are x86 which means they simply won't work.  


- **Computer with ARM processor**  
If your computer has an ARM processor (e.g. Apple Silicon, Qualcomm) you should be able to emulate Android without issue.  
Create a virtual device using Android studio and make sure that the system image has an ABI that includes 'arm'.

  On macs with Apple Silicon (M1, M2), ARM emulators should be the default install option.  

  This was tested on Mac, but not on Linux or Windows.

- **Computer with x86/x64 processor**  
If you computer does not have an ARM processor, like most computers running on Intel or AMD, your options are limited. 
  
  You have 2 options:  
  - Download an ARM emulator from Android Studio anyway.  
    While adding a virtual device in android studio, on the 'System image' screen, select 'other images' and make sure to use and ABI that includes 'arm'.  
  The emulator will likely crash immediately or run extremely slow.  
  **This is not recommended.**  
  - Use the Chrome OS architecture  
  This is not officialy supported by Unity and there is no guarantee that it will work, but the Chrome OS target does seem to work on x86 Android emulators.  
  **Expect (visual) glitches and bugs**  
    - Enable`x86 (Chrome OS)` and `x86-64 (Chrome OS)` in the Unity player settings before making an export.  
    You might now be able to run on an regular Android emulator.  
    - Disable these settings again if you want to publish your app.  

-----
</details>

  
## Communicating 

### Flutter-Unity

1. On a `UnityWidget` widget, get the `UnityWidgetController` received by the `onUnityCreated` callback.

2. Use the method `postMessage` to send a string, using the GameObject name and the name of a behaviour method that should be called.

### Unity-Flutter

1. Select the GameObject that should execute the communication and go to **Inspector > Add Component > Unity Message Manager**.

<img src="https://i.stack.imgur.com/1gSOy.png" width="400" />

2. Create a new `MonoBehaviour` subclass and add to the same GameObject as a script.

3. On this new behaviour, call `GetComponent<UnityMessageManager>()` to get a `UnityMessageManager`.

4. Use the method `SendMessageToFlutter` to send a string. Receive this message using the `onUnityMessage` callback of a `UnityWidget`.




## Examples
### Simple Example

```dart
import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

void main() {
  runApp(
    const MaterialApp(
      home: UnityDemoScreen(),
    ),
  );
}

class UnityDemoScreen extends StatefulWidget {
  const UnityDemoScreen({Key? key}) : super(key: key);

  @override
  State<UnityDemoScreen> createState() => _UnityDemoScreenState();
}

class _UnityDemoScreenState extends State<UnityDemoScreen> {
  static final GlobalKey<ScaffoldState> _scaffoldKey =
      GlobalKey<ScaffoldState>();
  UnityWidgetController? _unityWidgetController;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      key: _scaffoldKey,
      body: SafeArea(
        bottom: false,
        child: WillPopScope(
          onWillPop: () async {
            // Pop the category page if Android back button is pressed.
            return true;
          },
          child: Container(
            color: Colors.yellow,
            child: UnityWidget(
              onUnityCreated: onUnityCreated,
            ),
          ),
        ),
      ),
    );
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    _unityWidgetController = controller;
  }
}

```
<br />

### Communicating with and from Unity

```dart
import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

void main() => runApp(const MyApp());

class MyApp extends StatefulWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  State<MyApp> createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  static final GlobalKey<ScaffoldState> _scaffoldKey =
      GlobalKey<ScaffoldState>();
  UnityWidgetController? _unityWidgetController;
  double _sliderValue = 0.0;

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        key: _scaffoldKey,
        appBar: AppBar(
          title: const Text('Unity Flutter Demo'),
        ),
        body: Card(
          margin: const EdgeInsets.all(8),
          clipBehavior: Clip.antiAlias,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(20.0),
          ),
          child: Stack(
            children: <Widget>[
              UnityWidget(
                onUnityCreated: onUnityCreated,
                onUnityMessage: onUnityMessage,
                onUnitySceneLoaded: onUnitySceneLoaded,
                fullscreen: false,
              ),
              Positioned(
                bottom: 20,
                left: 20,
                right: 20,
                // <You need a PointerInterceptor here on web>
                child: Card(
                  elevation: 10,
                  child: Column(
                    children: <Widget>[
                      const Padding(
                        padding: EdgeInsets.only(top: 20),
                        child: Text("Rotation speed:"),
                      ),
                      Slider(
                        onChanged: (value) {
                          setState(() {
                            _sliderValue = value;
                          });
                          setRotationSpeed(value.toString());
                        },
                        value: _sliderValue,
                        min: 0,
                        max: 20,
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  // Communcation from Flutter to Unity
  void setRotationSpeed(String speed) {
    _unityWidgetController?.postMessage(
      'Cube',
      'SetRotationSpeed',
      speed,
    );
  }

  // Communication from Unity to Flutter
  void onUnityMessage(message) {
    print('Received message from unity: ${message.toString()}');
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    _unityWidgetController = controller;
  }

  // Communication from Unity when new scene is loaded to Flutter
  void onUnitySceneLoaded(SceneLoaded? sceneInfo) {
    if (sceneInfo != null) {
      print('Received scene loaded from unity: ${sceneInfo.name}');
      print(
          'Received scene loaded from unity buildIndex: ${sceneInfo.buildIndex}');
    }
  }
}

```

## Props
 - `fullscreen` (Enable or disable fullscreen mode on Android)

## API
 - `pause()` (Use this to pause unity player)
 - `resume()` (Use this to resume unity player)
 - `unload()` (Use this to unload unity player) *Requires Unity 2019.4.3 or later
 - `quit()` (Use this to quit unity player)
 - `postMessage(String gameObject, methodName, message)` (Allows you invoke commands in Unity from flutter)
 - `onUnityMessage(data)` (Unity to flutter binding and listener)
 - `onUnityUnloaded()` (Unity to flutter listener when unity is unloaded)
 - `onUnitySceneLoaded(String name, int buildIndex, bool isLoaded, bool isValid,)` (Unity to flutter binding and listener when new scene is loaded)

 ## Troubleshooting

**Location:** Unity

**Error:**

```
Multiple precompiled assemblies with the same name Newtonsoft.Json.dll included on the current platform. Only one assembly with the same name is allowed per platform. (Assets/FlutterUnityIntegration/JsonDotNet/Assemblies/AOT/Newtonsoft.Json.dll)

PrecompiledAssemblyException: Multiple precompiled assemblies with the same name Newtonsoft.Json.dll included on the current platform. Only one assembly with the same name is allowed per platform.
```

**Solution:**
Locate the listed dll file, in this case:
`Assets/FlutterUnityIntegration/JsonDotNet/Assemblies/AOT/Newtonsoft.Json.dll`

- Option 1:
Delete the dll file or rename the file extension (e.g. `.dll.txt`) to stop it from being imported.
- Option 2:
Uninstall the package that conflicts in the Unity package manager (usually Version control, or Collab).
The exact package can be found by looking for newtonsoft in `package-lock.json`

---


**Location:** Unity

**Error:**

```
The type or namespace name 'Newtonsoft' could not be found (are you missing a using directive or an assembly reference?)
The type or namespace name 'JObject' could not be found (are you missing a using directive or an assembly reference?)
The type or namespace name 'JToken' could not be found (are you missing a using directive or an assembly reference?)
The type or namespace name 'JToken' could not be found (are you missing a using directive or an assembly reference?)
```

**Solution:**

Include the Newtonsoft JsonDotNet library.
It is likely already included in your project with a wrong file extension:
`Assets/FlutterUnityIntegration/JsonDotNet/Assemblies/AOT/Newtonsoft.Json.dll.txt`
Rename the `.dll.txt` extension to `.dll` in your file explorer and open Unity again.

Alternatively you can manually add [the library](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.1/manual/index.html) from the Unity package manager.

---


**Location:** Unity

**Error:**

```
InvalidOperationException: The build target does not support build appending.
```

**Solution:**

1. Open the *unity/__project-name__/Assets/FlutterUnityIntegration/Editor/Build.cs* file.

1.1. On line 48, change the following:

```diff
-    var options = BuildOptions.AcceptExternalModificationsToPlayer;
+    var options = BuildOptions.AllowDebugging;
+    EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
```

1.2. On line 115, change the following:

```diff
-    var options = BuildOptions.AcceptExternalModificationsToPlayer;
+    var options = BuildOptions.AllowDebugging;
```

---

**Location:** Android Studio

**Error:**

```
minSdkVersion XX cannot be smaller than version 19 declared in library 
    \ [:flutter_unity_widget] .../AndroidManifest.xml as the library might be using 
    \ APIs not available in XX
```

**Solution:**

1. Open the *android/app/build.gradle* file and change the following:

```diff
-    minSdkVersion XX
+    minSdkVersion 19
```

---

**Location**: Android Studio

**Error:**

```
e: .../FlutterUnityWidgetBuilder.kt: (15, 42): Expecting a parameter declaration
e: .../FlutterUnityWidgetBuilder.kt: (23, 25): Expecting an argument
e: .../FlutterUnityWidgetController.kt: (22, 44): Expecting a parameter declaration
e: .../FlutterUnityWidgetFactory.kt: (13, 58): Expecting a parameter declaration
```

**Solution:** 

1. Open the *android/build.gradle* file and change the following:

```diff
-    ext.kotlin_version = '1.3.50'
+    ext.kotlin_version = '1.4.31'
```

---

**Location:** Android Studio

**Error:**

```
Unable to find a matching variant of project :unityLibrary:
```

**Solution:**

1. Open the *android/app/build.gradle* file and change the following:

```diff
     lintOptions {
         disable 'InvalidPackage'
+        checkReleaseBuilds false
     }
```
 
  
 
## Flavors

### Recommendation

The easiest way to apply flavors for your app would be: [flutter_flavorizr](https://pub.dev/packages/flutter_flavorizr).

If you use flavors in your app you will notice that especially iOS crashes while running or building your app! 
Here are the necessary steps for flavored apps:

### Android

No changes needed. Flavors are applied without any additional setups.

### iOS

For your Unity iOS-Build you have to add your flavors to your Unity iOS Configuration.

1. Check your actual `Runner` (your app) configurations. If you have for example the flavors:

- dev
- prod

Your `Runner` configurations are looking like this:

![iOS Runner Config](https://raw.githubusercontent.com/juicycleff/flutter-unity-view-widget/master/files/iOSRunnerConfig.png)

So you have the flavors:

- `Debug-dev`
- `Profile-dev`
- `Release-dev`
- `Debug-prod`
- `Profile-prod`
- `Release-prod`

These flavors needs to be added to your `Unity-IPhone` project.

2. Go into your `Unity-IPhone` project -> PROJECT `Unity-IPhone` -> Info:

![Unity-IPhone](https://raw.githubusercontent.com/juicycleff/flutter-unity-view-widget/master/files/UnityIPhone.png)

Here you can see in the Configurations section only:

- `Release`
- `ReleaseForProfiling`
- `ReleaseForRunning`
- `Debug`

3. Copy `Debug` configuration twice and rename them to `Debug-dev` and the second to `Debug-prod`.

You can do that by selecting `+` and duplicate the configuration like this:

![Duplicate configuration](https://raw.githubusercontent.com/juicycleff/flutter-unity-view-widget/master/files/DuplicateConfig.png)

4. Repeat this with `Release` to `Release-dev` and `Release-prod`.

5. Repeat this with `Release` to `Profile-dev` and `Profile-prod`.

6. Your `Unity-IPhone` configurations should now look like this:

![Unity Configurations](https://raw.githubusercontent.com/juicycleff/flutter-unity-view-widget/master/files/UnityConfigurations.png)

### Web

Flutter on default doesn't support `--flavor` for building web. But you can set your target `main.dart` entrypoint (with `-t main.dart`) while running and building. So if you setup your flavors properly there're also no changes needed for web to apply changes for your Flutter-Unity web App.

## Known issues
 - Remember to disabled fullscreen in unity player settings to disable unity fullscreen.
 - Unity freezes and crashes on Android, please use OpenGL3 as Graphics API.
 - Project fails to build due to some native dependencies in your unity project, please integrate the native libraries for those dependencies on Android or iOS
 - App crashes on screen exit and re-entry do this
   > Build Setting - iOS - Other Settings - Configuration - Enable Custom Background Behaviors or iOS
 - Android builds takes forever to complete Unity 2022.1.*, remove these lines from unityLibrary/build.gradle file
   > commandLineArgs.add("--enable-debugger")
   > commandLineArgs.add("--profiler-report")
   > commandLineArgs.add("--profiler-output-file=" + workingDir + "/build/il2cpp_"+ abi + "_" + configuration + "/il2cpp_conv.traceevents")

### Web GL

Flutter widgets stacked on top of the UnityWidget will not register clicks or taps. This is a [Flutter issue](https://github.com/flutter/flutter/issues/72273) and can be solved by using the  [PointerInterceptor](https://pub.dev/packages/pointer_interceptor) package.

Example usage:

```dart
Stack(
  children: [
    UnityWidget(
      onUnityCreated: onUnityCreated,
      onUnityMessage: onUnityMessage,
      onUnitySceneLoaded: onUnitySceneLoaded,
    ),
    Positioned(
      bottom: 20,
      left: 20,
      right: 20,
      child: PointerInterceptor(
        child: ElevatedButton(
          onPressed: () {
            // do something
          },
          child: const Text('Example button'),
        ),
      ),
    ),
```


We already integrated this into our [Examples](/example/lib/screens/) in the `/example` folder.


#### Sponsors

Support this project with your organization. Your donations will be used to help children first and then those in need. Your logo will show up here with a link to your website. [[Contribute](https://opencollective.com/ultimate-backend/contribute)]

<a href="https://opencollective.com/ultimate-backend/sponsor/0/website"><img src="https://opencollective.com/ultimate-backend/sponsor/0/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/1/website"><img src="https://opencollective.com/ultimate-backend/sponsor/1/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/2/website"><img src="https://opencollective.com/ultimate-backend/sponsor/2/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/3/website"><img src="https://opencollective.com/ultimate-backend/sponsor/3/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/4/website"><img src="https://opencollective.com/ultimate-backend/sponsor/4/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/5/website"><img src="https://opencollective.com/ultimate-backend/sponsor/5/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/6/website"><img src="https://opencollective.com/ultimate-backend/sponsor/6/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/7/website"><img src="https://opencollective.com/ultimate-backend/sponsor/7/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/8/website"><img src="https://opencollective.com/ultimate-backend/sponsor/8/avatar.svg"></a>
<a href="https://opencollective.com/ultimate-backend/sponsor/9/website"><img src="https://opencollective.com/ultimate-backend/sponsor/9/avatar.svg"></a>

[version-badge]: https://img.shields.io/pub/v/flutter_unity_widget.svg?style=flat-square
[package]: https://pub.dartlang.org/packages/flutter_unity_widget/
[license-badge]: https://img.shields.io/github/license/juicycleff/flutter-unity-view-widget.svg?style=flat-square
[license]: https://github.com/juicycleff/flutter-unity-view-widget/blob/master/LICENSE
[prs-badge]: https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square
[prs]: https://makeapullrequest.com
[github-watch-badge]: https://img.shields.io/github/watchers/juicycleff/flutter-unity-view-widget.svg?style=social
[github-watch]: https://github.com/juicycleff/flutter-unity-view-widget/watchers
[github-star-badge]: https://img.shields.io/github/stars/juicycleff/flutter-unity-view-widget.svg?style=social
[github-star]: https://github.com/juicycleff/flutter-unity-view-widget/stargazers

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://www.xraph.com"><img src="https://avatars2.githubusercontent.com/u/11243590?v=4" width="100px;" alt="Rex Raphael"/><br /><sub><b>Rex Raphael</b></sub></a><br /><a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=juicycleff" title="Code">💻</a> <a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=juicycleff" title="Documentation">📖</a> <a href="#question-juicycleff" title="Answering Questions">💬</a> <a href="https://github.com/juicycleff/flutter-unity-view-widget/issues?q=author%3Ajuicycleff" title="Bug reports">🐛</a> <a href="#review-juicycleff" title="Reviewed Pull Requests">👀</a> <a href="#tutorial-juicycleff" title="Tutorials">✅</a></td>
    <td align="center"><a href="https://stockxit.com"><img src="https://avatars1.githubusercontent.com/u/1475368?v=4" width="100px;" alt="Thomas Stockx"/><br /><sub><b>Thomas Stockx</b></sub></a><br /><a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=thomas-stockx" title="Code">💻</a> <a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=thomas-stockx" title="Documentation">📖</a> <a href="#question-thomas-stockx" title="Answering Questions">💬</a> <a href="#tutorial-thomas-stockx" title="Tutorials">✅</a></td>
    <td align="center"><a href="https://krispypen.github.io/"><img src="https://avatars1.githubusercontent.com/u/156955?v=4" width="100px;" alt="Kris Pypen"/><br /><sub><b>Kris Pypen</b></sub></a><br /><a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=krispypen" title="Code">💻</a> <a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=krispypen" title="Documentation">📖</a> <a href="#question-krispypen" title="Answering Questions">💬</a> <a href="#tutorial-krispypen" title="Tutorials">✅</a></td>
    <td align="center"><a href="https://github.com/lorant-csonka-planorama"><img src="https://avatars2.githubusercontent.com/u/48209860?v=4" width="100px;" alt="Lorant Csonka"/><br /><sub><b>Lorant Csonka</b></sub></a><br /><a href="https://github.com/juicycleff/flutter-unity-view-widget/commits?author=lorant-csonka-planorama" title="Documentation">📖</a> <a href="#video-lorant-csonka-planorama" title="Videos">📹</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
