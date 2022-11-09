import 'dart:async';
import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/services.dart';
import 'package:flutter_unity_widget/src/helpers/method_handler.dart';
import 'package:stream_transform/stream_transform.dart';

import '../helpers/events.dart';
import '../helpers/misc.dart';
import '../helpers/types.dart';
import 'unity_widget_platform.dart';
import 'windows_unity_widget_view.dart';


class MethodChannelUnityWidget extends UnityWidgetPlatform {
  // Every method call passes the int unityId
  late final Map<int, MethodChannel> _channels = {};

  /// Set [UnityWidgetFlutterPlatform] to use [AndroidViewSurface] to build the Google Maps widget.
  ///
  /// This implementation uses hybrid composition to render the Unity Widget
  /// Widget on Android. This comes at the cost of some performance on Android
  /// versions below 10. See
  /// https://flutter.dev/docs/development/platform-integration/platform-views#performance for more
  /// information.
  /// Defaults to false.
  bool useAndroidViewSurface = true;

 final _defaultChannel = MethodHandler.defaultChannel;
 final _events = MethodHandler.events;

  /// Accesses the MethodChannel associated to the passed unityId.
  MethodChannel channel(int unityId) {
    return _defaultChannel;
  }


  /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  @override
  Future<void> init(int unityId) {
    MethodHandler.ensureChannelInitialized(unityId);
    return _defaultChannel.invokeMethod<void>('unity#waitForUnity', <String, dynamic>{
      'unityId': unityId,
    });
  }

  /// Dispose of the native resources.
  @override
  Future<void> dispose({int? unityId}) async {
    try {
      if (unityId != null) await channel(unityId).invokeMethod('unity#dispose', <String, dynamic>{
        'unityId': unityId,
      });
    } catch (e) {
      // ignore
    }
  }


  @override
  Future<bool?> isPaused({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#isPaused', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<bool?> isReady({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#isReady', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<bool?> isLoaded({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#isLoaded', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<bool?> inBackground({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#inBackground', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<bool?> createUnityPlayer({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#createPlayer', <String, dynamic>{
      'unityId': unityId,
    });
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
    await channel(unityId).invokeMethod('unity#postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': message,
    });
  }

  @override
  Future<void> postJsonMessage({
    required int unityId,
    required String gameObject,
    required String methodName,
    required Map message,
  }) async {
    await channel(unityId).invokeMethod('unity#postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': json.encode(message),
    });
  }

  @override
  Future<void> pausePlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#pausePlayer', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<void> resumePlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#resumePlayer', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<void> openInNativeProcess({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#openInNativeProcess', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<void> unloadPlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#unloadPlayer', <String, dynamic>{
      'unityId': unityId,
    });
  }

  @override
  Future<void> quitPlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#quitPlayer', <String, dynamic>{
      'unityId': unityId,
    });
  }
}
