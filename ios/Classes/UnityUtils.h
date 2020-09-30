#import <Foundation/Foundation.h>
#include <UnityFramework/UnityFramework.h>

#ifndef UnityUtils_h
#define UnityUtils_h

#ifdef __cplusplus
extern "C" {
#endif

    void InitArgs(int argc, char* argv[]);

    bool UnityIsInited(void);

    bool IsUnityPaused(void);

    bool IsUnityLoaded(void);
    
    bool IsUnityInBackground(void);

    void UnityPostMessage(NSString* gameObject, NSString* methodName, NSString* message);

    void SetUnityUnloaded(bool loaded);

    void UnityPauseCommand(void);

    void UnityResumeCommand(void);

    void UnityShowWindowCommand(void);

    void UnityQuitCommand(void);

#ifdef __cplusplus
} // extern "C"
#endif

@interface UnityUtils : NSObject

+ (bool)isUnityReady;
+ (void)createPlayer:(void (^)(void))completed;
+ (void)unloadUnity;
+ (void)resetUnityReady;
+ (void)unregisterUnityListener;
+ (void)registerUnityListener;
@end

@interface AppDelegate : UIResponder<UIApplicationDelegate, UnityFrameworkListener>
@property UnityFramework* ufw;
@end
#endif /* UnityUtils_h */
