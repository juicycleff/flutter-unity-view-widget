# ITestRunSettings
`ITestRunSettings` lets you set any of the global settings right before building a Player for a test run and then reverts the settings afterward.
`ITestRunSettings` implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable), and runs after building the Player with tests.

## Public methods

| Syntax           | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| `void Apply()`   | A method called before building the Player.                  |
| `void Dispose()` | A method called after building the Player or if the build failed. |

## Example
The following example sets the iOS SDK version to be the simulator SDK and resets it to the original value after the run.
``` C#
public class MyTestSettings : ITestRunSettings
{
    private iOSSdkVersion originalSdkVersion;
    public void Apply()
    {
        originalSdkVersion = PlayerSettings.iOS.sdkVersion;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
    }

    public void Dispose()
    {
        PlayerSettings.iOS.sdkVersion = originalSdkVersion;
    }
}
```