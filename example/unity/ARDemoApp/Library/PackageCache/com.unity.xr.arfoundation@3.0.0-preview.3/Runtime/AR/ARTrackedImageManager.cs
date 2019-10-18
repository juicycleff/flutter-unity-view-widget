using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A manager for <see cref="ARTrackedImage"/>s. Uses the <c>XRImageTrackingSubsystem</c>
    /// to recognize and track 2D images in the physical environment.
    /// </summary>
    [DefaultExecutionOrder(ARUpdateOrder.k_TrackedImageManager)]
    [RequireComponent(typeof(ARSessionOrigin))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARTrackedImageManager.html")]
    public sealed class ARTrackedImageManager : ARTrackableManager<
        XRImageTrackingSubsystem,
        XRImageTrackingSubsystemDescriptor,
        XRTrackedImage,
        ARTrackedImage>
    {
        [SerializeField]
        [FormerlySerializedAs("m_ReferenceLibrary")]
        [Tooltip("The library of images which will be detected and/or tracked in the physical environment.")]
        XRReferenceImageLibrary m_SerializedLibrary;

        /// <summary>
        /// Get or set the reference image library, the set of images to search for in the physical environment.
        /// </summary>
        /// <remarks>
        /// An <c>IReferenceImageLibrary</c> can be either an <c>XRReferenceImageLibrary</c>
        /// or a <c>RuntimeReferenceImageLibrary</c>. <c>XRReferenceImageLibrary</c>s can only be
        /// constructed at edit-time and are immutable at runtime. A <c>RuntimeReferenceImageLibrary</c>
        /// is the runtime representation of a <c>XRReferenceImageLibrary</c> and may be mutable
        /// at runtime (see <c>MutableRuntimeReferenceImageLibrary</c>).
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">Thrown if the <see cref="referenceLibrary"/> is set to <c>null</c> while image tracking is enabled.</exception>
        public IReferenceImageLibrary referenceLibrary
        {
            get
            {
                if (subsystem != null)
                {
                    return subsystem.imageLibrary;
                }
                else
                {
                    return m_SerializedLibrary;
                }
            }

            set
            {
                if (value == null && subsystem != null && subsystem.running)
                    throw new InvalidOperationException("Cannot set a null reference library while image tracking is enabled.");

                if (value is XRReferenceImageLibrary serializedLibrary)
                {
                    m_SerializedLibrary = serializedLibrary;
                    if (subsystem != null)
                        subsystem.imageLibrary = subsystem.CreateRuntimeLibrary(serializedLibrary);
                }
                else if (value is RuntimeReferenceImageLibrary runtimeLibrary)
                {
                    m_SerializedLibrary = null;
                    CreateSubsystemIfNecessary();
                    if (subsystem != null)
                        subsystem.imageLibrary = runtimeLibrary;
                }

                if (subsystem != null)
                    UpdateReferenceImages(subsystem.imageLibrary);
            }
        }

        /// <summary>
        /// Creates a <c>UnityEngine.XR.ARSubsystems.RuntimeReferenceImageLibrary</c> from an existing
        /// <c>UnityEngine.XR.ARSubsystems.XRReferenceImageLibrary</c>
        /// or an empty library if <paramref name="serializedLibrary"/> is <c>null</c>.
        /// Use this to construct reference image libraries at runtime. If the library is of type
        /// <c>MutableRuntimeReferenceImageLibrary</c>, it is modifiable at runtime.
        /// </summary>
        /// <param name="serializedLibrary">An existing <c>XRReferenceImageLibrary</c>, or <c>null</c> to create an empty mutable image library.</param>
        /// <returns>A new <c>RuntimeReferenceImageLibrary</c> representing the deserialized version of <paramref name="serializedLibrary"/>or an empty library if <paramref name="serializedLibrary"/> is <c>null</c>.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if there is no subsystem. This usually means image tracking is not supported.</exception>
        public RuntimeReferenceImageLibrary CreateRuntimeLibrary(XRReferenceImageLibrary serializedLibrary = null)
        {
            CreateSubsystemIfNecessary();

            if (subsystem == null)
                throw new NotSupportedException("No image tracking subsystem found. This usually means image tracking is not supported.");

            return subsystem.CreateRuntimeLibrary(serializedLibrary);
        }

        [SerializeField]
        [Tooltip("The maximum number of moving images to track in realtime. Not all implementations support this feature.")]
        int m_MaxNumberOfMovingImages;

        /// <summary>
        /// The maximum number of moving images to track in realtime. Support may vary between devices and providers. Check
        /// for support at runtime with <see cref="subsystem"/><c>.SubsystemDescriptor.supportsMovingImages</c>.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the subsystem does not support moving images.</exception>
        public int maxNumberOfMovingImages
        {
            get { return m_MaxNumberOfMovingImages; }
            set
            {
                if (value == m_MaxNumberOfMovingImages)
                    return;

                SetMaxNumberOfMovingImages(value);
            }
        }

        [SerializeField]
        [Tooltip("If not null, instantiates this prefab for each detected image.")]
        GameObject m_TrackedImagePrefab;

        /// <summary>
        /// If not null, instantiates this prefab for each detected image.
        /// </summary>
        public GameObject trackedImagePrefab
        {
            get { return m_TrackedImagePrefab; }
            set { m_TrackedImagePrefab = value; }
        }

        protected override GameObject GetPrefab()
        {
            return m_TrackedImagePrefab;
        }

        /// <summary>
        /// Invoked once per frame with information about the <see cref="ARTrackedImage"/>s that have changed, i.e., been added, updated, or removed.
        /// This happens just before <see cref="ARTrackedImage"/>s are destroyed, so you can set <c>ARTrackedImage.destroyOnRemoval</c> to <c>false</c>
        /// from this event to suppress this behavior.
        /// </summary>
        public event Action<ARTrackedImagesChangedEventArgs> trackedImagesChanged;

        /// <summary>
        /// The name to be used for the <c>GameObject</c> whenever a new image is detected.
        /// </summary>
        protected override string gameObjectName
        {
            get { return "ARTrackedImage"; }
        }

        /// <summary>
        /// Sets the image library on the subsystem before Start() is called on the <c>XRImageTrackingSubsystem</c>.
        /// </summary>
        protected override void OnBeforeStart()
        {
            if (subsystem.imageLibrary == null && m_SerializedLibrary != null)
            {
                subsystem.imageLibrary = subsystem.CreateRuntimeLibrary(m_SerializedLibrary);
                m_SerializedLibrary = null;
            }

            UpdateReferenceImages(subsystem.imageLibrary);
            SetMaxNumberOfMovingImages(m_MaxNumberOfMovingImages);

            enabled = (subsystem.imageLibrary != null);
#if DEVELOPMENT_BUILD
            if (subsystem.imageLibrary == null)
            {
                Debug.LogWarning($"{nameof(ARTrackedImageManager)} '{name}' was enabled but no reference image library is specified. To enable, set a valid reference image library and then re-enable this component.");
            }
#endif
        }

        bool FindReferenceImage(Guid guid, out XRReferenceImage referenceImage)
        {
            if (m_ReferenceImages.TryGetValue(guid, out referenceImage))
                return true;

            // If we are using a mutable library, then its possible an image
            // has been added that we don't yet know about, so search the library.
            if (referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                foreach (var candidateImage in mutableLibrary)
                {
                    if (candidateImage.guid.Equals(guid))
                    {
                        referenceImage = candidateImage;
                        m_ReferenceImages.Add(referenceImage.guid, referenceImage);
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void OnAfterSetSessionRelativeData(
            ARTrackedImage image,
            XRTrackedImage sessionRelativeData)
        {
            if (FindReferenceImage(sessionRelativeData.sourceImageId, out XRReferenceImage referenceImage))
            {
                image.referenceImage = referenceImage;
            }
#if DEVELOPMENT_BUILD
            else
            {
                Debug.LogError($"Could not find reference image with guid {sessionRelativeData.sourceImageId}");
            }
#endif
        }

        /// <summary>
        /// Invokes the <see cref="trackedImagesChanged"/> event.
        /// </summary>
        /// <param name="added">A list of images added this frame.</param>
        /// <param name="updated">A list of images updated this frame.</param>
        /// <param name="removed">A list of images removed this frame.</param>
        protected override void OnTrackablesChanged(
            List<ARTrackedImage> added,
            List<ARTrackedImage> updated,
            List<ARTrackedImage> removed)
        {
            if (trackedImagesChanged != null)
                trackedImagesChanged(
                    new ARTrackedImagesChangedEventArgs(
                        added,
                        updated,
                        removed));
        }

        void UpdateReferenceImages(RuntimeReferenceImageLibrary library)
        {
            if (library == null)
                return;

            int count = library.count;
            for (int i = 0; i < count; ++i)
            {
                var referenceImage = library[i];
                m_ReferenceImages[referenceImage.guid] = referenceImage;
            }
        }

        void SetMaxNumberOfMovingImages(int value)
        {
            m_MaxNumberOfMovingImages = value;
            if (subsystem != null && descriptor.supportsMovingImages)
            {
                subsystem.maxNumberOfMovingImages = value;
            }
        }

        Dictionary<Guid, XRReferenceImage> m_ReferenceImages = new Dictionary<Guid, XRReferenceImage>();
    }
}
