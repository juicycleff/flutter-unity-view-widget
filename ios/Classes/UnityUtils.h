#import <Foundation/Foundation.h>

#ifndef UnityUtils_h
#define UnityUtils_h

#ifdef __cplusplus
extern "C" {
#endif

    void InitArgs(int argc, char* argv[]);

    bool UnityIsInited(void);

    bool UnityIsInited(void);

    bool IsUnityPaused(void);

    bool IsUnityLoaded(void);
    
    bool IsUnityInBackground(void);

    void InitUnity(void);

    void UnityPostMessage(NSString* gameObject, NSString* methodName, NSString* message);

    void UnityPauseCommand(void);

    void UnityResumeCommand(void);

    void UnityShowWindowCommand(void);

    void UnityUnloadCommand(void);

    void UnityQuitCommand(void);

#ifdef __cplusplus
} // extern "C"
#endif

@interface UnityUtils : NSObject

+ (BOOL)isUnityReady;
+ (void)createPlayer:(void (^)(void))completed;
+ (void)recreatePlayer:(void (^)(void))completed;
+ (void)resetUnityReady;
@end

#endif /* UnityUtils_h */
