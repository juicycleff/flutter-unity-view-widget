#pragma once

#import <Foundation/NSNotification.h>

// view changes on the main view controller

@protocol UnityViewControllerListener<NSObject>
@optional
- (void)viewWillLayoutSubviews:(NSNotification*)notification;
- (void)viewDidLayoutSubviews:(NSNotification*)notification;
- (void)viewWillDisappear:(NSNotification*)notification;
- (void)viewDidDisappear:(NSNotification*)notification;
- (void)viewWillAppear:(NSNotification*)notification;
- (void)viewDidAppear:(NSNotification*)notification;

- (void)interfaceWillChangeOrientation:(NSNotification*)notification;
- (void)interfaceDidChangeOrientation:(NSNotification*)notification;
@end

void UnityRegisterViewControllerListener(id<UnityViewControllerListener> obj);
void UnityUnregisterViewControllerListener(id<UnityViewControllerListener> obj);

extern "C" __attribute__((visibility("default"))) NSString* const kUnityViewWillLayoutSubviews;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityViewDidLayoutSubviews;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityViewWillDisappear;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityViewDidDisappear;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityViewWillAppear;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityViewDidAppear;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityInterfaceWillChangeOrientation;
extern "C" __attribute__((visibility("default"))) NSString* const kUnityInterfaceDidChangeOrientation;
