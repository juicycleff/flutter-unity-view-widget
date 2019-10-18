#pragma once

#define ENABLE_INTERNAL_PROFILER 0

// 4.x ENABLE_BLOCK_ON_GPU_PROFILER and 5.x ENABLE_GPU_TIMING were removed in favor of using xcode tools
// INCLUDE_OPENGLES_IN_RENDER_TIME was removed


#if ENABLE_INTERNAL_PROFILER

void Profiler_InitProfiler();
void Profiler_UninitProfiler();
void Profiler_FrameStart();
void Profiler_FrameEnd();
void Profiler_FramePresent(const UnityFrameStats*);

void Profiler_StartMSAAResolve();
void Profiler_EndMSAAResolve();

#else

inline void Profiler_InitProfiler()                             {}
inline void Profiler_UninitProfiler()                           {}
inline void Profiler_FrameStart()                               {}
inline void Profiler_FrameEnd()                                 {}
inline void Profiler_FramePresent(const struct UnityFrameStats*) {}

inline void Profiler_StartMSAAResolve()                         {}
inline void Profiler_EndMSAAResolve()                           {}


#endif // ENABLE_INTERNAL_PROFILER
