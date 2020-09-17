part of flutter_unity_widget;

typedef void UnityWidgetCreatedCallback(UnityWidgetController controller);

class UnityWidgetController {
  final _UnityWidgetState _unityWidgetState;
  final MethodChannel channel;

  UnityWidgetController._(
    this.channel,
    this._unityWidgetState,
  ) {
    channel.setMethodCallHandler(_handleMethod);
  }

  static UnityWidgetController init(
      int id, _UnityWidgetState unityWidgetState) {
    final MethodChannel channel =
        MethodChannel('plugins.xraph.com/unity_view_$id');
    return UnityWidgetController._(
      channel,
      unityWidgetState,
    );
  }

  Future<bool> isReady() async {
    final bool isReady = await channel.invokeMethod('isReady');
    return isReady;
  }

  Future<bool> isPaused() async {
    final bool isReady = await channel.invokeMethod('isPaused');
    return isReady;
  }

  Future<bool> isLoaded() async {
    final bool isReady = await channel.invokeMethod('isLoaded');
    return isReady;
  }

  Future<bool> isInBackground() async {
    final bool isReady = await channel.invokeMethod('isInBackground');
    return isReady;
  }

  Future<bool> createUnity() async {
    final bool isReady = await channel.invokeMethod('createUnity');
    return isReady;
  }

  postMessage(String gameObject, methodName, message) {
    channel.invokeMethod('postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': message,
    });
  }

  pause() async {
    await channel.invokeMethod('pause');
  }

  resume() async {
    await channel.invokeMethod('resume');
  }

  /// Opens unity in it's own activity. Android only.
  openNative() async {
    await channel.invokeMethod('openNative');
  }

  unload() async {
    await channel.invokeMethod('unload');
  }

  quitPlayer() async {
    await channel.invokeMethod('quitPlayer');
  }

  silentQuitPlayer() async {
    await channel.invokeMethod('silentQuitPlayer');
  }

  Future<void> _dispose() async {
    await channel.invokeMethod('dispose');
  }

  Future<dynamic> _handleMethod(MethodCall call) async {
    switch (call.method) {
      case "onUnityMessage":
        if (_unityWidgetState.widget != null) {
          _unityWidgetState.widget.onUnityMessage(this, call.arguments);
        }
        break;
      case "onUnityUnloaded":
        if (_unityWidgetState.widget != null) {
          _unityWidgetState.widget.onUnityUnloaded(this);
        }
        break;
      case "onUnitySceneLoaded":
        if (_unityWidgetState.widget != null) {
          _unityWidgetState.widget.onUnitySceneLoaded(
            this,
            name: call.arguments['name'],
            buildIndex: call.arguments['buildIndex'],
            isLoaded: call.arguments['isLoaded'],
            isValid: call.arguments['isValid'],
          );
        }
        break;
      default:
        throw UnimplementedError("Unimplemented ${call.method} method");
    }
  }
}

typedef onUnityMessageCallback = void Function(
    UnityWidgetController controller, dynamic handler);

typedef onUnitySceneChangeCallback = void Function(
  UnityWidgetController controller, {
  String name,
  int buildIndex,
  bool isLoaded,
  bool isValid,
});

typedef onUnityUnloadCallback = void Function(UnityWidgetController controller);
