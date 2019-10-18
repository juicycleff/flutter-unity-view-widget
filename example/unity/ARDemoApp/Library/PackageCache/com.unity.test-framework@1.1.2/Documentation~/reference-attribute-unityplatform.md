# UnityPlatform attribute

Use this attribute to define a specific set of platforms you want or do not want your test(s) to run on.

You can use this attribute on the test method, test class, or test assembly level. Use the supported [RuntimePlatform](https://docs.unity3d.com/ScriptReference/RuntimePlatform.html) enumeration values to specify the platforms. You can also specify which platforms to test by passing one or more `RuntimePlatform` values along with or without the include or exclude properties as parameters to the [Platform](https://github.com/nunit/docs/wiki/Platform-Attribute) attribute constructor. 

The test(s) skips if the current target platform is:

- Not explicitly specified in the included platforms list 
- In the excluded platforms list 

```c#
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

[TestFixture]
public class TestClass
{
    [Test]
    [UnityPlatform(RuntimePlatform.WindowsPlayer)]
    public void TestMethod()
    {
        Assert.AreEqual(Application.platform, RuntimePlatform.WindowsPlayer);
    }
}
```

## Properties

| Syntax                      | Description                                                  |
| --------------------------- | ------------------------------------------------------------ |
| `RuntimePlatform[] exclude` | List the platforms you do not want to have your tests run on. |
| `RuntimePlatform[] include` | A subset of platforms you need to have your tests run on.    |

