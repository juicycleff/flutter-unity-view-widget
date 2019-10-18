# Test Utils

This contains test utility functions for float value comparison and creating primitives.

## Static Methods

| Syntax                                                       | Description                                                  |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| `bool AreFloatsEqual(float expected, float actual, float allowedRelativeError)` | Relative epsilon comparison of two float values for equality. `allowedRelativeError` is the relative error to be used in relative epsilon comparison. The relative error is the absolute error divided by the magnitude of the exact value. Returns `true` if the actual value is equivalent to the expected value. |
| `bool AreFloatsEqualAbsoluteError(float expected, float actual, float allowedAbsoluteError)` | Compares two floating point numbers for equality under the given absolute tolerance. `allowedAbsoluteError` is the permitted error tolerance. Returns `true` if the actual value is equivalent to the expected value under the given tolerance. |
| `GameObject CreatePrimitive( type)`                          | Creates a [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) with a primitive [MeshRenderer](https://docs.unity3d.com/ScriptReference/MeshRenderer.html). This is an analogue to the [GameObject.CreatePrimitive](https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html), but creates a primitive `MeshRenderer` with a fast [Shader](https://docs.unity3d.com/ScriptReference/Shader.html) instead of the default built-in `Shader`, optimized for testing performance.  `type` is the [primitive type](https://docs.unity3d.com/ScriptReference/PrimitiveType.html) of the required `GameObject`. Returns a `GameObject` with primitive `MeshRenderer` and [Collider](https://docs.unity3d.com/ScriptReference/Collider.html). |

## Example

```c#
[TestFixture]
class UtilsTests
{
    [Test]
    public void ChechThat_FloatsAreEqual()
    {
        float expected = 10e-8f;
        float actual = 0f;
        float allowedRelativeError = 10e-6f;

        Assert.That(Utils.AreFloatsEqual(expected, actual, allowedRelativeError), Is.True);
    }
	
    [Test]
    public void ChechThat_FloatsAreAbsoluteEqual()
    {
        float expected = 0f;
        float actual = 10e-6f;
        float error = 10e-5f;

        Assert.That(Utils.AreFloatsEqualAbsoluteError(expected, actual, error), Is.True);
    }
}
```

