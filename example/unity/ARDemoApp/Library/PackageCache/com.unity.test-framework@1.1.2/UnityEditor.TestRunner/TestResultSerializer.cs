using System;
using System.Reflection;
using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner
{
    [Serializable]
    internal class TestResultSerializer
    {
        private static readonly BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        [SerializeField] public string id;

        [SerializeField] public string fullName;

        [SerializeField] private double duration;

        [SerializeField] private string label;

        [SerializeField] private string message;

        [SerializeField] private string output;

        [SerializeField] private string site;

        [SerializeField] private string stacktrace;

        [SerializeField] private double startTimeAO;

        [SerializeField] private string status;

        public static TestResultSerializer MakeFromTestResult(ITestResult result)
        {
            var wrapper = new TestResultSerializer();
            wrapper.id = result.Test.Id;
            wrapper.fullName = result.FullName;
            wrapper.status = result.ResultState.Status.ToString();
            wrapper.label = result.ResultState.Label;
            wrapper.site = result.ResultState.Site.ToString();
            wrapper.output = result.Output;
            wrapper.duration = result.Duration;
            wrapper.stacktrace = result.StackTrace;
            wrapper.message = result.Message;
            wrapper.startTimeAO = result.StartTime.ToOADate();
            return wrapper;
        }

        public void RestoreTestResult(TestResult result)
        {
            var resultState = new ResultState((TestStatus)Enum.Parse(typeof(TestStatus), status), label,
                (FailureSite)Enum.Parse(typeof(FailureSite), site));
            result.GetType().BaseType.GetField("_resultState", flags).SetValue(result, resultState);
            result.GetType().BaseType.GetField("_output", flags).SetValue(result, new StringBuilder(output));
            result.GetType().BaseType.GetField("_duration", flags).SetValue(result, duration);
            result.GetType().BaseType.GetField("_message", flags).SetValue(result, message);
            result.GetType().BaseType.GetField("_stackTrace", flags).SetValue(result, stacktrace);
            result.GetType()
                .BaseType.GetProperty("StartTime", flags)
                .SetValue(result, DateTime.FromOADate(startTimeAO), null);
        }

        public bool IsPassed()
        {
            return status == TestStatus.Passed.ToString();
        }
    }
}
