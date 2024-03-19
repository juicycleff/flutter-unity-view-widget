package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.content.res.Configuration
import android.util.Log
import android.view.InputDevice
import android.view.MotionEvent
import com.unity3d.player.IUnityPlayerLifecycleEvents
import com.unity3d.player.UnityPlayer

@SuppressLint("NewApi")
class CustomUnityPlayer(context: Activity, upl: IUnityPlayerLifecycleEvents?) : UnityPlayer(context, upl) {

    companion object {
        internal const val LOG_TAG = "CustomUnityPlayer"
    }

    override fun onConfigurationChanged(newConfig: Configuration?) {
        Log.i(LOG_TAG, "ORIENTATION CHANGED")
        super.onConfigurationChanged(newConfig)
    }

    override fun onAttachedToWindow() {
        Log.i(LOG_TAG, "onAttachedToWindow")
        super.onAttachedToWindow()
        UnityPlayerUtils.resume()
        UnityPlayerUtils.pause()
        UnityPlayerUtils.resume()
    }

    override fun onDetachedFromWindow() {
        Log.i(LOG_TAG, "onDetachedFromWindow")
        // todo: fix more than one unity view, don't add to background.
//        UnityPlayerUtils.addUnityViewToBackground()
        super.onDetachedFromWindow()
    }

    override fun dispatchTouchEvent(ev: MotionEvent): Boolean {
        ev.source = InputDevice.SOURCE_TOUCHSCREEN
        return super.dispatchTouchEvent(ev)
    }

    @SuppressLint("ClickableViewAccessibility")
    override fun onTouchEvent(event: MotionEvent?): Boolean{
        if (event == null) return false

        event.source = InputDevice.SOURCE_TOUCHSCREEN
        
        // true for Flutter Virtual Display, false for Hybrid composition.
        if (event.deviceId == 0) {        
            /* 
              Flutter creates a touchscreen motion event with deviceId 0. (https://github.com/flutter/flutter/blob/34b454f42dd6f8721dfe43fc7de5d215705b5e52/packages/flutter/lib/src/services/platform_views.dart#L639)
              Unity's new Input System package does not detect these touches, copy the motion event to change the immutable deviceId.
            */
            val modifiedEvent = event.copy(deviceId = -1)
            event.recycle()
            return super.onTouchEvent(modifiedEvent)
        } else {
            return super.onTouchEvent(event)
        }
    }

}