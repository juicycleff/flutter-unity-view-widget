import 'package:flutter/material.dart';
import 'package:webview_flutter_platform_interface/webview_flutter_platform_interface.dart';
// ignore: unused_import
import 'package:webview_flutter_web/webview_flutter_web.dart'; // used indirectly through webview_flutter_platform_interface

class WebUnityWidgetView extends StatefulWidget {
  const WebUnityWidgetView({
    super.key,
    required this.onWebViewCreated,
    required this.unityOptions,
  });

  final Map<String, dynamic> unityOptions;
  final void Function() onWebViewCreated;

  @override
  State<WebUnityWidgetView> createState() => _WebUnityWidgetViewState();
}

class _WebUnityWidgetViewState extends State<WebUnityWidgetView> {
  final PlatformWebViewController _controller = PlatformWebViewController(
    const PlatformWebViewControllerCreationParams(),
  )..loadRequest(
      LoadRequestParams(
        uri: Uri.parse('${_getBasePath()}/UnityLibrary/index.html'),
      ),
    );

  @override
  void initState() {
    super.initState();
    widget.onWebViewCreated();
  }

  @override
  void dispose() {
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return PlatformWebViewWidget(
      PlatformWebViewWidgetCreationParams(controller: _controller),
    ).build(context);
  }

  static String _getBasePath() {
    var prefix = Uri.base.origin + Uri.base.path;
    if (prefix.endsWith("/")) prefix = prefix.substring(0, prefix.length - 1);
    return prefix;
  }
}
