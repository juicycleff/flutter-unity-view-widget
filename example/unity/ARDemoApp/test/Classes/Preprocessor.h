#pragma once

#include <Availability.h>
#include <TargetConditionals.h>

//------------------------------------------------------------------------------
//
// ensuring proper compiler/xcode/whatever selection
//

#ifndef __clang__
#error Please use clang compiler.
#endif

// NOT the best way but apple do not care about adding extensions properly
#if __clang_major__ < 9
#error Please use Xcode 9.0 or newer
#endif

#if !defined(__IPHONE_11_0) || __IPHONE_OS_VERSION_MAX_ALLOWED < __IPHONE_11_0
#error Please use iOS SDK 11.0 or newer
#endif

#if defined(TARGET_OS_TV) && TARGET_OS_TV && !defined(__TVOS_11_0)
#error Please use tvOS SDK 11.0 or newer
#endif

#if TARGET_OS_IOS && (!defined(__IPHONE_10_0) || __IPHONE_OS_VERSION_MIN_REQUIRED < __IPHONE_10_0)
#error Please target iOS 10.0 or newer
#endif

#if TARGET_OS_TV && (!defined(__TVOS_10_0) || __TV_OS_VERSION_MIN_REQUIRED < __TVOS_10_0)
#error Please target tvOS 10.0 or newer
#endif

//------------------------------------------------------------------------------
//
// defines for target platform
//

#define UNITY_TRAMPOLINE_IN_USE 1

#if defined(TARGET_OS_TV) && TARGET_OS_TV
#define PLATFORM_TVOS 0
#define PLATFORM_IOS 1
#else
#define PLATFORM_TVOS 0
#define PLATFORM_IOS 1
#endif

#define PLATFORM_OSX 0


//------------------------------------------------------------------------------
//
// defines for sdk/target version
//

#if !TARGET_IPHONE_SIMULATOR && !TARGET_TVOS_SIMULATOR
    #define UNITY_CAN_USE_METAL     1
#elif TARGET_IPHONE_SIMULATOR && defined(__IPHONE_13_0)
    #define UNITY_CAN_USE_METAL     1
#elif TARGET_TVOS_SIMULATOR && defined(__TVOS_13_0)
    #define UNITY_CAN_USE_METAL     1
#else
    #define UNITY_CAN_USE_METAL     0
#endif

// It's hard to figure out which SDK we are using as the availability macros defined in the SDK
// have various quirks.
//
// It's not possible to use *_VERSION_MAX_ALLOWED macros because they not always corresponded to
// the SDK version. In particular, __TV_OS_VERSION_MAX_ALLOWED was out of sync in all Xcode dot
// releases except the first so far.
//
// The highest __IPHONE_X_Y or __TVOS_X_Y macro that is defined in Availability.h correctly
// corresponds to the version of the SDK (at least in each Xcode version since 6.0 up to 9.0).
// However, some other headers (e.g. System/Library/Frameworks/QuartzCore.framework/Headers/CABase.h
// in SDKs up to 9.3) may define the macros itself and this does not correspond to the what's in
// Availability.h. Thus we make sure to include "Preprocessor.h" before the CABase.h header.
#if defined(CABASE_H)
#error "Please include Preprocessor.h before other includes"
#endif

#if defined(__IPHONE_10_0)
    #define UNITY_HAS_IOSSDK_10_0  1
#else
    #define UNITY_HAS_IOSSDK_10_0  0
#endif
#if defined(__IPHONE_10_2)
    #define UNITY_HAS_IOSSDK_10_2  1
#else
    #define UNITY_HAS_IOSSDK_10_2  0
#endif
#if defined(__IPHONE_10_3)
    #define UNITY_HAS_IOSSDK_10_3  1
#else
    #define UNITY_HAS_IOSSDK_10_3  0
#endif
#if defined(__IPHONE_11_0)
    #define UNITY_HAS_IOSSDK_11_0  1
#else
    #define UNITY_HAS_IOSSDK_11_0  0
#endif
#if defined(__IPHONE_11_1)
    #define UNITY_HAS_IOSSDK_11_1  1
#else
    #define UNITY_HAS_IOSSDK_11_1  0
#endif
#if defined(__TVOS_10_0)
    #define UNITY_HAS_TVOSSDK_10_0 1
#else
    #define UNITY_HAS_TVOSSDK_10_0 0
#endif
#if defined(__TVOS_10_2)
    #define UNITY_HAS_TVOSSDK_10_2 1
#else
    #define UNITY_HAS_TVOSSDK_10_2 0
#endif
#if defined(__TVOS_11_0)
    #define UNITY_HAS_TVOSSDK_11_0 1
#else
    #define UNITY_HAS_TVOSSDK_11_0 0
#endif

// The following UNITY_USES_* flags disable functionality in the trampoline project
// whenever the user does not use it from his scripts. We detect the API usage and
// adjust the value of these flags whenever the project is built (including when the
// project is appended)

#define UNITY_USES_REMOTE_NOTIFICATIONS 0
#define UNITY_USES_WEBCAM 0
#define UNITY_USES_MICROPHONE 0
#define UNITY_USES_REPLAY_KIT 0
#define UNITY_SNAPSHOT_VIEW_ON_APPLICATION_PAUSE 0
#define UNITY_DEVELOPER_BUILD 0
#define UNITY_USES_DYNAMIC_PLAYER_LIB 0
#define UNITY_USES_LOCATION 0

#define USE_IL2CPP_PCH 0
#define UNITY_SUPPORT_ROTATION PLATFORM_IOS
#if PLATFORM_TVOS
    #define UNITY_TVOS_ORIENTATION landscapeLeft
#endif

#if PLATFORM_IOS    // available in ios9 sdk which is min requirement
    #define UNITY_REPLAY_KIT_AVAILABLE UNITY_USES_REPLAY_KIT
#elif PLATFORM_TVOS // available in tvos10 sdk which is min requirement
    #define UNITY_REPLAY_KIT_AVAILABLE UNITY_USES_REPLAY_KIT && defined(__TVOS_10_0)
#else
    #define UNITY_REPLAY_KIT_AVAILABLE 0
#endif

// On tvOS simulator we implement a fake remote as tvOS simulator does not support controllers (yet)
#define UNITY_TVOS_SIMULATOR_FAKE_REMOTE (PLATFORM_TVOS && TARGET_TVOS_SIMULATOR)
