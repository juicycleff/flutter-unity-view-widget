// DO NOT PUT #pragma once or include guard check here
// This header is designed to be able to be included multiple times

// This header is used to temporary undefine all platform definitions in case there is a naming conflict with
// 3rd party code. Please make sure to always use this paired with the RedefinePlatforms.h header.
//
// ex.
//
// #include "UndefinePlatforms.h"
// #include "Some3rdParty.h"
// #include "RedefinePlatforms.h"

#ifdef DETAIL__PLATFORMS_HAD_BEEN_UNDEFINED_BY_UNDEFINEPLATFORMS_H
#error "UndefinePlatforms.h has been included more than once or RedefinePlatforms.h is missing."
#endif

// define all other platforms to 0
#if PLATFORM_WIN
    #define DETAIL__TEMP_PLATFORM_WIN_WAS_1
#endif
#undef PLATFORM_WIN

#if PLATFORM_OSX
    #define DETAIL__TEMP_PLATFORM_OSX_WAS_1
#endif
#undef PLATFORM_OSX

#if PLATFORM_LINUX
    #define DETAIL__TEMP_PLATFORM_LINUX_WAS_1
#endif
#undef PLATFORM_LINUX

#if PLATFORM_WINRT
    #define DETAIL__TEMP_PLATFORM_WINRT_WAS_1
#endif
#undef PLATFORM_WINRT

#if PLATFORM_WEBGL
    #define DETAIL__TEMP_PLATFORM_WEBGL_WAS_1
#endif
#undef PLATFORM_WEBGL

#if PLATFORM_ANDROID
    #define DETAIL__TEMP_PLATFORM_ANDROID_WAS_1
#endif
#undef PLATFORM_ANDROID

#if PLATFORM_PS4
    #define DETAIL__TEMP_PLATFORM_PS4_WAS_1
#endif
#undef PLATFORM_PS4

#if PLATFORM_IPHONE
    #define DETAIL__TEMP_PLATFORM_IPHONE_WAS_1
#endif
#undef PLATFORM_IPHONE

#if PLATFORM_IOS
    #define DETAIL__TEMP_PLATFORM_IOS_WAS_1
#endif
#undef PLATFORM_IOS

#if PLATFORM_TVOS
    #define DETAIL__TEMP_PLATFORM_TVOS_WAS_1
#endif
#undef PLATFORM_TVOS

#if PLATFORM_XBOXONE
    #define DETAIL__TEMP_PLATFORM_XBOXONE_WAS_1
#endif
#undef PLATFORM_XBOXONE

#if PLATFORM_SWITCH
    #define DETAIL__TEMP_PLATFORM_SWITCH_WAS_1
#endif
#undef PLATFORM_SWITCH

#if PLATFORM_LUMIN
    #define DETAIL__TEMP_PLATFORM_LUMIN_WAS_1
#endif
#undef PLATFORM_LUMIN

#if PLATFORM_GGP
    #define DETAIL__TEMP_PLATFORM_GGP_WAS_1
#endif
#undef PLATFORM_GGP

#if PLATFORM_NETBSD
    #define DETAIL__TEMP_PLATFORM_NETBSD_WAS_1
#endif
#undef PLATFORM_NETBSD

#define DETAIL__PLATFORMS_HAD_BEEN_UNDEFINED_BY_UNDEFINEPLATFORMS_H
