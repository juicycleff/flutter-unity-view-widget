#pragma once

#include "UnityAppController.h"
#include <AvailabilityMacros.h>


@interface UnityAppController (ViewHandling)

// tweaking view hierarchy and handling of orientation

// there are 3 main uses cases regarding UI handling:
//
// 1. normal game case: you shouldnt care about all this at all
//
// 2. you need some not-so-trivial overlayed views and/or minor UI tweaking
//    most likely all you need is to subscribe to "orientation changed" notification
//    or in case you have per-orientation UI logic override willTransitionToViewController
//
// 3. you create UI-rich app where unity view is just one of many
//    in that case you might want to create your own controllers and implement transitions on top
//    also instead of orientUnity: (and Screen.orientation in script) you should use orientInterface


// override this if you need customized unityview (subclassing)
// if you simply want different root view, tweak view hierarchy in createAutorotatingUnityViewController
- (UnityView*)createUnityView;

// for view controllers we discern between platforms that do support orientation (e.g. iOS) and the ones that dont (e.g. tvOS)
// both have concept of "default" view controller: for iOS it will be auto-rotating one (with possible constraints) and "simple" controller otherwise
// in case of supporting orientation we will discern case of fixed-orientation view controller (that seems to be the only way to handle it robustly)
// _unityView will be inited at the point of calling any of "create view controller" methods
// please note that these are actual "create" methods: there is no need to tweak hierarchy right away

- (UIViewController*)createUnityViewControllerDefault;
#if UNITY_SUPPORT_ROTATION
- (UIViewController*)createUnityViewControllerForOrientation:(UIInterfaceOrientation)orient;
#endif

#if UNITY_SUPPORT_ROTATION
// if you override these you need to call super
// if your root controller is not subclassed from UnityViewControllerBase, call these when rotation is happening
- (void)interfaceWillChangeOrientationTo:(UIInterfaceOrientation)toInterfaceOrientation;
- (void)interfaceDidChangeOrientationFrom:(UIInterfaceOrientation)fromInterfaceOrientation;
#endif

// handling of changing ViewControllers:
// willStartWithViewController: will be called on startup, when creating view hierarchy
// willTransitionToViewController:fromViewController: didTransitionToViewController:fromViewController:
// are called before/after we are doing some magic to switch to new root controller due to forced orientation change

// by default:
// willStartWithViewController: will make _unityView as root view
// willTransitionToViewController:fromViewController: will do nothing
// didTransitionToViewController:fromViewController: will send orientation events to unity view
// you can use them to tweak view hierarchy if needed

- (void)willStartWithViewController:(UIViewController*)controller;
- (void)willTransitionToViewController:(UIViewController*)toController fromViewController:(UIViewController*)fromController;
- (void)didTransitionToViewController:(UIViewController*)toController fromViewController:(UIViewController*)fromController;


// override this if you want to have custom snapshot view.
// by default it will capture the frame drawn inside applicationWillResignActive specifically to let app respond to OnApplicationPause
// will be called on every applicationWillResignActive; returned view will be released in applicationDidBecomeActive
// NB: case of returning nil will be handled gracefully
- (UIView*)createSnapshotView;

// you should not override these methods

// creates initial UI hierarchy (e.g. splash screen) and calls willStartWithViewController
- (void)createUI;
// shows game itself (hides splash, and bring _rootView to front)
- (void)showGameUI;

// returns the topmost presentedViewController if there is one, or just rootViewController
- (UIViewController*)topMostController;

// will create or return from cache correct view controller for requested orientation
- (UIViewController*)createRootViewController;

// old deprecated methods: no longer used
// the caveat is: there are some issues in clang related to method deprecation
// which results in warnings not being generated for overriding deprecated methods (in some circumstances).
// so instead of deprecating these methods we just remove them and will check at runtime if user have them and whine about it

//- (UnityView*)createUnityViewImpl DEPRECATED_MSG_ATTRIBUTE("Will not be called. Override createUnityView");
//- (void)createViewHierarchyImpl DEPRECATED_MSG_ATTRIBUTE("Will not be called. Override willStartWithViewController");
//- (void)createViewHierarchy DEPRECATED_MSG_ATTRIBUTE("Is not implemented. Use createUI");

@end

#if UNITY_SUPPORT_ROTATION
@interface UnityAppController (OrientationSupport)
// will create or return from cache correct view controller for given orientation
- (UIViewController*)createRootViewControllerForOrientation:(UIInterfaceOrientation)orientation;

// forcibly orient interface
- (void)orientInterface:(UIInterfaceOrientation)orient;

// check unity requested orientation and applies it
- (void)checkOrientationRequest;

- (void)orientUnity:(UIInterfaceOrientation)orient __deprecated_msg("use orientInterface instead.");
@end
#endif
