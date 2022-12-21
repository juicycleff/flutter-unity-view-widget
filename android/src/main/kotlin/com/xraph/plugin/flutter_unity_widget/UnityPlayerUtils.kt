package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Intent
import android.os.Build
import android.os.Handler
import android.os.Looper
import android.util.Log
import android.view.View
import android.view.ViewGroup
import android.view.WindowManager
import android.widget.FrameLayout
import com.unity3d.player.IUnityPlayerLifecycleEvents
import com.unity3d.player.UnityPlayer
import java.util.concurrent.CopyOnWriteArraySet


class UnityPlayerUtils {

    companion object {
        private const val LOG_TAG = "UnityPlayerUtils"
        var controllers = mutableMapOf<String, FlutterUnityWidgetController>()
        var unityPlayer: CustomUnityPlayer? = null
        var activity: Activity? = null
        var prevActivityRequestedOrientation: Int? = null

        var options: FlutterUnityWidgetOptions = FlutterUnityWidgetOptions()

        var unityPaused: Boolean = false
        var unityLoaded: Boolean = false
        var viewStaggered: Boolean = false

        private val mUnityEventListeners = CopyOnWriteArraySet<UnityEventListener>()

        fun focus() {
            try {
                unityPlayer!!.windowFocusChanged(unityPlayer!!.requestFocus())
                unityPlayer!!.resume()
            } catch (e: Exception) {
                Log.e(LOG_TAG, e.toString())
            }
        }

        private fun performWindowUpdate() {
            if (!options.fullscreenEnabled) {
                activity!!.window.addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN)
                activity!!.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
            } else {
                activity!!.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
                activity!!.window.decorView.systemUiVisibility = (View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                        or View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN)
            }
        }

        /**
         * Create a new unity player with callback
         */
        @SuppressLint("NewApi")
        fun createUnityPlayer(ule: IUnityPlayerLifecycleEvents, callback: OnCreateUnityViewCallback?) {
            if (activity == null) {
                throw java.lang.Exception("Unity activity is null")
            }

            if (unityPlayer != null) {
                unityLoaded = true
                unityPlayer!!.bringToFront()
                unityPlayer!!.requestLayout()
                unityPlayer!!.invalidate()
                focus()
                callback?.onReady()
                performWindowUpdate()
                return
            }

            try {
                unityPlayer = CustomUnityPlayer(activity!!, ule)
                // unityPlayer!!.z = (-1).toFloat()
                // addUnityViewToBackground(activity!!)
                unityLoaded = true

                DataStreamEventNotifier.notifier.onNext(
                    DataStreamEvent(
                        DataStreamEventTypes.OnUnityPlayerCreated.name,
                        true,
                    )
                )

                if (!options.fullscreenEnabled) {
                    activity!!.window.addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN)
                    activity!!.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
                } else {
                    activity!!.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
                    activity!!.window.decorView.systemUiVisibility = (View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                            or View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN)
                }

                focus()
                callback?.onReady()
            } catch (e: Exception) {
                Log.e(LOG_TAG, e.toString())
            }
        }

        fun postMessage(gameObject: String, methodName: String, message: String) {
            UnityPlayer.UnitySendMessage(gameObject, methodName, message)
        }

        fun pause() {
            try {
                if (unityPlayer != null) {
                    unityPlayer!!.pause()
                    unityPaused = true
                }
            } catch (e: Exception) {
                Log.e(LOG_TAG, e.toString())
            }
        }

        fun resume() {
            try {
                if (unityPlayer != null) {
                    unityPlayer!!.resume()
                    unityPaused = false
                }
            } catch (e: Exception) {
                Log.e(LOG_TAG, e.toString())
            }
        }

        fun unload() {
            try {
                if (unityPlayer != null) {
                    unityPlayer!!.unload()
                    unityLoaded = false
                }
            } catch (e: Exception) {
                Log.e(LOG_TAG, e.toString())
            }
        }

        fun quitPlayer() {
            try {
                if (unityPlayer != null) {
                    unityPlayer!!.quit()
                    unityLoaded = false
                }
            } catch (e: Error) {
                e.message?.let { Log.e(LOG_TAG, it) }
            }
        }

        /**
         * Invoke by unity C#
         */
        @JvmStatic
        fun onUnitySceneLoaded(name: String, buildIndex: Int, isLoaded: Boolean, isValid: Boolean) {
            try {
                handleSceneLoaded(name, buildIndex, isLoaded, isValid)
            } catch (e: Exception) {
                e.message?.let { Log.e(LOG_TAG, it) }
            }
        }

        fun handleSceneLoaded(name: String, buildIndex: Int, isLoaded: Boolean, isValid: Boolean) {
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

        /**
         * Invoke by unity C#
         */
        @JvmStatic
        fun onUnityMessage(message: String) {
            Log.d("UnityListener", "total listeners are ${mUnityEventListeners.size}")
            Handler(Looper.getMainLooper()).post {
                DataStreamEventNotifier.notifier.onNext(
                    DataStreamEvent(
                        DataStreamEventTypes.OnUnityMessage.name,
                        message,
                    )
                )
            }
        }

        fun addUnityEventListener(listener: UnityEventListener) {
            mUnityEventListeners.add(listener)
        }

        fun removeUnityEventListener(listener: UnityEventListener) {
            mUnityEventListeners.remove(listener)
        }

        private fun shakeActivity() {
            unityPlayer?.windowFocusChanged(true)
            if (prevActivityRequestedOrientation != null) {
                activity?.requestedOrientation = prevActivityRequestedOrientation!!
            }
        }

        fun removePlayer(controller: FlutterUnityWidgetController) {
            if (unityPlayer!!.parent == controller.view) {
                if (controllers.isEmpty()) {
                    (controller.view as FrameLayout).removeView(unityPlayer)
                    pause()
                    shakeActivity()
                } else {
                    val controllersRefs = controllers.values.toList()
                    controllersRefs[controllersRefs.size - 1].reattachToView()
                }
            }
        }

        fun reset() {
            unityLoaded = false
        }

        fun addUnityViewToGroup(group: ViewGroup) {
             val layoutParams = FrameLayout.LayoutParams(FrameLayout.LayoutParams.WRAP_CONTENT, FrameLayout.LayoutParams.WRAP_CONTENT)
//             val layoutParams = ViewGroup.LayoutParams(LayoutParams.FILL_PARENT, LayoutParams.FILL_PARENT)
//            val layoutParams = ViewGroup.LayoutParams(570, 770)
            group.addView(unityPlayer, layoutParams)
        }

        fun addUnityViewToBackground() {
            if (unityPlayer == null) {
                return
            }
            if (unityPlayer!!.parent != null) {
                (unityPlayer!!.parent as ViewGroup).removeView(unityPlayer)
            }
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
                unityPlayer!!.z = -1f
            }
            val layoutParams = ViewGroup.LayoutParams(1, 1)
            activity!!.addContentView(unityPlayer, layoutParams)
        }

        fun openNativeUnity() {
            if (activity == null) { return }

            val intent = Intent(activity, OverrideUnityActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_REORDER_TO_FRONT
            intent.putExtra("fullscreen", options.fullscreenEnabled)
            intent.putExtra("flutterActivity", activity?.javaClass)
            activity?.startActivityForResult(intent, 1)
        }
    }
}