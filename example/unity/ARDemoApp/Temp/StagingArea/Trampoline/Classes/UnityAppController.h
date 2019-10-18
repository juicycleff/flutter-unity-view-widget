#pragma once

#import <QuartzCore/CADisplayLink.h>

#include "RenderPluginDelegate.h"

@class UnityView;
@class UnityViewControllerBase;
@class DisplayConnection;

__attribute__ ((visibility("default")))
@interface UnityAppController : NSObject<UIApplicationDelegate>
{
    UnityView*          _unityView;
    CADisplayLink*      _displayLink;

    UIWindow*           _window;
    UIView*             _rootView;
    UIViewController*   _rootController;
    UIView*             _snapshotView;

    DisplayConnection*  _mainDisplay;

    // We will cache view controllers used for fixed orientation (indexed by UIInterfaceOrientation).
    // Default view contoller goes to index 0. The default view controller is used when autorotation is enabled.
    //
    // There's no way to force iOS to change orientation when autorotation is enabled and the current orientation is disabled.
    // [UIViewController attemptRotationToDeviceOrientation] is insufficient to force iOS to change orientation in this circumstance.
    // We will recreate _viewControllerForOrientation[0] in that case immediately (see checkOrientationRequest for more comments)
#if UNITY_SUPPORT_ROTATION
    UIViewController*       _viewControllerForOrientation[5];
    UIInterfaceOrientation  _curOrientation;
#else
    UIViewController*       _viewControllerForOrientation[1];
#endif

    id<RenderPluginDelegate>    _renderDelegate;
}

// override it to add your render plugin delegate
- (void)shouldAttachRenderDelegate;

// this one is called at the very end of didFinishLaunchingWithOptions:
// after views have been created but before initing engine itself
// override it to register plugins, tweak UI etc
- (void)preStartUnity;

// this one is called at first applicationDidBecomeActive
// NB: it will be started with delay 0, so it will run on next run loop iteration
// this is done to make sure that activity indicator animation starts before blocking loading
- (void)startUnity:(UIApplication*)application;

// this is a part of UIApplicationDelegate protocol starting with ios5
// setter will be generated empty
@property (retain, nonatomic) UIWindow* window;

@property (readonly, copy, nonatomic) UnityView*            unityView;
@property (readonly, copy, nonatomic) CADisplayLink*        unityDisplayLink;

@property (readonly, copy, nonatomic) UIView*               rootView;
@property (readonly, copy, nonatomic) UIViewController*     rootViewController;
@property (readonly, copy, nonatomic) DisplayConnection*    mainDisplay;

#if UNITY_SUPPORT_ROTATION
@property (readonly, nonatomic) UIInterfaceOrientation      interfaceOrientation;
#endif

@property (nonatomic, retain) id                            renderDelegate;
@property (nonatomic, copy)                                 void(^quitHandler)();

@end

// accessing app controller
#ifdef __cplusplus
extern "C" {
#endif

extern UnityAppController* _UnityAppController;
extern UnityAppController* GetAppController();

#ifdef __cplusplus
} // extern "C"
#endif

// Put this into mm file with your subclass implementation
// pass subclass name to define

#define IMPL_APP_CONTROLLER_SUBCLASS(ClassName) \
@interface ClassName(OverrideAppDelegate)       \
{                                               \
}                                               \
+(void)load;                                    \
@end                                            \
@implementation ClassName(OverrideAppDelegate)  \
+(void)load                                     \
{                                               \
    extern const char* AppControllerClassName;  \
    AppControllerClassName = #ClassName;        \
}                                               \
@end                                            \


// plugins

#define APP_CONTROLLER_RENDER_PLUGIN_METHOD(method)                         \
do {                                                                        \
    id<RenderPluginDelegate> delegate = GetAppController().renderDelegate;  \
    if([delegate respondsToSelector:@selector(method)])                     \
        [delegate method];                                                  \
} while(0)

#define APP_CONTROLLER_RENDER_PLUGIN_METHOD_ARG(method, arg)                \
do {                                                                        \
    id<RenderPluginDelegate> delegate = GetAppController().renderDelegate;  \
    if([delegate respondsToSelector:@selector(method:)])                    \
        [delegate method:arg];                                              \
} while(0)


// these are simple wrappers about ios api, added for convenience
void AppController_SendNotification(NSString* name);
void AppController_SendNotificationWithArg(NSString* name, id arg);

void AppController_SendUnityViewControllerNotification(NSString* name);

// in the case when apple adds new api that has easy fallback path for old ios
// we will add new api methods at runtime on older ios, so we can switch to new api universally
// in that case we still need actual declaration: we will do it here as it is the most convenient place

// history:
// [CADisplayLink preferredFramesPerSecond], [UIScreen maximumFramesPerSecond], [UIView safeAreaInsets]
//   were removed after we started to enforce xcode9 (sdk 11)
