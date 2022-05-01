part of flutter_unity_widget;

class UnityWebWidget extends StatefulWidget {
  const UnityWebWidget({
    Key? key,
    required this.unitySrcUrl,
    required this.onWebViewCreated,
  }) : super(key: key);

  /// Unity export sorce path, can be hosted or local
  final String unitySrcUrl;

  final Function(WebViewXController controller) onWebViewCreated;

  @override
  State<UnityWebWidget> createState() => _UnityWebWidgetState();
}

class _UnityWebWidgetState extends State<UnityWebWidget> {
  @override
  Widget build(BuildContext context) {
    return WebViewX(
      initialContent: widget.unitySrcUrl,
      javascriptMode: JavascriptMode.unrestricted,
      initialSourceType: SourceType.url,
      onWebViewCreated: widget.onWebViewCreated,
      height: MediaQuery.of(context).size.height,
      width: MediaQuery.of(context).size.width,
    );
  }
}
