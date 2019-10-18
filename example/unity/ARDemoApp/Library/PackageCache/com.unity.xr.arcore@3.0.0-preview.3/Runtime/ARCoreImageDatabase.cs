using System;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    internal sealed class ARCoreImageDatabase : MutableRuntimeReferenceImageLibrary
    {
        IntPtr m_NativePtr;

        struct AddImageJob : IJob
        {
            [DeallocateOnJobCompletion]
            public NativeArray<byte> grayscaleImage;

            [DeallocateOnJobCompletion]
            public NativeArray<byte> name;

            public IntPtr database;

            public int width;

            public int height;

            public ManagedReferenceImage managedReferenceImage;

            public unsafe void Execute()
            {
                bool success = UnityARCore_ImageDatabase_addImage(
                    database,
                    ref managedReferenceImage,
                    grayscaleImage.GetUnsafePtr(),
                    width, height,
                    name.GetUnsafePtr());

                if (!success)
                    managedReferenceImage.Dispose();

                RcoApi.Release(database);
            }

            [DllImport("UnityARCore")]
            static extern unsafe bool UnityARCore_ImageDatabase_addImage(
                IntPtr database,
                ref ManagedReferenceImage managedReferenceImage,
                void* image, int width, int height, void* name);
        }

        public unsafe ARCoreImageDatabase(XRReferenceImageLibrary serializedLibrary)
        {
            if (serializedLibrary == null)
            {
                m_NativePtr = UnityARCore_ImageDatabase_deserialize(default(NativeView), default(NativeView));
            }
            else
            {
                using (var webRequest = new UnityWebRequest(ARCoreImageTrackingProvider.GetPathForLibrary(serializedLibrary)))
                {
                    webRequest.downloadHandler = new DownloadHandlerBuffer();
                    webRequest.disposeDownloadHandlerOnDispose = true;
                    webRequest.SendWebRequest();
                    while (!webRequest.isDone) {}

                    byte[] libraryBlob = webRequest.downloadHandler.data;
                    if (libraryBlob == null || libraryBlob.Length == 0)
                    {
                        throw new InvalidOperationException(string.Format(
                            "Failed to load image library '{0}' - file was empty!", serializedLibrary.name));
                    }

                    var managedReferenceImages = new NativeArray<ManagedReferenceImage>(serializedLibrary.count, Allocator.Temp);
                    try
                    {
                        for (int i = 0; i < serializedLibrary.count; ++i)
                        {
                            managedReferenceImages[i] = new ManagedReferenceImage(serializedLibrary[i]);
                        }

                        fixed (byte* blob = libraryBlob)
                        {
                            m_NativePtr = UnityARCore_ImageDatabase_deserialize(new NativeView(blob, libraryBlob.Length), NativeView.Create(managedReferenceImages));
                        }
                    }
                    finally
                    {
                        managedReferenceImages.Dispose();
                    }
                }
            }
        }

        public IntPtr nativePtr => m_NativePtr;

        ~ARCoreImageDatabase()
        {
            Assert.AreNotEqual(m_NativePtr, IntPtr.Zero);

            // Release references
            int n = count;
            for (int i = 0; i < n; ++i)
            {
                UnityARCore_ImageDatabase_getReferenceImage(m_NativePtr, i).Dispose();
            }
            RcoApi.Release(m_NativePtr);
        }

        static readonly TextureFormat[] k_SupportedFormats =
        {
            TextureFormat.Alpha8,
            TextureFormat.R8,
            TextureFormat.RFloat,
            TextureFormat.RGB24,
            TextureFormat.RGBA32,
            TextureFormat.ARGB32,
            TextureFormat.BGRA32,
        };

        public override int supportedTextureFormatCount => k_SupportedFormats.Length;

        protected override TextureFormat GetSupportedTextureFormatAtImpl(int index) => k_SupportedFormats[index];

        protected override JobHandle ScheduleAddImageJobImpl(
            NativeSlice<byte> imageBytes,
            Vector2Int sizeInPixels,
            TextureFormat format,
            XRReferenceImage referenceImage,
            JobHandle inputDeps)
        {
            var grayscaleImage = new NativeArray<byte>(
                sizeInPixels.x * sizeInPixels.y,
                Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);

            var conversionHandle = ConversionJob.Schedule(imageBytes, sizeInPixels, format, grayscaleImage, inputDeps);

            // Add a reference in case we are destroyed while the job is running
            RcoApi.Retain(m_NativePtr);
            return new AddImageJob
            {
                database = m_NativePtr,
                managedReferenceImage = new ManagedReferenceImage(referenceImage),
                grayscaleImage = grayscaleImage,
                width = sizeInPixels.x,
                height = sizeInPixels.y,
                name = new NativeArray<byte>(Encoding.UTF8.GetBytes(referenceImage.name + "\0"), Allocator.Persistent),
            }.Schedule(conversionHandle);
        }

        protected override XRReferenceImage GetReferenceImageAt(int index)
        {
            Assert.AreNotEqual(m_NativePtr, IntPtr.Zero);
            return UnityARCore_ImageDatabase_getReferenceImage(m_NativePtr, index).ToReferenceImage();
        }

        public override int count
        {
            get
            {
                Assert.AreNotEqual(m_NativePtr, IntPtr.Zero);
                return UnityARCore_ImageDatabase_getReferenceImageCount(m_NativePtr);
            }
        }

        public override int GetHashCode() => m_NativePtr.GetHashCode();
        public override bool Equals(object obj) => (obj is ARCoreImageDatabase) && Equals((ARCoreImageDatabase)obj);
        public bool Equals(ARCoreImageDatabase other) => !ReferenceEquals(other, null) && (m_NativePtr == other.m_NativePtr);
        public static bool operator ==(ARCoreImageDatabase lhs, ARCoreImageDatabase rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;

            return lhs.Equals(rhs);
        }
        public static bool operator !=(ARCoreImageDatabase lhs, ARCoreImageDatabase rhs) => !(lhs == rhs);

        [DllImport("UnityARCore")]
        static extern ManagedReferenceImage UnityARCore_ImageDatabase_getReferenceImage(IntPtr imageDatabase, int index);

        [DllImport("UnityARCore")]
        static extern int UnityARCore_ImageDatabase_getReferenceImageCount(IntPtr imageDatabase);

        [DllImport("UnityARCore")]
        static extern IntPtr UnityARCore_ImageDatabase_deserialize(NativeView serializedDatabase, NativeView referenceImages);
    }
}
