package com.xraph.plugin.flutter_unity_widget

import android.annotation.SuppressLint
import android.app.Activity
import android.app.Application
import android.os.Bundle
import android.util.Log
import androidx.annotation.NonNull
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.LifecycleRegistry

import io.flutter.embedding.engine.plugins.FlutterPlugin
import io.flutter.embedding.engine.plugins.activity.ActivityAware
import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding
import io.flutter.embedding.engine.plugins.lifecycle.FlutterLifecycleAdapter

/** FlutterUnityWidgetPlugin */
class FlutterUnityWidgetPlugin : FlutterPlugin, ActivityAware {
    private var lifecycle: Lifecycle? = null

    override fun onAttachedToEngine(@NonNull binding: FlutterPlugin.FlutterPluginBinding) {
        UnityPlayerUtils.applicationContext = binding.applicationContext
        binding
                .platformViewRegistry
                .registerViewFactory(
                        VIEW_TYPE,
                        FlutterUnityWidgetFactory(
                                binding.binaryMessenger,
                                binding.applicationContext,
                                object : LifecycleProvider {
                                    override fun getLifecycle(): Lifecycle {
                                        return lifecycle!!
                                    }
                                }))
    }

    override fun onDetachedFromEngine(@NonNull binding: FlutterPlugin.FlutterPluginBinding) {}

    companion object {
        private const val VIEW_TYPE = "plugin.xraph.com/unity_view"

        fun registerWith(
                registrar: io.flutter.plugin.common.PluginRegistry.Registrar) {
            val activity = registrar.activity()
                    ?: // When a background flutter view tries to register the plugin, the registrar has no activity.
                    // We stop the registration process as this plugin is foreground only.
                    return

            UnityPlayerUtils.activity = activity

            if (activity is LifecycleOwner) {
                registrar
                        .platformViewRegistry()
                        .registerViewFactory(
                                VIEW_TYPE,
                                FlutterUnityWidgetFactory(
                                        registrar.messenger(),
                                        registrar.context(),
                                        object : LifecycleProvider {
                                            override fun getLifecycle(): Lifecycle {
                                                return (activity as LifecycleOwner).lifecycle
                                            }
                                        }))
            } else {
                registrar
                        .platformViewRegistry()
                        .registerViewFactory(
                                VIEW_TYPE,
                                FlutterUnityWidgetFactory(
                                        registrar.messenger(),
                                        registrar.context(),
                                        ProxyLifecycleProvider(activity)))
            }
        }
    }

    @SuppressLint("LongLogTag")
    override fun onAttachedToActivity(binding: ActivityPluginBinding) {
        UnityPlayerUtils.activity = binding.activity
        lifecycle = FlutterLifecycleAdapter.getActivityLifecycle(binding)
    }

    override fun onDetachedFromActivityForConfigChanges() {
        onDetachedFromActivity()
    }

    override fun onReattachedToActivityForConfigChanges(binding: ActivityPluginBinding) {
        onAttachedToActivity(binding)
    }

    override fun onDetachedFromActivity() {
        // UnityPlayerUtils.activity = null
        lifecycle = null
    }

    /**
     * This class provides a {@link LifecycleOwner} for the activity driven by {@link
     * ActivityLifecycleCallbacks}.
     *
     * <p>This is used in the case where a direct Lifecycle/Owner is not available.
     */
    @SuppressLint("NewApi")
    private class ProxyLifecycleProvider(activity: Activity) : Application.ActivityLifecycleCallbacks, LifecycleOwner, LifecycleProvider {
        private val lifecycle = LifecycleRegistry(this)
        private var registrarActivityHashCode: Int = 0

        init {
            UnityPlayerUtils.activity = activity
            this.registrarActivityHashCode = activity.hashCode()
            activity.application.registerActivityLifecycleCallbacks(this)
        }

        override fun onActivityCreated(activity: Activity, savedInstanceState: Bundle?) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_CREATE)
        }

        override fun onActivityStarted(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_START)
        }

        override fun onActivityResumed(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_RESUME)
        }

        override fun onActivityPaused(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_PAUSE)
        }

        override fun onActivityStopped(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_STOP)
        }

        override fun onActivitySaveInstanceState(activity: Activity, outState: Bundle) {
            UnityPlayerUtils.activity = activity
        }

        override fun onActivityDestroyed(activity: Activity) {
            UnityPlayerUtils.activity = activity
            if (activity.hashCode() != registrarActivityHashCode) {
                return
            }
            activity.application.unregisterActivityLifecycleCallbacks(this)
            lifecycle.handleLifecycleEvent(Lifecycle.Event.ON_DESTROY)
        }

        override fun getLifecycle(): Lifecycle {
            return lifecycle
        }
    }
}