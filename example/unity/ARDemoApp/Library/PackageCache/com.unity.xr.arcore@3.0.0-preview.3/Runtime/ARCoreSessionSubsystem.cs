using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// ARCore implementation of the <c>XRSessionSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARCoreSessionSubsystem : XRSessionSubsystem
    {
        /// <summary>
        /// Creates the provider interface.
        /// </summary>
        /// <returns>The provider interface for ARCore</returns>
        protected override Provider CreateProvider() => new ARCoreProvider(this);

        class ARCoreProvider : Provider
        {
            ARCoreSessionSubsystem m_Subsystem;

            public ARCoreProvider(ARCoreSessionSubsystem subsystem)
            {
                m_Subsystem = subsystem;

                NativeApi.UnityARCore_session_construct(CameraPermissionRequestProvider);
                if (SystemInfo.graphicsMultiThreaded)
                {
                    m_RenderEventFunc = NativeApi.UnityARCore_session_getRenderEventFunc();
                }
            }

            public override void Resume()
            {
                CreateTexture();
                NativeApi.UnityARCore_session_resume();
            }

            public override void Pause() => NativeApi.UnityARCore_session_pause();

            public override void Update(XRSessionUpdateParams updateParams)
            {
                NativeApi.UnityARCore_session_update(
                    updateParams.screenOrientation,
                    updateParams.screenDimensions);
            }

            public override void Destroy()
            {
                DeleteTexture();
                NativeApi.UnityARCore_session_destroy();
            }

            public override void Reset()
            {
                NativeApi.UnityARCore_session_reset();
                if (m_Subsystem.running)
                    Resume();
            }

            public override void OnApplicationPause() => NativeApi.UnityARCore_session_onApplicationPause();

            public override void OnApplicationResume() => NativeApi.UnityARCore_session_onApplicationResume();

            public override Promise<SessionAvailability> GetAvailabilityAsync()
            {
                return ExecuteAsync<SessionAvailability>((context) =>
                {
                    NativeApi.ArPresto_checkApkAvailability(OnCheckApkAvailability, context);
                });
            }

            public override Promise<SessionInstallationStatus> InstallAsync()
            {
                return ExecuteAsync<SessionInstallationStatus>((context) =>
                {
                    NativeApi.ArPresto_requestApkInstallation(true, OnApkInstallation, context);
                });
            }

            public override IntPtr nativePtr => NativeApi.UnityARCore_session_getNativePtr();

            public override TrackingState trackingState => NativeApi.UnityARCore_session_getTrackingState();

            public override NotTrackingReason notTrackingReason => NativeApi.UnityARCore_session_getNotTrackingReason();

            public override bool matchFrameRate
            {
                get => NativeApi.UnityARCore_session_getMatchFrameRateEnabled();
                set => NativeApi.UnityARCore_session_setMatchFrameRateEnabled(value);
            }

            public override int frameRate => 30;

            static Promise<T> ExecuteAsync<T>(Action<IntPtr> apiMethod)
            {
                var promise = new ARCorePromise<T>();
                GCHandle gch = GCHandle.Alloc(promise);
                apiMethod(GCHandle.ToIntPtr(gch));
                return promise;
            }

            [MonoPInvokeCallback(typeof(Action<NativeApi.ArPrestoApkInstallStatus, IntPtr>))]
            static void OnApkInstallation(NativeApi.ArPrestoApkInstallStatus status, IntPtr context)
            {
                var sessionInstallation = SessionInstallationStatus.None;
                switch (status)
                {
                    case NativeApi.ArPrestoApkInstallStatus.ErrorDeviceNotCompatible:
                        sessionInstallation = SessionInstallationStatus.ErrorDeviceNotCompatible;
                        break;

                    case NativeApi.ArPrestoApkInstallStatus.ErrorUserDeclined:
                        sessionInstallation = SessionInstallationStatus.ErrorUserDeclined;
                        break;

                    case NativeApi.ArPrestoApkInstallStatus.Requested:
                        // This shouldn't happen
                        sessionInstallation = SessionInstallationStatus.Error;
                        break;

                    case NativeApi.ArPrestoApkInstallStatus.Success:
                        sessionInstallation = SessionInstallationStatus.Success;
                        break;

                    case NativeApi.ArPrestoApkInstallStatus.Error:
                    default:
                        sessionInstallation = SessionInstallationStatus.Error;
                        break;
                }

                ResolvePromise(context, sessionInstallation);
            }

            [MonoPInvokeCallback(typeof(Action<NativeApi.ArAvailability, IntPtr>))]
            static void OnCheckApkAvailability(NativeApi.ArAvailability availability, IntPtr context)
            {
                var sessionAvailability = SessionAvailability.None;
                switch (availability)
                {
                    case NativeApi.ArAvailability.SupportedNotInstalled:
                    case NativeApi.ArAvailability.SupportedApkTooOld:
                        sessionAvailability = SessionAvailability.Supported;
                        break;

                    case NativeApi.ArAvailability.SupportedInstalled:
                        sessionAvailability = SessionAvailability.Supported | SessionAvailability.Installed;
                        break;

                    default:
                        sessionAvailability = SessionAvailability.None;
                        break;
                }

                ResolvePromise(context, sessionAvailability);
            }

            [MonoPInvokeCallback(typeof(NativeApi.CameraPermissionRequestProviderDelegate))]
            static void CameraPermissionRequestProvider(NativeApi.CameraPermissionsResultCallbackDelegate callback, IntPtr context)
            {
                ARCorePermissionManager.RequestPermission(k_CameraPermissionName, (permissinName, granted) =>
                {
                    callback(granted, context);
                });
            }

            static void ResolvePromise<T>(IntPtr context, T arg) where T : struct
            {
                GCHandle gch = GCHandle.FromIntPtr(context);
                var promise = (ARCorePromise<T>)gch.Target;
                if (promise != null)
                    promise.Resolve(arg);
                gch.Free();
            }

            void IssueRenderEventAndWaitForCompletion(NativeApi.RenderEvent renderEvent)
            {
                // NB: If m_RenderEventFunc is zero, it means
                //     1. We are running in the Editor.
                //     2. The UnityARCore library could not be loaded or similar catastrophic failure.
                if (m_RenderEventFunc != IntPtr.Zero)
                {
                    NativeApi.UnityARCore_session_setRenderEventPending();
                    GL.IssuePluginEvent(m_RenderEventFunc, (int)renderEvent);
                    NativeApi.UnityARCore_session_waitForRenderEvent();
                }
            }

            // Safe to call multiple times; does nothing if already created.
            void CreateTexture()
            {
                if (SystemInfo.graphicsMultiThreaded)
                {
                    IssueRenderEventAndWaitForCompletion(NativeApi.RenderEvent.CreateTexture);
                }
                else
                {
                    NativeApi.UnityARCore_session_createTextureMainThread();
                }
            }

            // Safe to call multiple times; does nothing if already destroyed.
            void DeleteTexture()
            {
                if (SystemInfo.graphicsMultiThreaded)
                {
                    IssueRenderEventAndWaitForCompletion(NativeApi.RenderEvent.DeleteTexture);
                }
                else
                {
                    NativeApi.UnityARCore_session_deleteTextureMainThread();
                }
            }

            const string k_CameraPermissionName = "android.permission.CAMERA";

            IntPtr m_RenderEventFunc;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            XRSessionSubsystemDescriptor.RegisterDescriptor(new XRSessionSubsystemDescriptor.Cinfo
            {
                id = "ARCore-Session",
                subsystemImplementationType = typeof(ARCoreSessionSubsystem),
                supportsInstall = true,
                supportsMatchFrameRate = true
            });
#endif
        }

        static class NativeApi
        {
            public enum ArPrestoApkInstallStatus
            {
                Uninitialized = 0,
                Requested = 1,
                Success = 100,
                Error = 200,
                ErrorDeviceNotCompatible = 201,
                ErrorUserDeclined = 203,
            }

            public enum ArAvailability
            {
                UnknownError = 0,
                UnknownChecking = 1,
                UnknownTimedOut = 2,
                UnsupportedDeviceNotCapable = 100,
                SupportedNotInstalled = 201,
                SupportedApkTooOld = 202,
                SupportedInstalled = 203
            }

            public enum RenderEvent
            {
                CreateTexture,
                DeleteTexture
            }

            public delegate void CameraPermissionRequestProviderDelegate(
                CameraPermissionsResultCallbackDelegate resultCallback,
                IntPtr context);

            public delegate void CameraPermissionsResultCallbackDelegate(
                bool granted,
                IntPtr context);

            [DllImport("UnityARCore")]
            public static extern IntPtr UnityARCore_session_getNativePtr();

            [DllImport("UnityARCore")]
            public static extern void ArPresto_checkApkAvailability(
                Action<ArAvailability, IntPtr> onResult, IntPtr context);

            [DllImport("UnityARCore")]
            public static extern void ArPresto_requestApkInstallation(
                bool userRequested, Action<ArPrestoApkInstallStatus, IntPtr> onResult, IntPtr context);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_update(
                ScreenOrientation orientation,
                Vector2Int screenDimensions);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_construct(
                CameraPermissionRequestProviderDelegate cameraPermissionRequestProvider);

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_destroy();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_resume();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_pause();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_onApplicationResume();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_onApplicationPause();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_reset();

            [DllImport("UnityARCore")]
            public static extern TrackingState UnityARCore_session_getTrackingState();

            [DllImport("UnityARCore")]
            public static extern NotTrackingReason UnityARCore_session_getNotTrackingReason();

            [DllImport("UnityARCore")]
            public static extern IntPtr UnityARCore_session_getRenderEventFunc();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_setRenderEventPending();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_waitForRenderEvent();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_createTextureMainThread();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_deleteTextureMainThread();

            [DllImport("UnityARCore")]
            public static extern bool UnityARCore_session_getMatchFrameRateEnabled();

            [DllImport("UnityARCore")]
            public static extern void UnityARCore_session_setMatchFrameRateEnabled(bool enabled);
        }
    }
}
