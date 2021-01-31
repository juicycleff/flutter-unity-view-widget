package com.xraph.plugin.flutter_unity_widget

import android.app.Activity
import android.content.Context
import io.flutter.plugin.common.BinaryMessenger
import io.flutter.plugin.common.StandardMessageCodec
import io.flutter.plugin.platform.PlatformView
import io.flutter.plugin.platform.PlatformViewFactory

class FlutterUnityWidgetFactory internal constructor(
        private val binaryMessenger: BinaryMessenger,
        private var activity: Activity,
        private var lifecycleProvider: LifecycleProvider,
        ) : PlatformViewFactory(StandardMessageCodec.INSTANCE) {

    override fun create(context: Context, id: Int, args: Any): PlatformView {
        val builder = FlutterUnityWidgetBuilder()
        val params = args as Map<String, Any>

        if (params.containsKey("ar")) {
            builder.setAREnabled(params["ar"] as Boolean)
        }

        if (params.containsKey("fullscreen")) {
            builder.setFullscreenEnabled(params["fullscreen"] as Boolean)
        }

        return builder.build(
                id,
                context,
                activity,
                binaryMessenger,
                lifecycleProvider
        )
    }
}