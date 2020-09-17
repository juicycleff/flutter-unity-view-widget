package com.xraph.plugins.flutterunitywidget;

import android.app.Activity;

import com.unity3d.player.IUnityPlayerLifecycleEvents;

public class SurrogateActivity extends Activity implements IUnityPlayerLifecycleEvents {
    @Override
    public void onUnityPlayerUnloaded() {

    }

    @Override
    public void onUnityPlayerQuitted() {

    }
}
