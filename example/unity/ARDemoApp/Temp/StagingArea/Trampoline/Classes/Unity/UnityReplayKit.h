#pragma once

#if UNITY_REPLAY_KIT_AVAILABLE

#import <Foundation/Foundation.h>
#import <ReplayKit/ReplayKit.h>

@interface UnityReplayKit : NSObject<RPPreviewViewControllerDelegate, RPScreenRecorderDelegate>
{
}

+ (instancetype)sharedInstance;

@property(nonatomic, readonly) BOOL apiAvailable;

@property(nonatomic, readonly) NSString* lastError;
@property(nonatomic, readonly) RPPreviewViewController* previewController;
@property(nonatomic, readonly) BOOL recordingPreviewAvailable;
@property(nonatomic, readonly, getter = isRecording) BOOL recording;

- (BOOL)startRecording;
- (BOOL)stopRecording;
- (BOOL)showPreview;
- (BOOL)discardPreview;

- (void)screenRecorder:(RPScreenRecorder*)screenRecorder didStopRecordingWithError:(NSError*)error previewViewController:(RPPreviewViewController*)previewViewController;
- (void)previewControllerDidFinish:(RPPreviewViewController*)previewController;

@property(nonatomic, readonly) BOOL broadcastingApiAvailable;
@property(nonatomic, readonly) BOOL isBroadcasting;
@property(nonatomic, readonly) BOOL isBroadcastingPaused;
@property(nonatomic, readonly) BOOL isPreviewControllerActive;
@property(nonatomic, readonly) NSURL* broadcastURL;
@property(nonatomic, setter = setCameraEnabled:, getter = isCameraEnabled) BOOL cameraEnabled;
@property(nonatomic, setter = setMicrophoneEnabled:, getter = isMicrophoneEnabled) BOOL microphoneEnabled;

- (void)startBroadcastingWithCallback:(void *)callback;
- (void)stopBroadcasting;
- (void)pauseBroadcasting;
- (void)resumeBroadcasting;
- (BOOL)showCameraPreviewAt:(CGPoint)position width:(float)width height:(float)height;
- (void)hideCameraPreview;
- (void)createOverlayWindow;
@end


#endif  // UNITY_REPLAY_KIT_AVAILABLE
