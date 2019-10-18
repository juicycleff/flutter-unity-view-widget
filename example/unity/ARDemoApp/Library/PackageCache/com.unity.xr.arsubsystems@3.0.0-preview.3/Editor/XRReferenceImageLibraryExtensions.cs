using System;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARSubsystems
{
    /// <summary>
    /// Extension methods for the <see cref="XRReferenceImageLibrary"/>.
    /// </summary>
    /// <remarks>
    /// At runtime, <see cref="XRReferenceImageLibrary"/>s are immutable. These
    /// Editor-only extension methods let you build and manipulate image libraries
    /// in Editor scripts.
    /// </remarks>
    public static class XRReferenceImageLibraryExtensions
    {
        /// <summary>
        /// Creates an empty <c>XRReferenceImage<c/> and adds it to the library. The new
        /// reference image is inserted at the end of the list of reference images.
        /// </summary>
        /// <param name="library">The <see cref="XRReferenceImageLibrary"/> being extended.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is <c>null</c>.</exception>
        public static void Add(this XRReferenceImageLibrary library)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            library.m_Images.Add(new XRReferenceImage
            {
                m_SerializedGuid = SerializableGuidUtil.Create(Guid.NewGuid())
            });
        }

        /// <summary>
        /// Set the texture on the reference image.
        /// </summary>
        /// <param name="library">The <see cref="XRReferenceImageLibrary"/> being extended.</param>
        /// <param name="index">The reference image index to modify.</param>
        /// <param name="texture">The texture to set.</param>
        /// <param name="keepTexture">Whether to store a strong reference to the texture. If <c>true</c>,
        /// the texture will be available in the Player. Otherwise, <c>XRReferenceImage.texture</c> will be set to <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is <c>null</c>.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <paramref name="library"/><c>.count - 1</c>.</exception>
        public static void SetTexture(this XRReferenceImageLibrary library, int index, Texture2D texture, bool keepTexture)
        {
            ValidateAndThrow(library, index);

            var referenceImage = library.m_Images[index];
            referenceImage.m_SerializedTextureGuid = SerializableGuidUtil.Create(GetGuidForTexture(texture));
            referenceImage.m_Texture = keepTexture ? texture : null;
            library.m_Images[index] = referenceImage;
        }

        /// <summary>
        /// Sets the <c>XRReferenceImage.specifySize</c> value on the <c><c>XRReferenceImage</c> at <paramref name="index"/>.
        /// This value is read-only in the Player; it can only be modified in the Editor.
        /// </summary>
        /// <param name="library">The <c>XRReferenceImageLibrary</c> being extended.</param>
        /// <param name="index">The index of the reference image within the library to modify.</param>
        /// <param name="specifySize">Whether <c>XRReferenceImage.size</c> is specified.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is <c>null</c>.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <paramref name="library"/><c>.count - 1</c>.</exception>
        public static void SetSpecifySize(this XRReferenceImageLibrary library, int index, bool specifySize)
        {
            ValidateAndThrow(library, index);

            var image = library.m_Images[index];
            image.m_SpecifySize = specifySize;
            library.m_Images[index] = image;
        }

        /// <summary>
        /// Sets the <c>XRReferenceImage.size</c> value on the <c><c>XRReferenceImage</c> at <paramref name="index"/>.
        /// This value is read-only in the Player; it can only be modified in the Editor.
        /// </summary>
        /// <param name="library">The <c>XRReferenceImageLibrary</c> being extended.</param>
        /// <param name="index">The index of the reference image within the library to modify.</param>
        /// <param name="size"></param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is <c>null</c>.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <paramref name="library"/><c>.count - 1</c>.</exception>
        public static void SetSize(this XRReferenceImageLibrary library, int index, Vector2 size)
        {
            ValidateAndThrow(library, index);

            var image = library.m_Images[index];
            image.m_Size = size;
            library.m_Images[index] = image;
        }

        /// <summary>
        /// Sets the <c>XRReferenceImage.name</c> value on the <c><c>XRReferenceImage</c> at <paramref name="index"/>.
        /// This value is read-only in the Player; it can only be modified in the Editor.
        /// </summary>
        /// <param name="library">The <c>XRReferenceImageLibrary</c> being extended.</param>
        /// <param name="index">The index of the reference image within the library to modify.</param>
        /// <param name="name"></param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is <c>null</c>.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <paramref name="library"/><c>.count - 1</c>.</exception>
        public static void SetName(this XRReferenceImageLibrary library, int index, string name)
        {
            ValidateAndThrow(library, index);

            var image = library.m_Images[index];
            image.m_Name = name;
            library.m_Images[index] = image;
        }

        /// <summary>
        /// Removes the <see cref="XRReferenceImage"/> at <paramref name="index"/>.
        /// </summary>
        /// <param name="library">The <see cref="XRReferenceImageLibrary"/> being extended.</param>
        /// <param name="index">The index in the list of images to remove.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is <c>null</c>.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <paramref name="library"/><c>.count - 1</c>.</exception>
        public static void RemoveAt(this XRReferenceImageLibrary library, int index)
        {
            ValidateAndThrow(library, index);

            library.m_Images.RemoveAt(index);
        }

        static Guid GetGuidForTexture(Texture2D texture)
        {
            if (texture == null)
                return Guid.Empty;

            string guid;
            long localId;
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(texture, out guid, out localId))
                return new Guid(guid);

            return Guid.Empty;
        }

        static void ValidateAndThrow(XRReferenceImageLibrary library, int index)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            if (library.count == 0)
                throw new IndexOutOfRangeException("The reference image library is empty; cannot index into it.");

            if (index < 0 || index >= library.count)
                throw new IndexOutOfRangeException(string.Format("{0} is out of range. 'index' must be between 0 and {1}", index, library.count - 1));
        }
    }
}
