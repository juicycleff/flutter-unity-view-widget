# Example



This example **requires** you to first follow the Readme setup and make an export in Unity.  
An example Unity project can be found in `example/unity/DemoApp`.

For Android and iOS we recommended to run this on a real device. Emulator support is very limited.

## Flutter


```dart
import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

void main() {
  runApp(
    const MaterialApp(
      home: UnityDemoScreen(),
    ),
  );
}

class UnityDemoScreen extends StatefulWidget {
  const UnityDemoScreen({Key? key}) : super(key: key);

  @override
  State<UnityDemoScreen> createState() => _UnityDemoScreenState();
}

class _UnityDemoScreenState extends State<UnityDemoScreen> {
  UnityWidgetController? _unityWidgetController;
  double _sliderValue = 0.0;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Unity Flutter Demo'),
      ),
      body: Stack(
        children: <Widget>[
        // This plugin's widget.
          UnityWidget(
            onUnityCreated: onUnityCreated,
            onUnityMessage: onUnityMessage,
            onUnitySceneLoaded: onUnitySceneLoaded,
          ),

          // Flutter UI Stacked on top of Unity to demo Flutter -> Unity interactions.
          // On web this requires a PointerInterceptor widget.
          Positioned(
            bottom: 0,
            // <You need a PointerInterceptor here on web>
            child: SafeArea(
              child: Card(
                elevation: 10,
                child: Column(
                  children: <Widget>[
                    const Padding(
                      padding: EdgeInsets.only(top: 20),
                      child: Text("Rotation speed:"),
                    ),
                    Slider(
                      onChanged: (value) {
                        setState(() {
                          _sliderValue = value;
                        });
                        // Send value to Unity
                        setRotationSpeed(value.toString());
                      },
                      value: _sliderValue,
                      min: 0.0,
                      max: 1.0,
                    ),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(UnityWidgetController controller) {
    _unityWidgetController = controller;
  }

  // Communcation from Flutter to Unity
  void setRotationSpeed(String speed) {
    // Set the rotation speed of a cube in our example Unity project.
    _unityWidgetController?.postMessage(
      'Cube',
      'SetRotationSpeed',
      speed,
    );
  }

  // Communication from Unity to Flutter
  void onUnityMessage(dynamic message) {
    print('Received message from unity: ${message.toString()}');
  }

  // Communication from Unity when new scene is loaded to Flutter
  void onUnitySceneLoaded(SceneLoaded? sceneInfo) {
    if (sceneInfo != null) {
      print('Received scene loaded from unity: ${sceneInfo.name}');
      print(
          'Received scene loaded from unity buildIndex: ${sceneInfo.buildIndex}');
    }
  }
}

```