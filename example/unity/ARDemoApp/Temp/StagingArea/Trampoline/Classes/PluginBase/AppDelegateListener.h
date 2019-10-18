#pragma once

#include "LifeCycleListener.h"


@protocol AppDelegateListener<LifeCycleListener>
@optional
// these do not have apple defined notifications, so we use our own notifications

// notification will be posted from
// - (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
// notification user data is deviceToken
- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification*)notification;

// notification will be posted from
// - (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
// notification user data is error
- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification*)notification;

// notification will be posted from
// - (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
// notification user data is userInfo
- (void)didReceiveRemoteNotification:(NSNotification*)notification;

// notification will be posted from
// - (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification
// notification user data is notification
- (void)didReceiveLocalNotification:(NSNotification*)notification;

// notification will be posted from
// - (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
// notification user data is the NSDictionary containing all the params
- (void)onOpenURL:(NSNotification*)notification;

// notification will be posted from
// - (BOOL)application:(UIApplication*)application willFinishLaunchingWithOptions:(NSDictionary*)launchOptions
// notification user data is the NSDictionary containing launchOptions
- (void)applicationWillFinishLaunchingWithOptions:(NSNotification*)notification;
// notification will be posted from
// - (void)application:(UIApplication*)application handleEventsForBackgroundURLSession:(nonnull NSString *)identifier completionHandler:(nonnull void (^)())completionHandler
// notification user data is NSDictionary with one item where key is session identifier and value is completion handler
- (void)onHandleEventsForBackgroundURLSession:(NSNotification*)notification;

// these are just hooks to existing notifications
- (void)applicationDidReceiveMemoryWarning:(NSNotification*)notification;
- (void)applicationSignificantTimeChange:(NSNotification*)notification;
- (void)applicationWillChangeStatusBarFrame:(NSNotification*)notification;
- (void)applicationWillChangeStatusBarOrientation:(NSNotification*)notification;
@end

void UnityRegisterAppDelegateListener(id<AppDelegateListener> obj);
void UnityUnregisterAppDelegateListener(id<AppDelegateListener> obj);

extern "C" __attribute__((visibility("default"))) NSString* const kUnityDidRegisterForRemoteNotificationsWithDeviceToken;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityDidFailToRegisterForRemoteNotificationsWithError;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityDidReceiveRemoteNotification;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityDidReceiveLocalNotification;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityOnOpenURL;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityWillFinishLaunchingWithOptions;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityHandleEventsForBackgroundURLSession;
