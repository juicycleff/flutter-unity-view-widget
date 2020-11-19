package com.xraph.plugins.flutterunitywidget;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.WindowManager;

import com.unity3d.player.UnityPlayerActivity;

import java.util.Objects;

public class OverrideUnityActivity extends UnityPlayerActivity {
    public static OverrideUnityActivity instance = null;
    static final String LOG_TAG = "ExtendedUnityActivity";
    Class mMainActivityClass;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        instance = this;
        this.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
        Intent intent = getIntent();
        handleIntent(intent);
    }

    protected void unloadPlayer() {
        mUnityPlayer.unload();
        showMainActivity();
    }

    protected void quitPlayer() {
        mUnityPlayer.quit();
    }

    protected void showMainActivity() {
        Intent intent = new Intent(this, mMainActivityClass);
        intent.putExtra("showMain",true);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.CUPCAKE) {
            intent.setFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT | Intent.FLAG_ACTIVITY_SINGLE_TOP);
        }
        startActivity(intent);
    }

    @Override public void onUnityPlayerUnloaded() {
        showMainActivity();
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        handleIntent(intent);
        setIntent(intent);
    }

    private void handleIntent(Intent intent) {
        // Set activity not fullscreen
        Class st = (Class) Objects.requireNonNull(intent.getExtras()).get("flutterActivity");
        if(st != null) {
            mMainActivityClass = st;
        }

        // Set activity not fulllscreen
        if(Objects.requireNonNull(intent.getExtras()).getBoolean("fullscreen")) {
            boolean fullscreen = intent.getExtras().getBoolean("fullscreen");
            if(!fullscreen) {
                this.getWindow().addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN);
                this.getWindow().addFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
            } else {
                this.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
            }
        }

        // Unloads unity
        if(Objects.requireNonNull(intent.getExtras()).containsKey("unload")) {
            if(mUnityPlayer != null) {
                mUnityPlayer.unload();
            }
        }
    }

    @Override
    public void onBackPressed() {
        Log.i(LOG_TAG, "onBackPressed called");
        this.showMainActivity();
        super.onBackPressed();
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
    }

    @Override
    public void onPause() {
        super.onPause();
        this.mUnityPlayer.pause();
    }

    @Override
    public void onResume() {
        super.onResume();
        this.mUnityPlayer.resume();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        instance = null;
    }
}
