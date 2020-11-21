#include <csignal>
#import <UIKit/UIKit.h>
#import "UnityUtils.h"

#include <UnityFramework/UnityFramework.h>

static bool unity_warmed_up = false;

// Load unity framework for fisrt run
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

// Hack to work around iOS SDK 4.3 linker problem
// we need at least one __TEXT, __const section entry in main application .o files
// to get this section emitted at right time and so avoid LC_ENCRYPTION_INFO size miscalculation
static const int constsection = 0;

// keep arg for unity init from non main
int gArgc = 0;
char** gArgv = nullptr;
NSDictionary* appLaunchOpts = [[NSDictionary alloc] init];

UnityUtils* hostDelegate = NULL;

extern "C" void InitArgs(int argc, char* argv[])
{
    gArgc = argc;
    gArgv = argv;
}

// -------------------------------
// -------------------------------
// -------------------------------

@implementation UnityUtils

static NSHashTable* mUnityEventListeners = [NSHashTable weakObjectsHashTable];
UnityAppController *controller;

static bool _isUnityPaused = false;
static bool _isUnityReady = false;
static bool _isUnityLoaded = false;

- (bool)unityIsInitialized
{
    return [self ufw] && [[self ufw] appController];
}

// initialize unity framework
- (void)initUnity
{
    if([self unityIsInitialized]) {
        [[self ufw] showUnityWindow];
        return;
    }

    [self setUfw: UnityFrameworkLoad()];
    
    [[self ufw] setDataBundleId: "com.unity3d.framework"];
    [[self ufw] registerFrameworkListener: self];

    [self registerUnityListener];
    [[self ufw] frameworkWarmup: gArgc argv: gArgv];

    // [[self ufw] runEmbeddedWithArgc: gArgc argv: gArgv appLaunchOpts: appLaunchOpts];
    [[[[self ufw] appController] window] setWindowLevel: UIWindowLevelNormal - 1];
    _isUnityLoaded = true;
}

- (void)unregisterUnityListener
{
    if ([self unityIsInitialized]) {
        [[self ufw] unregisterFrameworkListener: self];
    }
}

- (void)registerUnityListener
{
    if ([self unityIsInitialized]) {
        [[self ufw] registerFrameworkListener: self];
    }
}

- (void)unityDidUnload:(NSNotification*)notification
{
    NSLog(@"unityDidUnloaded called");

    [self unregisterUnityListener];
    [[self ufw] unregisterFrameworkListener: self];
    [self setUfw: nil];
    _isUnityReady = false;
    _isUnityLoaded = false;
}

// manage all app state notification
- (void)handleAppStateDidChange:(NSNotification *)notification
{
    if (!_isUnityReady) {
        return;
    }
    
    UnityAppController* unityAppController = [[self ufw] appController];
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

// Listener for app lifecycle eventa
- (void)listenAppState
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

// Create new unity player
- (void)createPlayer:(void (^)(void))completed
{
    if ([self unityIsInitialized] && _isUnityReady) {
        completed();
        return;
    }

    [[NSNotificationCenter defaultCenter] addObserverForName:@"UnityReady" object:nil queue:[NSOperationQueue mainQueue]  usingBlock:^(NSNotification * _Nonnull note) {
        _isUnityReady = true;
        completed();
    }];

    dispatch_async(dispatch_get_main_queue(), ^{
        UIApplication* application = [UIApplication sharedApplication];
        
        // Always keep Flutter window on top
        UIWindow* flutterUIWindow = application.keyWindow;
        flutterUIWindow.windowLevel = UIWindowLevelNormal + 1;// Always keep Flutter window in top
        application.keyWindow.windowLevel = UIWindowLevelNormal + 1;
        
        [self initUnity];

        if(!unity_warmed_up) {
            controller = GetAppController();
            [controller application:application didFinishLaunchingWithOptions:nil];
            [controller applicationDidBecomeActive:application];
            unity_warmed_up = true;
        }
        
        [self listenAppState];
    });
}

// Pause unity player
- (void)pauseUnity
{
    NSLog(@"Pause called = %i", _isUnityPaused);
    if (!_isUnityPaused) {
        id app = [UIApplication sharedApplication];
        id appController = [[self ufw] appController];
        [appController applicationWillResignActive: app];
        _isUnityPaused = true;
    }
}

// Resume unity player
- (void)resumeUnity
{
    NSLog(@"Resume called = %i", _isUnityPaused);
    if (_isUnityPaused) {
        id app = [UIApplication sharedApplication];
        id appController = [[self ufw] appController];
        [appController applicationWillEnterForeground: app];
        [appController applicationDidBecomeActive: app];
        _isUnityPaused = false;
    }
}

// Unload unity resources
- (void)unloadUnity
{
    if([self unityIsInitialized]){
        [UnityFrameworkLoad() unloadApplication];
    }
}

// Quit unity application
- (void)quitUnity
{
    if([self unityIsInitialized]){
        [UnityFrameworkLoad() unloadApplication];
    }
}

// Check if unity is paused
- (bool)isUnityPaused
{
    return _isUnityPaused;
}

// Is unity loaded
- (bool)isUnityLoaded
{
    return _isUnityLoaded;
}

// Post unity application
- (void)unityPostMessage:(NSString*) gameObject unityMethodName: (NSString*) methodName unityMessage: (NSString*) message;
{
    if([self unityIsInitialized]){
        [[self ufw] sendMessageToGOWithName:[gameObject UTF8String] functionName:[methodName UTF8String] message:[message UTF8String]];
    }
}

@end
