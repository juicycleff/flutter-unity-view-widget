# ITestResultAdaptor
The `ITestResultAdaptor` is the representation of the test results for a node in the test tree implemented as a wrapper around the [NUnit](http://www.nunit.org/) [ITest](https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Interfaces/ITestResults.cs) interface.
## Properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `ITestAdaptor Test` | The test details of the test result tree node as a [TestAdaptor](./reference-itest-adaptor.md). |
| `string Name` | The name of the test node. |
| `string FullName` | Gets the full name of the test result |
| `string ResultState` | The state of the result as a string. E.g., `Success`, `Skipped`, `Failure`, `Explicit`, `Cancelled`. |
| `TestStatus TestStatus` | The status of the test as an enum. Either `Inconclusive`, `Skipped`, `Passed`, or `Failed`. |
| `double Duration` | Gets the elapsed time for running the test in seconds. |
| `DateTime StartTime` | Gets or sets the time the test started running. |
| `DateTime EndTime` | Gets or sets the time the test finished running. |
| `string Message` | Gets the message associated with a test failure or with not running the test |
| `string StackTrace` | Gets any stack trace associated with an error or failure. Not available in the [Compact Framework](https://en.wikipedia.org/wiki/.NET_Compact_Framework) 1.0. |
| `int AssertCount` | Gets the number of asserts that ran during the test and all its children. |
| `int FailCount` | Gets the number of test cases that failed when running the test and all its children. |
| `int PassCount` | Gets the number of test cases that passed when running the test and all its children. |
| `int SkipCount` | Gets the number of test cases skipped when running the test and all its children. |
| `int InconclusiveCount` | Gets the number of test cases that were inconclusive when running the test and all its children. |
| `bool HasChildren` | Indicates whether this result has any child results. Accessing HasChildren should not force the creation of the Children collection in classes implementing this interface. |
| `IEnumerable<ITestResultAdaptor> Children` | Gets the collection of child results. |
| `string Output` | Gets any text output written to this result. |
| `TNode ToXml` | Gets the test results as an `NUnit` XML node. Use this to save the results to an XML file. |
