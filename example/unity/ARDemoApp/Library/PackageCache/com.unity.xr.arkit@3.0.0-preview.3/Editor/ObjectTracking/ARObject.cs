using UnityEngine;

namespace UnityEditor.XR.ARKit
{
    /// <summary>
    /// Represents an ARKit ARObject archive, often a file with a <c>.arobject</c> extension.
    /// See <a href="https://developer.apple.com/documentation/arkit/scanning_and_detecting_3d_objects">Scanning and Detecting 3D Objects</a>
    /// for instructions on how to generate these files.
    /// </summary>
    /// <seealso cref="ARObjectInfo"/>
    /// <seealso cref="ARKitReferenceObjectEntry"/>
    public struct ARObject
    {
        /// <summary>
        /// Constructs a <see cref="ARObject"/>.
        /// </summary>
        /// <param name="info">The <see cref="ARObjectInfo"/> associated with this <see cref="ARObject"/>.</param>
        /// <param name="preview">A preview image associated with the <c>ARObject</c>.</param>
        public ARObject(ARObjectInfo info, Texture2D preview)
        {
            this.info = info;
            this.preview = preview;
        }

        /// <summary>
        /// The <see cref="ARObjectInfo"/> associated with this <see cref="ARObject"/>.
        /// </summary>
        public ARObjectInfo info { get; private set; }

        /// <summary>
        /// A preview image associated with the <c>ARObject</c>.
        /// </summary>
        public Texture2D preview { get; private set; }
    }
}
