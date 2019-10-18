#include "UnityAppController+ViewHandling.h"
#include "UnityAppController+Rendering.h"

#include "UI/OrientationSupport.h"
#include "UI/UnityView.h"
#include "UI/UnityViewControllerBase.h"
#include "Unity/DisplayManager.h"


// TEMP: ?
#include "UI/ActivityIndicator.h"
#include "UI/SplashScreen.h"
#include "UI/Keyboard.h"
#include <utility>

extern bool _skipPresent;
extern bool _unityAppReady;


@implementation UnityAppController (ViewHandling)

#if UNITY_SUPPORT_ROTATION
// special case for when we DO know the app orientation, but dont get it through normal mechanism (UIViewController orientation handling)
// how can this happen:
// 1. On startup: ios is not sending "change orientation" notifications on startup (but rather we "start" in correct one already)
// 2. When using presentation controller it can override orientation constraints, so on dismissing we need to tweak app orientation;
//      pretty much like startup situation UIViewController would have correct orientation, and app will be out-of-sync
- (void)updateAppOrientation:(UIInterfaceOrientation)orientation
{
    _curOrientation = orientation;
    [_unityView willRotateToOrientation: orientation fromOrientation: (UIInterfaceOrientation)UIInterfaceOrientationUnknown];
    [_unityView didRotate];
}

#endif

- (UnityView*)createUnityView
{
    return [[UnityView alloc] initFromMainScreen];
}

- (UIViewController*)createUnityViewControllerDefault
{
    UnityDefaultViewController* ret = [[UnityDefaultViewController alloc] init];
#if PLATFORM_TVOS
    // This enables game controller use in on-screen keyboard
    ret.controllerUserInteractionEnabled = YES;
#endif
    return ret;
}

#if UNITY_SUPPORT_ROTATION
- (UIViewController*)createUnityViewControllerForOrientation:(UIInterfaceOrientation)orient
{
    switch (orient)
    {
        case UIInterfaceOrientationPortrait:            return [[UnityPortraitOnlyViewController alloc] init];
        case UIInterfaceOrientationPortraitUpsideDown:  return [[UnityPortraitUpsideDownOnlyViewController alloc] init];
        case UIInterfaceOrientationLandscapeLeft:       return [[UnityLandscapeLeftOnlyViewController alloc] init];
        case UIInterfaceOrientationLandscapeRight:      return [[UnityLandscapeRightOnlyViewController alloc] init];

        default:                                        NSAssert(false, @"bad UIInterfaceOrientation provided");
    }
    return nil;
}

#endif

- (UIViewController*)createRootViewController
{
    UIViewController* ret = nil;
    if (!UNITY_SUPPORT_ROTATION || UnityShouldAutorotate())
    {
        if (_viewControllerForOrientation[0] == nil)
            _viewControllerForOrientation[0] = [self createUnityViewControllerDefault];
        ret = _viewControllerForOrientation[0];
    }

#if UNITY_SUPPORT_ROTATION
    if (ret == nil)
    {
        UIInterfaceOrientation orientation = ConvertToIosScreenOrientation((ScreenOrientation)UnityRequestedScreenOrientation());
        ret = [self createRootViewControllerForOrientation: orientation];
    }
#endif
    return ret;
}

- (UIViewController*)topMostController
{
    UIViewController *topController = self.window.rootViewController;
    while (topController.presentedViewController)
        topController = topController.presentedViewController;
    return topController;
}

- (void)willStartWithViewController:(UIViewController*)controller
{
    _unityView.contentScaleFactor   = UnityScreenScaleFactor([UIScreen mainScreen]);
    _unityView.autoresizingMask     = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;

    _rootController.view = _rootView = _unityView;
}

- (void)willTransitionToViewController:(UIViewController*)toController fromViewController:(UIViewController*)fromController
{
}

- (void)didTransitionToViewController:(UIViewController*)toController fromViewController:(UIViewController*)fromController
{
#if UNITY_SUPPORT_ROTATION
    // when transitioning between view controllers ios will not send reorient events (because they are bound to controllers, not view)
    // so we imitate them here so unity view can update its size/orientation
    [_unityView willRotateToOrientation: UIViewControllerInterfaceOrientation(toController) fromOrientation: ConvertToIosScreenOrientation(_unityView.contentOrientation)];
    [_unityView didRotate];

    // NB: this is both important and insane at the same time (that we have several places to keep current orentation and we need to sync them)
    _curOrientation = UIViewControllerInterfaceOrientation(toController);
#endif
}

- (UIView*)createSnapshotView
{
    // Snapshot API appeared on iOS 7, however before iOS 8 tweaking hierarchy like that on going to
    // background results in all kind of weird things when going back to foreground so we do snapshotting
    // only on iOS 8 and newer.

    // Note that on iPads with iOS 9 or later (up to iOS 10.2 at least) there's a bug in the iOS
    // compositor: any use of -[UIView snapshotViewAfterScreenUpdates] causes black screen being shown
    // temporarily when 4 finger gesture to swipe to another app in the task switcher is being performed slowly
#if UNITY_SNAPSHOT_VIEW_ON_APPLICATION_PAUSE
    return [_rootView snapshotViewAfterScreenUpdates: YES];
#else
    return nil;
#endif
}

- (void)createUI
{
    NSAssert(_unityView != nil, @"_unityView should be inited at this point");
    NSAssert(_window != nil, @"_window should be inited at this point");

    _rootController = [self createRootViewController];

    [self willStartWithViewController: _rootController];

    NSAssert(_rootView != nil, @"_rootView  should be inited at this point");
    NSAssert(_rootController != nil, @"_rootController should be inited at this point");

    [_window makeKeyAndVisible];
    [UIView setAnimationsEnabled: NO];

    // TODO: extract it?

    ShowSplashScreen(_window);

#if UNITY_SUPPORT_ROTATION
    // to be able to query orientation from view controller we should actually show it.
    // at this point we can only show splash screen, so update app orientation after we started showing it
    // NB: _window.rootViewController = splash view controller (not _rootController)
    [self updateAppOrientation: ConvertToIosScreenOrientation(UIViewControllerOrientation(_window.rootViewController))];
#endif

    NSNumber* style = [[[NSBundle mainBundle] infoDictionary] objectForKey: @"Unity_LoadingActivityIndicatorStyle"];
    ShowActivityIndicator([SplashScreen Instance], style ? [style intValue] : -1);

    NSNumber* vcControlled = [[[NSBundle mainBundle] infoDictionary] objectForKey: @"UIViewControllerBasedStatusBarAppearance"];
    if (vcControlled && ![vcControlled boolValue])
        printf_console("\nSetting UIViewControllerBasedStatusBarAppearance to NO is no longer supported.\n"
            "Apple actively discourages that, and all application-wide methods of changing status bar appearance are deprecated\n\n"
        );
}

- (void)showGameUI
{
    HideActivityIndicator();
    HideSplashScreen();

    // make sure that we start up with correctly created/inited rendering surface
    // NB: recreateRenderingSurface won't go into rendering because _unityAppReady is false
#if UNITY_SUPPORT_ROTATION
    [self checkOrientationRequest];
#endif
    [_unityView recreateRenderingSurface];

    // UI hierarchy
    [_window addSubview: _rootView];
    _window.rootViewController = _rootController;
    [_window bringSubviewToFront: _rootView];

#if UNITY_SUPPORT_ROTATION
    // to be able to query orientation from view controller we should actually show it.
    // at this point we finally started to show game view controller. Just in case update orientation again
    [self updateAppOrientation: ConvertToIosScreenOrientation(UIViewControllerOrientation(_rootController))];
#endif

    // why we set level ready only now:
    // surface recreate will try to repaint if this var is set (poking unity to do it)
    // but this frame now is actually the first one we want to process/draw
    // so all the recreateSurface before now (triggered by reorientation) should simply change extents

    _unityAppReady = true;

    // why we skip present:
    // this will be the first frame to draw, so Start methods will be called
    // and we want to properly handle resolution request in Start (which might trigger surface recreate)
    // NB: we want to draw right after showing window, to avoid black frame creeping in

    _skipPresent = true;

    if (!UnityIsPaused())
        UnityRepaint();

    _skipPresent = false;
    [self repaint];

    [UIView setAnimationsEnabled: YES];
}

- (void)transitionToViewController:(UIViewController*)vc
{
    [self willTransitionToViewController: vc fromViewController: _rootController];

    // first: remove from view hierarchy.
    // if we simply hide the window before assigning the new view controller, it will cause black frame flickering
    // on the other hand, hiding the window is important by itself to better signal the intent to iOS
    //   e.g. unless we hide the window view, safeArea might stop working (due to bug in iOS if we're to speculate)
    // due to that we do this hide/unhide sequence: we want to to make it hidden, but still unhide it before changing window view controller.
    _window.hidden = YES;
    _window.hidden = NO;

    _rootController.view = nil;
    _window.rootViewController = nil;

    // second: assign new root controller (and view hierarchy with that), restore bounds
    _rootController = _window.rootViewController = vc;
    _rootController.view = _rootView;

    _window.bounds = [UIScreen mainScreen].bounds;
    // required for iOS 8, otherwise view bounds will be incorrect
    _rootView.bounds = _window.bounds;
    _rootView.center = _window.center;

    // third: restore window as key and layout subviews to finalize size changes
    [_window makeKeyAndVisible];
    [_window layoutSubviews];

    [self didTransitionToViewController: vc fromViewController: _rootController];
}

#if UNITY_SUPPORT_ROTATION
- (void)interfaceWillChangeOrientationTo:(UIInterfaceOrientation)toInterfaceOrientation
{
    UIInterfaceOrientation fromInterfaceOrientation = _curOrientation;

    _curOrientation = toInterfaceOrientation;
    [_unityView willRotateToOrientation: toInterfaceOrientation fromOrientation: fromInterfaceOrientation];
}

- (void)interfaceDidChangeOrientationFrom:(UIInterfaceOrientation)fromInterfaceOrientation
{
    [_unityView didRotate];
}

#endif

#define ARRAY_SIZE(x) (sizeof(x) / sizeof(x[0]))

- (void)executeForEveryViewController:(void(^)(UIViewController*))callback
{
    for (unsigned i = 0; i < ARRAY_SIZE(_viewControllerForOrientation); ++i)
    {
        UIViewController* vc = _viewControllerForOrientation[i];
        if (vc)
            callback(vc);
    }
}

- (void)notifyHideHomeButtonChange
{
    // Note that we need to update all view controllers because UIKit won't necessarily
    // update the properties of view controllers when orientation is changed.
#if PLATFORM_IOS
    if (@available(iOS 11.0, *))
    {
        [self executeForEveryViewController: ^(UIViewController* vc)
        {
            // setNeedsUpdateOfHomeIndicatorAutoHidden is not implemented on iOS 11.0.
            // The bug has been fixed in iOS 11.0.1. See http://www.openradar.me/35127134
            if ([vc respondsToSelector: @selector(setNeedsUpdateOfHomeIndicatorAutoHidden)])
                [vc setNeedsUpdateOfHomeIndicatorAutoHidden];
        }];
    }
#endif
}

- (void)notifyDeferSystemGesturesChange
{
#if PLATFORM_IOS
    if (@available(iOS 11.0, *))
    {
        [self executeForEveryViewController: ^(UIViewController* vc)
        {
            [vc setNeedsUpdateOfScreenEdgesDeferringSystemGestures];
        }];
    }
#endif
}

@end


#if UNITY_SUPPORT_ROTATION

@implementation UnityAppController (OrientationSupport)
- (UIViewController*)createRootViewControllerForOrientation:(UIInterfaceOrientation)orientation
{
    NSAssert(orientation != 0, @"Bad UIInterfaceOrientation provided");
    if (_viewControllerForOrientation[orientation] == nil)
        _viewControllerForOrientation[orientation] = [self createUnityViewControllerForOrientation: orientation];
    return _viewControllerForOrientation[orientation];
}

- (void)checkOrientationRequest
{
    if (!UnityHasOrientationRequest() && !UnityShouldChangeAllowedOrientations())
        return;

    // normally we want to call attemptRotationToDeviceOrientation to tell iOS that we changed orientation constraints
    // but if the current orientation is disabled we need special processing, as iOS will simply ignore us
    //   the only good/robust way is to simply recreate "autorotating" view controller and transition to it if needed

    // please note that we want to trigger "orientation request" code path if we recreate autorotating view controller
    bool changeOrient = UnityHasOrientationRequest();

    // first we check if we need to update orientations enabled for autorotation
    // this needs to be done *only* if we are to continue autorotating
    //   otherwise we will transition from this view controller
    //   and iOS will reread enabled orientations on next ViewController activation
    const bool autorot = UnityShouldAutorotate();
    if (UnityShouldChangeAllowedOrientations() && autorot)
    {
        NSUInteger rootOrient = 1 << UIViewControllerInterfaceOrientation(self.rootViewController);
        if (_rootController == _viewControllerForOrientation[0] && (rootOrient & EnabledAutorotationInterfaceOrientations()))
        {
            // if we are currently autorotating AND changed allowed orientations while keeping current interface orientation allowed:
            // we can simply trigger attemptRotationToDeviceOrientation and we are done
            // please note that this can happen when current *device* orientation is disabled (and we want to enable it)
            [UIViewController attemptRotationToDeviceOrientation];
        }
        else
        {
            // otherwise we recreate default autorotating view controller
            // please note that below we will check if root controller still equals _viewControllerForOrientation[0]
            // in that case (we update _viewControllerForOrientation[0]) the check will fail and will trigger transition (as expected)
            // you may look at this check as "are we autorotating with same constraints"
            _viewControllerForOrientation[0] = [self createUnityViewControllerDefault];
            changeOrient = true;
        }
    }

    if (changeOrient)
    {
        // on some devices like iPhone XS layoutSubview is not called when transitioning from different orientations with the same resolution
        // therefore forcing layoutSubview on all orientation changes
        [_unityView setNeedsLayout];

        if (autorot)
        {
            if (_viewControllerForOrientation[0] == nil)
                _viewControllerForOrientation[0] = [self createUnityViewControllerDefault];
            if (_rootController != _viewControllerForOrientation[0])
                [self transitionToViewController: _viewControllerForOrientation[0]];
            [UIViewController attemptRotationToDeviceOrientation];
        }
        else
        {
            UIInterfaceOrientation requestedOrient = ConvertToIosScreenOrientation((ScreenOrientation)UnityRequestedScreenOrientation());
            // on one hand orientInterface: should be perfectly fine "reorienting" to current orientation
            // in reality, ios might be confused by transitionToViewController: shenanigans coupled with "nothing have changed actually"
            // as an example: prior to ios12 that might result in status bar going "bad" (becoming transparent)
            if (_rootController != _viewControllerForOrientation[requestedOrient])
                [self orientInterface: requestedOrient];
        }
    }

    UnityOrientationRequestWasCommitted();
}

- (void)orientInterface:(UIInterfaceOrientation)orient
{
    if (_unityAppReady)
        UnityFinishRendering();

    [KeyboardDelegate StartReorientation];

    [CATransaction begin];
    {
        UIInterfaceOrientation oldOrient = _curOrientation;
        UIInterfaceOrientation newOrient = orient;

        [self interfaceWillChangeOrientationTo: newOrient];
        [self transitionToViewController: [self createRootViewControllerForOrientation: newOrient]];
        [self interfaceDidChangeOrientationFrom: oldOrient];

        [UIApplication sharedApplication].statusBarOrientation = orient;
    }
    [CATransaction commit];

    [KeyboardDelegate FinishReorientation];
}

- (void)orientUnity:(UIInterfaceOrientation)orient
{
    [self orientInterface: orient];
}

@end

#endif

extern "C" void UnityNotifyHideHomeButtonChange()
{
    [GetAppController() notifyHideHomeButtonChange];
}

extern "C" void UnityNotifyDeferSystemGesturesChange()
{
    [GetAppController() notifyDeferSystemGesturesChange];
}
