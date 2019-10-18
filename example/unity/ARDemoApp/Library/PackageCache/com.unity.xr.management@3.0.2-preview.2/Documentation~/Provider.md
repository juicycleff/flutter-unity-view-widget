# Package Author

## Lifecycle Management

This package provides for management of **XR SDK** subsystem lifecycle without the need for boilerplate code. The **XRManagerSettings** class provides a scriptable object that can be used by the app for start, stop and de-initialization of a set of subsystems defined by an **XRLoader** instance.

A provider must create a subclass of **XRLoader** to make a loader available for their particular runtime scheme.

The **XRLoader** interface looks like this:

```csharp
public abstract class XRLoader : ScriptableObject
{
    public virtual bool Initialize() { return false; }

    public virtual bool Start() { return false; }

    public virtual bool Stop() { return false; }

    public virtual bool Deinitialize() { return false; }

    public abstract T GetLoadedSubsystem<T>() where T : IntegratedSubsystem;
}
```

There is a class called **XRLoaderHelper** which you can derive from to handle subsystem management in a typesafe manner.  See **[Samples/SampleLoader.cs](/Samples/SampleLoader.cs)** for an example.

An **XRLoader** is simply a **ScriptableObject** and as such, the user is able to create and instance (or more if they want) of the loader. Each **XRLoader** subclass defines the subsystems and their load order and is responsible for managing the set of subsystems they require.

A user will add all the **XRLoaders** instances they created to the Loaders property on the **XRManagerSettings**, arranging them in the load order that they desire.

When asked to initialize, **XRManagerSettings** will call each **XRLoader** instance it has a reference to in the order it has and attempt to initialize each one. The first loader that succeeds initialization becomes the active loader and all further attempts to initialize are stopped. From this point the user can ask for the static **XRManagerSettings.ActiveLoader** instance to get access to the active loader. If initialization fails for all loaders, **activeLoader** is set to null.

Scene based Automatic lifecycle management hooks into the following **MonoBehaviour** callback points:

|Callback|Lifecycle Step|
|---|---|
|OnEnable|Find the first loader that succeeds initialization and set ActiveLoader.|
|Start|Start all subsystems|
|OnDisable|Stop all subsystems|
|OnDestroy|De-initialize all subsystems and remove the ActiveLoader instance.|

App lifetime based Automatic lifecycle management hooks into the following callback points:

|Callback|Lifecycle Step|
|---|---|
|Runtime Initialization After Assemblies Loaded|Find the first loader that succeeds intiialization and set ActiveLoader.|
|Runtime Initialization Before splash screen is shown|Start all subsystems|
|OnDisable|Stop all subsystems|
|OnDestroy|Deintialize all subsystems and remove the ActiveLoader instance.|

## Build and Runtime settings through *Unified Settings*

A provider may need optional settings to help manage build issues or runtime configuration. They can do this by adding the **XRConfigurationData** attribute to a ScriptableObject and providing the set of properties they want to surface for users to control configuration. Configuration options will be surfaced in the **Unified Settings** window under the **XR** entry. We will manage the lifecycle for one instance of the class marked with the attribute through the EditorBuildSettings config object API. If no special UI is provided, the **Unified Settings** window will display the configuration settings using the standard **ScriptableObject** UI Inspector. A provider can extend the UI by creating a custom **Editor** for their configuration settings type and that will be used in the **Unified Settings** window instead. 

The provider will need to handle getting the settings from **EditorUserBuildSettings** into the build application. This can be done with a custom build processing script. If all you need for build support is to make sure that you have access to the same settings at runtime you can derive from **XRBuildHelper<T>**. This is a generic abstract base class that handles the necessary work of getting the build settings stored in EditorUserBuildSettings and getting them into the build application for access at runtime. Simplest build script for your package would look like this:

```csharp
public class MyBuildProcessor : XRBuildHelper<MySettings> 
{
    public override string BuildSettingsKey { get { return "MyPackageSettingsKey"; } }
}
```

You can override the build processing steps from **IPreprocessBuildWithReport** and **IPostprocessBuildWithReport** but make sure that you call to the base class implementation or else your settings will not be copied.

```csharp
public class MyBuildProcessor : XRBuildHelper<MySettings> 
{ 
    public override string BuildSettingsKey { get { return "MyPackageSettingsKey"; } }

    public override void OnPreprocessBuild(BuildReport report)
    {
        base.OnPreprocessBuild(report);
        // Do your work here
    }

    public override void OnPostprocessBuild(BuildReport report)
    {
        base.OnPreprocessBuild(report);
        // Do your work here
    }
}
```

If you wish to support per platform settings at build time, you can override `UnityEngine.Object SettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)` and use the passed in buildTargetGroup to retrieve the appropriate platform settings. By default this method just uses the key associated with the settings instance to copy the entire settings object from EditorUserBuildSettings to PlayerSettings.

```csharp
public class MyBuildProcessor : XRBuildHelper<MySettings> 
{ 
    public override string BuildSettingsKey { get { return "MyPackageSettingsKey"; } }

    public override UnityEngine.Object SettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
    {
        // ... Get platform specific settings and return them... Something like the following
        // for simple settings data that isn't platform specific.
        UnityEngine.Object settingsObj = null;
        EditorBuildSettings.TryGetConfigObject(BuildSettingsKey, out settingsObj);
        if (settingsObj == null || !(settingsObj is T))
            return null;

        return settingsObj;

    }
}
```

If you need more extensive support and/or absolute control you can copy the **SampleBuildProcessor** in the _Samples/Editor_ folder and work from there.

## Package Initialization

Given the need for **ScriptableObject** instance to support loaders and settings, the user will be forced to create these instances at some point. Both **XRManagerSettings** and the **Unified Settings** support can handle creating the necessary instances on demand but it may be beneficial to create these up front at package installation. If you derive a class from **XRPackageInitializationBase** and fill out the interface properties and methods the management system will provide for creation of default instances of your loader and settings at installation. Creation of each one is driven by prompting the user if it is OK to do that and, if so, creating the instance in the appropriate default location in the Assets folder. For loaders this is Assets/XR/Loaders. For settings this is Assets/XR/Settings. While these are the recommended locations for putting these package based objects they do not have to be located there. The user is free to move them around as long as we can find at least one instance of each type for each package.

# Installing *XR SDK Management*

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).
