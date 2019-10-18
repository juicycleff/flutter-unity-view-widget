#pragma once

// depending on selected rendering api it will be or GLES or Metal texture cache

// returns CVOpenGLESTextureCacheRef/CVMetalTextureCacheRef
void*       CreateCVTextureCache();
// cache = CVOpenGLESTextureCacheRef/CVMetalTextureCacheRef
void        FlushCVTextureCache(void* cache);

// returns CVOpenGLESTextureRef/CVMetalTextureRef
// cache = CVOpenGLESTextureCacheRef/CVMetalTextureCacheRef
// image = CVImageBufferRef/CVPixelBufferRef
void*       CreateBGRA32TextureFromCVTextureCache(void* cache, void* image, size_t w, size_t h);
void*       CreateHalfFloatTextureFromCVTextureCache(void* cache, void* image, size_t w, size_t h);

// texture = CVOpenGLESTextureRef
unsigned        GetGLTextureFromCVTextureCache(void* texture);
// texture = CVMetalTextureRef
MTLTextureRef   GetMetalTextureFromCVTextureCache(void* texture);

// texture = CVOpenGLESTextureRef/CVMetalTextureRef
uintptr_t       GetTextureFromCVTextureCache(void* texture);


// returns CVPixelBufferRef
// enforces kCVPixelFormatType_32BGRA
void*       CreatePixelBufferForCVTextureCache(size_t w, size_t h);
// returns CVOpenGLESTextureRef
// cache = CVOpenGLESTextureCacheRef
// pb = CVPixelBufferRef (out)
// enforces rgba texture with bgra backing
void*       CreateReadableRTFromCVTextureCache(void* cache, size_t w, size_t h, void** pb);

// texture = CVOpenGLESTextureRef/CVMetalTextureRef
int         IsCVTextureFlipped(void* texture);
