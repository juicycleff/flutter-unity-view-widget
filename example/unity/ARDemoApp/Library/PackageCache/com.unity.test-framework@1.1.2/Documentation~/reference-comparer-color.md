# ColorEqualityComparer

Use this class to compare two `Color` objects. `ColorEqualityComparer.Instance` has default calculation error value set to 0.01f. To set a test specific error value instantiate a comparer instance using the [one argument constructor](#constructors).

## Static properties

| Syntax     | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| `Instance` | A singleton instance of the comparer with a default error value set to 0.01f. |

## Constructors

| Syntax                               | Description                                                  |
| ------------------------------------ | ------------------------------------------------------------ |
| `ColorEqualityComparer(float error)` | Creates an instance of the comparer with a custom error value. |

## Public methods

| Syntax                                       | Description                                                  |
| -------------------------------------------- | ------------------------------------------------------------ |
| `bool Equals(Color expected, Color actual);` | Compares the actual and expected `Color` objects for equality using  `Utils.AreFloatsEqualAbsoluteError` to compare the `RGB` and `Alpha` attributes of `Color`. Returns `true` if expected and actual objects are equal otherwise, it returns `false`. |

## Example

```c#
[TestFixture]
public class ColorEqualityTest
{
    [Test]
    public void GivenColorsAreEqual_WithAllowedCalculationError()
    {
        // Using default error
        var firstColor = new Color(0f, 0f, 0f, 0f);
        var secondColor = new Color(0f, 0f, 0f, 0f);

        Assert.That(firstColor, Is.EqualTo(secondColor).Using(ColorEqualityComparer.Instance));
		
        // Allowed error 10e-5f
        var comparer = new ColorEqualityComparer(10e-5f);
        firstColor = new Color(0f, 0f, 0f, 1f);
        secondColor = new Color(10e-6f, 0f, 0f, 1f);

        Assert.That(firstColor, Is.EqualTo(secondColor).Using(comparer));
    }
}
```

