# About com.unity.xr.legacyinputhelpers

The com.unity.xr.legacyinputhelpers package contains a number of useful helpers for building XR Projects.
These include the Tracked Pose Driver and the Input Asset XR Bindings Seed Utility.

## Requirements

The com.unity.xr.legacyinputhelpers package version 2.0.0 is compatible with the following versions of the Unity Editor:

* 2019.1 (recommended)

# Installing com.unity.xr.legacyinputhelpers

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

# Using com.unity.xr.legacyinputhelpers

The com.unity.xr.legacyinputhelpers package contains a number of useful helpers for building XR Projects.
* [Tracked Pose Driver](#Tracked-Pose-Driver)
* [XR Input Binding Seeder](#Seeding-XR-Input-Bindings)
* [Arm Models](ArmModels.md)
* [Using with XR Management](#Using-with-XR-Management)

# Tracked Pose Driver

The Tracked Pose Driver component is intended to be used to update a Game Object’s transform to match that of a Tracked Device. A Tracked Device is any input device which can generate a Pose. For Example: A VR HMD, an AR Device, or an MR Tracked Controller.

The following section of this document details the different settings and options available to the Tracked Pose Driver.

## Settings
### Device & Pose Source

The Tracked Pose Driver is used to update a target transform using a Pose Source. The Pose Source is defined as a combination of the settings of both the Device Field, and the Pose Source Field.

The Device field is used to indicate which type of device that the  pose source will be obtained from. This field has the following options:
* Generic XR Device  
   This option is intended to represent VR HMDs and AR Device poses.
* Generic XR Controller  
	This option is intended to represent VR Controllers.
* Generic XR Remote  
	This option is intended to represent mobile based remote devices.

The following image shows the options above in the actual Tracked Pose Driver component.

![Tracked Pose Driver](images/deviceselection.png)

Once the Device field has been set, the Pose Source field will be updated with the available sources for that Device. These options are listed below.

| Device | Source | Description | Usage |
| ------ | ------ | ----------- | ----- |
| Generic XR Device | Left Eye | The pose of the Left Eye of the device. | VR HMDs |
| | Right Eye | The pose of the Right Eye of the device. | VR HMDs |
| | Center Eye | The pose of the Center Eye of the device. | VR HMDs |
| | Head | The pose of the Head of the device, if available. In most devices, the Head pose will be the same as the Center Eye. | VR HMDs |
| | Color Camera | The pose of the Color Camera on the device, if available. The Color camera is intended for use with AR devices which support camera based spatial mapping. | AR Devices |
| Generic XR Controller | Left Controller | The Left Controller device pose if available. This is intended to be used with 6 Degrees of Freedom style controllers commonly used with VR and MR devices. | 6 and 3 Degrees of Freedom VR Tracked Controllers |
| | Right Controller | The Right Controller device pose if available. Zero if not. This is intended to be used with 6 Degrees of Freedom style controllers commonly used with VR and MR devices. | 6 and 3 Degrees of Freedom VR Tracked Controllers |
| Generic XR Remote | Device Pose | This pose is intended to be used with 3 Degrees of Freedom style controllers commonly used with mobile VR devices. Eg: The Google Daydream Controller |Mobile 3 Degrees of Freedom Tracked Controllers |

In the case where a requested Source Pose is not valid, a position vector consisting of Zeros, and an Identity Quaternion will be provided. The following image shows an example of the Pose Source drop down when the “Generic XR Device” device has been selected.

![Tracked Pose Driver](images/poseselection.png)

## Tracking Type

The tracking type option of the Tracked Pose Driver allows the developer to control how the tracked pose is applied to the target transform.

If the Position option is chosen, the Position part of the Source Pose will be applied to the target transform.

If the Rotation option is chosen, the Rotation part of the Source Pose will be applied to the target transform.

If the Both Position and Rotation is chose, the entire Source Pose will be applied to the target transform.  The following image shows the possible options.

![Tracked Pose Driver](images/trackingtypeselection.png)

The default selection is to apply both the Rotation and the Position of the tracked object to the target transform.

## Update Type
The update type option allows the developer to control when the Tracked Pose Driver applies updates from the tracked pose source. The two points are; Update, and Before Render. The default, and recommended, option is to apply updates to the target transform at both of these phases especially in situations where a Tracked Pose Driver is driving a camera Pose. It is critically important to have the position of the camera updated as close to rendering as possible for user comfort and latency reasons.

The Update option will cause the transform to be set in both Fixed Update (if happening that frame) and the start of the normal Update frame. This is to ensure that the target transform is in the correct location prior to executing any scripts during those phases. The following image shows the Update Type options

![Tracked Pose Driver](images/updatetypeselection.png)

The following table outlines when during the frame the Target Pose will be updated:

|Option | Fixed Update | Update | Before Render |
| ----- | ----------- | ------ | ------ |
| Before Render Only | No | No | Yes |
| Update Only | Yes | Yes | No |
| Both Update and Before Render | Yes | Yes | Yes |

## Use Relative Transform
The “Use Relative Transform” option allows the user to control how the pose source is applied to the target transform. This option will be deprecated in the future, please do not use.

When the “Use Relative Transform” option is set, the Tracked Pose Driver caches the original position of the transform internally. This is then used to offset any source pose values so that they will be correct relative to the starting location of the object being controlled.

Alternatively if the ”Use Relative Transform” option is not set, the Tracked Pose Driver will apply the source pose value directly to the target transform. This is useful for when the target transform is part of a greater transform hierarchy. The following image shows the “Use Relative Transform” option of the Tracked Pose Driver.

![Tracked Pose Driver](images/trackedposedriver.png)

The “Use Relative Transform” option was added to provide compatibility with the implicit camera control for VR cameras within Unity. It is intended that this option, along with the Reference Transform for implicit cameras, will be removed in the future. It is recommended that best practices for Object Hierarchies be followed to correctly reflect tracked objects in the correct space.
## Additional Information
* The  Tracked Pose Driver can only track one pose at a time
* If the device and pose combination are not valid, the resulting transform will be zero position and identity rotation.
* It is possible to change the tracked node at runtime via script

### Special Case Behaviour when attached to cameras
* When attached to a camera, the implicit VR Device control of the camera transform is disabled, and the value generated by the Tracked Pose Driver is used instead
<hr>

# Seeding XR Input Bindings

The Seed XR Input Bindings Tool can be used to populate the Input Asset with a set of crossplatform bindings intended for use with XR Devices.

## Using the XR Input Bindings Seeder

Loading the Legacy Input Helpers package will add an additional menu option under the "Assets" Top Level Menu called "Seed XR Bindings". 

![Asset Menu](images/assetmenu.png)

Clicking this menu option will seed the Input Asset with the Unity Cross Platform Input bindings. The XR Input Bindings will not replace any bindings which are already present in the Input Asset with the same name
<hr>


# Using with XR Management

When using com.unity.xr.legacyinputhelpers in conjunction with the com.xr.management package a number of additional helpers are available in the `Settings -> XR Plugin Management -> XR Tracking` Menu. These helpers can be used to automate adding Tracked Pose Drivers to various cameras within the current scene.

Implicit camera tracking is no longer supported with XRSDK. In order to correctly track the HMD, phone or other device the Tracked Pose Driver can be used to implement Camera Tracking.

The XR Tracking Pane provides three options
- Add a tracked pose driver to all cameras which are tagged with 'Main Camera' in the current scene
- Add a tracked pose driver to all cameras which are "stereo" in the current scene
- Add a tracked pose driver to any camera in the current scene

Any camera that is modified by clicking one of the options will be logged to the console window.


<hr>
# Document Revision History

|Date|Reason|
|---|---|
|Feb 21, 2018|Initial edit.|
|Sept 13, 2018|Update to final release version, changed name to final|
|Oct 8,2018| renamed to legacyinputhelpers|
|Oct 15,2018| Added section for seeding XR Input Bindings|
|July 19,2018| Added section for using this package with XR Management|