using System;

namespace UnityEditor.XR.ARSubsystems
{
    /// <summary>
    /// Extensions to <c>System.Guid</c>
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Decomposes a 16-byte <c>Guid</c> into two 8-byte <c>ulong</c>s.
        /// Recompose using <c>UnityEngine.XR.ARSubsystems.GuidUtil.Compopse</c>.
        /// </summary>
        /// <param name="guid">The <c>Guid</c> being extended</param>
        /// <param name="low">The lower 8 bytes of the guid.</param>
        /// <param name="high">The upper 8 bytes of the guid.</param>
        public static void Decompose(this Guid guid, out ulong low, out ulong high)
        {
            var bytes = guid.ToByteArray();
            low = BitConverter.ToUInt64(bytes, 0);
            high = BitConverter.ToUInt64(bytes, 8);
        }
    }
}
