package com.xraph.plugins.flutterunitywidget.utils;

import android.os.IBinder;
import java.lang.reflect.Field;

public final class ObjectWrapper<T> extends IObjectWrapper.Stub {
  private final T mWrappedObject;

  private ObjectWrapper(T object) {
    this.mWrappedObject = object;
  }

  public static <T> IObjectWrapper wrap(T object) {
    return new ObjectWrapper(object);
  }

  public static <T> T unwrap(IObjectWrapper remote) {
    if (remote instanceof ObjectWrapper) {
      return (T) ((ObjectWrapper) remote).mWrappedObject;
    }
    IBinder remoteBinder = remote.asBinder();
    Field[] allFields = remoteBinder.getClass().getDeclaredFields();
    if (allFields.length == 1) {
      Field f = allFields[0];
      if (f.isAccessible()) {
        throw new IllegalArgumentException("The concrete class implementing IObjectWrapper must have exactly one declared *private* field for the wrapped object. Preferably, this is an instance of the ObjectWrapper<T> class.");
      }
      f.setAccessible(true);
      try {
        return (T) f.get(remoteBinder);
      } catch (NullPointerException e) {
        throw new IllegalArgumentException("Binder object is null.", e);
      } catch (IllegalArgumentException e2) {
        throw new IllegalArgumentException("remoteBinder is the wrong class.", e2);
      } catch (IllegalAccessException e3) {
        throw new IllegalArgumentException("Could not access the field in remoteBinder.", e3);
      }
    }
    throw new IllegalArgumentException("The concrete class implementing IObjectWrapper must have exactly *one* declared private field for the wrapped object.  Preferably, this is an instance of the ObjectWrapper<T> class.");
  }
}
