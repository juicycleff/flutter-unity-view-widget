//==============================================================================
//
//  ReplayKit Unity Interface


#import "UnityReplayKit.h"

extern "C"
{
#if UNITY_REPLAY_KIT_AVAILABLE

    int UnityReplayKitAPIAvailable()
    {
        return [UnityReplayKit sharedInstance].apiAvailable ? 1 : 0;
    }

    int UnityReplayKitRecordingAvailable()
    {
        return [UnityReplayKit sharedInstance].recordingPreviewAvailable ? 1 : 0;
    }

    int UnityReplayKitIsCameraEnabled()
    {
        return [UnityReplayKit sharedInstance].cameraEnabled != NO ? 1 : 0;
    }

    int UnityReplayKitSetCameraEnabled(bool yes)
    {
        BOOL value = yes ? YES : NO;
        [UnityReplayKit sharedInstance].cameraEnabled = value;
        return [UnityReplayKit sharedInstance].cameraEnabled == value;
    }

    int UnityReplayKitIsMicrophoneEnabled()
    {
        return [UnityReplayKit sharedInstance].microphoneEnabled != NO ? 1 : 0;
    }

    int UnityReplayKitSetMicrophoneEnabled(bool yes)
    {
        if ([UnityReplayKit sharedInstance].isRecording)
        {
            printf_console("It is not possible to change microphoneEnabled during recording.\n");
            return 0;
        }

        BOOL value = yes ? YES : NO;
        [UnityReplayKit sharedInstance].microphoneEnabled = value;
        return [UnityReplayKit sharedInstance].microphoneEnabled == value;
    }

    const char* UnityReplayKitLastError()
    {
        NSString* err = [UnityReplayKit sharedInstance].lastError;
        if (err == nil)
        {
            return NULL;
        }
        const char* error = [err cStringUsingEncoding: NSUTF8StringEncoding];
        if (error != NULL)
        {
            error = strdup(error);
        }
        return error;
    }

    int UnityReplayKitStartRecording()
    {
        return [[UnityReplayKit sharedInstance] startRecording] ? 1 : 0;
    }

    int UnityReplayKitIsRecording()
    {
        return [UnityReplayKit sharedInstance].isRecording ? 1 : 0;
    }

    int UnityReplayKitShowCameraPreviewAt(float x, float y, float width, float height)
    {
        float q = 1.0f / UnityScreenScaleFactor([UIScreen mainScreen]);
        float h = [[UIScreen mainScreen] bounds].size.height;
        return [[UnityReplayKit sharedInstance] showCameraPreviewAt: CGPointMake(x * q, h - y * q) width: width height: height] ? 1 : 0;
    }

    void UnityReplayKitHideCameraPreview()
    {
        [[UnityReplayKit sharedInstance] hideCameraPreview];
    }

    int UnityReplayKitStopRecording()
    {
#if !PLATFORM_TVOS
        UnityReplayKitHideCameraPreview();
        UnityReplayKitSetCameraEnabled(false);
#endif
        return [[UnityReplayKit sharedInstance] stopRecording] ? 1 : 0;
    }

    int UnityReplayKitDiscard()
    {
        return [[UnityReplayKit sharedInstance] discardPreview] ? 1 : 0;
    }

    int UnityReplayKitPreview()
    {
        return [[UnityReplayKit sharedInstance] showPreview] ? 1 : 0;
    }

    int UnityReplayKitBroadcastingAPIAvailable()
    {
        return [[UnityReplayKit sharedInstance] broadcastingApiAvailable] ? 1 : 0;
    }

    void UnityReplayKitStartBroadcasting(void* callback)
    {
        [[UnityReplayKit sharedInstance] startBroadcastingWithCallback: callback];
    }

    void UnityReplayKitStopBroadcasting()
    {
#if !PLATFORM_TVOS
        UnityReplayKitHideCameraPreview();
#endif
        [[UnityReplayKit sharedInstance] stopBroadcasting];
    }

    void UnityReplayKitPauseBroadcasting()
    {
        [[UnityReplayKit sharedInstance] pauseBroadcasting];
    }

    void UnityReplayKitResumeBroadcasting()
    {
        [[UnityReplayKit sharedInstance] resumeBroadcasting];
    }

    int UnityReplayKitIsBroadcasting()
    {
        return [[UnityReplayKit sharedInstance] isBroadcasting] ? 1 : 0;
    }

    int UnityReplayKitIsBroadcastingPaused()
    {
        return [[UnityReplayKit sharedInstance] isBroadcastingPaused] ? 1 : 0;
    }

    int UnityReplayKitIsPreviewControllerActive()
    {
        return [[UnityReplayKit sharedInstance] isPreviewControllerActive] ? 1 : 0;
    }

    const char* UnityReplayKitGetBroadcastURL()
    {
        NSURL *url = [[UnityReplayKit sharedInstance] broadcastURL];
        if (url != nil)
        {
            return [[url absoluteString] UTF8String];
        }
        return nullptr;
    }

    void UnityReplayKitCreateOverlayWindow()
    {
        [[UnityReplayKit sharedInstance] createOverlayWindow];
    }

    extern "C" float UnityScreenScaleFactor(UIScreen* screen);

#else

// Impl when ReplayKit is not available.

    int UnityReplayKitAPIAvailable()        { return 0; }
    int UnityReplayKitRecordingAvailable()  { return 0; }
    const char* UnityReplayKitLastError()   { return NULL; }
    int UnityReplayKitStartRecording(int enableMicrophone, int enableCamera) { return 0; }
    int UnityReplayKitIsRecording()         { return 0; }
    int UnityReplayKitStopRecording()       { return 0; }
    int UnityReplayKitDiscard()             { return 0; }
    int UnityReplayKitPreview()             { return 0; }

    int UnityReplayKitIsCameraEnabled() { return 0; }
    int UnityReplayKitSetCameraEnabled(bool) { return 0; }
    int UnityReplayKitIsMicrophoneEnabled() { return 0; }
    int UnityReplayKitSetMicrophoneEnabled(bool) { return 0; }
    int UnityReplayKitShowCameraPreviewAt(float x, float y, float width, float height) { return 0; }
    void UnityReplayKitHideCameraPreview() {}
    void UnityReplayKitCreateOverlayWindow() {}

    void UnityReplayKitTriggerBroadcastStatusCallback(void*, bool, const char*);
    int UnityReplayKitBroadcastingAPIAvailable() { return 0; }
    void UnityReplayKitStartBroadcasting(void* callback) { UnityReplayKitTriggerBroadcastStatusCallback(callback, false, "ReplayKit not implemented."); }
    void UnityReplayKitStopBroadcasting() {}
    void UnityReplayKitPauseBroadcasting() {}
    void UnityReplayKitResumeBroadcasting() {}
    int UnityReplayKitIsBroadcasting() { return 0; }
    int UnityReplayKitIsBroadcastingPaused() { return 0; }
    int UnityReplayKitIsPreviewControllerActive() { return 0; }
    const char* UnityReplayKitGetBroadcastURL() { return nullptr; }

#endif  // UNITY_REPLAY_KIT_AVAILABLE
}  // extern "C"
