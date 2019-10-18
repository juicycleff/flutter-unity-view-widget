#include "RegisterFeatures.h"
#include <csignal>
#include "UnityInterface.h"
#include "../UnityFramework/UnityFramework.h"

void UnityInitTrampoline();

// WARNING: this MUST be c decl (NSString ctor will be called after +load, so we cant really change its value)
const char* AppControllerClassName = "UnityAppController";

#if UNITY_USES_DYNAMIC_PLAYER_LIB
extern "C" void SetAllUnityFunctionsForDynamicPlayerLib();
#endif

extern "C" void UnitySetExecuteMachHeader(const MachHeader* header);

extern "C" __attribute__((visibility("default"))) NSString* const kUnityDidUnload;

@implementation UnityFramework
{
    bool inSceneLessMode;
}

UnityFramework* _gUnityFramework = nil;
+ (UnityFramework*)getInstance
{
    if (_gUnityFramework == nil)
    {
        _gUnityFramework = [[UnityFramework alloc] init];
    }
    return _gUnityFramework;
}

- (UnityAppController*)appController
{
    return GetAppController();
}

- (void)setExecuteHeader:(const MachHeader*)header
{
    UnitySetExecuteMachHeader(header);
}

- (void)sendMessageToGOWithName:(const char*)goName functionName:(const char*)name message:(const char*)msg
{
    UnitySendMessage(goName, name, msg);
}

- (void)registerFrameworkListener:(id<UnityFrameworkListener>)obj
{
#define REGISTER_SELECTOR(sel, notif_name)                  \
if([obj respondsToSelector:sel])                        \
[[NSNotificationCenter defaultCenter]   addObserver:obj \
selector:sel                                            \
name:notif_name                                         \
object:nil];


    REGISTER_SELECTOR(@selector(unityDidUnload:), kUnityDidUnload);

#undef REGISTER_SELECTOR
}

- (void)unregisterFrameworkListener:(id<UnityFrameworkListener>)obj
{
#define REGISTER_SELECTOR(sel, notif_name)                                                          \
if([obj respondsToSelector:sel])                                                                \
[[NSNotificationCenter defaultCenter] addObserver:obj selector:sel name:notif_name object:nil];

    REGISTER_SELECTOR(@selector(unityDidUnload:), kUnityDidUnload);

#undef REGISTER_SELECTOR
}

- (void)frameworkWarmup:(int)argc argv:(char*[])argv
{
#if UNITY_USES_DYNAMIC_PLAYER_LIB
    SetAllUnityFunctionsForDynamicPlayerLib();
#endif


    UnityInitTrampoline();
    UnityInitRuntime(argc, argv);

    RegisterFeatures();

    // iOS terminates open sockets when an application enters background mode.
    // The next write to any of such socket causes SIGPIPE signal being raised,
    // even if the request has been done from scripting side. This disables the
    // signal and allows Mono to throw a proper C# exception.
    std::signal(SIGPIPE, SIG_IGN);
}

- (void)setDataBundleId:(const char*)bundleId
{
    UnitySetDataBundleDirWithBundleId(bundleId);
}

- (void)runUIApplicationMainWithArgc:(int)argc argv:(char*[])argv
{
    [self frameworkWarmup: argc argv: argv];
    UIApplicationMain(argc, argv, nil, [NSString stringWithUTF8String: AppControllerClassName]);
}

- (void)runEmbeddedWithArgc:(int)argc argv:(char*[])argv appLaunchOpts:(NSDictionary*)appLaunchOpts
{
    if (self->inSceneLessMode)
    {
        // initialize from partial unload ( sceneLessMode & onPause )
        UnityLoadApplicationFromSceneLessState();
        [self pause: false];
        [self showUnityWindow];
        self->inSceneLessMode = false;
    }
    else
    {
        // full initialization from ground up
        [self frameworkWarmup: argc argv: argv];

        id app = [UIApplication sharedApplication];

        id appCtrl = [[NSClassFromString([NSString stringWithUTF8String: AppControllerClassName]) alloc] init];
        [appCtrl application: app didFinishLaunchingWithOptions: appLaunchOpts];

        [appCtrl applicationWillEnterForeground: app];
        [appCtrl applicationDidBecomeActive: app];
    }
}

extern "C" void UnityResetSceneManager();
- (void)unloadApplication:(bool)allowReload
{
    if (allowReload)
    {
        UnityResetSceneManager();
        [self pause: true];
        self->inSceneLessMode = true;
        [[NSNotificationCenter defaultCenter] postNotificationName: @"kUnityDidUnload" object: nil];
    }
    else
    {
        self->inSceneLessMode = false;
        UnityUnloadApplication();
    }
}

- (void)showUnityWindow
{
    [[[self appController] window] makeKeyAndVisible];
}

- (void)pause:(bool)pause
{
    UnityPause(pause);
}

@end


#if TARGET_IPHONE_SIMULATOR && TARGET_TVOS_SIMULATOR
#include <pthread.h>

extern "C" int pthread_cond_init$UNIX2003(pthread_cond_t *cond, const pthread_condattr_t *attr)
{ return pthread_cond_init(cond, attr); }
extern "C" int pthread_cond_destroy$UNIX2003(pthread_cond_t *cond)
{ return pthread_cond_destroy(cond); }
extern "C" int pthread_cond_wait$UNIX2003(pthread_cond_t *cond, pthread_mutex_t *mutex)
{ return pthread_cond_wait(cond, mutex); }
extern "C" int pthread_cond_timedwait$UNIX2003(pthread_cond_t *cond, pthread_mutex_t *mutex,
    const struct timespec *abstime)
{ return pthread_cond_timedwait(cond, mutex, abstime); }

#endif // TARGET_IPHONE_SIMULATOR && TARGET_TVOS_SIMULATOR
