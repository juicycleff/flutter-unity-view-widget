using System.Collections.Generic;
using System.Linq;
using UnityEditor.Compilation;
using UnityEditor.TestTools.TestRunner.Api;

namespace UnityEditor.TestTools.TestRunner.UnityTestProtocol
{
    internal class UtpMessageReporter : IUtpMessageReporter
    {
        public ITestRunnerApiMapper TestRunnerApiMapper;
        public IUtpLogger Logger;

        public UtpMessageReporter(IUtpLogger utpLogger)
        {
            TestRunnerApiMapper = new TestRunnerApiMapper();
            Logger = utpLogger;
        }

        public void ReportAssemblyCompilationErrors(string assembly, IEnumerable<CompilerMessage> errorCompilerMessages)
        {
            var compilationErrorMessage = new AssemblyCompilationErrorsMessage
            {
                assembly = assembly,
                errors = errorCompilerMessages.Select(x => x.message).ToArray()
            };

            Logger.Log(compilationErrorMessage);
        }

        public void ReportTestRunStarted(ITestAdaptor testsToRun)
        {
            var msg = TestRunnerApiMapper.MapTestToTestPlanMessage(testsToRun);

            Logger.Log(msg);
        }

        public void ReportTestStarted(ITestAdaptor test)
        {
            if (test.IsSuite)
                return;

            var msg = TestRunnerApiMapper.MapTestToTestStartedMessage(test);

            Logger.Log(msg);
        }

        public void ReportTestFinished(ITestResultAdaptor result)
        {
            if (result.Test.IsSuite)
                return;

            var msg = TestRunnerApiMapper.TestResultToTestFinishedMessage(result);

            Logger.Log(msg);
        }
    }
}
