part of flutter_unity_widget;

class MethodChannelUnityViewFlutter extends UnityViewFlutterPlatform {
  // Every method call passes the int unityId
  final Map<int, MethodChannel> _channels = {};
  final String _viewType = "plugins.xraph.com/unity_view";

  /// Accesses the MethodChannel associated to the passed unityId.
  MethodChannel channel(int unityId) {
    return _channels[unityId];
  }

  /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  @override
  Future<void> init(int unityId) {
    MethodChannel channel;
    if (!_channels.containsKey(unityId)) {
      channel = MethodChannel('plugins.xraph.com/unity_view_$unityId');
      channel.setMethodCallHandler(
          (MethodCall call) => _handleMethodCall(call, unityId));
      _channels[unityId] = channel;
    }
    return channel.invokeMethod<void>('unity#waitForUnity');
  }

  /// Dispose of the native resources.
  @override
  Future<void> dispose({int unityId}) async {
    await channel(unityId).invokeMethod('unity#dispose');
  }

  // The controller we need to broadcast the different events coming
  // from handleMethodCall.
  //
  // It is a `broadcast` because multiple controllers will connect to
  // different stream views of this Controller.
  final StreamController<UnityEvent> _unityStreamController =
      StreamController<UnityEvent>.broadcast();

  // Returns a filtered view of the events in the _controller, by unityId.
  Stream<UnityEvent> _events(int unityId) =>
      _unityStreamController.stream.where((event) => event.unityId == unityId);

  Future<dynamic> _handleMethodCall(MethodCall call, int unityId) async {
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

  @override
  Future<bool> isPaused({@required int unityId}) async {
    final bool loaded = await channel(unityId).invokeMethod('unity#isPaused');
    return loaded;
  }

  @override
  Future<bool> isReady({@required int unityId}) async {
    final bool ready = await channel(unityId).invokeMethod('unity#isReady');
    return ready;
  }

  @override
  Future<bool> isLoaded({@required int unityId}) async {
    final bool loaded = await channel(unityId).invokeMethod('unity#isLoaded');
    return loaded;
  }

  @override
  Future<bool> inBackground({@required int unityId}) async {
    final bool inBg = await channel(unityId).invokeMethod('unity#inBackground');
    return inBg;
  }

  @override
  Future<bool> createUnityPlayer({@required int unityId}) async {
    final bool isReady =
        await channel(unityId).invokeMethod('unity#createUnityplayer');
    return isReady;
  }

  @override
  Stream<UnityMessageEvent> onUnityMessage({@required int unityId}) {
    return _events(unityId).whereType<UnityMessageEvent>();
  }

  @override
  Stream<UnityLoadedEvent> onUnityUnloaded({@required int unityId}) {
    return _events(unityId).whereType<UnityLoadedEvent>();
  }

  @override
  Stream<UnityCreatedEvent> onUnityCreated({@required int unityId}) {
    return _events(unityId).whereType<UnityCreatedEvent>();
  }

  @override
  Stream<UnitySceneLoadedEvent> onUnitySceneLoaded({@required int unityId}) {
    return _events(unityId).whereType<UnitySceneLoadedEvent>();
  }

  @override
  Widget buildView(
    Map<String, dynamic> creationParams,
    Set<Factory<OneSequenceGestureRecognizer>> gestureRecognizers,
    PlatformViewCreatedCallback onPlatformViewCreated,
  ) {
    if (defaultTargetPlatform == TargetPlatform.android) {
      return PlatformViewLink(
        viewType: _viewType,
        surfaceFactory:
            (BuildContext context, PlatformViewController controller) {
          return AndroidViewSurface(
            controller: controller,
            gestureRecognizers: const <Factory<OneSequenceGestureRecognizer>>{},
            hitTestBehavior: PlatformViewHitTestBehavior.opaque,
          );
        },
        onCreatePlatformView: (PlatformViewCreationParams params) {
          return PlatformViewsService.initSurfaceAndroidView(
            id: params.id,
            viewType: _viewType,
            layoutDirection: TextDirection.ltr,
            creationParams: creationParams,
            creationParamsCodec: StandardMessageCodec(),
          )
            ..addOnPlatformViewCreatedListener(params.onPlatformViewCreated)
            ..addOnPlatformViewCreatedListener(onPlatformViewCreated)
            ..create();
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
  Future<void> postMessage(
      {@required int unityId,
      @required String gameObject,
      @required String methodName,
      @required String message}) async {
    await channel(unityId).invokeMethod('unity#postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': message,
    });
  }

  @override
  Future<void> postJsonMessage(
      {@required int unityId,
      @required String gameObject,
      @required String methodName,
      @required Map message}) async {
    await channel(unityId).invokeMethod('unity#postMessage', <String, dynamic>{
      'gameObject': gameObject,
      'methodName': methodName,
      'message': json.encode(message),
    });
  }

  @override
  Future<void> pausePlayer({@required int unityId}) async {
    await channel(unityId).invokeMethod('unity#pausePlayer');
  }

  @override
  Future<void> resumePlayer({@required int unityId}) async {
    await channel(unityId).invokeMethod('unity#resumePlayer');
  }

  @override
  Future<void> openInNativeProcess({@required int unityId}) async {
    await channel(unityId).invokeMethod('unity#openInNativeProcess');
  }

  @override
  Future<void> unloadPlayer({@required int unityId}) async {
    await channel(unityId).invokeMethod('unity#unloadPlayer');
  }

  @override
  Future<void> quitPlayer({@required int unityId, bool silent}) async {
    if (silent) {
      await channel(unityId).invokeMethod('unity#quitPlayer');
    } else {
      await channel(unityId).invokeMethod('unity#silentQuitPlayer');
    }
  }
}
