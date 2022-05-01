part of flutter_unity_widget;

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

  UnityWebControllerInterceptor? _webControllerInterceptor;
  static Registrar? webRegistrar;

  /// Accesses the MethodChannel associated to the passed unityId.
  MethodChannel channel(int unityId) {
    MethodChannel? channel = _channels[unityId];
    if (channel == null) {
      throw UnknownUnityIDError(unityId);
    }
    return channel;
  }

  MethodChannel ensureChannelInitialized(int unityId) {
    MethodChannel? channel = _channels[unityId];
    if (channel == null) {
      channel = kIsWeb
          ? MethodChannel(
              'plugin.xraph.com/unity_view_$unityId',
              const StandardMethodCodec(),
              webRegistrar,
            )
          : MethodChannel('plugin.xraph.com/unity_view_$unityId');

      if (kIsWeb) {
        channel.setMethodCallHandler(_webControllerInterceptor?.handleMessages);
      } else {
        channel.setMethodCallHandler(
            (MethodCall call) => _handleMethodCall(call, unityId));
      }
      _channels[unityId] = channel;
    }
    return channel;
  }

  /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  @override
  Future<void> init(int unityId) {
    _webControllerInterceptor = UnityWebControllerInterceptor(
      onUnityReady: () {
        _unityStreamController.add(UnityCreatedEvent(unityId, {}));
      },
      onUnitySceneChanged: (data) {
        _unityStreamController
            .add(UnitySceneLoadedEvent(unityId, SceneLoaded.fromMap(data)));
      },
      onUnityMessage: (String data) {
        _unityStreamController.add(UnityMessageEvent(unityId, data));
      },
    );
    MethodChannel channel = ensureChannelInitialized(unityId);
    return channel.invokeMethod<void>('unity#waitForUnity');
  }

  /// Dispose of the native resources.
  @override
  Future<void> dispose({int? unityId}) async {
    try {
      if (unityId != null) await channel(unityId).invokeMethod('unity#dispose');
    } catch (e) {
      // ignore
    }
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
  Future<bool?> isPaused({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#isPaused');
  }

  @override
  Future<bool?> isReady({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#isReady');
  }

  @override
  Future<bool?> isLoaded({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#isLoaded');
  }

  @override
  Future<bool?> inBackground({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#inBackground');
  }

  @override
  Future<bool?> createUnityPlayer({required int unityId}) async {
    return await channel(unityId).invokeMethod('unity#createPlayer');
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

    if (kIsWeb) {
      return UnityWebWidget(
        unitySrcUrl: unitySrcUrl ?? '',
        onWebViewCreated: (_) {
          onPlatformViewCreated(0);
        },
      );
    }

    if (defaultTargetPlatform == TargetPlatform.android) {
      if (!useAndroidViewSurface) {
        return AndroidView(
          viewType: _viewType,
          onPlatformViewCreated: onPlatformViewCreated,
          gestureRecognizers: gestureRecognizers,
          creationParams: creationParams,
          creationParamsCodec: const StandardMessageCodec(),
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
          final SurfaceAndroidViewController controller =
              PlatformViewsService.initSurfaceAndroidView(
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
    await channel(unityId).invokeMethod('unity#pausePlayer');
  }

  @override
  Future<void> resumePlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#resumePlayer');
  }

  @override
  Future<void> openInNativeProcess({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#openInNativeProcess');
  }

  @override
  Future<void> unloadPlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#unloadPlayer');
  }

  @override
  Future<void> quitPlayer({required int unityId}) async {
    await channel(unityId).invokeMethod('unity#quitPlayer');
  }
}
