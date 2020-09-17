part of flutter_unity_widget;

class UnityWidget extends StatefulWidget {
  final UnityWidgetCreatedCallback onUnityViewCreated;

  ///Event fires when the [UnityWidget] gets a message from unity.
  final onUnityMessageCallback onUnityMessage;

  ///Event fires when the [UnityWidget] gets a scene loaded from unity.
  final onUnitySceneChangeCallback onUnitySceneLoaded;

  ///Event fires when the [UnityWidget] gets a message from unity.
  final onUnityUnloadCallback onUnityUnloaded;

  final Set<Factory<OneSequenceGestureRecognizer>> gestureRecognizers;
  final bool isARScene;
  final bool safeMode;
  final bool fullscreen;
  final bool enablePlaceholder;
  final bool disableUnload;
  final Widget placeholder;

  UnityWidget({
    Key key,
    @required this.onUnityViewCreated,
    this.onUnityMessage,
    this.isARScene = false,
    this.safeMode = false,
    this.fullscreen = false,
    this.enablePlaceholder = false,
    this.disableUnload = false,
    this.onUnityUnloaded,
    this.gestureRecognizers,
    this.placeholder,
    this.onUnitySceneLoaded,
  });

  @override
  _UnityWidgetState createState() => _UnityWidgetState();
}

class _UnityWidgetState extends State<UnityWidget> {
  final String _viewType = "plugins.xraph.com/unity_view";
  UnityWidgetController _controller;

  @override
  void initState() {
    super.initState();
  }

  @override
  void deactivate() {
    super.deactivate();
  }

  @override
  void dispose() {
    super.dispose();
    if (_controller != null) {
      _controller._dispose();
      _controller = null;
    }
  }

  createUnity() async {
    if (!widget.enablePlaceholder) {
      await _controller.createUnity();
      await _controller.resume();
    }
  }

  unloadUnity() async {
    if (!widget.enablePlaceholder) {
      await _controller.unload();
    }
  }

  @override
  Widget build(BuildContext context) {
    final Map<String, dynamic> creationParams = <String, dynamic>{
      'ar': widget.isARScene,
      'safeMode': widget.safeMode,
      'fullscreen': widget.fullscreen,
      'disableUnload': widget.disableUnload,
    };

    if (widget.enablePlaceholder) {
      return widget.placeholder ??
          Text('Placeholder mode enabled, no native code will be called');
    }

    if (defaultTargetPlatform == TargetPlatform.android) {
      return AndroidView(
        viewType: _viewType,
        onPlatformViewCreated: _onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),
        creationParams: creationParams,
        gestureRecognizers: widget.gestureRecognizers,
      );
    } else if (defaultTargetPlatform == TargetPlatform.iOS) {
      return UiKitView(
        viewType: _viewType,
        onPlatformViewCreated: _onPlatformViewCreated,
        creationParamsCodec: const StandardMessageCodec(),
        creationParams: creationParams,
        gestureRecognizers: widget.gestureRecognizers,
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
    _controller = UnityWidgetController.init(id, this);
    if (widget.onUnityViewCreated != null) {
      widget.onUnityViewCreated(_controller);
    }
    print('*********************************************');
    print('** flutter unity controller setup complete **');
    print('*********************************************');
  }
}
