package com.xraph.plugins.flutterunitywidget;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Application;
import android.os.Build;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.lifecycle.Lifecycle;
import androidx.lifecycle.DefaultLifecycleObserver;
import androidx.lifecycle.LifecycleOwner;

import java.util.concurrent.atomic.AtomicInteger;

import io.flutter.embedding.engine.plugins.FlutterPlugin;
import io.flutter.embedding.engine.plugins.activity.ActivityAware;
import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding;
import io.flutter.plugin.common.PluginRegistry;
import io.flutter.embedding.engine.plugins.lifecycle.FlutterLifecycleAdapter;

/** FlutterUnityWidgetPlugin */
@SuppressLint("NewApi")
public class FlutterUnityWidgetPlugin implements Application.ActivityLifecycleCallbacks,
        FlutterPlugin,
        ActivityAware,
        DefaultLifecycleObserver {

  static final String LOG_TAG = "FlutterUnityWidgetPlugin";
  static final int CREATED = 1;
  static final int STARTED = 2;
  static final int RESUMED = 3;
  static final int PAUSED = 4;
  static final int STOPPED = 5;
  static final int DESTROYED = 6;
  private final AtomicInteger state = new AtomicInteger(0);
  private int registrarActivityHashCode;
  private FlutterPluginBinding pluginBinding;
  private Lifecycle lifecycle;
  private static  PluginRegistry.Registrar mRegistrar;

  private static final String VIEW_TYPE = "plugins.xraph.com/unity_view";

  public static void registerWith(PluginRegistry.Registrar registrar) {
    if (registrar.activity() == null) {
      // When a background flutter view tries to register the plugin, the registrar has no activity.
      // We stop the registration process as this plugin is foreground only.
      return;
    }

    mRegistrar = registrar;
    final FlutterUnityWidgetPlugin plugin = new FlutterUnityWidgetPlugin(registrar.activity());
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.ICE_CREAM_SANDWICH) {
      registrar.activity().getApplication().registerActivityLifecycleCallbacks(plugin);
    }
    registrar
            .platformViewRegistry()
            .registerViewFactory(
                VIEW_TYPE,
                    new FlutterUnityViewFactory(
                        plugin.state,
                        registrar.messenger(),
                        null,
                        null,
                        registrar,
                        registrar.activity(),
                        -1));
  }

  // FlutterPlugin

  @Override
  public void onAttachedToEngine(FlutterPluginBinding binding) {
    pluginBinding = binding;
  }

  @Override
  public void onDetachedFromEngine(FlutterPluginBinding binding) {
    pluginBinding = null;
  }

  // DefaultLifecycleObserver methods

  @Override
  public void onCreate(@NonNull LifecycleOwner owner) {
    state.set(CREATED);
  }

  @Override
  public void onStart(@NonNull LifecycleOwner owner) {
    state.set(STARTED);
  }

  @Override
  public void onResume(@NonNull LifecycleOwner owner) {
    state.set(RESUMED);
  }

  @Override
  public void onPause(@NonNull LifecycleOwner owner) {
    state.set(PAUSED);
  }

  @Override
  public void onStop(@NonNull LifecycleOwner owner) {
    state.set(STOPPED);
  }

  @Override
  public void onDestroy(@NonNull LifecycleOwner owner) {
    state.set(DESTROYED);
  }

  // Application.ActivityLifecycleCallbacks methods
  @Override
  public void onActivityCreated(Activity activity, Bundle savedInstanceState) {
    if (activity.hashCode() != registrarActivityHashCode) {
      return;
    }
    state.set(CREATED);
  }

  @Override
  public void onActivityStarted(Activity activity) {
    if (activity.hashCode() != registrarActivityHashCode) {
      return;
    }
    state.set(STARTED);
  }

  @Override
  public void onActivityResumed(Activity activity) {
    if (activity.hashCode() != registrarActivityHashCode) {
      return;
    }
    state.set(RESUMED);
  }

  @Override
  public void onActivityPaused(Activity activity) {
    if (activity.hashCode() != registrarActivityHashCode) {
      return;
    }
    state.set(PAUSED);
  }

  @Override
  public void onActivityStopped(Activity activity) {
    if (activity.hashCode() != registrarActivityHashCode) {
      return;
    }
    state.set(STOPPED);
  }

  @Override
  public void onActivitySaveInstanceState(Activity activity, Bundle outState) {

  }

  @Override
  public void onActivityDestroyed(Activity activity) {
    if (activity.hashCode() != registrarActivityHashCode) {
      return;
    }
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.ICE_CREAM_SANDWICH) {
      activity.getApplication().unregisterActivityLifecycleCallbacks(this);
    }
    state.set(DESTROYED);
  }

  // ActivityAware

  @Override
  public void onAttachedToActivity(@NonNull ActivityPluginBinding binding) {
    lifecycle = FlutterLifecycleAdapter.getActivityLifecycle(binding);
    lifecycle.addObserver(this);

    pluginBinding
            .getPlatformViewRegistry()
            .registerViewFactory(
                VIEW_TYPE,
                    new FlutterUnityViewFactory(
                            state,
                            pluginBinding.getBinaryMessenger(),
                            binding.getActivity().getApplication(),
                            lifecycle,
                            null,
                            binding.getActivity(),
                            binding.getActivity().hashCode()));
  }

  @Override
  public void onDetachedFromActivityForConfigChanges() {
    this.onDetachedFromActivity();
  }

  @Override
  public void onReattachedToActivityForConfigChanges(@NonNull ActivityPluginBinding binding) {
    lifecycle = FlutterLifecycleAdapter.getActivityLifecycle(binding);
    lifecycle.addObserver(this);
  }

  @Override
  public void onDetachedFromActivity() {
    lifecycle.removeObserver(this);
  }

  // constructors
  public FlutterUnityWidgetPlugin() {}

  private FlutterUnityWidgetPlugin(Activity activity) {
    this.registrarActivityHashCode = activity.hashCode();
  }
}
