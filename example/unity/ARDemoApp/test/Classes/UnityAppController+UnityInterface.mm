#include "UnityAppController+UnityInterface.h"
#include "UnityAppController+Rendering.h"


@implementation UnityAppController (UnityInterface)

- (BOOL)paused
{
    return UnityIsPaused() ? YES : NO;
}

- (void)setPaused:(BOOL)pause
{
    const int newPause  = pause == YES ? 1 : 0;

    UnityPause(newPause);
}

@end
