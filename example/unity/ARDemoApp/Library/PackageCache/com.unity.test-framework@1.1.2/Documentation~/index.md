# About Unity Test Framework

The Unity Test Framework (UTF) enables Unity users to test their code in both **Edit Mode** and **Play Mode**, and also on target platforms such as [Standalone](https://docs.unity3d.com/Manual/Standalone.html), Android, iOS, etc.

This package provides a standard test framework for users of Unity and developers at Unity so that both benefit from the same features and can write tests the same way. 

UTF uses a Unity integration of NUnit library, which is an open-source unit testing library for .Net languages. For more information about NUnit, see the [official NUnit website](http://www.nunit.org/) and the [NUnit documentation on GitHub](https://github.com/nunit/docs/wiki/NUnit-Documentation).

> **Note**: UTF is not a new concept or toolset; it is an adjusted and more descriptive naming for the toolset otherwise known as Unity Test Runner, which is now available as this package. 

# Installing Unity Test Framework

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

> **Note**: Search for the Test Framework package. In Unity 2019.2 and higher, you may need to enable the package before use. 

# Using Unity Test Framework

To learn how to use the Unity Test Framework package in your project, read the [manual](./manual.md).

# Technical details

## Requirements

This version of the Unity Test Framework is compatible with the following versions of the Unity Editor:

* 2019.2 and later.

## Known limitations

Unity Test Framework version 1.0.18 includes the following known limitations:

* The `UnityTest` attribute does not support WebGL and WSA platforms.
* The `UnityTest` attribute does not support [Parameterized tests](https://github.com/nunit/docs/wiki/Parameterized-Tests) (except for `ValueSource`).
* The `UnityTest` attribute does not support the `NUnit` [Repeat](https://github.com/nunit/docs/wiki/Repeat-Attribute) attribute.
* Nested test fixture cannot run from the Editor UI. 
* When using the `NUnit` [Retry](https://github.com/nunit/docs/wiki/Retry-Attribute) attribute in PlayMode tests, it throws `InvalidCastException`.

## Package contents

The following table indicates the root folders in the package where you can find useful resources:

| Location                                   | Description                                 |
| :----------------------------------------- | :------------------------------------------ |
| _/com.unity.test-framework/Documentation~_ | Contains the documentation for the package. |

## Document revision history

| Date         | Reason                                                |
| :----------- | :---------------------------------------------------- |
| August 23, 2019 | Applied feedback to the documentation |
| July 25, 2019 | Documentation updated to include features in version 1.1.0 |
| July 11, 2019 | Documentation updated. Matches package version 1.0.18 |
| May 27, 2019 | Documentation created. Matches package version 1.0.14 |
