import 'dart:async';
import 'dart:convert';

import 'package:flutter/services.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:stream_transform/stream_transform.dart';

import 'flutter_unity_platform.dart';

const String _channelPrefix = 'plugin.xraph.com';
const String _streamChannelId = '$_channelPrefix/stream_channel';
const MethodChannel _channel = MethodChannel('$_channelPrefix/base_channel');

class FlutterUnityMethodChannel extends FlutterUnityPlatform {
  /// Accesses the MethodChannel associated to the passed unityId.
  MethodChannel get channel {
    return _channel;
  }

  static late final StreamController<EventDataPayload>
      _unityDataStreamController;

  // The controller we need to broadcast the different events coming
  // from handleMethodCall.
  //
  // It is a `broadcast` because multiple controllers will connect to
  // different stream views of this Controller.
  final StreamController<UnityEvent> _unityStreamController =
      StreamController<UnityEvent>.broadcast();

  // Returns a filtered view of the events in the _controller, by unityId.
  Stream<UnityEvent> get _events => _unityStreamController.stream;

  /// *************************** EVENT CHANNELS *******************************/

  final dataStreamChannel = EventChannel(_streamChannelId, _channel.codec);

  StreamController<T> _createBroadcastStream<T>() {
    return StreamController<T>.broadcast();
  }

  /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  @override
  Future<void> init() {
    return channel.invokeMethod<void>('unity#waitForUnity');
  }

  FlutterUnityMethodChannel() : super() {
    // Create a app instance broadcast stream for native listener events
    _unityDataStreamController = _createBroadcastStream<EventDataPayload>();
    _listenForMessages();
  }

  var lastUnityId = 0;

  void _listenForMessages() {
    dataStreamChannel.receiveBroadcastStream().listen(
      (arguments) {
        final payload = EventDataPayload.fromMap(arguments)!;
        switch (payload.eventType) {
          case UnityEventTypes.OnUnityViewCreated:
            _unityStreamController.add(UnityCreatedEvent(0, payload.data));
            break;
          case UnityEventTypes.OnUnityPlayerUnloaded:
            _unityStreamController.add(UnityLoadedEvent(0, payload.data));
            break;
          case UnityEventTypes.OnUnityMessage:
            _unityStreamController.add(UnityMessageEvent(0, payload.data));
            break;
          case UnityEventTypes.OnUnitySceneLoaded:
            _unityStreamController.add(
                UnitySceneLoadedEvent(0, SceneLoaded.fromMap(payload.data)));
            break;
          case UnityEventTypes.OnUnityPlayerReInitialize:
          case UnityEventTypes.OnViewReattached:
          case UnityEventTypes.OnUnityPlayerCreated:
          case UnityEventTypes.OnUnityPlayerQuited:
            break;
        }

        _unityDataStreamController.add(EventDataPayload.fromMap(arguments)!);
      },
    );
  }

  /// Dispose of the native resources.
  @override
  Future<void> dispose() async {
    try {
      await channel.invokeMethod('unity#dispose', <String, dynamic>{
        'unityId': lastUnityId,
      });
    } catch (e) {
      // ignore
    }
  }

  @override
  Future<bool?> isPaused() async {
    return await channel.invokeMethod('unity#isPaused', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<bool?> isReady() async {
    return await channel.invokeMethod('unity#isReady', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<bool?> isLoaded() async {
    return await channel.invokeMethod('unity#isLoaded', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<bool?> inBackground() async {
    return await channel.invokeMethod('unity#inBackground', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<bool?> createUnityPlayer() async {
    return await channel.invokeMethod('unity#createPlayer', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Stream<UnityMessageEvent> onUnityMessage() {
    return _events.whereType<UnityMessageEvent>();
  }

  @override
  Stream<EventDataPayload> get stream {
    return _unityDataStreamController.stream;
  }

  @override
  Stream<UnityLoadedEvent> onUnityUnloaded() {
    return _events.whereType<UnityLoadedEvent>();
  }

  @override
  Stream<UnityCreatedEvent> onUnityCreated() {
    return _events.whereType<UnityCreatedEvent>();
  }

  @override
  Stream<UnitySceneLoadedEvent> onUnitySceneLoaded() {
    return _events.whereType<UnitySceneLoadedEvent>();
  }

  @override
  Future<void> postMessage({
    required String gameObject,
    required String methodName,
    required String message,
  }) async {
    await channel.invokeMethod('unity#postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': message,
      'unityId': lastUnityId,
    });
  }

  @override
  Future<void> postJsonMessage({
    required String gameObject,
    required String methodName,
    required Map message,
  }) async {
    await channel.invokeMethod('unity#postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': json.encode(message),
      'unityId': lastUnityId,
    });
  }

  @override
  Future<void> pausePlayer() async {
    await channel.invokeMethod('unity#pausePlayer', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<void> resumePlayer() async {
    await channel.invokeMethod('unity#resumePlayer', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<void> openInNativeProcess() async {
    await channel.invokeMethod('unity#openInNativeProcess', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<void> unloadPlayer() async {
    await channel.invokeMethod('unity#unloadPlayer', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }

  @override
  Future<void> quitPlayer() async {
    await channel.invokeMethod('unity#quitPlayer', <String, dynamic>{
      'unityId': lastUnityId,
    });
  }
}
