package com.xraph.plugins.flutterunitywidget.utils;

import com.xraph.plugins.flutterunitywidget.OnCreateUnityViewCallback;

public interface UnityViewLifecycleDelegate extends LifecycleDelegate {
  void getUnityPlayerAsync(OnCreateUnityViewCallback callback);
}
