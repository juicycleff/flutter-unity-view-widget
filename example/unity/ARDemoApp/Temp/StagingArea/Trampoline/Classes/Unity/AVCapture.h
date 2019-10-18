#pragma once

enum
{
    avCapturePermissionUnknown  = 0,
    avCapturePermissionGranted  = 1,
    avCapturePermissionDenied   = 2,
};
enum
{
    avVideoCapture = 1,
    avAudioCapture = 2,
};

extern "C" int  UnityGetAVCapturePermission(int captureType);
extern "C" void UnityRequestAVCapturePermission(int captureType);
