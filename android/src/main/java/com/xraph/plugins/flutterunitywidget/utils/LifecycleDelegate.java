package com.xraph.plugins.flutterunitywidget.utils;

import android.app.Activity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

public interface LifecycleDelegate {
  void onCreate(Bundle bundle);

  View onCreateView(LayoutInflater layoutInflater, ViewGroup viewGroup, Bundle bundle);

  void onDestroy() throws Exception;

  void onDestroyView() throws Exception;

  void onInflate(Activity activity, Bundle bundle, Bundle bundle2);

  void onLowMemory() throws Exception;

  void onPause() throws Exception;

  void onResume() throws Exception;

  void onSaveInstanceState(Bundle bundle) throws Exception;

  void onStart() throws Exception;

  void onStop() throws Exception;
}