using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARKit
{
    /// <summary>
    /// Represents an AR Reference Image in an Xcode asset catalog. This is the
    /// Xcode representation of a UnityEngine.XR.ARSubsystems.XRReferenceImage.
    /// </summary>
    internal class ARReferenceImage : ARResource
    {
        public class InvalidWidthException : Exception {}

        public class BadTexturePathException : Exception {}

        public class MissingTextureException : Exception {}

        public class TextureNotExportableException : Exception {}

        public class LoadTextureException : Exception
        {
            public string path { get; private set; }

            public LoadTextureException(string path)
            {
                this.path = path;
            }
        }

        public override string extension
        {
            get { return "arreferenceimage"; }
        }

        public override void Write(string pathToResourceGroup)
        {
            // Create the ARReferenceImage in Xcode
            var pathToReferenceImage = Path.Combine(pathToResourceGroup, filename);
            Directory.CreateDirectory(pathToReferenceImage);

            // Copy or create the texture as a file in Xcode
            var textureFilename = Path.GetFileName(m_TexturePath);
            string destinationPath = Path.Combine(pathToReferenceImage, textureFilename);
            if (m_ImageBytes == null)
            {
                File.Copy(m_TexturePath, destinationPath, true);
            }
            else
            {
                var textureImporter = AssetImporter.GetAtPath(m_TexturePath) as TextureImporter;
                if (textureImporter != null)
                {
                    if (textureImporter.npotScale != TextureImporterNPOTScale.None)
                    {
                        Debug.LogWarningFormat(
                            "The import settings for texture at {0} cause its dimensions to be rounded to a power of two. " +
                            "To use this texture with ARKit, it will be exported to a PNG with the rounded dimensions. " +
                            "This may result in unexpected behavior. " +
                            "You can change this on the texture's import settings under Advanced > Non Power of 2, or " +
                            "use a JPG or PNG, since those formats can be used directly rather than exported with the texture importer settings.",
                            m_TexturePath);
                    }
                }

                // If the image is some other format, then attempt to convert it to a PNG
                textureFilename = Path.ChangeExtension(textureFilename, ".png");
                destinationPath = Path.ChangeExtension(destinationPath, ".png");

                // m_ImageBytes was read in the constructor
                File.WriteAllBytes(destinationPath, m_ImageBytes);
            }

            // Create the Contents.json
            var contents = new Json.ReferenceImage
            {
                info = new Json.AuthorInfo
                {
                    version = 1,
                    author = "unity"
                },
                images = new Json.FilenameWithIdiom[]
                {
                    new Json.FilenameWithIdiom
                    {
                        filename = textureFilename,
                        idiom = "universal"
                    }
                },
                properties = new Json.ImageProperties
                {
                    width = m_Width
                }
            };

            File.WriteAllText(Path.Combine(pathToReferenceImage, "Contents.json"), JsonUtility.ToJson(contents));
        }

        public ARReferenceImage(XRReferenceImage referenceImage)
        {
            if (!referenceImage.specifySize || (referenceImage.width <= 0f))
                throw new InvalidWidthException();

            if (referenceImage.textureGuid.Equals(Guid.Empty))
                throw new MissingTextureException();

            var textureGuid = referenceImage.textureGuid.ToString("N");
            m_TexturePath = AssetDatabase.GUIDToAssetPath(textureGuid);
            if (string.IsNullOrEmpty(m_TexturePath))
                throw new BadTexturePathException();

            m_Texture = AssetDatabase.LoadAssetAtPath<Texture2D>(m_TexturePath) as Texture2D;
            if (m_Texture == null)
                throw new LoadTextureException(m_TexturePath);

            var textureExtension = Path.GetExtension(m_TexturePath);
            if (!string.Equals(textureExtension, ".jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(textureExtension, ".png", StringComparison.OrdinalIgnoreCase))
            {
                // Attempt to read the bytes
                m_ImageBytes = ImageConversion.EncodeToPNG(m_Texture);
                if (m_ImageBytes == null)
                    throw new TextureNotExportableException();
            }

            m_Width = referenceImage.width;
            name = referenceImage.name + "_" + referenceImage.guid.ToUUIDString();
        }

        byte[] m_ImageBytes;

        float m_Width;

        string m_TexturePath;

        Texture2D m_Texture;
    }
}
