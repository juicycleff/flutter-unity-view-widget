# Setup and cleanup at build time

In some cases, it is relevant to perform changes to Unity or the file system before building the tests. In the same way, it may be necessary to clean up such changes after the test run. In response to such needs, you can incorporate the pre-build setup and post-build cleanup concepts into your tests in one of the following ways: 

1. Via implementation of `IPrebuildSetup` and `IPostBuildCleanup` interfaces by a test class.
2. Via applying the `PrebuildSetup` attribute and `PostBuildCleanup` attribute on your test class, one of the tests or the test assembly, providing a class name that implements the corresponding interface as an argument (fx `[PrebuildSetup("MyTestSceneSetup")]`). 

## Execution order

All setups run in a deterministic order one after another. The first to run are the setups defined with attributes. Then any test class implementing the interface runs, in alphabetical order inside their namespace, which is the same order as the tests run.

> **Note**: Cleanup runs right away for a standalone test run, but only after related tests run in the Unity Editor.

## PrebuildSetup and PostBuildCleanup

Both `PrebuildSetup` and `PostBuildCleanup` attributes run if the respective test or test class is in the current test run. The test is included either by running all tests or setting a [filter](./workflow-create-test.md#filters) that includes the test. If multiple tests reference the same pre-built setup or post-build cleanup, then it only runs once.

## IPrebuildSetup

Implement this interface if you want to define a set of actions to run as a pre-build step.

### Public methods

| Syntax         | Description                                                  |
| -------------- | ------------------------------------------------------------ |
| `void Setup()` | Implement this method to call actions automatically before the build process. |

## IPostBuildCleanup

Implement this interface if you want to define a set of actions to execute as a post-build step. Cleanup runs right away for a standalone test run, but only after all the tests run within the Editor.

### Public methods

| Syntax           | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| `void Cleanup()` | Implement this method to specify actions that should run as a post-build cleanup step. |

## Example

```c#
[TestFixture]
public class CreateSpriteTest : IPrebuildSetup
{
    Texture2D m_Texture;
    Sprite m_Sprite;
    
    public void Setup()
    {

#if UNITY_EDITOR

        var spritePath = "Assets/Resources/Circle.png";

        var ti = UnityEditor.AssetImporter.GetAtPath(spritePath) as UnityEditor.TextureImporter;

        ti.textureCompression = UnityEditor.TextureImporterCompression.Uncompressed;

        ti.SaveAndReimport();

#endif
    }

    [SetUp]
    public void SetUpTest()
    {
        m_Texture = Resources.Load<Texture2D>("Circle");
    }

    [Test]
    public void WhenNullTextureIsPassed_CreateShouldReturnNullSprite()
    {

        // Check with Valid Texture.

        LogAssert.Expect(LogType.Log, "Circle Sprite Created");

        Sprite.Create(m_Texture, new Rect(0, 0, m_Texture.width, m_Texture.height), new Vector2(0.5f, 0.5f));

        Debug.Log("Circle Sprite Created");

        // Check with NULL Texture. Should return NULL Sprite.

        m_Sprite = Sprite.Create(null, new Rect(0, 0, m_Texture.width, m_Texture.heig`t), new Vector2(0.5f, 0.5f));

        Assert.That(m_Sprite, Is.Null, "Sprite created with null texture should be null");

    }
}
```

> **Tip**: Use `#if UNITY_EDITOR` if you want to access Editor only APIs, but the setup/cleanup is inside a **Play Mode** assembly.
