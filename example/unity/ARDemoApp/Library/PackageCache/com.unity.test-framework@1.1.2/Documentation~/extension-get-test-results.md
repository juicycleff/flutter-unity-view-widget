# How to get test results
You can receive callbacks when the active test run, or individual tests, starts and finishes. You can register callbacks by invoking `RegisterCallbacks` on the [TestRunnerApi](./reference-test-runner-api.md) with an instance of a class that implements [ICallbacks](./reference-icallbacks.md). There are four `ICallbacks` methods for the start and finish of both the whole run and each level of the test tree. 

## Example
An example of how listeners can be set up: 

> **Note**: Listeners receive callbacks from all test runs, regardless of the registered `TestRunnerApi` for that instance.

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
       
    }

    public void TestStarted(ITestAdaptor test)
    {
       
    }

    public void TestFinished(ITestResultAdaptor result)
    {
        if (!result.HasChildren && result.ResultState != "Success")
        {
            Debug.Log(string.Format("Test {0} {1}", result.Test.Name, result.ResultState));
        }
    }
}
```

> **Note**: The registered callbacks are not persisted on domain reloads. So it is necessary to re-register the callback after a domain reloads, usually with [InitializeOnLoad](https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html).

It is possible to provide a `priority` as an integer as the second argument when registering a callback. This influences the invocation order of different callbacks. The default value is zero. It is also possible to provide `RegisterCallbacks` with a class instance that implements [IErrorCallbacks](./reference-ierror-callbacks.md) that is an extended version of `ICallbacks`. `IErrorCallbacks` also has a callback method for `OnError` that invokes if the run fails to start, for example, due to compilation errors or if an [IPrebuildSetup](./reference-setup-and-cleanup.md) throws an exception. 