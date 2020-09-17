#include <csignal>
#import <UIKit/UIKit.h>
#import "UnityUtils.h"

#include <UnityFramework/UnityFramework.h>

// Hack to work around iOS SDK 4.3 linker problem
// we need at least one __TEXT, __const section entry in main application .o files
// to get this section emitted at right time and so avoid LC_ENCRYPTION_INFO size miscalculation
static const int constsection = 0;

static bool player_created = false;
bool unity_inited = false;
bool is_unity_loaded = false;
bool is_unity_in_background = false;
bool is_unity_paused = false;
bool disabled_unload = false;

// keep arg for unity init from non main
int g_argc;
char** g_argv;
NSDictionary* appLaunchOpts;

void UnityInitTrampoline();

UnityFramework* ufw;

extern "C" void InitArgs(int argc, char* argv[])
{
    g_argc = argc;
    g_argv = argv;
}

extern "C" bool UnityIsInited()
{
    return unity_inited;
}

extern "C" bool IsUnityInBackground()
{
    return is_unity_in_background;
}

extern "C" bool IsUnityPaused()
{
    return is_unity_paused;
}

extern "C" bool IsUnityLoaded()
{
    return is_unity_loaded;
}

UnityFramework* UnityFrameworkLoad()
{
    NSString* bundlePath = nil;
    bundlePath = [[NSBundle mainBundle] bundlePath];
    bundlePath = [bundlePath stringByAppendingString: @"/Frameworks/UnityFramework.framework"];

    NSBundle* bundle = [NSBundle bundleWithPath: bundlePath];
    if ([bundle isLoaded] == false) [bundle load];

    UnityFramework* ufw = [bundle.principalClass getInstance];
    return ufw;
}

extern "C" void InitUnity()
{
    if (unity_inited) {
        return;
    }
    unity_inited = true;

    ufw = UnityFrameworkLoad();

    [ufw setDataBundleId: "com.unity3d.framework"];
    [ufw frameworkWarmup: g_argc argv: g_argv];
    
    // [ufw setExecuteHeader: &_mh_execute_header];
    // [ufw runEmbeddedWithArgc: g_argc argv: g_argv appLaunchOpts: appLaunchOpts];
}

extern "C" void UnityPostMessage(NSString* gameObject, NSString* methodName, NSString* message)
{
    if (is_unity_loaded) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [ufw sendMessageToGOWithName:[gameObject UTF8String] functionName:[methodName UTF8String] message:[message UTF8String]];
        });
    }
}

extern "C" void UnityPauseCommand()
{
    if (is_unity_loaded) {
        dispatch_async(dispatch_get_main_queue(), ^{
            is_unity_paused = true;
            [ufw pause:true];
        });
    }
}

extern "C" void UnityResumeCommand()
{
    if (is_unity_loaded) {
        dispatch_async(dispatch_get_main_queue(), ^{
            is_unity_paused = false;
            [ufw pause:false];
        });
    }
}


extern "C" void UnityUnloadCommand()
{
    if (is_unity_loaded) {
        dispatch_async(dispatch_get_main_queue(), ^{
            is_unity_loaded = false;
            unity_inited = false;
            [ufw unloadApplication];
        });
    }
}



extern "C" void UnityShowWindowCommand()
{
    if (is_unity_loaded) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [ufw showUnityWindow];
        });
    }
}

extern "C" void UnityQuitCommand()
{
    if (is_unity_loaded) {
        dispatch_async(dispatch_get_main_queue(), ^{
            is_unity_loaded = false;
            [ufw quitApplication:(0)];
        });
    }
}

@implementation UnityUtils

static NSHashTable* mUnityEventListeners = [NSHashTable weakObjectsHashTable];
static BOOL _isUnityReady = NO;
UnityAppController *controller;

+ (BOOL)isUnityReady
{
    return _isUnityReady;
}


+ (void)resetUnityReady
{
    _isUnityReady = NO;
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
    if (_isUnityReady && is_unity_loaded) {
        completed();
        return;
    }

    [[NSNotificationCenter defaultCenter] addObserverForName:@"UnityReady" object:nil queue:[NSOperationQueue mainQueue]  usingBlock:^(NSNotification * _Nonnull note) {
        _isUnityReady = YES;
        completed();
    }];

    if (UnityIsInited() || (player_created && IsUnityLoaded())) {
        return;
    }
    player_created = true;
    is_unity_loaded = true;
    
    dispatch_async(dispatch_get_main_queue(), ^{
        UIApplication* application = [UIApplication sharedApplication];

        // Always keep Flutter window on top
        UIWindow* flutterUIWindow = application.keyWindow;
        flutterUIWindow.windowLevel = UIWindowLevelNormal + 1;// Always keep Flutter window in top
        application.keyWindow.windowLevel = UIWindowLevelNormal + 1;

        InitUnity();

        controller = GetAppController();
        [controller application:application didFinishLaunchingWithOptions:nil];
        [controller applicationDidBecomeActive:application];

        [UnityUtils listenAppState];

        // Make Flutter the key window again after initializing Unity
        // This avoids other Flutter plugins attaching native UIViews to the Unity UIWindow
        [flutterUIWindow makeKeyWindow];
    });
}


+ (void)recreatePlayer:(void (^)(void))completed
{
    if (_isUnityReady && is_unity_loaded) {
        completed();
        return;
    }

    [[NSNotificationCenter defaultCenter] addObserverForName:@"UnityReady" object:nil queue:[NSOperationQueue mainQueue]  usingBlock:^(NSNotification * _Nonnull note) {
        _isUnityReady = YES;
        completed();
    }];

    if (UnityIsInited() || (player_created && IsUnityLoaded())) {
        return;
    }
    player_created = true;
    is_unity_loaded = true;
    
    dispatch_async(dispatch_get_main_queue(), ^{
        UIApplication* application = [UIApplication sharedApplication];

        // Always keep Flutter window on top
        UIWindow* flutterUIWindow = application.keyWindow;
        flutterUIWindow.windowLevel = UIWindowLevelNormal + 1;// Always keep Flutter window in top
        application.keyWindow.windowLevel = UIWindowLevelNormal + 1;
        InitUnity();
        
        // [controller application:application didFinishLaunchingWithOptions:nil];
        // [controller applicationDidBecomeActive:application];
        
        [UnityUtils listenAppState];
        
        // Make Flutter the key window again after initializing Unity
        // This avoids other Flutter plugins attaching native UIViews to the Unity UIWindow
        [flutterUIWindow makeKeyWindow];
    });
}
@end
