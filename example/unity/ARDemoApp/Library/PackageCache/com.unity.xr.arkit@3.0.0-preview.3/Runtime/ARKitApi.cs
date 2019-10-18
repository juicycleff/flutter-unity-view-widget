using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    internal static class Api
    {
        [DllImport("__Internal")]
        public static extern IntPtr UnityARKit_TrackableProvider_start(IntPtr self);

        [DllImport("__Internal")]
        public static extern IntPtr UnityARKit_TrackableProvider_stop(IntPtr self);

        [DllImport("__Internal")]
        public static extern NativeChanges UnityARKit_TrackableProvider_acquireChanges(IntPtr self);

        [DllImport("__Internal")]
        public static extern unsafe void UnityARKit_TrackableProvider_copyChanges(
            IntPtr self, NativeChanges changes, int stride,
            void* added, void* updated, void* removed);

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        internal static extern ARWorldMapRequestStatus UnityARKit_getWorldMapRequestStatus(int worldMapId);

        [DllImport("__Internal")]
        internal static extern void UnityARKit_disposeWorldMap(int worldMapId);

        [DllImport("__Internal")]
        internal static extern void UnityARKit_disposeWorldMapRequest(int worldMapId);

        [DllImport("__Internal")]
        internal static extern int UnityARKit_getWorldMapIdFromRequestId(int requestId);

        [DllImport("__Internal")]
        internal static extern bool UnityARKit_isWorldMapValid(int nativeHandle);

        [DllImport("__Internal")]
        internal static extern bool UnityARKit_trySerializeWorldMap(
            int nativeHandle, out IntPtr nsdata, out int length);

        [DllImport("__Internal")]
        internal static extern int UnityARKit_copyAndReleaseNsData(
            IntPtr destination, IntPtr sourceNsData, int length);

        [DllImport("__Internal")]
        internal static extern int UnityARKit_deserializeWorldMap(
            IntPtr buffer, int bufferLength);

        public static void CFRelease(ref IntPtr ptr)
        {
            UnityARKit_CFRelease(ptr);
            ptr = IntPtr.Zero;
        }

        [DllImport("__Internal")]
        static extern void UnityARKit_CFRelease(IntPtr ptr);
#else
        public static void CFRelease(ref IntPtr ptr) => ptr = IntPtr.Zero;

        internal static ARWorldMapRequestStatus UnityARKit_getWorldMapRequestStatus(int worldMapId)
        {
            return default(ARWorldMapRequestStatus);
        }

        internal static void UnityARKit_disposeWorldMap(int worldMapId)
        { }

        internal static void UnityARKit_disposeWorldMapRequest(int worldMapId)
        { }

        internal static int UnityARKit_getWorldMapIdFromRequestId(int requestId)
        {
            return default(int);
        }

        internal static int UnityARKit_deserializeWorldMap(IntPtr buffer, int bufferLength)
        {
            return ARWorldMap.k_InvalidHandle;
        }

        internal static bool UnityARKit_isWorldMapValid(int nativeHandle)
        {
            return false;
        }

        internal static bool UnityARKit_trySerializeWorldMap(
            int nativeHandle, out IntPtr nsdata, out int length)
        {
            nsdata = default(IntPtr);
            length = default(int);
            return false;
        }

        internal static int UnityARKit_copyAndReleaseNsData(
            IntPtr destination, IntPtr sourceNsData, int length)
        {
            return 0;
        }
#endif
    }
}
