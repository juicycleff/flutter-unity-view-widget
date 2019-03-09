package com.rexraphael.flutterunitywidget;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.view.View;

import io.flutter.plugin.common.MethodCall;
import io.flutter.plugin.common.MethodChannel;
import io.flutter.plugin.common.PluginRegistry;
import io.flutter.plugin.platform.PlatformView;

@SuppressLint("NewApi")
public class FlutterUnityView implements PlatformView, MethodChannel.MethodCallHandler,
        View.OnAttachStateChangeListener, UnityEventListener {
    private final Context context;
    UnityView unityView;
    MethodChannel channel;
    PluginRegistry.Registrar registrar;

    FlutterUnityView(Context context, PluginRegistry.Registrar registrar, int id) {
        this.context = context;
        this.registrar = registrar;
        unityView = getUnityView(registrar);

        channel = new MethodChannel(registrar.messenger(), "nativeweb_" + id);

        channel.setMethodCallHandler(this);
    }

    @Override
    public void onMethodCall(MethodCall methodCall, MethodChannel.Result result) {
        switch (methodCall.method) {
            case "createUnity":
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
                break;
            case "pause":
                UnityUtils.pause();
                break;
            case "resume":
                UnityUtils.resume();
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
        view.addOnAttachStateChangeListener(this);

        if (UnityUtils.getPlayer() != null) {
            view.setUnityPlayer(UnityUtils.getPlayer());
        } else {
            UnityUtils.createPlayer((Activity) context, new UnityUtils.CreateCallback() {
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
    public void onViewAttachedToWindow(View view) {
        restoreUnityUserState();
    }

    @Override
    public void onViewDetachedFromWindow(View view) {

    }

    @Override
    public void onMessage(String message) {

    }
}
