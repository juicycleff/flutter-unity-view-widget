using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Unity.CodeEditor;

namespace VSCodeEditor {
    [InitializeOnLoad]
    public class VSCodeScriptEditor : IExternalCodeEditor
    {
        const string vscode_argument = "vscode_arguments";
        const string vscode_generate_all = "unity_generate_all_csproj";
        const string vscode_extension = "vscode_userExtensions";
        static readonly GUIContent k_ResetArguments = EditorGUIUtility.TrTextContent("Reset argument");
        string m_Arguments;

        IDiscovery m_Discoverability;
        IGenerator m_ProjectGeneration;

        static readonly string[] k_SupportedFileNames = { "code.exe", "visualstudiocode.app", "visualstudiocode-insiders.app", "vscode.app", "code.app", "code.cmd", "code-insiders.cmd", "code", "com.visualstudio.code" };

        static bool IsOSX => Application.platform == RuntimePlatform.OSXEditor;

        static string GetDefaultApp => EditorPrefs.GetString("kScriptsDefaultApp");

        static string DefaultArgument { get; } = "\"$(ProjectPath)\" -g \"$(File)\":$(Line):$(Column)";
        string Arguments
        {
            get => m_Arguments ?? (m_Arguments = EditorPrefs.GetString(vscode_argument, DefaultArgument));
            set
            {
                m_Arguments = value;
                EditorPrefs.SetString(vscode_argument, value);
            }
        }

        static string[] defaultExtensions
        {
            get
            {
                var customExtensions = new[] {"json", "asmdef", "log"};
                return EditorSettings.projectGenerationBuiltinExtensions
                    .Concat(EditorSettings.projectGenerationUserExtensions)
                    .Concat(customExtensions)
                    .Distinct().ToArray();
            }
        }

        static string[] HandledExtensions
        {
            get
            {
                return HandledExtensionsString
                    .Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.TrimStart('.', '*'))
                    .ToArray();
            }
        }

        static string HandledExtensionsString
        {
            get => EditorPrefs.GetString(vscode_extension, string.Join(";", defaultExtensions));
            set => EditorPrefs.SetString(vscode_extension, value);
        }

        public bool TryGetInstallationForPath(string editorPath, out CodeEditor.Installation installation)
        {
            var lowerCasePath = editorPath.ToLower();
            var filename = Path.GetFileName(lowerCasePath).Replace(" ", "");
            var installations = Installations;
            if (!k_SupportedFileNames.Contains(filename))
            {
                installation = default;
                return false;
            }
            if (!installations.Any())
            {
                installation = new CodeEditor.Installation
                {
                    Name = "Visual Studio Code",
                    Path = editorPath
                };
            }
            else
            {
                try
                {
                    installation = installations.First(inst => inst.Path == editorPath);
                }
                catch (InvalidOperationException)
                {
                    installation = new CodeEditor.Installation
                    {
                        Name = "Visual Studio Code",
                        Path = editorPath
                    };
                }
            }

            return true;
        }

        public void OnGUI()
        {
            Arguments = EditorGUILayout.TextField("External Script Editor Args", Arguments);
            if (GUILayout.Button(k_ResetArguments, GUILayout.Width(120)))
            {
                Arguments = DefaultArgument;
            }
            var prevGenerate = EditorPrefs.GetBool(vscode_generate_all, false);

            var generateAll = EditorGUILayout.Toggle("Generate all .csproj files.", prevGenerate);
            if (generateAll != prevGenerate)
            {
                EditorPrefs.SetBool(vscode_generate_all, generateAll);
            }
            m_ProjectGeneration.GenerateAll(generateAll);

            HandledExtensionsString = EditorGUILayout.TextField(new GUIContent("Extensions handled: "), HandledExtensionsString);
        }

        public void CreateIfDoesntExist()
        {
            if (!m_ProjectGeneration.HasSolutionBeenGenerated())
            {
                m_ProjectGeneration.Sync();
            }
        }

        public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles, string[] movedFiles, string[] movedFromFiles, string[] importedFiles)
        {
            m_ProjectGeneration.SyncIfNeeded(addedFiles.Union(deletedFiles).Union(movedFiles).Union(movedFromFiles), importedFiles);
        }

        public void SyncAll()
        {
            AssetDatabase.Refresh();
            m_ProjectGeneration.Sync();
        }

        public bool OpenProject(string path, int line, int column)
        {
            if (path != "" && !SupportsExtension(path)) // Assets - Open C# Project passes empty path here
            {
                return false;
            }

            if (line == -1)
                line = 1;
            if (column == -1)
                column = 0;

            string arguments;
            if (Arguments != DefaultArgument)
            {
                arguments = m_ProjectGeneration.ProjectDirectory != path
                    ? CodeEditor.ParseArgument(Arguments, path, line, column)
                    : m_ProjectGeneration.ProjectDirectory;
            }
            else
            {
                arguments = $@"""{m_ProjectGeneration.ProjectDirectory}""";
                if (m_ProjectGeneration.ProjectDirectory != path && path.Length != 0)
                {
                    arguments += $@" -g ""{path}"":{line}:{column}";
                }
            }

            if (IsOSX)
            {
                return OpenOSX(arguments);
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetDefaultApp,
                    Arguments = arguments,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                }
            };

            process.Start();
            return true;
        }

        static bool OpenOSX(string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "open",
                    Arguments = $"-n \"{GetDefaultApp}\" --args {arguments}",
                    UseShellExecute = true,
                }
            };

            process.Start();
            return true;
        }

        static bool SupportsExtension(string path)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
                return false;
            return HandledExtensions.Contains(extension.TrimStart('.'));
        }

        public CodeEditor.Installation[] Installations => m_Discoverability.PathCallback();

        public VSCodeScriptEditor(IDiscovery discovery, IGenerator projectGeneration)
        {
            m_Discoverability = discovery;
            m_ProjectGeneration = projectGeneration;
        }

        static VSCodeScriptEditor()
        {
            var editor = new VSCodeScriptEditor(new VSCodeDiscovery(), new ProjectGeneration());
            CodeEditor.Register(editor);

            if (IsVSCodeInstallation(CodeEditor.CurrentEditorInstallation))
            {
                editor.CreateIfDoesntExist();
            }
        }

        static bool IsVSCodeInstallation(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            var lowerCasePath = path.ToLower();
            var filename = Path
                .GetFileName(lowerCasePath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar))
                .Replace(" ", "");
            return k_SupportedFileNames.Contains(filename);
        }

        public void Initialize(string editorInstallationPath)
        {
        }
    }
}
