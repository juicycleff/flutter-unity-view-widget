# Parameterized tests

For data-driven testing, you may want to have your tests parameterized. You may use both the [NUnit](http://www.nunit.org/) attributes [TestCase](https://github.com/nunit/docs/wiki/TestCase-Attribute) and [ValueSource](https://github.com/nunit/docs/wiki/ValueSource-Attribute) with a unit test. 

> **Note**: With `UnityTest` it is recommended to use `ValueSource` since `TestCase` is not supported.  

## Example

```c#
static int[] values = new int[] { 1, 5, 6 };

[UnityTest]
public IEnumerator MyTestWithMultipleValues([ValueSource("values")] int value)
{
    yield return null;
}
```

