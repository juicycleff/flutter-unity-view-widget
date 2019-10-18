using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes the capabilities of an <see cref="XRReferencePointSubsystem"/>.
    /// </summary>
    public class XRReferencePointSubsystemDescriptor : SubsystemDescriptor<XRReferencePointSubsystem>
    {
        /// <summary>
        /// <c>true</c> if the subsystem supports attachments, i.e., the ability to attach a reference point to a trackable.
        /// </summary>
        public bool supportsTrackableAttachments { get; private set; }

        /// <summary>
        /// Constructor info used to register a descriptor.
        /// </summary>
        public struct Cinfo : IEquatable<Cinfo>
        {
            /// <summary>
            /// The string identifier for this subsystem.
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// The concrete <c>Type</c> of the subsystem which will be instantiated if a subsystem is created from this descriptor.
            /// </summary>
            public Type subsystemImplementationType { get; set; }

            /// <summary>
            /// <c>true</c> if the subsystem supports attachments, i.e., the ability to attach a reference point to a trackable.
            /// </summary>
            public bool supportsTrackableAttachments { get; set; }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = (id == null) ? 0 : id.GetHashCode();
                    hash = hash * 486187739 + ((subsystemImplementationType == null) ? 0 : subsystemImplementationType.GetHashCode());
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Cinfo))
                    return false;

                return Equals((Cinfo)obj);
            }

            public bool Equals(Cinfo other)
            {
                return
                    String.Equals(id, other.id) &&
                    subsystemImplementationType == other.subsystemImplementationType;
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

        /// <summary>
        /// Creates a new subsystem descriptor and registers it with the <c>SubsystemManager</c>.
        /// </summary>
        /// <param name="cinfo">Constructor info describing the descriptor to create.</param>
        public static void Create(Cinfo cinfo)
        {
            SubsystemRegistration.CreateDescriptor(new XRReferencePointSubsystemDescriptor(cinfo));
        }

        XRReferencePointSubsystemDescriptor(Cinfo cinfo)
        {
            id = cinfo.id;
            subsystemImplementationType = cinfo.subsystemImplementationType;
            supportsTrackableAttachments = cinfo.supportsTrackableAttachments;
        }
    }
}
