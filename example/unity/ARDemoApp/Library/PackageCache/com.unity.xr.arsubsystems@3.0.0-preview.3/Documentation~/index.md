# About AR Subsystems

A [subsystem](https://docs.unity3d.com/ScriptReference/Experimental.Subsystem.html) is a platform-agnostic interface for surfacing different types of functionality and data. The AR-related subsystems are defined in this package and use the namespace `UnityEngine.XR.ARSubsystems`. This package only provides the interface for various subsystems. Implementations for these subsystems (called "providers") can typically be found in another package or plugin.

This package provides interfaces for the following subsystems:

- [Session](session-subsystem.md)
- [Raycasting](raycasting-subsystem.md)
- [Camera](camera-subsystem.md)
- [Plane Detection](plane-subsystem.md)
- [Depth](depth-subsystem.md)
- [Image Tracking](image-tracking.md)
- [Face Tracking](face-tracking.md)
- [Environment Probes](environment-probe-subsystem.md)
- [Object Tracking](object-tracking.md)

# Installing AR Subsystems

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

Subsystems are implemented in other packages, so to use AR Subsystems, you will also need to install at least one platform-specific AR package (*Window > Package Manager*):

- ARKit XR Plugin
- ARCore XR Plugin

These packages are called subsystem "providers".

# Using AR Subsystems

All subsystems have the same lifecycle: they can be created, started, stopped, and destroyed. Each subsystem has a corresponding `SubsystemDescriptor`, which describes the capabilities of a particular provider. Use the `SubsystemManager` to enumerate the available subsystems of a particular type. Once you have a valid subsystem descriptor, you can `Create()` the subsystem. This is the only way to construct a valid subsystem.

**Example:** Picking a plane subsystem

In this example, we iterate through all the `XRPlaneSubsystemDescriptor`s looking for one which supports a particular feature, then create it. You may only have a single subsystem per platform.

```csharp
XRPlaneSubsystem CreatePlaneSubsystem()
{
    // Get all available plane subsystems:
    var descriptors = new List<XRPlaneSubsystemDescriptor>();
    SubsystemManager.GetSubsystemDescriptors(descriptors);

    // Find one that supports boundary vertices
    foreach (var descriptor in descriptors)
    {
        if (descriptor.supportsBoundaryVertices)
        {
            // Create this plane subsystem
            return descriptor.Create();
        }
    }

    return null;
}
```

Once created, you can `Start` and `Stop` the subsystem. The exact behavior of `Start` and `Stop` varies by subsystem, but generally corresponds to "start doing work" and "stop doing work". A subsystem can be started and stopped multiple times. To completely destroy the subsystem instance, call `Destroy` on the subsystem. It is not valid to access a subsystem after it has been destroyed.

```csharp
var planeSubsystem = CreatePlaneSubsystem();
if (planeSubsystem != null)
{
    // Start plane detection
    planeSubsystem.Start();
}

// ... some time later ...
if (planeSubsystem != null)
{
    // Stop plane detection. This does not discard already detected planes.
    planeSubsystem.Stop();
}

// ... when shutting down the AR portion of the app ...
if (planeSubsystem != null)
{
    planeSubsystem.Destroy();
    planeSubsystem = null;
}
```

Refer to the subsystem-specific documentation list above for more details concerning each subsystem provided by this package.

# Implementing an AR Subsystem

If you are implementing one of the AR Subsystems described by this package (e.g., you are a hardware manufacturer for a new AR device), you will need to implement a concrete instance of the relevant abstract base class provided in this package. Those types are typically named `XR<feature>Subsystem`.

Each subsystem has a nested class called `IProvider`. This is the primary interface you'll need to implement for each subsystem you plan to support.

## Tracking Subsystems

A "tracking" subsystem is any subsystem which detects and tracks something in the physical environment. Examples include plane tracking and image tracking. The thing tracked by the tracking subsystem is called a "trackable". For example, the plane subsystem detects planes, so a plane is a trackable.

Each tracking subsystem requires you to implement a method called `GetChanges`. The purpose of this method is to retrieve data about the trackables it manages. Each trackable can be uniquely identified by a `TrackableId`, a 128-bit guid. A trackable can be added, updated, or removed. It is an error to update or remove a trackable that has not been added. Likewise, a trackable cannot be removed without having been added, nor updated if it has not been added or was already removed.

`GetChanges` should report all added, updated, and removed trackables since the previous call to `GetChanges`. You should expect `GetChanges` to be called once per frame.

Refer to the [Scripting API Reference](../api/) for more details.

# Technical details
## Requirements

This version of AR Foundation is compatible with the following versions of the Unity Editor:

* 2019.2a8 and later

## Known limitations

AR Foundation includes the following known limitations:

* No known issues

## Document revision history

|Date|Reason|
|---|---|
|April 22, 2019|Document created.|