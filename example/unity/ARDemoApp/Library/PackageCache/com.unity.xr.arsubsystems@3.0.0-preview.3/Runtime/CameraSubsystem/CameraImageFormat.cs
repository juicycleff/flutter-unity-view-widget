namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Formats used by the raw <see cref="XRCameraImage"/> data. See <see cref="XRCameraImage.format"/>.
    /// </summary>
    public enum CameraImageFormat
    {
        /// <summary>
        /// The format is unknown or could not be determined.
        /// </summary>
        Unknown,

        /// <summary>
        /// <para>Three-Plane YUV 420 format commonly used by Android. See
        /// <a href="https://developer.android.com/ndk/reference/group/media#group___media_1gga9c3dace30485a0f28163a882a5d65a19aea9797f9b5db5d26a2055a43d8491890">
        /// AIMAGE_FORMAT_YUV_420_888</a>.</para>
        /// <para>This format consists of three image planes. The first is the Y (luminocity) plane, with 8 bits per
        /// pixel. The second and third are the U and V (chromaticity) planes, respectively. Each 2x2 block of pixels
        /// share the same chromaticity value, so a given (x, y) pixel's chromaticity value is given by
        /// <code>
        /// u = UPlane[(y / 2) * rowStride + (x / 2) * pixelStride];
        /// v = VPlane[(y / 2) * rowStride + (x / 2) * pixelStride];
        /// </code></para>
        /// </summary>
        AndroidYuv420_888,

        /// <summary>
        /// <para>Bi-Planar Component Y'CbCr 8-bit 4:2:0, full-range (luma=[0,255] chroma=[1,255]) commonly used by
        /// iOS. See
        /// <a href="https://developer.apple.com/documentation/corevideo/1563591-pixel_format_identifiers/kcvpixelformattype_420ypcbcr8biplanarfullrange">
        /// kCVPixelFormatType_420YpCbCr8BiPlanarFullRange</a>.</para>
        /// <para>This format consists of two image planes. The first is the Y (luminocity) plane, with 8 bits per
        /// pixel. The second plane is the UV (chromaticity) plane. The U and V chromaticity values are interleaved
        /// (u0, v0, u1, v1, etc.). Each 2x2 block of pixels share the same chromaticity values, so a given (x, y)
        /// pixel's chromaticity value is given by
        /// <code>
        /// u = UvPlane[(y / 2) * rowStride + (x / 2) * pixelStride];
        /// v = UvPlane[(y / 2) * rowStride + (x / 2) * pixelStride + 1];
        /// </code>
        /// pixelStride is always 2 for this format, so this can be optimized to
        /// <code>
        /// u = UvPlane[(y >> 1) * rowStride + x &amp; ~1];
        /// v = UvPlane[(y >> 1) * rowStride + x | 1];
        /// </code></para>
        /// </summary>
        IosYpCbCr420_8BiPlanarFullRange
    }
}
