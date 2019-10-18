using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Unity.Collections;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARSubsystems;
using Diag = System.Diagnostics;

namespace UnityEditor.XR.ARCore
{
    internal class ARCorePreprocessBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.Android)
                return;

            EnsureARCoreSupportedIsNotChecked();
            EnsureGoogleARCoreIsNotPresent();
            EnsureMinSdkVersion();
            EnsureOnlyOpenGLES3IsUsed();
            EnsureGradleIsUsed();
            BuildImageTrackingAssets();

            BuildHelper.AddBackgroundShaderToProject(ARCoreCameraSubsystem.backgroundShaderName);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.Android)
                return;

            BuildHelper.RemoveShaderFromProject(ARCoreCameraSubsystem.backgroundShaderName);
            RemoveGeneratedStreamingAssets();
        }

        void EnsureGradleIsUsed()
        {
            if (EditorUserBuildSettings.androidBuildSystem != AndroidBuildSystem.Gradle)
                throw new BuildFailedException("ARCore XR Plugin requires the Gradle build system. See File > Build Settings... > Android");
        }

        void EnsureMinSdkVersion()
        {
            var arcoreSettings = ARCoreSettings.GetOrCreateSettings();
            int minSdkVersion;
            if (arcoreSettings.requirement == ARCoreSettings.Requirement.Optional)
                minSdkVersion = 14;
            else
                minSdkVersion = 24;

            if ((int)PlayerSettings.Android.minSdkVersion < minSdkVersion)
                throw new BuildFailedException(string.Format("ARCore {0} apps require a minimum SDK version of {1}. Currently set to {2}",
                    arcoreSettings.requirement, minSdkVersion, PlayerSettings.Android.minSdkVersion));
        }

        void EnsureARCoreSupportedIsNotChecked()
        {
            if (PlayerSettings.Android.ARCoreEnabled)
                throw new BuildFailedException("\"ARCore Supported\" (Player Settings > XR Settings) refers to the built-in ARCore support in Unity and conflicts with the \"ARCore XR Plugin\" package.");
        }

        void EnsureGoogleARCoreIsNotPresent()
        {
            var googleARAssetPath = AssetDatabase.GUIDToAssetPath("afb3e05691ff94d2cbad20643e5c5879");
            if (!string.IsNullOrEmpty(googleARAssetPath))
                throw new BuildFailedException("GoogleARCore detected. Google's \"ARCore SDK for Unity\" and Unity's \"ARCore XR Plugin\" package cannot be used together.");
        }

        void EnsureOnlyOpenGLES3IsUsed()
        {
            var graphicsApis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
            if (graphicsApis.Length > 0)
            {
                var graphicsApi = graphicsApis[0];
                if (graphicsApi != GraphicsDeviceType.OpenGLES3)
                    throw new BuildFailedException(
                        string.Format("You have enabled the {0} graphics API, which is not supported by ARCore.", graphicsApi));
            }
        }

        void SetExecutablePermission(string pathToARCoreImg)
        {
            var startInfo = new Diag.ProcessStartInfo();
            startInfo.WindowStyle = Diag.ProcessWindowStyle.Hidden;
            startInfo.FileName = "/bin/chmod";

            startInfo.Arguments = string.Format(
                "+x {0}",
                pathToARCoreImg);

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            var process = new Diag.Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.Start();
            process.WaitForExit();
        }

        void BuildImageTrackingAssets()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                s_ShouldDeleteStreamingAssetsFolder = false;
            }
            else
            {
                // Delete the streaming assets folder at the end of the build pipeline
                // since it did not exist before we created it here.
                s_ShouldDeleteStreamingAssetsFolder = true;
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            if (!Directory.Exists(ARCoreImageTrackingProvider.k_StreamingAssetsPath))
                Directory.CreateDirectory(ARCoreImageTrackingProvider.k_StreamingAssetsPath);

            try
            {
                string[] libGuids = AssetDatabase.FindAssets("t:xrReferenceImageLibrary");
                if (libGuids == null || libGuids.Length == 0)
                    return;

                // This is how much each library will contribute to the overall progress
                var progressPerLibrary = 1f / libGuids.Length;
                const string progressBarText = "Building ARCore Image Library";

                for (int libraryIndex = 0; libraryIndex < libGuids.Length; ++libraryIndex)
                {
                    var libGuid = libGuids[libraryIndex];
                    var overallProgress = progressPerLibrary * libraryIndex;
                    var numSteps = libGuids.Length + 1; // 1 per image plus arcoreimg
                    var libraryPath = AssetDatabase.GUIDToAssetPath(libGuid);
                    var imageLib = AssetDatabase.LoadAssetAtPath<XRReferenceImageLibrary>(libraryPath);

                    EditorUtility.DisplayProgressBar(progressBarText, imageLib.name, overallProgress);

                    var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    var inputImageListPath = Path.Combine(tempDirectory, Guid.NewGuid().ToString("N") + ".txt");

                    // prepare text file for arcoreimg to read from
                    try
                    {
                        Directory.CreateDirectory(tempDirectory);
                        using (var writer = new StreamWriter(inputImageListPath, false))
                        {
                            for (int i = 0; i < imageLib.count; i++)
                            {
                                var referenceImage = imageLib[i];
                                var textureGuid = referenceImage.textureGuid.ToString("N");
                                var assetPath = AssetDatabase.GUIDToAssetPath(textureGuid);
                                var referenceImageName = referenceImage.guid.ToString("N");

                                EditorUtility.DisplayProgressBar(
                                    progressBarText,
                                    imageLib.name + ": " + assetPath,
                                    overallProgress + progressPerLibrary * i / numSteps);

                                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                                if (texture == null)
                                    throw new BuildFailedException(string.Format(
                                        "ARCore Image Library Generation: Reference library at '{0}' is missing a texture at index {1}.",
                                        libraryPath, i));

                                var extension = Path.GetExtension(assetPath);

                                var entry = new StringBuilder();

                                if (string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase))
                                {
                                    entry.Append(referenceImageName).Append('|').Append(assetPath);
                                }
                                else
                                {
                                    var pngFilename = Path.Combine(tempDirectory, textureGuid + ".png");
                                    var bytes = ImageConversion.EncodeToPNG(texture);
                                    if (bytes == null)
                                    {
                                        throw new BuildFailedException(string.Format(
                                            "ARCore Image Library Generation: Texture format for image '{0}' not supported. Inspect other error messages emitted during this build for more details.",
                                            texture.name));
                                    }

                                    File.WriteAllBytes(pngFilename, bytes);
                                    entry.Append(referenceImageName).Append('|').Append(pngFilename);
                                }

                                if (referenceImage.specifySize)
                                    entry.Append('|').Append(referenceImage.width.ToString("G", CultureInfo.InvariantCulture));

                                writer.WriteLine(entry.ToString());
                            }
                        }
                    }
                    catch
                    {
                        Directory.Delete(tempDirectory, true);
                        throw;
                    }

                    // launch arcoreimg and wait for it to return so we can process the asset
                    try
                    {
                        EditorUtility.DisplayProgressBar(
                            progressBarText,
                            imageLib.name + ": Invoking arcoreimg",
                            overallProgress + progressPerLibrary * (numSteps - 1) / numSteps);

                        var packagePath = Path.GetFullPath("Packages/com.unity.xr.arcore");

                        string extension = "";
                        string platformName = "Undefined";
    #if UNITY_EDITOR_WIN
                        platformName = "Windows";
                        extension = ".exe";
    #elif UNITY_EDITOR_OSX
                        platformName = "MacOS";
                        extension = "";
    #elif UNITY_EDITOR_LINUX
                        platformName = "Linux";
                        extension = "";
    #endif
                        var arcoreimgPath = Path.Combine(packagePath, "Tools~", platformName, "arcoreimg" + extension);

    #if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
                        SetExecutablePermission(arcoreimgPath);
    #endif

                        var startInfo = new Diag.ProcessStartInfo();
                        startInfo.WindowStyle = Diag.ProcessWindowStyle.Hidden;
                        startInfo.FileName = arcoreimgPath;

                        // This file must have the .imgdb extension (the tool adds it otherwise)
                        var outputDbPath = ARCoreImageTrackingProvider.GetPathForLibrary(imageLib);

                        if (File.Exists(outputDbPath))
                            File.Delete(outputDbPath);

                        startInfo.Arguments = string.Format(
                            "build-db --input_image_list_path={0} --output_db_path={1}",
                            $"\"{inputImageListPath}\"",
                            $"\"{outputDbPath}\"");

                        startInfo.UseShellExecute = false;
                        startInfo.RedirectStandardOutput = true;
                        startInfo.RedirectStandardError = true;
                        startInfo.CreateNoWindow = true;

                        var process = new Diag.Process();
                        process.StartInfo = startInfo;
                        process.EnableRaisingEvents = true;
                        var stdout = new StringBuilder();
                        var stderr = new StringBuilder();
                        process.OutputDataReceived += (sender, args) => stdout.Append(args.Data.ToString());
                        process.ErrorDataReceived += (sender, args) => stderr.Append(args.Data.ToString());
                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                        process.CancelOutputRead();
                        process.CancelErrorRead();

                        if (!File.Exists(outputDbPath))
                            throw new BuildFailedException(string.Format(
                                "Failed to generate image database. Output from arcoreimg:\n\nstdout:\n{0}\n====\n\nstderr:\n{1}\n====",
                                stdout.ToString(),
                                stderr.ToString()));
                    }
                    catch
                    {
                        Debug.LogErrorFormat("Failed to generated ARCore reference image library '{0}'", imageLib.name);
                        throw;
                    }
                    finally
                    {
                        Directory.Delete(tempDirectory, true);
                    }
                }
            }
            catch
            {
                RemoveGeneratedStreamingAssets();
                throw;
            }
        }

        static void RemoveDirectoryWithMetafile(string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);

            var meta = directory + ".meta";
            if (File.Exists(meta))
                File.Delete(meta);
        }

        static void RemoveGeneratedStreamingAssets()
        {
            RemoveDirectoryWithMetafile(ARCoreImageTrackingProvider.k_StreamingAssetsPath);
            if (s_ShouldDeleteStreamingAssetsFolder)
                RemoveDirectoryWithMetafile(Application.streamingAssetsPath);
        }

        static bool s_ShouldDeleteStreamingAssetsFolder;
    }

    internal class ARCoreManifest : IPostGenerateGradleAndroidProject
    {
        static readonly string k_AndroidURI = "http://schemas.android.com/apk/res/android";

        static readonly string k_AndroidNameValue = "com.google.ar.core";

        static readonly string k_AndroidManifestPath = "/src/main/AndroidManifest.xml";

        static readonly string k_AndroidHardwareCameraAr = "android.hardware.camera.ar";

        static readonly string k_AndroidPermissionCamera = "android.permission.CAMERA";

        XmlNode FindFirstChild(XmlNode node, string tag)
        {
            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; ++i)
                {
                    var child = node.ChildNodes[i];
                    if (child.Name == tag)
                        return child;
                }
            }

            return null;
        }

        void AppendNewAttribute(XmlDocument doc, XmlElement element, string attributeName, string attributeValue)
        {
            var attribute = doc.CreateAttribute(attributeName, k_AndroidURI);
            attribute.Value = attributeValue;
            element.Attributes.Append(attribute);
        }

        void FindOrCreateTagWithAttribute(XmlDocument doc, XmlNode containingNode, string tagName,
            string attributeName, string attributeValue)
        {
            if (containingNode.HasChildNodes)
            {
                for (int i = 0; i < containingNode.ChildNodes.Count; ++i)
                {
                    var child = containingNode.ChildNodes[i];
                    if (child.Name == tagName)
                    {
                        var childElement = child as XmlElement;
                        if (childElement != null && childElement.HasAttributes)
                        {
                            var attribute = childElement.GetAttributeNode(attributeName, k_AndroidURI);
                            if (attribute != null && attribute.Value == attributeValue)
                                return;
                        }
                    }
                }
            }

            // Didn't find it, so create it
            var element = doc.CreateElement(tagName);
            AppendNewAttribute(doc, element, attributeName, attributeValue);
            containingNode.AppendChild(element);
        }

        void FindOrCreateTagWithAttributes(XmlDocument doc, XmlNode containingNode, string tagName,
            string firstAttributeName, string firstAttributeValue, string secondAttributeName, string secondAttributeValue)
        {
            if (containingNode.HasChildNodes)
            {
                for (int i = 0; i < containingNode.ChildNodes.Count; ++i)
                {
                    var childNode = containingNode.ChildNodes[i];
                    if (childNode.Name == tagName)
                    {
                        var childElement = childNode as XmlElement;
                        if (childElement != null && childElement.HasAttributes)
                        {
                            var firstAttribute = childElement.GetAttributeNode(firstAttributeName, k_AndroidURI);
                            if (firstAttribute == null || firstAttribute.Value != firstAttributeValue)
                                continue;

                            var secondAttribute = childElement.GetAttributeNode(secondAttributeName, k_AndroidURI);
                            if (secondAttribute != null)
                            {
                                secondAttribute.Value = secondAttributeValue;
                                return;
                            }

                            // Create it
                            AppendNewAttribute(doc, childElement, secondAttributeName, secondAttributeValue);
                            return;
                        }
                    }
                }
            }

            // Didn't find it, so create it
            var element = doc.CreateElement(tagName);
            AppendNewAttribute(doc, element, firstAttributeName, firstAttributeValue);
            AppendNewAttribute(doc, element, secondAttributeName, secondAttributeValue);
            containingNode.AppendChild(element);
        }

        // This ensures the Android Manifest corresponds to
        // https://developers.google.com/ar/develop/java/enable-arcore
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            string manifestPath = path + k_AndroidManifestPath;
            var manifestDoc = new XmlDocument();
            manifestDoc.Load(manifestPath);

            var manifestNode = FindFirstChild(manifestDoc, "manifest");
            if (manifestNode == null)
                return;

            var applicationNode = FindFirstChild(manifestNode, "application");
            if (applicationNode == null)
                return;

            FindOrCreateTagWithAttribute(manifestDoc, manifestNode, "uses-permission", "name", k_AndroidPermissionCamera);
            FindOrCreateTagWithAttributes(manifestDoc, applicationNode, "meta-data", "name", "unityplayer.SkipPermissionsDialog", "value", "true");

            var settings = ARCoreSettings.GetOrCreateSettings();
            if (settings.requirement == ARCoreSettings.Requirement.Optional)
            {
                FindOrCreateTagWithAttributes(manifestDoc, applicationNode, "meta-data", "name", k_AndroidNameValue, "value", "optional");
            }
            else if (settings.requirement == ARCoreSettings.Requirement.Required)
            {
                FindOrCreateTagWithAttributes(manifestDoc, manifestNode, "uses-feature", "name", k_AndroidHardwareCameraAr, "required", "true");
                FindOrCreateTagWithAttributes(manifestDoc, applicationNode, "meta-data", "name", k_AndroidNameValue, "value", "required");
            }

            manifestDoc.Save(manifestPath);
        }

        void DebugPrint(XmlDocument doc)
        {
            var sw = new System.IO.StringWriter();
            var xw = XmlWriter.Create(sw);
            doc.Save(xw);
            Debug.Log(sw);
        }

        public int callbackOrder { get { return 2; } }
    }
}
