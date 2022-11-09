import 'dart:async';

import 'package:flutter/services.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

class MethodHandler {
  static final MethodChannel defaultChannel = MethodChannel('plugin.xraph.com/default_unity_view_channel');
  static var _isChannelInitialized = false;

  // The controller we need to broadcast the different events coming
  // from handleMethodCall.
  //
  // It is a `broadcast` because multiple controllers will connect to
  // different stream views of this Controller.
  static final StreamController<UnityEvent> _unityStreamController =
  StreamController<UnityEvent>.broadcast();

  // Returns a filtered view of the events in the _controller, by unityId.
  static Stream<UnityEvent> events(int unityId) =>
      _unityStreamController.stream.where((event) => event.unityId == unityId);

  static MethodChannel ensureChannelInitialized(int unityId) {
    if(!MethodHandler._isChannelInitialized){
      defaultChannel.setMethodCallHandler(
              (MethodCall call) => _handleMethodCall(call, unityId));
      MethodHandler._isChannelInitialized = true;
    }
    return defaultChannel;
  }

  static Future<dynamic> _handleMethodCall(MethodCall call, int unityId) async {
    switch (call.method) {
      case "events#onUnityMessage":
        _unityStreamController.add(UnityMessageEvent(unityId, call.arguments));
        break;
      case "events#onUnityUnloaded":
        _unityStreamController.add(UnityLoadedEvent(unityId, call.arguments));
        break;
      case "events#onUnitySceneLoaded":
        _unityStreamController.add(UnitySceneLoadedEvent(
            unityId, SceneLoaded.fromMap(call.arguments)));
        break;
      case "events#onUnityCreated":
        _unityStreamController.add(UnityCreatedEvent(unityId, call.arguments));
        break;
      default:
        throw UnimplementedError("Unimplemented ${call.method} method");
    }
  }
}