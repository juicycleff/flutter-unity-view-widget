#include "CVTextureCache.h"

#include "DisplayManager.h"

#include <OpenGLES/ES2/glext.h>
#include <OpenGLES/ES3/gl.h>
#include <OpenGLES/ES3/glext.h>
#include <CoreVideo/CVOpenGLESTextureCache.h>

#include "UnityMetalSupport.h"
#if UNITY_CAN_USE_METAL
    #include <CoreVideo/CVMetalTextureCache.h>
#else

typedef void* CVMetalTextureCacheRef;
typedef void* CVMetalTextureRef;

const CFStringRef kCVPixelBufferMetalCompatibilityKey = CFSTR("MetalCompatibility");
inline CVReturn         CVMetalTextureCacheCreate(CFAllocatorRef, CFDictionaryRef, MTLDeviceRef, CFDictionaryRef, CVMetalTextureCacheRef*)  { return 0; }
inline CVReturn         CVMetalTextureCacheCreateTextureFromImage(CFAllocatorRef, CVMetalTextureCacheRef, CVImageBufferRef, CFDictionaryRef, MTLPixelFormat, size_t, size_t, size_t, CVMetalTextureRef*)    { return 0; }
inline void             CVMetalTextureCacheFlush(CVMetalTextureCacheRef, uint64_t options)  {}
inline MTLTextureRef    CVMetalTextureGetTexture(CVMetalTextureRef) { return nil; }
inline Boolean          CVMetalTextureIsFlipped(CVMetalTextureRef)  { return 0; }

#endif


void* CreateCVTextureCache()
{
    void* ret = 0;

    CVReturn err = 0;
    if (UnitySelectedRenderingAPI() == apiMetal)
        err = CVMetalTextureCacheCreate(kCFAllocatorDefault, 0, UnityGetMetalDevice(), 0, (CVMetalTextureCacheRef*)&ret);
    else
        err = CVOpenGLESTextureCacheCreate(kCFAllocatorDefault, 0, UnityGetMainScreenContextGLES(), 0, (CVOpenGLESTextureCacheRef*)&ret);

    if (err)
    {
        ::printf("Error at CVOpenGLESTextureCacheCreate: %d", err);
        ret = 0;
    }
    return ret;
}

void FlushCVTextureCache(void* cache)
{
    if (UnitySelectedRenderingAPI() == apiMetal)
        CVMetalTextureCacheFlush((CVMetalTextureCacheRef)cache, 0);
    else
        CVOpenGLESTextureCacheFlush((CVOpenGLESTextureCacheRef)cache, 0);
}

void* CreateBGRA32TextureFromCVTextureCache(void* cache, void* image, size_t w, size_t h)
{
    void* texture = 0;

    CVReturn err = 0;
    if (UnitySelectedRenderingAPI() == apiMetal)
    {
        err = CVMetalTextureCacheCreateTextureFromImage(
            kCFAllocatorDefault, (CVMetalTextureCacheRef)cache, (CVImageBufferRef)image, 0,
            (MTLPixelFormat)((UnityDisplaySurfaceMTL*)GetMainDisplaySurface())->colorFormat, w, h, 0, (CVMetalTextureRef*)&texture
        );
    }
    else
    {
        err = CVOpenGLESTextureCacheCreateTextureFromImage(
            kCFAllocatorDefault, (CVOpenGLESTextureCacheRef)cache, (CVImageBufferRef)image, 0,
            GL_TEXTURE_2D, GL_RGBA, (GLsizei)w, (GLsizei)h, GL_BGRA_EXT, GL_UNSIGNED_BYTE,
            0, (CVOpenGLESTextureRef*)&texture
        );
    }

    if (err)
    {
        ::printf("Error at CVOpenGLESTextureCacheCreateTextureFromImage: %d\n", err);
        texture = 0;
    }
    return texture;
}

void* CreateHalfFloatTextureFromCVTextureCache(void* cache, void* image, size_t w, size_t h)
{
    void* texture = 0;

    CVReturn err = 0;
    if (UnitySelectedRenderingAPI() == apiMetal)
    {
        err = CVMetalTextureCacheCreateTextureFromImage(
            kCFAllocatorDefault, (CVMetalTextureCacheRef)cache, (CVImageBufferRef)image, 0,
            MTLPixelFormatR16Float, w, h, 0, (CVMetalTextureRef*)&texture
        );
    }
    else if (UnitySelectedRenderingAPI() == apiOpenGLES3)
    {
        err = CVOpenGLESTextureCacheCreateTextureFromImage(
            kCFAllocatorDefault, (CVOpenGLESTextureCacheRef)cache, (CVImageBufferRef)image, 0,
            GL_TEXTURE_2D, GL_R16F, (GLsizei)w, (GLsizei)h, GL_RED, GL_HALF_FLOAT,
            0, (CVOpenGLESTextureRef*)&texture
        );
    }
    else // OpenGLES2
    {
        err = CVOpenGLESTextureCacheCreateTextureFromImage(
            kCFAllocatorDefault, (CVOpenGLESTextureCacheRef)cache, (CVImageBufferRef)image, 0,
            GL_TEXTURE_2D, GL_RED_EXT, (GLsizei)w, (GLsizei)h, GL_RED_EXT, GL_HALF_FLOAT_OES,
            0, (CVOpenGLESTextureRef*)&texture
        );
    }

    if (err)
    {
        ::printf("Error at CVOpenGLESTextureCacheCreateTextureFromImage: %d\n", err);
        texture = 0;
    }
    return texture;
}

unsigned GetGLTextureFromCVTextureCache(void* texture)
{
    assert(UnitySelectedRenderingAPI() != apiMetal);
    return CVOpenGLESTextureGetName((CVOpenGLESTextureRef)texture);
}

id<MTLTexture> GetMetalTextureFromCVTextureCache(void* texture)
{
    assert(UnitySelectedRenderingAPI() == apiMetal);
    return CVMetalTextureGetTexture((CVMetalTextureRef)texture);
}

uintptr_t GetTextureFromCVTextureCache(void* texture)
{
    if (UnitySelectedRenderingAPI() == apiMetal)
        return (uintptr_t)(__bridge void*)GetMetalTextureFromCVTextureCache(texture);
    else
        return (uintptr_t)GetGLTextureFromCVTextureCache(texture);
}

void* CreatePixelBufferForCVTextureCache(size_t w, size_t h)
{
    NSString* apiKey = UnitySelectedRenderingAPI() == apiMetal  ? (__bridge NSString*)kCVPixelBufferMetalCompatibilityKey
        : (__bridge NSString*)kCVPixelBufferOpenGLESCompatibilityKey;
    CVPixelBufferRef pb = 0;
    NSDictionary* options = @{  (__bridge NSString*)kCVPixelBufferPixelFormatTypeKey: @(kCVPixelFormatType_32BGRA),
                                (__bridge NSString*)kCVPixelBufferWidthKey: @(w),
                                (__bridge NSString*)kCVPixelBufferHeightKey: @(h),
                                apiKey: @(YES),
                                (__bridge NSString*)kCVPixelBufferIOSurfacePropertiesKey: @{}};

    CVPixelBufferCreate(kCFAllocatorDefault, w, h, kCVPixelFormatType_32BGRA, (__bridge CFDictionaryRef)options, &pb);
    return pb;
}

void* CreateReadableRTFromCVTextureCache(void* cache, size_t w, size_t h, void** pb)
{
    *pb = CreatePixelBufferForCVTextureCache(w, h);
    return CreateBGRA32TextureFromCVTextureCache(cache, *pb, w, h);
}

int IsCVTextureFlipped(void* texture)
{
    if (UnitySelectedRenderingAPI() == apiMetal)
        return CVMetalTextureIsFlipped((CVMetalTextureRef)texture);
    else
        return CVOpenGLESTextureIsFlipped((CVOpenGLESTextureRef)texture);
}
