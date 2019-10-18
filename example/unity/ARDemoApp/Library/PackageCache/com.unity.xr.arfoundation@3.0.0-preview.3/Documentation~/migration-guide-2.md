# Migration Guide

This will guide you through the changes coming from AR Foundation 2.x to 3.x.

## Image Tracking

The image tracking manager `ARTrackedImageManager` has a `referenceLibrary` property on it to set the reference image library (the set of images to detect in the environment). Previously, this was an `XRReferenceImageLibrary`. Now, it is an `IReferenceImageLibrary`, and `XRReferenceImageLibrary` implements `IReferenceImageLibrary`. If you had code that was setting the `referenceLibrary` property to an `XRReferenceImageLibrary`, it should continue to work as before. However, if you previoulsy treated the `referenceLibrary` as an `XRReferenceImageLibrary`, you will have to attempt to cast it to a `XRReferenceImageLibrary`.

In the Editor, it will always be an `XRReferenceImageLibrary`. However, at runtime with image tracking enabled, `ARTrackedImageManager.referenceLibrary` will return a new type, `RuntimeReferenceImageLibrary`. This still behaves like an `XRReferenceImageLibrary` (e.g., you can enumerate its reference images), and it may also have additional functionality (see `MutableRuntimeReferenceImageLibrary`).

## Background shaders

The `ARCameraBackground` has been updated to support the lightweight render pipeline (LWRP) and Universal Render Pipelines (UniversalRP) when those packages are present. This involved a breaking change to the `XRCameraSubsystem`: the property `shaderName` is now `cameraMaterial`. It is unlikely most developers would need to access this directly. The shader name was only used by ARFoundation to construct the background material. That functionality has moved to the subsystem.

## Point Clouds

The [`ARPointCloud`](point-cloud-manager.md) properties
[`positions`](../api/UnityEngine.XR.ARFoundation.ARPointCloud.html#UnityEngine_XR_ARFoundation_ARPointCloud_positions),
[`confidenceValues`](../api/UnityEngine.XR.ARFoundation.ARPointCloud.html#UnityEngine_XR_ARFoundation_ARPointCloud_confidenceValues),
and
[`identifiers`](../api/UnityEngine.XR.ARFoundation.ARPointCloud.html#UnityEngine_XR_ARFoundation_ARPointCloud_identifiers)
have changed from returning [`NativeArray`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.html)s to [nullabe](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/nullable-types/) [`NativeSlice`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeSlice_1.html)s. The `ARPointCloud` manages the memory contained in these `NativeArray`s, so callers should only be able to see a `NativeSlice` (i.e., you should not be able to [`Dispose`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.Dispose.html) of the `NativeArray`).

Additionally, these arrays are not necessarily present. Previously, you could check for existence with [`NativeArray<T>.IsCreated`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.IsCreated.html). `NativeSlice` does not have an `IsCreated` property, so these properties have been made nullable.

## Face Tracking

The [`ARFaceManager`](face-manager.md)'s `supported` property has been removed. If face tracking is not supported, the manager's subsystem will be null. This was done for consistency as no other manager has this property. If a manager's subsystem is null after enabling the manager, that generally means the subsystem is not supported.
