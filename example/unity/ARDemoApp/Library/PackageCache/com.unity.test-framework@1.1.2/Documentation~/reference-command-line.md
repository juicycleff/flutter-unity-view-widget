# Running tests from the command line

It’s pretty simple to run a test project from the command line. Here is an example in Windows:

```bash
Unity.exe -runTests -batchmode -projectPath PATH_TO_YOUR_PROJECT -testResults C:\temp\results.xml -testPlatform PS4
```

For more information, see [Command line arguments](https://docs.unity3d.com/Manual/CommandLineArguments.html).

## Commands

### batchmode 

Runs Unity in batch mode and ensures no pop-up windows appear to eliminate the need for any human intervention.

### forgetProjectPath

Don't save your current **Project** into the Unity launcher/hub history.

### runTest

Runs tests in the Project.

### testCategory

A semicolon-separated list of test categories to include in the run. If using both `testFilter` and `testCategory`, then tests only run that matches both.

### testFilter

A semicolon-separated list of test names to run, or a regular expression pattern to match tests by their full name.

### testPlatform

The platform you want to run tests on. Available platforms are **EditMode** and **PlayMode**. 

> **Note**: If unspecified, tests run in Edit Mode by default.

Platform/Type convention is from the [BuildTarget](https://docs.unity3d.com/ScriptReference/BuildTarget.html) enum. Supported platforms are:

* StandaloneWindows
* StandaloneWindows64
* StandaloneLinux64
* StandaloneOSX
* iOS
* Android
* PS4
* XboxOne

### testResults

The path where Unity should save the result file. By default, Unity saves it in the Project’s root folder.

### testSettingsFile 

Path to a *TestSettings.json* file that allows you to set up extra options for your test run. An example of the *TestSettings.json* file could look like this:

```json
{
  "scriptingBackend":2,
  "Architecture":null,
  "apiProfile":0
}
```

#### apiProfile

The .Net compatibility level. Set to one of the following values:  

- 1 - .Net 2.0 
- 2 - .Net 2.0 Subset 
- 3 - .Net 4.6 
- 5 - .Net micro profile (used by Mono scripting backend if **Stripping Level** is set to **Use micro mscorlib**) 
- 6 - .Net Standard 2.0 

#### appleEnableAutomaticSigning

Sets option for automatic signing of Apple devices.

#### appleDeveloperTeamID 

Sets the team ID for the apple developer account.

#### architecture

Target architecture for Android. Set to one of the following values: 

* None = 0
* ARMv7 = 1
* ARM64 = 2
* X86 = 4
* All = 4294967295

#### iOSManualProvisioningProfileType

Set to one of the following values: 

* 0 - Automatic 
* 1 - Development 
* 2 - Distribution iOSManualProvisioningProfileID

#### scriptingBackend

 Set to one of the following values:

- Mono2x = 0 
- IL2CPP = 1 
- WinRT DotNET = 2 

#### useLatestScriptingRuntimeVersion

Sets option to always use the latest [Scripting Runtime Version](https://docs.unity3d.com/Manual/ScriptingRuntimeUpgrade.html).