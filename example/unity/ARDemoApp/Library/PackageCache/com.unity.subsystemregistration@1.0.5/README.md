#Subsytem Registration

This package `com.unity.subsystemregistration` is an internal package that will be a dependency of any newly defined "standalone" subsystem. This package will allow that subsystem to register itself with the Subsystem Manager within Unity.  This will let the Subsystem manager to keep track of the features exposed by the newly registered subsystem and provide lifecycle management for it.

## Installing this package

You will not normally need to install this package explicitly since it is installed as a dependency of other packages.  But in case you need to install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).


## Package structure

```none
<root>
  ├── package.json
  ├── README.md
  ├── CHANGELOG.md
  ├── LICENSE.md
  ├── Runtime
  │   ├── Unity.Subsystem.Declaration.asmdef
  │   └── SubsystemRegistration.cs
  └── Documentation~
      └── com.unity.subsystemregistration.md
```

