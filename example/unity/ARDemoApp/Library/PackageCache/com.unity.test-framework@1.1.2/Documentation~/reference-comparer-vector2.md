# Vector2EqualityComparer

Use this class to compare two [Vector2](https://docs.unity3d.com/ScriptReference/Vector2.html) objects for equality with [NUnit](http://www.nunit.org/) constraints. Use the static `Vector2EqualityComparer.Instance` to have the calculation error value set to default 0.0001f. For any other error value, instantiate a new comparer object with the [one argument constructor](#constructors).

## Static properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `Instance` | A comparer instance with the default error value set to 0.0001f. |

## Constructors

| Syntax                                 | Description                                    |
| -------------------------------------- | ---------------------------------------------- |
| `Vector2EqualityComparer(float error)` | Creates an instance with a custom error value. |

## Public methods

| Syntax                                     | Description                                                  |
| ------------------------------------------ | ------------------------------------------------------------ |
| `Equals(Vector2 expected, Vector2 actual)` | Compares the `actual` and `expected` `Vector2` objects for equality using the [Utils.AreFloatsEqual](./reference-test-utils.md) method. |

## Example

```c#
[TestFixture]
public class Vector2Test
{
    [Test]
    public void VerifyThat_TwoVector2ObjectsAreEqual()
    {
        // Custom calculation error
        var actual = new Vector2(10e-7f, 10e-7f);
        var expected = new Vector2(0f, 0f);
        var comparer = new Vector2EqualityComparer(10e-6f);

        Assert.That(actual, Is.EqualTo(expected).Using(comparer));

        //Default error 0.0001f
        actual = new Vector2(0.01f, 0.01f);
        expected = new Vector2(0.01f, 0.01f);

        Assert.That(actual, Is.EqualTo(expected).Using(Vector2EqualityComparer.Instance));
    }
}
```

