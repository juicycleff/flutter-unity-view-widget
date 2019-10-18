using Unity.Collections;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// Base class for subsystems that detect and track things in the physical environment.
    /// </summary>
    /// <typeparam name="TTrackable">The trackable's data, often a blittable type to interop with native code.</typeparam>
    /// <typeparam name="TSubsystemDescriptor">The subsystem descriptor for the underlying subsystem</typeparam>
    public abstract class TrackingSubsystem<TTrackable, TSubsystemDescriptor> : XRSubsystem<TSubsystemDescriptor>
        where TTrackable : struct, ITrackable
        where TSubsystemDescriptor : class, ISubsystemDescriptor
    {
        /// <summary>
        /// Retrieves a set of changes (additions, updates, and removals) since the last
        /// time <see cref="GetChanges(Allocator)"/> was called. This is typically called
        /// once per frame to update the derived class's internal state.
        /// </summary>
        /// <param name="allocator">The <c>Allocator</c> to use when creating the <c>NativeArray</c>s in <see cref="TrackableChanges{T}"/>.</param>
        /// <returns>The set of changes since the last time this method was invoked.</returns>
        public abstract TrackableChanges<TTrackable> GetChanges(Allocator allocator);
    }
}
