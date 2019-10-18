using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Descriptor for the <see cref="XRRaycastSubsystem"/>. Describes capabilities of a specific raycast provider.
    /// </summary>
    public sealed class XRRaycastSubsystemDescriptor : SubsystemDescriptor<XRRaycastSubsystem>
    {
        /// <summary>
        /// Used to register a descriptor. See <see cref="RegisterDescriptor(Cinfo)"/>.
        /// </summary>
        public struct Cinfo : IEquatable<Cinfo>
        {
            /// <summary>
            /// A provider-specific identifier.
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// The <c>Type</c> of the subsystem.
            /// </summary>
            public Type subsystemImplementationType { get; set; }

            /// <summary>
            /// Whether the provider supports casting a ray from a screen point.
            /// </summary>
            public bool supportsViewportBasedRaycast { get; set; }

            /// <summary>
            /// Whether the provider supports casting an arbitrary ray.
            /// </summary>
            public bool supportsWorldBasedRaycast { get; set; }

            /// <summary>
            /// The types of trackables against which raycasting is supported.
            /// </summary>
            public TrackableType supportedTrackableTypes { get; set; }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = (id == null) ? 0 : id.GetHashCode();
                    hash = hash * 486187739 + ((subsystemImplementationType == null) ? 0 : subsystemImplementationType.GetHashCode());
                    hash = hash * 486187739 + supportsViewportBasedRaycast.GetHashCode();
                    hash = hash * 486187739 + supportsWorldBasedRaycast.GetHashCode();
                    hash = hash * 486187739 + supportedTrackableTypes.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Cinfo))
                    return false;

                return Equals((Cinfo)obj);
            }

            public override string ToString()
            {
                return string.Format("XRRaycastSubsystemDescriptor:\nsupportsViewportBasedRaycast: {0}\nsupportsWorldBasedRaycast: {1}",
                    supportsViewportBasedRaycast, supportsWorldBasedRaycast);
            }

            public bool Equals(Cinfo other)
            {
                return
                    String.Equals(id, other.id) &&
                    (subsystemImplementationType == other.subsystemImplementationType) &&
                    (supportsViewportBasedRaycast == other.supportsViewportBasedRaycast) &&
                    (supportsWorldBasedRaycast == other.supportsWorldBasedRaycast) &&
                    (supportedTrackableTypes == other.supportedTrackableTypes);
            }

            public static bool operator ==(Cinfo lhs, Cinfo rhs) { return lhs.Equals(rhs); }

            public static bool operator !=(Cinfo lhs, Cinfo rhs) { return !lhs.Equals(rhs); }
        }

        /// <summary>
        /// Whether the provider supports casting a ray from a screen point.
        /// </summary>
        public bool supportsViewportBasedRaycast { get; private set; }

        /// <summary>
        /// Whether the provider supports casting an arbitrary ray.
        /// </summary>
        public bool supportsWorldBasedRaycast { get; private set; }

        /// <summary>
        /// The types of trackables against which raycasting is supported.
        /// </summary>
        public TrackableType supportedTrackableTypes { get; private set; }

        /// <summary>
        /// Registers a new descriptor. Should be called by provider implementations.
        /// </summary>
        /// <param name="cinfo"></param>
        public static void RegisterDescriptor(Cinfo cinfo)
        {
            SubsystemRegistration.CreateDescriptor(new XRRaycastSubsystemDescriptor(cinfo));
        }

        XRRaycastSubsystemDescriptor(Cinfo cinfo)
        {
            id = cinfo.id;
            subsystemImplementationType = cinfo.subsystemImplementationType;
            supportsViewportBasedRaycast = cinfo.supportsViewportBasedRaycast;
            supportsWorldBasedRaycast = cinfo.supportsWorldBasedRaycast;
            supportedTrackableTypes = cinfo.supportedTrackableTypes;
        }
    }
}
