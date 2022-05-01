// In order to *not* need this ignore, consider extracting the "web" version
// of your plugin as a separate package, instead of inlining it in the same
// package as the core of your plugin.
// ignore: avoid_web_libraries_in_flutter
import 'dart:convert';
import 'dart:html' as html;

import 'package:flutter/services.dart';

class UnityWebEvent {
  UnityWebEvent({
    required this.name,
    this.data,
  }) {}
  final String name;
  final dynamic data;
}

class UnityWebControllerInterceptor {
  late html.MessageEvent _unityFlutterBiding;
  late html.MessageEvent _unityFlutterBidingFn;

  final Function(String data) onUnityMessage;
  final Function(dynamic data) onUnitySceneChanged;
  final Function() onUnityReady;

  bool unityReady = false;
  bool unityPause = true;

  UnityWebControllerInterceptor({
    required this.onUnityMessage,
    required this.onUnitySceneChanged,
    required this.onUnityReady,
  }) {
    html.window.addEventListener('message', (event) {
      final raw = (event as html.MessageEvent).data.toString();
      if (raw == '' || raw == null) return;
      if (raw == 'unityReady') {
        unityReady = true;
        unityPause = false;
        onUnityReady();
        _refreshUnityView();
        return;
      }

      _processEvents(UnityWebEvent(
        name: event.data['name'],
        data: event.data['data'],
      ));
      _refreshUnityView();
    });
  }

  void _processEvents(UnityWebEvent e) {
    switch (e.name) {
      case 'onUnityMessage':
        this.onUnityMessage(e.data);
        break;
      case 'onUnitySceneLoaded':
        this.onUnitySceneChanged(e.data);
        break;
    }
  }

  void _refreshUnityView() {
    html.IFrameElement? frame = (html.document
        .querySelector('flt-platform-view')!
        .querySelector('iframe')! as html.IFrameElement);
    frame.focus();
  }

  Future<dynamic> handleMessages(MethodCall call) {
    switch (call.method) {
      case "unity#waitForUnity":
        return Future.value(null);
      case "unity#dispose":
        dispose();
        return Future.value(null);
      case "unity#postMessage":
        messageUnity(
          gameObject: call.arguments['gameObject'],
          methodName: call.arguments['methodName'],
          message: call.arguments['message'],
        );
        return Future.value(null);
      case "unity#resumePlayer":
        callUnityFn(fnName: 'resume');
        return Future.value(null);
      case "unity#pausePlayer":
        callUnityFn(fnName: 'pause');
        return Future.value(null);
      case "unity#unloadPlayer":
        callUnityFn(fnName: 'unload');
        return Future.value(null);
      case "unity#quitPlayer":
        callUnityFn(fnName: 'quit');
        return Future.value(null);
      default:
        throw UnimplementedError("Unimplemented ${call.method} method");
    }
  }

  void callUnityFn({required String fnName}) {
    _unityFlutterBidingFn = html.MessageEvent(
      'unityFlutterBidingFnCal',
      data: fnName,
    );
    html.window.dispatchEvent(_unityFlutterBidingFn);
    _refreshUnityView();
  }

  void messageUnity({
    required String gameObject,
    required String methodName,
    required String message,
  }) {
    _unityFlutterBiding = html.MessageEvent(
      'unityFlutterBiding',
      data: json.encode({
        "gameObject": gameObject,
        "method": methodName,
        "message": message,
      }),
    );
    html.window.dispatchEvent(_unityFlutterBiding);
    _refreshUnityView();
  }

  void dispose() {
    html.window.removeEventListener('message', (_) {});
    html.window.removeEventListener('unityFlutterBiding', (event) {});
    html.window.removeEventListener('unityFlutterBidingFnCal', (event) {});
  }
}
