package com.xraph.plugin.flutter_unity_widget

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.os.Bundle
import androidx.lifecycle.DefaultLifecycleObserver
import com.unity3d.player.IUnityPlayerLifecycleEvents
import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding
import io.flutter.plugin.common.BinaryMessenger
import io.flutter.plugin.common.MethodCall
import io.flutter.plugin.common.MethodChannel
import io.flutter.plugin.platform.PlatformView


class FlutterUnityWidgetController(
        id: Int,
        context: Context,
        activity: Activity,
        binaryMessenger: BinaryMessenger,
        lifecycleProvider: LifecycleProvider,
        options: FlutterUnityWidgetOptions,
) :     PlatformView,
        DefaultLifecycleObserver,
        ActivityPluginBinding.OnSaveInstanceStateListener,
        FlutterUnityWidgetOptionsSink,
        MethodChannel.MethodCallHandler,
        UnityEventListener,
        IUnityPlayerLifecycleEvents {

    private var lifecycleProvider: LifecycleProvider

    private val methodChannel: MethodChannel
    private val context: Context
    private val activity: Activity
    private val options: FlutterUnityWidgetOptions

    private var methodChannelResult: MethodChannel.Result? = null
    private var unityView: UnityView? = null
    private var disposed: Boolean = false

    init {
        // set context and activity
        this.context = context
        this.activity = activity

        // lifecycle
        this.lifecycleProvider = lifecycleProvider
        this.lifecycleProvider.getLifecycle().addObserver(this)

        // set options
        this.options = options

        // setup method channel
        methodChannel = MethodChannel(binaryMessenger, "plugins.xraph.com/unity_view_$id")
        methodChannel.setMethodCallHandler(this)

        // setup unity view
        unityView = getUnityView()

        // Set unity listener
        UnityPlayerUtils.addUnityEventListener(this)
    }

    override fun getView(): UnityView? {
        return unityView
    }

    private fun getUnityView(): UnityView? {
        val view = UnityView.getInstance(context)
        if (UnityPlayerUtils.unityPlayer != null && UnityPlayerUtils.isUnityLoaded) {
            view.player = UnityPlayerUtils.unityPlayer
        } else {
            createPlayer(view, false)
        }
        return view
    }

    override fun dispose() {
        if (disposed) {
            return
        }
        disposed = true
        methodChannel.setMethodCallHandler(null)

        // setGoogleMapListener(null)
        // destroyMapViewIfNecessary()
        val lifecycle = lifecycleProvider.getLifecycle()
        if (lifecycle != null) {
            lifecycle.removeObserver(this)
        }
    }

    override fun onMethodCall(methodCall: MethodCall, result: MethodChannel.Result) {
        when (methodCall.method) {
            "unity#waitForUnity" -> {
                if (unityView != null) {
                    result.success(null)
                    return
                }
                methodChannelResult = result
            }
            "unity#createUnityPlayer" -> this.createPlayer(unityView, true)
            "unity#isReady" -> result.success(UnityPlayerUtils.isUnityReady)
            "unity#isLoaded" -> {
                result.success(UnityPlayerUtils.isUnityLoaded)
            }
            "unity#isPaused" -> {
                result.success(UnityPlayerUtils.isUnityPaused)
            }
            "unity#inBackground" -> result.success(UnityPlayerUtils.isUnityInBackground)
            "unity#postMessage" -> {
                val gameObject: String = methodCall.argument<String>("gameObject").toString()
                val methodName: String = methodCall.argument<String>("methodName").toString()
                val message: String = methodCall.argument<String>("message").toString()

                UnityPlayerUtils.postMessage(gameObject, methodName, message)
                result.success(true)
            }
            "unity#pausePlayer" -> {
                UnityPlayerUtils.pause()
                result.success(true)
            }
            "unity#openInNativeProcess" -> {
                openNativeUnity()
                result.success(true)
            }
            "unity#resumePlayer" -> {
                UnityPlayerUtils.resume()
                result.success(true)
            }
            "unity#unloadPlayer" -> {
                UnityPlayerUtils.unload()
                result.success(true)
            }
            "unity#dispose" ->                 // TODO: Handle disposing player resource efficiently
                // UnityUtils.unload();
                result.success(true)
            "unity#silentQuitPlayer" -> {
                UnityPlayerUtils.quitPlayer()
                result.success(true)
            }
            "unity#quitPlayer" -> {
                if (UnityPlayerUtils.unityPlayer != null) {
                    UnityPlayerUtils.unityPlayer.destroy()
                }
                result.success(true)
            }
            else -> result.notImplemented()
        }
    }

    override fun onSaveInstanceState(bundle: Bundle) {
        if (disposed) {
            return
        }
    }

    override fun onRestoreInstanceState(bundle: Bundle?) {
        if (disposed) {
            return
        }
    }

    override fun setAREnabled(arEnabled: Boolean) {
        UnityPlayerUtils.options.arEnabled = arEnabled
    }

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        UnityPlayerUtils.options.fullscreenEnabled = fullscreenEnabled
    }

    override fun onMessage(message: String) {
        activity.runOnUiThread { methodChannel.invokeMethod("events#onUnityMessage", message) }
    }

    override fun onSceneLoaded(name: String, buildIndex: Int, isLoaded: Boolean, isValid: Boolean) {
        activity.runOnUiThread {
            val payload: MutableMap<String, Any> = HashMap()
            payload["name"] = name
            payload["buildIndex"] = buildIndex
            payload["isLoaded"] = isLoaded
            payload["isValid"] = isValid
            methodChannel.invokeMethod("events#onUnitySceneLoaded", payload)
        }
    }

    override fun onUnityPlayerUnloaded() {
        activity.runOnUiThread { methodChannel.invokeMethod("events#onUnityUnloaded", true) }
    }

    override fun onUnityPlayerQuitted() {
        TODO("Not yet implemented")
    }

    private fun openNativeUnity() {
        val intent = Intent(context.applicationContext, OverrideUnityActivity::class.java)
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.CUPCAKE) {
            intent.flags = Intent.FLAG_ACTIVITY_REORDER_TO_FRONT
        }
        intent.putExtra("fullscreen", options.fullscreenEnabled)
        intent.putExtra("flutterActivity", activity.javaClass)
        activity.startActivityForResult(intent, 1)
    }

    private fun createPlayer(view: UnityView?, reInitialize: Boolean) {
        UnityPlayerUtils.createPlayer(activity, this, reInitialize, object : OnCreateUnityViewCallback {
            override fun onReady() {
                if (view != null) {
                    view.player = UnityPlayerUtils.unityPlayer
                }
                if (methodChannelResult != null) {
                    methodChannelResult!!.success(null)
                    methodChannelResult = null
                }
            }
        })
    }

}