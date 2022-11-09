package com.xraph.plugin.flutter_unity_widget

import android.content.Context
import io.flutter.plugin.common.BinaryMessenger
import io.flutter.plugin.common.MethodChannel

class FlutterUnityWidgetBuilder : FlutterUnityWidgetOptionsSink {
    private val options = FlutterUnityWidgetOptions()

    fun build(
        id: Int,
        context: Context?,
        methodChannel: MethodChannel,
        lifecycle: LifecycleProvider
    ): FlutterUnityWidgetController {
        UnityPlayerUtils.options = options
        val controller = FlutterUnityWidgetController(
                id,
                context,
                methodChannel,
                lifecycle
        )
        controller.bootstrap()

        return controller
    }

    override fun setFullscreenEnabled(fullscreenEnabled: Boolean) {
        options.fullscreenEnabled = fullscreenEnabled
        UnityPlayerUtils.options.fullscreenEnabled = fullscreenEnabled
    }

    override fun setHideStatusBar(hideStatusBar: Boolean) {
        options.hideStatus = hideStatusBar
        UnityPlayerUtils.options.hideStatus = hideStatusBar
    }

    override fun setRunImmediately(runImmediately: Boolean) {
        options.runImmediately = runImmediately
        UnityPlayerUtils.options.runImmediately = runImmediately
    }

    override fun setUnloadOnDispose(unloadOnDispose: Boolean) {
        options.unloadOnDispose = unloadOnDispose
        UnityPlayerUtils.options.unloadOnDispose = unloadOnDispose
    }
}