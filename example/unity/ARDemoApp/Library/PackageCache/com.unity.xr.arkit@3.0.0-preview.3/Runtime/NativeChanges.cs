using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeChanges : IDisposable, IEquatable<NativeChanges>
    {
        IntPtr m_Ptr;

        public bool created => m_Ptr != IntPtr.Zero;
        public int addedLength => GetAddedLength(this);
        public int updatedLength => GetUpdatedLength(this);
        public int removedLength => GetRemovedLength(this);
        public MemoryLayout memoryLayout => GetMemoryLayout(this);
        public TrackingState trackingState => GetTrackingState(this);
        public void Dispose() => Api.CFRelease(ref m_Ptr);

        public override bool Equals(object obj) => (obj is NativeChanges) && Equals((NativeChanges)obj);
        public override int GetHashCode() => m_Ptr.GetHashCode();
        public bool Equals(NativeChanges other) => m_Ptr == other.m_Ptr;
        public static bool operator ==(NativeChanges lhs, NativeChanges rhs) => lhs.Equals(rhs);
        public static bool operator !=(NativeChanges lhs, NativeChanges rhs) => !lhs.Equals(rhs);

        [DllImport("__Internal", EntryPoint="UnityARKit_TrackableChanges_addedLength")]
        static extern int GetAddedLength(NativeChanges self);

        [DllImport("__Internal", EntryPoint="UnityARKit_TrackableChanges_updatedLength")]
        static extern int GetUpdatedLength(NativeChanges self);

        [DllImport("__Internal", EntryPoint="UnityARKit_TrackableChanges_removedLength")]
        static extern int GetRemovedLength(NativeChanges self);

        [DllImport("__Internal", EntryPoint="UnityARKit_TrackableChanges_memoryLayout")]
        static extern MemoryLayout GetMemoryLayout(NativeChanges self);

        [DllImport("__Internal", EntryPoint="UnityARKit_TrackableChanges_trackingState")]
        static extern TrackingState GetTrackingState(NativeChanges self);
    }
}
