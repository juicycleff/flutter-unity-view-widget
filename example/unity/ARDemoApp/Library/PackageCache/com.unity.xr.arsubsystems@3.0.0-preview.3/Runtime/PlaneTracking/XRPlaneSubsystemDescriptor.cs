using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Describes the capabilities of an <see cref="XRPlaneSubsystem"/>.
    /// </summary>
    public class XRPlaneSubsystemDescriptor : SubsystemDescriptor<XRPlaneSubsystem>
    {
        /// <summary>
        /// <c>true</c> if the subsystem supports horizontal plane detection.
        /// </summary>
        public bool supportsHorizontalPlaneDetection { get; private set; }

        /// <summary>
        /// <c>true</c> if the subsystem supports vertical plane detection.
        /// </summary>
        public bool supportsVerticalPlaneDetection { get; private set; }

        /// <summary>
        /// <c>true</c> if the subsystem supports arbitrarily angled plane detection.
        /// </summary>
        public bool supportsArbitraryPlaneDetection { get; private set; }

        /// <summary>
        /// <c>true</c> if the subsystem supports boundary vertices for its planes.
        /// </summary>
        public bool supportsBoundaryVertices { get; private set; }

        /// <summary>
        /// Constructor info used to register a descriptor.
        /// </summary>
        public struct Cinfo : IEquatable<Cinfo>
        {
            /// <summary>
            /// The string identifier for a specific implementation.
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// The concrete <c>Type</c> which will be instantiated if <c>Create</c> is called on this subsystem descriptor.
            /// </summary>
            public Type subsystemImplementationType { get; set; }

            /// <summary>
            /// <c>true</c> if the subsystem supports horizontal plane detection.
            /// </summary>
            public bool supportsHorizontalPlaneDetection { get; set; }

            /// <summary>
            /// <c>true</c> if the subsystem supports vertical plane detection.
            /// </summary>
            public bool supportsVerticalPlaneDetection { get; set; }

            /// <summary>
            /// <c>true</c> if the subsystem supports arbitrarily angled plane detection.
            /// </summary>
            public bool supportsArbitraryPlaneDetection { get; set; }

            /// <summary>
            /// <c>true</c> if the subsystem supports boundary vertices for its planes.
            /// </summary>
            public bool supportsBoundaryVertices { get; set; }

            public bool Equals(Cinfo other)
            {
                return
                    id.Equals(other.id) &&
                    (subsystemImplementationType == other.subsystemImplementationType) &&
                    (supportsHorizontalPlaneDetection == other.supportsHorizontalPlaneDetection) &&
                    (supportsVerticalPlaneDetection == other.supportsVerticalPlaneDetection) &&
                    (supportsArbitraryPlaneDetection == other.supportsArbitraryPlaneDetection) &&
                    (supportsBoundaryVertices == other.supportsBoundaryVertices);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Cinfo))
                    return false;

                return Equals((Cinfo)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = id.GetHashCode();
                    hashCode = (hashCode * 486187739) + subsystemImplementationType.GetHashCode();
                    hashCode = (hashCode * 486187739) + supportsHorizontalPlaneDetection.GetHashCode();
                    hashCode = (hashCode * 486187739) + supportsVerticalPlaneDetection.GetHashCode();
                    hashCode = (hashCode * 486187739) + supportsArbitraryPlaneDetection.GetHashCode();
                    hashCode = (hashCode * 486187739) + supportsBoundaryVertices.GetHashCode();
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

        /// <summary>
        /// Creates a new subsystem descriptor and registers it with the <c>SubsystemManager</c>.
        /// </summary>
        /// <param name="cinfo">Construction info for the descriptor.</param>
        public static void Create(Cinfo cinfo)
        {
            var descriptor = new XRPlaneSubsystemDescriptor(cinfo);
            SubsystemRegistration.CreateDescriptor(descriptor);
        }

        XRPlaneSubsystemDescriptor(Cinfo cinfo)
        {
            id = cinfo.id;
            subsystemImplementationType = cinfo.subsystemImplementationType;
            supportsHorizontalPlaneDetection = cinfo.supportsHorizontalPlaneDetection;
            supportsVerticalPlaneDetection = cinfo.supportsVerticalPlaneDetection;
            supportsArbitraryPlaneDetection = cinfo.supportsArbitraryPlaneDetection;
            supportsBoundaryVertices = cinfo.supportsBoundaryVertices;
        }
    }
}
