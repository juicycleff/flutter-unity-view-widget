#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>

#import "UnityAppController.h"

#include "UndefinePlatforms.h"
#include <mach-o/ldsyms.h>
typedef struct
#ifdef __LP64__
    mach_header_64
#else
    mach_header
#endif
    MachHeader;
#include "RedefinePlatforms.h"


//! Project version number for UnityFramework.
FOUNDATION_EXPORT double UnityFrameworkVersionNumber;

//! Project version string for UnityFramework.
FOUNDATION_EXPORT const unsigned char UnityFrameworkVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <UnityFramework/PublicHeader.h>

#pragma once

// important app life-cycle events
__attribute__ ((visibility("default")))
@protocol UnityFrameworkListener<NSObject>
@optional
- (void)unityDidUnload:(NSNotification*)notification;
@end

__attribute__ ((visibility("default")))
@interface UnityFramework : NSObject
{
}

- (UnityAppController*)appController;

+ (UnityFramework*)getInstance;

- (void)setDataBundleId:(const char*)bundleId;

- (void)runUIApplicationMainWithArgc:(int)argc argv:(char*[])argv;
- (void)runEmbeddedWithArgc:(int)argc argv:(char*[])argv appLaunchOpts:(NSDictionary*)appLaunchOpts;

- (void)unloadApplication:(bool)allowReload;

- (void)registerFrameworkListener:(id<UnityFrameworkListener>)obj;
- (void)unregisterFrameworkListener:(id<UnityFrameworkListener>)obj;

- (void)showUnityWindow;
- (void)pause:(bool)pause;

- (void)setExecuteHeader:(const MachHeader*)header;
- (void)sendMessageToGOWithName:(const char*)goName functionName:(const char*)name message:(const char*)msg;

@end
