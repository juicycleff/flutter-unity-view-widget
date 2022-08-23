import 'package:flutter/material.dart';
import 'package:webviewx/webviewx.dart';

class WebUnityWidgetView extends StatefulWidget {
  const WebUnityWidgetView({
    Key? key,
    required this.onWebViewCreated,
    required this.unityOptions,
  }) : super(key: key);

  final Map<String, dynamic> unityOptions;
  final void Function() onWebViewCreated;

  @override
  State<WebUnityWidgetView> createState() => _WebUnityWidgetViewState();
}

class _WebUnityWidgetViewState extends State<WebUnityWidgetView> {
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
    var path = Uri.base.path == '/' ? '' : Uri.base.path;
    var pathConnector = Uri.base.path.endsWith('/') ? '' : '/';
    print('Uri.base.origin ${Uri.base.origin}');
    print('Uri.base.path ${Uri.base.path}');
    print('path $path');
    print('pathConnector $pathConnector');
    return WebViewX(
      initialContent: '${Uri.base.origin}${path}${pathConnector}UnityLibrary/index.html',
      initialSourceType: SourceType.url,
      javascriptMode: JavascriptMode.unrestricted,
      onWebViewCreated: (_) {},
      height: MediaQuery.of(context).size.height,
      width: MediaQuery.of(context).size.width,
    );
  }
}
