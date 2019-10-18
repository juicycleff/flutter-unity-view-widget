# ICallbacks
An interface for receiving callbacks when running tests. All test runs invoke the callbacks until the next domain reload.

The `RunStarted` method runs when the whole test run starts. Then the `TestStarted` method runs with information about the tests it is about to run on an assembly level. Afterward, it runs on a test fixture level and then on the individual test. If the test is a [parameterized test](./https://github.com/nunit/docs/wiki/Parameterized-Tests), then it is also invoked for each parameter combination. After each part of the test tree have completed running, the corresponding `TestFinished` method runs with the test result. At the end of the run, the `RunFinished` event runs with the test result.

An extended version of the callback, [IErrorCallbacks](./reference-ierror-callbacks.md), extends this `ICallbacks` to receive calls when a run fails due to a build error.

## Public methods

| Syntax                                         | Description                                                  |
| ---------------------------------------------- | ------------------------------------------------------------ |
| `void RunStarted(ITestAdaptor testsToRun)`     | Invoked when the test run starts. The [ITestAdaptor](./reference-itest-adaptor.md) represents the tree of tests to run. |
| `void RunFinished(ITestResultAdaptor result)`  | Invoked when the test run finishes. The [ITestResultAdaptor](./reference-itest-result-adaptor.md) represents the results of the set of tests that have run. |
| `void TestStarted(ITestAdaptor test)`          | Invoked on each node of the test tree, as that part of the tree starts to run. |
| `void TestFinished(ITestResultAdaptor result)` | Invoked on each node of the test tree once that part of the test tree has finished running. The [ITestResultAdaptor](./reference-itest-result-adaptor.md) represents the results of the current node of the test tree. |

## Example
An example that sets up a listener on the API. The listener prints the number of failed tests after the run has finished:
``` C#
public void SetupListeners()
{
    var api = ScriptableObject.CreateInstance<TestRunnerApi>();
    api.RegisterCallbacks(new MyCallbacks());
}

private class MyCallbacks : ICallbacks
{
    public void RunStarted(ITestAdaptor testsToRun)
    {
  
    }

    public void RunFinished(ITestResultAdaptor result)
    {
        Debug.Log(string.Format("Run finished {0} test(s) failed.", result.FailCount));
    }

    public void TestStarted(ITestAdaptor test)
    {
  
    }

    public void TestFinished(ITestResultAdaptor result)
    {
  
    }
}
```