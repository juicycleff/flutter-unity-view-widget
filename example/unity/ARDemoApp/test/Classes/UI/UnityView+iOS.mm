#if PLATFORM_IOS

#import "UnityView.h"
#import "UnityAppController+Rendering.h"
#include "OrientationSupport.h"

extern bool _unityAppReady;

@interface UnityView ()
@property (nonatomic, readwrite) ScreenOrientation contentOrientation;
@end

@implementation UnityView (iOS)
- (void)willRotateToOrientation:(UIInterfaceOrientation)toOrientation fromOrientation:(UIInterfaceOrientation)fromOrientation;
{
    // to support the case of interface and unity content orientation being different
    // we will cheat a bit:
    // we will calculate transform between interface orientations and apply it to unity view orientation
    // you can still tweak unity view as you see fit in AppController, but this is what you want in 99% of cases

    ScreenOrientation to    = ConvertToUnityScreenOrientation(toOrientation);
    ScreenOrientation from  = ConvertToUnityScreenOrientation(fromOrientation);

    if (fromOrientation == UIInterfaceOrientationUnknown)
        _curOrientation = to;
    else
        _curOrientation = OrientationAfterTransform(_curOrientation, TransformBetweenOrientations(from, to));

    _viewIsRotating = YES;
}

- (void)didRotate
{
    if (_shouldRecreateView)
    {
        [self recreateRenderingSurface];
    }

    _viewIsRotating = NO;
}

- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event      { UnitySendTouchesBegin(touches, event); }
- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event      { UnitySendTouchesEnded(touches, event); }
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event  { UnitySendTouchesCancelled(touches, event); }
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event      { UnitySendTouchesMoved(touches, event); }

@end

#endif // PLATFORM_IOS
