package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Context
import android.content.ContextWrapper
import android.content.Intent
import android.graphics.Color
import android.os.Build
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.util.Log
import android.view.View
import android.widget.TextView
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

@SuppressLint("NewApi")
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
        IUnityPlayerLifecycleEvents,
        View.OnAttachStateChangeListener {

    private val LOG_TAG = "FlutterUnity"
    private var lifecycleProvider: LifecycleProvider = lifecycleProvider

    private val _id: Int
    private val methodChannel: MethodChannel
    private val _context: Context
    private val _appContext: Context
    private val _options: FlutterUnityWidgetOptions

    private var methodChannelResult: MethodChannel.Result? = null
    private var unityView: UnityView? = null

    init {
        _id = id
        _context = context
        _appContext = appContext
        _options = options

        // setup method channel
        methodChannel = MethodChannel(binaryMessenger, "plugin.xraph.com/unity_view_$_id")
        methodChannel.setMethodCallHandler(this)

        unityView = getInternalUnityView()
        // Set unity listener
        UnityPlayerUtils.addUnityEventListener(this)
    }

    fun bootstrap() {
        this.lifecycleProvider.getLifecycle().addObserver(this)
    }

    override fun getView(): View {
        if (unityView != null) return unityView!!
        return getErrorView()
    }

    private fun getErrorView(): View {
        val textView = TextView(_context)
        textView.text = "Error loading unity"
        textView.setBackgroundColor(Color.RED)
        textView.setTextColor(Color.YELLOW)
        return textView
    }

    private fun getInternalUnityView(): UnityView {
        unityView = UnityPlayerUtils.initInternalView(getActivity(null)!!, this)

        if (!UnityPlayerUtils.isUnityLoaded && UnityPlayerUtils.isUnityReady) {
            this.createPlayer(unityView!!)
        }

        if (UnityPlayerUtils.unityPlayer != null) {
            unityView?.setUnityPlayer(UnityPlayerUtils.unityPlayer!!)
            return unityView!!
        }

        createPlayer(unityView!!)
        UnityPlayerUtils.disposed = false

        return unityView!!
    }

    override fun dispose() {
        if (UnityPlayerUtils.disposed) {
            return
        }

        unityView?.removeOnAttachStateChangeListener(this)
        UnityPlayerUtils.removeUnityEventListener(this)
        destroyUnityViewIfNecessary()
        // methodChannel.setMethodCallHandler(null)

        val lifecycle = lifecycleProvider.getLifecycle()
        lifecycle.removeObserver(this)

        UnityPlayerUtils.disposed = true
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
                this.createPlayer()
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
            "unity#dispose" -> {
                // destroyUnityViewIfNecessary()
                // UnityPlayerUtils.disposed = true
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
        if (UnityPlayerUtils.disposed) {
            return
        }
    }

    override fun onRestoreInstanceState(bundle: Bundle?) {
        if (UnityPlayerUtils.disposed) {
            return
        }
    }

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        UnityPlayerUtils.options.fullscreenEnabled = fullscreenEnabled
    }

    override fun setHideStatusBar(hideStatusBar: Boolean) {
        UnityPlayerUtils.options.hideStatus = hideStatusBar
    }

    override fun setRunImmediately(runImmediately: Boolean) {
        UnityPlayerUtils.options.runImmediately = runImmediately
    }

    override fun setUnloadOnDispose(unloadOnDispose: Boolean) {
        UnityPlayerUtils.options.unloadOnDispose = unloadOnDispose
    }

    override fun onMessage(message: String) {
        // UnityPlayerUtils.activity!!.runOnUiThread {
        Handler(Looper.getMainLooper()).post {
            methodChannel.invokeMethod("events#onUnityMessage", message)
        }
    }

    override fun onSceneLoaded(name: String, buildIndex: Int, isLoaded: Boolean, isValid: Boolean) {
        Handler(Looper.getMainLooper()).post {
        // UnityPlayerUtils.activity!!.runOnUiThread {
            val payload: MutableMap<String, Any> = HashMap()
            payload["name"] = name
            payload["buildIndex"] = buildIndex
            payload["isLoaded"] = isLoaded
            payload["isValid"] = isValid
            methodChannel.invokeMethod("events#onUnitySceneLoaded", payload)
        }
    }

    override fun onUnityPlayerUnloaded() {
        // UnityPlayerUtils.activity!!.runOnUiThread {
        Handler(Looper.getMainLooper()).post {
            methodChannel.invokeMethod("events#onUnityUnloaded", true)
        }
    }

    override fun onUnityPlayerQuitted() {
        TODO("Not yet implemented")
    }

    private fun openNativeUnity() {
        val activity = getActivity(null)
        if (activity != null) {
            val intent = Intent(_context.applicationContext, OverrideUnityActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_REORDER_TO_FRONT
            intent.putExtra("fullscreen", _options.fullscreenEnabled)
            intent.putExtra("flutterActivity", activity.javaClass)
            activity.startActivityForResult(intent, 1)
        }
    }

    override fun onCreate(owner: LifecycleOwner) {
        if (UnityPlayerUtils.options.runImmediately && !UnityPlayerUtils.isUnityReady) {
            UnityPlayerUtils.createPlayer(getActivity(_context)!!, this, null)
        }

        owner.lifecycle.addObserver(this)
    }

    override fun onStart(owner: LifecycleOwner) {
        if (MultiWindowSupport.getAllowResizableWindow(getActivity(_context))) return
        if(UnityPlayerUtils.isUnityReady) {
            if(!UnityPlayerUtils.isUnityLoaded) {
                createPlayer()
            }
            Handler(Looper.getMainLooper()).post {
                UnityPlayerUtils.resume()
            }
        }
    }

    override fun onResume(owner: LifecycleOwner) {
        if (MultiWindowSupport.getAllowResizableWindow(getActivity(_context))) return
        if(UnityPlayerUtils.isUnityReady) {
            if(!UnityPlayerUtils.isUnityLoaded && UnityPlayerUtils.options.unloadOnDispose) {
                createPlayer()
            }

            Handler(Looper.getMainLooper()).post {
                UnityPlayerUtils.pause()
                UnityPlayerUtils.resume()
            }
        }
    }

    override fun onPause(owner: LifecycleOwner) {
        if (MultiWindowSupport.getAllowResizableWindow(getActivity(_context))) return
        if(UnityPlayerUtils.isUnityReady && UnityPlayerUtils.isUnityLoaded) {
            Handler(Looper.getMainLooper()).post {
                UnityPlayerUtils.pause()
            }
        }
    }

    override fun onStop(owner: LifecycleOwner) {
        if (MultiWindowSupport.getAllowResizableWindow(getActivity(_context))) return
        if(UnityPlayerUtils.isUnityReady && UnityPlayerUtils.isUnityLoaded) {
            Handler(Looper.getMainLooper()).post {
                UnityPlayerUtils.pause()
            }
        }
    }

    override fun onDestroy(owner: LifecycleOwner) {
        if (UnityPlayerUtils.disposed) {
            return
        }

        owner.lifecycle.removeObserver(this)
    }

    private fun destroyUnityViewIfNecessary() {
        if (UnityPlayerUtils.unityPlayer == null) {
            return
        }

        if (UnityPlayerUtils.options.unloadOnDispose && !UnityPlayerUtils.isWorking) {
            UnityPlayerUtils.unload()
            // methodChannel.setMethodCallHandler(null)
        }

        if (unityView == null) {
            return
        }

        // unityView?.removeUnityPlayer()
        unityView = null
    }

    private fun createPlayer() {
        val parInst = this
        try {
            val activity = getActivity(null)
            if (activity != null) {
                UnityPlayerUtils.isWorking = true
                UnityPlayerUtils.createPlayer(activity, this, object : OnCreateUnityViewCallback {
                    override fun onReady() {
                        UnityPlayerUtils.isUnityReady = true
                        UnityPlayerUtils.isUnityLoaded = true

                        UnityPlayerUtils.initInternalView(activity, parInst)

                        if (methodChannelResult != null) {
                            methodChannelResult!!.success(true)
                            methodChannelResult = null
                        }
                        UnityPlayerUtils.isWorking = false
                    }
                })
            }
        } catch (e: Exception) {
            UnityPlayerUtils.isWorking = false
            if (methodChannelResult != null) {
                methodChannelResult!!.error("FLUTTER_UNITY_WIDGET", e.message, e)
                methodChannelResult!!.success(false)
                methodChannelResult = null
            }
        }
    }

    private fun createPlayer(v: UnityView) {
        val parInst = this
        try {
            val activity = getActivity(null)
            if (activity != null) {
                UnityPlayerUtils.createPlayer(activity, this, object : OnCreateUnityViewCallback {
                    override fun onReady() {
                        UnityPlayerUtils.isUnityReady = true
                        UnityPlayerUtils.isUnityLoaded = true

                        UnityPlayerUtils.initInternalView(activity, parInst)
                        v.setUnityPlayer(UnityPlayerUtils.unityPlayer!!)

                        if (methodChannelResult != null) {
                            methodChannelResult!!.success(true)
                            methodChannelResult = null
                        }
                    }
                })
            }
        } catch (e: Exception) {
            if (methodChannelResult != null) {
                methodChannelResult!!.error("FLUTTER_UNITY_WIDGET", e.message, e)
                methodChannelResult!!.success(false)
                methodChannelResult = null
            }
        }
    }

    private fun getActivity(context: Context?): Activity? {
        if (context == null) {
            return UnityPlayerUtils.activity
        } else if (context is ContextWrapper) {
            return if (context is Activity) {
                context
            } else {
                getActivity(context.baseContext)
            }
        }
        return UnityPlayerUtils.activity
    }

    private fun restoreUnityUserState() {
        // restore the unity player state
//        if (!UnityPlayerUtils.isUnityLoaded &&
//                UnityPlayerUtils.isUnityReady &&
//                UnityPlayerUtils.options.unloadOnDispose
//        ) {
//            val handler = Handler()
//            handler.postDelayed({
//                if (!UnityPlayerUtils.isUnityLoaded && UnityPlayerUtils.isUnityReady) {
//                    this.createPlayer()
//                }
//            }, 300)
//        }

        // restore the unity player state
        if (UnityPlayerUtils.isUnityPaused) {
            val handler = Handler()
            handler.postDelayed({
                if (UnityPlayerUtils.unityPlayer != null) {
                    UnityPlayerUtils.pause()
                }
            }, 300)
        }
    }

    override fun onViewAttachedToWindow(v: View?) {
        restoreUnityUserState()
    }

    override fun onViewDetachedFromWindow(v: View?) {
        // restore unity
    }
}