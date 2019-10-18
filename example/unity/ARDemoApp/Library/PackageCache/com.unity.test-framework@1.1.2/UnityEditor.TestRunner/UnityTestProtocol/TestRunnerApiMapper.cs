using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor.TestTools.TestRunner.Api;

namespace UnityEditor.TestTools.TestRunner.UnityTestProtocol
{
    internal class TestRunnerApiMapper : ITestRunnerApiMapper
    {
        public TestPlanMessage MapTestToTestPlanMessage(ITestAdaptor testsToRun)
        {
            var testsNames = testsToRun != null ? FlattenTestNames(testsToRun) : new List<string>();

            var msg = new TestPlanMessage
            {
                tests = testsNames
            };

            return msg;
        }

        public TestStartedMessage MapTestToTestStartedMessage(ITestAdaptor test)
        {
            return new TestStartedMessage
            {
                name = test.FullName
            };
        }

        public TestFinishedMessage TestResultToTestFinishedMessage(ITestResultAdaptor result)
        {
            return new TestFinishedMessage
            {
                name = result.Test.FullName,
                duration = Convert.ToUInt64(result.Duration * 1000),
                durationMicroseconds = Convert.ToUInt64(result.Duration * 1000000),
                message = result.Message,
                state = GetTestStateFromResult(result)
            };
        }

        public string GetRunStateFromResultNunitXml(ITestResultAdaptor result)
        {
            var doc = new XmlDocument();
            doc.LoadXml(result.ToXml().OuterXml);
            return doc.FirstChild.Attributes["runstate"].Value;
        }

        public TestState GetTestStateFromResult(ITestResultAdaptor result)
        {
            var state = TestState.Failure;

            if (result.TestStatus == TestStatus.Passed)
            {
                state = TestState.Success;

                var runstate = GetRunStateFromResultNunitXml(result);
                runstate = runstate ?? String.Empty;

                if (runstate.ToLowerInvariant().Equals("explicit"))
                    state = TestState.Skipped;
            }
            else if (result.TestStatus == TestStatus.Skipped)
            {
                state = TestState.Skipped;

                if (result.ResultState.ToLowerInvariant().EndsWith("ignored"))
                    state = TestState.Ignored;
            }
            else
            {
                if (result.ResultState.ToLowerInvariant().Equals("inconclusive"))
                    state = TestState.Inconclusive;

                if (result.ResultState.ToLowerInvariant().EndsWith("cancelled") ||
                    result.ResultState.ToLowerInvariant().EndsWith("error"))
                    state = TestState.Error;
            }

            return state;
        }

        public List<string> FlattenTestNames(ITestAdaptor test)
        {
            var results = new List<string>();

            if (!test.IsSuite)
                results.Add(test.FullName);

            if (test.Children != null && test.Children.Any())
                foreach (var child in test.Children)
                    results.AddRange(FlattenTestNames(child));

            return results;
        }
    }
}
