#include "RegisterMonoModules.h"
#include "RegisterFeatures.h"
#include <csignal>
#import <UIKit/UIKit.h>
#import "UnityInterface.h"
#import "UnityUtils.h"
#import "UnityAppController.h"

// Hack to work around iOS SDK 4.3 linker problem
// we need at least one __TEXT, __const section entry in main application .o files
// to get this section emitted at right time and so avoid LC_ENCRYPTION_INFO size miscalculation
static const int constsection = 0;

bool unity_inited = false;

int g_argc;
char** g_argv;

void UnityInitTrampoline();

extern "C" void InitArgs(int argc, char* argv[])
{
    g_argc = argc;
    g_argv = argv;
}

extern "C" bool UnityIsInited()
{
    return unity_inited;
}

extern "C" void InitUnity()
{
    if (unity_inited) {
        return;
    }
    unity_inited = true;

    UnityInitStartupTime();

    @autoreleasepool
    {
        UnityInitTrampoline();
        UnityInitRuntime(g_argc, g_argv);

        RegisterMonoModules();
        NSLog(@"-> registered mono modules %p\n", &constsection);
        RegisterFeatures();

        // iOS terminates open sockets when an application enters background mode.
        // The next write to any of such socket causes SIGPIPE signal being raised,
        // even if the request has been done from scripting side. This disables the
        // signal and allows Mono to throw a proper C# exception.
        std::signal(SIGPIPE, SIG_IGN);
    }
}

extern "C" void UnityPostMessage(NSString* gameObject, NSString* methodName, NSString* message)
{
    UnitySendMessage([gameObject UTF8String], [methodName UTF8String], [message UTF8String]);
}

extern "C" void UnityPauseCommand()
{
    dispatch_async(dispatch_get_main_queue(), ^{
        UnityPause(1);
    });
}

extern "C" void UnityResumeCommand()
{
    dispatch_async(dispatch_get_main_queue(), ^{
        UnityPause(0);
    });
}

@implementation UnityUtils

static NSHashTable* mUnityEventListeners = [NSHashTable weakObjectsHashTable];
static BOOL _isUnityReady = NO;

+ (BOOL)isUnityReady
{
    return _isUnityReady;
}

+ (void)handleAppStateDidChange:(NSNotification *)notification
{
    if (!_isUnityReady) {
        return;
    }
    UnityAppController* unityAppController = GetAppController();

    UIApplication* application = [UIApplication sharedApplication];

    if ([notification.name isEqualToString:UIApplicationWillResignActiveNotification]) {
        [unityAppController applicationWillResignActive:application];
    } else if ([notification.name isEqualToString:UIApplicationDidEnterBackgroundNotification]) {
        [unityAppController applicationDidEnterBackground:application];
    } else if ([notification.name isEqualToString:UIApplicationWillEnterForegroundNotification]) {
        [unityAppController applicationWillEnterForeground:application];
    } else if ([notification.name isEqualToString:UIApplicationDidBecomeActiveNotification]) {
        [unityAppController applicationDidBecomeActive:application];
    } else if ([notification.name isEqualToString:UIApplicationWillTerminateNotification]) {
        [unityAppController applicationWillTerminate:application];
    } else if ([notification.name isEqualToString:UIApplicationDidReceiveMemoryWarningNotification]) {
        [unityAppController applicationDidReceiveMemoryWarning:application];
    }
}

+ (void)listenAppState
{
    for (NSString *name in @[UIApplicationDidBecomeActiveNotification,
                             UIApplicationDidEnterBackgroundNotification,
                             UIApplicationWillTerminateNotification,
                             UIApplicationWillResignActiveNotification,
                             UIApplicationWillEnterForegroundNotification,
                             UIApplicationDidReceiveMemoryWarningNotification]) {

        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(handleAppStateDidChange:)
                                                     name:name
                                                   object:nil];
    }
}

+ (void)createPlayer:(void (^)(void))completed
{
    if (_isUnityReady) {
        completed();
        return;
    }

    [[NSNotificationCenter defaultCenter] addObserverForName:@"UnityReady" object:nil queue:[NSOperationQueue mainQueue]  usingBlock:^(NSNotification * _Nonnull note) {
        _isUnityReady = YES;
        completed();
    }];

    if (UnityIsInited()) {
        return;
    }

    dispatch_async(dispatch_get_main_queue(), ^{
        UIApplication* application = [UIApplication sharedApplication];

        // Always keep RN window in top
        application.keyWindow.windowLevel = UIWindowLevelNormal + 1;

        InitUnity();

        UnityAppController *controller = GetAppController();
        [controller application:application didFinishLaunchingWithOptions:nil];
        [controller applicationDidBecomeActive:application];

        [UnityUtils listenAppState];
    });
}

extern "C" void onUnityMessage(const char* message)
{
    for (id<UnityEventListener> listener in mUnityEventListeners) {
        [listener onMessage:[NSString stringWithUTF8String:message]];
    }
}

+ (void)addUnityEventListener:(id<UnityEventListener>)listener
{
    [mUnityEventListeners addObject:listener];
}

+ (void)removeUnityEventListener:(id<UnityEventListener>)listener
{
    [mUnityEventListeners removeObject:listener];
}

@end
