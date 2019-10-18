using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARKit
{
    /// <summary>
    /// Importer for <c>.arobject</c> files.
    /// See <a href="https://developer.apple.com/documentation/arkit/scanning_and_detecting_3d_objects">Scanning and Detecting 3D Objects</a>
    /// for instructions on how to generate these files.
    /// </summary>
    /// <seealso cref="ARKitReferenceObjectEntry"/>
    [ScriptedImporter(1, "arobject")]
    public class ARObjectImporter : ScriptedImporter
    {
        /// <summary>
        /// Invoked automatically when a <c>.arobject</c> file is imported.
        /// </summary>
        /// <param name="ctx">The context associated with the asset import.</param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var entry = ScriptableObject.CreateInstance<ARKitReferenceObjectEntry>();
            var arobject = ReadARObject(ctx.assetPath);
            if (arobject.HasValue)
            {
                entry.m_ReferenceOrigin = arobject.Value.info.referenceOrigin;
            }

            ctx.AddObjectToAsset("arobject", entry, arobject.HasValue ? arobject.Value.preview : null);
            ctx.SetMainObject(entry);
        }

        /// <summary>
        /// Attempts to read the contents of the <c>.arobject</c> archive.
        /// </summary>
        /// <param name="path">The path to a <c>.arobject</c> archive.</param>
        /// <returns>If successful, a <see cref="ARObject"/> describing the archive. Otherwise, <c>null</c>.</returns>
        public static ARObject? ReadARObject(string path)
        {
            var info = ReadInfo(path);
            if (!info.HasValue)
                return null;

            var preview = new Texture2D(1, 1);
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (string.Equals(entry.Name, info.Value.imageReference, StringComparison.OrdinalIgnoreCase))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                entry.Open().CopyTo(memoryStream);
                                if (ImageConversion.LoadImage(preview, memoryStream.ToArray()))
                                    return new ARObject(info.Value, RotateTextureClockwise(preview));
                            }
                        }
                    }
                }
            }

            return new ARObject(info.Value, null);
        }

        /// <summary>
        /// Attempts to read metadata from the <c>.arobject</c> archive.
        /// </summary>
        /// <param name="path">The path to a <c>.arobject</c> archive.</param>
        /// <returns>If successful, a <see cref="ARObjectInfo"/> containing metadata describing the archive. Otherwise, <c>null</c>.</returns>
        public static ARObjectInfo? ReadInfo(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (string.Equals(entry.Name, "Info.plist", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var stream = entry.Open())
                            using (var reader = new StreamReader(stream))
                            {
                                var plist = new XmlDocument();
                                plist.Load(reader);
                                return new ARObjectInfo(plist);
                            }
                        }
                    }
                }
            }

            return null;
        }

        static Texture2D RotateTextureClockwise(Texture2D texture)
        {
            var start = Time.realtimeSinceStartup;
            var pixels = texture.GetPixels32();
            var halfWidth = texture.width / 2;
            var halfHeight = texture.height / 2;
            var w = texture.width;
            var h = texture.height;
            int n = pixels.Length;
            var rotatedPixels = new Color32[n];

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    rotatedPixels[(x + 1) * h - y - 1] = pixels[n - 1 - (y * w + x)];
                }
            }

            var rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotatedPixels);
            rotatedTexture.Apply();
            return rotatedTexture;
        }
    }
}
