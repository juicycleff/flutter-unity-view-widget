# Accessing the Camera Image on the CPU

For some applications, it may be desirable to perform processing of the camera image on the CPU, for example, if using a custom computer vision algorithm.

Since the [`XRCameraSubsystem.GetTextures`](https://docs.unity3d.com/ScriptReference/Experimental.XR.XRCameraSubsystem.GetTextures.html) provides a list of external textures, they would need to be read back from the GPU in order to do additional processing, which can be prohibitively expensive. In addition, the number and format of textures varies by platform.

To interact with the CPU camera image, you will need to first obtain a `CameraImage` using the `ARCameraManager`:

```csharp
public bool TryGetLatestImage(out XRCameraImage cameraImage)
```

The `CameraImage` is a `struct` which represents a native resource. When you are finished using it, you must call `Dispose` on it to release it back to the system. Although you may hold a `CameraImage` for multiple frames, most platforms have a limited number of them, so failure to `Dispose` them may prevent the system from providing new camera images.

The `CameraImage` gives you access to three features:
- [Raw Image Planes](#raw-image-planes)
- [Synchronously Convert to Grayscale and Color](#synchronously-convert-to-grayscale-and-color)
- [Asynchronously Convert to Grayscale and Color](#asynchronously-convert-to-grayscale-and-color)

## Raw Image Planes

**Note:** An image "plane" in this context refers to a channel used in the video format (not a planar surface and not related to an `ARPlane`).

Most video formats use a YUV encoding variant, where Y is the luminance plane, and the UV plane(s) contain chromaticity information. U and V may be interleaved or separate planes, and there could be additional padding per pixel or per row.

If you need access to the raw, platform-specific YUV data, you can get each image "plane" with the method `CameraImage.GetPlane`.

### Example:

```csharp
CameraImage image;
if (!cameraManager.TryGetLatestImage(out image))
    return;

// Consider each image plane
for (int planeIndex = 0; planeIndex < image.planeCount; ++planeIndex)
{
    // Log information about the image plane
    CameraImagePlane plane = image.GetPlane(planeIndex);
    Debug.LogFormat("Plane {0}:\n\tsize: {1}\n\trowStride: {2}\n\tpixelStride: {3}",
        planeIndex, plane.data.Length, plane.rowStride, plane.pixelStride);

    // Do something with the data:
    MyComputerVisionAlgorithm(plane.data);
}

// You must dispose the CameraImage to avoid resource leaks.
image.Dispose();
```

A `CameraImagePlane` provides direct access to a native memory buffer via a `NativeArray<byte>`. This represents a "view" into the native memory; you do not need to dispose the `NativeArray`, and the data is only valid until the `CameraImage` is disposed. You should consider the memory read-only.

## Synchronously Convert to Grayscale and Color

To obtain grayscale or color versions of the camera image, the raw plane data needs to be converted. `CameraImage` provides both synchronous and asynchronous conversion methods. This section convers the synchronous version.

This method converts the `CameraImage` into the `TextureFormat` specified by `conversionParams` and writes the data to the buffer at `destinationBuffer`. Grayscale images (`TextureFormat.Alpha8` and `TextureFormat.R8`) are typically very fast, while color conversions require CPU intensive computations.

```csharp
public void Convert(CameraImageConversionParams conversionParams, IntPtr destinationBuffer, int bufferLength)
```

Let's look at `CameraImageConversionParams` in more detail:

```csharp
public struct CameraImageConversionParams
{
    public RectInt inputRect;
    public Vector2Int outputDimensions;
    public TextureFormat outputFormat;
    public CameraImageTransformation transformation;
}
```

|Property|Meaning|
|-|-|
|`inputRect`|The portion of the `CameraImage` to convert. This can be the full image or some sub rectangle of the image. The `inputRect` must fit completely inside the original image. It can be significantly faster convert a sub rectangle of the original image if know which part of the image you need.|
|`outputDimensions`|The dimensions of the output image. The `CameraImage` converter supports downsampling (using nearest neighbor), allowing you to specify a smaller output image than the `inputRect.width` and `inputRect.height` parameters. For example, you could supply `(inputRect.width / 2, inputRect.height / 2)` to get a half resolution image. This can decrease the time it takes to perform a color conversion. The `outputDimensions` must be less than or equal to the `inputRect`'s dimensions (no upsampling).|
|`outputFormat`|The following formats are currently supported<ul><li>`TextureFormat.RGB24`</li><li>`TextureFormat.RGBA24`</li><li>`TextureFormat.ARGB32`</li><li>`TextureFormat.BGRA32`</li><li>`TextureFormat.Alpha8`</li><li>`TextureFormat.R8`</li></ul>You can also use `CameraImage.FormatSupported` to test a texture format before calling one of the conversion methods.|
|`transformation`|Let's you specify a transformation to apply during the conversion, such as mirroring the image across the X or Y (or both) axis. This typically does not increase the processing time.|

Since you must supply the destination buffer, you also need to know how many bytes you'll need to store the converted image. Use

```csharp
public int GetConvertedDataSize(int width, int height, TextureFormat format)
```
To get the required number of bytes.

The data produced by the conversion is compatible with `Texture2D` using [`Texture2D.LoadRawTextureData`](https://docs.unity3d.com/ScriptReference/Texture2D.LoadRawTextureData.html).

### Example:

```csharp
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraImageExample : MonoBehaviour
{
    Texture2D m_Texture;

    void OnEnable()
    {
        cameraManager.cameraFrameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        cameraManager.cameraFrameReceived -= OnCameraFrameReceived;
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        CameraImage image;
        if (!cameraManager.TryGetLatestImage(out image))
            return;

        var conversionParams = new CameraImageConversionParams
        {
            // Get the entire image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Choose RGBA format
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image)
            transformation = CameraImageTransformation.MirrorY
        };

        // See how many bytes we need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so we can dispose of the CameraImage. We must do this or it will leak resources.
        image.Dispose();

        // At this point, we could process the image, pass it to a computer vision algorithm, etc.
        // In this example, we'll just apply it to a texture to visualize it.

        // We've got the data; let's put it into a texture so we can visualize it.
        m_Texture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        m_Texture.LoadRawTextureData(buffer);
        m_Texture.Apply();

        // Done with our temporary data
        buffer.Dispose();
    }
}
```

## Asynchronously Convert to Grayscale and Color

If you do not need the current image immediately, there is also an asynchronous version: `CameraImage.ConvertAsync`. You can make as many asynchronous image requests as you like. They are typically ready by the next frame, but since there is no limit on the number of outstanding requests, it could take some time if there are several in the queue. Requests are processed in the order they are received.

`CameraImage.ConvertAsync` returns an `AsyncCameraImageConversion`. This lets you query the status of the conversion and, once complete, get the pixel data.

Once you have a conversion object, you can query its status to find out if it is done:
```csharp
AsyncCameraImageConversion conversion = image.ConvertAsync(...);
while (!conversion.status.IsDone())
    yield return null;
```
Use the `status` to determine whether the request has completed. If the status `AsyncCameraImageConversionStatus.Ready`, then you may call `GetData<T>` to get the pixel data as a `NativeArray<T>`.

`GetData<T>` returns a `NativeArray<T>` which is a direct "view" into native memory. It is valid until you call `Dispose` on the `AsyncCameraImageConversion`. It is an error to access the `NativeArray<T>` after the `AsyncCameraImageConversion` has been disposed. You do not need to dispose the `NativeArray<T>` returned by `GetData<T>`, only the `AsyncCameraImageConversion`.

**Important:** `AsyncCameraImageConversion`s must be explicitly disposed. Failing to dispose an `AsyncCameraImageConversion` will leak memory until the `XRCameraSubsystem` is destroyed. (The `XRCameraSubsystem` will remove all async conversions when it is destroyed.)

**Note:** `CameraImage` may be disposed before the asynchronous conversion has completed. The data contained by the `AsyncCameraImageConversion` is not tied to the `CameraImage`.

### Example

```csharp
Texture2D m_Texture;

public void GetImageAsync()
{
    // Get information about the camera image
    CameraImage image;
    if (cameraManager.TryGetLatestImage(out image))
    {
        // If successful, launch a coroutine that waits for the image
        // to be ready, then apply it to a texture.
        StartCoroutine(ProcessImage(image));

        // It is safe to dispose the image before the async operation completes.
        image.Dispose();
    }
}

IEnumerator ProcessImage(CameraImage image)
{
    // Create the async conversion request
    var request = image.ConvertAsync(new CameraImageConversionParams
    {
        // Use the full image
        inputRect = new RectInt(0, 0, image.width, image.height),

        // Downsample by 2
        outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

        // Color image format
        outputFormat = TextureFormat.RGB24,

        // Flip across the Y axis
        transformation = CameraImageTransformation.MirrorY
    });

    // Wait for it to complete
    while (!request.status.IsDone())
        yield return null;

    // Check status to see if it completed successfully.
    if (request.status != AsyncCameraImageConversionStatus.Ready)
    {
        // Something went wrong
        Debug.LogErrorFormat("Request failed with status {0}", request.status);

        // Dispose even if there is an error.
        request.Dispose();
        yield break;
    }

    // Image data is ready. Let's apply it to a Texture2D.
    var rawData = request.GetData<byte>();

    // Create a texture if necessary
    if (m_Texture == null)
    {
        m_Texture = new Texture2D(
            request.conversionParams.outputDimensions.x,
            request.conversionParams.outputDimensions.y,
            request.conversionParams.outputFormat,
            false);
    }

    // Copy the image data into the texture
    m_Texture.LoadRawTextureData(rawData);
    m_Texture.Apply();

    // Need to dispose the request to delete resources associated
    // with the request, including the raw data.
    request.Dispose();
}
```

There is also a version of `ConvertAsync` which accepts a delegate and does not return an `AsynCameraImageConversion`:

```csharp
public void GetImageAsync()
{
    // Get information about the camera image
    CameraImage image;
    if (cameraManager.TryGetLatestImage(out image))
    {
        // If successful, launch a coroutine that waits for the image
        // to be ready, then apply it to a texture.
        image.ConvertAsync(new CameraImageConversionParams
        {
            // Get the full image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Color image format
            outputFormat = TextureFormat.RGB24,

            // Flip across the Y axis
            transformation = CameraImageTransformation.MirrorY

            // Call ProcessImage when the async operation completes
        }, ProcessImage);

        // It is safe to dispose the image before the async operation completes.
        image.Dispose();
    }
}

void ProcessImage(AsyncCameraImageConversionStatus status, CameraImageConversionParams conversionParams, NativeArray<byte> data)
{
    if (status != AsyncCameraImageConversionStatus.Ready)
    {
        Debug.LogErrorFormat("Async request failed with status {0}", status);
        return;
    }

    // Do something useful, like copy to a Texture2D or pass to a computer vision algorithm
    DoSomethingWithImageData(data);

    // data is destroyed upon return; no need to dispose
}
```

In this version, the `NativeArray<byte>` is again a "view" into the native memory associated with the request, and you need not dispose of it. It is only valid for the duration of the delegate invocation and is destroyed immediately upon return. If you need the data to live beyond the lifetime of your delegate, make a copy (see [`NativeArray<T>.CopyTo`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.CopyTo.html) & [`NativeArray<T>.CopyFrom`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.CopyFrom.html)).
