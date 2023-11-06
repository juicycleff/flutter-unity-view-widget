//
//  HandleEventSink.swift
//  flutter_unity_widget
//
//  Created by Rex Raphael on 14/12/2022.
//

import Foundation

extension Notification.Name {
    static var publishToFlutter: Notification.Name {
          return .init(rawValue: "message.publishToFlutter")}
}

/// StepCounter, handles step count streaming
public class HandleEventSink: NSObject, FlutterStreamHandler {
    private var eventSink: FlutterEventSink?

    public func onListen(withArguments arguments: Any?, eventSink: @escaping FlutterEventSink) -> FlutterError? {
        self.eventSink = eventSink

        NotificationCenter
            .default
            .addObserver(
                self,
                selector: #selector(publishMessage),
                name: .publishToFlutter,
                object: nil
            )

        if #available(iOS 10.0, *) {

        } else {
            eventSink(FlutterError(code: "1", message: "Requires iOS 10.0 minimum", details: nil))
        }
        return nil
    }

    @objc func publishMessage(_ notification: Notification){
        var payload = notification.userInfo?["payload"] as? [String: Any] ?? [:]
        eventSink!(payload)
    }

    public func onCancel(withArguments arguments: Any?) -> FlutterError? {
        NotificationCenter.default.removeObserver(self)
        NotificationCenter.default
            .removeObserver(self, name: .publishToFlutter, object: nil)

        eventSink = nil
        return nil
    }
}

enum DataStreamEventTypes : String {
    case OnUnityViewCreated
    case OnUnityPlayerReInitialize
    case OnViewReattached
    case OnUnityPlayerCreated
    case OnUnityPlayerUnloaded
    case OnUnityPlayerQuited
    case OnUnitySceneLoaded
    case OnUnityMessage
}

struct DataStreamEvent {
    var eventType: DataStreamEventTypes
    var data: Any

    func toMap() -> [String: Any] {
        let m = ["eventType": eventType.rawValue, "data": data]
        return m
    }
}
