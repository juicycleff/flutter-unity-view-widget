import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/services.dart';
import 'package:stream_transform/stream_transform.dart';

import '../helpers/events.dart';
import '../helpers/types.dart';
import 'flutter_unity_controller.dart';
import 'unity_widget_platform.dart';
import 'windows_unity_widget_view.dart';

class MethodChannelUnityWidget extends UnityWidgetPlatform {
  var _unityId = 0;

  /// Set [UnityWidgetFlutterPlatform] to use [AndroidViewSurface] to build the Google Maps widget.
  ///
  /// This implementation uses hybrid composition to render the Unity Widget
  /// Widget on Android. This comes at the cost of some performance on Android
  /// versions below 10. See
  /// https://flutter.dev/docs/development/platform-integration/platform-views#performance for more
  /// information.
  /// Defaults to false.
  bool useAndroidViewSurface = true;

  MethodChannelUnityWidget() {
    _handleInternalStreaming();
  }

  /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  @override
  Future<void> init(int unityId) async {
    _unityId = unityId;
    FlutterUnityController.instance.lastUnityId = unityId;
    await FlutterUnityController.instance.init();
  }

  /// Dispose of the native resources.
  @override
  void dispose({int? unityId}) {
    try {
      if (unityId != null) {
        FlutterUnityController.instance.lastUnityId = unityId;
        // _unityStreamController.close();
      }
    } catch (e) {
      // ignore
    }
  }

  // The controller we need to broadcast the different events coming
  // from handleMethodCall.
  //
  // It is a `broadcast` because multiple controllers will connect to
  // different stream views of this Controller.
  StreamController<UnityEvent> _unityStreamController =
      StreamController<UnityEvent>.broadcast();

  // Returns a filtered view of the events in the _controller, by unityId.
  Stream<UnityEvent> _events(int unityId) =>
      _unityStreamController.stream.where((event) => event.unityId == unityId);

  Future<dynamic> _handleInternalStreaming() async {
    if (_unityStreamController.isClosed) {
      _unityStreamController = StreamController<UnityEvent>.broadcast();
    }
    FlutterUnityController.instance.stream.listen((event) {
      switch (event.eventType) {
        case UnityEventTypes.OnUnityViewCreated:
          _unityStreamController.add(UnityCreatedEvent(0, event.data));
          break;
        case UnityEventTypes.OnUnityPlayerUnloaded:
          _unityStreamController.add(UnityLoadedEvent(0, event.data));
          break;
        case UnityEventTypes.OnUnityMessage:
          _unityStreamController.add(UnityMessageEvent(_unityId, event.data));
          break;
        case UnityEventTypes.OnUnitySceneLoaded:
          _unityStreamController
              .add(UnitySceneLoadedEvent(0, SceneLoaded.fromMap(event.data)));
          break;
        case UnityEventTypes.OnUnityPlayerReInitialize:
        case UnityEventTypes.OnViewReattached:
        case UnityEventTypes.OnUnityPlayerCreated:
        case UnityEventTypes.OnUnityPlayerQuited:
          // TODO: Handle this case.
          break;
      }
    });
  }

  @override
  Future<bool?> isPaused({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.isPaused();
  }

  @override
  Future<bool?> isReady({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.isReady();
  }

  @override
  Future<bool?> isLoaded({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.isLoaded();
  }

  @override
  Future<bool?> inBackground({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.inBackground();
  }

  @override
  Future<bool?> createUnityPlayer({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.create();
  }

  @override
  Stream<UnityMessageEvent> onUnityMessage({required int unityId}) {
    return _events(unityId).whereType<UnityMessageEvent>();
  }

  @override
  Stream<UnityLoadedEvent> onUnityUnloaded({required int unityId}) {
    return _events(unityId).whereType<UnityLoadedEvent>();
  }

  @override
  Stream<UnityCreatedEvent> onUnityCreated({required int unityId}) {
    return _events(unityId).whereType<UnityCreatedEvent>();
  }

  @override
  Stream<UnitySceneLoadedEvent> onUnitySceneLoaded({required int unityId}) {
    return _events(unityId).whereType<UnitySceneLoadedEvent>();
  }

  @override
  Widget buildViewWithTextDirection(
    int creationId,
    PlatformViewCreatedCallback onPlatformViewCreated, {
    required TextDirection textDirection,
    Set<Factory<OneSequenceGestureRecognizer>>? gestureRecognizers,
    Map<String, dynamic> unityOptions = const <String, dynamic>{},
    bool? useAndroidViewSurf,
    bool? height,
    bool? width,
    bool? unityWebSource,
    String? unitySrcUrl,
  }) {
    final String _viewType = 'plugin.xraph.com/unity_view';

    if (useAndroidViewSurf != null) useAndroidViewSurface = useAndroidViewSurf;

    final Map<String, dynamic> creationParams = unityOptions;

    if (defaultTargetPlatform == TargetPlatform.windows) {
      return WindowsUnityWidgetView();
    }

    if (defaultTargetPlatform == TargetPlatform.android) {
      if (!useAndroidViewSurface) {
        return AndroidView(
          viewType: _viewType,
          onPlatformViewCreated: onPlatformViewCreated,
          gestureRecognizers: gestureRecognizers,
          creationParams: creationParams,
          creationParamsCodec: const StandardMessageCodec(),
          hitTestBehavior: PlatformViewHitTestBehavior.opaque,
          layoutDirection: TextDirection.ltr,
        );
      }

      return PlatformViewLink(
        viewType: _viewType,
        surfaceFactory: (
          BuildContext context,
          PlatformViewController controller,
        ) {
          return AndroidViewSurface(
            controller: controller as AndroidViewController,
            gestureRecognizers: gestureRecognizers ??
                const <Factory<OneSequenceGestureRecognizer>>{},
            hitTestBehavior: PlatformViewHitTestBehavior.opaque,
          );
        },
        onCreatePlatformView: (PlatformViewCreationParams params) {
          final controller = PlatformViewsService.initExpensiveAndroidView(
            id: params.id,
            viewType: _viewType,
            layoutDirection: TextDirection.ltr,
            creationParams: creationParams,
            creationParamsCodec: const StandardMessageCodec(),
            onFocus: () => params.onFocusChanged(true),
          );

          controller
            ..addOnPlatformViewCreatedListener(params.onPlatformViewCreated)
            ..addOnPlatformViewCreatedListener(onPlatformViewCreated)
            ..create();
          return controller;
        },
      );
    } else if (defaultTargetPlatform == TargetPlatform.iOS) {
      return UiKitView(
        viewType: _viewType,
        onPlatformViewCreated: onPlatformViewCreated,
        gestureRecognizers: gestureRecognizers,
        creationParams: creationParams,
        creationParamsCodec: const StandardMessageCodec(),
      );
    }
    return Text(
        '$defaultTargetPlatform is not yet supported by the unity player plugin');
  }

  @override
  Widget buildView(
    int creationId,
    PlatformViewCreatedCallback onPlatformViewCreated, {
    Map<String, dynamic> unityOptions = const {},
    Set<Factory<OneSequenceGestureRecognizer>>? gestureRecognizers,
    bool? useAndroidViewSurf,
    String? unitySrcUrl,
  }) {
    return buildViewWithTextDirection(
      creationId,
      onPlatformViewCreated,
      textDirection: TextDirection.ltr,
      gestureRecognizers: gestureRecognizers,
      unityOptions: unityOptions,
      useAndroidViewSurf: useAndroidViewSurf,
      unitySrcUrl: unitySrcUrl,
    );
  }

  @override
  Future<void> postMessage({
    required int unityId,
    required String gameObject,
    required String methodName,
    required String message,
  }) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance
        .postMessage(gameObject, methodName, message);
  }

  @override
  Future<void> postJsonMessage({
    required int unityId,
    required String gameObject,
    required String methodName,
    required Map message,
  }) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.postJsonMessage(
        gameObject, methodName, message as Map<String, dynamic>);
  }

  @override
  Future<void> pausePlayer({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    await FlutterUnityController.instance.pause();
  }

  @override
  Future<void> resumePlayer({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.resume();
  }

  @override
  Future<void> openInNativeProcess({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.openInNativeProcess();
  }

  @override
  Future<void> unloadPlayer({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.unload();
  }

  @override
  Future<void> quitPlayer({required int unityId}) async {
    FlutterUnityController.instance.lastUnityId = unityId;
    return await FlutterUnityController.instance.quit();
  }
}
