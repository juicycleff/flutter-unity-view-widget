# TestRunnerApi
The `TestRunnerApi` retrieves and runs tests programmatically from code inside the project, or inside other packages. `TestRunnerApi` is a [ScriptableObject](https://docs.unity3d.com/ScriptReference/ScriptableObject.html). 

You can initialize the API like this:

```c#
var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
```
> **Note**: You can subscribe and receive test results in one instance of the API, even if the run starts from another instance.

The `TestRunnerApi` supports the following workflows:
* [How to run tests programmatically](./extension-run-tests.md)
* [How to get test results](./extension-get-test-results.md)
* [How to retrieve the list of tests](./extension-retrieve-test-list.md)

## Public methods

| Syntax                                     | Description                                                  |
| ------------------------------------------ | ------------------------------------------------------------ |
| `void Execute(ExecutionSettings executionSettings)` | Starts a test run with a given set of [ExecutionSettings](./reference-execution-settings.md). |
| `void RegisterCallbacks(ICallbacks testCallbacks, int priority = 0)` | Sets up a given instance of [ICallbacks](./reference-icallbacks.md) to be invoked on test runs. |
| `void UnregisterCallbacks(ICallbacks testCallbacks)` | Unregisters an instance of ICallbacks to no longer receive callbacks from test runs. |
| `void RetrieveTestList(TestMode testMode, Action<ITestAdaptor> callback)` | Retrieve the full test tree as [ITestAdaptor](./reference-itest-adaptor.md) for a given test mode. |