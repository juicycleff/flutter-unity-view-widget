import Flutter
import UIKit

public class SwiftFlutterUnityWidgetPlugin: NSObject, FlutterPlugin {
    private static var methodChannel: FlutterMethodChannel?
    private static var unityEventHandler: HandleEventSink?
    private static var unityEventChannel: FlutterEventChannel?
    
    public static func register(with registrar: FlutterPluginRegistrar) {
        methodChannel = FlutterMethodChannel(name: "plugin.xraph.com/base_channel", binaryMessenger: registrar.messenger())
        unityEventChannel = FlutterEventChannel.init(name: "plugin.xraph.com/stream_channel", binaryMessenger: registrar.messenger())
        unityEventHandler = HandleEventSink()
        
        methodChannel?.setMethodCallHandler(methodHandler)
        unityEventChannel?.setStreamHandler(unityEventHandler)
        
        let fuwFactory = FLTUnityWidgetFactory(registrar: registrar)
        registrar.register(fuwFactory, withId: "plugin.xraph.com/unity_view", gestureRecognizersBlockingPolicy: FlutterPlatformViewGestureRecognizersBlockingPolicyWaitUntilTouchesEnded)
    }
    
    private static func methodHandler(_ call: FlutterMethodCall, result: FlutterResult) {
        
        let arguments = call.arguments as? NSDictionary
        let id = arguments?["unityId"] as? Int ?? 0
        let unityId = "unity-id-\(id)"
        
        if call.method == "unity#dispose" {
            GetUnityPlayerUtils().activeController?.dispose()
            result(nil)
        } else {
            GetUnityPlayerUtils().activeController?.reattachView()
            if call.method == "unity#isReady" {
                result(GetUnityPlayerUtils().unityIsInitiallized())
            } else if call.method == "unity#isLoaded" {
                let _isUnloaded = GetUnityPlayerUtils().isUnityLoaded()
                result(_isUnloaded)
            } else if call.method == "unity#createPlayer" {
                GetUnityPlayerUtils().activeController?.startUnityIfNeeded()
                result(nil)
            } else if call.method == "unity#isPaused" {
                let _isPaused = GetUnityPlayerUtils().isUnityPaused()
                result(_isPaused)
            } else if call.method == "unity#pausePlayer" {
                GetUnityPlayerUtils().pause()
                result(nil)
            } else if call.method == "unity#postMessage" {
                postMessage(call: call, result: result)
                result(nil)
            } else if call.method == "unity#resumePlayer" {
                GetUnityPlayerUtils().resume()
                result(nil)
            } else if call.method == "unity#unloadPlayer" {
                GetUnityPlayerUtils().unload()
                result(nil)
            } else if call.method == "unity#quitPlayer" {
                GetUnityPlayerUtils().quit()
                result(nil)
            } else if call.method == "unity#waitForUnity" {
                result(nil)
            } else {
                result(FlutterMethodNotImplemented)
            }
        }
    }
    
    /// Post messages to unity from flutter
    private static func postMessage(call: FlutterMethodCall, result: FlutterResult) {
        guard let args = call.arguments else {
            result("iOS could not recognize flutter arguments in method: (postMessage)")
            return
        }
        
        if let myArgs = args as? [String: Any],
           let gObj = myArgs["gameObject"] as? String,
           let method = myArgs["methodName"] as? String,
           let message = myArgs["message"] as? String {
            GetUnityPlayerUtils().postMessageToUnity(gameObject: gObj, unityMethodName: method, unityMessage: message)
            result(nil)
        } else {
            result(FlutterError(code: "-1", message: "iOS could not extract " +
                                "flutter arguments in method: (postMessage)", details: nil))
        }
    }
}
