using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Holds arrays of changes (added, updated, and removed) to trackables.
    /// This is typically used by subsystems to report changes each frame.
    /// </summary>
    /// <typeparam name="T">The <see cref="ITrackable"/> that can be added and updated.</typeparam>
    public struct TrackableChanges<T> : IDisposable where T : struct, ITrackable
    {
        /// <summary>
        /// An array of added trackables.
        /// </summary>
        public NativeArray<T> added { get { return m_Added; } }

        /// <summary>
        /// An array of updated trackables.
        /// </summary>
        public NativeArray<T> updated { get { return m_Updated; } }

        /// <summary>
        /// An array of <see cref="TrackableId"/>s that have been removed.
        /// </summary>
        public NativeArray<TrackableId> removed { get { return m_Removed; } }

        /// <summary>
        /// Whether the <c>NativeArray</c>s have been created.
        /// </summary>
        public bool isCreated { get; private set; }

        /// <summary>
        /// Constructs a <see cref="TrackableChanges{T}"/> from the number of added, updated, and removed trackables.
        /// This creates the <c>NativeArray</c>s <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/>
        /// and should be disposed by calling <see cref="Dispose"/> on this object.
        /// </summary>
        /// <param name="addedCount">The number of added trackables.</param>
        /// <param name="updatedCount">The number of updated trackables.</param>
        /// <param name="removedCount">The number of removed trackables.</param>
        /// <param name="allocator">
        /// An allocator to use for each of
        /// <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/> arrays.
        /// </param>
        public TrackableChanges(
            int addedCount,
            int updatedCount,
            int removedCount,
            Allocator allocator)
        {
            m_Added = new NativeArray<T>(addedCount, allocator);
            m_Updated = new NativeArray<T>(updatedCount, allocator);
            m_Removed = new NativeArray<TrackableId>(removedCount, allocator);
            isCreated = true;
        }

        /// <summary>
        /// Constructor which creates all three arrays and fills the
        /// <see cref="added"/> and <see cref="updated"/> arrays with repeated copies of
        /// <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="addedCount">The number of added trackables.</param>
        /// <param name="updatedCount">The number of updated trackables.</param>
        /// <param name="removedCount">The number of removed trackables.</param>
        /// <param name="allocator">The allocator to use for each of <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/> arrays.</param>
        /// <param name="defaultValue">The value with which to fill the <see cref="added"/> and <see cref="updated"/> arrays.</param>
        public TrackableChanges(
            int addedCount,
            int updatedCount,
            int removedCount,
            Allocator allocator,
            T defaultValue)
        {
            m_Added = NativeCopyUtility.CreateArrayFilledWithValue(defaultValue, addedCount, allocator);
            m_Updated = NativeCopyUtility.CreateArrayFilledWithValue(defaultValue, updatedCount, allocator);
            m_Removed = new NativeArray<TrackableId>(removedCount, allocator);
            isCreated = true;
        }

        /// <summary>
        /// Constructs a <see cref="TrackableChanges{T}"/> from native memory.
        /// </summary>
        /// <remarks>
        /// Because native code may be using an older version of <typeparamref name="T"/>,
        /// this constructor first fills the <see cref="added"/> and <see cref="updated"/>
        /// arrays with copies of <paramref name="defaultT"/> before copying the data from
        /// the <paramref name="addedPtr"/> and <paramref name="updatedPtr"/> pointers.
        /// This ensures that the addition of new fields to <typeparamref name="T"/> will have
        /// appropriate default values.
        /// </remarks>
        /// <param name="addedPtr">A pointer to a block of memory continaing <paramref name="addedCount"/> elements each of size <paramref name="stride"/>.</param>
        /// <param name="addedCount">The number of added elements.</param>
        /// <param name="updatedPtr">A pointer to a block of memory continaing <paramref name="updatedCount"/> elements each of size <paramref name="stride"/>.</param>
        /// <param name="updatedCount">The number of updated elements.</param>
        /// <param name="removedPtr">A pointer to a block of memory containing <paramref name="removedCount"/> <see cref="TrackableId"/>s.</param>
        /// <param name="removedCount">The number of removed elements.</param>
        /// <param name="defaultT">A default <typeparamref name="T"/> which should be used to fill the <see cref="added"/> and <see cref="updated"/>
        /// arrays before copying data from <paramref name="addedPtr"/> and <paramref name="updatedPtr"/>, respectively.</param>
        /// <param name="stride">The number of bytes for each element in the <paramref name="addedPtr"/> and <paramref name="updatedPtr"/> arrays.</param>
        /// <param name="allocator">An allocator to use when creating the <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/> arrays.</param>
        public unsafe TrackableChanges(
            void* addedPtr, int addedCount,
            void* updatedPtr, int updatedCount,
            void* removedPtr, int removedCount,
            T defaultT, int stride,
            Allocator allocator)
        {
            m_Added = NativeCopyUtility.PtrToNativeArrayWithDefault<T>(defaultT, addedPtr, stride, addedCount, allocator);
            m_Updated = NativeCopyUtility.PtrToNativeArrayWithDefault<T>(defaultT, updatedPtr, stride, updatedCount, allocator);
            m_Removed = new NativeArray<TrackableId>(removedCount, allocator);
            if (removedCount > 0)
            {
                m_Removed.CopyFrom(NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<TrackableId>(
                    removedPtr, removedCount, Allocator.None));
            }
            isCreated = true;
        }

        /// <summary>
        /// Creates a new <see cref="TrackableChanges{T}"/> from existing <c>NativeArrays</c>.
        /// </summary>
        /// <param name="added">An array of added elements.</param>
        /// <param name="updated">An array of updated elements.</param>
        /// <param name="removed">An array of removed elements.</param>
        /// <param name="allocator">The allocator to use for each of the <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/> arrays.</param>
        /// <returns>a new <see cref="TrackableChanges{T}"/> from existing <c>NativeArrays</c>. The caller must <see cref="Dispose"/>
        /// the returned <see cref="TrackableChanges{T}"/> to avoid memory leaks.</returns>
        public static TrackableChanges<T> CopyFrom(
            NativeArray<T> added,
            NativeArray<T> updated,
            NativeArray<TrackableId> removed,
            Allocator allocator)
        {
            var addedCopy = new NativeArray<T>(added.Length, allocator);
            addedCopy.CopyFrom(added);

            var updatedCopy = new NativeArray<T>(updated.Length, allocator);
            updatedCopy.CopyFrom(updated);

            var removedCopy = new NativeArray<TrackableId>(removed.Length, allocator);
            removedCopy.CopyFrom(removed);

            return new TrackableChanges<T>(addedCopy, updatedCopy, removedCopy);
        }

        /// <summary>
        /// Disposes the <see cref="added"/>, <see cref="updated"/>, and <see cref="removed"/> arrays.
        /// Safe to call on a default-constructed <see cref="TrackableChanges{T}"/>.
        /// </summary>
        public void Dispose()
        {
            if (isCreated)
            {
                m_Added.Dispose();
                m_Updated.Dispose();
                m_Removed.Dispose();
            }

            isCreated = false;
        }

        TrackableChanges(
            NativeArray<T> added,
            NativeArray<T> updated,
            NativeArray<TrackableId> removed)
        {
            m_Added = added;
            m_Updated = updated;
            m_Removed = removed;
            isCreated = true;
        }

        NativeArray<T> m_Added;

        NativeArray<T> m_Updated;

        NativeArray<TrackableId> m_Removed;
    }
}
