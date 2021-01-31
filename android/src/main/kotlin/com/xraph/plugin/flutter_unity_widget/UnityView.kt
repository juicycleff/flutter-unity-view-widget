package com.xraph.plugin.flutter_unity_widget

import android.content.Context
import android.content.res.Configuration
import android.view.*
import android.widget.FrameLayout
import com.unity3d.player.UnityPlayer
import com.xraph.plugin.flutter_unity_widget.utils.SingletonHolder
import io.flutter.Log

class UnityView(context: Context) : FrameLayout(context) {

    companion object : SingletonHolder<UnityView, Context>(::UnityView){
        internal const val LOG_TAG = "UnityView"
    }

    lateinit var player: UnityPlayer

    /* fun postMessage(gameObject: String?, methodName: String?, message: String?) {
        if (!isUnityReady) {
            return
        }
        UnityPlayer.UnitySendMessage(gameObject, methodName, message)
    }

    fun pause() {
        if (player != null && isUnityLoaded && !isUnityPaused) {
            player.pause()
            isUnityPaused = true
        }
    }

    fun resume() {
        if (player != null) {
            player.resume()
            isUnityPaused = false
        }
    }

    fun unload() {
        if (player != null) {
            player.unload()
            isUnityLoaded = false
        }
    }

    fun quitPlayer() {
        try {
            if (player != null) {
                isUnityLoaded = false
                isUnityReady = false
                player.quit()
            }
        } catch (e: Error) {
            Log.e(LOG_TAG, e.message!!)
        }
    }

    fun setUnityPlayer(pl: UnityPlayer) {
        player = pl
    }

    fun addUnityViewToGroup(group: ViewGroup) {
        if (player == null) {
            return
        }
        if (player.parent != null) {
            (player.parent as ViewGroup).removeView(player)
        }
        val layoutParams = ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT)
        group.addView(player, 0, layoutParams)
        player.windowFocusChanged(true)
        player.requestFocus()
        player.resume()
    }

    fun addUnityViewToBackground(activity: Activity) {
        if (player == null) {
            return
        }
        if (player.parent != null) {
            (player.parent as ViewGroup).removeView(player)
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            player.z = -1f
        }
        val layoutParams = ViewGroup.LayoutParams(1, 1)
        activity.addContentView(player, layoutParams)
        isUnityInBackground = true
    }
     */

    override fun onWindowFocusChanged(hasWindowFocus: Boolean) {
        super.onWindowFocusChanged(hasWindowFocus)
        if (player != null) {
            player.windowFocusChanged(hasWindowFocus)
        }
    }

    override fun onConfigurationChanged(newConfig: Configuration?) {
        Log.i(LOG_TAG, "ORIENTATION CHANGED")
        if (player != null) {
            player.configurationChanged(newConfig)
        }
        super.onConfigurationChanged(newConfig)
    }

    override fun dispatchTouchEvent(ev: MotionEvent): Boolean {
        if (player != null) {
            ev.source = InputDevice.SOURCE_TOUCHSCREEN
            player.injectEvent(ev)
        }
        return super.dispatchTouchEvent(ev)
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    override fun onKeyUp(keyCode: Int, event: KeyEvent?): Boolean {
        return if (player != null) {
            player.injectEvent(event)
        } else true
    }

    override fun onKeyDown(keyCode: Int, event: KeyEvent?): Boolean {
        return if (player != null) {
            player.injectEvent(event)
        } else true
    }

    override fun onTouchEvent(event: MotionEvent?): Boolean {
        return if (player != null) {
            player.injectEvent(event)
        } else true
    }

    override fun dispatchWindowFocusChanged(hasFocus: Boolean) {
        if (player !== null) {
            player.dispatchWindowFocusChanged(hasFocus)
        }
        super.dispatchWindowFocusChanged(hasFocus)
    }

    override fun dispatchConfigurationChanged(newConfig: Configuration?) {
        if (player != null) {
            player.dispatchConfigurationChanged(newConfig)
        }
        super.dispatchConfigurationChanged(newConfig)
    }
    override fun setOnLongClickListener(l: OnLongClickListener?) {
        if (player != null) {
            player.setOnLongClickListener(l)
        }
        super.setOnLongClickListener(l)
    }

    override fun performClick(): Boolean {
        if (player != null) {
            player.performClick()
        }
        return super.performClick()
    }

    override fun callOnClick(): Boolean {
        if (player != null) {
            player.callOnClick()
        }
        return super.callOnClick()
    }

    override fun performLongClick(): Boolean {
        if (player != null) {
            player.performLongClick()
        }
        return super.performLongClick()
    }

    override fun setOnKeyListener(l: OnKeyListener?) {
        if (player != null) {
            player.setOnKeyListener(l)
        }
        super.setOnKeyListener(l)
    }

    override fun setOnGenericMotionListener(l: OnGenericMotionListener?) {
        if (player != null) {
            player.setOnGenericMotionListener(l)
        }
        super.setOnGenericMotionListener(l)
    }

    override fun setOnHoverListener(l: OnHoverListener?) {
        if (player != null) {
            player.setOnHoverListener(l)
        }
        super.setOnHoverListener(l)
    }

    override fun setOnDragListener(l: OnDragListener?) {
        if (player != null) {
            player.setOnDragListener(l)
        }
        super.setOnDragListener(l)
    }

    override fun setScrollX(value: Int) {
        if (player != null) {
            player.scrollX = value
        }
        super.setScrollX(value)
    }

    override fun setScrollY(value: Int) {
        if (player != null) {
            player.scrollY = value
        }
        super.setScrollY(value)
    }

    override fun onGenericMotionEvent(event: MotionEvent?): Boolean {
        return if (player != null) {
            player.injectEvent(event)
        } else true
    }

    override fun onDetachedFromWindow() {
        super.onDetachedFromWindow()
    }
}