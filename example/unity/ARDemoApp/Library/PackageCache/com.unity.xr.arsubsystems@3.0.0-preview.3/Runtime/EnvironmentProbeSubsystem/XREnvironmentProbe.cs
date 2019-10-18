using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Encapsulates all of the data provided for an individual environment probe in an AR session.
    /// </summary>
    [StructLayout (LayoutKind.Sequential)]
    public struct XREnvironmentProbe : IEquatable<XREnvironmentProbe>, ITrackable
    {
        /// <summary>
        /// Creates an <see cref="XREnvironmentProbe"/> populated with default values.
        /// </summary>
        public static XREnvironmentProbe defaultValue => s_Default;

        static readonly XREnvironmentProbe s_Default = new XREnvironmentProbe
        {
            trackableId = TrackableId.invalidId,
            pose = Pose.identity
        };

        /// <summary>
        /// Uniquely identifies each environment probe in an AR session.
        /// </summary>
        /// <value>
        /// The unique trackable ID of the environment probe.
        /// </value>
        public TrackableId trackableId
        {
            get => m_TrackableId;
            private set => m_TrackableId = value;
        }
        TrackableId m_TrackableId;

        /// <summary>
        /// Contains the scale of the environment probe in the AR session.
        /// </summary>
        /// <value>
        /// The scale of the environment probe.
        /// </value>
        public Vector3 scale
        {
            get => m_Scale;
            private set => m_Scale = value;
        }
        Vector3 m_Scale;

        /// <summary>
        /// Contains the pose (position and rotation) of the environment probe in the AR session.
        /// </summary>
        /// <value>
        /// The pose (position and rotation) of the environment probe.
        /// </value>
        public Pose pose
        {
            get => m_Pose;
            private set => m_Pose = value;
        }
        Pose m_Pose;

        /// <summary>
        /// Specifies the volume size around the environment probe's position for use when projecting the environment
        /// texture for parallax correction.
        /// </summary>
        /// <value>
        /// The bounding volume size of the environment probe.
        /// </value>
        /// <remarks>
        /// Note that <c>size</c> may validly be infinite.
        /// </remarks>
        public Vector3 size
        {
            get => m_Size;
            private set => m_Size = value;
        }
        Vector3 m_Size;

        /// <summary>
        /// Contains the texture descriptor captured from the device.
        /// </summary>
        /// <value>
        /// The texture descriptor of the environment probe.
        /// </value>
        /// <remarks>
        /// The <c>environmentTextureData</c> value may be invalid indicating that the device has yet to capture an
        /// environment texture for this probe. Newly created environment probes have no environment texture. The
        /// <see cref="XRTextureDescriptor.valid" /> property should be used to determine whether the texture data
        /// is valid.
        /// </remarks>
        public XRTextureDescriptor textureDescriptor
        {
            get => m_TextureDescriptor;
            private set => m_TextureDescriptor = value;
        }
        XRTextureDescriptor m_TextureDescriptor;

        /// <summary>
        /// The <see cref="TrackingState"/> associated with this environment probe.
        /// </summary>
        public TrackingState trackingState
        {
            get => m_TrackingState;
            private set => m_TrackingState = value;
        }
        TrackingState m_TrackingState;

        /// <summary>
        /// A native pointer associated with this environment probe.
        /// The data pointed to by this pointer is implementation-defined. Its lifetime
        /// is also implementation-defined, but will be valid at least until the next
        /// call to <see cref="XREnvironmentProbeSubsystem.GetChanges(Allocator)"/>.
        /// </summary>
        public IntPtr nativePtr
        {
            get => m_NativePtr;
            private set => m_NativePtr = value;
        }
        IntPtr m_NativePtr;

        public bool Equals(XREnvironmentProbe other)
        {
            // Environment probes are equivalent if the trackable IDs match.
            return m_TrackableId.Equals(other.m_TrackableId);
        }

        public override bool Equals(System.Object obj)
        {
            return (!ReferenceEquals(obj, null)
                    && (ReferenceEquals(this, obj)
                        || ((obj is XREnvironmentProbe) && Equals((XREnvironmentProbe)obj))));
        }

        public static bool operator ==(XREnvironmentProbe lhs, XREnvironmentProbe rhs) => lhs.Equals(rhs);

        public static bool operator !=(XREnvironmentProbe lhs, XREnvironmentProbe rhs) => !(lhs == rhs);

        public override int GetHashCode() => m_TrackableId.GetHashCode();

        public override string ToString() => ToString("0.000");

        public string ToString(string floatingPointformat)
        {
            return string.Format("{0} scale:{1} pose:{2} size:{3} environmentTextureData:{4} trackingState:{5} nativePtr:{6}",
                                 m_TrackableId.ToString(), m_Scale.ToString(floatingPointformat),
                                 m_Pose.ToString(floatingPointformat), m_Size.ToString(floatingPointformat),
                                 m_TextureDescriptor.ToString(), m_TrackingState.ToString(), m_NativePtr.ToString());
        }
    }
}
