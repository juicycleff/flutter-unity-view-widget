package com.xraph.plugin.flutter_unity_widget

import android.content.Context
import io.flutter.plugin.common.BinaryMessenger
import io.flutter.plugin.common.StandardMessageCodec
import io.flutter.plugin.platform.PlatformView
import io.flutter.plugin.platform.PlatformViewFactory

class FlutterUnityWidgetFactory(
        private val binaryMessenger: BinaryMessenger,
        private val appContext: Context,
        private var lifecycleProvider: LifecycleProvider
        ) : PlatformViewFactory(StandardMessageCodec.INSTANCE) {

    override fun create(context: Context, id: Int, args: Any): PlatformView {
        val builder = FlutterUnityWidgetBuilder()
        val params = args as Map<*, *>

        if (params.containsKey("fullscreen")) {
            builder.setFullscreenEnabled(params["fullscreen"] as Boolean)
        }

        return builder.build(
                id,
                context,
                appContext,
                binaryMessenger,
                lifecycleProvider
        )
    }
}