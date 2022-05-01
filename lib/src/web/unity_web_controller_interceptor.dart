// In order to *not* need this ignore, consider extracting the "web" version
// of your plugin as a separate package, instead of inlining it in the same
// package as the core of your plugin.
// ignore: avoid_web_libraries_in_flutter
import 'dart:html' as html;

class UnityWebControllerInterceptor {
  late html.MessageEvent _unityFlutterBiding;

  UnityWebControllerInterceptor({
    required Function(String data) onUnityMessage,
    required Function(String data) onUnitySceneChnged,
    required Function() onUnityReady,
  }) {
    html.window.addEventListener('message', (event) {
      // listenMessageFromUnity((event as html.MessageEvent).data.toString());
      print((event as html.MessageEvent).data.toString());
      //refreshUnityView();
    });
  }
}
