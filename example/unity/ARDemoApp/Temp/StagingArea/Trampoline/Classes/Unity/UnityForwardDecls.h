#pragma once

#include <stdint.h>
#include "UnitySharedDecls.h"

#ifdef __OBJC__
@class UIScreen;
@class UIWindow;
@class UIView;
@class UIViewController;
@class UIEvent;
@class UILocalNotification;
@class NSString;
@class NSDictionary;
@class NSSet;
@class NSData;
@class NSError;
@class NSBundle;
@class UIKeyCommand;

@class UnityViewControllerBase;
#else
typedef struct objc_object UIScreen;
typedef struct objc_object UIWindow;
typedef struct objc_object UIView;
typedef struct objc_object UIViewController;
typedef struct objc_object UIEvent;
typedef struct objc_object UILocalNotification;
typedef struct objc_object NSString;
typedef struct objc_object NSDictionary;
typedef struct objc_object NSSet;
typedef struct objc_object NSError;
typedef struct objc_object NSData;
typedef struct objc_object NSBundle;
typedef struct objc_object UIKeyCommand;

typedef struct objc_object UnityViewControllerBase;
#endif

// unity internal audio effect definition struct
struct UnityAudioEffectDefinition;

// new unity rendering api
struct IUnityInterfaces;

// be aware that this struct is shared with unity implementation so you should absolutely not change it
struct UnityFrameStats
{
    uint64_t    fixedBehaviourManagerDt;
    uint64_t    fixedPhysicsManagerDt;
    uint64_t    dynamicBehaviourManagerDt;
    uint64_t    coroutineDt;
    uint64_t    skinMeshUpdateDt;
    uint64_t    animationUpdateDt;
    uint64_t    renderDt;
    uint64_t    cullingDt;
    uint64_t    clearDt;
    int         fixedUpdateCount;

    int         batchCount;
    uint64_t    drawCallTime;
    int         drawCallCount;
    int         triCount;
    int         vertCount;

    uint64_t    dynamicBatchDt;
    int         dynamicBatchCount;
    int         dynamicBatchedDrawCallCount;
    int         dynamicBatchedTris;
    int         dynamicBatchedVerts;

    int         staticBatchCount;
    int         staticBatchedDrawCallCount;
    int         staticBatchedTris;
    int         staticBatchedVerts;
};


// be aware that this enum is shared with unity implementation so you should absolutely not change it
typedef enum
    LogType
{
    logError        = 0,
    logAssert       = 1,
    logWarning      = 2,
    logLog          = 3,
    logException    = 4,
    logDebug        = 5,
}
LogType;

// this dictates touches processing on os level: should we transform touches to unity view coords or not.
// N.B. touch.position will always be adjusted to current resolution
//      i.e. if you touch right border of view, touch.position.x will be Screen.width, not view.width
//      to get coords in view space (os-coords), use touch.rawPosition
typedef enum ViewTouchProcessing
{
    // the touches originated from view will be ignored by unity
    touchesIgnored = 0,

    // touches would be processed as if they were originated in unity view:
    // coords will be transformed from view coords to unity view coords
    touchesTransformedToUnityViewCoords = 1,

    // touches coords will be kept intact (in originated view coords)
    // it is default value
    touchesKeptInOriginalViewCoords = 2,
}
ViewTouchProcessing;

// be aware that this enum is shared with unity implementation so you should absolutely not change it
typedef enum KeyboardStatus
{
    Visible     = 0,
    Done        = 1,
    Canceled    = 2,
    LostFocus   = 3,
}
KeyboardStatus;

// misc
#ifdef __cplusplus
extern "C" {
    bool UnityiOS81orNewer();
    bool UnityiOS82orNewer();
    bool UnityiOS90orNewer();
    bool UnityiOS91orNewer();
    bool UnityiOS100orNewer();
    bool UnityiOS101orNewer();
    bool UnityiOS102orNewer();
    bool UnityiOS103orNewer();
    bool UnityiOS110orNewer();
    bool UnityiOS111orNewer();
    bool UnityiOS112orNewer();
}
#endif
