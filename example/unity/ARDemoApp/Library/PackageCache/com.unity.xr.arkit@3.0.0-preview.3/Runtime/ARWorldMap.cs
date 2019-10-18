using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The space-mapping state and set of planes and reference points from
    /// an AR session. This is a wrapper for
    /// <a href="https://developer.apple.com/documentation/arkit/arworldmap">ARKit's ARWorldMap</a>
    /// Note: The <c>ARWorldMap</c> must be explicitly disposed to avoid leaking native resources.
    /// </summary>
    public struct ARWorldMap : IDisposable, IEquatable<ARWorldMap>
    {
        /// <summary>
        /// Dispose of the world map. This will invalidate any <c>NativeArray</c>s
        /// previously returned by <see cref="GetRawData"/>.
        /// </summary>
        public void Dispose()
        {
            Api.UnityARKit_disposeWorldMap(nativeHandle);
            nativeHandle = k_InvalidHandle;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.Release(m_SafetyHandle);
#endif
        }

        /// <summary>
        /// Use this to determine whether this <c>ARWorldMap</c> is valid, or
        /// if it has been disposed.
        /// </summary>
        public bool valid
        {
            get
            {
                return
                    (nativeHandle != k_InvalidHandle) &&
                    Api.UnityARKit_isWorldMapValid(nativeHandle);
            }
        }

        /// <summary>
        /// Serialize the <c>ARWorldMap</c> to an array of bytes. This array may be saved to disk
        /// and loaded at another time to persist the session, or sent over a network
        /// to another ARKit-enabled app to share the session.
        /// It is an error to call this method after the <c>ARWorldMap</c> has been disposed.
        /// </summary>
        /// <returns>An array of bytes representing the serialized <c>ARWorldMap</c>.</returns>
        public unsafe NativeArray<byte> Serialize(Allocator allocator)
        {
            if (!valid)
                throw new InvalidOperationException("ARWorldMap has been disposed.");

            IntPtr nsdata;
            int length;
            if (!Api.UnityARKit_trySerializeWorldMap(nativeHandle, out nsdata, out length))
                throw new InvalidOperationException("Internal error.");

            var serializedWorldMap = new NativeArray<byte>(
                length, allocator, NativeArrayOptions.UninitializedMemory);

            Api.UnityARKit_copyAndReleaseNsData(
                new IntPtr(serializedWorldMap.GetUnsafePtr()), nsdata, length);

            return serializedWorldMap;
        }

        /// <summary>
        /// Create a new <c>ARWorldMap</c> from a <c>NativeArray</c> of bytes produced
        /// from <see cref="Serialize"/>. Use this to create an <c>ARWorldMap</c> from
        /// a serialized array of bytes, either loaded from disk or sent from another device.
        /// </summary>
        /// <param name="serializedWorldMap">An array of bytes representing a serialized <c>ARWorldMap</c>,
        /// produced by <see cref="Serialize"/>.</param>
        public static unsafe bool TryDeserialize(NativeArray<byte> serializedWorldMap, out ARWorldMap worldMap)
        {
            var nativeHandle = Api.UnityARKit_deserializeWorldMap(
                new IntPtr(serializedWorldMap.GetUnsafePtr()), serializedWorldMap.Length);

            if (nativeHandle == k_InvalidHandle)
            {
                worldMap = default(ARWorldMap);
                return false;
            }

            worldMap = new ARWorldMap(nativeHandle);
            return true;
        }

        public override int GetHashCode()
        {
            return nativeHandle.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARWorldMap))
                return false;

            return Equals((ARWorldMap)obj);
        }

        public bool Equals(ARWorldMap other)
        {
            return (nativeHandle == other.nativeHandle);
        }

        public static bool operator ==(ARWorldMap lhs, ARWorldMap rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARWorldMap lhs, ARWorldMap rhs)
        {
            return !lhs.Equals(rhs);
        }

        internal ARWorldMap(int nativeHandle)
        {
            this.nativeHandle = nativeHandle;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_SafetyHandle = AtomicSafetyHandle.Create();
#endif
        }

        internal const int k_InvalidHandle = 0;

        internal int nativeHandle { get; private set; }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle m_SafetyHandle;
#endif
    }
}
