import 'package:plugin_platform_interface/plugin_platform_interface.dart';

import '../helpers/events.dart';
import '../helpers/types.dart';
import 'flutter_unity_device_method.dart';

abstract class FlutterUnityPlatform extends PlatformInterface {
  /// Constructs a UnityViewFlutterPlatform.
  FlutterUnityPlatform() : super(token: _token);

  static final Object _token = Object();
  static FlutterUnityPlatform _instance = FlutterUnityMethodChannel();

  /// The default instance of [FlutterUnityPlatform] to use.
  ///
  /// Defaults to [MethodChannelUnityWidgetFlutter].
  static FlutterUnityPlatform get instance => _instance;

  /// Platform-specific plugins should set this with their own platform-specific
  /// class that extends [FlutterUnityPlatform] when they register themselves.
  static set instance(FlutterUnityPlatform instance) {
    PlatformInterface.verifyToken(instance, _token);
    _instance = instance;
  }

  var lastUnityId = 0;

  /// /// Initializes the platform interface with [id].
  ///
  /// This method is called when the plugin is first initialized.
  Future<void> init() {
    throw UnimplementedError('init() has not been implemented.');
  }

  Stream<EventDataPayload> get stream {
    throw UnimplementedError('stream() has not been implemented.');
  }

  Future<bool?> isReady() async {
    throw UnimplementedError('init() has not been implemented.');
  }

  Future<bool?> isPaused() async {
    throw UnimplementedError('isPaused() has not been implemented.');
  }

  Future<bool?> isLoaded() async {
    throw UnimplementedError('isLoaded() has not been implemented.');
  }

  Future<bool?> inBackground() async {
    throw UnimplementedError('inBackground() has not been implemented.');
  }

  Future<bool?> createUnityPlayer() async {
    throw UnimplementedError('createUnityPlayer() has not been implemented.');
  }

  Future<void> postMessage({
    required String gameObject,
    required String methodName,
    required String message,
  }) {
    throw UnimplementedError('postMessage() has not been implemented.');
  }

  Future<void> postJsonMessage({
    required String gameObject,
    required String methodName,
    required Map message,
  }) {
    throw UnimplementedError('postJsonMessage() has not been implemented.');
  }

  Future<void> pausePlayer() async {
    throw UnimplementedError('pausePlayer() has not been implemented.');
  }

  Future<void> resumePlayer() async {
    throw UnimplementedError('resumePlayer() has not been implemented.');
  }

  /// Opens unity in it's own activity. Android only.
  Future<void> openInNativeProcess() async {
    throw UnimplementedError('openInNativeProcess() has not been implemented.');
  }

  Future<void> unloadPlayer() async {
    throw UnimplementedError('unloadPlayer() has not been implemented.');
  }

  Future<void> quitPlayer() async {
    throw UnimplementedError('quitPlayer() has not been implemented.');
  }

  Stream<UnityMessageEvent> onUnityMessage() {
    throw UnimplementedError('onUnityMessage() has not been implemented.');
  }

  Stream<UnityLoadedEvent> onUnityUnloaded() {
    throw UnimplementedError('onUnityUnloaded() has not been implemented.');
  }

  Stream<UnityCreatedEvent> onUnityCreated() {
    throw UnimplementedError('onUnityUnloaded() has not been implemented.');
  }

  Stream<UnitySceneLoadedEvent> onUnitySceneLoaded() {
    throw UnimplementedError('onUnitySceneLoaded() has not been implemented.');
  }

  /// Dispose of whatever resources the `unityId` is holding on to.
  Future<void> dispose() {
    throw UnimplementedError('dispose() has not been implemented.');
  }
}
