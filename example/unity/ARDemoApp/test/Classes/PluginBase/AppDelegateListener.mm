#include "AppDelegateListener.h"

#define DEFINE_NOTIFICATION(name) extern "C" __attribute__((visibility ("default"))) NSString* const name = @#name;

DEFINE_NOTIFICATION(kUnityDidRegisterForRemoteNotificationsWithDeviceToken);
DEFINE_NOTIFICATION(kUnityDidFailToRegisterForRemoteNotificationsWithError);
DEFINE_NOTIFICATION(kUnityDidReceiveRemoteNotification);
DEFINE_NOTIFICATION(kUnityDidReceiveLocalNotification);
DEFINE_NOTIFICATION(kUnityOnOpenURL);
DEFINE_NOTIFICATION(kUnityWillFinishLaunchingWithOptions);
DEFINE_NOTIFICATION(kUnityHandleEventsForBackgroundURLSession);

#undef DEFINE_NOTIFICATION

void UnityRegisterAppDelegateListener(id<AppDelegateListener> obj)
{
    #define REGISTER_SELECTOR(sel, notif_name)                  \
    if([obj respondsToSelector:sel])                            \
        [[NSNotificationCenter defaultCenter]   addObserver:obj \
                                                selector:sel    \
                                                name:notif_name \
                                                object:nil      \
        ];                                                      \

    UnityRegisterLifeCycleListener(obj);

    REGISTER_SELECTOR(@selector(didRegisterForRemoteNotificationsWithDeviceToken:), kUnityDidRegisterForRemoteNotificationsWithDeviceToken);
    REGISTER_SELECTOR(@selector(didFailToRegisterForRemoteNotificationsWithError:), kUnityDidFailToRegisterForRemoteNotificationsWithError);
    REGISTER_SELECTOR(@selector(didReceiveRemoteNotification:), kUnityDidReceiveRemoteNotification);
    REGISTER_SELECTOR(@selector(didReceiveLocalNotification:), kUnityDidReceiveLocalNotification);
    REGISTER_SELECTOR(@selector(onOpenURL:), kUnityOnOpenURL);

    REGISTER_SELECTOR(@selector(applicationDidReceiveMemoryWarning:), UIApplicationDidReceiveMemoryWarningNotification);
    REGISTER_SELECTOR(@selector(applicationSignificantTimeChange:), UIApplicationSignificantTimeChangeNotification);
#if !PLATFORM_TVOS
    REGISTER_SELECTOR(@selector(applicationWillChangeStatusBarFrame:), UIApplicationWillChangeStatusBarFrameNotification);
    REGISTER_SELECTOR(@selector(applicationWillChangeStatusBarOrientation:), UIApplicationWillChangeStatusBarOrientationNotification);
#endif

    REGISTER_SELECTOR(@selector(applicationWillFinishLaunchingWithOptions:), kUnityWillFinishLaunchingWithOptions);
    REGISTER_SELECTOR(@selector(onHandleEventsForBackgroundURLSession:), kUnityHandleEventsForBackgroundURLSession);

    #undef REGISTER_SELECTOR
}

void UnityUnregisterAppDelegateListener(id<AppDelegateListener> obj)
{
    UnityUnregisterLifeCycleListener(obj);

    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityDidRegisterForRemoteNotificationsWithDeviceToken object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityDidFailToRegisterForRemoteNotificationsWithError object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityDidReceiveRemoteNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityDidReceiveLocalNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityOnOpenURL object: nil];

    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationDidReceiveMemoryWarningNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationSignificantTimeChangeNotification object: nil];
#if !PLATFORM_TVOS
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationWillChangeStatusBarFrameNotification object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: UIApplicationWillChangeStatusBarOrientationNotification object: nil];
#endif
}
