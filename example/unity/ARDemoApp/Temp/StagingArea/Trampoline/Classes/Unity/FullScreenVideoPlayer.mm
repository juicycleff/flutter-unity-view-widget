#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>
#import <AVKit/AVKit.h>
#import <UIKit/UIGestureRecognizerSubclass.h>

#include "UnityAppController.h"
#include "UI/UnityView.h"
#include "UI/UnityViewControllerBase.h"
#include "UI/OrientationSupport.h"
#include "UI/UnityAppController+ViewHandling.h"
#include "Unity/ObjCRuntime.h"
#include "Unity/VideoPlayer.h"
#include "PluginBase/UnityViewControllerListener.h"

@interface UICancelGestureRecognizer : UITapGestureRecognizer
@end

@interface AVKitVideoPlayback : NSObject<VideoPlayerDelegate, UIViewControllerTransitioningDelegate, UIGestureRecognizerDelegate>
{
    AVPlayerViewController*     videoViewController;
    VideoPlayer*                videoPlayer;

    UIColor*                    bgColor;
    const NSString*             videoGravity;
    BOOL                        showControls;
    BOOL                        cancelOnTouch;
}

- (void)onPlayerReady;
- (void)onPlayerDidFinishPlayingVideo;

- (id)initAndPlay:(NSURL*)url bgColor:(UIColor*)color showControls:(BOOL)controls videoGravity:(const NSString*)scaling cancelOnTouch:(BOOL)cot;
- (void)actuallyStartTheMovie:(NSURL*)url;
- (void)finish;
@end

static AVKitVideoPlayback*  _AVKitVideoPlayback = nil;

@implementation AVKitVideoPlayback

#if PLATFORM_IOS
static void AVPlayerViewController_SetAllowsPictureInPicturePlayback_OldIOSImpl(id self_, SEL _cmd, BOOL allow) {}
static NSUInteger supportedInterfaceOrientations_DefaultImpl(id self_, SEL _cmd)
{
    return GetAppController().rootViewController.supportedInterfaceOrientations;
}

static bool prefersStatusBarHidden_DefaultImpl(id self_, SEL _cmd)
{
    if (_AVKitVideoPlayback) // video is still playing
        return _AVKitVideoPlayback->videoViewController.showsPlaybackControls ? NO : YES;
    else                    // video has beed stopped
        return GetAppController().rootViewController.prefersStatusBarHidden;
}

#endif

+ (void)initialize
{
    if (self == [AVKitVideoPlayback class])
    {
#if PLATFORM_IOS
        class_replaceMethod([AVPlayerViewController class], @selector(supportedInterfaceOrientations), (IMP)&supportedInterfaceOrientations_DefaultImpl, UIViewController_supportedInterfaceOrientations_Enc);
        class_replaceMethod([AVPlayerViewController class], @selector(prefersStatusBarHidden), (IMP)&prefersStatusBarHidden_DefaultImpl, UIViewController_prefersStatusBarHidden_Enc);
#endif
    }
}

- (id)initAndPlay:(NSURL*)url bgColor:(UIColor*)color showControls:(BOOL)controls videoGravity:(const NSString*)scaling cancelOnTouch:(BOOL)cot
{
    if ((self = [super init]))
    {
        UnityPause(1);

        showControls    = controls;
        videoGravity    = scaling;
        bgColor         = color;
        cancelOnTouch   = cot;

        [self performSelector: @selector(actuallyStartTheMovie:) withObject: url afterDelay: 0];
    }
    return self;
}

- (void)dealloc
{
    [self finish];
}

- (void)actuallyStartTheMovie:(NSURL*)url
{
    @autoreleasepool
    {
        videoViewController = [[AVPlayerViewController alloc] init];

        videoViewController.showsPlaybackControls = showControls;
        videoViewController.view.backgroundColor = bgColor;
        videoViewController.videoGravity = (NSString*)videoGravity;
        videoViewController.transitioningDelegate = self;

#if PLATFORM_IOS
        videoViewController.allowsPictureInPicturePlayback = NO;
#endif
#if PLATFORM_TVOS
        // In tvOS clicking Menu button while video is playing will exit the app. So when
        // app disables exiting to menu behavior, we need to catch the click and ignore it.
        if (!UnityGetAppleTVRemoteAllowExitToMenu())
        {
            UITapGestureRecognizer *tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget: self action: @selector(handleTap:)];
            tapRecognizer.allowedPressTypes = @[@(UIPressTypeMenu)];
            [videoViewController.view addGestureRecognizer: tapRecognizer];
        }
#endif

        if (cancelOnTouch)
        {
            UICancelGestureRecognizer *cancelTouch = [[UICancelGestureRecognizer alloc] initWithTarget: self action: @selector(handleTap:)];
            cancelTouch.delegate = self;
            [videoViewController.view addGestureRecognizer: cancelTouch];
        }

        videoPlayer = [[VideoPlayer alloc] init];
        videoPlayer.delegate = self;
        [videoPlayer loadVideo: url];
    }
}

- (void)handleTap:(UITapGestureRecognizer*)sender
{
    if (cancelOnTouch && (sender.state == UIGestureRecognizerStateEnded))
        [self finish];
}

- (void)onPlayerReady
{
    videoViewController.player = videoPlayer.player;

    CGSize screenSize = GetAppController().rootView.bounds.size;
    BOOL ret = [VideoPlayer CheckScalingModeAspectFill: videoPlayer.videoSize screenSize: screenSize];
    if (ret == YES && [videoViewController.videoGravity isEqualToString: AVLayerVideoGravityResizeAspect] == YES)
    {
        videoViewController.videoGravity = AVLayerVideoGravityResizeAspectFill;
    }

    [videoPlayer playVideoPlayer];
#if PLATFORM_TVOS
    GetAppController().window.rootViewController = videoViewController;
#else
    UIViewController *viewController = [GetAppController() topMostController];
    if ([viewController isEqual: videoViewController] == NO && [videoViewController isBeingPresented] == NO)
        [viewController presentViewController: videoViewController animated: NO completion: nil];
#endif
}

- (void)onPlayerDidFinishPlayingVideo
{
    [self finish];
}

- (void)onPlayerTryResume
{
    if (![videoPlayer isPlaying])
        [videoPlayer resume];
}

- (void)onPlayerError:(NSError*)error
{
    [self finish];
}

- (id<UIViewControllerAnimatedTransitioning>)animationControllerForDismissedController:(UIViewController *)dismissed
{
    if ([dismissed isEqual: videoViewController] == YES)
    {
        [self finish];
    }

    return nil;
}

- (void)finish
{
    @synchronized(self)
    {
#if PLATFORM_TVOS
        GetAppController().window.rootViewController = GetAppController().rootViewController;
#else
        UIViewController *viewController = [GetAppController() topMostController];
        if ([viewController isEqual: videoViewController] == YES && [viewController isBeingDismissed] == NO)
            [viewController dismissViewControllerAnimated: NO completion: nil];
#endif

        [videoPlayer unloadPlayer];

        videoPlayer = nil;
        videoViewController = nil;

        _AVKitVideoPlayback = nil;

#if PLATFORM_TVOS
        UnityCancelTouches();
#endif

        if (UnityIsPaused())
            UnityPause(0);
    }
}

@end

@implementation UICancelGestureRecognizer
//instead of having lots of UITapGestureRecognizers with different finger numbers
- (void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event
{
    [self setState: UIGestureRecognizerStateRecognized];
}

@end

extern "C" void UnityPlayFullScreenVideo(const char* path, const float* color, unsigned controls, unsigned scaling)
{
    const BOOL  cancelOnTouch[] = { NO, NO, YES, NO };
    UIColor*    bgColor         = [UIColor colorWithRed: color[0] green: color[1] blue: color[2] alpha: color[3]];

    const bool isURL = ::strstr(path, "://") != 0;
    NSURL* url = nil;
    if (isURL)
    {
        url = [NSURL URLWithString: [NSString stringWithUTF8String: path]];
    }
    else
    {
        NSString* relPath   = path[0] == '/' ? [NSString stringWithUTF8String: path] : [NSString stringWithFormat: @"Data/Raw/%s", path];
        NSString* fullPath  = [[NSString stringWithUTF8String: UnityDataBundleDir()] stringByAppendingPathComponent: relPath];
        url = [NSURL fileURLWithPath: fullPath];
    }

    const BOOL      showControls[]  =   { YES, YES, NO, NO };
    const NSString* videoGravity[]  =
    {
        AVLayerVideoGravityResizeAspectFill,    // ???
        AVLayerVideoGravityResizeAspect,
        AVLayerVideoGravityResizeAspectFill,
        AVLayerVideoGravityResize,
    };

    if (_AVKitVideoPlayback)
        [_AVKitVideoPlayback finish];
    _AVKitVideoPlayback = [[AVKitVideoPlayback alloc] initAndPlay: url bgColor: bgColor
                           showControls: showControls[controls] videoGravity: videoGravity[scaling] cancelOnTouch: cancelOnTouch[controls]];
}

extern "C" void UnityStopFullScreenVideoIfPlaying()
{
    if (_AVKitVideoPlayback)
        [_AVKitVideoPlayback finish];
}

extern "C" int UnityIsFullScreenPlaying()
{
    return _AVKitVideoPlayback ? 1 : 0;
}

extern "C" void TryResumeFullScreenVideo()
{
    if (_AVKitVideoPlayback)
        [_AVKitVideoPlayback onPlayerTryResume];
}
