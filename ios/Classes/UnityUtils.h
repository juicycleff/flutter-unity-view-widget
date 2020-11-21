#import <Foundation/Foundation.h>
#include <UnityFramework/UnityFramework.h>

#ifndef UnityUtils_h
#define UnityUtils_h

#ifdef __cplusplus
extern "C" {
#endif

    void InitArgs(int argc, char* argv[]);

#ifdef __cplusplus
} // extern "C"
#endif

@interface UnityUtils : UIResponder<UIApplicationDelegate, UnityFrameworkListener>

@property UnityFramework* ufw;

- (void)initUnity;
- (bool)unityIsInitialized;
- (void)createPlayer:(void (^)(void))completed;
- (void)unregisterUnityListener;
- (void)registerUnityListener;
- (void)pauseUnity;
- (void)resumeUnity;
- (void)unloadUnity;
- (bool)isUnityLoaded;
- (bool)isUnityPaused;
- (void)quitUnity;
- (void)unityPostMessage: (NSString*)gameObject unityMethodName: (NSString*) methodName unityMessage: (NSString*) message;
@end

#endif /* UnityUtils_h */
