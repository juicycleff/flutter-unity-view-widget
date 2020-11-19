package com.xraph.plugins.flutterunitywidget;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Application;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import androidx.annotation.NonNull;
import androidx.lifecycle.DefaultLifecycleObserver;
import androidx.lifecycle.Lifecycle;
import androidx.lifecycle.LifecycleOwner;

import com.xraph.plugins.flutterunitywidget.utils.ThreadUtils;
import com.unity3d.player.IUnityPlayerLifecycleEvents;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.atomic.AtomicInteger;

import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding;
import io.flutter.plugin.common.BinaryMessenger;
import io.flutter.plugin.common.MethodCall;
import io.flutter.plugin.common.MethodChannel;
import io.flutter.plugin.common.PluginRegistry;
import io.flutter.plugin.platform.PlatformView;

import static com.xraph.plugins.flutterunitywidget.FlutterUnityWidgetPlugin.CREATED;
import static com.xraph.plugins.flutterunitywidget.FlutterUnityWidgetPlugin.DESTROYED;
import static com.xraph.plugins.flutterunitywidget.FlutterUnityWidgetPlugin.PAUSED;
import static com.xraph.plugins.flutterunitywidget.FlutterUnityWidgetPlugin.RESUMED;
import static com.xraph.plugins.flutterunitywidget.FlutterUnityWidgetPlugin.STARTED;
import static com.xraph.plugins.flutterunitywidget.FlutterUnityWidgetPlugin.STOPPED;

@SuppressLint("NewApi")
final class FlutterUnityViewController
        implements PlatformView,
                Application.ActivityLifecycleCallbacks,
                DefaultLifecycleObserver,
                ActivityPluginBinding.OnSaveInstanceStateListener,
                FlutterUnityViewOptionsSink,
                MethodChannel.MethodCallHandler,
                UnityEventListener,
                IUnityPlayerLifecycleEvents {

    static final String LOG_TAG = "FlutterUnityView";
    private final PluginRegistry.Registrar registrar;
    private final Context context;
    private UnityView unityView;
    private MethodChannel channel;
    private MethodChannel.Result unityReadyResult;
    private int channelId;
    private ThreadUtils mThreadUtils;
    private final AtomicInteger activityState;
    private final Application mApplication;
    private final Lifecycle lifecycle;
    private final int activityHashCode;
    private final Activity activity;
    private final FlutterUnityViewOptions options;
    private SurrogateActivity sa;
    private boolean disposed = false;
    private final BinaryMessenger binaryMessenger;


    FlutterUnityViewController(
        int id,
        Context context,
        AtomicInteger activityState,
        BinaryMessenger binaryMessenger,
        Application application,
        Lifecycle lifecycle,
        PluginRegistry.Registrar registrar,
        int registrarActivityHashCode,
        FlutterUnityViewOptions options,
        Activity activity
    ) {
        this.sa = new SurrogateActivity();
        this.mThreadUtils = new ThreadUtils();
        this.context = context;
        this.channelId = id;
        this.activityState = activityState;
        this.binaryMessenger = binaryMessenger;
        this.options = options;
        mApplication = application;
        this.lifecycle = lifecycle;
        this.registrar = registrar;
        this.activity = activity;
        this.activityHashCode = registrarActivityHashCode;
        initView(id);
    }

    void initView(int id) {
        unityView = getUnityView();
        channel = new MethodChannel(this.binaryMessenger, "plugins.xraph.com/unity_view_" + id);
        channel.setMethodCallHandler(this);
        UnityUtils.addUnityEventListener(this);
    }

    void init() {
        switch (activityState.get()) {
            case STOPPED:
                if (unityView != null) {
                    unityView.onStop();
                }
                break;
            case PAUSED:
                if (unityView != null) {
                    unityView.onPause();
                }
                break;
            case RESUMED:
                if (unityView != null) {
                    unityView.onResume();
                }
                break;
            case STARTED:
                if (unityView != null) {
                    unityView.onStart();
                }
                break;
            case CREATED:
                if (unityView == null) {
                    // TODO: handle created lifecycle
                }
                break;
            case DESTROYED:
                // Nothing to do, the activity has been completely destroyed.
                UnityUtils.removeUnityEventListener(this);
                break;
            default:
                throw new IllegalArgumentException(
                    "Cannot interpret " + activityState.get() + " as an activity state");
        }
        if (lifecycle != null) {
            lifecycle.addObserver(this);
        } else {
            getApplication().registerActivityLifecycleCallbacks(this);
        }
    }

    @Override
    public void onMethodCall(MethodCall methodCall, final MethodChannel.Result result) {
        switch (methodCall.method) {
            case "unity#waitForUnity":
                if (unityView != null) {
                    result.success(null);
                    return;
                }
                unityReadyResult = result;
                break;
            case "unity#createUnityPlayer":
                this.createPlayer(unityView, true);

                break;
            case "unity#isReady":
                result.success(unityView.isUnityReady());
                break;
            case "unity#isLoaded":
                result.success(unityView.isUnityLoaded());
            case "unity#isPaused":
                result.success(unityView.isUnityPaused());
            case "unity#inBackground":
                result.success(unityView.isUnityInBackground());
                break;
            case "unity#postMessage":
                String gameObject, methodName, message;
                gameObject = methodCall.argument("gameObject");
                methodName = methodCall.argument("methodName");
                message = methodCall.argument("message");

                UnityUtils.postMessage(gameObject, methodName, message);
                result.success(true);
                break;
            case "unity#pausePlayer":
                UnityUtils.pause();
                result.success(true);
                break;
            case "unity#openInNativeProcess":
                openNativeUnity();
                result.success(true);
                break;
            case "unity#resumePlayer":
                UnityUtils.resume();
                result.success(true);
                break;
            case "unity#unloadPlayer":
                if (unityView != null && unityView.getUnityPlayer() != null) {
                    unityView.unload();
                }
                UnityUtils.unload();
                result.success(true);
                break;
            case "unity#dispose":
                // TODO: Handle disposing player resource efficiently
                // UnityUtils.unload();
                result.success(true);
                break;
            case "unity#silentQuitPlayer":
                UnityUtils.quitPlayer();
                result.success(true);
                break;
            case "unity#quitPlayer":
                if (UnityUtils.getPlayer() != null)
                    UnityUtils.getPlayer().destroy();
                result.success(true);
                break;
            default:
                result.notImplemented();
        }

    }

    private void createPlayer(final UnityView view, boolean reInitialize) {
        UnityUtils.createPlayer(this.activity, mThreadUtils,this, reInitialize, new OnCreateUnityViewCallback() {
            @Override
            public void onReady() {
                view.setUnityPlayer(UnityUtils.getPlayer());
                if (unityReadyResult != null) {
                    unityReadyResult.success(null);
                    unityReadyResult = null;
                }
            }
        });
    }

    private void openNativeUnity() {
        Intent intent = new Intent(activity, OverrideUnityActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
        intent.putExtra("ar", options.isArEnable());
        intent.putExtra("fullscreen", options.isFullscreenEnabled());
        intent.putExtra("safemode", options.isSafeModeEnabled());
        intent.putExtra("flutterActivity", activity.getClass());
        activity.startActivityForResult(intent, 1);
    }

    @Override
    public View getView() {
        return unityView;
    }

    private int getActivityHashCode() {
        if (registrar != null && registrar.activity() != null) {
            return registrar.activity().hashCode();
        } else {
            return activityHashCode;
        }
    }

    private Application getApplication() {
        if (registrar != null && registrar.activity() != null) {
            return registrar.activity().getApplication();
        } else {
            return mApplication;
        }
    }

    @Override
    public void dispose() {
        if (disposed) {
            return;
        }
        disposed = true;

        // TODO: remove listeners
        getApplication().unregisterActivityLifecycleCallbacks(this);
    }

    @Override
    public void onFlutterViewAttached(View flutterView) {
        if (!unityView.isUnityLoaded()) {
            // TODO: initView(channelId);
        }
    }

    @Override
    public void onFlutterViewDetached() {
        // TODO: Handle cycle
    }

    private UnityView getUnityView() {
        final UnityView view = new UnityView(this.context, this.options);

        if (UnityUtils.getPlayer() != null && UnityUtils.isUnityLoaded()) {
            view.setUnityPlayer(UnityUtils.getPlayer());
        } else {
            this.createPlayer(view, false);
        }
        return view;
    }

    @Override
    public void onMessage(final String message) {
        activity.runOnUiThread(new Runnable() {
            public void run() {
                getChannel().invokeMethod("events#onUnityMessage", message);
            }
        });
    }

    @Override
    public void onSceneLoaded(final String name, final int buildIndex, final boolean isLoaded, final boolean isValid) {
        activity.runOnUiThread(new Runnable() {
            public void run() {
                Map<String, Object> payload =  new HashMap<String, Object>();
                payload.put("name", name);
                payload.put("buildIndex", buildIndex);
                payload.put("isLoaded", isLoaded);
                payload.put("isValid", isValid);
                getChannel().invokeMethod("events#onUnitySceneLoaded", payload);
            }
        });
    }

    private MethodChannel getChannel() {
        return channel;
    }

    // Unity state methods

    @Override
    public void onUnityPlayerUnloaded() {
        activity.runOnUiThread(new Runnable() {
            public void run() {
                getChannel().invokeMethod("events#onUnityUnloaded", true);
            }
        });
    }

    @Override
    public void onUnityPlayerQuitted() {
    }

    // Lifecycle methods

    @Override
    public void onActivityCreated(Activity activity, Bundle savedInstanceState) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    @Override
    public void onActivityStarted(Activity activity) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    @Override
    public void onActivityResumed(Activity activity) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    @Override
    public void onActivityPaused(Activity activity) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    @Override
    public void onActivityStopped(Activity activity) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    @Override
    public void onActivitySaveInstanceState(Activity activity, Bundle outState) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    @Override
    public void onActivityDestroyed(Activity activity) {
        if (disposed || activity.hashCode() != getActivityHashCode()) {
            return;
        }
    }

    // DefaultLifecycleObserver and OnSaveInstanceStateListener

    @Override
    public void onCreate(@NonNull LifecycleOwner owner) {
        if (disposed) {
            return;
        }

        if (unityView != null) {
            // TODO: handle onCreate
        }
    }

    @Override
    public void onStart(@NonNull LifecycleOwner owner) {
        if (disposed) {
            return;
        }
        if (unityView != null) {
            unityView.onStart();
        }
    }

    @Override
    public void onResume(@NonNull LifecycleOwner owner) {
        if (disposed) {
            return;
        }
        if (unityView != null) {
            unityView.onResume();
            UnityUtils.resume();
        }

        if (UnityUtils.isUnityInBackground()) {
            // unityView.restoreUnityViewFromBackground();
        }
    }

    @Override
    public void onPause(@NonNull LifecycleOwner owner) {
        if (disposed) {
            return;
        }
        if (unityView != null) {
            unityView.onPause();
            UnityUtils.pause();
        }
    }

    @Override
    public void onStop(@NonNull LifecycleOwner owner) {
        if (disposed) {
            return;
        }
        if (unityView != null) {
            unityView.onStop();
        }
    }

    @Override
    public void onDestroy(@NonNull LifecycleOwner owner) {
        if (disposed) {
            return;
        }
        if (unityView != null) {
            unityView.onDestroy();
        }
    }

    @Override
    public void onRestoreInstanceState(Bundle bundle) {
        if (disposed) {
            return;
        }
        if (unityView != null) {
            // unityView.onCreate(bundle);
        }
    }

    @Override
    public void onSaveInstanceState(Bundle bundle) {
        if (disposed) {
            return;
        }

    }

    // Options methods

    @Override
    public void setAREnabled(boolean arEnabled) {
        UnityUtils.getOptions().setArEnable(arEnabled);
        if (unityView != null) {
            unityView.getOptions().setArEnable(arEnabled);
        }
    }

    @Override
    public void setFullscreenEnabled(boolean fullscreenEnabled) {
        UnityUtils.getOptions().setFullscreenEnabled(fullscreenEnabled);
        if (unityView != null) {
            unityView.getOptions().setArEnable(fullscreenEnabled);
        }
    }

    @Override
    public void setSafeModeEnabled(boolean safeModeEnabled) {
        UnityUtils.getOptions().setSafeModeEnabled(safeModeEnabled);
        if (unityView != null) {
            unityView.getOptions().setSafeModeEnabled(safeModeEnabled);
        }
    }
}
