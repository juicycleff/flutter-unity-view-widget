# QuaternionEqualityComparer

Use this utility to compare two [Quaternion](https://docs.unity3d.com/ScriptReference/Quaternion.html) objects for equality with [NUnit](http://www.nunit.org/) assertion constraints. Use the static instance `QuaternionEqualityComparer.Instance` to have the default calculation error value set to 0.00001f. For any other custom error value, use the [one argument constructor](#constructors).

## Static properties

| Syntax     | Description                                                |
| ---------- | ---------------------------------------------------------- |
| `Instance` | A comparer instance with the default error value 0.00001f. |

## Constructors

| Syntax                                           | Description                                                  |
| ------------------------------------------------ | ------------------------------------------------------------ |
| `QuaternionEqualityComparer(float allowedError)` | Creates an instance of the comparer with a custom allowed error value. |

## Public methods

| Syntax                                                | Description                                                  |
| ----------------------------------------------------- | ------------------------------------------------------------ |
| `bool Equals(Quaternion expected, Quaternion actual)` | Compares the `actual` and `expected` `Quaternion` objects for equality using the [Quaternion.Dot](https://docs.unity3d.com/ScriptReference/Quaternion.Dot.html) method. |

## Example

```c#
[TestFixture]
public class QuaternionTest
{
    [Test]
    public void VerifyThat_TwoQuaternionsAreEqual()
    {
        var actual = new Quaternion(10f, 0f, 0f, 0f);
        var expected = new Quaternion(1f, 10f, 0f, 0f);
        var comparer = new QuaternionEqualityComparer(10e-6f);

        Assert.That(actual, Is.EqualTo(expected).Using(comparer));

        //Using default error 0.00001f
        actual = new Quaternion(10f, 0f, 0.1f, 0f);
        expected = new Quaternion(1f, 10f, 0.1f, 0f);

        Assert.That(actual, Is.EqualTo(expected).Using(QuaternionEqualityComparer.Instance));
    }
}
```

