#include "RenderPluginDelegate.h"

@implementation RenderPluginDelegate

- (void)mainDisplayInited:(struct UnityDisplaySurfaceBase*)surface
{
    mainDisplaySurface = surface;

    // TODO: move lifecycle to init?
    UnityRegisterLifeCycleListener(self);
}

@end


#define CALL_METHOD_ON_ARRAY(method)                    \
do{                                                     \
    for(id<RenderPluginDelegate> del in delegateArray)  \
    {                                                   \
        if([del respondsToSelector:@selector(method)])  \
            [del method];                               \
    }                                                   \
} while(0)

#define CALL_METHOD_ON_ARRAY_ARG(method, arg)           \
do{                                                     \
    for(id<RenderPluginDelegate> del in delegateArray)  \
    {                                                   \
        if([del respondsToSelector:@selector(method:)]) \
            [del method:arg];                           \
    }                                                   \
} while(0)


@implementation RenderPluginArrayDelegate

@synthesize delegateArray;

- (void)mainDisplayInited:(struct UnityDisplaySurfaceBase*)surface
{
    [super mainDisplayInited: surface];
    CALL_METHOD_ON_ARRAY_ARG(mainDisplayInited, surface);
}

- (void)onBeforeMainDisplaySurfaceRecreate:(struct RenderingSurfaceParams*)params
{
    CALL_METHOD_ON_ARRAY_ARG(onBeforeMainDisplaySurfaceRecreate, params);
}

- (void)onAfterMainDisplaySurfaceRecreate;
{
    CALL_METHOD_ON_ARRAY(onAfterMainDisplaySurfaceRecreate);
}

- (void)onFrameResolved;
{
    CALL_METHOD_ON_ARRAY(onFrameResolved);
}


- (void)didBecomeActive:(NSNotification*)notification
{
    CALL_METHOD_ON_ARRAY_ARG(didBecomeActive, notification);
}

- (void)willResignActive:(NSNotification*)notification
{
    CALL_METHOD_ON_ARRAY_ARG(willResignActive, notification);
}

- (void)didEnterBackground:(NSNotification*)notification
{
    CALL_METHOD_ON_ARRAY_ARG(didEnterBackground, notification);
}

- (void)willEnterForeground:(NSNotification*)notification
{
    CALL_METHOD_ON_ARRAY_ARG(willEnterForeground, notification);
}

- (void)willTerminate:(NSNotification*)notification
{
    CALL_METHOD_ON_ARRAY_ARG(willTerminate, notification);
}

@end


#undef CALL_METHOD_ON_ARRAY
#undef CALL_METHOD_ON_ARRAY_ARG
