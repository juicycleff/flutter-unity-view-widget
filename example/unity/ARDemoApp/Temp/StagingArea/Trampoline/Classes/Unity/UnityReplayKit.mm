#if UNITY_REPLAY_KIT_AVAILABLE

#import "UnityReplayKit.h"
#import "UnityAppController.h"
#import "UI/UnityViewControllerBase.h"
#import "UnityInterface.h"
#import <UIKit/UIKit.h>

extern "C" void UnityReplayKitTriggerBroadcastStatusCallback(void* callback, bool hasSucceeded, const char* errorMessage);

static UnityReplayKit* _replayKit = nil;

@protocol UnityReplayKit_RPScreenRecorder<NSObject>

- (void)setMicrophoneEnabled:(BOOL)value;
- (BOOL)isMicrophoneEnabled;
- (void)setCameraEnabled:(BOOL)value;
- (BOOL)isCameraEnabled;
- (BOOL)isPreviewControllerActive;

@property (nonatomic, setter = setMicrophoneEnabled:, getter = isMicrophoneEnabled) BOOL microphoneEnabled;
@property (nonatomic, setter = setCameraEnabled:, getter = isCameraEnabled) BOOL cameraEnabled;
@property (nonatomic, readonly) UIView* cameraPreviewView;
@property (nonatomic, getter = isPreviewControllerActive) BOOL previewControllerActive;

@end

@protocol UnityReplayKit_RPBroadcastController<NSObject>

@property(nonatomic, readonly) NSURL *broadcastURL;
@property(nonatomic, readonly, getter = isBroadcasting) BOOL broadcasting;
@property(nonatomic, readonly) NSString *broadcastExtensionBundleID;
//@property(nonatomic, weak) id<RPBroadcastControllerDelegate> delegate;
@property(nonatomic, readonly, getter = isBroadcastingPaused) BOOL paused;
@property(nonatomic, readonly) NSDictionary<NSString *, NSObject<NSCoding> *> *serviceInfo;

- (BOOL)isBroadcasting;
- (BOOL)isBroadcastingPaused;
- (void)finishBroadcastWithHandler:(void (^)(NSError *error))handler;
- (void)startBroadcastWithHandler:(void (^)(NSError *error))handler;
- (void)pauseBroadcast;
- (void)resumeBroadcast;

@end

@interface UnityReplayKit_RPBroadcastActivityViewController : UIViewController<NSObject>

@property (nonatomic, weak) id delegate;

@end

// why do we care about orientation handling:
// ReplayKit will disable top-window autorotation
// as users keep asking to do autorotation during broadcast/record we create fake empty window with fake view controller
// this window will have autorotation disabled instead of unity one
// but this is not the end of the story: what fake view controller does is also important
// now it is hard to speculate what *actually* happens but with setup like fake view controller takes over control over "supported orientations"
// meaning that if we dont do anything suddenly all orientations become enabled.
// to avoid that we create this monstrosity that pokes unity for orientation.

#if PLATFORM_IOS
@interface UnityReplayKitViewController : UnityViewControllerBase
{
}
- (NSUInteger)supportedInterfaceOrientations;
@end
@implementation UnityReplayKitViewController
- (NSUInteger)supportedInterfaceOrientations
{
    NSUInteger ret = 0;
    if (UnityShouldAutorotate())
    {
        if (UnityIsOrientationEnabled(portrait))
            ret |= (1 << UIInterfaceOrientationPortrait);
        if (UnityIsOrientationEnabled(portraitUpsideDown))
            ret |= (1 << UIInterfaceOrientationPortraitUpsideDown);
        if (UnityIsOrientationEnabled(landscapeLeft))
            ret |= (1 << UIInterfaceOrientationLandscapeRight);
        if (UnityIsOrientationEnabled(landscapeRight))
            ret |= (1 << UIInterfaceOrientationLandscapeLeft);
    }
    else
    {
        switch (UnityRequestedScreenOrientation())
        {
            case portrait:              ret = (1 << UIInterfaceOrientationPortrait);            break;
            case portraitUpsideDown:    ret = (1 << UIInterfaceOrientationPortraitUpsideDown);  break;
            case landscapeLeft:         ret = (1 << UIInterfaceOrientationLandscapeRight);      break;
            case landscapeRight:        ret = (1 << UIInterfaceOrientationLandscapeLeft);       break;
        }
    }
    return ret;
}

@end
#else
    #define UnityReplayKitViewController UnityViewControllerBase
#endif

@implementation UnityReplayKit
{
    id<UnityReplayKit_RPBroadcastController> broadcastController;
    void* broadcastStartStatusCallback;
    UIView* currentCameraPreviewView;
    bool currentPreviewControllerActive;

    UIWindow* overlayWindow;
}

- (void)shouldCreateOverlayWindow
{
    UnityShouldCreateReplayKitOverlay();
}

- (void)createOverlayWindow
{
    if (self->overlayWindow == nil)
    {
        UIWindow* wnd = self->overlayWindow = [[UIWindow alloc] initWithFrame: [UIScreen mainScreen].bounds];
        wnd.hidden = wnd.userInteractionEnabled = NO;
        wnd.backgroundColor = nil;

        wnd.rootViewController = [[UnityReplayKitViewController alloc] init];
    }
}

+ (UnityReplayKit*)sharedInstance
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _replayKit = [[UnityReplayKit alloc] init];
    });
    return _replayKit;
}

- (BOOL)apiAvailable
{
    return ([RPScreenRecorder class] != nil) && [RPScreenRecorder sharedRecorder].isAvailable;
}

- (BOOL)recordingPreviewAvailable
{
    return _previewController != nil;
}

- (BOOL)startRecording
{
    RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
    if (recorder == nil)
    {
        _lastError = [NSString stringWithUTF8String: "Failed to get Screen Recorder"];
        return NO;
    }

    recorder.delegate = self;
    __block BOOL success = YES;
    [recorder startRecordingWithHandler:^(NSError* error) {
        if (error != nil)
        {
            _lastError = [error description];
            success = NO;
        }
        else
        {
            [self shouldCreateOverlayWindow];
        }
    }];

    return success;
}

- (BOOL)isRecording
{
    RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
    if (recorder == nil)
    {
        _lastError = [NSString stringWithUTF8String: "Failed to get Screen Recorder"];
        return NO;
    }
    return recorder.isRecording;
}

- (BOOL)stopRecording
{
    RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
    if (recorder == nil)
    {
        _lastError = [NSString stringWithUTF8String: "Failed to get Screen Recorder"];
        return NO;
    }

    __block BOOL success = YES;
    [recorder stopRecordingWithHandler:^(RPPreviewViewController* previewViewController, NSError* error) {
        self->overlayWindow = nil;
        if (error != nil)
        {
            _lastError = [error description];
            success = NO;
            return;
        }
        if (previewViewController != nil)
        {
            [previewViewController setPreviewControllerDelegate: self];
            _previewController = previewViewController;
        }
    }];

    return success;
}

- (void)screenRecorder:(RPScreenRecorder*)screenRecorder didStopRecordingWithError:(NSError*)error previewViewController:(RPPreviewViewController*)previewViewController
{
    if (error != nil)
    {
        _lastError = [error description];
    }
    self->overlayWindow = nil;
    _previewController = previewViewController;
}

- (BOOL)showPreview
{
    if (_previewController == nil)
    {
        _lastError = [NSString stringWithUTF8String: "No recording available"];
        return NO;
    }

    [_previewController setModalPresentationStyle: UIModalPresentationFullScreen];
    [GetAppController().rootViewController presentViewController: _previewController animated: YES completion:^()
    {
        _previewController = nil;
    }];

    currentPreviewControllerActive = YES;

    return YES;
}

- (BOOL)discardPreview
{
    if (_previewController == nil)
    {
        return YES;
    }

    RPScreenRecorder* recorder = [RPScreenRecorder sharedRecorder];
    if (recorder == nil)
    {
        _lastError = [NSString stringWithUTF8String: "Failed to get Screen Recorder"];
        return NO;
    }

    [recorder discardRecordingWithHandler:^()
    {
        _previewController = nil;
    }];
    // TODO - the above callback doesn't seem to be working at the moment.
    _previewController = nil;

    currentPreviewControllerActive = NO;

    return YES;
}

- (void)previewControllerDidFinish:(RPPreviewViewController*)previewController
{
    if (previewController != nil)
    {
        [previewController dismissViewControllerAnimated: YES completion: nil];
    }

    currentPreviewControllerActive = NO;
}

- (BOOL)isPreviewControllerActive
{
    return currentPreviewControllerActive;
}

/****************************************
 *   ReplayKit Broadcasting API         *
 ****************************************/

- (BOOL)broadcastingApiAvailable
{
    return nil != NSClassFromString(@"RPBroadcastController")
        && nil != NSClassFromString(@"RPBroadcastActivityViewController");
}

- (NSURL*)broadcastURL
{
    if (broadcastController == nil)
    {
        return nil;
    }
    return [broadcastController broadcastURL];
}

- (BOOL)isBroadcasting
{
    if (broadcastController == nil)
    {
        return NO;
    }
    return [broadcastController isBroadcasting];
}

- (BOOL)isBroadcastingPaused
{
    if (broadcastController == nil)
    {
        return NO;
    }
    return [broadcastController isBroadcastingPaused];
}

- (void)broadcastActivityViewController:(UnityReplayKit_RPBroadcastActivityViewController *)sBroadcastActivityViewController
    didFinishWithBroadcastController:(id<UnityReplayKit_RPBroadcastController>)sBroadcastController
    error:(NSError *)error
{
    dispatch_sync(dispatch_get_main_queue(), ^{
        UnityPause(0);
    });

    if (sBroadcastController == nil)
    {
        _lastError = [error description];
        UnityReplayKitTriggerBroadcastStatusCallback(broadcastStartStatusCallback, false, [_lastError UTF8String]);
        broadcastStartStatusCallback = nullptr;
        [UnityGetGLViewController() dismissViewControllerAnimated: YES completion: nil];
        return;
    }

    broadcastController = sBroadcastController;
    [UnityGetGLViewController() dismissViewControllerAnimated: YES completion:^
    {
        [broadcastController startBroadcastWithHandler:^(NSError* error)
        {
            if (error != nil)
            {
                _lastError = [error description];
                UnityReplayKitTriggerBroadcastStatusCallback(broadcastStartStatusCallback, false, [_lastError UTF8String]);
                broadcastStartStatusCallback = nullptr;
                broadcastController = nil;
                return;
            }
            UnityReplayKitTriggerBroadcastStatusCallback(broadcastStartStatusCallback, true, "");
            broadcastStartStatusCallback = nullptr;
            _lastError = nil;
        }];
    }];
}

- (void)startBroadcastingWithCallback:(void *)callback
{
    Class class_BroadcastActivityViewController = NSClassFromString(@"RPBroadcastActivityViewController");

    if (class_BroadcastActivityViewController == nil)
    {
        return;
    }

    if (broadcastController != nil && broadcastController.broadcasting)
    {
        _lastError = @"Broadcast already in progress";
        UnityReplayKitTriggerBroadcastStatusCallback(callback, false, [_lastError UTF8String]);
        return;
    }

    if (broadcastStartStatusCallback != nullptr)
    {
        _lastError = @"The last attempt to start a broadcast didn\'t finish yet.";
        UnityReplayKitTriggerBroadcastStatusCallback(callback, false, [_lastError UTF8String]);
        return;
    }

    [class_BroadcastActivityViewController performSelector: @selector(loadBroadcastActivityViewControllerWithHandler:) withObject:^(UnityReplayKit_RPBroadcastActivityViewController* vc, NSError* error)
    {
        if (vc == nil || error != nil)
        {
            _lastError = [error description];
            UnityReplayKitTriggerBroadcastStatusCallback(callback, false, [_lastError UTF8String]);
            return;
        }

        [self shouldCreateOverlayWindow];
        UnityPause(1);
        vc.delegate = self;
        broadcastStartStatusCallback = callback;

    #if PLATFORM_TVOS
        vc.modalPresentationStyle = UIModalPresentationFullScreen;
    #else
        vc.modalPresentationStyle = UIModalPresentationPopover;
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
        {
            vc.popoverPresentationController.sourceRect = CGRectMake(GetAppController().rootView.bounds.size.width / 2, 0, 0, 0);
            vc.popoverPresentationController.sourceView = GetAppController().rootView;
        }
    #endif

        [UnityGetGLViewController() presentViewController: vc animated: YES completion: nil];
    }];
    return;
}

- (void)stopBroadcasting
{
    self->overlayWindow = nil;

    if (broadcastController == nil || !broadcastController.broadcasting)
    {
        broadcastController = nil;
        return;
    }

    [broadcastController finishBroadcastWithHandler:^(NSError* error)
    {
        broadcastController = nil;
        if (error == nil)
            return;
        _lastError = [error description];
    }];
}

- (void)pauseBroadcasting
{
    if (broadcastController == nil || !broadcastController.broadcasting)
    {
        return;
    }

    [broadcastController pauseBroadcast];
}

- (void)resumeBroadcasting
{
    if (broadcastController == nil || !broadcastController.broadcasting)
    {
        return;
    }

    [broadcastController resumeBroadcast];
}

- (BOOL)isCameraEnabled
{
    if (![self apiAvailable])
    {
        return NO;
    }

    id<UnityReplayKit_RPScreenRecorder> screenRecorder = (id)[RPScreenRecorder sharedRecorder];
    if (![screenRecorder respondsToSelector: @selector(isCameraEnabled)])
    {
        return NO;
    }

    return screenRecorder.cameraEnabled;
}

- (void)setCameraEnabled:(BOOL)cameraEnabled
{
    if (![self apiAvailable])
    {
        return;
    }

    id<UnityReplayKit_RPScreenRecorder> screenRecorder = (id)[RPScreenRecorder sharedRecorder];
    if (![screenRecorder respondsToSelector: @selector(setCameraEnabled:)])
    {
        return;
    }

    screenRecorder.cameraEnabled = cameraEnabled;
}

- (BOOL)isMicrophoneEnabled
{
    if (![self apiAvailable])
    {
        return NO;
    }

    id<UnityReplayKit_RPScreenRecorder> screenRecorder = (id)[RPScreenRecorder sharedRecorder];
    if (![screenRecorder respondsToSelector: @selector(isMicrophoneEnabled)])
    {
        return NO;
    }

    return screenRecorder.microphoneEnabled;
}

- (void)setMicrophoneEnabled:(BOOL)microphoneEnabled
{
    if (![self apiAvailable])
    {
        return;
    }

    id<UnityReplayKit_RPScreenRecorder> screenRecorder = (id)[RPScreenRecorder sharedRecorder];
    if (![screenRecorder respondsToSelector: @selector(setMicrophoneEnabled:)])
    {
        return;
    }

    screenRecorder.microphoneEnabled = microphoneEnabled;
}

- (BOOL)showCameraPreviewAt:(CGPoint)position width:(float)width height:(float)height
{
    if (currentCameraPreviewView == nil)
    {
        if (![self apiAvailable])
        {
            return NO;
        }

        id<UnityReplayKit_RPScreenRecorder> screenRecorder = (id)[RPScreenRecorder sharedRecorder];
        UIView* cameraPreviewView = screenRecorder.cameraPreviewView;
        if (cameraPreviewView == nil)
        {
            return NO;
        }

        [[UnityGetGLViewController() view] addSubview: cameraPreviewView];
        currentCameraPreviewView = cameraPreviewView;
        [cameraPreviewView setUserInteractionEnabled: NO];
    }

    if (width < 0.0f)
        width = currentCameraPreviewView.frame.size.width;

    if (height < 0.0f)
        height = currentCameraPreviewView.frame.size.height;

    [currentCameraPreviewView setFrame: CGRectMake(position.x, position.y, width, height)];

    return YES;
}

- (void)hideCameraPreview
{
    if (currentCameraPreviewView != nil)
    {
        [currentCameraPreviewView removeFromSuperview];
        currentCameraPreviewView = nil;
    }
}

@end

#endif  // UNITY_REPLAY_KIT_AVAILABLE
