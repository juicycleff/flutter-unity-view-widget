import 'package:flutter/material.dart';

class WindowsUnityWidgetView extends StatefulWidget {
  const WindowsUnityWidgetView({super.key});

  @override
  State<WindowsUnityWidgetView> createState() => _WindowsUnityWidgetViewState();
}

class _WindowsUnityWidgetViewState extends State<WindowsUnityWidgetView> {
  @override
  Widget build(BuildContext context) {
    // TODO: Rex Update windows view
    return const MouseRegion(
      child: Texture(
        textureId: 0,
      ),
    );
  }
}
