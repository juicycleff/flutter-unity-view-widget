using System;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A reference image library that can be constructed and modified at runtime.
    /// By contrast, an <see cref="XRReferenceImageLibrary"/> can only be constructed
    /// at edit-time and is immutable at runtime.
    /// </summary>
    /// <remarks>
    /// Subsystem providers must implement this class for their provider if
    /// <see cref="XRImageTrackingSubsystemDescriptor.supportsMutableLibrary"/>
    /// is <c>true</c> to provide the functionality to support runtime mutable libraries.
    /// This is not something consumers of the ARSubsystems package should implement.
    /// </remarks>
    /// <seealso cref="XRImageTrackingSubsystem.CreateRuntimeLibrary(XRReferenceImageLibrary)"/>
    public abstract class MutableRuntimeReferenceImageLibrary : RuntimeReferenceImageLibrary
    {
        /// <summary>
        /// This method should schedule a [Unity Job](https://docs.unity3d.com/Manual/JobSystem.html)
        /// which adds an image to this reference image library.
        /// </summary>
        /// <param name="imageBytes">The raw image bytes in <paramref name="format"/>. Assume the bytes will be valid until the job completes.</param>
        /// <param name="sizeInPixels">The width and height of the image, in pixels.</param>
        /// <param name="format">The format of <paramref name="imageBytes"/>. The format has already been validated with <see cref="IsTextureFormatSupported(TextureFormat)"/>.<param>
        /// <param name="referenceImage">The <see cref="XRReferenceImage"/> data associated with the image to add to the library.
        /// This includes information like physical dimensions, associated <c>Texture2D</c> (optional), and string name.</param>
        /// <param name="inputDeps">Input dependencies for the add image job.</param>
        /// <returns>A [JobHandle](https://docs.unity3d.com/ScriptReference/Unity.Jobs.JobHandle.html) which can be used
        /// to chain together multiple tasks or to query for completion.</returns>
        /// <seealso cref="ScheduleAddImageJob(NativeSlice<byte>, Vector2Int, TextureFormat, XRReferenceImage, JobHandle)"/>
        protected abstract JobHandle ScheduleAddImageJobImpl(
            NativeSlice<byte> imageBytes,
            Vector2Int sizeInPixels,
            TextureFormat format,
            XRReferenceImage referenceImage,
            JobHandle inputDeps);

        /// <summary>
        /// Asynchronously adds an image to this library.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Image addition can take some time (several frames) due to extra processing that
        /// must occur to insert the image into the library. This is done using
        /// the [Unity Job System](https://docs.unity3d.com/Manual/JobSystem.html). The returned
        /// [JobHandle](https://docs.unity3d.com/ScriptReference/Unity.Jobs.JobHandle.html) can be used
        /// to chain together multiple tasks or to query for completion, but may be safely discarded if you do not need it.
        /// </para><para>
        /// This job, like all Unity jobs, can have dependencies (using the <paramref name="inputDeps"/>). This can be useful,
        /// for example, if <paramref name="imageBytes"/> is the output of another job. If you are adding multiple
        /// images to the library, it is not necessary to pass a previous <c>ScheduleAddImageJob</c> JobHandle as the input
        /// dependency to the next <c>ScheduleAddImageJob</c>; they can be processed concurrently.
        /// </para><para>
        /// The <paramref name="imageBytes"/> must be valid until this job completes. The caller is responsible for managing its memory.
        /// </para>
        /// </remarks>
        /// <param name="imageBytes">The raw image bytes in <paramref name="format"/>.</param>
        /// <param name="sizeInPixels">The width and height of the image, in pixels.</param>
        /// <param name="format">The format of <paramref name="imageBytes"/>. Test for and enumerate supported formats with
        /// <see cref="supportedTextureFormatCount"/>, <see cref="GetSupportedTextureFormatAt(int)"/>, and <see cref="IsTextureFormatSupported(TextureFormat)"/>.</param>
        /// <param name="referenceImage">The <see cref="XRReferenceImage"/> data associated with the image to add to the library.
        /// This includes information like physical dimensions, associated <c>Texture2D</c> (optional), and string name.
        /// The <see cref="XRReferenceImage.guid"/> must be set to zero (empty).
        /// A new guid is automatically generated for the new image.</param>
        /// <param name="inputDeps">(Optional) input dependencies for the add image job.</param>
        /// <returns>A [JobHandle](https://docs.unity3d.com/ScriptReference/Unity.Jobs.JobHandle.html) which can be used
        /// to chain together multiple tasks or to query for completion. May be safely discarded.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="imageBytes"/> does not contain any bytes.</exception>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="referenceImage"/><c>.guid</c> is not <c>Guid.Empty</c>.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="referenceImage"/><c>.name</c> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="referenceImage"/><c>.specifySize</c> is <c>true</c> and <paramref name="referenceImage"/><c>.size.x</c> contains a value less than or equal to zero.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <paramref name="texture"/>'s format is not supported. You can query for support using <c>XRImageTrackingSubsystem.IsTextureFormatSupported</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="sizeInPixels"/><c>.x</c> or <paramref name="sizeInPixels"/><c>.y</c> is less than or equal to zero.</exception>
        public JobHandle ScheduleAddImageJob(
            NativeSlice<byte> imageBytes,
            Vector2Int sizeInPixels,
            TextureFormat format,
            XRReferenceImage referenceImage,
            JobHandle inputDeps = default(JobHandle))
        {
            unsafe
            {
                if (imageBytes.GetUnsafePtr() == null)
                    throw new ArgumentException($"{nameof(imageBytes)} does not contain any bytes.", nameof(imageBytes));
            }

            if (!referenceImage.guid.Equals(Guid.Empty))
                throw new ArgumentException($"{nameof(referenceImage)}.{nameof(referenceImage.guid)} must be empty (all zeroes).", $"{nameof(referenceImage)}.{nameof(referenceImage.guid)}");

            // Generate and assign a new guid for the new image
            referenceImage.m_SerializedGuid = GenerateNewGuid();

            if (string.IsNullOrEmpty(referenceImage.name))
                throw new ArgumentNullException($"{nameof(referenceImage)}.{nameof(referenceImage.name)}");

            if (referenceImage.specifySize && referenceImage.size.x <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(referenceImage)}.{nameof(referenceImage.size)}", referenceImage.size.x, $"Invalid physical image dimensions.");

            if (!IsTextureFormatSupported(format))
                throw new InvalidOperationException($"The texture format {format} is not supported by the current image tracking subsystem.");

            if (sizeInPixels.x <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(sizeInPixels)}.{nameof(sizeInPixels.x)}", sizeInPixels.x, "Image width must be greater than zero.");

            if (sizeInPixels.y <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(sizeInPixels)}.{nameof(sizeInPixels.y)}", sizeInPixels.y, "Image height must be greater than zero.");

            return ScheduleAddImageJobImpl(imageBytes, sizeInPixels, format, referenceImage, inputDeps);
        }

        /// <summary>
        /// The number of texture formats that are supported for image addition.
        /// </summary>
        public abstract int supportedTextureFormatCount { get; }

        /// <summary>
        /// Returns the supported texture format at <paramref name="index"/>. Useful for enumerating the supported texture formats for image addition.
        /// </summary>
        /// <param name="index">The index of the format to retrieve.</param>
        /// <returns>The supported format at <paramref name="index"/>.</returns>
        public TextureFormat GetSupportedTextureFormatAt(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} must be greater than or equal to zero.");

            if (index >= supportedTextureFormatCount)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} must be less than {nameof(supportedTextureFormatCount)} ({supportedTextureFormatCount}).");

            return GetSupportedTextureFormatAtImpl(index);
        }

        /// <summary>
        /// Derived methods should return the [TextureFormat](https://docs.unity3d.com/ScriptReference/TextureFormat.html) at the given <paramref name="index"/>.
        /// <paramref name="index"/> has already been validated to be within [0..<see cref="supportedTextureFormatCount"/>).
        /// </summary>
        /// <param name="index">The index of the format to retrieve.</param>
        /// <returns>The supported format at <paramref name="index"/>.</returns>
        protected abstract TextureFormat GetSupportedTextureFormatAtImpl(int index);

        /// <summary>
        /// Determines whether the given <paramref name="format"/> is supported.
        /// </summary>
        /// <param name="format">The [TextureFormat](https://docs.unity3d.com/ScriptReference/TextureFormat.html) to test.</param>
        /// <returns><c>true</c> if <paramref name="format"/> is supported for image addition; otherwise <c>false</c>.</returns>
        public bool IsTextureFormatSupported(TextureFormat format)
        {
            int n = supportedTextureFormatCount;
            for (int i = 0; i < n; ++i)
            {
                if (GetSupportedTextureFormatAtImpl(i) == format)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets an enumerator for this collection of reference images. This allows this image library to act as a collection in a <c>foreach</c> statement.
        /// The <see cref="Enumerator"/> is a <c>struct</c>, so no garbage is generated.
        /// </summary>
        /// <returns>An enumerator that can be used in a <c>foreach</c> statement.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        // Converts a System.Guid into two ulongs
        static unsafe SerializableGuid GenerateNewGuid()
        {
            var newGuid = Guid.NewGuid();
            var trackableId = *(TrackableId*)&newGuid;
            return new SerializableGuid(trackableId.subId1, trackableId.subId2);
        }

        /// <summary>
        /// An enumerator to be used in a <c>foreach</c> statement.
        /// </summary>
        public struct Enumerator : IEquatable<Enumerator>
        {
            internal Enumerator(MutableRuntimeReferenceImageLibrary lib)
            {
                m_Library = lib;
                m_Index = -1;
            }

            MutableRuntimeReferenceImageLibrary m_Library;

            int m_Index;

            /// <summary>
            /// Moves to the next element in the collection.
            /// </summary>
            /// <returns><c>true</c> if <see cref="Current"/> is valid after this call.</returns>
            public bool MoveNext() => ++m_Index < m_Library.count;

            /// <summary>
            /// The current <see cref="XRReferenceImage"/>.
            /// </summary>
            public XRReferenceImage Current => m_Library[m_Index];

            /// <summary>
            /// Disposes of the enumerator. This method does nothing.
            /// </summary>
            public void Dispose() {}

            /// <summary>
            /// Generates a hash code suitable for use in a Dictionary or HashSet.
            /// </summary>
            /// <returns>A hash code of this Enumerator.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = ReferenceEquals(m_Library, null) ? 0 : m_Library.GetHashCode();
                    return hash * 486187739 + m_Index.GetHashCode();
                }
            }

            /// <summary>
            /// Compares for equality.
            /// </summary>
            /// <param name="obj">The <c>object</c> to compare against.</param>
            /// <returns><c>true</c> if <paramref name="obj"/> is of type <see cref="Enumerator"/> and <see cref="Equals(Enumerator)"/> is <c>true</c>.</returns>
            public override bool Equals(object obj) => (obj is Enumerator) && Equals((Enumerator)obj);

            /// <summary>
            /// Compares for equality.
            /// </summary>
            /// <param name="other">The other enumerator to compare against.</param>
            /// <returns><c>true</c> if the other enumerator is equal to this one.</returns>
            public bool Equals(Enumerator other)
            {
                return
                    ReferenceEquals(m_Library, other.m_Library) &&
                    (m_Index == other.m_Index);
            }

            /// <summary>
            /// Compares for equality.
            /// </summary>
            /// <param name="lhs">The left-hand side of the comparison.</param>
            /// <param name="rhs">The right-hand side of the comparison.</param>
            /// <returns>The same as <see cref="Equals(Enumerator)"/>.</returns>
            public static bool operator ==(Enumerator lhs, Enumerator rhs) => lhs.Equals(rhs);

            /// <summary>
            /// Compares for inequality.
            /// </summary>
            /// <param name="lhs">The left-hand side of the comparison.</param>
            /// <param name="rhs">The right-hand side of the comparison.</param>
            /// <returns>The negation of <see cref="Equals(Enumerator)"/>.</returns>
            public static bool operator !=(Enumerator lhs, Enumerator rhs) => !lhs.Equals(rhs);
        }
    }
}
