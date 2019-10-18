# ITestAdaptor
`ITestAdaptor` is a representation of a node in the test tree implemented as a wrapper around the [NUnit](http://www.nunit.org/) [ITest](https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Interfaces/ITest.cs) interface.

## Properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `string Id`               | The ID of the test tree node. The ID can change if you add new tests to the suite. Use `UniqueName`, if you want to have a more permanent point of reference. |
| `string Name`             | The name of the test. E.g., `MyTest`. |
| `string FullName`         | The full name of the test. E.g., `MyNamespace.MyTestClass.MyTest`. |
| `int TestCaseCount`       | The total number of test cases in the node and all sub-nodes. |
| `bool HasChildren`        | Whether the node has any children. |
| `bool IsSuite`            | Whether the node is a test suite/fixture. |
| `IEnumerable<ITestAdaptor> Children` | The child nodes. |
| `ITestAdaptor Parent`     | The parent node, if any. |
| `int TestCaseTimeout`     | The test case timeout in milliseconds. Note that this value is only available on TestFinished. |
| `ITypeInfo TypeInfo`      | The type of test class as an `NUnit` [ITypeInfo](https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Interfaces/ITypeInfo.cs). If the node is not a test class, then the value is `null`. |
| `IMethodInfo Method`      | The [Nunit IMethodInfo](https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Interfaces/IMethodInfo.cs) of the test method. If the node is not a test method, then the value is `null`. |
| `string[] Categories`     | An array of the categories applied to the test or fixture. |
| `bool IsTestAssembly`     | Whether the node represents a test assembly. |
| `RunState RunState`       | The run state of the test node. Either `NotRunnable`, `Runnable`, `Explicit`, `Skipped`, or `Ignored`. |
| `string Description`      | The description of the test. |
| `string SkipReason`       | The skip reason. E.g., if ignoring the test. |
| `string ParentId`         | The ID of the parent node. |
| `string ParentFullName`   | The full name of the parent node. |
| `string UniqueName`       | A unique generated name for the test node. E.g., `Tests.dll/MyNamespace/MyTestClass/[Tests][MyNamespace.MyTestClass.MyTest]`. |
| `string ParentUniqueName` | A unique name of the parent node. E.g., `Tests.dll/MyNamespace/[Tests][MyNamespace.MyTestClass][suite]`. |
| `int ChildIndex`          | The child index of the node in its parent. |
| `TestMode TestMode`       | The mode of the test. Either **Edit Mode** or **Play Mode**. |

> **Note**: Some properties are not available when receiving the test tree as a part of a test result coming from a standalone Player, such as `TypeInfo` and `Method`.