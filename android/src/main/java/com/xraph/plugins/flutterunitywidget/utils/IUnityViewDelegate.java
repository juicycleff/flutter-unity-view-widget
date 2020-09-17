package com.xraph.plugins.flutterunitywidget.utils;

import android.os.Bundle;

import com.unity3d.player.UnityPlayer;

public interface IUnityViewDelegate {

  UnityPlayer getUnityPlayer() throws Exception;

  void onCreate(Bundle bundle) throws Exception;

  void onResume() throws Exception;

  void onPause() throws Exception;

  void onDestroy() throws Exception;

  IObjectWrapper getView() throws Exception;

  void onLowMemory() throws Exception;

  void onSaveInstanceState(Bundle bundle) throws Exception;

  void onStart() throws Exception;

  void onStop() throws Exception;
}
