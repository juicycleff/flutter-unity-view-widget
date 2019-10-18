import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';


class WithoutARkitScreen extends StatefulWidget {
  @override
  _WithoutARkitScreenState createState() => _WithoutARkitScreenState();
}

class _WithoutARkitScreenState extends State<WithoutARkitScreen> {

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
    return Scaffold(
      appBar: AppBar(
        title: Text('Standard Demo'),
      ),
      body:Card(
        margin: const EdgeInsets.all(8),
        clipBehavior: Clip.antiAlias,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20.0),
        ),
        child: Stack(
          children: <Widget>[
            UnityWidget(
              onUnityViewCreated: onUnityCreated,
              isARScene: false,
              onUnityMessage: onUnityMessage,
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

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    this._unityWidgetController = controller;
  }
}
