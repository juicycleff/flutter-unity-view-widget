package com.xraph.plugins.flutterunitywidget;

import android.annotation.SuppressLint;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.os.Build;
import android.os.Bundle;
import android.os.StrictMode;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;

import androidx.annotation.Nullable;

import com.xraph.plugins.flutterunitywidget.utils.DeferredLifecycleHelper;
import com.xraph.plugins.flutterunitywidget.utils.IUnityViewDelegate;
import com.xraph.plugins.flutterunitywidget.utils.ObjectWrapper;
import com.xraph.plugins.flutterunitywidget.utils.OnDelegateCreatedListener;
import com.xraph.plugins.flutterunitywidget.utils.Preconditions;
import com.xraph.plugins.flutterunitywidget.utils.UnityViewLifecycleDelegate;
import com.unity3d.player.IUnityPlayerLifecycleEvents;
import com.unity3d.player.UnityPlayer;

import java.util.ArrayList;
import java.util.List;

import io.flutter.Log;

import static android.view.ViewGroup.LayoutParams.MATCH_PARENT;

public class UnityView extends FrameLayout {
  static final String LOG_TAG = "UnityView";

  private UnityPlayer unityPlayer;
  private DeferredUnityView deferredUnityView;
  private FlutterUnityViewOptions options;

  public FlutterUnityViewOptions getOptions() {
    if (options == null) {
      options = new FlutterUnityViewOptions();
    }
    return options;
  }

  public UnityPlayer getUnityPlayer() {
    return unityPlayer;
  }

  public boolean isUnityReady() {
    return unityReady;
  }

  public void setUnityReady(boolean unityReady) {
    this.unityReady = unityReady;
  }

  public boolean isUnityPaused() {
    return unityPaused;
  }

  public void setUnityPaused(boolean unityPaused) {
    this.unityPaused = unityPaused;
  }

  public boolean isUnityLoaded() {
    return unityLoaded;
  }

  public void setUnityLoaded(boolean unityLoaded) {
    this.unityLoaded = unityLoaded;
  }

  public boolean isUnityInBackground() {
    return unityInBackground;
  }

  public void setUnityInBackground(boolean unityInBackground) {
    this.unityInBackground = unityInBackground;
  }

  private boolean unityReady;
  private boolean unityPaused;
  private boolean unityLoaded;
  private boolean unityInBackground = false;

  protected UnityView(Context context) {
    super(context);
    this.options = new FlutterUnityViewOptions();
    // TODO: this.deferredUnityView = new DeferredUnityView(this, context, this.options);
  }

  protected UnityView(Context context, FlutterUnityViewOptions options) {
    super(context);
    this.options = options;
    // TODO: this.deferredUnityView = new DeferredUnityView(this, context, this.options);
  }

  @SuppressLint("NewApi")
  public final void onCreate(Bundle savedInstanceState) {
    StrictMode.ThreadPolicy threadPolicy = StrictMode.getThreadPolicy();
    StrictMode.setThreadPolicy((new StrictMode.ThreadPolicy.Builder(threadPolicy)).permitAll().build());

    try {
      this.deferredUnityView.onCreate(savedInstanceState);
      if (this.deferredUnityView.getDelegate() == null) {
        DeferredLifecycleHelper.showPlayerMessage(this);
      }
    } finally {
      StrictMode.setThreadPolicy(threadPolicy);
    }
  }

  private void init(final IUnityPlayerLifecycleEvents ule, boolean reInitialize, final OnCreateUnityViewCallback callback) {
    if (unityPlayer != null && !reInitialize) {
      if (callback != null) {
        callback.onReady();
      }
    }
  }

  public void postMessage(String gameObject, String methodName, String message) {
    if (!isUnityReady()) {
      return;
    }
    UnityPlayer.UnitySendMessage(gameObject, methodName, message);
  }

  public void quitPlayer() {
    try {
      if (unityPlayer != null) {
        unityPlayer.quit();
        setUnityLoaded(false);
        setUnityReady(false);
        unityPlayer = null;
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  public void pause() {
    try {
      if (unityPlayer != null && isUnityLoaded()) {
        unityPlayer.pause();
        setUnityPaused(true);
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  public void resume() {
    try {
      if (unityPlayer != null) {
        unityPlayer.resume();
        setUnityPaused(false);
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  public void unload() {
    try {
      if (unityPlayer != null) {
        unityPlayer.unload();
        setUnityLoaded(false);
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  public final void onResume() {
    try {
      if (this.unityPlayer != null) {
        this.unityPlayer.resume();
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  public final void onPause() {
    try {
      if (this.unityPlayer != null) {
        this.unityPlayer.pause();
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  public final void onStart() {
    // todo: handle on start
  }

  public final void onStop() {
    // todo: handle on onStop
  }

  public final void onDestroy() {
    // todo: handle on onDestroy
  }

  public final void onLowMemory() {
    try {
      if (this.unityPlayer != null) {
        this.unityPlayer.lowMemory();
      }
    } catch (Error e) {
      Log.e(LOG_TAG, e.getMessage());
    }
  }

  @Override
  public void onWindowFocusChanged(boolean hasWindowFocus) {
    super.onWindowFocusChanged(hasWindowFocus);
    if (unityPlayer != null) {
      unityPlayer.windowFocusChanged(hasWindowFocus);
    }
  }

  @Override
  protected void onConfigurationChanged(Configuration newConfig) {
    Log.i(LOG_TAG, "ORIENTATION CHANGED");
    Log.i(LOG_TAG, "ORIENTATION CHANGED");
    Log.i(LOG_TAG, "ORIENTATION CHANGED");
    if (unityPlayer != null) {
      unityPlayer.configurationChanged(newConfig);
    }
    super.onConfigurationChanged(newConfig);
  }

  @Override
  public boolean dispatchTouchEvent(MotionEvent ev) {
    if (unityPlayer != null) {
      ev.setSource(InputDevice.SOURCE_TOUCHSCREEN);
      unityPlayer.injectEvent(ev);
    }
    return super.dispatchTouchEvent(ev);
  }

  // Pass any events not handled by (unfocused) views straight to UnityPlayer
  @Override public boolean onKeyUp(int keyCode, KeyEvent event) {
    if (unityPlayer != null) {
      return unityPlayer.injectEvent(event);
    }
    return true;
  }

  @Override public boolean onKeyDown(int keyCode, KeyEvent event) {
    if (unityPlayer != null) {
      return unityPlayer.injectEvent(event);
    }
    return true;
  }

  @Override public boolean onTouchEvent(MotionEvent event) {
    if (unityPlayer != null) {
      return unityPlayer.injectEvent(event);
    }
    return true;
  }

  @Override
  public void dispatchWindowFocusChanged(boolean hasFocus) {
    if (unityPlayer != null) {
      unityPlayer.dispatchWindowFocusChanged(hasFocus);
    }
    super.dispatchWindowFocusChanged(hasFocus);
  }

  @Override
  public void dispatchConfigurationChanged(Configuration newConfig) {
    if (unityPlayer != null) {
      unityPlayer.dispatchConfigurationChanged(newConfig);
    }
    super.dispatchConfigurationChanged(newConfig);
  }

  @Override
  public void setOnLongClickListener(@Nullable OnLongClickListener l) {
    if (unityPlayer != null) {
      unityPlayer.setOnLongClickListener(l);
    }
    super.setOnLongClickListener(l);
  }

  @Override
  public boolean performClick() {
    if (unityPlayer != null) {
      unityPlayer.performClick();
    }
    return super.performClick();
  }

  @Override
  public boolean callOnClick() {
    if (unityPlayer != null) {
      unityPlayer.callOnClick();
    }
    return super.callOnClick();
  }

  @Override
  public boolean performLongClick() {
    if (unityPlayer != null) {
      unityPlayer.performLongClick();
    }
    return super.performLongClick();
  }

  @Override
  public boolean performLongClick(float x, float y) {
    if (unityPlayer != null) {
      unityPlayer.performLongClick(x, y);
    }
    return super.performLongClick(x, y);
  }

  @Override
  public void setOnKeyListener(OnKeyListener l) {
    if (unityPlayer != null) {
      unityPlayer.setOnKeyListener(l);
    }
    super.setOnKeyListener(l);
  }

  @Override
  public void setOnTouchListener(OnTouchListener l) {
    if (unityPlayer != null) {
      unityPlayer.setOnTouchListener(l);
    }
    super.setOnTouchListener(l);
  }

  @Override
  public void setOnGenericMotionListener(OnGenericMotionListener l) {
    if (unityPlayer != null) {
      unityPlayer.setOnGenericMotionListener(l);
    }
    super.setOnGenericMotionListener(l);
  }

  @Override
  public void setOnHoverListener(OnHoverListener l) {
    if (unityPlayer != null) {
      unityPlayer.setOnHoverListener(l);
    }
    super.setOnHoverListener(l);
  }

  @Override
  public void setOnDragListener(OnDragListener l) {
    if (unityPlayer != null) {
      unityPlayer.setOnDragListener(l);
    }
    super.setOnDragListener(l);
  }

  @Override
  public void setScrollX(int value) {
    if (unityPlayer != null) {
      unityPlayer.setScrollX(value);
    }
    super.setScrollX(value);
  }

  @Override
  public void setScrollY(int value) {
    if (unityPlayer != null) {
      unityPlayer.setScrollY(value);
    }
    super.setScrollY(value);
  }

  public boolean onGenericMotionEvent(MotionEvent event)  {
    if (unityPlayer != null) {
      return unityPlayer.injectEvent(event);
    }
    return true;
  }

  @Override
  protected void onDetachedFromWindow() {
    super.onDetachedFromWindow();
  }

  public void setUnityPlayer(UnityPlayer player) {
    this.unityPlayer = player;
    UnityUtils.addUnityViewToGroup(this);
  }

  public void addUnityViewToGroup(ViewGroup group) {
    if (unityPlayer == null) {
      return;
    }
    if (unityPlayer.getParent() != null) {
      ((ViewGroup) unityPlayer.getParent()).removeView(unityPlayer);
    }
    ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(MATCH_PARENT, MATCH_PARENT);
    group.addView(unityPlayer, 0, layoutParams);
    unityPlayer.windowFocusChanged(true);
    unityPlayer.requestFocus();
    unityPlayer.resume();
  }

  static class DeferredUnityView extends DeferredLifecycleHelper<UnityView.UnityViewDelegate> {
    private final ViewGroup viewGroup;
    private final Context context;
    private UnityView unityView;
    private final FlutterUnityViewOptions viewOptions;
    private final List<OnCreateUnityViewCallback> viewCallbacks = new ArrayList();
    private OnDelegateCreatedListener<UnityView.UnityViewDelegate> delegateCreatedListener;

    DeferredUnityView(ViewGroup viewGroup, Context context, FlutterUnityViewOptions viewOptions) {
      this.viewGroup = viewGroup;
      this.context = context;
      this.viewOptions = viewOptions;
    }

    protected final void createDelegate(OnDelegateCreatedListener<UnityView.UnityViewDelegate> delegate) {
      this.delegateCreatedListener = delegate;
      DeferredUnityView deferredUnityView = this;
      if (this.delegateCreatedListener != null && this.getDelegate() == null) {
        try {
          IUnityViewDelegate viewDelegate = null;

          deferredUnityView.delegateCreatedListener.onDelegateCreated(new UnityView.UnityViewDelegate(deferredUnityView.viewGroup, viewDelegate));

          for (OnCreateUnityViewCallback readyCallback : deferredUnityView.viewCallbacks) {
            ((UnityViewDelegate) deferredUnityView.getDelegate()).getUnityPlayerAsync(readyCallback);
          }

          viewCallbacks.clear();
        } catch (Exception ignore) {
        }
      }
    }

    public final void getUnityPlayerAsync(OnCreateUnityViewCallback callback) {
      if (this.getDelegate() != null) {
        ((UnityView.UnityViewDelegate)this.getDelegate()).getUnityPlayerAsync(callback);
      } else {
        this.viewCallbacks.add(callback);
      }
    }
  }

  static class UnityViewDelegate implements UnityViewLifecycleDelegate {
    private final IUnityViewDelegate delegate;
    private final ViewGroup parent;
    private View view;

    UnityViewDelegate(ViewGroup viewGroup, IUnityViewDelegate delegate) {
      this.delegate = (IUnityViewDelegate) Preconditions.checkNotNull(delegate);
      this.parent = (ViewGroup) Preconditions.checkNotNull(viewGroup);
    }

    @Override
    public void onCreate(Bundle bundle) {
      try {
        Bundle var2 = new Bundle();
        this.delegate.onCreate(var2);
        this.view = (View) ObjectWrapper.unwrap(this.delegate.getView());
        this.parent.removeAllViews();
        this.parent.addView(this.view);
      } catch (Exception e) {
        e.printStackTrace();
      }
    }

    @Override
    public View onCreateView(LayoutInflater layoutInflater, ViewGroup viewGroup, Bundle bundle) {
      throw new UnsupportedOperationException("onCreateView not allowed on UnityViewDelegate");
    }

    @Override
    public void onDestroy() throws Exception {
      this.delegate.onDestroy();
    }

    @Override
    public void onDestroyView() throws Exception {
      throw new UnsupportedOperationException("onDestroyView not allowed on UnityViewDelegate");
    }

    @Override
    public void onInflate(Activity activity, Bundle bundle, Bundle bundle2) {
      throw new UnsupportedOperationException("onInflate not allowed on UnityViewDelegate");
    }

    @Override
    public void onLowMemory() throws Exception {
      this.delegate.onLowMemory();
    }

    @Override
    public void onPause() throws Exception {
      this.delegate.onPause();
    }

    @Override
    public void onResume() throws Exception {
      this.delegate.onResume();
    }

    @Override
    public void onSaveInstanceState(Bundle bundle) throws Exception {
      this.delegate.onSaveInstanceState(bundle);
    }

    @Override
    public void onStart() throws Exception {
      this.delegate.onStart();
    }

    @Override
    public void onStop() throws Exception {
      this.delegate.onStop();
    }

    @Override
    public void getUnityPlayerAsync(OnCreateUnityViewCallback callback) {
      // this.delegate.getUnityPlayer(new zzac(this, var1));
    }
  }
}