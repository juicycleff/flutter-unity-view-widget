package com.xraph.plugin.flutter_unity_widget

import android.app.Activity
import android.graphics.PixelFormat
import android.os.Build
import android.os.Handler
import android.os.Looper
import android.util.Log
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

        var isUnityReady: Boolean = false
        var isUnityPaused: Boolean = false
        var isUnityLoaded: Boolean = false
        var isUnityInBackground: Boolean = false

        private val mUnityEventListeners = CopyOnWriteArraySet<UnityEventListener>()

        /**
         * Create a new unity player with callback
         */
        fun createPlayer(activity: Activity, ule: IUnityPlayerLifecycleEvents, reInitialize: Boolean, callback: OnCreateUnityViewCallback?) {
            if (unityPlayer != null && !reInitialize) {
                callback?.onReady()
                return
            }

            try {
                Handler(Looper.getMainLooper()).post {
                    if (!reInitialize) {
                        activity.window.setFormat(PixelFormat.RGBA_8888)
                        unityPlayer = UnityPlayer(activity, ule)
                    }

                    try {
                        if (!reInitialize) {
                            // wait a moment. fix unity cannot start when startup.
                            Thread.sleep(700)
                        }
                    } catch (e: Exception) {
                    }

                    // start unity
                    if (!reInitialize) {
                        addUnityViewToBackground(activity)
                        unityPlayer!!.windowFocusChanged(true)
                        unityPlayer!!.requestFocus()
                        unityPlayer!!.resume()

                        // restore window layout
                        if (!options.fullscreenEnabled) {
                            activity.window.addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN)
                            activity.window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN)
                            if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.KITKAT) {
                                activity.window.addFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
                            }
                        }
                    }

                    isUnityReady = true
                    isUnityLoaded = true

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

        fun addUnityViewToBackground(activity: Activity) {
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

        fun restoreUnityViewFromBackground(activity: Activity) {
            if (unityPlayer == null) {
                return
            }

            if (unityPlayer!!.parent != null) {
                (unityPlayer!!.parent as ViewGroup).addView(unityPlayer)
            }

            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
                unityPlayer!!.z = 1f
            }

            val layoutParams = ViewGroup.LayoutParams(1, 1)
            activity.addContentView(unityPlayer, layoutParams)
            isUnityInBackground = false
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

            unityPlayer!!.windowFocusChanged(true)
            unityPlayer!!.requestFocus()
            unityPlayer!!.resume()
        }
    }
}