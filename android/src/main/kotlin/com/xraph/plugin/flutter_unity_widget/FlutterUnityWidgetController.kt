package com.xraph.plugin.flutter_unity_widget

import android.app.Activity
import android.content.Context
import android.content.ContextWrapper
import android.content.Intent
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.view.View
import androidx.lifecycle.DefaultLifecycleObserver
import androidx.lifecycle.LifecycleOwner
import com.unity3d.player.IUnityPlayerLifecycleEvents
import com.unity3d.player.MultiWindowSupport
import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding
import io.flutter.plugin.common.BinaryMessenger
import io.flutter.plugin.common.MethodCall
import io.flutter.plugin.common.MethodChannel
import io.flutter.plugin.common.MethodChannel.MethodCallHandler
import io.flutter.plugin.platform.PlatformView
import java.lang.Exception

class FlutterUnityWidgetController(
        id: Int,
        context: Context,
        appContext: Context,
        binaryMessenger: BinaryMessenger,
        lifecycleProvider: LifecycleProvider,
        options: FlutterUnityWidgetOptions
) :     PlatformView,
        DefaultLifecycleObserver,
        ActivityPluginBinding.OnSaveInstanceStateListener,
        FlutterUnityWidgetOptionsSink,
        MethodCallHandler,
        UnityEventListener,
        IUnityPlayerLifecycleEvents {

    private val LOG_TAG = "UnityPlayerUtils"
    private var lifecycleProvider: LifecycleProvider

    private val methodChannel: MethodChannel
    private val id: Int
    private val context: Context
    private val appContext: Context
    private val options: FlutterUnityWidgetOptions

    private var methodChannelResult: MethodChannel.Result? = null
    private var unityView: UnityView? = null
    private var disposed: Boolean = false

    init {
        // set context and activity
        this.context = context
        this.appContext = appContext

        this.id = id

        // lifecycle
        this.lifecycleProvider = lifecycleProvider

        // set options
        this.options = options

        // setup method channel
        methodChannel = MethodChannel(binaryMessenger, "plugin.xraph.com/unity_view_$id")
        methodChannel.setMethodCallHandler(this)

        // setup unity view
        unityView = getUnityView()

        // Set unity listener
        UnityPlayerUtils.addUnityEventListener(this)
    }

    fun bootstrap() {
        this.lifecycleProvider.getLifecycle().addObserver(this)
    }

    override fun getView(): View? {
        return unityView
    }

    private fun getUnityView(): UnityView? {
        val view = UnityView.getInstance(context)
        if (UnityPlayerUtils.isUnityLoaded && UnityPlayerUtils.unityPlayer != null) {
            view.player = UnityPlayerUtils.unityPlayer!!
        } else {
            createPlayer(view,false)
        }
        return view
    }

    override fun dispose() {
        if (disposed) {
            return
        }
        disposed = true
        // methodChannel.setMethodCallHandler(null)

        val lifecycle = lifecycleProvider.getLifecycle()
        if (lifecycle != null) {
            lifecycle.removeObserver(this)
        }
    }

    override fun onMethodCall(methodCall: MethodCall, result: MethodChannel.Result) {
        when (methodCall.method) {
            "unity#waitForUnity" -> {
                if (UnityPlayerUtils.unityPlayer != null) {
                    result.success(null)
                    return
                }
                methodChannelResult = result
            }
            "unity#createPlayer" -> {
                this.createPlayer(unityView)
            }
            "unity#isReady" -> {
                result.success(UnityPlayerUtils.isUnityReady)
            }
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
            "unity#dispose" -> {         // TODO: Handle disposing player resource efficiently
                // UnityPlayerUtils.unload()
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

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        UnityPlayerUtils.options.fullscreenEnabled = fullscreenEnabled
    }

    override fun onMessage(message: String) {
        Handler(Looper.getMainLooper()).post {
            methodChannel.invokeMethod("events#onUnityMessage", message)
        }
    }

    override fun onSceneLoaded(name: String, buildIndex: Int, isLoaded: Boolean, isValid: Boolean) {
        Handler(Looper.getMainLooper()).post {
            val payload: MutableMap<String, Any> = HashMap()
            payload["name"] = name
            payload["buildIndex"] = buildIndex
            payload["isLoaded"] = isLoaded
            payload["isValid"] = isValid
            methodChannel.invokeMethod("events#onUnitySceneLoaded", payload)
        }
    }

    override fun onUnityPlayerUnloaded() {
        Handler(Looper.getMainLooper()).post {
            methodChannel.invokeMethod("events#onUnityUnloaded", true)
        }
    }

    override fun onUnityPlayerQuitted() {
        TODO("Not yet implemented")
    }

    private fun openNativeUnity() {
        val activity = getActivity(this.context)
        if (activity != null) {
            val intent = Intent(context.applicationContext, OverrideUnityActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_REORDER_TO_FRONT
            intent.putExtra("fullscreen", options.fullscreenEnabled)
            intent.putExtra("flutterActivity", activity.javaClass)
            activity.startActivityForResult(intent, 1)
        }
    }

    override fun onCreate(owner: LifecycleOwner) {
    }

    override fun onStart(owner: LifecycleOwner) {
        if(UnityPlayerUtils.isUnityReady) {
            if (MultiWindowSupport.getAllowResizableWindow(UnityPlayerUtils.activity)) return
            UnityPlayerUtils.resume()
        }
    }

    override fun onResume(owner: LifecycleOwner) {
        if(UnityPlayerUtils.isUnityReady) {
            if (MultiWindowSupport.getAllowResizableWindow(UnityPlayerUtils.activity)) return
            UnityPlayerUtils.resume()
        }
    }

    override fun onPause(owner: LifecycleOwner) {
        if (MultiWindowSupport.getAllowResizableWindow(UnityPlayerUtils.activity)) return
        UnityPlayerUtils.pause()
    }

    override fun onStop(owner: LifecycleOwner) {
        if (MultiWindowSupport.getAllowResizableWindow(UnityPlayerUtils.activity)) return

        UnityPlayerUtils.pause()
    }

    override fun onDestroy(owner: LifecycleOwner) {
        owner.lifecycle.removeObserver(this)
        if (disposed) {
            return
        }
        destroyUnityViewIfNecessary()
    }

    private fun destroyUnityViewIfNecessary() {
        if (UnityPlayerUtils.unityPlayer == null && unityView == null) {
            return
        }
        UnityPlayerUtils.isUnityReady = false
        UnityPlayerUtils.unityPlayer?.destroy()
        unityView = null
        UnityPlayerUtils.unityPlayer = null
    }

    private fun createPlayer(view: UnityView?, reInitialize: Boolean) {
        try {
            val activity = getActivity(this.context)
            if (activity != null) {
                UnityPlayerUtils.createPlayer(this, reInitialize, object : OnCreateUnityViewCallback {
                    override fun onReady() {
                        if (!reInitialize) view?.setUnityPlayer(UnityPlayerUtils.unityPlayer!!)
                        else  view?.player = UnityPlayerUtils.unityPlayer!!
                        if (methodChannelResult != null) {
                            methodChannelResult!!.success(true)
                            methodChannelResult = null
                        }
                    }
                })
            }
        } catch (e: Exception) {
            if (methodChannelResult != null){
                methodChannelResult!!.error("FLUTTER_UNITY_WIDGET", e.message, e)
                methodChannelResult!!.success(false)
                methodChannelResult = null
            }
        }
    }

    private fun createPlayer(view: UnityView?) {
        try {
            val activity = getActivity(this.context)
            if (activity != null) {
                UnityPlayerUtils.createPlayer(this, true, object : OnCreateUnityViewCallback {
                    override fun onReady() {
                        view?.player = UnityPlayerUtils.unityPlayer!!
                        if (methodChannelResult != null) {
                            methodChannelResult!!.success(true)
                            methodChannelResult = null
                        }
                    }
                })
            }
        } catch (e: Exception) {
            if (methodChannelResult != null){
                methodChannelResult!!.error("FLUTTER_UNITY_WIDGET", e.message, e)
                methodChannelResult!!.success(false)
                methodChannelResult = null
            }
        }
    }

    private fun getActivity(context: Context?): Activity? {
        if (context == null) {
            return null
        } else if (context is ContextWrapper) {
            return if (context is Activity) {
                context
            } else {
                getActivity((context as ContextWrapper).baseContext)
            }
        }
        return null
    }
}