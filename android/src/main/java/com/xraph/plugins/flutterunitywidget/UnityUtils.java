package com.xraph.plugins.flutterunitywidget;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.graphics.PixelFormat;
import android.os.Build;
import android.os.StrictMode;
import android.view.ViewGroup;
import android.view.WindowManager;

import com.xraph.plugins.flutterunitywidget.utils.DeferredLifecycleHelper;
import com.xraph.plugins.flutterunitywidget.utils.ThreadUtils;
import com.unity3d.player.IUnityPlayerLifecycleEvents;
import com.unity3d.player.UnityPlayer;

import java.util.concurrent.CopyOnWriteArraySet;

import io.flutter.Log;

import static android.view.ViewGroup.LayoutParams.MATCH_PARENT;

public class UnityUtils {
    static final String LOG_TAG = "UnityUtils";

    private static FlutterUnityViewOptions options;

    private static UnityPlayer unityPlayer;
    private static boolean _isUnityReady;
    private static boolean _isUnityPaused;
    private static boolean _isUnityLoaded;
    private static boolean _isUnityInBackground = false;

    private static final CopyOnWriteArraySet<UnityEventListener> mUnityEventListeners =
            new CopyOnWriteArraySet<>();

    public static FlutterUnityViewOptions getOptions() {
        if (options == null) {
            options = new FlutterUnityViewOptions();
        }
        return options;
    }

    public static UnityPlayer getPlayer() {
        if (!_isUnityReady) {
            return null;
        }
        return unityPlayer;
    }

    public static boolean isUnityReady() {
        return _isUnityReady;
    }

    public static boolean isUnityPaused() {
        return _isUnityPaused;
    }

    public static boolean isUnityLoaded() {
        return _isUnityLoaded;
    }

    public static boolean isUnityInBackground() {
        return _isUnityInBackground;
    }

    @SuppressLint("NewApi")
    public static void createPlayer(final Activity activity, ThreadUtils threadUtils, final IUnityPlayerLifecycleEvents ule, boolean reInitialize, final OnCreateUnityViewCallback callback) {
        if (unityPlayer != null && !reInitialize) {
            callback.onReady();
            return;
        }

        StrictMode.ThreadPolicy threadPolicy = StrictMode.getThreadPolicy();
        StrictMode.setThreadPolicy((new StrictMode.ThreadPolicy.Builder(threadPolicy)).permitAll().build());

        try {
            threadUtils.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    activity.getWindow().setFormat(PixelFormat.RGBA_8888);
                    unityPlayer = new UnityPlayer(options.isArEnable() ? activity : activity.getApplicationContext(), ule);

                    try {
                        // wait a moument. fix unity cannot start when startup.
                        Thread.sleep( 1000 );
                    } catch (Exception e) {
                    }

                    // start unity
                    addUnityViewToBackground(activity);
                    unityPlayer.windowFocusChanged(true);
                    unityPlayer.requestFocus();
                    unityPlayer.resume();

                    // restore window layout
                    if (!options.isFullscreenEnabled()) {
                        activity.getWindow().addFlags(WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN);
                        activity.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
                        activity.getWindow().addFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
                    }

                    _isUnityReady = true;
                    _isUnityLoaded = true;

                    if (callback != null) {
                        callback.onReady();
                    }
                }
            });
        } finally {
            StrictMode.setThreadPolicy(threadPolicy);
        }
    }

    public static void postMessage(String gameObject, String methodName, String message) {
        if (!_isUnityReady) {
            return;
        }
        UnityPlayer.UnitySendMessage(gameObject, methodName, message);
    }

    public static void pause() {
        if (unityPlayer != null && _isUnityLoaded) {
            unityPlayer.pause();
            _isUnityPaused = true;
        }
    }

    public static void resume() {
        if (unityPlayer != null) {
            unityPlayer.resume();
            _isUnityPaused = false;
        }
    }

    public static void unload() {
        if (unityPlayer != null) {
            unityPlayer.unload();
            _isUnityLoaded = false;
        }
    }

    public static void moveToBackground() {
        if (unityPlayer != null) {
            _isUnityInBackground = true;
        }
    }

    public static void quitPlayer() {
        try {
            if (unityPlayer != null) {
                unityPlayer.quit();
                _isUnityLoaded = false;
                _isUnityReady = false;
                unityPlayer = null;
            }
        } catch (Error e) {
            Log.e(LOG_TAG, e.getMessage());
        }
    }

    /**
     * Invoke by unity C#
     */
    public static void onUnitySceneLoaded(String name, int buildIndex, boolean isLoaded, boolean isValid) {
        for (UnityEventListener listener : mUnityEventListeners) {
            try {
                listener.onSceneLoaded(name, buildIndex, isLoaded, isValid);
            } catch (Exception e) {
                Log.e(LOG_TAG, e.getMessage());
            }
        }
    }

    /**
     * Invoke by unity C#
     */
    public static void onUnityMessage(String message) {
        for (UnityEventListener listener : mUnityEventListeners) {
            try {
                listener.onMessage(message);
            } catch (Exception e) {
                Log.e(LOG_TAG, e.getMessage());
            }
        }
    }

    public static void addUnityEventListener(UnityEventListener listener) {
        mUnityEventListeners.add(listener);
    }

    public static void removeUnityEventListener(UnityEventListener listener) {
        mUnityEventListeners.remove(listener);
    }

    public static void addUnityViewToBackground(final Activity activity) {
        if (unityPlayer == null) {
            return;
        }
        if (unityPlayer.getParent() != null) {
            ((ViewGroup)unityPlayer.getParent()).removeView(unityPlayer);
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            unityPlayer.setZ(-1f);
        }
        ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(1, 1);
        activity.addContentView(unityPlayer, layoutParams);
        _isUnityInBackground = true;
    }

    public static void restoreUnityViewFromBackground(final Activity activity) {
        if (unityPlayer == null) {
            return;
        }
        if (unityPlayer.getParent() != null) {
            ((ViewGroup)unityPlayer.getParent()).addView(unityPlayer);
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            unityPlayer.setZ(1f);
        }
        ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(1, 1);
        activity.addContentView(unityPlayer, layoutParams);
        _isUnityInBackground = false;
    }

    public static void addUnityViewToGroup(ViewGroup group) {
        if (unityPlayer == null) {
            return;
        }
        if (unityPlayer.getParent() != null) {
            ((ViewGroup)unityPlayer.getParent()).removeView(unityPlayer);
        }
        ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(MATCH_PARENT, MATCH_PARENT);
        group.addView(unityPlayer, 0, layoutParams);
        unityPlayer.windowFocusChanged(true);
        unityPlayer.requestFocus();
        unityPlayer.resume();
    }
}
