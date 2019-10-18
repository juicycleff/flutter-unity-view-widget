using System;

namespace UnityEngine.XR.ARSubsystems
{
    public struct XRHumanBodySubsystemCinfo : IEquatable<XRHumanBodySubsystemCinfo>
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
        /// Specifies the provider implementation type to use for instantiation.
        /// </value>
        public Type implementationType { get; set; }

        /// <summary>
        /// Specifies if the current subsystem supports 2D human body pose estimation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports 2D human body pose estimation. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanBody2D { get; set; }

        /// <summary>
        /// Specifies if the current subsystem supports 3D human body pose estimation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports 3D human body pose estimation. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanBody3D { get; set; }

        /// <summary>
        /// Specifies if the current subsystem supports 3D human body scale estimation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports 3D human body scale estimation. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanBody3DScaleEstimation { get; set; }

        /// <summary>
        /// Specifies if the current subsystem is allowed to provide human stencil images.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem is allowed to provide human stencil images. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanStencilImage { get; set; }

        /// <summary>
        /// Specifies if the current subsystem is allowed to provide human depth images.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem is allowed to provide human depth images. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanDepthImage { get; set; }

        public bool Equals(XRHumanBodySubsystemCinfo other)
        {
            return (id.Equals(other.id) && implementationType.Equals(other.implementationType)
                    && supportsHumanBody2D.Equals(other.supportsHumanBody2D)
                    && supportsHumanBody3D.Equals(other.supportsHumanBody3D)
                    && supportsHumanBody3DScaleEstimation.Equals(other.supportsHumanBody3DScaleEstimation)
                    && supportsHumanStencilImage.Equals(other.supportsHumanStencilImage)
                    && supportsHumanDepthImage.Equals(other.supportsHumanDepthImage));
        }

        public override bool Equals(System.Object obj)
        {
            return ((obj is XRHumanBodySubsystemCinfo) && Equals((XRHumanBodySubsystemCinfo)obj));
        }

        public static bool operator ==(XRHumanBodySubsystemCinfo lhs, XRHumanBodySubsystemCinfo rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(XRHumanBodySubsystemCinfo lhs, XRHumanBodySubsystemCinfo rhs)
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
                hashCode = (hashCode * 486187739) + supportsHumanBody2D.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsHumanBody3D.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsHumanBody3DScaleEstimation.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsHumanStencilImage.GetHashCode();
                hashCode = (hashCode * 486187739) + supportsHumanDepthImage.GetHashCode();
            }
            return hashCode;
        }
    }

    public class XRHumanBodySubsystemDescriptor : SubsystemDescriptor<XRHumanBodySubsystem>
    {
        XRHumanBodySubsystemDescriptor(XRHumanBodySubsystemCinfo humanBodySubsystemCinfo)
        {
            id = humanBodySubsystemCinfo.id;
            subsystemImplementationType = humanBodySubsystemCinfo.implementationType;
            supportsHumanBody2D = humanBodySubsystemCinfo.supportsHumanBody2D;
            supportsHumanBody3D = humanBodySubsystemCinfo.supportsHumanBody3D;
            supportsHumanBody3DScaleEstimation = humanBodySubsystemCinfo.supportsHumanBody3DScaleEstimation;
            supportsHumanStencilImage = humanBodySubsystemCinfo.supportsHumanStencilImage;
            supportsHumanDepthImage = humanBodySubsystemCinfo.supportsHumanDepthImage;
        }

        /// <summary>
        /// Specifies if the current subsystem supports 2D human body pose estimation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports 2D human body pose estimation. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanBody2D { get; private set; }

        /// <summary>
        /// Specifies if the current subsystem supports 3D human body pose estimation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports 3D human body pose estimation. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanBody3D { get; private set; }

        /// <summary>
        /// Specifies if the current subsystem supports 3D human body scale estimation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem supports 3D human body scale estimation. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanBody3DScaleEstimation { get; private set; }

        /// <summary>
        /// Specifies if the current subsystem is allowed to provide human stencil images.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem is allowed to provide human stencil images. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanStencilImage { get; private set; }

        /// <summary>
        /// Specifies if the current subsystem is allowed to provide human depth images.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current subsystem is allowed to provide human depth images. Otherwise, <c>false</c>.
        /// </value>
        public bool supportsHumanDepthImage { get; private set; }

        internal static XRHumanBodySubsystemDescriptor Create(XRHumanBodySubsystemCinfo humanBodySubsystemCinfo)
        {
            if (String.IsNullOrEmpty(humanBodySubsystemCinfo.id))
            {
                throw new ArgumentException("Cannot create human body subsystem descriptor because id is invalid",
                                            "humanBodySubsystemCinfo");
            }

            if ((humanBodySubsystemCinfo.implementationType == null)
                || !humanBodySubsystemCinfo.implementationType.IsSubclassOf(typeof(XRHumanBodySubsystem)))
            {
                throw new ArgumentException("Cannot create human body subsystem descriptor because implementationType is invalid",
                                            "humanBodySubsystemCinfo");
            }

            return new XRHumanBodySubsystemDescriptor(humanBodySubsystemCinfo);
        }
    }
}
