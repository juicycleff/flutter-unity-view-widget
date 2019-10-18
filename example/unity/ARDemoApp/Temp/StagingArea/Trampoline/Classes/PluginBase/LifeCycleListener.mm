#include "LifeCycleListener.h"

#define DEFINE_NOTIFICATION(name) extern "C" __attribute__((visibility ("default"))) NSString* const name = @#name;
DEFINE_NOTIFICATION(kUnityDidUnload);

void UnityRegisterLifeCycleListener(id<LifeCycleListener> obj)
{
    #define REGISTER_SELECTOR(sel, notif_name)                  \
    if([obj respondsToSelector:sel])                            \
        [[NSNotificationCenter defaultCenter]   addObserver:obj \
                                                selector:sel    \
                                                name:notif_name \
                                                object:nil      \
        ];                                                      \

    REGISTER_SELECTOR(@selector(didFinishLaunching:), UIApplicationDidFinishLaunchingNotification);
    REGISTER_SELECTOR(@selector(didBecomeActive:), UIApplicationDidBecomeActiveNotification);
    REGISTER_SELECTOR(@selector(willResignActive:), UIApplicationWillResignActiveNotification);
    REGISTER_SELECTOR(@selector(didEnterBackground:), UIApplicationDidEnterBackgroundNotification);
    REGISTER_SELECTOR(@selector(willEnterForeground:), UIApplicationWillEnterForegroundNotification);
    REGISTER_SELECTOR(@selector(willTerminate:), UIApplicationWillTerminateNotification);
    REGISTER_SELECTOR(@selector(unityDidUnload:), kUnityDidUnload);

    #undef REGISTER_SELECTOR
}

void UnityUnregisterLifeCycleListener(id<LifeCycleListener> obj)
{
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationDidFinishLaunchingNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationDidBecomeActiveNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationWillResignActiveNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationDidEnterBackgroundNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationWillEnterForegroundNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationWillTerminateNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityDidUnload object: nil];
}
