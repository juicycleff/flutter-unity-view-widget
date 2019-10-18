using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A structure for camera-related information pertaining to a particular frame.
    /// This is used to communicate information in the <see cref="ARSubsystemManager.cameraFrameReceived" /> event.
    /// </summary>
    public struct ARCameraFrameEventArgs : IEquatable<ARCameraFrameEventArgs>
    {
        /// <summary>
        /// The <see cref="LightEstimationData" /> associated with this frame.
        /// </summary>
        public ARLightEstimationData lightEstimation { get; set; }

        /// <summary>
        /// The time, in nanoseconds, associated with this frame.
        /// Use <c>timestampNs.HasValue</c> to determine if this data is available.
        /// </summary>
        public long? timestampNs { get; set; }

        /// <summary>
        /// Gets or sets the projection matrix for the AR Camera. Use
        /// <c>projectionMatrix.HasValue</c> to determine if this data is available.
        /// </summary>
        public Matrix4x4? projectionMatrix { get; set; }

        /// <summary>
        /// Gets or sets the display matrix for use in the shader used
        /// by the <see cref="ARFoundationBackgroundRenderer"/>.
        /// Use <c>displayMatrix.HasValue</c> to determine if this data is available.
        /// </summary>
        public Matrix4x4? displayMatrix { get; set; }

        /// <summary>
        /// The textures associated with this camera frame. These are generally
        /// external textures, which exist only on the GPU. To use them on the
        /// CPU, e.g., for computer vision processing, you will need to read
        /// them back from the GPU.
        /// </summary>
        public List<Texture2D> textures { get; set; }

        /// <summary>
        /// Ids of the property name associated with each texture. This is a
        /// parallel <c>List</c> to the <see cref="textures"/> list.
        /// </summary>
        public List<int> propertyNameIds { get; set; }

        /// <summary>
        /// The exposure duration in seconds with sub-millisecond precision.  Utilized in calculating motion blur.
        /// </summary>
        /// <remarks>
        /// <see cref="exposureDuration"/> may be null if platform does not support exposure duration.
        /// </remarks>
        public double? exposureDuration { get; set; }

        /// <summary>
        /// The offset of camera exposure.  Used to scale scene lighting in post-processed lighting stage.
        /// </summary>
        /// <remarks>
        /// <see cref="exposureOffset"/> may be null if platform does not support exposure offset.
        /// </remarks>
        public float? exposureOffset { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = lightEstimation.GetHashCode();
                hash = hash * 486187739 + timestampNs.GetHashCode();
                hash = hash * 486187739 + projectionMatrix.GetHashCode();
                hash = hash * 486187739 + displayMatrix.GetHashCode();
                hash = hash * 486187739 + (textures == null ? 0 : textures.GetHashCode());
                hash = hash * 486187739 + (propertyNameIds == null ? 0 : propertyNameIds.GetHashCode());
                hash = hash * 486187739 + exposureDuration.GetHashCode();
                hash = hash * 486187739 + exposureOffset.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ARCameraFrameEventArgs))
                return false;

            return Equals((ARCameraFrameEventArgs)obj);
        }

        /// <summary>
        /// Generates a string representation of this struct suitable for debug
        /// logging.
        /// </summary>
        /// <returns>A string representation of this struct suitable for debug
        /// logging.</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("lightEstimation: " + lightEstimation.ToString());
            stringBuilder.Append("\ntimestamp: "  + timestampNs);
            if (timestampNs.HasValue)
                stringBuilder.Append("ns");
            stringBuilder.Append("\nprojectionMatrix: " + projectionMatrix);
            stringBuilder.Append("\ndisplayMatrix: " + displayMatrix);
            stringBuilder.Append("\ntexture count: " + (textures == null ? 0 : textures.Count));
            stringBuilder.Append("\npropertyNameId count: " + (propertyNameIds == null ? 0 : propertyNameIds.Count));

            return stringBuilder.ToString();
        }

        public bool Equals(ARCameraFrameEventArgs other)
        {
            return
                lightEstimation.Equals(other.lightEstimation)
                && timestampNs.Equals(other.timestampNs)
                && projectionMatrix.Equals(other.projectionMatrix)
                && displayMatrix.Equals(other.displayMatrix)
                && textures.Equals(other.textures)
                && propertyNameIds.Equals(other.propertyNameIds) 
                && (exposureDuration.Equals(other.exposureDuration))
                && (exposureOffset.Equals(other.exposureOffset));
        }

        public static bool operator ==(ARCameraFrameEventArgs lhs, ARCameraFrameEventArgs rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ARCameraFrameEventArgs lhs, ARCameraFrameEventArgs rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
