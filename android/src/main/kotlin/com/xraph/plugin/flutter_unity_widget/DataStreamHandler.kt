package com.xraph.plugin.flutter_unity_widget

import io.flutter.plugin.common.EventChannel
import io.reactivex.rxjava3.android.schedulers.AndroidSchedulers
import io.reactivex.rxjava3.subjects.PublishSubject
import org.json.JSONObject

class DataStreamEventNotifier {
    companion object {
        val notifier: PublishSubject<DataStreamEvent> = PublishSubject.create()
    }
}

data class DataStreamEvent(val eventType: String, val data: Any) {
    fun toMap(): Map<String, Any> {
        return mapOf("eventType" to eventType, "data" to data)
    }

    fun toJsonString(): String {
        return JSONObject(toMap()).toString()
    }
}

enum class DataStreamEventTypes {
    OnUnityViewCreated,
    OnUnityPlayerReInitialize,
    OnViewReattached,
    OnUnityPlayerCreated,
    OnUnityPlayerUnloaded,
    OnUnityPlayerQuited,
    OnUnitySceneLoaded,
    OnUnityMessage,
}

class DataStreamHandler: EventChannel.StreamHandler {
    override fun onListen(arguments: Any?, events: EventChannel.EventSink) {
        DataStreamEventNotifier.notifier.subscribeOn(AndroidSchedulers.mainThread())
            .observeOn(AndroidSchedulers.mainThread()).subscribe {
                events.success(it.toMap())
            }
    }

    override fun onCancel(arguments: Any?) {
        DataStreamEventNotifier.notifier.unsubscribeOn(AndroidSchedulers.mainThread())
    }
}