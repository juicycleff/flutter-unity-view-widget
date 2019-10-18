#include "CMVideoSampling.h"

#include "CVTextureCache.h"
#include "GlesHelper.h"

#include <OpenGLES/ES3/glext.h>
#include <AVFoundation/AVFoundation.h>

void CMVideoSampling_Initialize(CMVideoSampling* sampling)
{
    ::memset(sampling, 0x00, sizeof(CMVideoSampling));
    sampling->cvTextureCache = CreateCVTextureCache();
}

void CMVideoSampling_Uninitialize(CMVideoSampling* sampling)
{
    if (sampling->cvImageBuffer)
    {
        CFRelease(sampling->cvImageBuffer);
        sampling->cvImageBuffer = 0;
    }
    if (sampling->cvTextureCacheTexture)
    {
        CFRelease(sampling->cvTextureCacheTexture);
        sampling->cvTextureCacheTexture = 0;
    }
    if (sampling->cvTextureCache)
    {
        CFRelease(sampling->cvTextureCache);
        sampling->cvTextureCache = 0;
    }
}

intptr_t CMVideoSampling_ImageBuffer(CMVideoSampling* sampling, CVImageBufferRef buffer, size_t* w, size_t* h)
{
    intptr_t retTex = 0;

    if (sampling->cvImageBuffer)
        CFRelease(sampling->cvImageBuffer);
    sampling->cvImageBuffer = buffer;
    CFRetain(sampling->cvImageBuffer);

    *w = CVPixelBufferGetWidth((CVImageBufferRef)sampling->cvImageBuffer);
    *h = CVPixelBufferGetHeight((CVImageBufferRef)sampling->cvImageBuffer);
    if (sampling->cvTextureCacheTexture)
    {
        CFRelease(sampling->cvTextureCacheTexture);
        FlushCVTextureCache(sampling->cvTextureCache);
        sampling->cvTextureCacheTexture = nil;
    }

    OSType pixelFormat = CVPixelBufferGetPixelFormatType(buffer);
    switch (pixelFormat)
    {
        case kCVPixelFormatType_32BGRA:
            sampling->cvTextureCacheTexture = CreateBGRA32TextureFromCVTextureCache(sampling->cvTextureCache, sampling->cvImageBuffer, *w, *h);
            break;
#if UNITY_HAS_IOSSDK_11_0
        case kCVPixelFormatType_DepthFloat16:
            sampling->cvTextureCacheTexture = CreateHalfFloatTextureFromCVTextureCache(sampling->cvTextureCache, sampling->cvImageBuffer, *w, *h);
            break;
#endif
        default:
            #define FourCC2Str(fourcc) (const char[]){*(((char*)&fourcc)+3), *(((char*)&fourcc)+2), *(((char*)&fourcc)+1), *(((char*)&fourcc)+0),0}
            ::printf("CMVideoSampling_SampleBuffer: unexpected pixel format \'%s\'\n", FourCC2Str(pixelFormat));
            break;
    }

    if (sampling->cvTextureCacheTexture)
        retTex = GetTextureFromCVTextureCache(sampling->cvTextureCacheTexture);

    if (UnitySelectedRenderingAPI() == apiOpenGLES2 || UnitySelectedRenderingAPI() == apiOpenGLES3)
    {
        GLint oldTexBinding = 0;

        glGetIntegerv(GL_TEXTURE_BINDING_2D, &oldTexBinding);
        glBindTexture(GL_TEXTURE_2D, (GLuint)retTex);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
        glBindTexture(GL_TEXTURE_2D, oldTexBinding);
    }

    return retTex;
}

intptr_t CMVideoSampling_SampleBuffer(CMVideoSampling* sampling, void* buffer, size_t* w, size_t* h)
{
    return CMVideoSampling_ImageBuffer(sampling, CMSampleBufferGetImageBuffer((CMSampleBufferRef)buffer), w, h);
}

intptr_t CMVideoSampling_LastSampledTexture(CMVideoSampling* sampling)
{
    return GetTextureFromCVTextureCache(sampling->cvTextureCacheTexture);
}
