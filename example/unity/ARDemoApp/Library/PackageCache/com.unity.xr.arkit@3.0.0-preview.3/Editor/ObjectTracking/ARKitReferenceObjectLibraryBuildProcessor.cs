#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARKit
{
    static class ARKitReferenceObjectLibraryBuildProcessor
    {
        [Flags]
        enum Warnings
        {
            None = 0,
            MissingEntry = 1 << 0,
            EmptyLibrary = 1 << 1,
            MissingName = 1 << 2,
            All =
                MissingEntry |
                EmptyLibrary |
                MissingName
        }

        static IEnumerable<ARResourceGroup> ResourceGroups(Warnings warnings)
        {
            foreach (var library in ARKitBuildProcessor.AssetsOfType<XRReferenceObjectLibrary>())
            {
                // Create a resource group for each reference object library
                var resourceGroup = new ARResourceGroup(library.name + "_" + library.guid.ToUUIDString());
                int resourceCount = 0;

                foreach (var referenceObject in library)
                {
                    if (string.IsNullOrEmpty(referenceObject.name) && (warnings & Warnings.MissingName) != 0)
                    {
                        Debug.LogWarningFormat("Reference object {0} named '{1}' in library {2} does not have a name. The reference object will still work, but you will not be able to refer to it by name.",
                            library.indexOf(referenceObject), referenceObject.name, AssetDatabase.GetAssetPath(library));
                    }

                    var arkitEntry = referenceObject.FindEntry<ARKitReferenceObjectEntry>();
                    if (arkitEntry == null)
                    {
                        if ((warnings & Warnings.MissingEntry) != 0)
                        {
                            Debug.LogWarningFormat("The ARKit variant for reference object {0} named '{1}' in library {2} is missing. This reference object will omitted from the library.",
                                library.indexOf(referenceObject), referenceObject.name, AssetDatabase.GetAssetPath(library));
                        }
                    }
                    else
                    {
                        try
                        {
                            resourceGroup.AddResource(new ARReferenceObject(referenceObject, arkitEntry));
                            resourceCount++;
                        }
                        catch (ARReferenceObject.InvalidAssetPathException)
                        {
                            throw new BuildFailedException(string.Format(
                                "The ARKit variant for reference object {0} named '{1}' in reference object library {2} does not refer to a valid asset file.",
                                library.indexOf(referenceObject), referenceObject.name, AssetDatabase.GetAssetPath(library)));
                        }
                        catch (ARReferenceObject.MissingMetadataException)
                        {
                            throw new BuildFailedException(string.Format(
                                "The ARKit variant for reference object {0} named '{1}' in reference object library {2} could not be read. The arobject file may be corrupt.",
                                library.indexOf(referenceObject), referenceObject.name, AssetDatabase.GetAssetPath(library)));
                        }
                        catch (ARReferenceObject.MissingTrackingDataException)
                        {
                            throw new BuildFailedException(string.Format(
                                "The ARKit variant for reference object {0} named '{1}' in reference object library {2} is missing tracking data (the 3D object scan data). The arobject file may be corrupt.",
                                library.indexOf(referenceObject), referenceObject.name, AssetDatabase.GetAssetPath(library)));
                        }
                    }
                }

                if ((resourceCount == 0) && (warnings & Warnings.EmptyLibrary) != 0)
                {
                    Debug.LogWarningFormat("Reference object library at {0} does not contain any ARKit reference objects. The library will be empty.",
                        AssetDatabase.GetAssetPath(library));
                }

                yield return resourceGroup;
            }
        }

        class Preprocessor : IPreprocessBuildWithReport
        {
            public int callbackOrder { get { return 1; } }

            // Validates the ARKit reference objects
            public void OnPreprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                foreach (var resourceGroup in ResourceGroups(Warnings.None))
                {
                    // Generating a resource group will throw exceptions
                    // if any of the reference objects are invalid.
                }
            }
        }

        class Postprocessor : IPostprocessBuildWithReport
        {
            public int callbackOrder { get { return 1; } }

            public void OnPostprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                var buildOutputPath = report.summary.outputPath;

                // Read in the Xcode project
                var pbxProjectPath = PBXProject.GetPBXProjectPath(buildOutputPath);
                var pbxProject = new PBXProject();
                pbxProject.ReadFromString(File.ReadAllText(pbxProjectPath));

                // Create a new asset catalog
                var assetCatalog = new XcodeAssetCatalog("ARReferenceObjects");

                // Create a resource group for each reference object set
                foreach (var resourceGroup in ResourceGroups(Warnings.All))
                {
                    assetCatalog.AddResourceGroup(resourceGroup);
                }

                // Write the asset catalog to disk
                assetCatalog.WriteAndAddToPBXProject(pbxProject, buildOutputPath);

                // Write out the updated Xcode project
                File.WriteAllText(pbxProjectPath, pbxProject.WriteToString());
            }
        }
    }
}
#endif
