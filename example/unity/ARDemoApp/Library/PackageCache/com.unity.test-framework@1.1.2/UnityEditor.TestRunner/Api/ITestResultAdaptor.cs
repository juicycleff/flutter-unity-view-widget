using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace UnityEditor.TestTools.TestRunner.Api
{
    public interface ITestResultAdaptor
    {
        ITestAdaptor Test { get; }
        string Name { get; }

        /// <summary>Gets the full name of the test result</summary>
        string FullName { get; }

        string ResultState { get; }

        TestStatus TestStatus { get; }

        /// <summary>Gets the elapsed time for running the test in seconds</summary>
        double Duration { get; }

        /// <summary>Gets or sets the time the test started running.</summary>
        DateTime StartTime { get; }

        /// <summary>Gets or sets the time the test finished running.</summary>
        DateTime EndTime { get; }

        /// <summary>
        /// Gets the message associated with a test
        /// failure or with not running the test
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets any stacktrace associated with an
        /// error or failure. Not available in
        /// the Compact Framework 1.0.
        /// </summary>
        string StackTrace { get; }

        /// <summary>
        /// Gets the number of asserts executed
        /// when running the test and all its children.
        /// </summary>
        int AssertCount { get; }

        /// <summary>
        /// Gets the number of test cases that failed
        /// when running the test and all its children.
        /// </summary>
        int FailCount { get; }

        /// <summary>
        /// Gets the number of test cases that passed
        /// when running the test and all its children.
        /// </summary>
        int PassCount { get; }

        /// <summary>
        /// Gets the number of test cases that were skipped
        /// when running the test and all its children.
        /// </summary>
        int SkipCount { get; }

        /// <summary>
        /// Gets the number of test cases that were inconclusive
        /// when running the test and all its children.
        /// </summary>
        int InconclusiveCount { get; }

        /// <summary>
        /// Indicates whether this result has any child results.
        /// Accessing HasChildren should not force creation of the
        /// Children collection in classes implementing this interface.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>Gets the the collection of child results.</summary>
        IEnumerable<ITestResultAdaptor> Children { get; }

        /// <summary>Gets any text output written to this result.</summary>
        string Output { get; }

        TNode ToXml();
    }
}
