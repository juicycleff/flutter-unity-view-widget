using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    [Preserve]
    /// <summary>
    /// ARKit implementation of the <c>XRImageTrackingSubsystem</c>. You shouldn't
    /// need to interact directly with this unless using the <see cref="maximumNumberOfTrackedImages"/>
    /// property.
    /// </summary>
    public sealed class ARKitImageTrackingSubsystem : XRImageTrackingSubsystem
    {
        class ARKitProvider : Provider
        {
            public unsafe override RuntimeReferenceImageLibrary CreateRuntimeLibrary(
                XRReferenceImageLibrary serializedLibrary)
            {
                return new ARKitImageDatabase(serializedLibrary);
            }

            public override RuntimeReferenceImageLibrary imageLibrary
            {
                set
                {
                    if (value == null)
                    {
                        UnityARKit_imageTracking_stop();
                    }
                    else if (value is ARKitImageDatabase database)
                    {
                        UnityARKit_imageTracking_setDatabase(database.nativePtr);
                    }
                    else
                    {
                        throw new ArgumentException($"{value.GetType().Name} is not a valid ARKit image library.");
                    }
                }
            }

            public unsafe override TrackableChanges<XRTrackedImage> GetChanges(
                XRTrackedImage defaultTrackedImage,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedLength, updatedLength, removedLength, stride;

                var context = UnityARKit_imageTracking_acquireChanges(
                    out addedPtr, out addedLength,
                    out updatedPtr, out updatedLength,
                    out removedPtr, out removedLength,
                    out stride);

                try
                {
                    return new TrackableChanges<XRTrackedImage>(
                        addedPtr, addedLength,
                        updatedPtr, updatedLength,
                        removedPtr, removedLength,
                        defaultTrackedImage, stride,
                        allocator);
                }
                finally
                {
                    UnityARKit_imageTracking_releaseChanges(context);
                }
            }

            public override void Destroy() => UnityARKit_imageTracking_destroy();

            public override int maxNumberOfMovingImages
            {
                set => UnityARKit_imageTracking_setMaximumNumberOfTrackedImages(value);
            }
        }

        [DllImport("__Internal")]
        static extern void UnityARKit_imageTracking_setMaximumNumberOfTrackedImages(
            int maxNumTrackedImages);

        [DllImport("__Internal")]
        static extern void UnityARKit_imageTracking_setDatabase(IntPtr database);

        [DllImport("__Internal")]
        static extern void UnityARKit_imageTracking_stop();

        [DllImport("__Internal")]
        static extern void UnityARKit_imageTracking_destroy();

        [DllImport("__Internal")]
        static extern unsafe void* UnityARKit_imageTracking_acquireChanges(
            out void* addedPtr, out int addedLength,
            out void* updatedPtr, out int updatedLength,
            out void* removedPtr, out int removedLength,
            out int stride);

        [DllImport("__Internal")]
        static extern unsafe void UnityARKit_imageTracking_releaseChanges(void* changes);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
#if UNITY_IOS && !UNITY_EDITOR
            var iOSversion = OSVersion.Parse(UnityEngine.iOS.Device.systemVersion);

            // No support before iOS 11.3
            if (iOSversion < new OSVersion(11, 3))
                return;

            XRImageTrackingSubsystemDescriptor.Create(new XRImageTrackingSubsystemDescriptor.Cinfo
            {
                id = "ARKit-ImageTracking",
                subsystemImplementationType = typeof(ARKitImageTrackingSubsystem),
                supportsMovingImages = (iOSversion >= new OSVersion(12)),
                supportsMutableLibrary = true,
                requiresPhysicalImageDimensions = true
            });
#endif
        }

        protected override Provider CreateProvider()
        {
            return new ARKitProvider();
        }
    }
}
