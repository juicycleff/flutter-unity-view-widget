# ExecutionSettings
The `ExecutionSettings` is a set of filters and other settings provided when running a set of tests from the [TestRunnerApi](./reference-test-runner-api.md).

## Constructors

| Syntax                                                | Description                                              |
| ----------------------------------------------------- | -------------------------------------------------------- |
| `ExecutionSettings(params Filter[] filtersToExecute)` | Creates an instance with a given set of filters, if any. |

## Fields

| Syntax                       | Description                                                  |
| ---------------------------- | ------------------------------------------------------------ |
| `Filter[] filters`          | A collection of [Filters](./reference-filter.md) to execute tests on. |
| `ITestRunSettings overloadTestRunSettings` | An instance of [ITestRunSettings](./reference-itest-run-settings.md) to set up before running tests on a Player. |

