# XR Face Subsystem

The face subsystem detects and tracks human faces in the environment.

The face subsystem is a type of [tracking subsystem](index.html#tracking-subsystems), and [`XRFace`](../api/UnityEngine.XR.ARSubsystems.XRFace.html) is its trackable.

## Face Mesh

In addition to a pose, the face subsystem can supply a mesh representing each tracked face. Vertices, indices, normals, and texture coordinates are all optional. Check the [`XRFaceSubsystemDescriptor`](../api/UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.html) to determine runtime capabilities.
