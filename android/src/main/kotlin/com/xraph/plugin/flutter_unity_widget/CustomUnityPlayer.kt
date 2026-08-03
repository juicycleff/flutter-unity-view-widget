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

        /// Every MotionEvent handed to Unity is retagged to this deviceId so a
        /// gesture's press and release can never land on two different Unity
        /// input devices. -1 is the value the original Flutter workaround used,
        /// and is known to be picked up by Unity's Input System.
        private const val NORMALIZED_DEVICE_ID = -1
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

        // Normalize deviceId on EVERY event, not just deviceId == 0.
        //
        // Android does not guarantee a stable deviceId across one gesture here:
        // measured on device, a single drag arrived as DOWN(devId=5) followed by
        // UP(devId=0) with the same downTime. The original `if (deviceId == 0)`
        // rewrite then sent the press to Unity input device 5 and the release to
        // device -1, so device 5's touch was never released. <Touchscreen>/Press
        // latched pressed forever, and every action bound to it — all 3D object
        // interaction — went silent, while <Pointer>-bound UGUI kept working.
        //
        // Rewriting unconditionally keeps a gesture's DOWN/MOVE/UP on one device.
        if (event.deviceId != NORMALIZED_DEVICE_ID) {
            /*
              Flutter creates touchscreen motion events with deviceId 0
              (https://github.com/flutter/flutter/blob/34b454f4/packages/flutter/lib/src/services/platform_views.dart#L639),
              which Unity's Input System does not pick up. deviceId is immutable,
              so the event has to be copied to change it.
            */
            val modifiedEvent = event.copy(deviceId = NORMALIZED_DEVICE_ID)
            // Do NOT recycle `event`. View.onTouchEvent does not take ownership —
            // ViewRootImpl still owns it and recycles it after dispatch. Recycling
            // here returned it to the pool twice, so a later MotionEvent.obtain()
            // could hand the same instance to two owners. That was a real bug, but
            // measured on device it was NOT the cause of the input freeze; the
            // deviceId split above was.
            //
            // `modifiedEvent` is intentionally left unrecycled: Unity may retain it
            // past this call, and recycling it here would reintroduce a genuine
            // use-after-free. An un-recycled obtain() is only a missed pool return.
            return super.onTouchEvent(modifiedEvent)
        } else {
            return super.onTouchEvent(event)
        }
    }

}