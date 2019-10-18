# ConditionalIgnore attribute

This attribute is an alternative to the standard `Ignore` attribute in [NUnit](http://www.nunit.org/). It allows for ignoring tests only under a specified condition. The condition evaluates during `OnLoad`, referenced by ID. 

## Example

The following example shows a method to use the `ConditionalIgnore` attribute to ignore a test if the Unity Editor is running macOS:

```C#
using UnityEditor;
using NUnit.Framework;
using UnityEngine.TestTools;

[InitializeOnLoad]
public class OnLoad
{
    static OnLoad()
    {
        var editorIsOSX = false;
        #if UNITY_EDITOR_OSX
            editorIsOSX = true;
        #endif
        
        ConditionalIgnoreAttribute.AddConditionalIgnoreMapping("IgnoreInMacEditor", editorIsOSX);
    }
}

public class MyTestClass
{
    [Test, ConditionalIgnore("IgnoreInMacEditor", "Ignored on Mac editor.")]
    public void TestNeverRunningInMacEditor()
    {
        Assert.Pass();
    }
}

```

> **Note**: You can only use `InitializeOnLoad` in **Edit Mode** tests.