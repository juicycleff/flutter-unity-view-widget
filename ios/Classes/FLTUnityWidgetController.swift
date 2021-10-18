//
//  FLTUnityViewController.swift
//  flutter_unity_widget
//
//  Created by Rex Raphael on 30/01/2021.
//

import Foundation
import UnityFramework

// Defines unity controllable from Flutter.
class FLTUnityWidgetController: NSObject, FLTUnityOptionsSink, FlutterPlatformView {
    private var fltUnityView: FLTUnityView
    private var viewId: Int64 = 0
    private var channel: FlutterMethodChannel?
    private weak var registrar: (NSObjectProtocol & FlutterPluginRegistrar)?
    
    init(
        frame: CGRect,
        viewIdentifier viewId: Int64,
        arguments args: Any?,
        registrar: NSObjectProtocol & FlutterPluginRegistrar
    ) {
        let dict = args as? Dictionary<String, Any>;
        let curUILevel = dict?["uiLevel"] as? Int64 ?? 1;
        
        self.fltUnityView = FLTUnityView(frame: frame)
        super.init()
        self.setUILevel(level: curUILevel)

        self.viewId = viewId
        
        let channelName = String(format: "plugin.xraph.com/unity_view_%lld", viewId)
        self.channel = FlutterMethodChannel(name: channelName, binaryMessenger: registrar.messenger())
        globalChannel = self.channel
        
        self.channel?.setMethodCallHandler(self.methodHandler)
        self.initView()
    }
    
    func methodHandler(_ call: FlutterMethodCall, result: FlutterResult) {
        if call.method == "unity#dispose" {
            self.dispose()
            result(nil)
        } else {
            self.reattachView()
            if call.method == "unity#isReady" {
                result(GetUnityPlayerUtils()?.unityIsInitiallized())
            } else if call.method == "unity#isLoaded" {
                let _isUnloaded = GetUnityPlayerUtils()?.isUnityLoaded()
                result(_isUnloaded)
            } else if call.method == "unity#createUnityPlayer" {
                self.initView()
                result(nil)
            } else if call.method == "unity#isPaused" {
                let _isPaused = GetUnityPlayerUtils()?.isUnityPaused()
                result(_isPaused)
            } else if call.method == "unity#pausePlayer" {
                GetUnityPlayerUtils()?.pause()
                result(nil)
            } else if call.method == "unity#postMessage" {
                self.postMessage(call: call, result: result)
                result(nil)
            } else if call.method == "unity#resumePlayer" {
                GetUnityPlayerUtils()?.resume()
                result(nil)
            } else if call.method == "unity#unloadPlayer" {
                GetUnityPlayerUtils()?.unload()
                result(nil)
            } else if call.method == "unity#quitPlayer" {
                GetUnityPlayerUtils()?.quit()
                result(nil)
            } else if call.method == "unity#waitForUnity" {
                result(nil)
            } else {
                result(FlutterMethodNotImplemented)
            }
        }
    }

    func setDisabledUnload(enabled: Bool) {
        // Omitted for now
    }

    func setUILevel(level: Int64) {
        globalUILevel = level
    }
    
    func view() -> UIView {
        return fltUnityView
    }

    func initView() {
        if (GetUnityPlayerUtils()?.unityIsInitiallized() == true) {
            fltUnityView.setUnityView(GetUnityPlayerUtils()?.ufw?.appController()?.rootView)
        } else {
            GetUnityPlayerUtils()?.createPlayer(completed: { [self] (view: UIView?) in
                if let v = view {
                    fltUnityView.setUnityView(v)
                    self.channel?.invokeMethod("events#onUnityCreated", arguments: nil)
                }
            })
        }
    }
    
    @objc
    public func unityMessageHandler(_ message: UnsafePointer<Int8>?) {
        if let strMsg = message {
            self.channel?.invokeMethod("events#onUnityMessage", arguments: String(utf8String: strMsg))
        } else {
            self.channel?.invokeMethod("events#onUnityMessage", arguments: "")
        }
    }

    @objc
    public func unitySceneLoadedHandler(name: UnsafePointer<Int8>?, buildIndex: UnsafePointer<Int>?, isLoaded: UnsafePointer<ObjCBool>?, isValid: UnsafePointer<ObjCBool>?) {
        if let sceneName = name,
           let bIndex = buildIndex,
           let loaded = isLoaded,
           let valid = isValid {
        
            let addObject: Dictionary<String, Any> = [
                "name": String(utf8String: sceneName) ?? "",
                "buildIndex": bIndex,
                "isLoaded": loaded,
                "isValid": valid,
            ]
            self.channel?.invokeMethod("events#onUnitySceneLoaded", arguments: addObject)
        }
    }
    
    func attachView() {
        if (GetUnityPlayerUtils() != nil) {
            GetUnityPlayerUtils()?.initUnity()
            let unityView = GetUnityPlayerUtils()?.ufw?.appController()?.rootView
            if let superview = unityView?.superview {
                unityView?.removeFromSuperview()
                superview.layoutIfNeeded()
            }

            if let unityView = unityView {
                fltUnityView.addSubview(unityView)
            }
            GetUnityPlayerUtils()?.resume()
        }
    }

    func reattachView() {
        let unityView = GetUnityPlayerUtils()?.ufw?.appController()?.rootView
        let superview = unityView?.superview
        if superview != fltUnityView {
            attachView()
        }
    }
    
    func dispose() {
        channel?.setMethodCallHandler(nil)
        globalChannel?.setMethodCallHandler(nil)
        if GetUnityPlayerUtils() != nil {
            let unityView = GetUnityPlayerUtils()?.ufw?.appController()?.rootView
            let superview = unityView?.superview
            unityView?.removeFromSuperview()
            superview?.layoutIfNeeded()
            GetUnityPlayerUtils()?.pause()
        }
    }

    func postMessage(call: FlutterMethodCall, result: FlutterResult) {
        guard let args = call.arguments else {
            result("iOS could not recognize flutter arguments in method: (postMessage)")
            return
        }

        if let myArgs = args as? [String: Any],
           let gObj = myArgs["gameObject"] as? String,
           let method = myArgs["methodName"] as? String,
           let message = myArgs["message"] as? String {
            GetUnityPlayerUtils()?.postMessageToUnity(gameObject: gObj, unityMethodName: method, unityMessage: message)
            result(nil)
        } else {
            result(FlutterError(code: "-1", message: "iOS could not extract " +
                   "flutter arguments in method: (postMessage)", details: nil))
        }
    }
}
