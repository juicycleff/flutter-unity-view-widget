import 'dart:async';

import 'package:flutter_unity_widget/flutter_unity_widget.dart';

import 'global_unity_widget_platform.dart';

class GlobalUnityController {
  GlobalUnityController._() {}

  static GlobalUnityController? _instance = null;

  /// Returns an instance using the default [Fitness].
  static GlobalUnityController get instance {
    return GlobalUnityController.instanceFor();
  }

  /// Returns an instance using a specified [Fitness].
  factory GlobalUnityController.instanceFor() {
    if (_instance == null) {
      return _instance = GlobalUnityController._();
    }
    return _instance!;
  }

  /// The unityId for this controller
  int lastUnityId = 0;

  /// used for cancel the subscription
  StreamSubscription? _onUnityMessageSub,
      _onUnitySceneLoadedSub,
      _onUnityUnloadedSub;
  // StreamSubscription? _onDataEventSub;

  Future<void> init() {
    return GlobalUnityWidgetPlatform.instance.init();
  }

  Stream<EventDataPayload> get stream {
    return GlobalUnityWidgetPlatform.instance.stream;
  }

  /// Checks to see if unity player is ready to be used
  /// Returns `true` if unity player is ready.
  Future<bool?>? isReady() {
    return GlobalUnityWidgetPlatform.instance.isReady();
  }

  /// Get the current pause state of the unity player
  /// Returns `true` if unity player is paused.
  Future<bool?>? isPaused() {
    return GlobalUnityWidgetPlatform.instance.isPaused();
  }

  /// Get the current load state of the unity player
  /// Returns `true` if unity player is loaded.
  Future<bool?>? isLoaded() {
    return GlobalUnityWidgetPlatform.instance.isLoaded();
  }

  /// Helper method to know if Unity has been put in background mode (WIP) unstable
  /// Returns `true` if unity player is in background.
  Future<bool?>? inBackground() {
    return GlobalUnityWidgetPlatform.instance.inBackground();
  }

  /// Creates a unity player if it's not already created. Please only call this if unity is not ready,
  /// or is in unloaded state. Use [isLoaded] to check.
  /// Returns `true` if unity player was created succesfully.
  Future<bool?>? create() {
    return GlobalUnityWidgetPlatform.instance.createUnityPlayer();
  }

  /// Post message to unity from flutter. This method takes in a string [message].
  /// The [gameObject] must match the name of an actual unity game object in a scene at runtime, and the [methodName],
  /// must exist in a `MonoDevelop` `class` and also exposed as a method. [message] is an parameter taken by the method
  ///
  /// ```dart
  /// postMessage("GameManager", "openScene", "ThirdScene")
  /// ```
  Future<void>? postMessage(String gameObject, methodName, message) {
    return GlobalUnityWidgetPlatform.instance.postMessage(
      gameObject: gameObject,
      methodName: methodName,
      message: message,
    );
  }

  /// Post message to unity from flutter. This method takes in a Json or map structure as the [message].
  /// The [gameObject] must match the name of an actual unity game object in a scene at runtime, and the [methodName],
  /// must exist in a `MonoDevelop` `class` and also exposed as a method. [message] is an parameter taken by the method
  ///
  /// ```dart
  /// postJsonMessage("GameManager", "openScene", {"buildIndex": 3, "name": "ThirdScene"})
  /// ```
  Future<void>? postJsonMessage(
      String gameObject, String methodName, Map<String, dynamic> message) {
    return GlobalUnityWidgetPlatform.instance.postJsonMessage(
      gameObject: gameObject,
      methodName: methodName,
      message: message,
    );
  }

  /// Pause the unity in-game player with this method
  Future<void>? pause() {
    return GlobalUnityWidgetPlatform.instance.pausePlayer();
  }

  /// Resume the unity in-game player with this method idf it is in a paused state
  Future<void>? resume() {
    return GlobalUnityWidgetPlatform.instance.resumePlayer();
  }

  /// Sometimes you want to open unity in it's own process and openInNativeProcess does just that.
  /// It works for Android and iOS is WIP
  Future<void>? openInNativeProcess() {
    return GlobalUnityWidgetPlatform.instance.openInNativeProcess();
  }

  /// Unloads unity player from th current process (Works on Android only for now)
  /// iOS is WIP. For more information please read [Unity Docs](https://docs.unity3d.com/2020.2/Documentation/Manual/UnityasaLibrary.html)
  Future<void>? unload() {
    return GlobalUnityWidgetPlatform.instance.unloadPlayer();
  }

  /// Quits unity player. Note that this kills the current flutter process, thus quiting the app
  Future<void>? quit() {
    return GlobalUnityWidgetPlatform.instance.quitPlayer();
  }

  /// cancel the subscriptions when dispose called
  void _cancelSubscriptions() {
    _onUnityMessageSub?.cancel();
    _onUnitySceneLoadedSub?.cancel();
    _onUnityUnloadedSub?.cancel();

    _onUnityMessageSub = null;
    _onUnitySceneLoadedSub = null;
    _onUnityUnloadedSub = null;
  }

  Future<void> dispose() async {
    _cancelSubscriptions();
    await GlobalUnityWidgetPlatform.instance.dispose();
  }
}
