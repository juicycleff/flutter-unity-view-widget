# About *XR SDK Management* package

Use the **XR SDK Management** package to help streamline **XR SDK** lifecycle management and potentially provide users with build time UI through the Unity **Unified Settings** system.

# Installation

To use this package you need to add a reference to it in the Packages/manifest.json file located in your project. There are three ways you can reference a specific version of a package depending and how you are using it.

## Using a production version of the package

For a released version of the package in production, referencing the package is no different than any other released package. If visible in the Package Manager UI you can just select the package and install it. Alternatively, you can edit the manifest.json file to add it manually like this:

```json
	"dependencies": {
        ...
		"com.unity.xr.management":"<full version number>"
	}
```

## Using a local clone of the package

If you want to use a cloned version of the package directly, you can point the Package Manager at a local folder as the location from which to get the package from.

```json
	"dependencies": {
        ...
		"com.unity.xr.management":"file:path/to/package/root"
	}
```

NOTE: The package root is not necessarily the root of the cloned repo. The package root the folder where the package's package.json file located.


# Using XR SDK Management

There are two target audiences for XR SDK Management: The End User and the Provider. Documentation for those can be found here:

* [End Users Documentation](./EndUser.md)
* [Provider Documentation](./Provider.md)

# Technical details

## Requirements

This version of **XR SDK Management** is compatible with the following versions of the Unity Editor:

* 2019.1 and later (recommended)

## Known limitations

* Still in preview. Package structure, code and documentation may change and break existing users at any time.

## Package contents

This version of **XR SDK Management** includes:

* **XRManagerSettings** - This is a **ScriptableObject** that can be accessed from the script and provides for management of **XRLoader** instances and their lifecycle.
* **XRLoader** - This is the base class all loaders should derive from. It provides a basic the **XRManagerSettings** can use to manage lifecycle and a simple API to allow users to request specific subsystems from the loader as and when needed.
* **XRConfigurationData** - This is an attribute that allows for build and runtime settings to be hosted within the **Unified Settings** window. All instances will be hosted under the top level **XR** entry within the **Unified Settings** window under the name supplied as part of the attribute. The management package will maintain and manage the lifecycle for one instance of the build settings using **EditorBuildSettings** config object API, stored with the key provided in the attribute. At any time, the provider or the user is free access the configuration settings instance by asking **EditorBuildSettings** for the instance associated with the chosen key (as set in the attribute).
* **XRPackageInitializationBase** - Helper class to derive from that simplifies some of the busy work for package initialization. Helps to create a default instance of the packages XRLoader and default settings at the time of package installation. Initialization is run once only and the package developer should not depend on the user creating the specified instances.
* **XRBuildHelper** - Abstract class useful for handling some boilerplate around moving settings from Editor -> Runtime. Derive from this class and specific the appropriate settings type and the system will take care of marshaling that type from EditorUserBuildSettings to PlayerSettings to be used at runtime.
* **XRGeneralSettings** - Contains settings specific to all of XR SDK and not to any single provider.
* **Samples** - There is a samples folder in the package that contains an implementation of all parts of XR Management. Copy that folder to a location in your project/package to get started with implementing XR Management for your needs.
