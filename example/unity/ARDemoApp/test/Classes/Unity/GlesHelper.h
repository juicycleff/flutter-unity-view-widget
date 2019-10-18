#pragma once


#ifdef __OBJC__
@class CAEAGLLayer;
@class EAGLContext;
#else
typedef struct objc_object CAEAGLLayer;
typedef struct objc_object EAGLContext;
#endif


#define MSAA_DEFAULT_SAMPLE_COUNT 1

// in case of rendering to non-native resolution the texture filter we will use for upscale blit
#define GLES_UPSCALE_FILTER GL_LINEAR
//#define GLES_UPSCALE_FILTER GL_NEAREST

// if gles support MSAA. We will need to recreate unity view if AA samples count was changed
extern  bool    _supportsMSAA;


#ifdef __cplusplus
extern "C" {
#endif

void CheckGLESError(const char* file, int line);

#ifdef __cplusplus
} // extern "C"
#endif
