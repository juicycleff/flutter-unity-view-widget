# XR Raycast Subsystem

Raycasts allow you to perform hit testing against AR-specific features. It is conceptually similar to the [Physics.Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html), but raycast targets do not require a presence in the physics world.

There are two types of raycasts:
- Screen point
- Arbitrary ray

Some implementations only support one or the other. You can check for support with the [`XRRaycastSubsystemDescriptor`](../api/UnityEngine.XR.ARSubsystems.XRRaycastSubsystemDescriptor.html).

## Performing Raycasts

See the [Script API Reference](../api/UnityEngine.XR.ARSubsystems.XRRaycastSubsystem.html) for API help. Raycasts are tested aginst specified [`TrackableType`](../api/UnityEngine.XR.ARSubsystems.TrackableType.html)s, a mask of trackable types against which to raycast, and sorted by distance from the ray's origin.
