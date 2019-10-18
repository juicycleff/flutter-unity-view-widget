using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Assertions;

namespace UnityEngine.XR.ARKit
{
    internal sealed class ARKitImageDatabase : MutableRuntimeReferenceImageLibrary
    {
        internal IntPtr nativePtr { get; private set; }

        ~ARKitImageDatabase()
        {
            Assert.AreNotEqual(nativePtr, IntPtr.Zero);

            // Release references
            int n = count;
            for (int i = 0; i < n; ++i)
            {
                UnityARKit_ImageDatabase_GetReferenceImage(nativePtr, i).Dispose();
            }

            UnityARKit_CFRelease(nativePtr);
        }

        public unsafe ARKitImageDatabase(XRReferenceImageLibrary serializedLibrary)
        {
            if (serializedLibrary == null)
            {
                nativePtr = UnityARKit_ImageDatabase_createEmpty();
            }
            else
            {
                var managedReferenceImages = new NativeArray<ManagedReferenceImage>(serializedLibrary.count, Allocator.Temp);
                for (int i = 0; i < serializedLibrary.count; ++i)
                {
                    managedReferenceImages[i] = new ManagedReferenceImage(serializedLibrary[i]);
                }

                using (managedReferenceImages)
                {
                    var nativeReturnCode = UnityARKit_ImageDatabase_tryCreateFromResourceGroup(
                        serializedLibrary.name, serializedLibrary.name.Length, serializedLibrary.guid,
                        managedReferenceImages.GetUnsafePtr(), managedReferenceImages.Length,
                        out IntPtr ptr);

                    switch (nativeReturnCode)
                    {
                        case SetReferenceLibraryResult.Success:
                            nativePtr = ptr;
                            break;

                        case SetReferenceLibraryResult.FeatureUnavailable:
                            throw new InvalidOperationException($"Failed to resolve image library '{serializedLibrary.name}'. This feature only works on versions of ARKit 11.3 and newer.");

                        case SetReferenceLibraryResult.ResourceDoesNotExist:
                            throw new InvalidOperationException($"Failed to resolve image library '{serializedLibrary.name}'. There is no matching resource group, or the resource group does not contain any reference images.");

                        default:
                            throw new InvalidOperationException($"Unexpected return code {nativeReturnCode} encountered while trying to create a reference image library with name {serializedLibrary.name}.");
                    }
                }
            }
        }

        protected override unsafe JobHandle ScheduleAddImageJobImpl(
            NativeSlice<byte> imageBytes,
            Vector2Int sizeInPixels,
            TextureFormat format,
            XRReferenceImage referenceImage,
            JobHandle inputDeps)
        {
            if (!referenceImage.specifySize)
                throw new InvalidOperationException("ARKit requires physical dimensions for all reference images.");

            // Add a reference to keep the native object alive
            // even if we get finalized while a job is running
            UnityARKit_CFRetain(nativePtr);

            // RGBA32 is not supported by CVPixelBuffer, but ARGB32 is, so
            // we offer a conversion for this common case.
            var convertedImage = new NativeArray<byte>();
            if (format == TextureFormat.RGBA32)
            {
                int numPixels = sizeInPixels.x * sizeInPixels.y;
                convertedImage = new NativeArray<byte>(
                    numPixels * 4,
                    Allocator.Persistent,
                    NativeArrayOptions.UninitializedMemory);

                inputDeps = new ConvertRGBA32ToARGB32Job
                {
                    rgbaImage = imageBytes.SliceConvert<uint>(),
                    argbImage = convertedImage.Slice().SliceConvert<uint>()
                }.Schedule(numPixels, 64, inputDeps);

                // Format is now ARGB32
                format = TextureFormat.ARGB32;
            }

            // Schedule the actual addition of the image to the database
            inputDeps = new AddImageJob
            {
                image = convertedImage.IsCreated ? new NativeSlice<byte>(convertedImage) : imageBytes,
                database = nativePtr,
                width = sizeInPixels.x,
                height = sizeInPixels.y,
                physicalWidth = referenceImage.size.x,
                format = format,
                managedReferenceImage = new ManagedReferenceImage(referenceImage)
            }.Schedule(inputDeps);

            // If we had to perform a conversion, then release that memory
            if (convertedImage.IsCreated)
            {
                inputDeps = new DeallocateNativeArrayJob<byte> { array = convertedImage }.Schedule(inputDeps);
            }

            return inputDeps;
        }

        static readonly TextureFormat[] k_SupportedFormats =
        {
            TextureFormat.Alpha8,
            TextureFormat.R8,
            TextureFormat.R16,
            TextureFormat.RFloat,
            TextureFormat.RGB24,
            TextureFormat.RGBA32,
            TextureFormat.ARGB32,
            TextureFormat.BGRA32,
        };

        public override int supportedTextureFormatCount => k_SupportedFormats.Length;

        protected override TextureFormat GetSupportedTextureFormatAtImpl(int index) => k_SupportedFormats[index];

        protected override XRReferenceImage GetReferenceImageAt(int index)
        {
            Assert.AreNotEqual(nativePtr, IntPtr.Zero);
            return UnityARKit_ImageDatabase_GetReferenceImage(nativePtr, index).ToReferenceImage();
        }

        public override int count
        {
            get
            {
                Assert.AreNotEqual(nativePtr, IntPtr.Zero);
                return UnityARKit_ImageDatabase_GetReferenceImageCount(nativePtr);
            }
        }

        struct DeallocateNativeArrayJob<T> : IJob where T : struct
        {
            [DeallocateOnJobCompletion]
            public NativeArray<T> array;

            public void Execute() {}
        }

        struct ConvertRGBA32ToARGB32Job : IJobParallelFor
        {
            public NativeSlice<uint> rgbaImage;

            public NativeSlice<uint> argbImage;

            public unsafe void Execute(int index)
            {
                uint rgba = rgbaImage[index];
                argbImage[index] = (rgba << 8) | ((rgba & 0xff000000) >> 24);
            }
        }

        struct AddImageJob : IJob
        {
            public NativeSlice<byte> image;

            public IntPtr database;

            public int width;

            public int height;

            public float physicalWidth;

            public TextureFormat format;

            public ManagedReferenceImage managedReferenceImage;

            public unsafe void Execute()
            {
                bool success = UnityARKit_ImageDatabase_AddImage(database, image.GetUnsafePtr(), format, width, height, physicalWidth, ref managedReferenceImage);
                if (!success)
                    managedReferenceImage.Dispose();

                UnityARKit_CFRelease(database);
            }

            [DllImport("__Internal")]
            static extern unsafe bool UnityARKit_ImageDatabase_AddImage(
                IntPtr database, void* bytes, TextureFormat format,
                int width, int height, float physicalWidth,
                ref ManagedReferenceImage managedReferenceImage);
        }

        [DllImport("__Internal")]
        static extern void UnityARKit_CFRetain(IntPtr ptr);

        [DllImport("__Internal")]
        static extern void UnityARKit_CFRelease(IntPtr ptr);

        [DllImport("__Internal")]
        static extern IntPtr UnityARKit_ImageDatabase_createEmpty();

        [DllImport("__Internal")]
        static unsafe extern SetReferenceLibraryResult UnityARKit_ImageDatabase_tryCreateFromResourceGroup(
            [MarshalAs(UnmanagedType.LPWStr)] string name, int nameLength, Guid guid,
            void* managedReferenceImages, int managedReferenceImageCount,
            out IntPtr ptr);

        [DllImport("__Internal")]
        static extern ManagedReferenceImage UnityARKit_ImageDatabase_GetReferenceImage(IntPtr database, int index);

        [DllImport("__Internal")]
        static extern int UnityARKit_ImageDatabase_GetReferenceImageCount(IntPtr database);
    }
}
