# Vector3EqualityComparer

Use this class to compare two [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) objects for equality with `NUnit` constraints. Call `Vector3EqualityComparer.Instance` comparer to perform a comparison with the default calculation error value 0.0001f. To specify a different error value, use the [one argument constructor](#constructors) to instantiate a new comparer.

## Static properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `Instance` | A comparer instance with the default calculation error value equal to 0.0001f. |

## Constructors

| Syntax                                        | Description                                    |
| --------------------------------------------- | ---------------------------------------------- |
| `Vector3EqualityComparer(float allowedError)` | Creates an instance with a custom error value. |

## Public methods

| Syntax                                          | Description                                                  |
| ----------------------------------------------- | ------------------------------------------------------------ |
| `bool Equals(Vector3 expected, Vector3 actual)` | Compares the `actual` and `expected` `Vector3` objects for equality using [Utils.AreFloatsEqual](http://todo) to compare the `x`, `y`, and `z` attributes of `Vector3`. |

## Example

```c#
[TestFixture]
public class Vector3Test
{
    [Test]
    public void VerifyThat_TwoVector3ObjectsAreEqual()
    {
        // Custom error 10e-6f
        var actual = new Vector3(10e-8f, 10e-8f, 10e-8f);
        var expected = new Vector3(0f, 0f, 0f);
        var comparer = new Vector3EqualityComparer(10e-6f);

        Assert.That(actual, Is.EqualTo(expected).Using(comparer));

        //Default error 0.0001f
        actual = new Vector3(0.01f, 0.01f, 0f);
        expected = new Vector3(0.01f, 0.01f, 0f);

        Assert.That(actual, Is.EqualTo(expected).Using(Vector3EqualityComparer.Instance));
    }
}
```

