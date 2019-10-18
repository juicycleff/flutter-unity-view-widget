# XR Plane Subsystem

The plane subsystem detects flat, planar surfaces in the environment. It is a type of [tracking subsystem](index.html#tracking-subsystems) and follows the same `GetChanges` pattern to inform the user about changes to the state of tracked planes. The trackable for the plane subsystem is the [`BoundedPlane`](../api/UnityEngine.XR.ARSubsystems.BoundedPlane.html).

## Plane Lifecycle

The plane subsystem is designed to inform the user about static surfaces. It is not meant to track moving or otherwise dynamic planes. When a plane is "removed", it generally does not mean a surface has been removed, but rather that the subsystem's understanding of the environment has improved or changed in a way that invalidates a particular plane.

When a surface is first detected, it will be reported as "added". Subsequent updates to the plane are refinements on this initial plane detection. A plane will typically grow as you scan more of the environment.

Some platforms support the concept of planes merging. If a plane is merged into another one, the `BoundedPlane.subsumedById` will contain the id of the plane which subsumed the plane in question. Not all platforms support this, and instead may simply remove one plane and make another plane larger to encompass the first.

## Boundary

A `BoundedPlane` is finite (unlike a `Plane`, which is infinite). In addition to a position and orientation (which would define an infinite plane), planes have a size (width and height) and may also have boundary points. Boundary points are two dimensional vertices defined clockwise in plane-space (a space relative to the plane's `Pose`). The boundary points should define a convex shape.
