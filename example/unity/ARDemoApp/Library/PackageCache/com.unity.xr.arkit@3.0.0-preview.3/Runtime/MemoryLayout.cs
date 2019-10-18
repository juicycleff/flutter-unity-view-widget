using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARKit
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MemoryLayout : IEquatable<MemoryLayout>
    {
        /// <summary>
        /// The number of bytes required to store a single session relative data object
        /// </summary>
        public int size;

        /// <summary>
        /// The number of bytes between consecutive elements in an array of session relative data objects
        /// </summary>
        public int stride;

        /// <summary>
        /// The alignment required by a session relative data object
        /// </summary>
        public int alignment;

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = size.GetHashCode();
                hash = hash * 486187739 + stride.GetHashCode();
                hash = hash * 486187739 + alignment.GetHashCode();
                return hash;
            }
        }
        public bool Equals(MemoryLayout other)
        {
            return
                (size == other.size) &&
                (stride == other.stride) &&
                (alignment == other.alignment);
        }

        public override bool Equals(object obj) => (obj is MemoryLayout) && Equals((MemoryLayout)obj);
        public static bool operator ==(MemoryLayout lhs, MemoryLayout rhs) => lhs.Equals(rhs);
        public static bool operator !=(MemoryLayout lhs, MemoryLayout rhs) => !lhs.Equals(rhs);
    }
}
