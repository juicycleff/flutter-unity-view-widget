#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEditor.XR.ARKit
{
    /// <summary>
    /// Looks at all XRReferenceImageLibraries in the project and generates
    /// an AR Resource Group for each library, then inserts them into a new
    /// Xcode asset catalog called "ARReferenceImages".
    /// </summary>
    static class ARKitReferenceImageLibraryBuildProcessor
    {
        static IEnumerable<ARResourceGroup> ResourceGroups()
        {
            // Create a resource group for each reference image library
            foreach (var library in ARKitBuildProcessor.AssetsOfType<XRReferenceImageLibrary>())
            {
                var resourceGroup = new ARResourceGroup(library.name + "_" + library.guid.ToUUIDString());

                // Create a resource group for each library
                foreach (var referenceImage in library)
                {
                    try
                    {
                        resourceGroup.AddResource(new ARReferenceImage(referenceImage));
                    }
                    catch (ARReferenceImage.InvalidWidthException)
                    {
                        throw new BuildFailedException(string.Format("ARKit requires dimensions for all images. Reference image at index {0} named '{1}' in library '{2}' requires a non-zero width.",
                            library.indexOf(referenceImage), referenceImage.name, AssetDatabase.GetAssetPath(library)));
                    }
                    catch (ARReferenceImage.MissingTextureException)
                    {
                        throw new BuildFailedException(string.Format("Reference image at index {0} named '{1}' in library '{2}' is missing a texture.",
                            library.indexOf(referenceImage), referenceImage.name, AssetDatabase.GetAssetPath(library)));
                    }
                    catch (ARReferenceImage.BadTexturePathException)
                    {
                        throw new BuildFailedException(string.Format("Could not resolve texture path for reference image at index {0} named '{1}' in library '{2}'.",
                            library.indexOf(referenceImage), referenceImage.name, AssetDatabase.GetAssetPath(library)));
                    }
                    catch (ARReferenceImage.LoadTextureException e)
                    {
                        throw new BuildFailedException(string.Format("Could not load texture at path {0} for reference image at index {1} named '{2}' in library '{3}'.",
                            e.path, library.indexOf(referenceImage), referenceImage.name, AssetDatabase.GetAssetPath(library)));
                    }
                    catch (ARReferenceImage.TextureNotExportableException)
                    {
                        throw new BuildFailedException(string.Format(
                            "Reference image at index {0} named '{1}' in library '{2}' could not be exported. " +
                            "ARKit can directly use a texture's source asset if it is a JPG or PNG. " +
                            "For all other formats, the texture must be exported to PNG, which requires the texture to be readable and uncompressed. " +
                            "Change the Texture Import Settings or use a JPG or PNG.",
                            library.indexOf(referenceImage), referenceImage.name, AssetDatabase.GetAssetPath(library)));
                    }
                    catch
                    {
                        Debug.LogErrorFormat("Failed to generate reference image at index {0} named '{1}' in library '{2}'.",
                            library.indexOf(referenceImage), referenceImage.name, AssetDatabase.GetAssetPath(library));

                        throw;
                    }
                }

                yield return resourceGroup;
            }
        }

        // Fail the build if any of the reference images are invalid
        class Preprocessor : IPreprocessBuildWithReport
        {
            public int callbackOrder { get { return 0; } }

            public void OnPreprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                foreach (var resourceGroup in ResourceGroups())
                {
                    // Generating a resource group will throw exceptions
                    // if any of the reference images are invalid.
                }
            }
        }

        class Postprocessor : IPostprocessBuildWithReport
        {
            public int callbackOrder { get { return 0; } }

            public void OnPostprocessBuild(BuildReport report)
            {
                if (report.summary.platform != BuildTarget.iOS)
                    return;

                var buildOutputPath = report.summary.outputPath;

                // Read in the PBXProject
                var project = new PBXProject();
                var pbxProjectPath = PBXProject.GetPBXProjectPath(buildOutputPath);
                project.ReadFromString(File.ReadAllText(pbxProjectPath));

                // Create a new asset catalog
                var assetCatalog = new XcodeAssetCatalog("ARReferenceImages");

                // Generate resource groups and add each one to the asset catalog
                foreach (var resourceGroup in ResourceGroups())
                {
                    assetCatalog.AddResourceGroup(resourceGroup);
                }

                // Create the asset catalog on disk
                assetCatalog.WriteAndAddToPBXProject(project, buildOutputPath);

                // Write out the updated Xcode project file
                File.WriteAllText(pbxProjectPath, project.WriteToString());
            }
        }
    }
}
#endif
