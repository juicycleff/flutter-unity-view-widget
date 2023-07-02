// ignore_for_file: avoid_print

import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:pointer_interceptor/pointer_interceptor.dart';

class ApiScreen extends StatefulWidget {
  const ApiScreen({Key? key}) : super(key: key);

  @override
  State<ApiScreen> createState() => _ApiScreenState();
}

class _ApiScreenState extends State<ApiScreen> {
  UnityWidgetController? _unityWidgetController;
  double _sliderValue = 0.0;

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    _unityWidgetController?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('API Screen'),
      ),
      body: Card(
        margin: const EdgeInsets.all(8),
        clipBehavior: Clip.antiAlias,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.0),
        ),
        child: Stack(
          children: [
            UnityWidget(
              onUnityCreated: onUnityCreated,
              onUnityMessage: onUnityMessage,
              onUnitySceneLoaded: onUnitySceneLoaded,
              fullscreen: false,
              useAndroidViewSurface: false,
            ),
            Positioned(
              bottom: 20,
              left: 20,
              right: 20,
              child: PointerInterceptor(
                child: Card(
                  elevation: 10,
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
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
                          setRotationSpeed(value.toString());
                        },
                        value: _sliderValue,
                        min: 0,
                        max: 20,
                      ),
                      FittedBox(
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            MaterialButton(
                              onPressed: () {
                                _unityWidgetController?.quit();
                              },
                              child: const Text("Quit"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                _unityWidgetController?.create();
                              },
                              child: const Text("Create"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                _unityWidgetController?.pause();
                              },
                              child: const Text("Pause"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                _unityWidgetController?.resume();
                              },
                              child: const Text("Resume"),
                            ),
                          ],
                        ),
                      ),
                      FittedBox(
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            MaterialButton(
                              onPressed: () async {
                                await _unityWidgetController
                                    ?.openInNativeProcess();
                              },
                              child: const Text("Open Native"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                _unityWidgetController?.unload();
                              },
                              child: const Text("Unload"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                _unityWidgetController?.quit();
                              },
                              child: const Text("Silent Quit"),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  void setRotationSpeed(String speed) {
    _unityWidgetController?.postMessage(
      'Cube',
      'SetRotationSpeed',
      speed,
    );
  }

  void onUnityMessage(message) {
    print('Received message from unity: ${message.toString()}');
  }

  void onUnitySceneLoaded(SceneLoaded? scene) {
    if (scene != null) {
      print('Received scene loaded from unity: ${scene.name}');
      print('Received scene loaded from unity buildIndex: ${scene.buildIndex}');
    } else {
      print('Received scene loaded from unity: null');
    }
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    _unityWidgetController = controller;
  }
}
