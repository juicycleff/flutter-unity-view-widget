package com.rexraphael.flutterunitywidget;

import android.content.Context;
import android.content.res.Configuration;
import android.view.InputDevice;
import android.view.MotionEvent;
import android.widget.FrameLayout;

import com.unity3d.player.UnityPlayer;

public class UnityView extends FrameLayout {

    private UnityPlayer view;

    protected UnityView(Context context) {
        super(context);
    }

    public void setUnityPlayer(UnityPlayer player) {
        this.view = player;
        UnityUtils.addUnityViewToGroup(this);
    }

    @Override
    public void onWindowFocusChanged(boolean hasWindowFocus) {
        super.onWindowFocusChanged(hasWindowFocus);
        if (view != null) {
            view.windowFocusChanged(hasWindowFocus);
        }
    }

    @Override
    protected void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        if (view != null) {
            view.configurationChanged(newConfig);
        }
    }

    @Override
    public boolean dispatchTouchEvent(MotionEvent ev) {
        ev.setSource(InputDevice.SOURCE_TOUCHSCREEN);
        view.injectEvent(ev);
        return super.dispatchTouchEvent(ev);
    }

    @Override
    protected void onDetachedFromWindow() {
        // todo: fix more than one unity view, don't add to background.
        // UnityUtils.addUnityViewToBackground();
        super.onDetachedFromWindow();
    }
}