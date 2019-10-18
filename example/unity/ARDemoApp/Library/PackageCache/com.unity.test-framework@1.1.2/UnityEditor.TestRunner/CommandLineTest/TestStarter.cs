using System;
using System.IO;
using UnityEditor.TestRunner.CommandLineParser;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner.CommandLineTest
{
    [InitializeOnLoad]
    static class TestStarter
    {
        static TestStarter()
        {
            if (!ShouldRunTests())
            {
                return;
            }

            if (EditorApplication.isCompiling)
            {
                return;
            }

            executer.ExitOnCompileErrors();

            if (RunData.instance.isRunning)
            {
                executer.SetUpCallbacks(RunData.instance.executionSettings);
                return;
            }

            EditorApplication.update += UpdateWatch;
        }

        static void UpdateWatch()
        {
            EditorApplication.update -= UpdateWatch;

            if (RunData.instance.isRunning)
            {
                return;
            }

            RunData.instance.isRunning = true;
            var commandLineArgs = Environment.GetCommandLineArgs();
            RunData.instance.executionSettings = executer.BuildExecutionSettings(commandLineArgs);
            executer.SetUpCallbacks(RunData.instance.executionSettings);
            executer.InitializeAndExecuteRun(commandLineArgs);
        }

        static bool ShouldRunTests()
        {
            var shouldRunTests = false;
            var optionSet = new CommandLineOptionSet(
                new CommandLineOption("runTests", () => { shouldRunTests = true; }),
                new CommandLineOption("runEditorTests", () => { shouldRunTests = true; })
            );
            optionSet.Parse(Environment.GetCommandLineArgs());
            return shouldRunTests;
        }

        static Executer s_Executer;

        static Executer executer
        {
            get
            {
                if (s_Executer == null)
                {
                    Func<bool> compilationCheck = () => EditorUtility.scriptCompilationFailed;
                    Action<string> actionLogger = (string msg) => { Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, msg); };
                    var apiSettingsBuilder = new SettingsBuilder(new TestSettingsDeserializer(() => new TestSettings()), actionLogger, Debug.LogWarning, File.Exists, compilationCheck);
                    s_Executer = new Executer(ScriptableObject.CreateInstance<TestRunnerApi>(), apiSettingsBuilder, Debug.LogErrorFormat, Debug.LogException, EditorApplication.Exit, compilationCheck);
                }

                return s_Executer;
            }
        }
    }
}
