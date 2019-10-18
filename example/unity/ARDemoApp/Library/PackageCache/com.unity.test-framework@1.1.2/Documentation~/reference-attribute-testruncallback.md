# TestRunCallback attribute

It is possible for the test framework to invoke callbacks as the current test run progresses. To do this, there is a `TestRunCallback` attribute which takes the type of `ITestRunCallback` implementation. You can invoke the callbacks with [NUnit](http://www.nunit.org/) `ITest` and `ITestResult` classes. 

At the `RunStarted` and `RunFinished` methods, the test and test results are for the whole test tree. These methods invoke at each node in the test tree; first with the whole test assembly, then with the test class, and last with the test method.

From these callbacks, it is possible to read the partial or the full results, and it is furthermore possible to save the XML version of the result for further processing or continuous integration.

## Example

```C#
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.TestRunner;

[assembly:TestRunCallback(typeof(MyTestRunCallback))]

public class MyTestRunCallback : ITestRunCallback
{
    public void RunStarted(ITest testsToRun)
    {
        
    }

    public void RunFinished(ITestResult testResults)
    {
        
    }

    public void TestStarted(ITest test)
    {
        
    }

    public void TestFinished(ITestResult result)
    {
        if (!result.Test.IsSuite)
        {
            Debug.Log($"Result of {result.Name}: {result.ResultState.Status}");
        }
    }
}

```

> **Note:** The `TestRunCallback` does not need any references to the `UnityEditor` namespace and is thus able to run in standalone Players, on the **Player** side.