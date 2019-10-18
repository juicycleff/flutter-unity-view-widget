# ARKit XR SDK Package

The purpose of this package is to provide ARKit XR Support. 

## Package structure

```none
<root>
  ├── CHANGELOG.md
  ├── Documentation
  │   └── com.unity.arkit.md
  ├── Editor
  │   ├── Unity.XR.ARKit.Editor.asmdef
  │   └── UnityARKitPostBuild.cs
  ├── LICENSE.md
  ├── package.json
  ├── QAReport.md
  ├── README.md
  └── Runtime
      ├── FaceTracking
      │   ├── ARKitFaceSubsystem.cs
      │   ├── Unity.XR.ARKit.FaceTracking.asmdef 
      ├── iOS
      │   ├── link.xml
      │   ├── Resources 
      │   │   └── ARKitShader.shader 
      │   ├── UnityARKit.a 
	  │   └── UnityARKit.m 
      ├── UnitySubsystemsManifest.json
      └── Unity.XR.ARKit.asmdef
```

## Building

1. Initialize submodules:
    1. `git submodule init`
    1. `git submodule update`
1. Build
    1. Run `build.sh` from `Source~/` OR
    1. Open the Xcode project at `Source~/UnityARKit.xcodeproj` and build `UnityARKit` and `UnityARKitFaceTracking`.
