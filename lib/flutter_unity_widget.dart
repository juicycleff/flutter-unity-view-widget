import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

typedef void UnityWidgetCreatedCallback(UnityWidgetController controller);

class UnityWidgetController {
  UnityWidget _widget;
  int _id;
  static MethodChannel _channel = const MethodChannel('unity_view');

  UnityWidgetController(int id, UnityWidget widget) {
    _id = id;
    _channel.setMethodCallHandler(_handleMethod);
    _channel = new MethodChannel('unity_view_$id');
    _channel.setMethodCallHandler(_handleMethod);
    print('********************************************');
    print('Controller setup complete');
    print('********************************************');
  }

  init(int id) {
    _id = id;
    _channel = new MethodChannel('unity_view_$id');
    _channel.setMethodCallHandler(_handleMethod);
    print('********************************************');
    print('Controller setup complete');
    print('********************************************');
  }

  Future<bool> isReady() async {
    final bool isReady = await _channel.invokeMethod('isReady');
    return isReady;
  }

  Future<bool> createUnity() async {
    final bool isReady = await _channel.invokeMethod('createUnity');
    return isReady;
  }

  postMessage(String gameObject, methodName, message) {
    _channel.invokeMethod('postMessage', [gameObject, methodName, message]);
  }

  pause() async {
    print('Pressed paused');
    await _channel.invokeMethod('pause');
  }

  resume() async {
    await _channel.invokeMethod('resume');
  }

  Future<void> _dispose() async {
    await _channel.invokeMethod('dispose');
  }

  Future<dynamic> _handleMethod(MethodCall call) async {
    switch (call.method) {
      case "onUnityMessage":
        dynamic handler = call.arguments["handler"];
        if (_widget != null) _widget.onUnityMessage(this, handler);
        break;
      default:
        throw UnimplementedError("Unimplemented ${call.method} method");
    }
  }
}

typedef onUnityMessageCallback = void Function(
    UnityWidgetController controller, dynamic handler);

class UnityWidget extends StatefulWidget {
  final UnityWidgetCreatedCallback onUnityViewCreated;

  ///Event fires when the [UnityWidget] gets a message from unity.
  final onUnityMessageCallback onUnityMessage;


  UnityWidget(
      {Key key, @required this.onUnityViewCreated, this.onUnityMessage});

  @override
  _UnityWidgetState createState() => _UnityWidgetState();
}

class _UnityWidgetState extends State<UnityWidget> {
  
  UnityWidgetController _controller;

  @override
  void initState() {
    // widget.controller = 

    super.initState();
  }

@override
  void dispose() {
    super.dispose();
    if (_controller != null) {
      _controller._dispose();
      _controller = null;
    }
  }

  @override
  Widget build(BuildContext context) {
    if (defaultTargetPlatform == TargetPlatform.android) {
      return AndroidView(
        viewType: 'unity_view',
        onPlatformViewCreated: _onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),
      );
    } else if (defaultTargetPlatform == TargetPlatform.iOS) {
      return UiKitView(
        viewType: 'unity_view',
        onPlatformViewCreated: _onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),
      );
    }

    return new Text(
        '$defaultTargetPlatform is not yet supported by this plugin');
  }

  @override
  void didUpdateWidget(UnityWidget oldWidget) {
    super.didUpdateWidget(oldWidget);
  }

  void _onPlatformViewCreated(int id) {
    _controller = UnityWidgetController(id, widget);
    if (widget.onUnityViewCreated != null) {
      widget.onUnityViewCreated(_controller);
    }
  }
}
