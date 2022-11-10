import Flutter
import UIKit



public class SwiftFlutterUnityWidgetPlugin: NSObject, FlutterPlugin {
    
    static var publicChannel: FlutterMethodChannel?
    
    public static func register(with registrar: FlutterPluginRegistrar) {
        publicChannel = FlutterMethodChannel(name: "plugin.xraph.com/default_unity_view_channel", binaryMessenger: registrar.messenger())
        let fuwFactory = FLTUnityWidgetFactory(registrar: registrar)
        
        publicChannel?.setMethodCallHandler(methodHandler)
        registrar.register(fuwFactory, withId: "plugin.xraph.com/unity_view", gestureRecognizersBlockingPolicy: FlutterPlatformViewGestureRecognizersBlockingPolicyWaitUntilTouchesEnded)
    }
    
    static func methodHandler(_ call: FlutterMethodCall, result: FlutterResult) {
        
        guard let args = call.arguments as? [String: Any] else {
            result("iOS could not recognize flutter arguments in method: (postMessage)")
            return
        }
        
        let unityId = args["unityId"] as? Int
        if call.method == "unity#dispose" {
            if unityId != nil {
                let id = "\(unityId ?? -1)"
                if globalControllerIDs[id] != nil {
                    let cont = (globalControllerIDs[id]! as FLTUnityWidgetController)
                    cont.dispose()
                }
            }
            result(nil)
        } else {
            if unityId != nil {
                let id = "\(unityId ?? -1)"
                if globalControllerIDs[id] != nil {
                    let cont = (globalControllerIDs[id]! as FLTUnityWidgetController)
                    cont.reattachView()
                }
            }
            if call.method == "unity#isReady" {
                result(GetUnityPlayerUtils().unityIsInitiallized())
            } else if call.method == "unity#isLoaded" {
                let _isUnloaded = GetUnityPlayerUtils().isUnityLoaded()
                result(_isUnloaded)
            } else if call.method == "unity#createUnityPlayer" {
                if unityId != nil {
                    let id = "\(unityId ?? -1)"
                    if globalControllerIDs[id] != nil {
                        let cont = (globalControllerIDs[id]! as FLTUnityWidgetController)
                        cont.startUnityIfNeeded()
                    }
                }
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
