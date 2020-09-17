package com.xraph.plugins.flutterunitywidget.utils;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

public class Preconditions {
  @NonNull
  public static <T> T checkNotNull(@Nullable T var) {
    if (var == null) {
      throw new NullPointerException("null reference");
    } else {
      return var;
    }
  }
}
