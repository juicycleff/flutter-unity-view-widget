#pragma once

// we allow to build with sdk 9.0 (and run on ios7) so we need to take an extra care about Metal support
// it is expected to substitute Metal.h so only objc

#ifdef __cplusplus
extern "C" typedef MTLDeviceRef (*MTLCreateSystemDefaultDeviceFunc)();
#else
typedef MTLDeviceRef (*MTLCreateSystemDefaultDeviceFunc)();
#endif


#if UNITY_CAN_USE_METAL

    #import <Metal/Metal.h>
    #import <QuartzCore/CAMetalLayer.h>

#else

// everything below is to allow compilation of the code that uses metal interfaces for simulator
// as protocols declarations should be available even if we do not call functions themselves

#if !(TARGET_IPHONE_SIMULATOR && defined(__IPHONE_11_0)) && !(TARGET_TVOS_SIMULATOR && defined(__TVOS_11_0))

typedef NSUInteger MTLPixelFormat;
enum
{
    MTLPixelFormatBGRA8Unorm,
    MTLPixelFormatBGRA8Unorm_sRGB,
    MTLPixelFormatR16Float,
};

#endif

@interface CAMetalLayer : CALayer
@property (readwrite) BOOL framebufferOnly;
@property (readwrite) CGSize drawableSize;
@property BOOL presentsWithTransaction;
@property (readwrite, retain) id<MTLDevice> device;
@property (readwrite) MTLPixelFormat pixelFormat;
@property (readonly) id<MTLTexture> texture;

- (id<CAMetalDrawable>)newDrawable;
- (id<CAMetalDrawable>)nextDrawable;
@end

@protocol MTLDrawable
@end
@protocol CAMetalDrawable<MTLDrawable>
@property (readonly) id<MTLTexture> texture;
@end

@protocol MTLDevice
- (id<MTLCommandQueue>)newCommandQueue;
- (id<MTLCommandQueue>)newCommandQueueWithMaxCommandBufferCount:(NSUInteger)maxCommandBufferCount;
- (BOOL)supportsTextureSampleCount:(NSUInteger)sampleCount;
@end

@protocol MTLCommandBuffer
- (void)presentDrawable:(id<MTLDrawable>)drawable;
@end

#endif
