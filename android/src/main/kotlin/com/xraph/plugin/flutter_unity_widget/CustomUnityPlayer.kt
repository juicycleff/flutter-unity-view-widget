package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.content.res.Configuration
import android.util.Log
import com.unity3d.player.IUnityPlayerLifecycleEvents
import com.unity3d.player.UnityPlayerForActivityOrService

@SuppressLint("NewApi")
class CustomUnityPlayer(context: Activity, upl: IUnityPlayerLifecycleEvents?) : UnityPlayerForActivityOrService(context, upl) {

    companion object {
        internal const val LOG_TAG = "CustomUnityPlayer"
    }

    // former FrameLayout override functions moved to CustomFrameLayout and UnityPlayerUtils (unityAttachListener).
}