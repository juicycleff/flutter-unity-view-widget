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

using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

/// <summary>
/// Adding this post build script to Unity project enables the flutter-unity-widget to access it
/// </summary>
public static class XcodePostBuild
{

    /// <summary>
    /// The identifier added to touched file to avoid double edits when building to existing directory without
    /// replace existing content.
    /// </summary>
    private const string TouchedMarker = "https://github.com/snowballdigital/flutter-unity-view-widget";

    [PostProcessBuild]
    public static void OnPostBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            return;
        }

        PatchUnityNativeCode(pathToBuiltProject);

        UpdateUnityProjectFiles(pathToBuiltProject);
    }

    /// <summary>
    /// We need to add the Data folder to the UnityFramework framework
    /// </summary>
    private static void UpdateUnityProjectFiles(string pathToBuiltProject)
    {
        var pbx = new PBXProject();
        var pbxPath = Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj/project.pbxproj");
        pbx.ReadFromFile(pbxPath);

        // Add UnityExport/Data
        var targetGuid = pbx.TargetGuidByName("UnityFramework");
        var fileGuid = pbx.AddFolderReference(Path.Combine(pathToBuiltProject, "Data"), "Data");
        pbx.AddFileToBuild(targetGuid, fileGuid);

        pbx.WriteToFile(pbxPath);
    }

    /// <summary>
    /// Make necessary changes to Unity build output that enables it to be embedded into existing Xcode project.
    /// </summary>
    private static void PatchUnityNativeCode(string pathToBuiltProject)
    {
        EditUnityFrameworkH(Path.Combine(pathToBuiltProject, "UnityFramework/UnityFramework.h"));
        EditUnityAppControllerH(Path.Combine(pathToBuiltProject, "Classes/UnityAppController.h"));
        EditUnityAppControllerMM(Path.Combine(pathToBuiltProject, "Classes/UnityAppController.mm"));
        EditUnityViewMM(Path.Combine(pathToBuiltProject, "Classes/UI/UnityView.mm"));
    }


    /// <summary>
    /// Edit 'UnityFramework.h': add  'frameworkWarmup' 
    /// </summary>
    private static void EditUnityFrameworkH(string path)
    {
        var inScope = false;

        // Add frameworkWarmup method
        EditCodeFile(path, line =>
        {
            inScope |= line.Contains("- (void)runUIApplicationMainWithArgc:");

            if (inScope)
            {
                if (line.Trim() == "")
                {
                    inScope = false;

                    return new string[]
                    {
                        "",
                        "// Added by " + TouchedMarker,
                        "- (void)frameworkWarmup:(int)argc argv:(char*[])argv;",
                        ""
                    };
                }
            }

            return new string[] { line };
        });
    }

    /// <summary>
    /// Edit 'UnityAppController.h': returns 'UnityAppController' from 'AppDelegate' class.
    /// </summary>
    private static void EditUnityAppControllerH(string path)
    {
        var inScope = false;
        var markerDetected = false;

        // Modify inline GetAppController
        EditCodeFile(path, line =>
        {
            inScope |= line.Contains("include \"RenderPluginDelegate.h\"");

            if (inScope && !markerDetected)
            {
                if (line.Trim() == "")
                {
                    inScope = false;
                    markerDetected = true;

                    return new string[]
                    {
                        "",
                        "// Added by " + TouchedMarker,
                        "@protocol UnityEventListener <NSObject>",
                        "- (void)onMessage:(NSString *)message;",
                        "@end",
                        "",
                    };
                }

                return new string[] { line };
            }

            return new string[] { line };
        });

        inScope = false;
        markerDetected = false;

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
                        "+ (void)addUnityEventListenerInternal:(id<UnityEventListener>)listener;",
                        "+ (void)removeUnityEventListenerInternal:(id<UnityEventListener>)listener;",
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
            inScope |= line.Contains("extern UnityAppController* GetAppController");

            if (inScope && !markerDetected)
            {
                if (line.Trim() == "")
                {
                    inScope = false;
					markerDetected = true;

                    return new string[]
                    {
                        "// }",
                        "",
                        "// Added by " + TouchedMarker,
                        "static inline UnityAppController* GetAppController()",
                        "{",
                        "    return [UnityAppController GetAppController];",
                        "}",
                        "",
                        "// Added by " + TouchedMarker,
                        "static inline void addUnityEventListenerInternal(id<UnityEventListener> listener)",
                        "{",
                        "    [UnityAppController addUnityEventListenerInternal: listener];",
                        "}",
                        "",
                        "// Added by " + TouchedMarker,
                        "static inline void removeUnityEventListenerInternal(id<UnityEventListener> listener)",
                        "{",
                        "    [UnityAppController removeUnityEventListenerInternal:listener];",
                        "}"
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
        var inScope = false;
        var markerDetected = false;

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
                    "// Added by " + TouchedMarker,
                    "static NSHashTable* mUnityEventListeners = [NSHashTable weakObjectsHashTable];",
                    "+ (void)addUnityEventListener2:(id<UnityEventListener>)listener",
                    "{",
                    "    [mUnityEventListeners addObject: listener];",
                    "}",
                    "",
                    "// Added by " + TouchedMarker,
                    "+(void)removeUnityEventListener2:(id<UnityEventListener>)listener",
                    "{",
                    "    [mUnityEventListeners removeObject: listener];",
                    "}",
                    line,
                    "// Added by " + TouchedMarker,
                    "extern \"C\" void onUnityMessage(const char* message)",
                    "{",
                    "    for (id<UnityEventListener> listener in mUnityEventListeners) {",
                    "        [listener onMessage:[NSString stringWithUTF8String:message]];",
                    "    }",
                    "}",
				};
            }

            inScope |= line.Contains("- (void)startUnity:");
            markerDetected |= inScope && line.Contains(TouchedMarker);

            if (inScope && line.Trim() == "}")
            {
                inScope = false;

                if (markerDetected)
                {
                    return new string[] { line };
                }
                else
                {
                    return new string[]
                    {
                        "    // Modified by " + TouchedMarker,
                        @"    [[NSNotificationCenter defaultCenter] postNotificationName: @""UnityReady"" object:self];",
                        "}",
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
            inScope |= line.Contains("UnityAppController* GetAppController()");

            if (inScope && !markerDetected)
            {
                if (line.Trim() == "}")
                {
                    inScope = false;
                    markerDetected = true;

                    return new string[]
                    {
                        "",
                    };
                }

                return new string[] { "// " + line };
            }

            return new string[] { line };
        });
    }

    /// <summary>
    /// Edit 'UnityView.mm': fix the width and height needed for the Metal renderer
    /// </summary>
    private static void EditUnityViewMM(string path)
    {
        var inScope = false;

        // Add frameworkWarmup method
        EditCodeFile(path, line =>
        {
            inScope |= line.Contains("UnityGetRenderingResolution(&requestedW, &requestedH)");

            if (inScope)
            {
                if (line.Trim() == "")
                {
                    inScope = false;

                    return new string[]
                    {
                        "",
                        "// Added by " + TouchedMarker,
                        "        if (requestedW == 0) {",
                        "            requestedW = _surfaceSize.width;",
                        "        }",
                        "        if (requestedH == 0) {",
                        "            requestedH = _surfaceSize.height;",
                        "        }",
                        ""
                    };
                }
            }

            return new string[] { line };
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