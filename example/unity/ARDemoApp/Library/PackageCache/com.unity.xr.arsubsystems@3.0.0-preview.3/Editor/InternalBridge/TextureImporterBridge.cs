using System;
using UnityEngine;

namespace UnityEditor.XR.ARSubsystems.InternalBridge
{
    /// <summary>
    /// Extension methods for the <c>UnityEditor.TextureImporter</c>.
    /// </summary>
    public static class TextureImporterInternals
    {
        /// <summary>
        /// Gets the original image dimensions. The texture import settings can affect the resulting
        /// texture size, for instance: rounding to a power of 2.
        /// </summary>
        /// <param name="textureImporter">The <c>TextureImporter</c> on which to operate.</param>
        /// <returns>The original dimensions of the imported image.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="textureImporter"/> is <c>null</c>.</exception>
        public static Vector2Int GetSourceTextureDimensions(TextureImporter textureImporter)
        {
            if (textureImporter == null)
                throw new ArgumentNullException("textureImporter");

            int width = 0;
            int height = 0;
            textureImporter.GetWidthAndHeight(ref width, ref height);
            return new Vector2Int(width, height);
        }
    }
}
