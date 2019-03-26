/*
MIT License
Copyright (c) 2017 Jiulong Wang
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#if UNITY_IOS

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using Application = UnityEngine.Application;

/// <summary>
/// Adding this post build script to Unity project enables Unity iOS build output to be embedded
/// into existing Xcode Swift project.
///
/// However, since this script touches Unity iOS build output, you will not be able to use Unity
/// iOS build directly in Xcode. As a result, it is recommended to put Unity iOS build output into
/// a temporary directory that you generally do not touch, such as '/tmp'.
///
/// In order for this to work, necessary changes to the target Xcode Swift project are needed.
/// Especially the 'AppDelegate.swift' should be modified to properly initialize Unity.
/// See https://github.com/jiulongw/swift-unity for details.
/// </summary>
public static class XcodePostBuild
{
    /// <summary>
    /// Path to the root directory of Xcode project.
    /// This should point to the directory of '${XcodeProjectName}.xcodeproj'.
    /// It is recommended to use relative path here.
    /// Current directory is the root directory of this Unity project, i.e. the directory of 'Assets' folder.
    /// Sample value: "../xcode"
    /// </summary>
    private const string XcodeProjectRoot = "../../ios";

    /// <summary>
    /// Name of the Xcode project.
    /// This script looks for '${XcodeProjectName} + ".xcodeproj"' under '${XcodeProjectRoot}'.
    /// Sample value: "DemoApp"
    /// </summary>
    private static string XcodeProjectName = Application.productName;

    /// <summary>
    /// Directories, relative to the root directory of the Xcode project, to put generated Unity iOS build output.
    /// </summary>
    private static string ClassesProjectPath = "UnityExport/Classes";
    private static string LibrariesProjectPath = "UnityExport/Libraries";
    private static string DataProjectPath = "UnityExport/Data";

    /// <summary>
    /// Path, relative to the root directory of the Xcode project, to put information about generated Unity output.
    /// </summary>
    private static string ExportsConfigProjectPath = "UnityExport/Exports.xcconfig";

    private static string PbxFilePath = XcodeProjectName + ".xcodeproj/project.pbxproj";

    private const string BackupExtension = ".bak";

    /// <summary>
    /// The identifier added to touched file to avoid double edits when building to existing directory without
    /// replace existing content.
    /// </summary>
    private const string TouchedMarker = "https://github.com/jiulongw/swift-unity#v1";

    [PostProcessBuild]
    public static void OnPostBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            return;
        }

        PatchUnityNativeCode(pathToBuiltProject);

        UpdateUnityIOSExports(pathToBuiltProject);

        UpdateUnityProjectFiles(pathToBuiltProject);
    }

    /// <summary>
    /// Writes current Unity version and output path to 'Exports.xcconfig' file.
    /// </summary>
    private static void UpdateUnityIOSExports(string pathToBuiltProject)
    {
        var config = new StringBuilder();
        config.AppendFormat("UNITY_RUNTIME_VERSION = {0};", Application.unityVersion);
        config.AppendLine();
        config.AppendFormat("UNITY_IOS_EXPORT_PATH = {0};", pathToBuiltProject);
        config.AppendLine();

        var configPath = Path.Combine(XcodeProjectRoot, ExportsConfigProjectPath);
        var configDir = Path.GetDirectoryName(configPath);
        if (!Directory.Exists(configDir))
        {
            Directory.CreateDirectory(configDir);
        }

        File.WriteAllText(configPath, config.ToString());
    }

    /// <summary>
    /// Enumerates Unity output files and add necessary files into Xcode project file.
    /// It only add a reference entry into project.pbx file, without actually copy it.
    /// Xcode pre-build script will copy files into correct location.
    /// </summary>
    private static void UpdateUnityProjectFiles(string pathToBuiltProject)
    {
        var pbx = new PBXProject();
        var pbxPath = Path.Combine(XcodeProjectRoot, PbxFilePath);
        pbx.ReadFromFile(pbxPath);

        // Add UnityExport/Classes
        ProcessUnityDirectory(
            pbx,
            Path.Combine(pathToBuiltProject, "Classes"),
            Path.Combine(XcodeProjectRoot, ClassesProjectPath),
            ClassesProjectPath);

        // Add UnityExport/Libraries
        ProcessUnityDirectory(
            pbx,
            Path.Combine(pathToBuiltProject, "Libraries"),
            Path.Combine(XcodeProjectRoot, LibrariesProjectPath),
            LibrariesProjectPath);

        // Add UnityExport/Data
        var targetGuid = pbx.TargetGuidByName(XcodeProjectName);
        var fileGuid = pbx.AddFolderReference(Path.Combine(pathToBuiltProject, "Data"), DataProjectPath);
        pbx.AddFileToBuild(targetGuid, fileGuid);

        pbx.WriteToFile(pbxPath);
    }

    /// <summary>
    /// Update pbx project file by adding src files and removing extra files that
    /// exists in dest but not in src any more.
    ///
    /// This method only updates the pbx project file. It does not copy or delete
    /// files in Swift Xcode project. The Swift Xcode project will do copy and delete
    /// during build, and it should copy files if contents are different, regardless
    /// of the file time.
    /// </summary>
    /// <param name="pbx">The pbx project.</param>
    /// <param name="src">The directory where Unity project is built.</param>
    /// <param name="dest">The directory of the Swift Xcode project where the
    /// Unity project is embedded into.</param>
    /// <param name="projectPathPrefix">The prefix of project path in Swift Xcode
    /// project for Unity code files. E.g. "DempApp/Unity/Classes" for all files
    /// under Classes folder from Unity iOS build output.</param>
    private static void ProcessUnityDirectory(PBXProject pbx, string src, string dest, string projectPathPrefix)
    {
        var targetGuid = pbx.TargetGuidByName(XcodeProjectName);
        if (string.IsNullOrEmpty(targetGuid))
        {
            throw new Exception(string.Format("TargetGuid could not be found for '{0}'", XcodeProjectName));
        }

        // newFiles: array of file names in build output that do not exist in project.pbx manifest.
        // extraFiles: array of file names in project.pbx manifest that do not exist in build output.
        // Build output files that already exist in project.pbx manifest will be skipped to minimize
        // changes to project.pbx file.
        string[] newFiles, extraFiles;
        CompareDirectories(src, dest, out newFiles, out extraFiles);

        foreach (var f in newFiles)
        {
            if (ShouldExcludeFile(f))
            {
                continue;
            }

            var projPath = Path.Combine(projectPathPrefix, f);
            if (!pbx.ContainsFileByProjectPath(projPath))
            {
                var guid = pbx.AddFile(projPath, projPath);
                pbx.AddFileToBuild(targetGuid, guid);

                Debug.LogFormat("Added file to pbx: '{0}'", projPath);
            }
        }

        foreach (var f in extraFiles)
        {
            var projPath = Path.Combine(projectPathPrefix, f);
            if (pbx.ContainsFileByProjectPath(projPath))
            {
                var guid = pbx.FindFileGuidByProjectPath(projPath);
                pbx.RemoveFile(guid);

                Debug.LogFormat("Removed file from pbx: '{0}'", projPath);
            }
        }
    }

    /// <summary>
    /// Compares the directories. Returns files that exists in src and
    /// extra files that exists in dest but not in src any more. 
    /// </summary>
    private static void CompareDirectories(string src, string dest, out string[] srcFiles, out string[] extraFiles)
    {
        srcFiles = GetFilesRelativePath(src);

        var destFiles = GetFilesRelativePath(dest);
        var extraFilesSet = new HashSet<string>(destFiles);

        extraFilesSet.ExceptWith(srcFiles);
        extraFiles = extraFilesSet.ToArray();
    }

    private static string[] GetFilesRelativePath(string directory)
    {
        var results = new List<string>();

        if (Directory.Exists(directory))
        {
            foreach (var path in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                var relative = path.Substring(directory.Length).TrimStart('/');
                results.Add(relative);
            }
        }

        return results.ToArray();
    }

    private static bool ShouldExcludeFile(string fileName)
    {
        if (fileName.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Make necessary changes to Unity build output that enables it to be embedded into existing Xcode project.
    /// </summary>
    private static void PatchUnityNativeCode(string pathToBuiltProject)
    {
        EditMainMM(Path.Combine(pathToBuiltProject, "Classes/main.mm"));
        EditUnityAppControllerH(Path.Combine(pathToBuiltProject, "Classes/UnityAppController.h"));
        EditUnityAppControllerMM(Path.Combine(pathToBuiltProject, "Classes/UnityAppController.mm"));

        if (Application.unityVersion == "2017.1.1f1")
        {
            EditMetalHelperMM(Path.Combine(pathToBuiltProject, "Classes/Unity/MetalHelper.mm"));
        }

        // TODO: Parse unity version number and do range comparison.
        if (Application.unityVersion.StartsWith("2017.3.0f") || Application.unityVersion.StartsWith("2017.3.1f"))
        {
            EditSplashScreenMM(Path.Combine(pathToBuiltProject, "Classes/UI/SplashScreen.mm"));
        }
    }

    /// <summary>
    /// Edit 'main.mm': removes 'main' entry that would conflict with the Xcode project it embeds into.
    /// </summary>
    private static void EditMainMM(string path)
    {
        EditCodeFile(path, line =>
        {
            if (line.TrimStart().StartsWith("int main", StringComparison.Ordinal))
            {
                return line.Replace("int main", "int old_main");
            }

            return line;
        });
    }

    /// <summary>
    /// Edit 'UnityAppController.h': returns 'UnityAppController' from 'AppDelegate' class.
    /// </summary>
    private static void EditUnityAppControllerH(string path)
    {
        var inScope = false;
        var markerDetected = false;
        var markerAdded = false;

        // Add static GetAppController
        EditCodeFile(path, line =>
        {
            inScope |= line.Contains("- (void)startUnity:");

            if (inScope)
            {
                if (line.Trim() == "")
                {
                    inScope = false;

                    return new string[]
                    {
                        "",
                        "// Added by " + TouchedMarker,
                        "+ (UnityAppController*)GetAppController;",
                        ""
                    };
                }
            }

            return new string[] { line };
        });

        inScope = false;
        markerDetected = false;

        // Modify inline GetAppController
        EditCodeFile(path, line =>
        {
            inScope |= line.Contains("inline UnityAppController");

            if (inScope && !markerDetected)
            {
                if (line.Trim() == "}")
                {
                    inScope = false;
                    markerDetected = true;

                    return new string[]
                    {
                        "// }",
                        "",
                        "static inline UnityAppController* GetAppController()",
                        "{",
                        "    return [UnityAppController GetAppController];",
                        "}",
                    };
                }

                if (!markerAdded)
                {
                    markerAdded = true;
                    return new string[]
                    {
                        "// Modified by " + TouchedMarker,
                        "// " + line,
                    };
                }

                return new string[] { "// " + line };
            }

            return new string[] { line };
        });
    }

    /// <summary>
    /// Edit 'UnityAppController.mm': triggers 'UnityReady' notification after Unity is actually started.
    /// </summary>
    private static void EditUnityAppControllerMM(string path)
    {
        EditCodeFile(path, line =>
        {
            if (line.Trim() == "@end")
            {
                return new string[]
                {
                    "",
                    "// Added by " + TouchedMarker,
                    "static UnityAppController *unityAppController = nil;",
                    "",
                    @"+ (UnityAppController*)GetAppController",
                    "{",
                    "    static dispatch_once_t onceToken;",
                    "    dispatch_once(&onceToken, ^{",
                    "        unityAppController = [[self alloc] init];",
                    "    });",
                    "    return unityAppController;",
                    "}",
                    "",
                    line,
                };
            }

            return new string[] { line };
        });
    }

    /// <summary>
    /// Edit 'MetalHelper.mm': fixes a bug (only in 2017.1.1f1) that causes crash.
    /// </summary>
    private static void EditMetalHelperMM(string path)
    {
        var markerDetected = false;

        EditCodeFile(path, line =>
        {
            markerDetected |= line.Contains(TouchedMarker);

            if (!markerDetected && line.Trim() == "surface->stencilRB = [surface->device newTextureWithDescriptor: stencilTexDesc];")
            {
                return new string[]
                {
                    "",
                    "    // Modified by " + TouchedMarker,
                    "    // Default stencilTexDesc.usage has flag 1. In runtime it will cause assertion failure:",
                    "    // validateRenderPassDescriptor:589: failed assertion `Texture at stencilAttachment has usage (0x01) which doesn't specify MTLTextureUsageRenderTarget (0x04)'",
                    "    // Adding MTLTextureUsageRenderTarget seems to fix this issue.",
                    "    stencilTexDesc.usage |= MTLTextureUsageRenderTarget;",
                    line,
                };
            }

            return new string[] { line };
        });
    }

    /// <summary>
    /// Edit 'SplashScreen.mm': Unity introduces its own 'LaunchScreen.storyboard' since 2017.3.0f3.
    /// Disable it here and use Swift project's launch screen instead.
    /// </summary>
    private static void EditSplashScreenMM(string path)
    {
        var markerDetected = false;
        var markerAdded = false;
        var inScope = false;
        var level = 0;

        EditCodeFile(path, line =>
        {
            inScope |= line.Trim() == "void ShowSplashScreen(UIWindow* window)";
            markerDetected |= line.Contains(TouchedMarker);

            if (inScope && !markerDetected)
            {
                if (line.Trim() == "{")
                {
                    level++;
                }
                else if (line.Trim() == "}")
                {
                    level--;
                }

                if (line.Trim() == "}" && level == 0)
                {
                    inScope = false;
                }

                if (level > 0 && line.Trim().StartsWith("bool hasStoryboard"))
                {
                    return new string[]
                    {
                        "    // " + line,
                        "    bool hasStoryboard = false;",
                    };
                }

                if (!markerAdded)
                {
                    markerAdded = true;
                    return new string[]
                    {
                        "// Modified by " + TouchedMarker,
                        line,
                    };
                }
            }

            return new string[] { line };
        });
    }

    private static void EditCodeFile(string path, Func<string, string> lineHandler)
    {
        EditCodeFile(path, line =>
        {
            return new string[] { lineHandler(line) };
        });
    }

    private static void EditCodeFile(string path, Func<string, IEnumerable<string>> lineHandler)
    {
        var bakPath = path + ".bak";
        if (File.Exists(bakPath))
        {
            File.Delete(bakPath);
        }

        File.Move(path, bakPath);

        using (var reader = File.OpenText(bakPath))
        using (var stream = File.Create(path))
        using (var writer = new StreamWriter(stream))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var outputs = lineHandler(line);
                foreach (var o in outputs)
                {
                    writer.WriteLine(o);
                }
            }
        }
    }
}

#endif