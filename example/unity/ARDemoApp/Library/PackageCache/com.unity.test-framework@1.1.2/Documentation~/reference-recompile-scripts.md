# RecompileScripts
`RecompileScripts` is an [IEditModeTestYieldInstruction](./reference-custom-yield-instructions.md) that you can yield in Edit Mode tests. It lets you trigger a recompilation of scripts in the Unity Editor.

## Constructors

| Syntax                                                       | Description                                                  |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| `RecompileScripts(bool expectScriptCompilation = true, bool expectScriptCompilationSuccess = true)` | Creates a new instance of the `RecompileScripts` yield instruction. The parameter `expectScriptCompilation` indicates if you expect a script compilation to start (defaults to true). If a script compilation does not start and `expectScriptCompilation` is `true`, then it throws an exception. |

## Example
``` C@
[UnitySetUp]
public IEnumerator SetUp()
{
    using (var file = File.CreateText("Assets/temp/myScript.cs"))
    {
        file.Write("public class ATempClass {  }");
    }
    AssetDatabase.Refresh();
    yield return new RecompileScripts();
}
```