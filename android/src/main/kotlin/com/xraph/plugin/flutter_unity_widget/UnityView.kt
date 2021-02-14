package com.xraph.plugin.flutter_unity_widget

import android.content.Context
import android.content.res.Configuration
import android.util.Log
import android.view.*
import android.widget.FrameLayout
import com.unity3d.player.UnityPlayer
import com.xraph.plugin.flutter_unity_widget.utils.SingletonHolder

class UnityView(context: Context) : FrameLayout(context) {

    companion object : SingletonHolder<UnityView, Context>(::UnityView){
        internal const val LOG_TAG = "UnityView"
    }

    var player: UnityPlayer? = null

    fun setUnityPlayer(unityPlayer: UnityPlayer) {
        player = unityPlayer
        UnityPlayerUtils.addUnityViewToGroup(this)
    }
    override fun onWindowFocusChanged(hasWindowFocus: Boolean) {
        super.onWindowFocusChanged(hasWindowFocus)
        player?.windowFocusChanged(hasWindowFocus)
    }

    override fun onConfigurationChanged(newConfig: Configuration?) {
        Log.i(LOG_TAG, "ORIENTATION CHANGED")
        player?.configurationChanged(newConfig)
        super.onConfigurationChanged(newConfig)
    }

    override fun dispatchTouchEvent(ev: MotionEvent): Boolean {
        if (player != null) {
            ev.source = InputDevice.SOURCE_TOUCHSCREEN
            player!!.injectEvent(ev)
        }
        return super.dispatchTouchEvent(ev)
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    override fun onKeyUp(keyCode: Int, event: KeyEvent?): Boolean {
        return player?.injectEvent(event) ?: true
    }

    override fun onKeyDown(keyCode: Int, event: KeyEvent?): Boolean {
        return player?.injectEvent(event) ?: true
    }

    override fun onTouchEvent(event: MotionEvent?): Boolean {
        return player?.injectEvent(event) ?: true
    }

    override fun dispatchWindowFocusChanged(hasFocus: Boolean) {
        if (player !== null) {
            player!!.dispatchWindowFocusChanged(hasFocus)
        }
        super.dispatchWindowFocusChanged(hasFocus)
    }

    override fun dispatchConfigurationChanged(newConfig: Configuration?) {
        player?.dispatchConfigurationChanged(newConfig)
        super.dispatchConfigurationChanged(newConfig)
    }
    override fun setOnLongClickListener(l: OnLongClickListener?) {
        player?.setOnLongClickListener(l)
        super.setOnLongClickListener(l)
    }

    override fun performClick(): Boolean {
        player?.performClick()
        return super.performClick()
    }

    override fun callOnClick(): Boolean {
        player?.callOnClick()
        return super.callOnClick()
    }

    override fun performLongClick(): Boolean {
        player?.performLongClick()
        return super.performLongClick()
    }

    override fun setOnKeyListener(l: OnKeyListener?) {
        player?.setOnKeyListener(l)
        super.setOnKeyListener(l)
    }

    override fun setOnGenericMotionListener(l: OnGenericMotionListener?) {
        player?.setOnGenericMotionListener(l)
        super.setOnGenericMotionListener(l)
    }

    override fun setOnHoverListener(l: OnHoverListener?) {
        player?.setOnHoverListener(l)
        super.setOnHoverListener(l)
    }

    override fun setOnDragListener(l: OnDragListener?) {
        player?.setOnDragListener(l)
        super.setOnDragListener(l)
    }

    override fun setScrollX(value: Int) {
        if (player != null) {
            player!!.scrollX = value
        }
        super.setScrollX(value)
    }

    override fun setScrollY(value: Int) {
        if (player != null) {
            player!!.scrollY = value
        }
        super.setScrollY(value)
    }

    override fun onGenericMotionEvent(event: MotionEvent?): Boolean {
        return player?.injectEvent(event) ?: true
    }

    override fun onDetachedFromWindow() {
        super.onDetachedFromWindow()
    }
}