using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARCore
{
    /// <summary>
    /// Similar to NativeSlice but blittable. Provides a "view"
    /// into a contiguous array of memory. Used to interop with C.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct NativeView
    {
        void* m_Ptr;
        int m_Length;

        public NativeView(void* ptr, int length)
        {
            m_Ptr = ptr;
            m_Length = length;
        }

        public static NativeView Create<T>(NativeArray<T> array) where T : struct => new NativeView(array.GetUnsafePtr(), array.Length);
        public static NativeView Create<T>(NativeSlice<T> slice) where T : struct => new NativeView(slice.GetUnsafePtr(), slice.Length);
    }
}
