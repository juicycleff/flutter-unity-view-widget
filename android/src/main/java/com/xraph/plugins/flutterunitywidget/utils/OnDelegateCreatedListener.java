package com.xraph.plugins.flutterunitywidget.utils;

public interface OnDelegateCreatedListener<T extends LifecycleDelegate> {
  void onDelegateCreated(T t);
}