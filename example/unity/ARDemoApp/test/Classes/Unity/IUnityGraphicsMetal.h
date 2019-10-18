// Unity Native Plugin API copyright © 2015 Unity Technologies ApS
//
// Licensed under the Unity Companion License for Unity - dependent projects--see[Unity Companion License](http://www.unity3d.com/legal/licenses/Unity_Companion_License).
//
// Unless expressly provided otherwise, the Software under this license is made available strictly on an “AS IS” BASIS WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED.Please review the license for details on these and other terms and conditions.

#pragma once
#include "IUnityInterface.h"

#ifndef __OBJC__
    #error metal plugin is objc code.
#endif
#ifndef __clang__
    #error only clang compiler is supported.
#endif

@class NSBundle;
@protocol MTLDevice;
@protocol MTLCommandBuffer;
@protocol MTLCommandEncoder;
@protocol MTLTexture;
@class MTLRenderPassDescriptor;


UNITY_DECLARE_INTERFACE(IUnityGraphicsMetalV1)
{
    NSBundle* (UNITY_INTERFACE_API * MetalBundle)();
    id<MTLDevice>(UNITY_INTERFACE_API * MetalDevice)();

    id<MTLCommandBuffer>(UNITY_INTERFACE_API * CurrentCommandBuffer)();

    // for custom rendering support there are two scenarios:
    // you want to use current in-flight MTLCommandEncoder (NB: it might be nil)
    id<MTLCommandEncoder>(UNITY_INTERFACE_API * CurrentCommandEncoder)();
    // or you might want to create your own encoder.
    // In that case you should end unity's encoder before creating your own and end yours before returning control to unity
    void(UNITY_INTERFACE_API * EndCurrentCommandEncoder)();

    // returns MTLRenderPassDescriptor used to create current MTLCommandEncoder
    MTLRenderPassDescriptor* (UNITY_INTERFACE_API * CurrentRenderPassDescriptor)();

    // converting trampoline UnityRenderBufferHandle into native RenderBuffer
    UnityRenderBuffer(UNITY_INTERFACE_API * RenderBufferFromHandle)(void* bufferHandle);

    // access to RenderBuffer's texure
    // NB: you pass here *native* RenderBuffer, acquired by calling (C#) RenderBuffer.GetNativeRenderBufferPtr
    // AAResolvedTextureFromRenderBuffer will return nil in case of non-AA RenderBuffer or if called for depth RenderBuffer
    // StencilTextureFromRenderBuffer will return nil in case of no-stencil RenderBuffer or if called for color RenderBuffer
    id<MTLTexture>(UNITY_INTERFACE_API * TextureFromRenderBuffer)(UnityRenderBuffer buffer);
    id<MTLTexture>(UNITY_INTERFACE_API * AAResolvedTextureFromRenderBuffer)(UnityRenderBuffer buffer);
    id<MTLTexture>(UNITY_INTERFACE_API * StencilTextureFromRenderBuffer)(UnityRenderBuffer buffer);
};
UNITY_REGISTER_INTERFACE_GUID(0x29F8F3D03833465EULL, 0x92138551C15D823DULL, IUnityGraphicsMetalV1)


// deprecated: please use versioned interface above

UNITY_DECLARE_INTERFACE(IUnityGraphicsMetal)
{
    NSBundle* (UNITY_INTERFACE_API * MetalBundle)();
    id<MTLDevice>(UNITY_INTERFACE_API * MetalDevice)();

    id<MTLCommandBuffer>(UNITY_INTERFACE_API * CurrentCommandBuffer)();
    id<MTLCommandEncoder>(UNITY_INTERFACE_API * CurrentCommandEncoder)();
    void(UNITY_INTERFACE_API * EndCurrentCommandEncoder)();
    MTLRenderPassDescriptor* (UNITY_INTERFACE_API * CurrentRenderPassDescriptor)();

    UnityRenderBuffer(UNITY_INTERFACE_API * RenderBufferFromHandle)(void* bufferHandle);

    id<MTLTexture>(UNITY_INTERFACE_API * TextureFromRenderBuffer)(UnityRenderBuffer buffer);
    id<MTLTexture>(UNITY_INTERFACE_API * AAResolvedTextureFromRenderBuffer)(UnityRenderBuffer buffer);
    id<MTLTexture>(UNITY_INTERFACE_API * StencilTextureFromRenderBuffer)(UnityRenderBuffer buffer);
};
UNITY_REGISTER_INTERFACE_GUID(0x992C8EAEA95811E5ULL, 0x9A62C4B5B9876117ULL, IUnityGraphicsMetal)
