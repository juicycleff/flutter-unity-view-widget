#pragma once

// small helper for getting texture from CMSampleBuffer

typedef struct
    CMVideoSampling
{
    void*   cvTextureCache;
    void*   cvTextureCacheTexture;
    void*   cvImageBuffer;
}
CMVideoSampling;

void CMVideoSampling_Initialize(CMVideoSampling* sampling);
void CMVideoSampling_Uninitialize(CMVideoSampling* sampling);

intptr_t  CMVideoSampling_ImageBuffer(CMVideoSampling* sampling, CVImageBufferRef buffer, size_t* w, size_t* h);
intptr_t  CMVideoSampling_SampleBuffer(CMVideoSampling* sampling, void* buffer, size_t* w, size_t* h); // buffer is CMSampleBufferRef
intptr_t  CMVideoSampling_LastSampledTexture(CMVideoSampling* sampling);
