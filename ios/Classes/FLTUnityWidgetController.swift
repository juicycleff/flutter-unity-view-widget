//
//  FLTUnityViewController.swift
//  flutter_unity_widget
//
//  Created by Rex Raphael on 30/01/2021.
//

import Foundation
import UnityFramework

// Defines unity controllable from Flutter.
public class FLTUnityWidgetController: NSObject, FLTUnityOptionsSink, FlutterPlatformView {
    private var _rootView: FLTUnityView
    private var viewId: Int64 = 0
    private var keyId = ""
    private var channel: FlutterMethodChannel?
    private weak var registrar: (NSObjectProtocol & FlutterPluginRegistrar)?

    private var _disposed = false

    init(
        frame: CGRect,
        viewIdentifier viewId: Int64,
        arguments args: Any?,
        registrar: NSObjectProtocol & FlutterPluginRegistrar
    ) {
        self._rootView = FLTUnityView(frame: frame)
        super.init()
        self.viewId = viewId
        keyId = "unity-id-\(viewId)"

        globalControllers.append(self)
        GetUnityPlayerUtils().activeController = self
        self.attachView()
    }

    func setDisabledUnload(enabled: Bool) {

    }

    public func view() -> UIView {
           return _rootView;
    }

    func startUnityIfNeeded() {
        GetUnityPlayerUtils().createPlayer(completed: { [self] (view: UIView?) in
            GetUnityPlayerUtils().notifyFlutter(
                data: DataStreamEvent(
                    eventType: DataStreamEventTypes.OnUnityPlayerCreated,
                    data: true))
        })
    }

    func attachView() {
        startUnityIfNeeded()

        let unityView = GetUnityPlayerUtils().ufw?.appController()?.rootView
        if let superview = unityView?.superview {
            unityView?.removeFromSuperview()
            superview.layoutIfNeeded()
        }

        if let unityView = unityView {
            _rootView.addSubview(unityView)
            _rootView.layoutIfNeeded()
        }
        
        GetUnityPlayerUtils().notifyFlutter(
            data: DataStreamEvent(
                eventType: DataStreamEventTypes.OnUnityViewCreated,
                data: true))
        
        GetUnityPlayerUtils().notifyFlutter(
            data: DataStreamEvent(
                eventType: DataStreamEventTypes.OnUnityViewCreated,
                data: true))
        GetUnityPlayerUtils().resume()
    }

    func reattachView() {
        let unityView = GetUnityPlayerUtils().ufw?.appController()?.rootView
        let superview = unityView?.superview
        if superview != _rootView {
            attachView()
            
            GetUnityPlayerUtils().notifyFlutter(
                data: DataStreamEvent(
                    eventType: DataStreamEventTypes.OnViewReattached,
                    data: true))
        }

        GetUnityPlayerUtils().resume()
    }

    func removeViewIfNeeded() {
        if GetUnityPlayerUtils().ufw == nil {
            return
        }

        let unityView = GetUnityPlayerUtils().ufw?.appController()?.rootView
        if _rootView == unityView?.superview {
            if globalControllers.isEmpty {
                unityView?.removeFromSuperview()
                unityView?.superview?.layoutIfNeeded()
            } else {
                globalControllers.last?.reattachView()
            }
        }
        GetUnityPlayerUtils().resume()
    }

    func dispose() {
        if _disposed {
            return
        }

        globalControllers.removeAll{ value in
            return value == self
        }

        removeViewIfNeeded()
        GetUnityPlayerUtils().activeController = globalControllers.last
           
        _disposed = true
    }
    
    /// Post messages to unity from flutter
    func postMessage(call: FlutterMethodCall, result: FlutterResult) {
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
