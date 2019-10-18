using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Utilities for copying native arrays.
    /// </summary>
    public static class NativeCopyUtility
    {
        /// <summary>
        /// Creates a <c>NativeArray</c> from a pointer by first copying <paramref name="length"/>
        /// <paramref name="defaultT"/>s into the <c>NativeArray</c>, and then overwriting the
        /// data in the array with <paramref name="source"/>, assuming each element in <paramref name="source"/>
        /// is <paramref name="sourceElementSize"/> bytes.
        /// </summary>
        /// <remarks>
        /// This is useful for native interops with structs that may change over time. This allows
        /// new fields to be added to the C# struct without breaking data obtained from data calls.
        /// </remarks>
        /// <typeparam name="T">The type of struct to copy.</typeparam>
        /// <param name="defaultT">A default version of <typeparamref name="T"/>, which will be used to first fill the array
        /// before copying from <paramref name="source"/>.</param>
        /// <param name="source">A pointer to a contiguous block of data of size <paramref name="sourceElementSize"/> * <paramref name="length"/>.</param>
        /// <param name="sourceElementSize">The size of one element in <paramref name="source"/>.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <param name="allocator">The allocator to use when creating the <c>NativeArray</c>.</param>
        /// <returns>
        /// A new <c>NativeArray</c> populating with <paramref name="defaultT"/> and <paramref name="source"/>.
        /// The caller owns the memory.
        /// </returns>
        public static unsafe NativeArray<T> PtrToNativeArrayWithDefault<T>(
            T defaultT,
            void* source,
            int sourceElementSize,
            int length,
            Allocator allocator) where T : struct
        {
            var array = CreateArrayFilledWithValue(defaultT, length, allocator);

            // Then overwrite with the source data, which may have a different size
            UnsafeUtility.MemCpyStride(
                array.GetUnsafePtr(),
                UnsafeUtility.SizeOf<T>(),
                source,
                sourceElementSize,
                sourceElementSize, length);

            return array;
        }

        /// <summary>
        /// Fills <paramref name="array"/> with repeated copies of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <c>NativeArray</c>. Must be a <c>struct</c>.</typeparam>
        /// <param name="array">The array to fill.</param>
        /// <param name="value">The value with which to fill the array.</param>
        public static unsafe void FillArrayWithValue<T>(NativeArray<T> array, T value) where T : struct
        {
            // Early out if array is zero, or iOS will crash in MemCpyReplicate.
            if (array.Length == 0)
                return;

            UnsafeUtility.MemCpyReplicate(
                array.GetUnsafePtr(),
                UnsafeUtility.AddressOf(ref value),
                UnsafeUtility.SizeOf<T>(),
                array.Length);
        }

        /// <summary>
        /// Creates a new array allocated with <paramref name="allocator"/> initialized with <paramref name="length"/>
        /// copies of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <c>NativeArray</c> to create. Must be a <c>struct</c>.</typeparam>
        /// <param name="value">The value with which to fill the array.</param>
        /// <param name="length">The length of the array to create.</param>
        /// <param name="allocator">The allocator with which to create the <c>NativeArray</c>.</param>
        /// <returns>A new <c>NativeArray</c> initialized with copies of <paramref name="value"/>.</returns>
        public static unsafe NativeArray<T> CreateArrayFilledWithValue<T>(T value, int length, Allocator allocator) where T : struct
        {
            var array = new NativeArray<T>(length, allocator, NativeArrayOptions.UninitializedMemory);
            FillArrayWithValue(array, value);
            return array;
        }
    }
}
