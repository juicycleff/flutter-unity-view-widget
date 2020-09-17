package com.xraph.plugins.flutterunitywidget;

import android.app.Activity;
import android.app.Application;
import android.content.Context;

import androidx.lifecycle.Lifecycle;

import java.util.concurrent.atomic.AtomicInteger;

import io.flutter.plugin.common.BinaryMessenger;
import io.flutter.plugin.common.PluginRegistry;

public class FlutterUnityViewBuilder implements FlutterUnityViewOptionsSink {
    private final FlutterUnityViewOptions options = new FlutterUnityViewOptions();

    FlutterUnityViewController build(
       int id,
       Context context,
       AtomicInteger state,
       BinaryMessenger binaryMessenger,
       Application application,
       Lifecycle lifecycle,
       PluginRegistry.Registrar registrar,
       int activityHashCode,
       Activity activity
    ) {
        final FlutterUnityViewController controller = new FlutterUnityViewController(
            id,
            context,
            state,
            binaryMessenger,
            application,
            lifecycle,
            registrar,
            activityHashCode,
            this.options,
            activity
        );
        controller.init();

        return controller;
    }

    @Override
    public void setAREnabled(boolean arEnabled) {
        UnityUtils.getOptions().setArEnable(arEnabled);
        options.setArEnable(arEnabled);
    }

    @Override
    public void setFullscreenEnabled(boolean fullscreenEnabled) {
        UnityUtils.getOptions().setFullscreenEnabled(fullscreenEnabled);
        options.setFullscreenEnabled(fullscreenEnabled);
    }

    @Override
    public void setSafeModeEnabled(boolean safeModeEnabled) {
        UnityUtils.getOptions().setSafeModeEnabled(safeModeEnabled);
        options.setSafeModeEnabled(safeModeEnabled);
    }
}
