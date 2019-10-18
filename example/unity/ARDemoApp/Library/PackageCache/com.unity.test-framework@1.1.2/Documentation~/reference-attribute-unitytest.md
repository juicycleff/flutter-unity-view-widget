# UnityTest attribute

`UnityTest` attribute is the main addition to the standard [NUnit](http://www.nunit.org/) library for the Unity Test Framework. This type of unit test allows you to skip a frame from within a test (so background tasks can finish) or give certain commands to the Unity **Editor**, such as performing a domain reload or entering **Play Mode** from an **Edit Mode** test.

In Play Mode, the `UnityTest` attribute runs as a [coroutine](https://docs.unity3d.com/Manual/Coroutines.html). Whereas Edit Mode tests run in the [EditorApplication.update](https://docs.unity3d.com/ScriptReference/EditorApplication-update.html) callback loop.

The `UnityTest` attribute is, in fact, an alternative to the `NUnit` [Test attribute](https://github.com/nunit/docs/wiki/Test-Attribute), which allows yielding instructions back to the framework. Once the instruction is complete, the test run continues. If you `yield return null`, you skip a frame. That might be necessary to ensure that some changes do happen on the next iteration of either the `EditorApplication.update` loop or the [game loop](https://docs.unity3d.com/Manual/ExecutionOrder.html).

## Edit Mode example

The most simple example of an Edit Mode test could be the one that yields `null` to skip the current frame and then continues to run:

```C#
[UnityTest]
public IEnumerator EditorUtility_WhenExecuted_ReturnsSuccess()
{
    var utility = RunEditorUtilityInTheBackgroud();

    while (utility.isRunning)
    {
        yield return null;
    }

    Assert.IsTrue(utility.isSuccess);
}    
```

## Play Mode example

In Play Mode, a test runs as a coroutine attached to a [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html). So all the yield instructions available in coroutines, are also available in your test. 

From a Play Mode test you can use one of Unity’s [Yield Instructions](https://docs.unity3d.com/ScriptReference/YieldInstruction.html):

- [WaitForFixedUpdate](https://docs.unity3d.com/ScriptReference/WaitForFixedUpdate.html): to ensure changes expected within the next cycle of physics calculations.
- [WaitForSeconds](https://docs.unity3d.com/ScriptReference/WaitForSeconds.html): if you want to pause your test coroutine for a fixed amount of time. Be careful about creating long-running tests.

The simplest example is to yield to `WaitForFixedUpdate`:

```c#
[UnityTest]
public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics()
{
    var go = new GameObject();
    go.AddComponent<Rigidbody>();
    var originalPosition = go.transform.position.y;

    yield return new WaitForFixedUpdate();

    Assert.AreNotEqual(originalPosition, go.transform.position.y);
}
```
