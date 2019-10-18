# AR Subsystems

The purpose of this `com.unity.xr.arsubsystems` package is to provide definitions of all subsystems that enable AR functionality.

Presently, this package defines the following XR subsystems:
    EnvironmentProbeSubsystem
    FaceSubsystem
    ImageTrackingSubsystem
    ObjectTrackingSubsystem

## Installing AR Subsystems

This package is normally not installed by a user, but rather as a dependency defined in other packages.

## Package structure

```none
<root>
  ├── package.json
  ├── README.md
  ├── CHANGELOG.md
  ├── LICENSE.md
  ├── QAReport.md
  └── Runtime
      ├── Unity.XR.ARSubsystems.asmdef
      ├── EnvironmentProbeSubsystem
      ├── FaceSubsystem
      ├── ImageTrackingSubsystem
      └── ObjectTrackingSubsystem
```

## Package usage

This package is used as a dependency for other packages in two scenarios:

1. The other package wants to use the APIs defined in this one.  Packages in this category include:
[ARFoundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.0/)

1. The other package wants to extend the API and implement a provider that provides data to the Subsystems defined in this package.  Packages in this category include:
[ARKit XR Plugin](https://docs.unity3d.com/Packages/com.unity.xr.arkit@1.0/)
[ARCore XR Plugin](https://docs.unity3d.com/Packages/com.unity.xr.arcore@1.0/)

## Documentation

* [Script API](Runtime/) <update?>
* [Manual](Documentation~/com.unity.xr.arsubsystems.md)
