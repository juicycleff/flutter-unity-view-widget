using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Extension methods for <c>UnityEngine.XR.ARSubsystems.MutableRuntimeReferenceImageLibrary</c>.
    /// </summary>
    public static class MutableRuntimeReferenceImageLibraryExtensions
    {
        /// <summary>
        /// Asynchronously adds <paramref name="texture"/> to <paramref name="library"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Image addition can take some time (several frames) due to extra processing that
        /// must occur to insert the image into the library. This is done using
        /// the [Unity Job System](https://docs.unity3d.com/Manual/JobSystem.html). The returned
        /// [JobHandle](https://docs.unity3d.com/ScriptReference/Unity.Jobs.JobHandle.html) can be used
        /// to chain together multiple tasks or to query for completion, but may be safely discarded if you do not need it.
        /// </para><para>
        /// This job, like all Unity jobs, can have dependencies (using the <paramref name="inputDeps"/>). If you are adding multiple
        /// images to the library, it is not necessary to pass a previous <c>ScheduleAddImageJob</c> JobHandle as the input
        /// dependency to the next <c>ScheduleAddImageJob</c>; they can be processed concurrently.
        /// </para><para>
        /// The bytes of the <paramref name="texture"/> are copied, so the texture may be safely
        /// destroyed after this method returns.
        /// </para>
        /// </remarks>
        /// <param name="library">The <c>MutableRuntimeReferenceImageLibrary</c> being extended.</param>
        /// <param name="texture">The <c>Texture2D</c> to use as image target.</param>
        /// <param name="name">The name of the image.</param>
        /// <param name="widthInMeters">The physical width of the image, in meters.</param>
        /// <param name="inputDeps">Input job dependencies (optional).</param>
        /// <returns>A [JobHandle](https://docs.unity3d.com/ScriptReference/Unity.Jobs.JobHandle.html) which can be used
        /// to chain together multiple tasks or to query for completion. May be safely discarded.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="library"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="texture"/> is <c>null</c>.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <paramref name="texture"/> is not readable.</exception>
        public static JobHandle ScheduleAddImageJob(
            this MutableRuntimeReferenceImageLibrary library,
            Texture2D texture,
            string name,
            float? widthInMeters,
            JobHandle inputDeps = default(JobHandle))
        {
            if (ReferenceEquals(library, null))
                throw new ArgumentNullException(nameof(library));

            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            if (!texture.isReadable)
                throw new InvalidOperationException("The texture must be readable to be used as the source for a reference image.");

            var imageBytesCopy = new NativeArray<byte>(texture.GetRawTextureData<byte>(), Allocator.Persistent);
            try
            {
                Nullable<Vector2> size = null;
                if (widthInMeters.HasValue)
                {
                    size = new Vector2(widthInMeters.Value, widthInMeters.Value * (float)texture.height / (float)texture.width);
                }

                var referenceImage = new XRReferenceImage(SerializableGuid.empty, SerializableGuid.empty, size, name, texture);
                var jobHandle = library.ScheduleAddImageJob(imageBytesCopy, new Vector2Int(texture.width, texture.height), texture.format, referenceImage, inputDeps);
                new DeallocateJob { data = imageBytesCopy }.Schedule(jobHandle);
                return jobHandle;
            }
            catch
            {
                imageBytesCopy.Dispose();
                throw;
            }
        }

        struct DeallocateJob : IJob
        {
            [DeallocateOnJobCompletion]
            public NativeArray<byte> data;

            public void Execute() {}
        }
    }
}
