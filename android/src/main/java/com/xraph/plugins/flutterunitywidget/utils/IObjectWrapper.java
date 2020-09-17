package com.xraph.plugins.flutterunitywidget.utils;

import android.os.Binder;
import android.os.IBinder;
import android.os.IInterface;
import android.os.Parcel;
import android.os.RemoteException;

public interface IObjectWrapper extends IInterface {

  public static abstract class Stub extends Binder implements IObjectWrapper {
    private static final String DESCRIPTOR = "com.google.android.gms.dynamic.IObjectWrapper";

    private static class Proxy implements IObjectWrapper {
      private IBinder mRemote;

      Proxy(IBinder remote) {
        this.mRemote = remote;
      }

      public IBinder asBinder() {
        return this.mRemote;
      }

      public String getInterfaceDescriptor() {
        return Stub.DESCRIPTOR;
      }
    }

    public Stub() {
      attachInterface(this, DESCRIPTOR);
    }

    public static IObjectWrapper asInterface(IBinder obj) {
      if (obj == null) {
        return null;
      }
      IInterface iin = obj.queryLocalInterface(DESCRIPTOR);
      if (iin == null || !(iin instanceof IObjectWrapper)) {
        return new Proxy(obj);
      }
      return (IObjectWrapper) iin;
    }

    public IBinder asBinder() {
      return this;
    }

    public boolean onTransact(int code, Parcel data, Parcel reply, int flags) throws RemoteException {
      switch (code) {
        case 1598968902:
          reply.writeString(DESCRIPTOR);
          return true;
        default:
          return super.onTransact(code, data, reply, flags);
      }
    }
  }
}