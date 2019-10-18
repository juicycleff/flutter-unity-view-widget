using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARCore
{
    internal static class RcoApi
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        [DllImport("UnityARCore", EntryPoint="UnityARCore_rco_retain")]
        public static extern int Retain(IntPtr ptr);

        [DllImport("UnityARCore", EntryPoint="UnityARCore_rco_release")]
        public static extern int Release(IntPtr ptr);

        [DllImport("UnityARCore", EntryPoint="UnityARCore_rco_retain_count")]
        public static extern int RetainCount(IntPtr ptr);
#else
        public static int Retain(IntPtr ptr) => 0;
        public static int Release(IntPtr ptr) => 0;
        public static int RetainCount(IntPtr ptr) => 0;
#endif
    }
}
