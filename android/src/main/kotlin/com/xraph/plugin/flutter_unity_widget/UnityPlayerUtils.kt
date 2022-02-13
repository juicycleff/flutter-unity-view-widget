package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Context
import android.os.Build
import android.os.Handler
import android.os.Looper
import android.util.Log
import android.view.View
import android.view.ViewGroup
import android.view.ViewGroup.LayoutParams.MATCH_PARENT
import android.view.WindowManager
import com.unity3d.player.IUnityPlayerLifecycleEvents
import com.unity3d.player.UnityPlayer
import java.util.concurrent.CopyOnWriteArraySet


class UnityPlayerUtils {

    companion object {
        private const val LOG_TAG = "UnityPlayerUtils"

        var activity: Activity? = null
        var options: FlutterUnityWidgetOptions = FlutterUnityWidgetOptions()
        var unityPlayer: UnityPlayer? = null
        var applicationContext: Context? = null
        // var unityView: UnityView? = null

        var isWorking: Boolean = false
        var isUnityReady: Boolean = false
        var isUnityPaused: Boolean = false
        var isUnityLoaded: Boolean = false
        var disposed: Boolean = false
        var isUnityInBackground: Boolean = false

        private val mUnityEventListeners = CopyOnWriteArraySet<UnityEventListener>()

        /**
         * Create a new unity player with callback
         */
        @SuppressLint("NewApi")
        fun initInternalView(context: Activity, lst: View.OnAttachStateChangeListener): UnityView {
            val unityView = UnityView(context)
            unityView.addOnAttachStateChangeListener(lst)
            return  unityView
        }

        private fun focus() {
            unityPlayer!!.windowFocusChanged(true)
            unityPlayer!!.requestFocus()
            unityPlayer!!.resume()
        }

        /**
         * Create a new unity player with callback
         */
        @SuppressLint("NewApi")
        fun createPlayer(context: Activity, ule: IUnityPlayerLifecycleEvents, callback: OnCreateUnityViewCallback?) {
            if (unityPlayer != null) {
                callback?.onReady()
                return
            }

            try {
                 if (!Looper.getMainLooper().thread.isAlive) return
                Handler(Looper.getMainLooper()).post {
                // context.runOnUiThread {
                    // context.window?.setFormat(PixelFormat.RGBA_8888)
                    unityPlayer = UnityPlayer(context, ule)

                    // wait a moment. fix unity cannot start when startup.
                    try {
                        // wait a moment. fix unity cannot start when startup.
                        Thread.sleep(1000)
                    } catch (e: java.lang.Exception) {
                    }

                    addUnityViewToBackground()
                    focus()

                    if (!options.fullscreenEnabled) {
                        context.window.addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN);
                        context.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
                    } else {
                        context.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
                    }

                    // restore window layout
                    /* if (!options.fullscreenEnabled) {
                        activity!!.window.addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN)
                        activity!!.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
                        activity!!.window.addFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
                    } else  {
                        activity!!.window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
                        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
                            activity!!.window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
                        }

                        activity!!.window.apply {
                            // decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
                                decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_LIGHT_STATUS_BAR
                            } else {
                                decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                            }
                            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
                                statusBarColor = Color.TRANSPARENT
                            }
                        }

                        // WindowCompat.setDecorFitsSystemWindows(activity!!.window, false)
                    }

                    // restore window layout
                    if (options.hideStatus) {
                        hideStatusBar()
                    }
                     */
                    isUnityReady = true
                    callback?.onReady()
                }
            } catch (e: Exception) {
                Log.e(LOG_TAG, e.toString())
            }
        }

        fun postMessage(gameObject: String, methodName: String, message: String) {
            if (!isUnityReady) {
                return
            }
            UnityPlayer.UnitySendMessage(gameObject, methodName, message)
        }

        fun pause() {
            if (unityPlayer != null && isUnityLoaded) {
                unityPlayer!!.pause()
                isUnityPaused = true
            }
        }

        fun resume() {
            if (unityPlayer != null) {
                unityPlayer!!.resume()
                isUnityPaused = false
            }
        }

        fun unload() {
            if (unityPlayer != null) {
                unityPlayer!!.unload()
                isUnityLoaded = false
            }
        }

        fun moveToBackground() {
            if (unityPlayer != null) {
                isUnityInBackground = true
            }
        }

        @SuppressLint("NewApi")
        private fun hideStatusBar() {
            // window.decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_FULLSCREEN
            if (unityPlayer != null) {
                unityPlayer!!.systemUiVisibility =  View.SYSTEM_UI_FLAG_FULLSCREEN
            }
        }

        fun quitPlayer() {
            try {
                if (unityPlayer != null) {
                    unityPlayer!!.quit()
                    isUnityLoaded = false
                    isUnityReady = false
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
            for (listener in mUnityEventListeners) {
                try {
                    listener.onSceneLoaded(name, buildIndex, isLoaded, isValid)
                } catch (e: Exception) {
                    e.message?.let { Log.e(LOG_TAG, it) }
                }
            }
        }

        /**
         * Invoke by unity C#
         */
        @JvmStatic
        fun onUnityMessage(message: String) {
            for (listener in mUnityEventListeners) {
                try {
                    listener.onMessage(message)
                } catch (e: Exception) {
                    e.message?.let { Log.e(LOG_TAG, it) }
                }
            }
        }

        fun addUnityEventListener(listener: UnityEventListener) {
            mUnityEventListeners.add(listener)
        }

        fun removeUnityEventListener(listener: UnityEventListener) {
            mUnityEventListeners.remove(listener)
        }

        private fun addUnityViewToBackground(activity: Activity) {
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
            activity.addContentView(unityPlayer, layoutParams)
            isUnityInBackground = true
        }

//        fun addUnityViewToGroup(group: ViewGroup) {
//            if (unityPlayer == null) {
//                return
//            }
//
//            if (unityPlayer!!.parent != null) {
//                (unityPlayer!!.parent as ViewGroup).removeView(unityPlayer)
//            }
//
//            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
//                unityPlayer!!.z = -1f
//            }
//
//            val layoutParams = ViewGroup.LayoutParams(MATCH_PARENT, MATCH_PARENT)
//            group.addView(unityPlayer, 0, layoutParams)
//
//            unityPlayer!!.windowFocusChanged(true)
//            unityPlayer!!.requestFocus()
//            unityPlayer!!.resume()
//        }

        fun removeUnityViewFromGroup(group: ViewGroup) {
            if (unityPlayer == null) {
                return
            }

            if (unityPlayer!!.parent != null && group.childCount > 0) {
                group.removeView(unityPlayer)
            }
        }

        fun removeUnityViewFromGroup() {
            if (unityPlayer == null) {
                return
            }

            if (unityPlayer!!.parent != null) {
                (unityPlayer!!.parent as ViewGroup).removeView(unityPlayer)
            }
        }

        // new
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
            val activity = unityPlayer!!.context as Activity
            val layoutParams = ViewGroup.LayoutParams(1, 1)
            activity.addContentView(unityPlayer, layoutParams)
        }

        fun addUnityViewToGroup(group: ViewGroup) {
            if (unityPlayer == null) {
                return
            }
            if (unityPlayer!!.parent != null) {
                (unityPlayer!!.parent as ViewGroup).removeView(unityPlayer)
            }
            val layoutParams = ViewGroup.LayoutParams(MATCH_PARENT, MATCH_PARENT)
            group.addView(unityPlayer, 0, layoutParams)
            focus()
        }
    }
}