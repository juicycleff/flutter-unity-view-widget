using UnityEditor.TestTools.TestRunner.Api;
using UnityEditor.TestTools.TestRunner.CommandLineTest;
using UnityEngine.TestTools.TestRunner.GUI;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner
{
    internal class RerunCallback : ScriptableObject, ICallbacks
    {
        public static bool useMockRunFilter = false;
        public static TestRunnerFilter mockRunFilter = null;

        public void RunFinished(ITestResultAdaptor result)
        {
            if (RerunCallbackData.instance.runFilters == null || RerunCallbackData.instance.runFilters.Length == 0)
                RerunCallbackData.instance.runFilters = new[] {new TestRunnerFilter()};

            var runFilter = RerunCallbackData.instance.runFilters[0];

            if (useMockRunFilter)
            {
                runFilter = mockRunFilter;
            }

            runFilter.testRepetitions--;
            if (runFilter.testRepetitions <= 0 || result.TestStatus != TestStatus.Passed)
            {
                ExitCallbacks.preventExit = false;
                return;
            }

            ExitCallbacks.preventExit = true;
            if (EditorApplication.isPlaying)
            {
                EditorApplication.playModeStateChanged += WaitForExitPlaymode;
                return;
            }

            if (!useMockRunFilter)
            {
                ExecuteTestRunnerAPI();
            }
        }

        private static void WaitForExitPlaymode(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                ExecuteTestRunnerAPI();
            }
        }

        private static void ExecuteTestRunnerAPI()
        {
            var runFilter = RerunCallbackData.instance.runFilters[0];
            var testMode = RerunCallbackData.instance.testMode;

            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            testRunnerApi.Execute(new Api.ExecutionSettings()
            {
                filters = new[]
                {
                    new Filter()
                    {
                        categoryNames = runFilter.categoryNames,
                        groupNames = runFilter.groupNames,
                        testMode = testMode,
                        testNames = runFilter.testNames
                    }
                }
            });
        }

        public void TestStarted(ITestAdaptor test)
        {
        }

        public void TestFinished(ITestResultAdaptor result)
        {
        }

        public void RunStarted(ITestAdaptor testsToRun)
        {
        }
    }
}
