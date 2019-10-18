using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Encapsulates the parameters for creating a new <see cref="XRCameraSubsystemDescriptor"/>.
    /// </summary>
    public struct XRCameraSubsystemCinfo : IEquatable<XRCameraSubsystemCinfo>
    {
        /// <summary>
        /// Specifies an identifier for the provider implementation of the subsystem.
        /// </summary>
        /// <value>
        /// The identifier for the provider implementation of the subsystem.
        /// </value>
        public string id { get; set; }

        /// <summary>
        /// Specifies the provider implementation type to use for instantiation.
        /// </summary>
        /// <value>
        /// The provider implementation type to use for instantiation.
        /// </value>
        public Type implementationType { get; set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide average brightness.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide average brightness. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsAverageBrightness { get; set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide average camera temperature.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide average camera temperature. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsAverageColorTemperature { get; set; }

        /// <summary>
        /// True if color correction is supported.
        /// </summary>
        public bool supportsColorCorrection { get; set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide display matrix.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide display matrix. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsDisplayMatrix { get; set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide projection matrix.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide projection matrix. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsProjectionMatrix { get; set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide timestamp.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide timestamp. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsTimestamp { get; set; }

        /// <summary>
        /// Specifies if the current subsystem supports camera configurations.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports camera configurations. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsCameraConfigurations { get; set; }

        /// <summary>
        /// Specifies if the current subsystem is allowed to provide camera images.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem is allowed to provide camera images. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsCameraImage { get; set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide average intensity in lumens.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide average intensity in lumens. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsAverageIntensityInLumens { get; set; }

        public bool Equals(XRCameraSubsystemCinfo other)
        {
            return (id.Equals(other.id) && implementationType.Equals(other.implementationType)
                    && supportsAverageBrightness.Equals(other.supportsAverageBrightness)
                    && supportsAverageColorTemperature.Equals(other.supportsAverageColorTemperature)
                    && supportsDisplayMatrix.Equals(other.supportsDisplayMatrix)
                    && supportsProjectionMatrix.Equals(other.supportsProjectionMatrix)
                    && supportsTimestamp.Equals(other.supportsTimestamp)
                    && supportsCameraConfigurations.Equals(other.supportsCameraConfigurations)
                    && supportsCameraImage.Equals(other.supportsCameraImage)
                    && supportsAverageIntensityInLumens.Equals(other.supportsAverageIntensityInLumens));
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is XRCameraSubsystemCinfo) && Equals((XRCameraSubsystemCinfo)obj));
        }

        public static bool operator ==(XRCameraSubsystemCinfo lhs, XRCameraSubsystemCinfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRCameraSubsystemCinfo lhs, XRCameraSubsystemCinfo rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + id.GetHashCode();
                hashCode = (hashCode * 486187739) + implementationType.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsAverageBrightness.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsAverageColorTemperature.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsDisplayMatrix.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsProjectionMatrix.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsTimestamp.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsCameraConfigurations.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsCameraImage.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsAverageIntensityInLumens.GetHashCode();
            }
            return hashCode;
        }
    }

    /// <summary>
    /// Specifies a functionality description that may be registered for each implementation that provides the
    /// <see cref="XRCameraSubsystem"/> interface.
    /// </summary>
    public sealed class XRCameraSubsystemDescriptor : SubsystemDescriptor<XRCameraSubsystem>
    {
        /// <summary>
        /// Constructs a <c>XRCameraSubsystemDescriptor</c> based on the given parameters.
        /// </summary>
        /// <param name="cameraSubsystemParams">The parameters required to initialize the descriptor.</param>
        XRCameraSubsystemDescriptor(XRCameraSubsystemCinfo cameraSubsystemParams)
        {
            id = cameraSubsystemParams.id;
            subsystemImplementationType = cameraSubsystemParams.implementationType;
            supportsAverageBrightness = cameraSubsystemParams.supportsAverageBrightness;
            supportsAverageColorTemperature = cameraSubsystemParams.supportsAverageColorTemperature;
            supportsDisplayMatrix = cameraSubsystemParams.supportsDisplayMatrix;
            supportsProjectionMatrix = cameraSubsystemParams.supportsProjectionMatrix;
            supportsTimestamp = cameraSubsystemParams.supportsTimestamp;
            supportsCameraConfigurations = cameraSubsystemParams.supportsCameraConfigurations;
            supportsCameraImage = cameraSubsystemParams.supportsCameraImage;
            supportsAverageIntensityInLumens = cameraSubsystemParams.supportsAverageIntensityInLumens;
        }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide average brightness.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide average brightness. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsAverageBrightness { get; private set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide average camera temperature.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide average camera temperature. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsAverageColorTemperature { get; private set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide display matrix.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide display matrix. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsDisplayMatrix { get; private set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide projection matrix.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide projection matrix. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsProjectionMatrix { get; private set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide timestamp.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide timestamp. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsTimestamp { get; private set; }

        /// <summary>
        /// Specifies if the current subsystem supports camera configurations.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports camera configurations. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsCameraConfigurations { get; private set; }

        /// <summary>
        /// Specifies if the current subsystem is allowed to provide camera images.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem is allowed to provide camera images. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsCameraImage { get; private set; }

        /// <summary>
        /// Specifies if current subsystem is allowed to provide average intensity in lumens.
        /// </summary>
        /// <value>
        /// <c>true</c> if current subsystem is allowed to provide average intensity in lumens. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsAverageIntensityInLumens { get; private set; }

        /// <summary>
        /// Creates a <c>XRCameraSubsystemDescriptor</c> based on the given parameters validating that the
        /// <see cref="XRCameraSubsystemCinfo.id"/> and <see cref="XRCameraSubsystemCinfo.implementationType"/>
        /// properties are properly specified.
        /// </summary>
        /// <param name="cameraSubsystemParams">The parameters defining how to initialize the descriptor.</param>
        /// <returns>
        /// The created <c>XRCameraSubsystemDescriptor</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown when the values specified in the
        /// <see cref="XRCameraSubsystemCinfo"/> parameter are invalid. Typically, this will occur
        /// <list type="bullet">
        /// <item>
        /// <description>if <see cref="XRCameraSubsystemCinfo.id"/> is <c>null</c> or empty</description>
        /// </item>
        /// <item>
        /// <description>if <see cref="XRCameraSubsystemCinfo.implementationType"/> is <c>null</c></description>
        /// </item>
        /// <item>
        /// <description>if <see cref="XRCameraSubsystemCinfo.implementationType"/> does not derive from the
        /// <see cref="XRCameraSubsystem"/> class
        /// </description>
        /// </item>
        /// </list>
        /// </exception>
        internal static XRCameraSubsystemDescriptor Create(XRCameraSubsystemCinfo cameraSubsystemParams)
        {
            if (String.IsNullOrEmpty(cameraSubsystemParams.id))
            {
                throw new ArgumentException("Cannot create camera subsystem descriptor because id is invalid",
                                            "cameraSubsystemParams");
            }

            if ((cameraSubsystemParams.implementationType == null)
                || !cameraSubsystemParams.implementationType.IsSubclassOf(typeof(XRCameraSubsystem)))
            {
                throw new ArgumentException("Cannot create camera subsystem descriptor because implementationType is invalid",
                                            "cameraSubsystemParams");
            }

            return new XRCameraSubsystemDescriptor(cameraSubsystemParams);
        }
    }
}
