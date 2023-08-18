package com.xraph.plugin.flutter_unity_widget

import io.flutter.embedding.android.FlutterActivity

/* 
The following Unity versions expect the mUnityPlayer property on the main activity: 
- 2020.3.46 or higher
- 2021.3.19 or higher
- 2022.2.4 or higher

Unity will function without it, but many Unity plugins (like ARFoundation) will not.
Implement FlutterUnityActivity or the interface to fix these plugins.
*/

open class FlutterUnityActivity: FlutterActivity() {
    @JvmField
    var mUnityPlayer: java.lang.Object? = null;
}


/*
 A function that is called when initializing Unity.
 Expected use is to set a mUnityPlayer property, just as defined in FlutterUnityActivity above.
*/
interface IFlutterUnityActivity {
    fun setUnityPlayer(unityPlayer: java.lang.Object?)
}