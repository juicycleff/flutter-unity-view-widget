# IErrorCallbacks
An extended version of the [ICallbacks](./reference-icallbacks.md), which get invoked if the test run fails due to a build error or if any [IPrebuildSetup](./reference-setup-and-cleanup.md) has a failure.

## Public methods

| Syntax                       | Description                                                         |
| ---------------------------- | ------------------------------------------------------------------- |
| void OnError(string message) | The error message detailing the reason for the run to fail.         |

