# Actions outside of tests

When writing tests, it is possible to avoid duplication of code by using the [SetUp and TearDown](https://github.com/nunit/docs/wiki/SetUp-and-TearDown) methods built into [NUnit](http://www.nunit.org/). The Unity Test Framework has extended these methods with extra functionality, which can yield commands and skip frames, in the same way as [UnityTest](./reference-unitytest.md).

## Action execution order

The actions related to a test run in the following order:

* Attributes implementing [IApplyToContext](https://github.com/nunit/docs/wiki/IApplyToContext-Interface) 
* Any attribute implementing [OuterUnityTestAction](#outerunitytestaction) has its `BeforeTest` invoked
* Tests with [UnitySetUp](#unitysetup-and-unityteardown) methods in their test class.
* Attributes implementing [IWrapSetUpTearDown](https://github.com/nunit/docs/wiki/ICommandWrapper-Interface) 
* Any [SetUp](https://github.com/nunit/docs/wiki/SetUp-and-TearDown) attributes 
* [Action attributes](https://nunit.org/docs/2.6/actionAttributes.html) have their `BeforeTest` method invoked 
* Attributes implementing of [IWrapTestMethod](https://github.com/nunit/docs/wiki/ICommandWrapper-Interface)  
* **The test itself runs**
* [Action attributes](https://nunit.org/docs/2.6/actionAttributes.html) have their `AfterTest` method invoked
* Any method with the [TearDown](https://github.com/nunit/docs/wiki/SetUp-and-TearDown) attribute
* Tests with [UnityTearDown](#unitysetup-and-unityteardown) methods in their test class
* Any [OuterUnityTestAction](#outerunitytestaction) has its `AfterTest` invoked

The list of actions is the same for both `Test` and `UnityTest`.

## UnitySetUp and UnityTearDown

The `UnitySetUp` and `UnityTearDown` attributes are identical to the standard `SetUp` and `TearDown` attributes, with the exception that they allow for [yielding instructions](reference-custom-yield-instructions.md). The `UnitySetUp` and `UnityTearDown` attributes expect a return type of [IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator?view=netframework-4.8). 

### Example

```c#
public class SetUpTearDownExample
{
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return new EnterPlayMode();
    }

    [Test]
    public void MyTest()
    {
        Debug.Log("This runs inside playmode");
    }

    [UnitySetUp]
    public IEnumerator TearDown()
    {

        yield return new ExitPlayMode();
    }
}
```



## OuterUnityTestAction

`OuterUnityTestAction` is a wrapper outside of the tests, which allows for any tests with this attribute to run code before and after the tests. This method allows for yielding commands in the same way as `UnityTest`. The attribute must inherit the `NUnit` attribute and implement `IOuterUnityTestAction`. 

### Example

```c#
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;

public class MyTestClass
{
    [UnityTest, MyOuterActionAttribute]
    public IEnumerator MyTestInsidePlaymode()
    {
        Assert.IsTrue(Application.isPlaying);
        yield return null;
    }
}

public class MyOuterActionAttribute : NUnitAttribute, IOuterUnityTestAction
{
    public IEnumerator BeforeTest(ITest test)
    {
        yield return new EnterPlayMode();
    }

    public IEnumerator AfterTest(ITest test)
    {
        yield return new ExitPlayMode();
    }
}

```



## Domain Reloads

In **Edit Mode** tests it is possible to yield instructions that can result in a domain reload, such as entering or exiting **Play Mode** (see [Custom yield instructions](./reference-custom-yield-instructions.md)). When a domain reload happens, all non-Unity actions (such as `OneTimeSetup` and `Setup`) are rerun before the code, which initiated the domain reload, continues. Unity actions (such as `UnitySetup`) are not rerun. If the Unity action is the code that initiated the domain reload, then the rest of the code in the `UnitySetup` method runs after the domain reload.