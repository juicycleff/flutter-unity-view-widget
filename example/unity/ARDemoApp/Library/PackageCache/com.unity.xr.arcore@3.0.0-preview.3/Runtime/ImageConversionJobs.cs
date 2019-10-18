using System;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARCore
{
    internal struct FlipVerticalJob : IJobParallelFor
    {
        public int width;

        public int height;

        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeSlice<byte> grayscaleIn;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> grayscaleOut;

        public void Execute(int row)
        {
            int inputOffset = (height - 1 - row) * width;
            int outputOffset = row * width;
            int lastOffset = outputOffset + width;
            while (outputOffset < lastOffset)
            {
                grayscaleOut[outputOffset++] = grayscaleIn[inputOffset++];
            }
        }
    }

    internal struct ConvertRFloatToGrayscaleJob : IJobParallelFor
    {
        public int width;

        public int height;

        [ReadOnly]
        public NativeSlice<float> rfloatIn;

        [WriteOnly]
        public NativeArray<byte> grayscaleImageOut;

        public void Execute(int row)
        {
            int inputOffset = (height - 1 - row) * width;
            int outputOffset = row * width;
            int lastOffset = outputOffset + width;
            while (outputOffset < lastOffset)
            {
                grayscaleImageOut[outputOffset++] = (byte)(rfloatIn[inputOffset++] * 255);
            }
        }
    }

    internal struct ConvertBGRA32ToGrayscaleJob : IJobParallelFor
    {
        public int width;

        public int height;

        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeSlice<byte> colorImageIn;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> grayscaleImageOut;

        public void Execute(int row)
        {
            int colorImageOffset = (height - 1 - row) * width * 4;
            int grayImageOffset = row * width;
            int lastOffset = grayImageOffset + width;
            while (grayImageOffset < lastOffset)
            {
                grayscaleImageOut[grayImageOffset++] = (byte)(
                    colorImageIn[colorImageOffset    ] * 0.11f +
                    colorImageIn[colorImageOffset + 1] * 0.59f +
                    colorImageIn[colorImageOffset + 2] * 0.3f);

                colorImageOffset += 4;
            }
        }
    }

    internal struct ConvertARGB32ToGrayscaleJob : IJobParallelFor
    {
        public int width;

        public int height;

        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeSlice<byte> colorImageIn;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> grayscaleImageOut;

        public void Execute(int row)
        {
            int colorImageOffset = (height - 1 - row) * width * 4 + 1;
            int grayImageOffset = row * width + 1;
            int lastOffset = grayImageOffset + width;
            while (grayImageOffset < lastOffset)
            {
                grayscaleImageOut[grayImageOffset++] = (byte)(
                    colorImageIn[colorImageOffset    ] * 0.3f +
                    colorImageIn[colorImageOffset + 1] * 0.59f +
                    colorImageIn[colorImageOffset + 2] * 0.11f);

                colorImageOffset += 4;
            }
        }
    }

    internal struct ConvertStridedToGrayscaleJob : IJobParallelFor
    {
        public int stride;

        public int width;

        public int height;

        // NB: NativeDisableParallelForRestriction to allow
        // us to read and write to indices other than the
        // one passed into the Execute method. This is because
        // we interpret the index as the row number and process
        // and entire row at a time. This takes about 75% the
        // time of doing it one pixel at a time.

        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeSlice<byte> colorImageIn;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> grayscaleImageOut;

        public void Execute(int row)
        {
            int colorImageOffset = (height - 1 - row) * width * stride;
            int grayImageOffset = row * width;
            int lastOffset = grayImageOffset + width;
            while (grayImageOffset < lastOffset)
            {
                grayscaleImageOut[grayImageOffset++] = (byte)(
                    colorImageIn[colorImageOffset    ] * 0.3f +
                    colorImageIn[colorImageOffset + 1] * 0.59f +
                    colorImageIn[colorImageOffset + 2] * 0.11f);

                colorImageOffset += stride;
            }
        }
    }

    internal static class ConversionJob
    {
        public static JobHandle Schedule(
            NativeSlice<byte> inputImage,
            Vector2Int sizeInPixels,
            TextureFormat format,
            NativeArray<byte> grayscaleImage,
            JobHandle inputDeps)
        {
            int width = sizeInPixels.x;
            int height = sizeInPixels.y;

            if ((format == TextureFormat.R8) ||
                (format == TextureFormat.Alpha8))
            {
                return new FlipVerticalJob
                {
                    width = width,
                    height = height,
                    grayscaleIn = inputImage,
                    grayscaleOut = grayscaleImage
                }.Schedule(height, 1, inputDeps);
            }

            // We'll have to convert it. Create an output buffer.
            if (format == TextureFormat.RGB24)
            {
                return new ConvertStridedToGrayscaleJob
                {
                    stride = 3,
                    width = width,
                    height = height,
                    colorImageIn = inputImage,
                    grayscaleImageOut = grayscaleImage
                }.Schedule(height, 1, inputDeps);
            }
            else if (format == TextureFormat.RGBA32)
            {
                return new ConvertStridedToGrayscaleJob
                {
                    stride = 4,
                    width = width,
                    height = height,
                    colorImageIn = inputImage,
                    grayscaleImageOut = grayscaleImage
                }.Schedule(height, 1, inputDeps);
            }
            else if (format == TextureFormat.ARGB32)
            {
                return new ConvertARGB32ToGrayscaleJob
                {
                    width = width,
                    height = height,
                    colorImageIn = inputImage,
                    grayscaleImageOut = grayscaleImage
                }.Schedule(height, 1, inputDeps);
            }
            else if (format == TextureFormat.BGRA32)
            {
                return new ConvertBGRA32ToGrayscaleJob
                {
                    width = width,
                    height = height,
                    colorImageIn = inputImage,
                    grayscaleImageOut = grayscaleImage
                }.Schedule(height, 1, inputDeps);
            }
            else if (format == TextureFormat.RFloat)
            {
                return new ConvertRFloatToGrayscaleJob
                {
                    width = width,
                    height = height,
                    rfloatIn = inputImage.SliceConvert<float>(),
                    grayscaleImageOut = grayscaleImage
                }.Schedule(height, 1, inputDeps);
            }
            else
            {
                throw new InvalidOperationException($"Texture format {format} is not supported.");
            }
        }
    }
}
