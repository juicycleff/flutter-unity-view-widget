using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine.TestRunner.NUnitExtensions;
using UnityEngine.TestRunner.TestLaunchers;

namespace UnityEditor.TestTools.TestRunner.Api
{
    internal class TestAdaptorFactory : ITestAdaptorFactory
    {
        private Dictionary<string, TestAdaptor> m_TestAdaptorCache = new Dictionary<string, TestAdaptor>();
        private Dictionary<string, TestResultAdaptor> m_TestResultAdaptorCache = new Dictionary<string, TestResultAdaptor>();
        public ITestAdaptor Create(ITest test)
        {
            var uniqueName = test.GetUniqueName();
            if (m_TestAdaptorCache.ContainsKey(uniqueName))
            {
                return m_TestAdaptorCache[uniqueName];
            }

            var adaptor = new TestAdaptor(test, test.Tests.Select(Create).ToArray());
            foreach (var child in adaptor.Children)
            {
                (child as TestAdaptor).SetParent(adaptor);
            }
            m_TestAdaptorCache[uniqueName] = adaptor;
            return adaptor;
        }

        public ITestAdaptor Create(RemoteTestData testData)
        {
            return new TestAdaptor(testData);
        }

        public ITestResultAdaptor Create(ITestResult testResult)
        {
            var uniqueName = testResult.Test.GetUniqueName();
            if (m_TestResultAdaptorCache.ContainsKey(uniqueName))
            {
                return m_TestResultAdaptorCache[uniqueName];
            }
            var adaptor = new TestResultAdaptor(testResult, Create(testResult.Test), testResult.Children.Select(Create).ToArray());
            m_TestResultAdaptorCache[uniqueName] = adaptor;
            return adaptor;
        }

        public ITestResultAdaptor Create(RemoteTestResultData testResult, RemoteTestResultDataWithTestData allData)
        {
            return new TestResultAdaptor(testResult, allData);
        }

        public ITestAdaptor BuildTree(RemoteTestResultDataWithTestData data)
        {
            var tests = data.tests.Select(remoteTestData => new TestAdaptor(remoteTestData)).ToList();

            foreach (var test in tests)
            {
                test.ApplyChildren(tests);
            }

            return tests.First();
        }

        public IEnumerator<ITestAdaptor> BuildTreeAsync(RemoteTestResultDataWithTestData data)
        {
            var tests = data.tests.Select(remoteTestData => new TestAdaptor(remoteTestData)).ToList();

            for (var index = 0; index < tests.Count; index++)
            {
                var test = tests[index];
                test.ApplyChildren(tests);
                if (index % 100 == 0)
                {
                    yield return null;
                }
            }

            yield return tests.First();
        }

        public void ClearResultsCache()
        {
            m_TestResultAdaptorCache.Clear();
        }
    }
}
