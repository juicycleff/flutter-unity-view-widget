package com.xraph.plugins.flutterunitywidget;

import android.app.Activity;
import android.app.Application;
import android.content.Context;

import androidx.lifecycle.Lifecycle;

import java.util.Map;
import java.util.concurrent.atomic.AtomicInteger;

import io.flutter.plugin.common.BinaryMessenger;
import io.flutter.plugin.common.PluginRegistry;
import io.flutter.plugin.common.StandardMessageCodec;
import io.flutter.plugin.platform.PlatformView;
import io.flutter.plugin.platform.PlatformViewFactory;

public class FlutterUnityViewFactory extends PlatformViewFactory {
  private final PluginRegistry.Registrar mRegistrar;
  private final BinaryMessenger mBinaryMessenger;
  private final Activity mActivity;
  private final AtomicInteger mActivityState;
  private final Application mApplication;
  private final int mActivityHashCode;
  private final Lifecycle mLifecycle;

  FlutterUnityViewFactory(
      AtomicInteger state,
      BinaryMessenger binaryMessenger,
      Application application,
      Lifecycle lifecycle,
      PluginRegistry.Registrar registrar,
      Activity activity,
      int activityHashCode
  ) {
    super(StandardMessageCodec.INSTANCE);
    mActivityState = state;
    this.mBinaryMessenger = binaryMessenger;
    this.mApplication = application;
    this.mActivityHashCode = activityHashCode;
    this.mLifecycle = lifecycle;
    this.mRegistrar = registrar;
    this.mActivity = activity != null ? activity : registrar.activity();
  }

  @Override
  public PlatformView create(Context context, int id, Object args) {
    final FlutterUnityViewBuilder builder = new FlutterUnityViewBuilder();

    Map<String, Object> params = (Map<String, Object>) args;
    if (params.containsKey("ar")) {
      UnityUtils.getOptions().setSafeModeEnabled((boolean) params.get("ar"));
      builder.setAREnabled((boolean) params.get("ar"));
    }

    if (params.containsKey("safeMode")) {
      UnityUtils.getOptions().setSafeModeEnabled((boolean) params.get("safeMode"));
      builder.setSafeModeEnabled((boolean) params.get("safeMode"));
    }

    if (params.containsKey("fullscreen")) {
      UnityUtils.getOptions().setSafeModeEnabled((boolean) params.get("fullscreen"));
      builder.setFullscreenEnabled((boolean) params.get("fullscreen"));
    }

    return builder.build(
        id,
        context,
        mActivityState,
        mBinaryMessenger,
        mApplication,
        mLifecycle,
        mRegistrar,
        mActivityHashCode,
        mActivity
    );
  }
}
