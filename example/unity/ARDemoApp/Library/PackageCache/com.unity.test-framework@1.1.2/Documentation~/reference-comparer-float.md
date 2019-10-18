# FloatEqualityComparer

Use this class to compare two float values for equality with [NUnit](http://www.nunit.org/) constraints. Use `FloatEqualityComparer.Instance` comparer to have the default error value set to 0.0001f. For any other error, use the [one argument constructor](#constructors) to create a comparer.

## Static Properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `Instance` | A singleton instance of the comparer with a default error value set to 0.0001f. |

## Constructors

| Syntax                                      | Description                                                  |
| ------------------------------------------- | ------------------------------------------------------------ |
| `FloatEqualityComparer(float allowedError)` | Creates an instance of the comparer with a custom error value. |

## Public methods

| Syntax                                       | Description                                                  |
| -------------------------------------------- | ------------------------------------------------------------ |
| `bool Equals(float expected, float actual);` | Compares the `actual` and `expected` float values for equality using `Utils.AreFloatsEqual`. |

## Example

```c#
[TestFixture]
public class FloatsTest
{
    [Test]
    public void VerifyThat_TwoFloatsAreEqual()
    {
        var comparer = new FloatEqualityComparer(10e-6f);
        var actual = -0.00009f;
        var expected = 0.00009f;

        Assert.That(actual, Is.EqualTo(expected).Using(comparer));

        // Default relative error 0.0001f
        actual = 10e-8f;
        expected = 0f;

        Assert.That(actual, Is.EqualTo(expected).Using(FloatEqualityComparer.Instance));
    }
}
```

