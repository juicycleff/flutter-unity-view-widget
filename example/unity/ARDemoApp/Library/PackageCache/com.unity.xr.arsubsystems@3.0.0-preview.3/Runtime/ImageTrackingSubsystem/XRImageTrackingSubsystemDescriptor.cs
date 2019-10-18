using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes the capabilities of an <see cref="XRImageTrackingSubsystem"/>.
    /// </summary>
    public class XRImageTrackingSubsystemDescriptor : SubsystemDescriptor<XRImageTrackingSubsystem>
    {
        /// <summary>
        /// Construction information for the <see cref="XRImageTrackingSubsystemDescriptor"/>.
        /// </summary>
        public struct Cinfo : IEquatable<Cinfo>
        {
            /// <summary>
            /// A string identifier used to name the subsystem provider.
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// The <c>System.Type</c> of the provider implementation, used to instantiate the class.
            /// </summary>
            public Type subsystemImplementationType { get; set; }

            /// <summary>
            /// Whether the subsystem supports tracking the poses of moving images in realtime.
            /// </summary>
            /// <remarks>
            /// If <c>true</c>,
            /// <see cref="XRImageTrackingSubsystem.IProvider.maxNumberOfMovingImages"/>
            /// must be implemented.
            /// </remarks>
            public bool supportsMovingImages { get; set; }

            /// <summary>
            /// Whether the subsystem requires physical image dimensions to be provided for all reference images.
            /// If <c>false</c>, specifying the physical dimensions is optional.
            /// </summary>
            public bool requiresPhysicalImageDimensions { get; set; }

            /// <summary>
            /// Whether the subsystem supports image libraries that may be mutated at runtime.
            /// </summary>
            /// <remarks>
            /// If <c>true</c>,
            /// <see cref="XRImageTrackingSubsystem.IProvider.CreateRuntimeLibrary(XRReferenceImageLibrary)"/>
            /// must be implemented and
            /// <see cref="XRImageTrackingSubsystem.IProvider.imageLibrary"/>
            /// will never be called.
            /// </remarks>
            /// <seealso cref="MutableRuntimeReferenceImageLibrary"/>
            public bool supportsMutableLibrary { get; set; }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = id == null ? 0 : id.GetHashCode();
                    hashCode = hashCode * 486187739 + (subsystemImplementationType == null ? 0 : subsystemImplementationType.GetHashCode());
                    hashCode = hashCode * 486187739 + supportsMovingImages.GetHashCode();
                    hashCode = hashCode * 486187739 + requiresPhysicalImageDimensions.GetHashCode();
                    hashCode = hashCode * 486187739 + supportsMutableLibrary.GetHashCode();
                    return hashCode;
                }
            }

            public bool Equals(Cinfo other)
            {
                return
                    (id == other.id) &&
                    (subsystemImplementationType == subsystemImplementationType) &&
                    (supportsMovingImages == other.supportsMovingImages) &&
                    (requiresPhysicalImageDimensions == other.requiresPhysicalImageDimensions) &&
                    (supportsMutableLibrary == other.supportsMutableLibrary);
            }

            public override bool Equals(object obj) => (obj is Cinfo) && Equals((Cinfo)obj);

            public static bool operator==(Cinfo lhs, Cinfo rhs) => lhs.Equals(rhs);

            public static bool operator!=(Cinfo lhs, Cinfo rhs) => !lhs.Equals(rhs);
        }

        /// <summary>
        /// Whether the subsystem supports tracking the poses of moving images in realtime.
        /// </summary>
        public bool supportsMovingImages { get; private set; }

        /// <summary>
        /// Whether the subsystem requires physical image dimensions to be provided for all reference images.
        /// If <c>false</c>, specifying the physical dimensions is optional.
        /// </summary>
        public bool requiresPhysicalImageDimensions { get; private set; }

        /// <summary>
        /// Whether the subsystem supports <see cref="MutableRuntimeReferenceImageLibrary"/>, a reference
        /// image library which can modified at runtime, as opposed to the <see cref="XRReferenceImageLibrary"/>,
        /// which is generated at edit time and cannot be modified at runtime.
        /// </summary>
        /// <seealso cref="MutableRuntimeReferenceImageLibrary"/>
        /// <seealso cref="XRImageTrackingSubsystem.CreateRuntimeLibrary(XRReferenceImageLibrary)"/>
        public bool supportsMutableLibrary { get; private set; }

        /// <summary>
        /// Registers a new descriptor with the <c>SubsystemManager</c>.
        /// </summary>
        /// <param name="cinfo">The construction information for the new descriptor.</param>
        public static void Create(Cinfo cinfo)
        {
            SubsystemRegistration.CreateDescriptor(new XRImageTrackingSubsystemDescriptor(cinfo));
        }

        XRImageTrackingSubsystemDescriptor(Cinfo cinfo)
        {
            this.id = cinfo.id;
            this.subsystemImplementationType = cinfo.subsystemImplementationType;
            this.supportsMovingImages = cinfo.supportsMovingImages;
            this.requiresPhysicalImageDimensions = cinfo.requiresPhysicalImageDimensions;
            this.supportsMutableLibrary = cinfo.supportsMutableLibrary;
        }
    }
}
