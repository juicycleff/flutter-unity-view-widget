import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

typedef void FlutterUnityWidgetCreatedCallback(UnityWidgetController controller);


class UnityWidgetController {
  static MethodChannel _channel =
  const MethodChannel('flutter_unity_widget');

  UnityWidgetController();

  init(int id) {
    _channel =  new MethodChannel('nativeweb_$id');
  }

  static Future<String> get platformVersion async {
    final String version = await _channel.invokeMethod('getPlatformVersion');
    return version;
  }
}

class UnityWidget extends StatefulWidget {

  UnityWidgetController onUnityViewCreated;

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
        viewType: 'unityview',
        onPlatformViewCreated: onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),

      );
    } else if(defaultTargetPlatform == TargetPlatform.iOS) {
      return UiKitView(
        viewType: 'unityview',
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
    widget.onUnityViewCreated = new UnityWidgetController().init(id);
  }
}

