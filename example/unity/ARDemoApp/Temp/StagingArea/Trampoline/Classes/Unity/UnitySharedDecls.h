#pragma once

// The contents of this file are used when building both Unity library and the trampoline. Do not change it.

// Classes/Unity/UnityForwardDecls
typedef enum ScreenOrientation
{
    orientationUnknown,

    portrait,
    portraitUpsideDown,
    landscapeLeft,
    landscapeRight,

    orientationCount,
}
ScreenOrientation;

// be aware that this enum is shared with unity implementation so you should absolutely not change it
typedef enum DeviceGeneration
{
    deviceUnknown       = 0,
    deviceiPhone3GS     = 3,
    deviceiPhone4       = 8,
    deviceiPodTouch4Gen = 9,
    deviceiPad2Gen      = 10,
    deviceiPhone4S      = 11,
    deviceiPad3Gen      = 12,
    deviceiPhone5       = 13,
    deviceiPodTouch5Gen = 14,
    deviceiPadMini1Gen  = 15,
    deviceiPad4Gen      = 16,
    deviceiPhone5C      = 17,
    deviceiPhone5S      = 18,
    deviceiPadAir1      = 19,
    deviceiPadMini2Gen  = 20,
    deviceiPhone6       = 21,
    deviceiPhone6Plus   = 22,
    deviceiPadMini3Gen  = 23,
    deviceiPadAir2      = 24,
    deviceiPhone6S      = 25,
    deviceiPhone6SPlus  = 26,
    deviceiPadPro1Gen   = 27,
    deviceiPadMini4Gen  = 28,
    deviceiPhoneSE1Gen  = 29,
    deviceiPadPro10Inch1Gen = 30,
    deviceiPhone7       = 31,
    deviceiPhone7Plus   = 32,
    deviceiPodTouch6Gen = 33,
    deviceiPad5Gen      = 34,
    deviceiPadPro2Gen = 35,
    deviceiPadPro10Inch2Gen = 36,
    deviceiPhone8       = 37,
    deviceiPhone8Plus   = 38,
    deviceiPhoneX       = 39,
    deviceiPhoneXS      = 40,
    deviceiPhoneXSMax   = 41,
    deviceiPhoneXR      = 42,
    deviceiPadPro11Inch = 43,
    deviceiPadPro3Gen   = 44,
    deviceiPad6Gen      = 45,

    deviceiPhoneUnknown     = 10001,
    deviceiPadUnknown       = 10002,
    deviceiPodTouchUnknown  = 10003,

    deviceAppleTV1Gen  = 1001,
    deviceAppleTV2Gen  = 1002
}
DeviceGeneration;

// Classes/UI/SplashScreen.mm
#ifdef __cplusplus
struct OrientationMask
{
    bool portrait;
    bool portraitUpsideDown;
    bool landscapeLeft;
    bool landscapeRight;
};
#endif
