using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A reference image library is a collection of images to search for in
    /// the physical environment when image tracking is enabled.
    /// </summary>
    /// <remarks>
    /// Image libraries are immutable at runtime. To create and manipulate
    /// an image library via Editor scripts, see the extension methods in
    /// <see cref="XRReferenceImageLibraryExtensions"/>.
    /// If you need to mutate the library at runtime, see <see cref="MutableRuntimeReferenceImageLibrary"/>.
    /// </remarks>
    [CreateAssetMenu(fileName="ReferenceImageLibrary", menuName="XR/Reference Image Library", order=1001)]
    public class XRReferenceImageLibrary : ScriptableObject, IReferenceImageLibrary
    {
        /// <summary>
        /// The number of images in the library.
        /// </summary>
        public int count { get { return m_Images.Count; } }

        /// <summary>
        /// Gets an enumerator which can be used to iterate over the images in this library.
        /// </summary>
        /// <example>
        /// This examples iterates over the reference images contained in the library.
        /// <code>
        /// XRReferenceImageLibrary imageLibrary = ...
        /// foreach (var referenceImage in imageLibrary)
        ///     Debug.LogFormat("Image guid: {0}", referenceImage.guid);
        /// </code>
        /// </example>
        /// <returns>An <c>IEnumerator</c> which can be used to iterate over the images in the library.</returns>
        public List<XRReferenceImage>.Enumerator GetEnumerator()
        {
            return m_Images.GetEnumerator();
        }

        /// <summary>
        /// Get an image by index.
        /// </summary>
        /// <param name="index">The index of the image in the library. Must be between 0 and count - 1.</param>
        /// <returns>The <see cref="XRReferenceImage"/> at <paramref name="index"/>.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <see cref="count"/><c> - 1</c>.</exception>
        public XRReferenceImage this[int index]
        {
            get
            {
                if (count == 0)
                    throw new IndexOutOfRangeException("The reference image library is empty; cannot index into it.");

                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException(string.Format("{0} is out of range. 'index' must be between 0 and {1}", index, count - 1));

                return m_Images[index];
            }
        }

        /// <summary>
        /// Get the index of <paramref name="referenceImage"/> in the image library.
        /// </summary>
        /// <param name="referenceImage">The <see cref="XRReferenceImage"/> to find.</param>
        /// <returns>The zero-based index of the <paramref name="referenceImage"/>, or -1 if not found.</returns>
        public int indexOf(XRReferenceImage referenceImage)
        {
            return m_Images.IndexOf(referenceImage);
        }

        /// <summary>
        /// A <c>Guid</c> associated with this reference library.
        /// The Guid is used to uniquely identify this library at runtime.
        /// </summary>
        public Guid guid
        {
            get { return GuidUtil.Compose(m_GuidLow, m_GuidHigh); }
        }

#if UNITY_EDITOR
        void Awake()
        {
            if ((m_GuidLow == 0) && (m_GuidHigh == 0))
            {
                var bytes = Guid.NewGuid().ToByteArray();
                m_GuidLow = BitConverter.ToUInt64(bytes, 0);
                m_GuidHigh = BitConverter.ToUInt64(bytes, 8);
                EditorUtility.SetDirty(this);
            }
        }
#endif

#pragma warning disable CS0649
        [SerializeField]
        ulong m_GuidLow;

        [SerializeField]
        ulong m_GuidHigh;
#pragma warning restore CS0649

        [SerializeField]
        internal List<XRReferenceImage> m_Images = new List<XRReferenceImage>();
    }
}
