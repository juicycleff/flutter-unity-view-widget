# Custom yield instructions

By implementing this interface below, you can define custom yield instructions in **Edit Mode** tests.

## IEditModeTestYieldInstruction

In an Edit Mode test, you can use `IEditModeTestYieldInstruction` interface to implement your own instruction. There are also a couple of commonly used implementations available:

- [EnterPlayMode](#enterplaymode)
- [ExitPlayMode](#exitplaymode)
- [RecompileScripts](./reference-recompile-scripts.md)
- [WaitForDomainReload](./reference-wait-for-domain-reload.md)

## Example

```c#
[UnityTest]

public IEnumerator PlayOnAwakeDisabled_DoesntPlayWhenEnteringPlayMode()

{
    var videoPlayer = PrefabUtility.InstantiatePrefab(m_VideoPlayerPrefab.GetComponent<VideoPlayer>()) as VideoPlayer;

    videoPlayer.playOnAwake = false;

    yield return new EnterPlayMode();

    var videoPlayerGO = GameObject.Find(m_VideoPlayerPrefab.name);

    Assert.IsFalse(videoPlayerGO.GetComponent<VideoPlayer>().isPlaying);

    yield return new ExitPlayMode();

    Object.DestroyImmediate(GameObject.Find(m_VideoPlayerPrefab.name));
}
```

## Properties

| Syntax                       | Description                                                  |
| ---------------------------- | ------------------------------------------------------------ |
| `bool ExpectDomainReload`    | Returns `true` if the instruction expects a domain reload to occur. |
| `bool ExpectedPlaymodeState` | Returns `true` if the instruction expects the Unity Editor to be in **Play Mode**. |

## Methods

| Syntax                  | Description                                                  |
| ----------------------- | ------------------------------------------------------------ |
| `IEnumerator Perform()` | Used to define multi-frame operations performed when instantiating a yield instruction. |

## EnterPlayMode

* Implements `IEditModeTestYieldInstruction`. Creates a yield instruction to enter Play Mode.
* When creating an Editor test that uses the `UnityTest` attribute, use this to trigger the Editor to enter Play Mode. 
* Throws an exception if the Editor is already in Play Mode or if there is a [script compilation error](https://support.unity3d.com/hc/en-us/articles/205930539-How-do-I-interpret-a-compiler-error-).

## ExitPlayMode

* Implements `IEditModeTestYieldInstruction`. A new instance of the class is a yield instruction to exit Play Mode.
* Throws an exception if the Editor is not in Play Mode.
