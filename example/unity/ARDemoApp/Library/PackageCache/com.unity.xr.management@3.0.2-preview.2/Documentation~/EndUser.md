# End Users

### Installing an XR Plugin using XR Management
To install an XR Plugin, do the following:
1. Install the XR Management package from [Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html)
2. Once installed, the XR Management package will take you to the project settings window.
3. Click on the **XR Plugin Management** tab
  - *Note*: The **XR Plugin Management** tab will not exist in Project Settings unless the XR Management package has been installed.
4. In the **XR Plugin Management** tab window, click on the plus button to add a Loader for your plugin.
5. If not already in the Available menu item, go to Download and select the plugin you are interested in. The XR Plugin will automatically download via the Package Manager. 

## Add default loader and settings instances if requested

At package install time, the package may prompt you to create an instance of a loader and an instance of settings for the package. This step is entirely optional and is there simply to help the user get things going at installation time.

If you wish not to create this at installation time, the appropriate portion of the editor that require them (**XRManagerSettings** and **Unified Settings** will) prompt you to create them as well.

**NOTE**: You can always manually control the XR SDK system by accessing the **XRGeneralSettings.Instance.Manager.activeLoader** field once XR SDK has been initialized.

## Add plugin loaders as needed
* Navigate to **Project Settings**.
* Select the **XR Plugin Management** item in the settings selection on the left.
* Modify loaders for each platform you are targeting. You can configure the set of loaders as well as their default ordering.

### Automatic manager loading of XR
By default XR Management will automatically initialize and start your XR environment on application load. At runtime this happens immediately before first scene load. In Play mode this happens immediately after first scene load but before Start is called on your game objects. In either case XR should be setup before Start is called so you should be able to query the state of XR in the Start method of your game objects.

### If you wish to start XR SDK on a per scene basis (i.e. start in 2D and transition into VR)
* Make sure you disable the **Initialize on Startup** toggle for each platform you support.
* At runtime use the **XRGeneralSettings.Instance.Manager** to add/create, remove and reorder the loaders you wish to use from the script.
* To setup the XR environment to run manually call **InitializeLoader(Async)** on the **XRGeneralSettings.Instance.Manager**.
* To start call **StartSubsystems** on **XRGeneralSettings.Instance.Manager**. This will put you into XR mode.
* To stop call **StopSubsystems** on the **XRGeneralSettings.Instance.Manager** to stop XR. This will take you out of XR but should allow you to call **StartSubsystems** again to restart XR.
* To shutdown XR entirely, call **DeinitializeLoader** on the **XRGeneralSettings.Instance.Manager**. This will clean up the XR environment and remove XR entirely. You must call **InitializeLoader(Async)** before you can run XR again.

## Customize build and runtime settings

Any package that needs build or runtime settings should provide a settings datatype for use. This will be surfaced in the **Unified Settings** UI window underneath a top level **XR** node. By default a custom settings data instance will not be created. If you wish to modify build or runtime settings for the package you must go to the package authors entry in **Unified Settings** and select **Create**. This will create an instance of the settings that you can then modify inside of **Unified Settings**.

# Installing *XR SDK Management*

Most likey the XR SDK Provider package you want to use already includes XR Management so you shouldn't need to install it. If you do you can follow the directions provided in the top level documentation or follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

# Installing *Legacy Input Helpers*

The Legacy Input Helpers package is required for correct operation of XR devices with unity. The **Input Helpers** setting page will check for, and allow easy installation of the legacy input helpers package if it is not found on the system. 

This **Input Helpers** setting page can be found by
* Navigate to Project Settings.
* Select the **Input Helpers** sub item of the **XR Plugin Management** item in the settings selection.
