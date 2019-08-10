package com.rexraphael.flutterunitywidget;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.os.Debug;
import android.os.Handler;
import android.view.View;

import io.flutter.plugin.common.MethodCall;
import io.flutter.plugin.common.MethodChannel;
import io.flutter.plugin.common.PluginRegistry;
import io.flutter.plugin.platform.PlatformView;

@SuppressLint("NewApi")
public class FlutterUnityView implements PlatformView, MethodChannel.MethodCallHandler, UnityEventListener {
    private final Context context;
    UnityView unityView;
    MethodChannel channel;
    public final PluginRegistry.Registrar registrar;
    static final String LOG_TAG = "FlutterUnityView";
    public final Activity activity;


    FlutterUnityView(Context context, PluginRegistry.Registrar registrar, int id) {
        this.context = context;
        this.registrar = registrar;
        this.activity = registrar.activity();

        unityView = getUnityView(registrar);

        channel = new MethodChannel(registrar.messenger(), "unity_view_" + id);

        channel.setMethodCallHandler(this);
        UnityUtils.addUnityEventListener(this);
    }

    @Override
    public void onMethodCall(MethodCall methodCall, final MethodChannel.Result result) {
        switch (methodCall.method) {
            case "createUnity":
                String isAR;
                isAR = methodCall.argument("isAR");

                if (isAR != null) {
                    UnityUtils.isAR = true;
                }

                UnityUtils.createPlayer(registrar.activity(), new UnityUtils.CreateCallback() {
                    @Override
                    public void onReady() {
                        result.success(true);
                    }
                });


                break;
            case "isReady":
                result.success(UnityUtils.isUnityReady());
                break;
            case "postMessage":
                String gameObject, methodName, message;
                gameObject = methodCall.argument("gameObject");
                methodName = methodCall.argument("methodName");
                message = methodCall.argument("message");

                UnityUtils.postMessage(gameObject, methodName, message);
                result.success(true);
                break;
            case "pause":
                UnityUtils.pause();
                result.success(true);
                break;
            case "resume":
                UnityUtils.resume();
                result.success(true);
                break;
            default:
                result.notImplemented();
        }

    }

    @Override
    public View getView() {
        return unityView;
    }

    @Override
    public void dispose() {
        if (UnityUtils.isUnityReady()) {
            UnityUtils.getPlayer().quit();
        }
    }

    private UnityView getUnityView(PluginRegistry.Registrar registrar) {
        final UnityView view = new UnityView(registrar.context());
        // view.addOnAttachStateChangeListener(this);

        if (UnityUtils.getPlayer() != null) {
            view.setUnityPlayer(UnityUtils.getPlayer());
        } else {
            UnityUtils.createPlayer(this.activity, new UnityUtils.CreateCallback() {
                @Override
                public void onReady() {
                    view.setUnityPlayer(UnityUtils.getPlayer());
                }
            });
        }
        return view;
    }

    private void restoreUnityUserState() {
        // restore the unity player state
        if (UnityUtils.isUnityPaused()) {
            Handler handler = new Handler();
            handler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    if (UnityUtils.getPlayer() != null) {
                        UnityUtils.getPlayer().pause();
                    }
                }
            }, 300); //TODO: 300 is the right one?
        }
    }

    @Override
    public void onMessage(final String message) {
        activity.runOnUiThread(new Runnable() {
            public void run() {
                getChannel().invokeMethod("onUnityMessage", message);
            }
        });
    }

    private MethodChannel getChannel() {
        return channel;
    }


}
