using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes features of an <see cref="XRObjectTrackingSubsystem"/>.
    /// </summary>
    /// <remarks>
    /// Enumerate available subsystems with <c>SubsystemManager.GetSubsystemDescriptors</c> and instantiate one by calling
    /// <c>Create</c> on one of the descriptors.
    /// Subsystem implementors can register their subsystem with
    /// <see cref="XRObjectTrackingSubsystem.Register{T}(string, XRObjectTrackingSubsystemDescriptor.Capabilities)"/>.
    /// </remarks>
    public class XRObjectTrackingSubsystemDescriptor : SubsystemDescriptor<XRObjectTrackingSubsystem>
    {
        /// <summary>
        /// Describes the capabilities of an <see cref="XRObjectTrackingSubsystem"/> implementation.
        /// </summary>
        public Capabilities capabilities { get; private set; }

        /// <summary>
        /// Describes the capabilities of an <see cref="XRObjectTrackingSubsystem"/> implementation.
        /// </summary>
        public struct Capabilities : IEquatable<Capabilities>
        {
            public bool Equals(Capabilities other)
            {
                return true;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Capabilities))
                    return false;

                return Equals((Capabilities)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return 0;
                }
            }

            public static bool operator ==(Capabilities lhs, Capabilities rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Capabilities lhs, Capabilities rhs)
            {
                return !lhs.Equals(rhs);
            }
        }

        internal XRObjectTrackingSubsystemDescriptor(string id, Type implementationType, Capabilities capabilities)
        {
            this.id = id;
            this.subsystemImplementationType = implementationType;
            this.capabilities = capabilities;
        }
    }
}
