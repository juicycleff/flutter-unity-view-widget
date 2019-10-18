using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes a set of conversion parameters for use with <see cref="XRCameraImage"/>'s conversion methods.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct XRCameraImageConversionParams : IEquatable<XRCameraImageConversionParams>
    {
        RectInt m_InputRect;
        Vector2Int m_OutputDimensions;
        TextureFormat m_Format;
        CameraImageTransformation m_Transformation;

        /// <summary>
        /// The portion of the original image that will be used as input to the conversion.
        /// </summary>
        /// <remarks>
        /// The input rectangle must be completely contained inside the <c>XRCameraImage</c>
        /// <see cref="XRCameraImage.dimensions"/>.
        /// </remarks>
        /// <value>
        /// The portion of the original image that will be converted.
        /// </value>
        public RectInt inputRect
        {
            get { return m_InputRect; }
            set { m_InputRect = value; }
        }

        /// <summary>
        /// The dimensions of the converted image. The output dimensions must be less than or equal to the
        /// <see cref="inputRect"/>'s dimensions. If the output dimensions are less than the <see cref="inputRect"/>'s
        /// dimensions, downsampling is performed using nearest neighbor.
        /// </summary>
        /// <value>
        /// The dimensions of the converted image.
        /// </value>
        public Vector2Int outputDimensions
        {
            get { return m_OutputDimensions; }
            set { m_OutputDimensions = value; }
        }

        /// <summary>
        /// The <c>TextureFormat</c> to which to convert. See <see cref="XRCameraImage.FormatSupported"/> for a list of
        /// supported formats.
        /// </summary>
        /// <value>
        /// The <c>TextureFormat</c> to which to convert.
        /// </value>
        public TextureFormat outputFormat
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        /// <summary>
        /// The transformation to apply to the image during conversion.
        /// </summary>
        /// <value>
        /// The transformation to apply to the image during conversion.
        /// </value>
        public CameraImageTransformation transformation
        {
            get { return m_Transformation; }
            set { m_Transformation =  value; }
        }

        /// <summary>
        /// Constructs a <see cref="XRCameraImageConversionParams"/> using the <paramref name="image"/>'s full
        /// resolution. That is, it sets <see cref="inputRect"/> to <c>(0, 0, image.width, image.height)</c> and
        /// <see cref="outputDimensions"/> to <c>(image.width, image.height)</c>.
        /// </summary>
        /// <param name="image">The source <see cref="XRCameraImage"/>.</param>
        /// <param name="format">The <c>TextureFormat</c> to convert to.</param>
        /// <param name="transformation">An optional <see cref="CameraImageTransformation"/> to apply.</param>
        public XRCameraImageConversionParams(
            XRCameraImage image,
            TextureFormat format,
            CameraImageTransformation transformation = CameraImageTransformation.None)
        {
            m_InputRect = new RectInt(0, 0, image.width, image.height);
            m_OutputDimensions = new Vector2Int(image.width, image.height);
            m_Format = format;
            m_Transformation = transformation;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = inputRect.GetHashCode();
                hash = hash * 486187739 + outputDimensions.GetHashCode();
                hash = hash * 486187739 + ((int)outputFormat).GetHashCode();
                hash = hash * 486187739 + ((int)transformation).GetHashCode();
                return hash;
            }
        }

        public bool Equals(XRCameraImageConversionParams other)
        {
            return (inputRect.Equals(other.inputRect) && outputDimensions.Equals(other.outputDimensions)
                    && outputFormat.Equals(other.outputFormat) && transformation.Equals(other.transformation));
        }

        public override bool Equals(object obj)
        {
            return (ReferenceEquals(this, obj)
                    || ((obj is XRCameraImageConversionParams) && Equals((XRCameraImageConversionParams)obj)));
        }

        public static bool operator ==(XRCameraImageConversionParams lhs, XRCameraImageConversionParams rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRCameraImageConversionParams lhs, XRCameraImageConversionParams rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return string.Format(
                "inputRect: {0} outputDimensions: {1} format: {2} transformation: {3})",
                inputRect,
                outputDimensions,
                outputFormat,
                transformation);
        }
    }
}
