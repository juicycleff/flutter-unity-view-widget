// DO NOT PUT #pragma once or include guard check here
// This header is designed to be able to be included multiple times

// This header is used to redefine platforms after they were temporary undefined by UndefinePlatforms.h
// Please make sure to always use this paired with the UndefinePlatforms.h header.
//
// ex.
//
// #include "UndefinePlatforms.h"
// #include "Some3rdParty.h"
// #include "RedefinePlatforms.h"

#ifndef DETAIL__PLATFORMS_HAD_BEEN_UNDEFINED_BY_UNDEFINEPLATFORMS_H
    #error "DefinePlatforms.h can only be used after UndefinePlatforms.h got included before."
#endif

#undef DETAIL__PLATFORMS_HAD_BEEN_UNDEFINED_BY_UNDEFINEPLATFORMS_H

// define all other platforms to 0
#undef PLATFORM_WIN
#if defined(DETAIL__TEMP_PLATFORM_WIN_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_WIN_WAS_1
    #define PLATFORM_WIN 1
#else
    #define PLATFORM_WIN 0
#endif

#undef PLATFORM_OSX
#if defined(DETAIL__TEMP_PLATFORM_OSX_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_OSX_WAS_1
    #define PLATFORM_OSX 1
#else
    #define PLATFORM_OSX 0
#endif

#undef PLATFORM_LINUX
#if defined(DETAIL__TEMP_PLATFORM_LINUX_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_LINUX_WAS_1
    #define PLATFORM_LINUX 1
#else
    #define PLATFORM_LINUX 0
#endif

#undef PLATFORM_WINRT
#if defined(DETAIL__TEMP_PLATFORM_WINRT_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_WINRT_WAS_1
    #define PLATFORM_WINRT 1
#else
    #define PLATFORM_WINRT 0
#endif

#undef PLATFORM_WEBGL
#if defined(DETAIL__TEMP_PLATFORM_WEBGL_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_WEBGL_WAS_1
    #define PLATFORM_WEBGL 1
#else
    #define PLATFORM_WEBGL 0
#endif

#undef PLATFORM_ANDROID
#if defined(DETAIL__TEMP_PLATFORM_ANDROID_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_ANDROID_WAS_1
    #define PLATFORM_ANDROID 1
#else
    #define PLATFORM_ANDROID 0
#endif

#undef PLATFORM_PS4
#if defined(DETAIL__TEMP_PLATFORM_PS4_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_PS4_WAS_1
    #define PLATFORM_PS4 1
#else
    #define PLATFORM_PS4 0
#endif

#undef PLATFORM_IPHONE
#if defined(DETAIL__TEMP_PLATFORM_IPHONE_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_IPHONE_WAS_1
    #define PLATFORM_IPHONE 1
#else
    #define PLATFORM_IPHONE 0
#endif

#undef PLATFORM_IOS
#if defined(DETAIL__TEMP_PLATFORM_IOS_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_IOS_WAS_1
    #define PLATFORM_IOS 1
#else
    #define PLATFORM_IOS 0
#endif

#undef PLATFORM_TVOS
#if defined(DETAIL__TEMP_PLATFORM_TVOS_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_TVOS_WAS_1
    #define PLATFORM_TVOS 1
#else
    #define PLATFORM_TVOS 0
#endif

#undef PLATFORM_XBOXONE
#if defined(DETAIL__TEMP_PLATFORM_XBOXONE_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_XBOXONE_WAS_1
    #define PLATFORM_XBOXONE 1
#else
    #define PLATFORM_XBOXONE 0
#endif

#undef PLATFORM_SWITCH
#if defined(DETAIL__TEMP_PLATFORM_SWITCH_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_SWITCH_WAS_1
    #define PLATFORM_SWITCH 1
#else
    #define PLATFORM_SWITCH 0
#endif

#undef PLATFORM_LUMIN
#if defined(DETAIL__TEMP_PLATFORM_LUMIN_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_LUMIN_WAS_1
    #define PLATFORM_LUMIN 1
#else
    #define PLATFORM_LUMIN 0
#endif

#undef PLATFORM_GGP
#if defined(DETAIL__TEMP_PLATFORM_GGP_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_GGP_WAS_1
    #define PLATFORM_GGP 1
#else
    #define PLATFORM_GGP 0
#endif

#undef PLATFORM_NETBSD
#if defined(DETAIL__TEMP_PLATFORM_NETBSD_WAS_1)
    #undef DETAIL__TEMP_PLATFORM_NETBSD_WAS_1
    #define PLATFORM_NETBSD 1
#else
    #define PLATFORM_NETBSD 0
#endif
