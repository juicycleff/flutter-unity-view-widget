#include <QuartzCore/QuartzCore.h>
#include <stdio.h>

#include "GlesHelper.h"
#include "UnityAppController.h"
#include "DisplayManager.h"
#include "EAGLContextHelper.h"
#include "CVTextureCache.h"
#include "InternalProfiler.h"

////////////////////////////////////////////////////////////////////////////////
// OpenGLES2 and OpenGLES3 on iOS and tvOS are deprecated as of Unity 2019.3 ///
////////////////////////////////////////////////////////////////////////////////

// here goes some gles magic

// we include gles3 header so we will use gles3 constants.
// sure all the actual gles3 is guarded (and constants are staying same)
#include <OpenGLES/ES3/gl.h>
#include <OpenGLES/ES3/glext.h>

// here are the prototypes for gles2 ext functions that moved to core in gles3
extern "C" void glDiscardFramebufferEXT(GLenum target, GLsizei numAttachments, const GLenum* attachments);
extern "C" void glRenderbufferStorageMultisampleAPPLE(GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height);
extern "C" void glResolveMultisampleFramebufferAPPLE(void);

#define SAFE_GL_DELETE(func, obj)   do { if(obj) { func(1,&obj); obj = 0; } } while(0)

#define DISCARD_FBO(ctx, fbo, cnt, att)                                     \
do{                                                                         \
    if(surface->context.API >= 3)   glInvalidateFramebuffer(fbo, cnt, att);\
    else if(_supportsDiscard)       glDiscardFramebufferEXT(fbo, cnt, att);\
} while(0)

#define CREATE_RB_AA(ctx, aa, fmt, w, h)                                                                    \
do{                                                                                                         \
    if(surface->context.API >= 3)   glRenderbufferStorageMultisample(GL_RENDERBUFFER, aa, fmt, w, h);   \
    else if(_supportsDiscard)       glRenderbufferStorageMultisampleAPPLE(GL_RENDERBUFFER, aa, fmt, w, h);\
} while(0)


static  bool    _supportsDiscard        = false;
static  bool    _supportsPackedStencil  = false;

extern "C" void InitRenderingGLES()
{
    int api = UnitySelectedRenderingAPI();
    assert(api == apiOpenGLES2 || api == apiOpenGLES3);

    _supportsDiscard        = api == apiOpenGLES2 ? UnityHasRenderingAPIExtension("GL_EXT_discard_framebuffer")         : true;
    _supportsMSAA           = api == apiOpenGLES2 ? UnityHasRenderingAPIExtension("GL_APPLE_framebuffer_multisample")   : true;
    _supportsPackedStencil  = api == apiOpenGLES2 ? UnityHasRenderingAPIExtension("GL_OES_packed_depth_stencil")        : true;
}

extern "C" void CreateSystemRenderingSurfaceGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);
    DestroySystemRenderingSurfaceGLES(surface);

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
    else
        surface->layer.opaque = YES;
    surface->layer.drawableProperties = @{ kEAGLDrawablePropertyRetainedBacking: @(FALSE), kEAGLDrawablePropertyColorFormat: kEAGLColorFormatRGBA8 };

    surface->colorFormat = GL_RGBA8;

    glGenRenderbuffers(1, &surface->systemColorRB);
    glBindRenderbuffer(GL_RENDERBUFFER, surface->systemColorRB);
    AllocateRenderBufferStorageFromEAGLLayer((__bridge void*)surface->context, (__bridge void*)surface->layer);

    glGenFramebuffers(1, &surface->systemFB);
    UnityBindFramebuffer(kDrawFramebuffer, surface->systemFB);
    glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_RENDERBUFFER, surface->systemColorRB);
}

extern "C" void CreateRenderingSurfaceGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);
    DestroyRenderingSurfaceGLES(surface);

    bool needRenderingSurface = surface->targetW != surface->systemW || surface->targetH != surface->systemH || surface->useCVTextureCache;
    if (needRenderingSurface)
    {
        GLint oldTexBinding = 0;
        if (surface->useCVTextureCache)
            surface->cvTextureCache = CreateCVTextureCache();

        if (surface->cvTextureCache)
        {
            surface->cvTextureCacheTexture = CreateReadableRTFromCVTextureCache(surface->cvTextureCache, surface->targetW, surface->targetH, &surface->cvPixelBuffer);
            surface->targetColorRT = GetGLTextureFromCVTextureCache(surface->cvTextureCacheTexture);
        }
        else
        {
            glGenTextures(1, &surface->targetColorRT);
        }

        glGetIntegerv(GL_TEXTURE_BINDING_2D, &oldTexBinding);

        glBindTexture(GL_TEXTURE_2D, surface->targetColorRT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GLES_UPSCALE_FILTER);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GLES_UPSCALE_FILTER);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

        if (!surface->cvTextureCache)
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, surface->targetW, surface->targetH, 0, GL_RGBA, GL_UNSIGNED_BYTE, 0);

        glGenFramebuffers(1, &surface->targetFB);
        UnityBindFramebuffer(kDrawFramebuffer, surface->targetFB);
        glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, surface->targetColorRT, 0);

        glBindTexture(GL_TEXTURE_2D, oldTexBinding);
    }

    if (_supportsMSAA && surface->msaaSamples > 1)
    {
        glGenRenderbuffers(1, &surface->msaaColorRB);
        glBindRenderbuffer(GL_RENDERBUFFER, surface->msaaColorRB);

        glGenFramebuffers(1, &surface->msaaFB);
        UnityBindFramebuffer(kDrawFramebuffer, surface->msaaFB);

        CREATE_RB_AA(surface->context, surface->msaaSamples, surface->colorFormat, surface->targetW, surface->targetH);
        glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_RENDERBUFFER, surface->msaaColorRB);
    }
}

extern "C" void CreateSharedDepthbufferGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);
    DestroySharedDepthbufferGLES(surface);
    if (surface->disableDepthAndStencil)
        return;

    surface->depthFormat = GL_DEPTH_COMPONENT24;
    if (_supportsPackedStencil)
        surface->depthFormat = GL_DEPTH24_STENCIL8;

    glGenRenderbuffers(1, &surface->depthRB);
    glBindRenderbuffer(GL_RENDERBUFFER, surface->depthRB);

    bool needMSAA = _supportsMSAA && surface->msaaSamples > 1;

    if (needMSAA)
        CREATE_RB_AA(surface->context, surface->msaaSamples, surface->depthFormat, surface->targetW, surface->targetH);

    if (!needMSAA)
        glRenderbufferStorage(GL_RENDERBUFFER, surface->depthFormat, surface->targetW, surface->targetH);

    if (surface->msaaFB)
        UnityBindFramebuffer(kDrawFramebuffer, surface->msaaFB);
    else if (surface->targetFB)
        UnityBindFramebuffer(kDrawFramebuffer, surface->targetFB);
    else
        UnityBindFramebuffer(kDrawFramebuffer, surface->systemFB);

    glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, surface->depthRB);
    if (_supportsPackedStencil)
        glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_STENCIL_ATTACHMENT, GL_RENDERBUFFER, surface->depthRB);
}

extern "C" void CreateUnityRenderBuffersGLES(UnityDisplaySurfaceGLES* surface)
{
    UnityRenderBufferDesc target_desc = {surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1};
    UnityRenderBufferDesc system_desc = {surface->systemW, surface->systemH, 1, 1, 1};

    {
        unsigned texid = 0, rbid = 0, fbo = 0;
        if (surface->msaaFB)
        {
            rbid  = surface->msaaColorRB;
            fbo = surface->msaaFB;
        }
        else if (surface->targetFB)
        {
            texid = surface->targetColorRT;
            fbo = surface->targetFB;
        }
        else
        {
            rbid  = surface->systemColorRB;
            fbo = surface->systemFB;
        }

        surface->unityColorBuffer = UnityCreateExternalSurfaceGLES(surface->unityColorBuffer, true, texid, rbid, surface->colorFormat, &target_desc);
        if (surface->depthRB)
            surface->unityDepthBuffer = UnityCreateExternalSurfaceGLES(surface->unityDepthBuffer, false, 0, surface->depthRB, surface->depthFormat, &target_desc);
        else
            surface->unityDepthBuffer = UnityCreateDummySurface(surface->unityDepthBuffer, false, &target_desc);

        UnityRegisterFBO(surface->unityColorBuffer, surface->unityDepthBuffer, fbo);
    }

    surface->systemColorBuffer = surface->systemDepthBuffer = 0;
    if (surface->msaaFB || surface->targetFB)
    {
        unsigned rbid = surface->systemColorRB;

        surface->systemColorBuffer = UnityCreateExternalSurfaceGLES(surface->systemColorBuffer, true, 0, rbid, surface->colorFormat, &system_desc);
        surface->systemDepthBuffer = UnityCreateDummySurface(surface->systemDepthBuffer, false, &system_desc);
        UnityRegisterFBO(surface->systemColorBuffer, surface->systemDepthBuffer, surface->systemFB);
    }

    surface->resolvedColorBuffer = 0;
    if (surface->msaaFB && surface->targetFB)
        surface->resolvedColorBuffer = UnityCreateExternalSurfaceGLES(surface->resolvedColorBuffer, true, surface->targetColorRT, 0, surface->colorFormat, &target_desc);
}

extern "C" void DestroySystemRenderingSurfaceGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);

    glBindRenderbuffer(GL_RENDERBUFFER, 0);
    UnityBindFramebuffer(kDrawFramebuffer, 0);

    if (surface->systemColorRB)
    {
        glBindRenderbuffer(GL_RENDERBUFFER, surface->systemColorRB);
        DeallocateRenderBufferStorageFromEAGLLayer((__bridge void*)surface->context);

        glBindRenderbuffer(GL_RENDERBUFFER, 0);
        glDeleteRenderbuffers(1, &surface->systemColorRB);
        surface->systemColorRB = 0;
    }

    if (surface->targetFB == 0 && surface->msaaFB == 0)
        SAFE_GL_DELETE(glDeleteRenderbuffers, surface->depthRB);
    SAFE_GL_DELETE(glDeleteFramebuffers, surface->systemFB);
}

extern "C" void DestroyRenderingSurfaceGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);

    if (surface->targetColorRT && !surface->cvTextureCache)
    {
        glDeleteTextures(1, &surface->targetColorRT); UnityOnDeleteGLTexture(surface->targetColorRT);
        surface->targetColorRT = 0;
    }

    UnityBindFramebuffer(kDrawFramebuffer, 0);
    UnityBindFramebuffer(kReadFramebuffer, 0);

    if (surface->cvTextureCacheTexture)
        CFRelease(surface->cvTextureCacheTexture);
    if (surface->cvPixelBuffer)
        CFRelease(surface->cvPixelBuffer);
    if (surface->cvTextureCache)
        CFRelease(surface->cvTextureCache);
    surface->cvTextureCache = 0;

    SAFE_GL_DELETE(glDeleteFramebuffers, surface->targetFB);
    SAFE_GL_DELETE(glDeleteRenderbuffers, surface->msaaColorRB);
    SAFE_GL_DELETE(glDeleteFramebuffers, surface->msaaFB);
}

extern "C" void DestroySharedDepthbufferGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);
    SAFE_GL_DELETE(glDeleteRenderbuffers, surface->depthRB);
}

extern "C" void DestroyUnityRenderBuffersGLES(UnityDisplaySurfaceGLES* surface)
{
    EAGLContextSetCurrentAutoRestore autorestore(surface->context);

    if (surface->unityColorBuffer)
        UnityDestroyExternalSurface(surface->unityColorBuffer);
    if (surface->systemColorBuffer)
        UnityDestroyExternalSurface(surface->systemColorBuffer);
    surface->unityColorBuffer = surface->systemColorBuffer = 0;

    if (surface->unityDepthBuffer)
        UnityDestroyExternalSurface(surface->unityDepthBuffer);
    if (surface->systemDepthBuffer)
        UnityDestroyExternalSurface(surface->systemDepthBuffer);
    surface->unityDepthBuffer = surface->systemDepthBuffer = 0;

    if (surface->resolvedColorBuffer)
        UnityDestroyExternalSurface(surface->resolvedColorBuffer);
    surface->resolvedColorBuffer = 0;
}

extern "C" void PreparePresentGLES(UnityDisplaySurfaceGLES* surface)
{
    {
        EAGLContextSetCurrentAutoRestore autorestore(surface->context);

        if (_supportsMSAA && surface->msaaSamples > 1)
        {
            Profiler_StartMSAAResolve();

            GLuint targetFB = surface->targetFB ? surface->targetFB : surface->systemFB;
            UnityBindFramebuffer(kReadFramebuffer, surface->msaaFB);
            UnityBindFramebuffer(kDrawFramebuffer, targetFB);

            GLenum  discardAttach[] = {GL_DEPTH_ATTACHMENT, GL_STENCIL_ATTACHMENT};
            DISCARD_FBO(surface->context, GL_READ_FRAMEBUFFER, 2, discardAttach);

            if (surface->context.API < 3)
            {
                glResolveMultisampleFramebufferAPPLE();
            }
            else
            {
                const GLint w = surface->targetW, h = surface->targetH;
                glBlitFramebuffer(0, 0, w, h, 0, 0, w, h, GL_COLOR_BUFFER_BIT, GL_NEAREST);
            }

            Profiler_EndMSAAResolve();
        }

        if (surface->allowScreenshot && UnityIsCaptureScreenshotRequested())
        {
            GLint targetFB = surface->targetFB ? surface->targetFB : surface->systemFB;
            UnityBindFramebuffer(kReadFramebuffer, targetFB);
            UnityCaptureScreenshot();
        }
    }

    APP_CONTROLLER_RENDER_PLUGIN_METHOD(onFrameResolved);

    if (surface->targetColorRT)
    {
        // shaders are bound to context
        EAGLContextSetCurrentAutoRestore autorestore(UnityGetMainScreenContextGLES());

        assert(surface->systemColorBuffer != 0 && surface->systemDepthBuffer != 0);

        UnityRenderBufferHandle src = surface->resolvedColorBuffer ? surface->resolvedColorBuffer : surface->unityColorBuffer;
        UnityBlitToBackbuffer(src, surface->systemColorBuffer, surface->systemDepthBuffer);
    }

    if (_supportsDiscard)
    {
        EAGLContextSetCurrentAutoRestore autorestore(surface->context);

        GLenum  discardAttach[] = {GL_COLOR_ATTACHMENT0, GL_DEPTH_ATTACHMENT, GL_STENCIL_ATTACHMENT};

        if (surface->msaaFB)
            DISCARD_FBO(surface->context, GL_READ_FRAMEBUFFER, 3, discardAttach);

        if (surface->targetFB)
        {
            UnityBindFramebuffer(kDrawFramebuffer, surface->targetFB);
            DISCARD_FBO(surface->context, GL_FRAMEBUFFER, 3, discardAttach);
        }

        UnityBindFramebuffer(kDrawFramebuffer, surface->systemFB);
        DISCARD_FBO(surface->context, GL_FRAMEBUFFER, 2, &discardAttach[1]);
    }
}

extern "C" void PresentGLES(UnityDisplaySurfaceGLES* surface)
{
    if (surface->context && surface->systemColorRB)
    {
        EAGLContextSetCurrentAutoRestore autorestore(surface->context);
        glBindRenderbuffer(GL_RENDERBUFFER, surface->systemColorRB);
        [surface->context presentRenderbuffer: GL_RENDERBUFFER];
    }
}

extern "C" void StartFrameRenderingGLES(UnityDisplaySurfaceGLES* /*surface*/)
{
}

extern "C" void EndFrameRenderingGLES(UnityDisplaySurfaceGLES* /*surface*/)
{
}

extern "C" void CheckGLESError(const char* file, int line)
{
    GLenum e = glGetError();
    if (e)
        ::printf("OpenGLES error 0x%04X in %s:%i\n", e, file, line);
}
