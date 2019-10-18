using System;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Logging;
using UnityEngine.TestTools.TestRunner;

namespace UnityEditor.TestTools.TestRunner
{
    internal abstract class TestLauncherBase
    {
        public abstract void Run();

        protected virtual RuntimePlatform? TestTargetPlatform
        {
            get { return Application.platform; }
        }

        protected bool ExecutePreBuildSetupMethods(ITest tests, ITestFilter testRunnerFilter)
        {
            var attributeFinder = new PrebuildSetupAttributeFinder();
            var logString = "Executing setup for: {0}";
            return ExecuteMethods<IPrebuildSetup>(tests, testRunnerFilter, attributeFinder, logString, targetClass => targetClass.Setup(), TestTargetPlatform);
        }

        public void ExecutePostBuildCleanupMethods(ITest tests, ITestFilter testRunnerFilter)
        {
            ExecutePostBuildCleanupMethods(tests, testRunnerFilter, TestTargetPlatform);
        }

        public static void ExecutePostBuildCleanupMethods(ITest tests, ITestFilter testRunnerFilter, RuntimePlatform? testTargetPlatform)
        {
            var attributeFinder = new PostbuildCleanupAttributeFinder();
            var logString = "Executing cleanup for: {0}";
            ExecuteMethods<IPostBuildCleanup>(tests, testRunnerFilter, attributeFinder, logString, targetClass => targetClass.Cleanup(), testTargetPlatform);
        }

        private static bool ExecuteMethods<T>(ITest tests, ITestFilter testRunnerFilter, AttributeFinderBase attributeFinder, string logString, Action<T> action, RuntimePlatform? testTargetPlatform)
        {
            var exceptionsThrown = false;

            if (testTargetPlatform == null)
            {
                Debug.LogError("Could not determine test target platform from build target " + EditorUserBuildSettings.activeBuildTarget);
                return true;
            }

            foreach (var targetClassType in attributeFinder.Search(tests, testRunnerFilter, testTargetPlatform.Value))
            {
                try
                {
                    var targetClass = (T)Activator.CreateInstance(targetClassType);

                    Debug.LogFormat(logString, targetClassType.FullName);

                    using (var logScope = new LogScope())
                    {
                        action(targetClass);

                        if (logScope.AnyFailingLogs())
                        {
                            var failingLog = logScope.FailingLogs.First();
                            throw new UnhandledLogMessageException(failingLog);
                        }
                        
                        if (logScope.ExpectedLogs.Any())
                        {
                            var expectedLogs = logScope.ExpectedLogs.First();
                            throw new UnexpectedLogMessageException(expectedLogs);
                        }
                    }
                }
                catch (InvalidCastException) {}
                catch (Exception e)
                {
                    Debug.LogException(e);
                    exceptionsThrown = true;
                }
            }

            return exceptionsThrown;
        }
    }
}
