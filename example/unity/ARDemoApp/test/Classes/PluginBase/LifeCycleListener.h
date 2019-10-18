#pragma once

// important app life-cycle events

@protocol LifeCycleListener<NSObject>
@optional
- (void)didFinishLaunching:(NSNotification*)notification;
- (void)didBecomeActive:(NSNotification*)notification;
- (void)willResignActive:(NSNotification*)notification;
- (void)didEnterBackground:(NSNotification*)notification;
- (void)willEnterForeground:(NSNotification*)notification;
- (void)willTerminate:(NSNotification*)notification;
- (void)unityDidUnload:(NSNotification*)notification;
@end

void UnityRegisterLifeCycleListener(id<LifeCycleListener> obj);
void UnityUnregisterLifeCycleListener(id<LifeCycleListener> obj);

#ifdef __cplusplus
extern "C" {
#endif

extern __attribute__((visibility("default"))) NSString* const kUnityDidUnload;

#ifdef __cplusplus
} // extern "C"
#endif
