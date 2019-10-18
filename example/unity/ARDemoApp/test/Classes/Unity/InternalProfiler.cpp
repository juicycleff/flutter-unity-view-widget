#include "InternalProfiler.h"

#include <stdint.h>
#include <stdio.h>
#include <mach/mach_time.h>


#define ENABLE_DETAILED_GC_STATS 0

#if ENABLE_INTERNAL_PROFILER

namespace
{
    enum
    {
        GpuProfilerBlockEachNthFrame = 4
    };

    mach_timebase_info_data_t timebaseInfo;
    void ProfilerInit()
    {
        mach_timebase_info(&timebaseInfo);
    }

    static float MachToMillisecondsDelta(int64_t delta)
    {
        // Convert to nanoseconds
        delta *= timebaseInfo.numer;
        delta /= timebaseInfo.denom;
        float result = (float)delta / 1000000.0F;
        return result;
    }

    struct ProfilerBlock
    {
        int64_t maxV, minV, avgV;
    };

    void ProfilerBlock_Update(struct ProfilerBlock* b, int64_t d, bool reset, bool avoidZero = false)
    {
        if (reset)
        {
            b->maxV = b->minV = b->avgV = d;
        }
        else
        {
            b->maxV = (d > b->maxV) ? d : b->maxV;
            if (avoidZero && (b->minV == 0 || d == 0))
                b->minV = (d > b->minV) ? d : b->minV;
            else
                b->minV = (d < b->minV) ? d : b->minV;
            b->avgV += d;
        }
    }

    ProfilerBlock   _framePB;
    ProfilerBlock   _playerPB;

    ProfilerBlock   _batchCountPB;
    ProfilerBlock   _drawCallCountPB;
    ProfilerBlock   _triCountPB;
    ProfilerBlock   _vertCountPB;

    ProfilerBlock   _dynamicBatchDtPB;
    ProfilerBlock   _dynamicBatchCountPB;
    ProfilerBlock   _dynamicBatchedDrawCallCountPB;
    ProfilerBlock   _dynamicBatchedTriCountPB;
    ProfilerBlock   _dynamicBatchedVertCountPB;

    ProfilerBlock   _staticBatchCountPB;
    ProfilerBlock   _staticBatchedDrawCallCountPB;
    ProfilerBlock   _staticBatchedTriCountPB;
    ProfilerBlock   _staticBatchedVertCountPB;

    ProfilerBlock   _fixedBehaviourManagerPB;
    ProfilerBlock   _fixedPhysicsManagerPB;
    ProfilerBlock   _dynamicBehaviourManagerPB;
    ProfilerBlock   _coroutinePB;
    ProfilerBlock   _skinMeshUpdatePB;
    ProfilerBlock   _animationUpdatePB;
    ProfilerBlock   _unityRenderLoopPB;
    ProfilerBlock   _unityCullingPB;
    ProfilerBlock   _unityMSAAResolvePB;
    ProfilerBlock   _fixedUpdateCountPB;
    ProfilerBlock   _GCCountPB;
    ProfilerBlock   _GCDurationPB;


    int     _frameId            = 0;

    // our rendering is dictated by ios display link
    // frametime will be time between two frame starts
    // unity player time will be time between frame start/end
    int64_t _lastFrameStart     = -1;
    int64_t _frameStart         = 0;
    int64_t _frameTime          = 0;
    int64_t _playerTime         = 0;

    int64_t _msaaResolveStart   = 0;
    int64_t _msaaResolve        = 0;
    void*   _msaaResolveCounter = 0;


    UnityFrameStats _unityFrameStats;
}

extern "C" int64_t UnityScriptingGetUsedSize();
extern "C" int64_t UnityScriptingGetHeapSize();
static      void    Profiler_SetupScriptingProfile();

void Profiler_InitProfiler()
{
    Profiler_SetupScriptingProfile();
    ProfilerInit();

    if (_msaaResolveCounter == 0)
        _msaaResolveCounter = UnityCreateProfilerCounter("iOS.MSAAResolve");
}

void Profiler_UninitProfiler()
{
    UnityDestroyProfilerCounter(_msaaResolveCounter);
}

void Profiler_FrameStart()
{
    _frameStart = mach_absolute_time();
    if (_lastFrameStart < 0)
    {
        _lastFrameStart = _frameStart; return;
    }

    _frameTime = _frameStart - _lastFrameStart;
    _lastFrameStart  = _frameStart;
}

void Profiler_FrameEnd()
{
    _playerTime = mach_absolute_time() - _frameStart;
}

void Profiler_FramePresent(const UnityFrameStats* unityFrameStats)
{
    if (!unityFrameStats)
        return;
    _unityFrameStats = *unityFrameStats;

    const int EachNthFrame = 32;
    if (_frameId == EachNthFrame)
    {
        _frameId = 0;

        ::printf("iPhone Unity internal profiler stats\n");
        ::printf("frametime>     min: %4.1f   max: %4.1f   avg: %4.1f\n", MachToMillisecondsDelta(_framePB.minV), MachToMillisecondsDelta(_framePB.maxV), MachToMillisecondsDelta(_framePB.avgV / EachNthFrame));
        ::printf("cpu-player>    min: %4.1f   max: %4.1f   avg: %4.1f\n", MachToMillisecondsDelta(_playerPB.minV), MachToMillisecondsDelta(_playerPB.maxV), MachToMillisecondsDelta(_playerPB.avgV / EachNthFrame));

        ::printf("batches>       min: %3d    max: %3d    avg: %3d\n", (int)_batchCountPB.minV, (int)_batchCountPB.maxV, (int)(_batchCountPB.avgV / EachNthFrame));
        ::printf("draw calls>    min: %3d    max: %3d    avg: %3d\n", (int)_drawCallCountPB.minV, (int)_drawCallCountPB.maxV, (int)(_drawCallCountPB.avgV / EachNthFrame));
        ::printf("tris>          min: %5d  max: %5d  avg: %5d\n", (int)_triCountPB.minV, (int)_triCountPB.maxV, (int)(_triCountPB.avgV / EachNthFrame));
        ::printf("verts>         min: %5d  max: %5d  avg: %5d\n", (int)_vertCountPB.minV, (int)_vertCountPB.maxV, (int)(_vertCountPB.avgV / EachNthFrame));
        ::printf("dynamic batching> batched draw calls: %3d batches: %3d tris: %5d verts: %5d\n",
            (int)(_dynamicBatchedDrawCallCountPB.avgV / EachNthFrame),
            (int)(_dynamicBatchCountPB.avgV / EachNthFrame),
            (int)(_dynamicBatchedTriCountPB.avgV / EachNthFrame),
            (int)(_dynamicBatchedVertCountPB.avgV / EachNthFrame));
        ::printf("static batching>  batched draw calls: %3d batches: %3d tris: %5d verts: %5d\n",
            (int)(_staticBatchedDrawCallCountPB.avgV / EachNthFrame),
            (int)(_staticBatchCountPB.avgV / EachNthFrame),
            (int)(_staticBatchedTriCountPB.avgV / EachNthFrame),
            (int)(_staticBatchedVertCountPB.avgV / EachNthFrame));

        ::printf("player-detail> physx: %4.1f animation: %4.1f culling %4.1f skinning: %4.1f batching: %4.1f render: %4.1f fixed-update-count: %d .. %d\n",
            MachToMillisecondsDelta(_fixedPhysicsManagerPB.avgV / EachNthFrame),
            MachToMillisecondsDelta(_animationUpdatePB.avgV / EachNthFrame),
            MachToMillisecondsDelta(_unityCullingPB.avgV / EachNthFrame),
            MachToMillisecondsDelta(_skinMeshUpdatePB.avgV / EachNthFrame),
            MachToMillisecondsDelta(_dynamicBatchDtPB.avgV / EachNthFrame),
            MachToMillisecondsDelta((_unityRenderLoopPB.avgV - _dynamicBatchDtPB.avgV - _unityCullingPB.avgV) / EachNthFrame),
            (int)_fixedUpdateCountPB.minV, (int)_fixedUpdateCountPB.maxV);
        ::printf("scripting-scripts>  update: %4.1f   fixedUpdate: %4.1f coroutines: %4.1f \n", MachToMillisecondsDelta(_dynamicBehaviourManagerPB.avgV / EachNthFrame), MachToMillisecondsDelta(_fixedBehaviourManagerPB.avgV / EachNthFrame), MachToMillisecondsDelta(_coroutinePB.avgV / EachNthFrame));
#if UNITY_DEVELOPER_BUILD
        ::printf("scripting-memory>   used heap: %lld allocated heap: %lld  max number of collections: %d collection total duration: %4.1f\n", UnityScriptingGetUsedSize(), UnityScriptingGetHeapSize(), (int)_GCCountPB.avgV, MachToMillisecondsDelta(_GCDurationPB.avgV));
#else
        ::printf("scripting-memory>   information not available on non-development player configuration\n");
#endif
        ::printf("----------------------------------------\n");
    }
    ProfilerBlock_Update(&_framePB, _frameTime, (_frameId == 0));
    ProfilerBlock_Update(&_playerPB, _playerTime, (_frameId == 0));

    ProfilerBlock_Update(&_batchCountPB, _unityFrameStats.batchCount, (_frameId == 0));
    ProfilerBlock_Update(&_drawCallCountPB, _unityFrameStats.drawCallCount, (_frameId == 0));
    ProfilerBlock_Update(&_triCountPB, _unityFrameStats.triCount, (_frameId == 0));
    ProfilerBlock_Update(&_vertCountPB, _unityFrameStats.vertCount, (_frameId == 0));

    ProfilerBlock_Update(&_dynamicBatchDtPB, _unityFrameStats.dynamicBatchDt, (_frameId == 0));
    ProfilerBlock_Update(&_dynamicBatchCountPB, _unityFrameStats.dynamicBatchCount, (_frameId == 0));
    ProfilerBlock_Update(&_dynamicBatchedDrawCallCountPB, _unityFrameStats.dynamicBatchedDrawCallCount, (_frameId == 0));
    ProfilerBlock_Update(&_dynamicBatchedTriCountPB, _unityFrameStats.dynamicBatchedTris, (_frameId == 0));
    ProfilerBlock_Update(&_dynamicBatchedVertCountPB, _unityFrameStats.dynamicBatchedVerts, (_frameId == 0));

    ProfilerBlock_Update(&_staticBatchCountPB, _unityFrameStats.staticBatchCount, (_frameId == 0));
    ProfilerBlock_Update(&_staticBatchedDrawCallCountPB, _unityFrameStats.staticBatchedDrawCallCount, (_frameId == 0));
    ProfilerBlock_Update(&_staticBatchedTriCountPB, _unityFrameStats.staticBatchedTris, (_frameId == 0));
    ProfilerBlock_Update(&_staticBatchedVertCountPB, _unityFrameStats.staticBatchedVerts, (_frameId == 0));

    ProfilerBlock_Update(&_fixedBehaviourManagerPB, _unityFrameStats.fixedBehaviourManagerDt, (_frameId == 0));
    ProfilerBlock_Update(&_fixedPhysicsManagerPB, _unityFrameStats.fixedPhysicsManagerDt, (_frameId == 0));
    ProfilerBlock_Update(&_dynamicBehaviourManagerPB, _unityFrameStats.dynamicBehaviourManagerDt, (_frameId == 0));
    ProfilerBlock_Update(&_coroutinePB, _unityFrameStats.coroutineDt, (_frameId == 0));
    ProfilerBlock_Update(&_skinMeshUpdatePB, _unityFrameStats.skinMeshUpdateDt, (_frameId == 0));
    ProfilerBlock_Update(&_animationUpdatePB, _unityFrameStats.animationUpdateDt, (_frameId == 0));
    ProfilerBlock_Update(&_unityRenderLoopPB, _unityFrameStats.renderDt, (_frameId == 0));
    ProfilerBlock_Update(&_unityCullingPB, _unityFrameStats.cullingDt, (_frameId == 0));
    ProfilerBlock_Update(&_unityMSAAResolvePB, _msaaResolve, (_frameId == 0));
    ProfilerBlock_Update(&_fixedUpdateCountPB, _unityFrameStats.fixedUpdateCount, (_frameId == 0));
    ProfilerBlock_Update(&_GCCountPB, 0, (_frameId == 0));
    ProfilerBlock_Update(&_GCDurationPB, 0, (_frameId == 0));

    _msaaResolve = 0;
    ++_frameId;
}

void Profiler_StartMSAAResolve()
{
    UnityStartProfilerCounter(_msaaResolveCounter);
    _msaaResolveStart = mach_absolute_time();
}

void Profiler_EndMSAAResolve()
{
    _msaaResolve += (mach_absolute_time() - _msaaResolveStart);
    UnityEndProfilerCounter(_msaaResolveCounter);
}

//
// scriptint memory profiling
//

extern "C"
{
    enum ScriptingGCEvent
    {
        SCRIPTING_GC_EVENT_START,
        SCRIPTING_GC_EVENT_MARK_START,
        SCRIPTING_GC_EVENT_MARK_END,
        SCRIPTING_GC_EVENT_RECLAIM_START,
        SCRIPTING_GC_EVENT_RECLAIM_END,
        SCRIPTING_GC_EVENT_END,
        SCRIPTING_GC_EVENT_PRE_STOP_WORLD,
        SCRIPTING_GC_EVENT_POST_STOP_WORLD,
        SCRIPTING_GC_EVENT_PRE_START_WORLD,
        SCRIPTING_GC_EVENT_POST_START_WORLD
    };

    enum ScriptingProfileFlags
    {
        SCRIPTING_PROFILE_NONE              = 0,
        SCRIPTING_PROFILE_APPDOMAIN_EVENTS  = 1 << 0,
        SCRIPTING_PROFILE_ASSEMBLY_EVENTS   = 1 << 1,
        SCRIPTING_PROFILE_MODULE_EVENTS     = 1 << 2,
        SCRIPTING_PROFILE_CLASS_EVENTS      = 1 << 3,
        SCRIPTING_PROFILE_JIT_COMPILATION   = 1 << 4,
        SCRIPTING_PROFILE_INLINING          = 1 << 5,
        SCRIPTING_PROFILE_EXCEPTIONS        = 1 << 6,
        SCRIPTING_PROFILE_ALLOCATIONS       = 1 << 7,
        SCRIPTING_PROFILE_GC                = 1 << 8,
        SCRIPTING_PROFILE_THREADS           = 1 << 9,
        SCRIPTING_PROFILE_REMOTING          = 1 << 10,
        SCRIPTING_PROFILE_TRANSITIONS       = 1 << 11,
        SCRIPTING_PROFILE_ENTER_LEAVE       = 1 << 12,
        SCRIPTING_PROFILE_COVERAGE          = 1 << 13,
        SCRIPTING_PROFILE_INS_COVERAGE      = 1 << 14,
        SCRIPTING_PROFILE_STATISTICAL       = 1 << 15,
        SCRIPTING_PROFILE_METHOD_EVENTS     = 1 << 16,
        SCRIPTING_PROFILE_MONITOR_EVENTS    = 1 << 17,
        SCRIPTING_PROFILE_IOMAP_EVENTS      = 1 << 18, /* this should likely be removed, too */
        SCRIPTING_PROFILE_GC_MOVES          = 1 << 19,
    };

    struct MemoryProfiler
    {
        int64_t gc_total_time;
        int64_t gc_mark_time;
        int64_t gc_reclaim_time;
        int64_t gc_stop_world_time;
        int64_t gc_start_world_time;
    };

    typedef void (*UnityScriptingProfileFunc)(void* /*MemoryProfiler*/ prof);
    typedef void (*UnityScriptingProfileGCFunc)(void* /*MemoryProfiler*/ prof, int event, int generation);
    typedef void (*UnityScriptingProfileGCResizeFunc)(void* /*MemoryProfiler*/ prof, int64_t new_size);

    void    UnityScriptingProfilerInstall(void* /*MemoryProfiler*/ prof, UnityScriptingProfileFunc shutdown_callback);
    void    UnityScriptingProfilerInstallGC(UnityScriptingProfileGCFunc callback, UnityScriptingProfileGCResizeFunc heap_resize_callback);
    void    UnityScriptingProfilerSetEvents(int /*ScriptingProfileFlags*/ events);
}


static MemoryProfiler _MemoryProfiler;

static void gc_event(void* profiler_, int event, int generation)
{
    MemoryProfiler* profiler = (MemoryProfiler*)profiler_;
    switch (event)
    {
        case SCRIPTING_GC_EVENT_START:
            profiler->gc_total_time = mach_absolute_time();
            break;
        case SCRIPTING_GC_EVENT_END:
        {
            profiler->gc_total_time = mach_absolute_time() - profiler->gc_total_time;
            float delta = profiler->gc_total_time;
            ProfilerBlock_Update(&_GCDurationPB, delta, false);
            ProfilerBlock_Update(&_GCCountPB, 1, false);
            break;
        }
        case SCRIPTING_GC_EVENT_MARK_START:
            profiler->gc_mark_time = mach_absolute_time();
            break;
        case SCRIPTING_GC_EVENT_MARK_END:
            profiler->gc_mark_time = mach_absolute_time() - profiler->gc_mark_time;
            break;
        case SCRIPTING_GC_EVENT_RECLAIM_START:
            profiler->gc_reclaim_time = mach_absolute_time();
            break;
        case SCRIPTING_GC_EVENT_RECLAIM_END:
            profiler->gc_reclaim_time = mach_absolute_time() - profiler->gc_reclaim_time;
            break;
        case SCRIPTING_GC_EVENT_PRE_STOP_WORLD:
            profiler->gc_stop_world_time = mach_absolute_time();
            break;
        case SCRIPTING_GC_EVENT_POST_STOP_WORLD:
            profiler->gc_stop_world_time = mach_absolute_time() - profiler->gc_stop_world_time;
            break;
        case SCRIPTING_GC_EVENT_PRE_START_WORLD:
            profiler->gc_start_world_time = mach_absolute_time();
            break;
        case SCRIPTING_GC_EVENT_POST_START_WORLD:
            profiler->gc_start_world_time = mach_absolute_time() - profiler->gc_start_world_time;
            break;
        default:
            break;
    }

#if ENABLE_DETAILED_GC_STATS
    if (event == SCRIPTING_GC_EVENT_END)
    {
        ::printf("scripting-gc>   stop time: %4.1f mark time: %4.1f reclaim time: %4.1f start time: %4.1f total time: %4.1f \n",
            MachToMillisecondsDelta(profiler->gc_stop_world_time),
            MachToMillisecondsDelta(profiler->gc_mark_time),
            MachToMillisecondsDelta(profiler->gc_reclaim_time),
            MachToMillisecondsDelta(profiler->gc_start_world_time),
            MachToMillisecondsDelta(profiler->gc_total_time)
        );
    }
#endif
}

static void gc_resize(void* profiler, int64_t new_size)
{
}

static void profiler_shutdown(void* profiler)
{
}

static void Profiler_SetupScriptingProfile()
{
    UnityScriptingProfilerInstall(&_MemoryProfiler, &profiler_shutdown);
    UnityScriptingProfilerInstallGC(&gc_event, &gc_resize);
    UnityScriptingProfilerSetEvents(SCRIPTING_PROFILE_GC);
}

#endif // ENABLE_INTERNAL_PROFILER
