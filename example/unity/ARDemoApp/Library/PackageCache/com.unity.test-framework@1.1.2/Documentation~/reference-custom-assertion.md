# Custom assertion

A test fails if Unity logs a message other than a regular log or warning message. Use [LogAssert](#logassert) to check for an expected message in the log so that the test does not fail when Unity logs the message.

Use `LogAssert.Expect` before running the code under test, as the check for expected logs runs at the end of each frame.

A test also reports a failure, if an expected message does not appear, or if Unity does not log any regular log or warning messages.

## Example

```c#
[Test]
public void LogAssertExample()
{
    // Expect a regular log message
    LogAssert.Expect(LogType.Log, "Log message");
    
    // The test fails without the following expected log message     
    Debug.Log("Log message");
    
    // An error log
    Debug.LogError("Error message");
    
    // Without expecting an error log, the test would fail
    LogAssert.Expect(LogType.Error, "Error message");
}
```

## LogAssert

`LogAssert` lets you expect Unity log messages that would otherwise cause the test to fail.

### Static properties

| Syntax                       | Description                                                  |
| ---------------------------- | ------------------------------------------------------------ |
| `bool ignoreFailingMessages` | Set this property to `true` to prevent unexpected error log messages from triggering an assertion. By default, it is `false`. |

### Static Methods

| Syntax                                                       | Description                                                  |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| `void Expect(LogType type, string message);` `void Expect(LogType type, Regex message);` | Verifies that a log message of a specified type appears in the log. A test won’t fail from an expected error, assertion, or exception log message. It does fail if an expected message does not appear in the log. |
| `void NoUnexpectedReceived();`                               | Triggers an assertion when receiving any log messages and fails the test if some are unexpected messages. If multiple tests need to check for no received unexpected logs, consider using the [TestMustExpectAllLogs](./reference-attribute-testmustexpectalllogs.md) attribute instead. |

### Expect string message

`void Expect(LogType type, string message);`

#### Parameters

| Syntax           | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| `LogType type`   | A type of log to expect. It can take one of the [LogType enum](https://docs.unity3d.com/ScriptReference/LogType.html) values. |
| `string message` | A string value that should equate to the expected message.   |

### Expect Regex message

`void Expect(LogType type, Regex message);`

#### Parameters

| Syntax          | Description                                                  |
| --------------- | ------------------------------------------------------------ |
| `LogType type`  | A type of log to expect. It can take one of the [LogType enum](https://docs.unity3d.com/ScriptReference/LogType.html) values. |
| `Regex message` | A regular expression pattern to match the expected message.  |