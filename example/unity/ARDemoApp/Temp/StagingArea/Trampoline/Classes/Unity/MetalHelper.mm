#include "UnityTrampolineCompatibility.h"
#include "UnityRendering.h"

#if UNITY_CAN_USE_METAL

#include "UnityMetalSupport.h"
#include <QuartzCore/QuartzCore.h>
#include <libkern/OSAtomic.h>

#if UNITY_TRAMPOLINE_IN_USE
#include "UnityAppController.h"
#include "CVTextureCache.h"
#endif

#include "ObjCRuntime.h"

#if UNITY_TRAMPOLINE_IN_USE
static Class MTLTextureDescriptorClass;
#else
extern Class MTLTextureDescriptorClass;
#endif

extern "C" void InitRenderingMTL()
{
#if UNITY_TRAMPOLINE_IN_USE
    extern bool _supportsMSAA;
    _supportsMSAA = true;

    MTLTextureDescriptorClass = [UnityGetMetalBundle() classNamed: @"MTLTextureDescriptor"];
#endif
}

static MTLPixelFormat GetColorFormatForSurface(const UnityDisplaySurfaceMTL* surface)
{
    MTLPixelFormat colorFormat = surface->srgb ? MTLPixelFormatBGRA8Unorm_sRGB : MTLPixelFormatBGRA8Unorm;
#if PLATFORM_IOS || PLATFORM_TVOS
    if (surface->wideColor && UnityIsWideColorSupported())
        colorFormat = surface->srgb ? MTLPixelFormatBGR10_XR_sRGB : MTLPixelFormatBGR10_XR;
#elif PLATFORM_OSX
    if (surface->wideColor)
        colorFormat = MTLPixelFormatRGBA16Float;
#endif
    return colorFormat;
}

extern "C" void CreateSystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    DestroySystemRenderingSurfaceMTL(surface);

    MTLPixelFormat colorFormat = GetColorFormatForSurface(surface);
    surface->layer.presentsWithTransaction = NO;
    surface->layer.drawsAsynchronously = YES;

#if !PLATFORM_OSX
    if (UnityPreserveFramebufferAlpha())
    {
        const CGFloat components[] = {1.0f, 1.0f, 1.0f, 0.0f};
        CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
        CGColorRef color = CGColorCreate(colorSpace, components);
        surface->layer.opaque = NO;
        surface->layer.backgroundColor = color;
        CGColorRelease(color);
        CGColorSpaceRelease(colorSpace);
    }
#endif

#if PLATFORM_OSX
    surface->layer.opaque = YES;
    MetalUpdateDisplaySync();
#endif


#if PLATFORM_OSX
    CGColorSpaceRef colorSpaceRef = nil;
    if (surface->wideColor)
        colorSpaceRef = CGColorSpaceCreateWithName(surface->srgb ? kCGColorSpaceExtendedLinearSRGB : kCGColorSpaceExtendedSRGB);
    else
        colorSpaceRef = CGColorSpaceCreateWithName(kCGColorSpaceSRGB);

    surface->layer.colorspace = colorSpaceRef;
    CGColorSpaceRelease(colorSpaceRef);
#endif

    // Update the native screen resolution
    UnityUpdateDrawableSize(surface);

    surface->layer.device = surface->device;
    surface->layer.pixelFormat = colorFormat;
    surface->layer.framebufferOnly = (surface->framebufferOnly != 0);
    surface->colorFormat = colorFormat;

    MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass texture2DDescriptorWithPixelFormat: colorFormat width: surface->systemW height: surface->systemH mipmapped: NO];
#if PLATFORM_OSX
    txDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModeManaged;
#endif
    txDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;

    @synchronized(surface->layer)
    {
        OSAtomicCompareAndSwap32Barrier(0, 0, &surface->bufferCompleted);
        OSAtomicCompareAndSwap32Barrier(0, 0, &surface->bufferSwap);

        for (int i = 0; i < kUnityNumOffscreenSurfaces; i++)
        {
            // Allocating a proxy texture is cheap until it's being rendered to and the GPU driver does allocation
            surface->drawableProxyRT[i] = [surface->device newTextureWithDescriptor: txDesc];
            surface->drawableProxyRT[i].label = @"DrawableProxy";
            // We mostly need the proxy for some of its state like width, height and pixelFormat (not the actual memory) before we can get the real drawable
            // Making it empty discards its backed memory/contents
            for (int i = 0; i < kUnityNumOffscreenSurfaces; ++i)
                [surface->drawableProxyRT[i] setPurgeableState: MTLPurgeableStateEmpty];
        }
    }
}

extern "C" void CreateRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    DestroyRenderingSurfaceMTL(surface);

    MTLPixelFormat colorFormat = GetColorFormatForSurface(surface);

    const int w = surface->targetW, h = surface->targetH;

    if (w != surface->systemW || h != surface->systemH || surface->useCVTextureCache)
    {
#if PLATFORM_IOS || PLATFORM_TVOS
        if (surface->useCVTextureCache)
            surface->cvTextureCache = CreateCVTextureCache();

        if (surface->cvTextureCache)
        {
            surface->cvTextureCacheTexture = CreateReadableRTFromCVTextureCache(surface->cvTextureCache, surface->targetW, surface->targetH, &surface->cvPixelBuffer);
            surface->targetColorRT = GetMetalTextureFromCVTextureCache(surface->cvTextureCacheTexture);
        }
        else
#endif
        {
            MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass new];
            txDesc.textureType = MTLTextureType2D;
            txDesc.width = w;
            txDesc.height = h;
            txDesc.depth = 1;
            txDesc.pixelFormat = colorFormat;
            txDesc.arrayLength = 1;
            txDesc.mipmapLevelCount = 1;
#if PLATFORM_OSX
            txDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModeManaged;
#endif
            txDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
            surface->targetColorRT = [surface->device newTextureWithDescriptor: txDesc];
        }
        surface->targetColorRT.label = @"targetColorRT";
    }

    if (surface->msaaSamples > 1)
    {
        MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass new];
        txDesc.textureType = MTLTextureType2DMultisample;
        txDesc.width = w;
        txDesc.height = h;
        txDesc.depth = 1;
        txDesc.pixelFormat = colorFormat;
        txDesc.arrayLength = 1;
        txDesc.mipmapLevelCount = 1;
        txDesc.sampleCount = surface->msaaSamples;
#if PLATFORM_OSX
        txDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModePrivate;
#endif
        txDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
        if (![surface->device supportsTextureSampleCount: txDesc.sampleCount])
            txDesc.sampleCount = 4;
        surface->targetAAColorRT = [surface->device newTextureWithDescriptor: txDesc];
        surface->targetAAColorRT.label = @"targetAAColorRT";

        if (!surface->targetColorRT)
        {
            MTLTextureDescriptor* txDescResolve = [txDesc copyWithZone: nil];
            txDescResolve.textureType = MTLTextureType2D;
            txDescResolve.sampleCount = 1;
            surface->targetColorRT = [surface->device newTextureWithDescriptor: txDescResolve];
        }
    }
}

extern "C" void DestroyRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    surface->targetColorRT = nil;
    surface->targetAAColorRT = nil;

    if (surface->cvTextureCacheTexture)
        CFRelease(surface->cvTextureCacheTexture);
    if (surface->cvPixelBuffer)
        CFRelease(surface->cvPixelBuffer);
    if (surface->cvTextureCache)
        CFRelease(surface->cvTextureCache);
    surface->cvTextureCache = 0;
}

extern "C" void CreateSharedDepthbufferMTL(UnityDisplaySurfaceMTL* surface)
{
    DestroySharedDepthbufferMTL(surface);
    if (surface->disableDepthAndStencil)
        return;

    MTLPixelFormat pixelFormat = MTLPixelFormatDepth32Float_Stencil8;

    MTLTextureDescriptor* depthTexDesc = [MTLTextureDescriptorClass texture2DDescriptorWithPixelFormat: pixelFormat width: surface->targetW height: surface->targetH mipmapped: NO];
#if PLATFORM_OSX
    depthTexDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModePrivate;
#endif

#if PLATFORM_IOS || PLATFORM_TVOS
    if (surface->memorylessDepth)
        depthTexDesc.storageMode = MTLStorageModeMemoryless;
#endif

    depthTexDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
    if (surface->msaaSamples > 1)
    {
        depthTexDesc.textureType = MTLTextureType2DMultisample;
        depthTexDesc.sampleCount = surface->msaaSamples;
        if (![surface->device supportsTextureSampleCount: depthTexDesc.sampleCount])
            depthTexDesc.sampleCount = 4;
    }
    surface->depthRB = [surface->device newTextureWithDescriptor: depthTexDesc];
    surface->stencilRB = surface->depthRB;
}

extern "C" void DestroySharedDepthbufferMTL(UnityDisplaySurfaceMTL* surface)
{
    surface->depthRB = nil;
    surface->stencilRB = nil;
}

extern "C" void CreateUnityRenderBuffersMTL(UnityDisplaySurfaceMTL* surface)
{
    UnityRenderBufferDesc sys_desc = { surface->systemW, surface->systemH, 1, 1, 1 };
    UnityRenderBufferDesc tgt_desc = { surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1 };

    // To avoid race condition with EndFrameRenderingMTL where systemColorRB is nulled we store it here
    MTLTextureRef systemColorRB = surface->drawableProxyRT[0];

    surface->systemColorRB = systemColorRB;
    if (surface->targetAAColorRT)
        surface->unityColorBuffer   = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetAAColorRT, surface->targetColorRT, &tgt_desc, nil);
    else if (surface->targetColorRT)
        surface->unityColorBuffer   = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetColorRT, nil, &tgt_desc, nil);
    else
        surface->unityColorBuffer   = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->systemColorRB, nil, &tgt_desc, surface);

    if (surface->depthRB)
        surface->unityDepthBuffer   = UnityCreateExternalDepthSurfaceMTL(surface->unityDepthBuffer, surface->depthRB, surface->stencilRB, &tgt_desc);
    else
        surface->unityDepthBuffer   = UnityCreateDummySurface(surface->unityDepthBuffer, false, &tgt_desc);

    surface->systemColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->systemColorBuffer, systemColorRB, nil, &sys_desc, surface);
    surface->systemDepthBuffer = UnityCreateDummySurface(surface->systemDepthBuffer, false, &sys_desc);
}

extern "C" void DestroySystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    // before we needed to nil surface->systemColorRB (to release drawable we get from the view)
    // but after we switched to proxy rt this is no longer needed
    // even more it is harmful when running rendering on another thread (as is default now)
    // as on render thread we do StartFrameRenderingMTL/AcquireDrawableMTL/EndFrameRenderingMTL
    // and DestroySystemRenderingSurfaceMTL comes on main thread so we might end up with race condition for no reason
}

extern "C" void DestroyUnityRenderBuffersMTL(UnityDisplaySurfaceMTL* surface)
{
    UnityDestroyExternalSurface(surface->unityColorBuffer);
    UnityDestroyExternalSurface(surface->systemColorBuffer);
    surface->unityColorBuffer = surface->systemColorBuffer = 0;

    UnityDestroyExternalSurface(surface->unityDepthBuffer);
    UnityDestroyExternalSurface(surface->systemDepthBuffer);
    surface->unityDepthBuffer = surface->systemDepthBuffer = 0;
}

extern "C" void PreparePresentMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->targetColorRT)
        UnityBlitToBackbuffer(surface->unityColorBuffer, surface->systemColorBuffer, surface->systemDepthBuffer);
#if UNITY_TRAMPOLINE_IN_USE
    APP_CONTROLLER_RENDER_PLUGIN_METHOD(onFrameResolved);
#endif
}

extern "C" void PresentMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->drawable)
    {
    #if PLATFORM_IOS || PLATFORM_TVOS
        if (@available(iOS 10.3, tvOS 10.2, *))
        {
            const int targetFPS = UnityGetTargetFPS(); assert(targetFPS > 0);
            [UnityCurrentMTLCommandBuffer() presentDrawable: surface->drawable afterMinimumDuration: 1.0 / targetFPS];
            return;
        }
    #endif

        // note that we end up here if presentDrawable: afterMinimumDuration: is not supported
        [UnityCurrentMTLCommandBuffer() presentDrawable: surface->drawable];
    }
}

extern "C" MTLTextureRef AcquireDrawableMTL(UnityDisplaySurfaceMTL* surface)
{
    if (!surface)
        return nil;

    if (!surface->drawable)
        surface->drawable = [surface->layer nextDrawable];

    // on A7 SoC nextDrawable may be nil before locking the screen
    if (!surface->drawable)
        return nil;

    surface->systemColorRB = [surface->drawable texture];
    return surface->systemColorRB;
}

extern "C" int UnityCommandQueueMaxCommandBufferCountMTL()
{
    // customizable argument to pass towards [MTLDevice newCommandQueueWithMaxCommandBufferCount:],
    // the default value is 64 but with Parallel Render Encoder workloads, it might need to be increased

    return 256;
}

extern "C" void StartFrameRenderingMTL(UnityDisplaySurfaceMTL* surface)
{
    // we will acquire drawable lazily in AcquireDrawableMTL
    surface->drawable = nil;

#if PLATFORM_OSX
    bool bufferSwap = OSAtomicCompareAndSwap32Barrier(1, 0, &surface->bufferSwap);

    if (bufferSwap || surface->bufferCompleted == 1)
    {
        MTLTextureRef texture0 = surface->drawableProxyRT[0];
        MTLTextureRef texture1 = surface->drawableProxyRT[1];
        surface->drawableProxyRT[0] = texture1;
        surface->drawableProxyRT[1] = texture0;
    }
#endif
    surface->systemColorRB  = surface->drawableProxyRT[0];

    UnityRenderBufferDesc sys_desc = { surface->systemW, surface->systemH, 1, 1, 1};
    UnityRenderBufferDesc tgt_desc = { surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1};

    surface->systemColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->systemColorBuffer, surface->systemColorRB, nil, &sys_desc, surface);
    if (surface->targetColorRT == nil)
    {
        if (surface->targetAAColorRT)
            surface->unityColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetAAColorRT, surface->systemColorRB, &tgt_desc, surface);
        else
            surface->unityColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->systemColorRB, nil, &tgt_desc, surface);
    }
}

extern "C" void EndFrameRenderingMTL(UnityDisplaySurfaceMTL* surface)
{
    @autoreleasepool
    {
        if (surface->presentCB)
        {
            [surface->presentCB enqueue]; [surface->presentCB commit];
            surface->presentCB = nil;
        }

        surface->systemColorRB  = nil;
        surface->drawable       = nil;
    }
}

extern "C" void PreparePresentNonMainScreenMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->drawable)
    {
        surface->presentCB = [surface->drawableCommandQueue commandBuffer];
        [surface->presentCB presentDrawable: surface->drawable];
    }
}

extern "C" void SetDrawableSizeMTL(UnityDisplaySurfaceMTL* surface, int width, int height)
{
    surface->layer.drawableSize = CGSizeMake(width, height);
}

#else

extern "C" void InitRenderingMTL()                                          {}

extern "C" void CreateSystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)    {}
extern "C" void CreateRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)          {}
extern "C" void DestroyRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)         {}
extern "C" void CreateSharedDepthbufferMTL(UnityDisplaySurfaceMTL*)         {}
extern "C" void DestroySharedDepthbufferMTL(UnityDisplaySurfaceMTL*)        {}
extern "C" void CreateUnityRenderBuffersMTL(UnityDisplaySurfaceMTL*)        {}
extern "C" void DestroySystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)   {}
extern "C" void DestroyUnityRenderBuffersMTL(UnityDisplaySurfaceMTL*)       {}
extern "C" void StartFrameRenderingMTL(UnityDisplaySurfaceMTL*)             {}
extern "C" void EndFrameRenderingMTL(UnityDisplaySurfaceMTL*)               {}
extern "C" void PreparePresentMTL(UnityDisplaySurfaceMTL*)                  {}
extern "C" void PresentMTL(UnityDisplaySurfaceMTL*)                         {}
extern "C" int  UnityCommandQueueMaxCommandBufferCountMTL()                 { return 0; }
extern "C" void SetDrawableSizeMTL(UnityDisplaySurfaceMTL*, int, int)       {}

extern "C" MTLTextureRef    AcquireDrawableMTL(UnityDisplaySurfaceMTL*)     { return nil; }


#endif
