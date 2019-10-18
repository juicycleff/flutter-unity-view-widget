# How to run tests programmatically
## Filters

Run tests by calling `Execute` on the [TestRunnerApi](./reference-test-runner-api.md), and provide some execution settings that consists of a [Filter](./reference-filter.md). The `Filter` specifies what tests to run. 

### Example

The following is an example of how to run all **Play Mode** tests in a project:

``` C#
var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
var filter = new Filter()
{
    testMode = TestMode.PlayMode
};
testRunnerApi.Execute(new ExecutionSettings(filter));
```
## Multiple filter values

It is possible to specify a more specific filter by filling out the fields on the `Filter` class in more detail.

Many of the fields allow for multiple values. The runner tries to match tests against at least one of the values provided and then runs any tests that match. 

### Example

In this example, the API runs tests with full names that fit either of the two names provided:

``` C#
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.Execute(new ExecutionSettings(new Filter()
{
    testNames = new[] {"MyTestClass.NameOfMyTest", "SpecificTestFixture.NameOfAnotherTest"}
}));
```
## Multiple filter fields

If using multiple different fields on the filter, then it matches against tests that fulfill all the different fields.

### Example

In this example, it runs any test that fits either of the two test names, and that also belongs to a test assembly that fits the given name.

``` C#
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.Execute(new ExecutionSettings(new Filter()
{
    assemblyNames = new [] {"MyTestAssembly"},
    testNames = new [] {"MyTestClass.NameOfMyTest", "MyTestClass.AnotherNameOfATest"}
}));
```
## Multiple constructor filters

The execution settings take one or more filters in its constructor. If there is no filter provided, then it runs all **Edit Mode** tests by default. If there are multiple filters provided, then a test runs if it matches any of the filters.

### Example

In this example, it runs any tests that are either in the assembly named `MyTestAssembly` or if the full name of the test matches either of the two provided test names:

``` C#
var api = ScriptableObject.CreateInstance<TestRunnerApi>();
api.Execute(new ExecutionSettings(
    new Filter()
    {
        assemblyNames = new[] {"MyTestAssembly"},
    },
    new Filter()
    {
        testNames = new[] {"MyTestClass.NameOfMyTest", "MyTestClass.AnotherNameOfATest"}
    }
));
```
> **Note**: Specifying different test modes or platforms in each `Filter` is not currently supported. The test mode and platform is from the first `Filter` only and defaults to Edit Mode, if not supplied. 