# XR Depth Subsystem

The depth subsystem is an interface into depth information detected in the scene. Currently, this means the detection of feature points, unique features in the environment which can be correlated between multiple frames.

A set of feature points is called a point cloud. The depth subsystem is a type of [tracking subsystem](index.html#tracking-subsystems) and [`XRPointCloud`](../api/UnityEngine.XR.ARSubsystems.XRPointCloud.html) is its trackable.

Some providers may only have one `XRPointCloud`, while others have several. Check with your provider's documentation for more details.
