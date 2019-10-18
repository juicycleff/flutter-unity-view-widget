namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A provider-specific reference object.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A reference object represents a 3D scan of a real object that can
    /// be recognized in the environment. Each <see cref="XRReferenceObject"/>
    /// contains a list of provider-specific <see cref="XRReferenceObjectEntry"/>.
    /// Each provider (implementation of <see cref="XRObjectTrackingSubsystem"/>)
    /// should derive a new type from this type.
    /// </para><para>
    /// Each <see cref="XRReferenceObjectEntry"/> is generally an asset on disk
    /// in a format specific to that provider.
    /// </para>
    /// </remarks>
    /// <seealso cref="XRReferenceObject"/>
    /// <seealso cref="XRReferenceObjectLibrary"/>
    public abstract class XRReferenceObjectEntry : ScriptableObject
    { }
}
