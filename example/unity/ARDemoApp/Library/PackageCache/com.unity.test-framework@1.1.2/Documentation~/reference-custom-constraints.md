# Custom constraints 

`NUnit` allows you to write test assertions in a more descriptive and human readable way using the [Assert.That](https://github.com/nunit/docs/wiki/Assertions) mechanism, where the first parameter is an object under test and the second parameter describes conditions that the object has to meet. 

## Is

We’ve extended `NUnit` API with a custom constraint type and declared an overlay `Is` class. To resolve ambiguity between the original implementation and the custom one you must explicitly declare it with a using statement or via addressing through the full type name `UnityEngine.TestTools.Constraints.Is`.

### Static Methods

| Syntax               | Description                                                  |
| -------------------- | ------------------------------------------------------------ |
| `AllocatingGCMemory` | A constraint type that invokes the delegate you provide as the parameter of `Assert.That` and checks whether it causes any GC memory allocations. It passes if any GC memory is allocated and fails if not. |

## Example

```c#
using Is = UnityEngine.TestTools.Constraints.Is;

class MyTestClass
{
    [Test]
    public void MyTest()
    {
        Assert.That(() => {
            var i = new int[500];
        }, Is.AllocatingGCMemory());
    }
}
```

