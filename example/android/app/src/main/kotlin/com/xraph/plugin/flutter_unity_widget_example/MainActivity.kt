package com.xraph.plugin.flutter_unity_widget_example

import com.xraph.plugin.flutter_unity_widget.FlutterUnityActivity;

class MainActivity: FlutterUnityActivity() {

}

 
// If you can't inherit FlutterUnityActivity directly, use the interface like this:
/*
import com.xraph.plugin.flutter_unity_widget.IFlutterUnityActivity;

class ActivityExample: SomeActivity(), IFlutterUnityActivity {
    @JvmField 
    var mUnityPlayer: java.lang.Object? = null;

    override fun setUnityPlayer(unityPlayer: java.lang.Object?) {
        mUnityPlayer = unityPlayer;
    }
}
*/