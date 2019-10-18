using System;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// The descriptor of the <see cref="XRParticipantSubsystem"/> that shows which features are available on that XRSubsystem.
    /// </summary>
    /// <remarks>
    /// You use <see cref="Register{T}"/> register a subsystem with the global <c>SubsystemManager</c>.
    /// </remarks>
    public sealed class XRParticipantSubsystemDescriptor : SubsystemDescriptor<XRParticipantSubsystem>
    {
        /// <summary>
        /// The capabilities of a particular <see cref="XRParticipantSubsystem"/>. This is typically
        /// used to query a subsystem for capabilities that may vary by platform or implementation.
        /// </summary>
        [Flags]
        public enum Capabilities
        {
            /// <summary>
            /// The <see cref="XRParticipantSubsystem"/> implementation has no
            /// additional capabilities other than the basic, required functionality.
            /// </summary>
            None = 0,
        }

        /// <summary>
        /// The capabilities provided by this implementation.
        /// </summary>
        public Capabilities capabilities { get; private set; }

        /// <summary>
        /// Register a subsystem implementation.
        /// This should only be used by subsystem implementors.
        /// </summary>
        /// <param name="subsystemId">The name of the specific subsystem implementation.</param>
        /// <param name="capabilities">The <see cref="Capabilities"/> of the specific subsystem implementation.</param>
        public static void Register<T>(string subsystemId, Capabilities capabilities) where T : XRParticipantSubsystem
        {
            SubsystemRegistration.CreateDescriptor(new XRParticipantSubsystemDescriptor(subsystemId, typeof(T), capabilities));
        }

        XRParticipantSubsystemDescriptor(string subsystemId, Type subsystemType, Capabilities capabilities)
        {
            id = subsystemId;
            subsystemImplementationType = subsystemType;
            this.capabilities = capabilities;
        }
    }
}
