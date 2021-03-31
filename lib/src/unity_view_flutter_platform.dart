part of flutter_unity_widget;

abstract class UnityViewFlutterPlatform extends PlatformInterface {
  /// Constructs a UnityViewFlutterPlatform.
  UnityViewFlutterPlatform() : super(token: _token);

  static final Object _token = Object();

  static UnityViewFlutterPlatform _instance = MethodChannelUnityViewFlutter();

  /// The default instance of [UnityViewFlutterPlatform] to use.
  ///
  /// Defaults to [MethodChannelUnityViewFlutter].
  static UnityViewFlutterPlatform get instance => _instance;

  /// Platform-specific plugins should set this with their own platform-specific
  /// class that extends [UnityViewFlutterPlatform] when they register themselves.
  static set instance(UnityViewFlutterPlatform instance) {
    PlatformInterface.verifyToken(instance, _token);
    _instance = instance;
  }

  /// /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  Future<void> init(int unityId) {
    throw UnimplementedError('init() has not been implemented.');
  }

  Future<bool?> isReady({required int unityId}) async {
    throw UnimplementedError('init() has not been implemented.');
  }

  Future<bool?> isPaused({required int unityId}) async {
    throw UnimplementedError('isPaused() has not been implemented.');
  }

  Future<bool?> isLoaded({required int unityId}) async {
    throw UnimplementedError('isLoaded() has not been implemented.');
  }

  Future<bool?> inBackground({required int unityId}) async {
    throw UnimplementedError('inBackground() has not been implemented.');
  }

  Future<bool?> createUnityPlayer({required int unityId}) async {
    throw UnimplementedError('createUnityPlayer() has not been implemented.');
  }

  Future<void> postMessage(
      {required int unityId,
      required String gameObject,
      required String methodName,
      required String message}) {
    throw UnimplementedError('postMessage() has not been implemented.');
  }

  Future<void> postJsonMessage(
      {required int unityId,
      required String gameObject,
      required String methodName,
      required Map message}) {
    throw UnimplementedError('postJsonMessage() has not been implemented.');
  }

  Future<void> pausePlayer({required int unityId}) async {
    throw UnimplementedError('pausePlayer() has not been implemented.');
  }

  Future<void> resumePlayer({required int unityId}) async {
    throw UnimplementedError('resumePlayer() has not been implemented.');
  }

  /// Opens unity in it's own activity. Android only.
  Future<void> openInNativeProcess({required int unityId}) async {
    throw UnimplementedError('openInNativeProcess() has not been implemented.');
  }

  Future<void> unloadPlayer({required int unityId}) async {
    throw UnimplementedError('unloadPlayer() has not been implemented.');
  }

  Future<void> quitPlayer({required int unityId}) async {
    throw UnimplementedError('quitPlayer() has not been implemented.');
  }

  Stream<UnityMessageEvent> onUnityMessage({required int unityId}) {
    throw UnimplementedError('onUnityMessage() has not been implemented.');
  }

  Stream<UnityLoadedEvent> onUnityUnloaded({required int unityId}) {
    throw UnimplementedError('onUnityUnloaded() has not been implemented.');
  }

  Stream<UnityCreatedEvent> onUnityCreated({required int unityId}) {
    throw UnimplementedError('onUnityUnloaded() has not been implemented.');
  }

  Stream<UnitySceneLoadedEvent> onUnitySceneLoaded({required int unityId}) {
    throw UnimplementedError('onUnitySceneLoaded() has not been implemented.');
  }

  /// Dispose of whatever resources the `unityId` is holding on to.
  void dispose({required int unityId}) {
    throw UnimplementedError('dispose() has not been implemented.');
  }

  /// Returns a widget displaying the map view
  Widget buildView(
      Map<String, dynamic> creationParams,
      Set<Factory<OneSequenceGestureRecognizer>>? gestureRecognizers,
      PlatformViewCreatedCallback onPlatformViewCreated,
      bool useAndroidView) {
    throw UnimplementedError('buildView() has not been implemented.');
  }
}
