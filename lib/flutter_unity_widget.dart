import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

typedef void UnityWidgetCreatedCallback(UnityWidgetController controller);


class UnityWidgetController {
  static MethodChannel _channel =
  const MethodChannel('unity_view');

  UnityWidgetController();

  init(int id) {
    _channel =  new MethodChannel('unity_view');
  }

  Future<bool> isReady() async {
    final bool isReady = await _channel.invokeMethod('isReady');
    return isReady;
  }

  Future<bool> createUnity() async {
    final bool isReady = await _channel.invokeMethod('createUnity');
    return isReady;
  }


  postMessage(String gameObject, methodName, message){
    _channel.invokeMethod('postMessage', [gameObject, methodName, message]);
  }

  pause() async{
    await _channel.invokeMethod('pause');
  }

  resume() async{
    await _channel.invokeMethod('resume');
  }
}

class UnityWidget extends StatefulWidget {

  UnityWidgetCreatedCallback onUnityViewCreated;

  UnityWidget({
    Key key,
    @required this.onUnityViewCreated,
  });

  @override
  _UnityWidgetState createState() => _UnityWidgetState();
}

class _UnityWidgetState extends State<UnityWidget> {
  @override
  Widget build(BuildContext context) {
    if(defaultTargetPlatform == TargetPlatform.android) {
      return AndroidView(
        viewType: 'unity_view',
        onPlatformViewCreated: onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),

      );
    } else if(defaultTargetPlatform == TargetPlatform.iOS) {
      return UiKitView(
        viewType: 'unity_view',
        onPlatformViewCreated: onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),
      );
    }

    return new Text('$defaultTargetPlatform is not yet supported by this plugin');
  }

  Future<void> onPlatformViewCreated(id) async {
    if (widget.onUnityViewCreated == null) {
      return;
    }
    widget.onUnityViewCreated(new UnityWidgetController().init(id));
  }
}

