using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARKit
{
    /// <summary>
    /// Represents an AR Reference Object in an Xcode asset catalog. This is the
    /// Xcode representation of a UnityEngine.XR.ARSubsystems.XRReferenceObject.
    /// </summary>
    internal class ARReferenceObject : ARResource
    {
        public class InvalidAssetPathException : Exception {}

        public class MissingMetadataException : Exception {}

        public class MissingTrackingDataException : Exception {}

        public ARReferenceObject(XRReferenceObject referenceObject, ARKitReferenceObjectEntry entry)
        {
            m_Path = AssetDatabase.GetAssetPath(entry);
            if (string.IsNullOrEmpty(m_Path))
            {
                throw new InvalidAssetPathException();
            }

            var info = ARObjectImporter.ReadInfo(m_Path);
            if (!info.HasValue)
            {
                throw new MissingMetadataException();
            }
            else if (string.IsNullOrEmpty(info.Value.trackingDataReference))
            {
                throw new MissingTrackingDataException();
            }

            name = referenceObject.name + "_" + referenceObject.guid.ToUUIDString();
        }

        public override string extension
        {
            get { return "arobject"; }
        }

        public override void Write(string pathToResourceGroup)
        {
            // Get the path to the object
            var pathToObjectInUnityProject = m_Path;
            var objectFilename = Path.GetFileName(pathToObjectInUnityProject);

            // Create the ARReferenceObject in Xcode
            var pathToReferenceObjectInXcode = Path.Combine(pathToResourceGroup, filename);
            Directory.CreateDirectory(pathToReferenceObjectInXcode);

            // Unzip the .arobject into Xcode
            ZipFile.ExtractToDirectory(pathToObjectInUnityProject, pathToReferenceObjectInXcode);

            // Read the plist for the Contents.json
            var pathToPlist = Path.Combine(pathToReferenceObjectInXcode, "Info.plist");
            var plist = new XmlDocument();
            plist.Load(pathToPlist);
            var info = new ARObjectInfo(plist);

            // Translate the plist into Contents.json
            var contents = new Json.ReferenceObject
            {
                info = new Json.AuthorInfo
                {
                    version = 1,
                    author = "unity"
                },
                properties = new Json.ObjectProperties
                {
                    preview = info.imageReference,
                    rotation = new float[4]
                    {
                         info.referenceOrigin.rotation.x,
                         info.referenceOrigin.rotation.y,
                        -info.referenceOrigin.rotation.z,
                        -info.referenceOrigin.rotation.w
                    },
                    content = info.trackingDataReference,
                    translation = new float[3]
                    {
                         info.referenceOrigin.position.x,
                         info.referenceOrigin.position.y,
                        -info.referenceOrigin.position.z
                    },
                    version = info.version
                }
            };

            File.WriteAllText(Path.Combine(pathToReferenceObjectInXcode, "Contents.json"), JsonUtility.ToJson(contents));
            File.Delete(pathToPlist);
        }

        string m_Path;
    }
}
