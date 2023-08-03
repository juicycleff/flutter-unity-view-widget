import 'package:flutter/material.dart';

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
    return Container();
  }
}
