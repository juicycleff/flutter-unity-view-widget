# Migration Guide

This will guide you through the changes coming from AR Foundation 1.0

## TL;DR

* Add an `ARInputManager` anywhere in your scene.
* Add an `ARCameraManager` to your AR camera (this replaces the `ARCameraOptions`).
* Raycast via the `ARRaycastManager` instead of the `ARSessionOrigin`
* Some `TryGet`/`TryAdd`/`TryRemove` APIs were renamed to just `Get`/`Add`/`Remove`
* `GetAllXXX` is now a `trackables` property
* Added, update, and removed events have been combined into a single event that gives you lists of all the added, updated, and removed trackables.

## Events

In 1.0, each trackable manager provided added, updated, and removed events for each trackable. In 2.0, each trackable manager has a single event invoked no more than once per frame containing all the changes (added, updated, and removed) since the last frame.

**Example:**

| 1.0 | 2.0 |
|-|-|
| `ARPlaneManager.planeAdded`, `ARPlaneManager.planeUpdated`, `ARPlaneManager.planeRemoved` | `ARPlaneManager.planesChanged` |

## Session Relative Data

Many of the trackables in AR Foundation 1.0 had "session relative data", e.g., `BoundedPlane` and `XRReferencePoint`. These are no longer directly accessible; all their memebers are now properties of an AR Foundation trackable.

**Example:**

| Trackable | 1.0 accessor | 2.0 accessor |
|-|-|-|
|`ARPlane`|`boundedPlane.Id`|`trackableId`|

## Removed Try

Several APIs used a `TryGet` or `TryAdd` style of API. Such methods that dealt with reference types have dropped the `Try` prefix.

**Examples:**

| 1.0 | 2.0 |
|-|-|
| `ARPlane.TryGetPlane(trackableId)` | `ARPlane.GetPlane(trackableId)` |
| `ARReferencePoint.TryAddReferencePoint` | `ARReferencePoint.AddReferencePoint` |
| `ARReferencePoint.TryAttachReferencePoint` | `ARReferencePoint.AttachReferencePoint` |
| `bool ARReferencePoint.TryRemoveReferencePoint` | `bool ARReferencePoint.RemoveReferencePoint` |

## Enumerating Trackables

Trackable managers previously had a way to obtain a `List` of all trackables, e.g., `ARPlaneManager.GetAllPlanes` and `ARReferencePointManager.GetAllReferencePoints`. There is now a common property called `trackables`, which can be used in a `foreach`, e.g.:

```csharp
var planeManager = GetComponent<ARPlaneManager>();
foreach (var plane in planeManager.trackables) {
    // Do something with the plane
}
```

This property returns a `TrackablesCollection` which does not generate any garbage or cause boxing.

## ARSubsystemManager Removed

The `ARSubsystemManager` has been removed in 2.0. It was previously a singleton which provided access to each of the "subsystems" (the low-level interface to the AR platform). However, some subsystems were also simultaneously managed by a `MonoBehavior`, e.g., `ARPlaneManager`.

The led to confusion about which object to interact with or subscribe to. Now, each "subsystem" has a manager component which not only provides access to that subsystem, but also manages its lifetime.

If you previously used the `ARSubsystemManager`, look for similar functionality on one of the managers:

| 1.0 Subsystem | 2.0 Manager |
|-|-|
| `XRPlaneSubsystem` | `ARPlaneManager` |
| `XRReferencePointSubsystem` | `ARReferencePointManager` |
| `XRDepthSubsystem` | `ARPointCloudManager` |
| `XRSessionSubsystem` | `ARSession` |
| `XRInputSubsystem` | `ARInputManager` (new) |
| `XRCameraSubsystem` | `ARCameraManager` (new) |
| `XRRaycastSubsystem` | `ARRaycastManager` (new) |

## ARInputManager

Previously, pose tracking was implicitly always on. Now, you must have an `ARInputManager` component in your scene to enable it. It does not matter what `GameObject` this is on.

The `ARInputManager` enables input tracking; the `TrackedPoseDriver` consumes pose data (as before).

## ARCameraManager and ARCameraOptions

The `ARCameraManager` enables the `XRCameraSubsystem`; without it, you will not be able to use background rendering or obtain light estimation information. This component should be placed on a Unity Camera, usually on the same one that is parented to the `ARSessionOrigin` and performs background rendering.

The `ARCameraBackground` now requires an `ARCameraManager` component.

The options that were previously on the `ARCameraOptions` component, like focus mode and light estimation mode, are now on the `ARCameraManager`. The `ARCameraOptions` no longer exists.

## ARPointCloudManager

In 1.0, the `ARPointCloudManager` managed a single point cloud. Now, it can manage a collection of them, similar to the way other trackable managers work. Some AR platforms, like ARCore and ARKit, still only have a single point cloud. However, other platforms (supported in the future) will generate multiple point clouds.

As a result, the `ARPointCloudManager.pointCloud` property no longer exists. You can enumerate the point clouds like any other trackable manager, by iterating over its `trackables` property.

## ARRaycastManager

The raycasting API is the same as before, only now it is on a new component `ARRaycastManager`. Previously it was on the `ARSessionOrigin`. If you need to perform a raycast, make sure you have a `ARRaycastManager` on the same `GameObject` as the `ARSessionOrigin` and use the `Raycast` methods on that component.
