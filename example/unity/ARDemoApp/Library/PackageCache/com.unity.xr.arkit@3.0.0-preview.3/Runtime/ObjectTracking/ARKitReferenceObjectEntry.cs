using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents an ARKit-specific reference object for participation in an
    /// <c>XRReferenceObjectLibrary</c>.
    /// </summary>
    /// <remarks>
    /// The actual data used at runtime is packaged into the Xcode project
    /// in an asset catalog called <c>ARReferenceObjects.xcassets</c>. It should
    /// exist on disk in your project as an <c>.arobject</c> file.
    /// See <a href="https://developer.apple.com/documentation/arkit/scanning_and_detecting_3d_objects">Scanning and Detecting 3D Objects</a>
    /// for instructions on how to generate these files.
    /// </remarks>
    /// <seealso cref="XRReferenceObject"/>
    /// <seealso cref="XRReferenceObjectLibrary"/>
    public sealed class ARKitReferenceObjectEntry : XRReferenceObjectEntry
    {
        /// <summary>
        /// The reference origin of the scanned object.
        /// </summary>
        public Pose referenceOrigin
        {
            get { return m_ReferenceOrigin; }
        }

#pragma warning disable CS0649
        [SerializeField]
        internal Pose m_ReferenceOrigin;
#pragma warning restore CS0649
    }
}
