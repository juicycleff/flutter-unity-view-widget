import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:pointer_interceptor/pointer_interceptor.dart';

class ApiScreen extends StatefulWidget {
  ApiScreen({Key key}) : super(key: key);

  @override
  _ApiScreenState createState() => _ApiScreenState();
}

class _ApiScreenState extends State<ApiScreen> {
  // UnityWidgetController _unityWidgetController;
  double _sliderValue = 0.0;

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    GlobalUnityController.instance.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('API Screen'),
      ),
      body: Card(
        margin: const EdgeInsets.all(8),
        clipBehavior: Clip.antiAlias,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.0),
        ),
        child: Stack(
          children: [
            Container(
              child: UnityWidget(
                onUnityCreated: onUnityCreated,
                onUnityMessage: onUnityMessage,
                onUnitySceneLoaded: onUnitySceneLoaded,
                fullscreen: false,
                useAndroidViewSurface: false,
              ),
            ),
            PointerInterceptor(
              child: Positioned(
                bottom: 20,
                left: 20,
                right: 20,
                child: Card(
                  elevation: 10,
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: <Widget>[
                      Padding(
                        padding: const EdgeInsets.only(top: 20),
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
                                GlobalUnityController.instance.quit();
                              },
                              child: Text("Quit"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                GlobalUnityController.instance.create();
                              },
                              child: Text("Create"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                GlobalUnityController.instance.pause();
                              },
                              child: Text("Pause"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                GlobalUnityController.instance.resume();
                              },
                              child: Text("Resume"),
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
                                await GlobalUnityController.instance
                                    .openInNativeProcess();
                              },
                              child: Text("Open Native"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                GlobalUnityController.instance.unload();
                              },
                              child: Text("Unload"),
                            ),
                            MaterialButton(
                              onPressed: () {
                                GlobalUnityController.instance.quit();
                              },
                              child: Text("Silent Quit"),
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
    GlobalUnityController.instance.postMessage(
      'Cube',
      'SetRotationSpeed',
      speed,
    );
  }

  void onUnityMessage(message) {
    print('Received message from unity: ${message.toString()}');
  }

  void onUnitySceneLoaded(SceneLoaded scene) {
    print('Received scene loaded from unity: ${scene.name}');
    print('Received scene loaded from unity buildIndex: ${scene.buildIndex}');
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    // this._unityWidgetController = controller;
  }
}
