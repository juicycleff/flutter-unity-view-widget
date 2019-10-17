# flutter_unity_widget_example

Demonstrates how to use the flutter_unity_widget plugin.

## Run the sample on Android

  1. Open the `unity` project and build it: Menu -> Flutter -> Export Android
  2. Copy `android/UnityExport/libs/unity-classes.jar` to `android/unity-classes/unity-classes.jar` and overwrite the existing file. You only need to do this each time you use a different Unity version.
  3. `flutter run`

## Run the sample on iOS
  1. Open the `unity` project and build it: Menu -> Flutter -> Export iOS
     
     Be sure you use at least Unity version 2019.3 or up.
     
  2. open ios/Runner.xcworkspace (workspace!, not the project) in Xcode and add the exported project in the workspace root (with a right click in the Navigator, not on an item -> Add Files to "Runner" -> add the UnityExport/Unity-Iphone.xcodeproj file
  <img src="../workspace.png" width="400" />
  
  3. Select the Unity-iPhone/Data folder and change the Target Membership for Data folder to UnityFramework
  <img src="../change_target_membership_data_folder.png" width="400" /> 
  
  4. `flutter run`
