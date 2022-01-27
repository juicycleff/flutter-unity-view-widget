part of flutter_unity_widget;

// This counter is used to provide a stable "constant" initialization id
// to the buildView function, so the web implementation can use it as a
// cache key. This needs to be provided from the outside, because web
// views seem to re-render much more often that mobile platform views.
int _nextUnityCreationId = 0;

/// Android specific settings for [UnityWidget].
class AndroidUnityWidgetFlutter {
  /// Whether to render [UnityWidget] with a [AndroidViewSurface] to build the Flutter Unity widget.
  ///
  /// This implementation uses hybrid composition to render the Flutter Unity
  /// Widget on Android. This comes at the cost of some performance on Android
  /// versions below 10. See
  /// https://flutter.dev/docs/development/platform-integration/platform-views#performance for more
  /// information.
  ///
  /// Defaults to true.
  static bool get useAndroidViewSurface {
    final UnityWidgetFlutterPlatform platform =
        UnityWidgetFlutterPlatform.instance;
    if (platform is MethodChannelUnityWidgetFlutter) {
      return platform.useAndroidViewSurface;
    }
    return false;
  }

  /// Set whether to render [UnityWidget] with a [AndroidViewSurface] to build the Flutter Unity widget.
  ///
  /// This implementation uses hybrid composition to render the Google Maps
  /// Widget on Android. This comes at the cost of some performance on Android
  /// versions below 10. See
  /// https://flutter.dev/docs/development/platform-integration/platform-views#performance for more
  /// information.
  ///
  /// Defaults to true.
  static set useAndroidViewSurface(bool useAndroidViewSurface) {
    final UnityWidgetFlutterPlatform platform =
        UnityWidgetFlutterPlatform.instance;
    if (platform is MethodChannelUnityWidgetFlutter) {
      platform.useAndroidViewSurface = useAndroidViewSurface;
    }
  }
}

class UnityWidget extends StatefulWidget {
  UnityWidget({
    Key? key,
    required this.onUnityCreated,
    this.onUnityMessage,
    this.fullscreen = false,
    this.enablePlaceholder = false,
    this.printSeupLog = true,
    this.onUnityUnloaded,
    this.gestureRecognizers,
    this.placeholder,
    this.useAndroidView,
    this.useAndroidViewSurface,
    this.onUnitySceneLoaded,
    this.uiLevel = 1,
    this.borderRadius = BorderRadius.zero,
    this.layoutDirection,
  });

  ///Event fires when the unity player is created.
  final UnityCreatedCallback onUnityCreated;

  ///Event fires when the [UnityWidget] gets a message from unity.
  final UnityMessageCallback? onUnityMessage;

  ///Event fires when the [UnityWidget] gets a scene loaded from unity.
  final UnitySceneChangeCallback? onUnitySceneLoaded;

  ///Event fires when the [UnityWidget] unity player gets unloaded.
  final UnityUnloadCallback? onUnityUnloaded;

  final Set<Factory<OneSequenceGestureRecognizer>>? gestureRecognizers;

  /// Set to true to force unity to fullscreen
  final bool fullscreen;

  /// Controls the layer in which unity widget is rendered in flutter (defaults to 1)
  final int uiLevel;

  /// This flag enables placeholder widget
  final bool enablePlaceholder;

  /// This flag enables placeholder widget
  final bool printSeupLog;

  /// This flag allows you use AndroidView instead of PlatformViewLink for android. Deprecated
  @deprecated
  final bool? useAndroidView;

  /// This flag allows you use AndroidView instead of PlatformViewLink for android
  /// replace [useAndroidView] with [useAndroidViewSurface]
  final bool? useAndroidViewSurface;

  /// This is just a helper to render a placeholder widget
  final Widget? placeholder;

  /// Border radius
  final BorderRadius borderRadius;

  /// The layout direction to use for the embedded view.
  ///
  /// If this is null, the ambient [Directionality] is used instead. If there is
  /// no ambient [Directionality], [TextDirection.ltr] is used.
  final TextDirection? layoutDirection;

  @override
  _UnityWidgetState createState() => _UnityWidgetState();
}

class _UnityWidgetState extends State<UnityWidget> {
  final _unityId = _nextUnityCreationId++;

  final Completer<UnityWidgetController> _controller =
      Completer<UnityWidgetController>();

  @override
  Future<void> dispose() async {
    super.dispose();
    UnityWidgetController controller = await _controller.future;
    controller.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final Map<String, dynamic> unityOptions = <String, dynamic>{
      'fullscreen': widget.fullscreen,
      'uiLevel': widget.uiLevel,
    };

    if (widget.enablePlaceholder) {
      return widget.placeholder ??
          Text('Placeholder mode enabled, no native code will be called');
    }

    return ClipRRect(
      child: UnityWidgetFlutterPlatform.instance.buildViewWithTextDirection(
        _unityId,
        onPlatformViewCreated,
        unityOptions: unityOptions,
        textDirection: widget.layoutDirection ??
            Directionality.maybeOf(context) ??
            TextDirection.ltr,
        gestureRecognizers: widget.gestureRecognizers,
        useAndroidViewSurf: widget.useAndroidViewSurface,
      ),
      borderRadius: widget.borderRadius,
    );
  }

  Future<void> onPlatformViewCreated(int id) async {
    final controller = await UnityWidgetController.init(id, this);
    _controller.complete(controller);
    final UnityCreatedCallback? onUnityCreated = widget.onUnityCreated;
    if (onUnityCreated != null) {
      onUnityCreated(controller);
    }

    if (widget.printSeupLog) {
      log('*********************************************');
      log('** flutter unity controller setup complete **');
      log('*********************************************');
    }
  }
}
