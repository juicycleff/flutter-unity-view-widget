package com.xraph.plugins.flutterunitywidget.utils;

import android.os.Handler;

public class ThreadUtils {
    static final String LOG_TAG = "ThreadUtils";
    private Thread mUiThread;
    private Handler mHandler;

    public ThreadUtils() {
        mHandler = new Handler();
        mUiThread = Thread.currentThread();
    }

    public final void runOnUiThread(Runnable action) {
        if (Thread.currentThread() != mUiThread) {
            mHandler.post(action);
        } else {
            action.run();
        }
    }
}
