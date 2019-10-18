# README #

The Unity Multiplayer High Level API is the open source component of the Unity Multiplayer system, this was formerly a Unity extension DLL with some parts in the engine itself, now it all exist in a package. In this package we have the whole networking system except the NetworkTransport related APIs and classes. This is all the high level classes and components which make up the user friendly system of creating multiplayer games. This document details how you can enable or embed the package and use it in your games and applications.

### What license is the Unity Multiplayer HLAPI package shipped under? ###
Unity Multiplayer HLAPI package is released under an MIT/X11 license; see the LICENSE.md file.

This means that you pretty much can customize and embed it in any software under any license without any other constraints than preserving the copyright and license information while adding your own copyright and license information.

You can keep the source to yourself or share your customized version under the same MIT license or a compatible license.

If you want to contribute patches back, please keep it under the unmodified MIT license so it can be integrated in future versions and shared under the same license.

### How do I get started? ###
* Go to the Package Manager UI in the Unity editor (found under the Window menu).
* The HLAPI package should appear in the list of packages, select it and click the Install button

or

* Add the package to your project manifest.json file, located in the Packages folder. Under dependencies add the line _"com.unity.multiplayer-hlapi": "1.0.4"_ to the list of packages. A specific version needs to be chosen.

or

* Clone this repository into the Packages folder.

### Running tests ###

When the package files are directly included in the Packages folder of the projects (or somewhere in the Assets folder), the tests will appear and can be executed. 

When including the package via the manifest.json file the `testable` field needs to be added:

```
{
  "dependencies": {
    "com.unity.multiplayer-hlapi": "1.0.4",
    ... more stuff...
  },
  "testables": [
    "com.unity.multiplayer-hlapi"
  ]
}
```

where there referenced package number should be the latest or whatever version is being tested.

When the package is included for the first time, it will be compiled, and some of the test will fail to run since the weaver has not had a chance to run yet. Triggering a recompile should fix that, for example by reimporting some script or triggering a build.

### Will you be taking pull requests? ###
We'll consider all incoming pull requests that we get. It's likely we'll take bug fixes this way but anything else will be handled on a case by case basis. Changes will not be applied directly to this repository but to an internal package repository which will be periodically synchronized with this one.