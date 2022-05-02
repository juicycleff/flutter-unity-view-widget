import 'package:flutter/material.dart';

class WindowsUnityWidgetView extends StatefulWidget {
  const WindowsUnityWidgetView({Key? key}) : super(key: key);

  @override
  State<WindowsUnityWidgetView> createState() => _WindowsUnityWidgetViewState();
}

class _WindowsUnityWidgetViewState extends State<WindowsUnityWidgetView> {
  @override
  Widget build(BuildContext context) {
    return MouseRegion(
      //  cursor: _cursor,
      child: Texture(
        textureId: 0,
      ),
    );
  }
}
