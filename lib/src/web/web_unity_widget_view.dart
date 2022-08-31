import 'package:flutter/material.dart';
import 'package:webview_flutter/webview_flutter.dart';

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
    return SizedBox(
      height: MediaQuery.of(context).size.height,
      width: MediaQuery.of(context).size.width,
      child: WebView(
        initialUrl: '${Uri.base.origin}/UnityLibrary/index.html',
        javascriptMode: JavascriptMode.unrestricted,
        onPageFinished: (_) {},
        onWebViewCreated: (controller) {},
      ),
    );
  }
}
