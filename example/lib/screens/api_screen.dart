import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:flutter_unity_widget_example/utils/screen_utils.dart';

class ApiScreen extends StatefulWidget {
  ApiScreen({Key key}) : super(key: key);

  @override
  _ApiScreenState createState() => _ApiScreenState();
}

class _ApiScreenState extends State<ApiScreen> {
  static final GlobalKey<ScaffoldState> _scaffoldKey =
      GlobalKey<ScaffoldState>();
  UnityWidgetController _unityWidgetController;
  double _sliderValue = 0.0;

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    final ScreenArguments arguments =
        ModalRoute.of(context).settings.arguments as ScreenArguments;

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
            Expanded(
              child: Container(
                child: UnityWidget(
                  onUnityViewCreated: onUnityCreated,
                  isARScene: arguments.enableAR,
                  onUnityMessage: onUnityMessage,
                  onUnitySceneLoaded: onUnitySceneLoaded,
                  fullscreen: false,
                ),
              ),
            ),
            Positioned(
              bottom: 20,
              left: 20,
              right: 20,
              child: Card(
                elevation: 10,
                child: Column(
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
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.quitPlayer();
                          },
                          child: Text("Quit"),
                        ),
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.createUnity();
                          },
                          child: Text("Create"),
                        ),
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.pause();
                          },
                          child: Text("Pause"),
                        ),
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.resume();
                          },
                          child: Text("Resume"),
                        ),
                      ],
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.openNative();
                          },
                          child: Text("Open Native"),
                        ),
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.unload();
                          },
                          child: Text("Unload"),
                        ),
                        MaterialButton(
                          onPressed: () {
                            _unityWidgetController.silentQuitPlayer();
                          },
                          child: Text("Silent Quit"),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  void setRotationSpeed(String speed) {
    _unityWidgetController.postMessage(
      'Cube',
      'SetRotationSpeed',
      speed,
    );
  }

  void onUnityMessage(controller, message) {
    print('Received message from unity: ${message.toString()}');
  }

  void onUnitySceneLoaded(
    controller, {
    int buildIndex,
    bool isLoaded,
    bool isValid,
    String name,
  }) {
    print('Received scene loaded from unity: $name');
    print('Received scene loaded from unity buildIndex: $buildIndex');
  }

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    this._unityWidgetController = controller;
  }
}
