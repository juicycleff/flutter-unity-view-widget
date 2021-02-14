package com.xraph.plugin.flutter_unity_widget

import android.content.Context
import io.flutter.plugin.common.BinaryMessenger

class FlutterUnityWidgetBuilder : FlutterUnityWidgetOptionsSink {
    private val options = FlutterUnityWidgetOptions()

    fun build(
            id: Int,
            context: Context,
            appContext: Context,
            binaryMessenger: BinaryMessenger,
            lifecycle: LifecycleProvider
    ): FlutterUnityWidgetController {
        val controller = FlutterUnityWidgetController(
                id,
                context,
                appContext,
                binaryMessenger,
                lifecycle,
                options
        )
        controller.bootstrap()

        return controller
    }

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        options.fullscreenEnabled = fullscreenEnabled
    }
}