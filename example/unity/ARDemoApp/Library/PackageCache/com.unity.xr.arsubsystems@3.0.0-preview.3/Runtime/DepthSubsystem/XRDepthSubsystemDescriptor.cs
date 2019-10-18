using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// The descriptor of the <see cref="XRDepthSubsystem"/> that shows which depth detectiomn features are available on that XRSubsystem.
    /// </summary>
    /// <remarks>
    /// You use the <c>Create<c> factory method along with <see cref="DepthSubsystemParams"/> struct to construct and
    /// register one of these from each depth data provider.
    /// </remarks>
    /// <seealso cref="XRDepthSubsystem"/>
    public class XRDepthSubsystemDescriptor : SubsystemDescriptor<XRDepthSubsystem>
    {
        /// <summary>
        /// Describes the capabilities of an <see cref="XRDepthSubsystem"/>.
        /// </summary>
        [Flags]
        public enum Capabilities
        {
            None = 0,
            FeaturePoints = 1 << 0,
            Confidence = 1 << 1,
            UniqueIds = 1 << 2
        }

        /// <summary>
        /// This struct is an initializer for the creation of a <see cref="XRDepthSubsystemDescriptor"/>.
        /// </summary>
        /// <remarks>
        /// Depth data provider should create during <c>InitializeOnLoad<c> a descriptor using
        /// the params here to specify which of the XRDepthSubsystem features it supports.
        /// </remarks>
        public struct Cinfo : IEquatable<Cinfo>
        {
            /// <summary>
            /// The string identifier for a specific implementation.
            /// </summary>
            public string id;

            /// <summary>
            /// The concrete <c>Type</c> which will be instantiated if <c>Create</c> is called on this subsystem descriptor.
            /// </summary>
            public Type implementationType;

            /// <summary>
            /// Whether the subsystem supports feature points
            /// </summary>
            public bool supportsFeaturePoints
            {
                get { return (capabilities & Capabilities.FeaturePoints) != 0; }
                set
                {
                    if (value)
                    {
                        capabilities |= Capabilities.FeaturePoints;
                    }
                    else
                    {
                        capabilities &= ~Capabilities.FeaturePoints;
                    }
                }
            }

            /// <summary>
            /// Whether the subsystem supports per feature point confidence values.
            /// </summary>
            public bool supportsConfidence
            {
                get { return (capabilities & Capabilities.Confidence) != 0; }
                set
                {
                    if (value)
                    {
                        capabilities |= Capabilities.Confidence;
                    }
                    else
                    {
                        capabilities &= ~Capabilities.Confidence;
                    }
                }
            }

            /// <summary>
            /// Whether the subsystem supports per-feature point identifiers.
            /// </summary>
            public bool supportsUniqueIds
            {
                get { return (capabilities & Capabilities.UniqueIds) != 0; }
                set
                {
                    if (value)
                    {
                        capabilities |= Capabilities.UniqueIds;
                    }
                    else
                    {
                        capabilities &= ~Capabilities.UniqueIds;
                    }
                }
            }

            /// <summary>
            /// The capabilities of the subsystem implementation.
            /// </summary>
            Capabilities capabilities { get; set; }

            //IEquatable boilerplate
            public bool Equals(Cinfo other)
            {
                return capabilities == other.capabilities && id.Equals(other.id) && implementationType == other.implementationType;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Cinfo))
                {
                    return false;
                }

                return Equals((Cinfo)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = id.GetHashCode();
                    hashCode = (hashCode * 486187739) + implementationType.GetHashCode();
                    hashCode = (hashCode * 486187739) + ((int)capabilities).GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(Cinfo lhs, Cinfo rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Cinfo lhs, Cinfo rhs)
            {
                return !lhs.Equals(rhs);
            }
        }

        XRDepthSubsystemDescriptor(Cinfo descriptorParams)
        {
            id = descriptorParams.id;
            subsystemImplementationType = descriptorParams.implementationType;
            supportsFeaturePoints = descriptorParams.supportsFeaturePoints;
            supportsUniqueIds = descriptorParams.supportsUniqueIds;
            supportsConfidence = descriptorParams.supportsConfidence;
        }

        /// <summary>
        /// Whether the implementation supports feature points.
        /// </summary>
        public bool supportsFeaturePoints { get; private set; }

        /// <summary>
        /// Whether the implementation supports per feature point identifiers.
        /// </summary>
        public bool supportsUniqueIds { get; private set; }

        /// <summary>
        /// Whether the implementation supports per feature point confidence values.
        /// </summary>
        public bool supportsConfidence { get; private set; }

        /// <summary>
        /// Registers a subsystem implementation with the <c>SubsystemManager</c>.
        /// </summary>
        /// <param name="descriptorParams"></param>
        public static void RegisterDescriptor(Cinfo descriptorParams)
        {
            var descriptor = new XRDepthSubsystemDescriptor(descriptorParams);
            SubsystemRegistration.CreateDescriptor(descriptor);
        }
    }
}
