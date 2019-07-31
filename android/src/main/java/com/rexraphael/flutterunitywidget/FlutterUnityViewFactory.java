package com.rexraphael.flutterunitywidget;

// import android.app.Activity;
import android.content.Context;

// import io.flutter.plugin.common.BinaryMessenger;
import java.util.Map;

import io.flutter.plugin.common.PluginRegistry;
import io.flutter.plugin.common.StandardMessageCodec;
import io.flutter.plugin.platform.PlatformView;
import io.flutter.plugin.platform.PlatformViewFactory;

public class FlutterUnityViewFactory extends PlatformViewFactory {
    private final PluginRegistry.Registrar mPluginRegistrar;
    // private final BinaryMessenger messenger;
    // private final Activity activity;

    public FlutterUnityViewFactory(PluginRegistry.Registrar registrar) {
        super(StandardMessageCodec.INSTANCE);
        mPluginRegistrar = registrar;
        // this.messenger = messenger;
        // this.activity = activity;
    }

    @Override
    public PlatformView create(Context context, int i, Object args) {
        Map<String, Object> params = (Map<String, Object>) args;

        if (params.containsKey("ar")) {
            UnityUtils.isAR = (boolean) params.get("ar");
        }

        return new FlutterUnityView(context, mPluginRegistrar, i);
    }
}
