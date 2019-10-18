using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARKit
{
    internal struct NSMutableData : IDisposable, IEquatable<NSMutableData>
    {
        IntPtr m_NativePtr;

        public static implicit operator IntPtr(NSMutableData data) => data.m_NativePtr;

        public unsafe NSMutableData(void* bytes, int length)
        {
            m_NativePtr = UnityARKit_NSMutableData_createWithBytes(bytes, length);
        }

        public NSData ToNSData() => new NSData(m_NativePtr);

        public bool created => m_NativePtr != IntPtr.Zero;

        public unsafe void* bytes => ToNSData().bytes;

        public int length => ToNSData().length;

        public IntPtr ptr => m_NativePtr;

        public unsafe void Append(void* bytes, int length)
        {
            if (!created)
                throw new InvalidOperationException("The NSMutableArray has not been created.");

            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            UnityARKit_NSMutableData_append(
                m_NativePtr,
                bytes,
                length);
        }

        public void Dispose()
        {
            UnityARKit_CFRelease(m_NativePtr);
            m_NativePtr = IntPtr.Zero;
        }

        public override int GetHashCode() => m_NativePtr.GetHashCode();
        public override bool Equals(object obj) => (obj is NSMutableData) && Equals((NSMutableData)obj);
        public bool Equals(NSMutableData other) => m_NativePtr == other.m_NativePtr;
        public static bool operator ==(NSMutableData lhs, NSMutableData rhs) => lhs.Equals(rhs);
        public static bool operator !=(NSMutableData lhs, NSMutableData rhs) => !lhs.Equals(rhs);

        [DllImport("__Internal")]
        static extern void UnityARKit_CFRelease(IntPtr ptr);

        [DllImport("__Internal")]
        static extern unsafe void UnityARKit_NSMutableData_append(
            IntPtr nsMutableData,
            void* bytes,
            int length);

        [DllImport("__Internal")]
        static extern unsafe IntPtr UnityARKit_NSMutableData_createWithBytes(
            void* bytes,
            int length);
    }
}
