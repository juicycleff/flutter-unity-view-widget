package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.app.Application
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.util.Log
import androidx.annotation.NonNull
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.LifecycleRegistry

import io.flutter.embedding.engine.plugins.FlutterPlugin
import io.flutter.embedding.engine.plugins.FlutterPlugin.FlutterPluginBinding
import io.flutter.embedding.engine.plugins.activity.ActivityAware
import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding
import io.flutter.embedding.engine.plugins.lifecycle.FlutterLifecycleAdapter
import io.flutter.plugin.common.EventChannel
import io.flutter.plugin.common.MethodCall
import io.flutter.plugin.common.MethodChannel

const val channelIdPrefix = "plugin.xraph.com"
const val STREAM_CHANNEL_NAME = "$channelIdPrefix/stream_channel"

/** FlutterUnityWidgetPlugin */
class FlutterUnityWidgetPlugin:
    FlutterPlugin,
    ActivityAware,
    MethodChannel.MethodCallHandler,
    UnityEventListener {

    private var lifecycle: Lifecycle? = null
    private var flutterPluginBinding: FlutterPluginBinding? = null
    private lateinit var methodChannel: MethodChannel
    private lateinit var streamChannel: EventChannel
    private lateinit var streamHandler: DataStreamHandler

    override fun onAttachedToEngine(@NonNull binding: FlutterPluginBinding) {
        Log.d(LOG_TAG, "onAttachedToEngine")
        methodChannel = MethodChannel(binding.binaryMessenger, "$channelIdPrefix/base_channel")
        methodChannel.setMethodCallHandler(this)

        streamChannel = EventChannel(binding.binaryMessenger, STREAM_CHANNEL_NAME)
        streamHandler = DataStreamHandler()
        streamChannel.setStreamHandler(streamHandler)


        // Set unity listener
        UnityPlayerUtils.addUnityEventListener(this)

        flutterPluginBinding = binding
        binding
                .platformViewRegistry
                .registerViewFactory(
                        VIEW_TYPE,
                        FlutterUnityWidgetFactory(
                                object : LifecycleProvider {
                                    override fun getLifecycle(): Lifecycle {
                                        return lifecycle!!
                                    }
                                }))
    }

    override fun onDetachedFromEngine(@NonNull binding: FlutterPluginBinding) {
        Log.d(LOG_TAG, "onDetachedFromEngine")
        UnityPlayerUtils.removeUnityEventListener(this)
        streamChannel.setStreamHandler(null)
        flutterPluginBinding = null
    }

    companion object {
        internal const val LOG_TAG = "FUWPlugin"
        private const val VIEW_TYPE = "plugin.xraph.com/unity_view"
    }

    @SuppressLint("LongLogTag")
    override fun onAttachedToActivity(binding: ActivityPluginBinding) {
        Log.d(LOG_TAG, "onAttachedToActivity")
        handleActivityChange(binding.activity)
        lifecycle = FlutterLifecycleAdapter.getActivityLifecycle(binding)
    }

    override fun onDetachedFromActivityForConfigChanges() {
        Log.d(LOG_TAG, "onDetachedFromActivityForConfigChanges")
        onDetachedFromActivity()
    }

    override fun onReattachedToActivityForConfigChanges(binding: ActivityPluginBinding) {
        Log.d(LOG_TAG, "onReattachedToActivityForConfigChanges")
        onAttachedToActivity(binding)
    }

    override fun onDetachedFromActivity() {
        Log.d(LOG_TAG, "onDetachedFromActivity")
        handleActivityChange(null)
        lifecycle = null
    }

    /**
     *
     */
    private fun handleActivityChange(activity: Activity?) {
        Log.d(LOG_TAG, "handleActivityChange")
        if (activity != null) {
            UnityPlayerUtils.prevActivityRequestedOrientation = activity.requestedOrientation
            UnityPlayerUtils.activity = activity
            return
        }

        UnityPlayerUtils.activity = null
        UnityPlayerUtils.reset()
        UnityPlayerUtils.quitPlayer()
    }

    /**
     * This class provides a {@link LifecycleOwner} for the activity driven by {@link
     * ActivityLifecycleCallbacks}.
     *
     * <p>This is used in the case where a direct Lifecycle/Owner is not available.
     */
    @SuppressLint("NewApi")
    private class ProxyLifecycleProvider(activity: Activity) : Application.ActivityLifecycleCallbacks, LifecycleOwner, LifecycleProvider {
        private val lifecycle = LifecycleRegistry(this)
        private var registrarActivityHashCode: Int = 0

        init {
            UnityPlayerUtils.activity = activity
            this.registrarActivityHashCode = activity.hashCode()
            activity.application.registerActivityLifecycleCallbacks(this)
        }

        override fun onActivityCreated(activity: Activity, savedInstanceState: Bundle?) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_CREATE)
        }

        override fun onActivityStarted(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_START)
        }

        override fun onActivityResumed(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_RESUME)
        }

        override fun onActivityPaused(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_PAUSE)
        }

        override fun onActivityStopped(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_STOP)
        }

        override fun onActivitySaveInstanceState(activity: Activity, outState: Bundle) {
            UnityPlayerUtils.activity = activity
        }

        override fun onActivityDestroyed(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }

            activity.application.unregisterActivityLifecycleCallbacks(this)
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_DESTROY)
        }

        override fun getLifecycle(): Lifecycle {
            return lifecycle
        }
    }

    override fun onMethodCall(methodCall: MethodCall, result: MethodChannel.Result) {
        val id: String = methodCall.argument<String?>("unityId").toString() ?: ""
        val unityId = "unity-id-$id"

        when (methodCall.method) {
            "unity#waitForUnity" -> {
                if (UnityPlayerUtils.unityPlayer != null) {
                    result.success(null)
                    return
                }
                result.success(null)
            }
            "unity#createPlayer" -> {
                UnityPlayerUtils.controllers[unityId]?.invalidateFrameIfNeeded()
                UnityPlayerUtils.controllers[unityId]?.createPlayer()
                UnityPlayerUtils.controllers[unityId]?.refocusUnity()
                result.success(null)
            }
            "unity#isReady" -> {
                result.success(UnityPlayerUtils.unityPlayer != null)
            }
            "unity#isLoaded" -> {
                result.success(UnityPlayerUtils.unityLoaded)
            }
            "unity#isPaused" -> {
                result.success(UnityPlayerUtils.unityPaused)
            }
            "unity#postMessage" -> {
                UnityPlayerUtils.controllers[unityId]?.invalidateFrameIfNeeded()
                val gameObject: String = methodCall.argument<String>("gameObject").toString()
                val methodName: String = methodCall.argument<String>("methodName").toString()
                val message: String = methodCall.argument<String>("message").toString()
                UnityPlayerUtils.postMessage(gameObject, methodName, message)
                result.success(true)
            }
            "unity#pausePlayer" -> {
                UnityPlayerUtils.controllers[unityId]?.invalidateFrameIfNeeded()
                UnityPlayerUtils.pause()
                result.success(true)
            }
            "unity#openInNativeProcess" -> {
                UnityPlayerUtils.openNativeUnity()
                result.success(true)
            }
            "unity#resumePlayer" -> {
                UnityPlayerUtils.controllers[unityId]?.invalidateFrameIfNeeded()
                UnityPlayerUtils.resume()
                result.success(true)
            }
            "unity#unloadPlayer" -> {
                UnityPlayerUtils.controllers[unityId]?.invalidateFrameIfNeeded()
                UnityPlayerUtils.unload()
                result.success(true)
            }
            "unity#dispose" -> {
                // destroyUnityViewIfNecessary()
                // if ()
                // dispose()
                result.success(null)
            }
            "unity#silentQuitPlayer" -> {
                UnityPlayerUtils.quitPlayer()
                result.success(true)
            }
            "unity#quitPlayer" -> {
                if (UnityPlayerUtils.unityPlayer != null) {
                    UnityPlayerUtils.unityPlayer!!.destroy()
                }
                result.success(true)
            }
            else -> result.notImplemented()
        }
    }

    //#region Unity Events
    override fun onMessage(message: String) {
        Handler(Looper.getMainLooper()).post {
            DataStreamEventNotifier.notifier.onNext(
                DataStreamEvent(
                    DataStreamEventTypes.OnUnityMessage.name,
                    message,
                )
            )
        }
    }

    override fun onSceneLoaded(name: String, buildIndex: Int, isLoaded: Boolean, isValid: Boolean) {
        Handler(Looper.getMainLooper()).post {
            val payload: MutableMap<String, Any> = HashMap()
            payload["name"] = name
            payload["buildIndex"] = buildIndex
            payload["isLoaded"] = isLoaded
            payload["isValid"] = isValid
            DataStreamEventNotifier.notifier.onNext(
                DataStreamEvent(
                    DataStreamEventTypes.OnUnitySceneLoaded.name,
                    payload,
                )
            )
        }
    }
    //#endregion
}