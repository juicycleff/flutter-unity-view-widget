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
        var controller = FlutterUnityWidgetController(
                id,
                context,
                activity,
                binaryMessenger,
                lifecycle,
                options,
        )
        controller.bootstrap()

        return controller
    }

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        options.fullscreenEnabled = fullscreenEnabled
    }
}