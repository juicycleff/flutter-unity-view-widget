using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARSubsystems
{
    /// <summary>
    /// Editor extensions to the <c>XRReferenceObjectLibrary</c>.
    /// </summary>
    public static class XRReferenceObjectLibraryExtensions
    {
        /// <summary>
        /// Creates a new <c>XRReferenceObject</c> and adds it to the library.
        /// </summary>
        /// <param name="library">The <c>XRReferenceObjectLibrary</c> being extended.</param>
        /// <returns>The index in the library at which the new reference object was created.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is null.</exception>
        public static int Add(this XRReferenceObjectLibrary library)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            ulong guidLow, guidHigh;
            Guid.NewGuid().Decompose(out guidLow, out guidHigh);
            library.m_ReferenceObjects.Add(new XRReferenceObject
            {
                m_Entries = new List<XRReferenceObjectEntry>(),
                m_GuidLow = guidLow,
                m_GuidHigh = guidHigh
            });
            return library.m_ReferenceObjects.Count - 1;
        }

        /// <summary>
        /// Removes the <c>XRReferenceObject</c> at <paramref name="index"/>.
        /// </summary>
        /// <param name="library">The <c>XRReferenceObjectLibrary</c> being extended.</param>
        /// <param name="index">The index of the <c>XRReferenceObject</c> to remove.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <c>library.count - 1</c>.</exception>
        public static void RemoveAt(this XRReferenceObjectLibrary library, int index)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            if (index < 0 || index >= library.count)
                throw new IndexOutOfRangeException(string.Format("index {0} is out of range [0 - {1}]", index, library.count - 1));

            library.m_ReferenceObjects.RemoveAt(index);
        }

        /// <summary>
        /// Sets the name of the <c>XRReferenceObject</c> at <paramref name="index"/>.
        /// </summary>
        /// <param name="library">The <c>XRReferenceObjectLibrary</c> being extended.</param>
        /// <param name="index">The index of the <c>XRReferenceObject</c> to modify.</param>
        /// <param name="name">The new name of the <c>XRReferenceObject</c>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <c>library.count - 1</c>.</exception>
        public static void SetReferenceObjectName(this XRReferenceObjectLibrary library, int index, string name)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            if (index < 0 || index >= library.count)
                throw new IndexOutOfRangeException(string.Format("index {0} is out of range [0 - {1}]", index, library.count - 1));

            var referenceObject = library.m_ReferenceObjects[index];
            referenceObject.m_Name = name;
            library.m_ReferenceObjects[index] = referenceObject;
        }

        /// <summary>
        /// Sets the entry for the given <paramref name="type"/> of the <c>XRReferenceObject</c> at index <paramref name="index"/>.
        /// </summary>
        /// <remarks>
        /// Each reference object contains a list of "entries", one for each provider (implementation of <c>XRObjectTrackingSubsystem</c>).
        /// This method sets the entry for a given type, which is the data that will be used when that provider is active.
        /// </remarks>
        /// <param name="library">The <c>XRReferenceObjectLibrary</c> being extended.</param>
        /// <param name="index">The index of the <c>XRReferenceObject</c> to modify.</param>
        /// <param name="entry"></param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="library"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if <paramref name="index"/> is not between 0 and <c>library.count - 1</c>.</exception>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="type"/> does not derive from <c>XRReferenceObjectEntry</c>.</exception>
        public static void SetReferenceObjectEntry(this XRReferenceObjectLibrary library, int index, Type type, XRReferenceObjectEntry entry)
        {
            if (library == null)
                throw new ArgumentNullException("library");

            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsSubclassOf(typeof(XRReferenceObjectEntry)))
                throw new ArgumentException("The type must derive from XRReferenceObjectEntry", "type");

            if (index < 0 || index >= library.m_ReferenceObjects.Count)
                throw new IndexOutOfRangeException(string.Format("index {0} is out of range [0 - {1}]", index, library.m_ReferenceObjects.Count - 1));

            var referenceObject = library.m_ReferenceObjects[index];
            for (int i = 0; i < referenceObject.m_Entries.Count; ++i)
            {
                if (referenceObject.m_Entries[i].GetType() == type)
                {
                    if (entry == null)
                    {
                        referenceObject.m_Entries.RemoveAt(i);
                    }
                    else
                    {
                        referenceObject.m_Entries[i] = entry;
                    }

                    return;
                }
            }

            // There isn't an entry for the given type, so add it.
            referenceObject.m_Entries.Add(entry);
        }
    }
}
