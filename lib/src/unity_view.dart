part of flutter_unity_widget;

class UnityWidget extends StatefulWidget {
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

  /// This flag enables placeholder widget
  final bool enablePlaceholder;

  /// This flag allows you use AndroidView instead of PlatformViewLink for android
  final bool useAndroidView;

  /// This is just a helper to render a placeholder widget
  final Widget? placeholder;

  UnityWidget({
    Key? key,
    required this.onUnityCreated,
    this.onUnityMessage,
    this.fullscreen = false,
    this.enablePlaceholder = false,
    this.onUnityUnloaded,
    this.gestureRecognizers,
    this.placeholder,
    this.useAndroidView = false,
    this.onUnitySceneLoaded,
  });

  @override
  _UnityWidgetState createState() => _UnityWidgetState();
}

class _UnityWidgetState extends State<UnityWidget> {
  final Completer<UnityWidgetController> _controller =
      Completer<UnityWidgetController>();

  @override
  void initState() {
    super.initState();
  }

  @override
  Future<void> dispose() async {
    super.dispose();
    UnityWidgetController controller = await _controller.future;
    controller.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final Map<String, dynamic> creationParams = <String, dynamic>{
      'fullscreen': widget.fullscreen,
    };

    if (widget.enablePlaceholder) {
      return widget.placeholder ??
          Text('Placeholder mode enabled, no native code will be called');
    }

    return _unityViewFlutterPlatform.buildView(
        creationParams,
        widget.gestureRecognizers,
        onPlatformViewCreated,
        widget.useAndroidView);
  }

  Future<void> onPlatformViewCreated(int id) async {
    final controller = await UnityWidgetController.init(id, this);
    _controller.complete(controller);
    widget.onUnityCreated(controller);
    print('*********************************************');
    print('** flutter unity controller setup complete **');
    print('*********************************************');
  }
}
