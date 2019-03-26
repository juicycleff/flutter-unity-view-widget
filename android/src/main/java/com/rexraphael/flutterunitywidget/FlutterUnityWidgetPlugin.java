package com.rexraphael.flutterunitywidget;

import io.flutter.plugin.common.PluginRegistry.Registrar;

/** FlutterUnityWidgetPlugin */
public class FlutterUnityWidgetPlugin {
  public static void registerWith(Registrar registrar) {
    registrar
            .platformViewRegistry()
            .registerViewFactory(
                    "unity_view", new FlutterUnityViewFactory(registrar));
  }
}
