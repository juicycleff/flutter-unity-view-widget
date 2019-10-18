using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// The runtime representation of a <see cref="XRReferenceImageLibrary"/>.
    /// Some libraries are mutable; see <see cref="MutableRuntimeReferenceImageLibrary"/>.
    /// </summary>
    public abstract class RuntimeReferenceImageLibrary : IReferenceImageLibrary
    {
        /// <summary>
        /// Gets the <see cref="XRReferenceImage"/> at the given <paramref name="index"/>.
        /// </summary>
        public XRReferenceImage this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} must be greater than or equal to zero.");

                if (index >= count)
                    throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} must be less than count ({count}).");

                return GetReferenceImageAt(index);
            }
        }

        /// <summary>
        /// The number of reference images contained in this library.
        /// </summary>
        public abstract int count { get; }

        /// <summary>
        /// Derived methods should return the <see cref="XRReferenceImage"/> at the given <paramref name="index"/>.
        /// The <paramref name="index"/> has already been validated to be within the range [0..<see cref="count"/>).
        /// </summary>
        /// <param name="index">The index of the reference image to get.</param>
        /// <returns>A <see cref="XRReferenceImage"/> representing the reference image at index <paramref name="index"/>.</returns>
        protected abstract XRReferenceImage GetReferenceImageAt(int index);
    }
}
