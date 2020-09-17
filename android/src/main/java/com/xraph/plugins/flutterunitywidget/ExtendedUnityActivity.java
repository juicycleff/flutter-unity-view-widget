package com.xraph.plugins.flutterunitywidget;

import android.content.Intent;
import android.graphics.Color;
import android.graphics.Point;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.Display;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;

import com.unity3d.player.UnityPlayerActivity;

import java.util.Objects;

public class ExtendedUnityActivity extends UnityPlayerActivity {
    public static ExtendedUnityActivity instance = null;
    static final String LOG_TAG = "ExtendedUnityActivity";
    Class mMainActivityClass;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        instance = this;
        this.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
        addControlsToUnityFrame();
        Intent intent = getIntent();
        handleIntent(intent);
    }

    public void addControlsToUnityFrame() {
        Display display = getWindowManager().getDefaultDisplay();
        Point point = new Point();
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            display.getSize(point);
        }
        int size = (point.x / 2);

        Button backButton = new Button(this);
        backButton.setBackgroundColor(Color.BLUE);
        backButton.setText("GO BACK");
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
            backButton.setX(10f);
            backButton.setY(25f);
        }
        backButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                showMainActivity();
            }
        });


        Button unloadButton = new Button(this);
        unloadButton.setBackgroundColor(Color.BLUE);
        unloadButton.setText("UNLOAD");
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB) {
            backButton.setX(25f);
            backButton.setY(300f);
        }
        unloadButton.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                mUnityPlayer.unload();
            }
        });

        mUnityPlayer.addView(backButton, size, size / 4);
        mUnityPlayer.addView(unloadButton, size, size / 4);
    }

    private void showMainActivity() {
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
        // moveTaskToBack(true);
        Log.i(LOG_TAG, "onBackPressed called");
        // this.mUnityPlayer.quit();
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
