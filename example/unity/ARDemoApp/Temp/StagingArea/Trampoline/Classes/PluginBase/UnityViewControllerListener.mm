#include "UnityViewControllerListener.h"
#include <UIKit/UIApplication.h>

#define DEFINE_NOTIFICATION(name) extern "C" __attribute__((visibility ("default"))) NSString* const name = @#name;

DEFINE_NOTIFICATION(kUnityViewWillLayoutSubviews);
DEFINE_NOTIFICATION(kUnityViewDidLayoutSubviews);
DEFINE_NOTIFICATION(kUnityViewWillDisappear);
DEFINE_NOTIFICATION(kUnityViewDidDisappear);
DEFINE_NOTIFICATION(kUnityViewWillAppear);
DEFINE_NOTIFICATION(kUnityViewDidAppear);
DEFINE_NOTIFICATION(kUnityInterfaceWillChangeOrientation);
DEFINE_NOTIFICATION(kUnityInterfaceDidChangeOrientation);

#undef DEFINE_NOTIFICATION

void UnityRegisterViewControllerListener(id<UnityViewControllerListener> obj)
{
    #define REGISTER_SELECTOR(sel, notif_name)                                                          \
    if([obj respondsToSelector:sel])                                                                    \
        [[NSNotificationCenter defaultCenter] addObserver:obj selector:sel name:notif_name object:nil]; \

    REGISTER_SELECTOR(@selector(viewWillLayoutSubviews:), kUnityViewWillLayoutSubviews);
    REGISTER_SELECTOR(@selector(viewDidLayoutSubviews:), kUnityViewDidLayoutSubviews);
    REGISTER_SELECTOR(@selector(viewWillDisappear:), kUnityViewWillDisappear);
    REGISTER_SELECTOR(@selector(viewDidDisappear:), kUnityViewDidDisappear);
    REGISTER_SELECTOR(@selector(viewWillAppear:), kUnityViewWillAppear);
    REGISTER_SELECTOR(@selector(viewDidAppear:), kUnityViewDidAppear);
    REGISTER_SELECTOR(@selector(interfaceWillChangeOrientation:), kUnityInterfaceWillChangeOrientation);
    REGISTER_SELECTOR(@selector(interfaceDidChangeOrientation:), kUnityInterfaceDidChangeOrientation);

    #undef REGISTER_SELECTOR
}

void UnityUnregisterViewControllerListener(id<UnityViewControllerListener> obj)
{
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityViewWillLayoutSubviews object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityViewDidLayoutSubviews object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityViewWillDisappear object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityViewDidDisappear object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityViewWillAppear object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityViewDidAppear object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityInterfaceWillChangeOrientation object: nil];
    [[NSNotificationCenter defaultCenter] removeObserver: obj name: kUnityInterfaceDidChangeOrientation object: nil];
}
