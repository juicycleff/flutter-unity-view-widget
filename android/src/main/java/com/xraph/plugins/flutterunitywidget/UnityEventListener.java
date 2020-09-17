package com.xraph.plugins.flutterunitywidget;

public interface UnityEventListener {
    void onMessage(String message);

    void onSceneLoaded(String name, int buildIndex, boolean isLoaded, boolean isValid);
}
