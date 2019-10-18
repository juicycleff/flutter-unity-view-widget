#pragma once

@interface UnityRenderingView : UIView
{
}
+ (void)InitializeForAPI:(UnityRenderingAPI)api;
@end

@interface UnityView : UnityRenderingView
{
    @private ScreenOrientation _curOrientation;
    @private BOOL _shouldRecreateView;
    @private BOOL _viewIsRotating;
}

// we take scale factor into account because gl backbuffer size depends on it
- (id)initWithFrame:(CGRect)frame scaleFactor:(CGFloat)scale;
- (id)initWithFrame:(CGRect)frame;
- (id)initFromMainScreen;

// in here we will go through subviews and call onUnityUpdateViewLayout selector (if present)
// that allows to handle simple overlay child view layout without doing view controller magic
- (void)layoutSubviews;

- (void)recreateRenderingSurfaceIfNeeded;
- (void)recreateRenderingSurface;

// will match script-side Screen.orientation
@property (nonatomic, readonly) ScreenOrientation contentOrientation;

@end

@interface UnityView (Deprecated)
- (void)recreateGLESSurfaceIfNeeded __deprecated_msg("use recreateRenderingSurfaceIfNeeded instead.");
- (void)recreateGLESSurface __deprecated_msg("use recreateRenderingSurface instead.");
@end

@interface UnityView (Keyboard)
- (void)processKeyboard;
@end

#if PLATFORM_IOS
    #include "UnityView+iOS.h"
#elif PLATFORM_TVOS
    #include "UnityView+tvOS.h"
#endif

void ReportSafeAreaChangeForView(UIView* view);

// Computes safe area for a view in Unity coordinate system (origin of the view
// is bottom-left, as compared to standard top-left)
CGRect ComputeSafeArea(UIView* view);
