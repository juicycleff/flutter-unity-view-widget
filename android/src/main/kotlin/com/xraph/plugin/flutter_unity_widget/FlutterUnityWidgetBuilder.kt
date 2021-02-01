package com.xraph.plugin.flutter_unity_widget

import android.app.Activity
import android.content.Context
import io.flutter.plugin.common.BinaryMessenger

class FlutterUnityWidgetBuilder : FlutterUnityWidgetOptionsSink {
    private val options = FlutterUnityWidgetOptions()

    internal fun build(
            id: Int,
            context: Context,
            activity: Activity,
            binaryMessenger: BinaryMessenger,
            lifecycle: LifecycleProvider,
    ): FlutterUnityWidgetController {
        return FlutterUnityWidgetController(
                id,
                context,
                activity,
                binaryMessenger,
                lifecycle,
                options,
        )
    }

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        options.fullscreenEnabled = fullscreenEnabled
    }
}