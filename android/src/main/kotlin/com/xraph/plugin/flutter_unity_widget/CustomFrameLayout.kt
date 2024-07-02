package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.util.Log
import android.view.InputDevice
import android.view.MotionEvent
import com.xraph.plugin.flutter_unity_widget.UnityPlayerUtils
import android.content.Context
import android.content.res.Configuration
import android.widget.FrameLayout

// These functions used to be in CustomUnityPlayer as UnityPlayer used to extend FrameLayout in Unity < 2023.
// We now use these on FlutterUnityWidgetController's framelayout, which is 1 parent up in the hierarchy.


public class CustomFrameLayout : FrameLayout  {

    constructor(context: Context) : super(context)


    companion object {
        internal const val LOG_TAG = "CustomUnityFrameLayout"
    }

    override fun onConfigurationChanged(newConfig: Configuration?) {
        Log.i(LOG_TAG, "ORIENTATION CHANGED")
        super.onConfigurationChanged(newConfig)
    }

    override fun dispatchTouchEvent(event: MotionEvent): Boolean {
        // Log.i(LOG_TAG, "dispatchTouch")
        event.source = InputDevice.SOURCE_TOUCHSCREEN

        // instead of modifying the event in Unity onTouchEvent, intercept it before Unity gets it.

         // true for Flutter Virtual Display, false for Hybrid composition.
        if (event.deviceId == 0) {        
             
            //  Flutter creates a touchscreen motion event with deviceId 0. (https://github.com/flutter/flutter/blob/34b454f42dd6f8721dfe43fc7de5d215705b5e52/packages/flutter/lib/src/services/platform_views.dart#L639)
            //  Unity's new Input System package does not detect these touches, copy the motion event to change the immutable deviceId.
            
            val modifiedEvent = event.copy(deviceId = -1)
            event.recycle()
            return super.dispatchTouchEvent(modifiedEvent)
        } else {
            return super.dispatchTouchEvent(event)
        }
    }

}
